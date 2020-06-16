
using System.Web;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Services;

namespace PulsePersonalizationApp.Segments
{
    /*[VisitorGroupCriterion(
        Category = "Pulse Analytics",
        DisplayName = "Climate & Energy Segments",
        Description = "Pulse uses artificial intelligence and social listening to determine visitor interest based on the neighborhood they're from.",
        ScriptUrl = "../PulsePersonalizationApp/ClientResources/Scripts/Editors/PulseHelper.js"
        )]
    public class PulseTemplateSegments : PulseService<PulseClimateSegmentModel>
    { 
        public override bool IsMatch(System.Security.Principal.IPrincipal principal, HttpContextBase httpContext)
        {
            var session = httpContext.Session;

            if (session != null)
            {
                if (session["pulse_segments"] != null)
                {
                    string[] pulse_segments = (string[])httpContext.Session["pulse_segments"];
                    bool retval = pulse_segments.Contains(Model.SegmentId);
                    return retval;
                }
            }
            return false;
        }
    }*/
}
