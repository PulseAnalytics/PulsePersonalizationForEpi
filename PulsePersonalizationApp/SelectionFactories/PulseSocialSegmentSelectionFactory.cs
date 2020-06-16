
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Repositories;

namespace PulsePersonalizationApp.SelectionFactories
{
    public class PulseSocialSegmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            SegmentsListModel model = DataStoreRepository.Instance.LoadData<SegmentsListModel>();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "", Text = "Select a Segment..." });

            foreach (MarketSegmentModel marketSegment in model.Segments) {
                if (marketSegment.segment_name.StartsWith("s_")) {
                    list.Add(new SelectListItem { Value = marketSegment.segment_name, Text = marketSegment.name });
                }
            }

            return list;
        }
    }
}