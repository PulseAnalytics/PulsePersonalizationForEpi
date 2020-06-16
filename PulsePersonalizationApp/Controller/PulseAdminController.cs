using EPiServer.Shell;
using EPiServer.PlugIn;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using EPiServer.ServiceLocation;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.Helpers;
using System.Text;

namespace PulsePersonalizationApp.Controller
{
    [GuiPlugIn(
        Area = EPiServer.PlugIn.PlugInArea.AdminMenu,
        Url = "/PulseAdmin",
        DisplayName = "Pulse Personalization App")]
    [Authorize(Roles = "CmsAdmins")]
    public class PulseAdminController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            return View(Paths.ToResource(GetType(), "Views/PulseAdmin/Index.cshtml"), DataStoreRepository.Instance.LoadData<PulseAdminModel>());
        }

        [HttpPost]
        public ActionResult SaveApiKey(string apiKey)
        {
            try {
                Debug.WriteLine("SaveApiKey(): API: " + apiKey);

                PulseAdminModel model = DataStoreRepository.Instance.LoadData<PulseAdminModel>();
                model.PulseApiKey = apiKey;
                DataStoreRepository.Instance.SaveData(model);

                HttpHelper.RecreateClient();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch (Exception ex) {
                Debug.WriteLine("SaveApiKey(): Error: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult SaveCredentials(PulseAdminModel credentials)
        {
            try
            {
                Debug.WriteLine("SaveCredentials(): START");

                PulseAdminModel data = new PulseAdminModel
                {
                    PulseApiKey = credentials.PulseApiKey,
                    FirstName = credentials.FirstName,
                    LastName = credentials.LastName,
                    CompanyName = credentials.CompanyName,
                    Email = credentials.Email
                };

                DataStoreRepository.Instance.SaveData(data);

                HttpHelper.RecreateClient();

                Debug.WriteLine("SaveCredentials(): END");
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveCredentials(): Error: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult RemoveAllVisitorGroups()
        {
            try
            {
                Debug.WriteLine("RemoveAllPulseVisitorGroups(): START");

                // Get IVisitorGroupRepository
                IVisitorGroupRepository visitorGroupRepository = ServiceLocator.Current.GetInstance<IVisitorGroupRepository>();

                // Create a list of GUIDs to delete
                List<Guid> toDeleteList = new List<Guid>();

                // List trough all Visitor Group items
                foreach (VisitorGroup item in visitorGroupRepository.List()) {

                    // Check if item contains Addon Name in Type Name
                    VisitorGroupCriterion criteria = item.Criteria.Where(x => x.TypeName.Contains(Global.AddonName) || x.TypeName.Contains("PulseEpiserverConnector")).FirstOrDefault();

                    // Add it to the list for later deleting
                    if (criteria != null) {
                        Debug.WriteLine("Type name: " + criteria.TypeName);
                        toDeleteList.Add(item.Id);
                    }
                }

                // Delete all Visitor Group items
                if (toDeleteList.Any()) { 
                    foreach (Guid delete in toDeleteList) { 
                        visitorGroupRepository.Delete(delete);
                    }
                }

                Debug.WriteLine("RemoveAllPulseVisitorGroups(): END");
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RemoveAllPulseVisitorGroups(): Error: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ContentResult RunDiagnostics()
        {
            var diag = new StringBuilder();
            try
            {
                Debug.WriteLine("RunDiagnostics(): START");

                string currentIP = IpHelper.GetIPAddress(HttpContext.Request);
                string testingIP = "13.82.179.176";

                Debug.WriteLine("RunDiagnostics(): Current IP: " + currentIP);
                Debug.WriteLine("RunDiagnostics(): Testing IP: " + testingIP);

                diag.AppendLine("Current IP: " + currentIP);
                diag.AppendLine("Testing IP: " + testingIP);

                var locTesting = GeoLocationHelper.GetLocationFromIp(IPAddress.Parse(testingIP));

                // Geolocation not configured or not working
                if (locTesting == null)
                {
                    diag.AppendLine("GeoLocation: Not working! Please configure Maxmind or another GeoIP provider");
                    return Content(diag.ToString(), "text/plain");
                }
                else 
                {
                    var locCurrent = GeoLocationHelper.GetLocationFromIp(IPAddress.Parse(currentIP));
                    string psCurrent = HttpHelper.GetSegmentList(GeoLocationHelper.GetCoordinatesFromIp(IPAddress.Parse(currentIP)));
                    Debug.WriteLine("RunDiagnostics(): GeoLocation (current IP): {0} - {1}", locCurrent.CountryCode, locCurrent.Region);
                    Debug.WriteLine("RunDiagnostics(): Segments (current IP): " + psCurrent);
                    diag.AppendLine("GeoLocation (current IP): " + locCurrent.CountryCode + " - " + locCurrent.Region);
                    diag.AppendLine("Segments (current IP): " + psCurrent);

                    string psTesting = HttpHelper.GetSegmentList(GeoLocationHelper.GetCoordinatesFromIp(IPAddress.Parse(testingIP)));
                    Debug.WriteLine("RunDiagnostics(): GeoLocation (testing IP): {0} - {1}", locTesting.CountryCode, locTesting.Region);
                    Debug.WriteLine("RunDiagnostics(): Segments (testing IP): " + psTesting);
                    diag.AppendLine("GeoLocation (testing IP): " + locTesting.CountryCode + " - " + locTesting.Region);
                    diag.AppendLine("Segments (testing IP): " + psTesting);

                    Debug.WriteLine("RunDiagnostics(): END");

                    return Content(diag.ToString(), "text/plain");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RunDiagnostics(): Error: " + ex.Message);
                diag.AppendLine("Error while running diagnostics" + ex.Message);

                return Content(diag.ToString(), "text/plain");
            }
        }
    }
}