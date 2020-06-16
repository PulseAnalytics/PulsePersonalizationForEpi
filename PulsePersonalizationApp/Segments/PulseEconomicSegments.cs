
using System.Web;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Services;

namespace PulsePersonalizationApp.Segments
{
    /*[VisitorGroupCriterion(
        Category = "Pulse Analytics",
        DisplayName = "Economic Segments",
        Description = "Pulse uses artificial intelligence and geospatial analysis to determine visitor interest based on the neighborhood they're from.",
        ScriptUrl = "../PulsePersonalizationApp/ClientResources/Scripts/Editors/PulseHelper.js"
        )]
    public class PulseEconomicSegments : PulseService<PulseEconomicSegmentModel>
    {
        public override bool IsMatch(System.Security.Principal.IPrincipal principal, HttpContextBase httpContext)
        {
            var session = httpContext.Session;

            if (session != null)
            {
                if (session["pulse_segments"] != null)
                {
                    string[] pulse_segments = (string[]) httpContext.Session["pulse_segments"];
                    bool retval = pulse_segments.Contains(Model.SegmentId);
                    return retval;
                }
            }
            return false;
        }
    }*/
}
