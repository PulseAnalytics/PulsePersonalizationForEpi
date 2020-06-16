using System;
using System.Web.Mvc;
using System.Diagnostics;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Repositories;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PulsePersonalizationApp.Controller
{
    [Authorize(Roles = "CmsAdmins, VisitorGroupAdmins")]
    public class MarketSegmentsController : System.Web.Mvc.Controller
    {
        [HttpGet]
        public JsonResult GetSegmentDiscription(string segmentName)
        {
            try
            {
                Debug.WriteLine("MarketSegmentsController.GetAll(): START");
                SegmentsListModel model = DataStoreRepository.Instance.LoadData<SegmentsListModel>();

                if (model.Segments != null)
                {
                    foreach (MarketSegmentModel marketSegment in model.Segments)
                    {
                        if (marketSegment.segment_name.Equals(segmentName))
                        {
                            return Json(marketSegment.description, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                Debug.WriteLine("MarketSegmentsController.GetAll(): END");
                return Json("No discription", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MarketSegmentsController.GetAll(): Error: " + ex.Message);
                return Json("No discription", JsonRequestBehavior.AllowGet);
            }
        }
    }
}