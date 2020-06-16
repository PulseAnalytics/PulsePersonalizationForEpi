
using System.Web;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Services;

namespace PulsePersonalizationApp.Segments
{
    [VisitorGroupCriterion(
        Category = "Pulse Analytics",
        DisplayName = "Social Interest",
        Description = "Pulse uses artificial intelligence and social listening to determine visitor interest based on the neighborhood they're from.",
        ScriptUrl = "../PulsePersonalizationApp/ClientResources/Scripts/Editors/PulseHelper.js"
        )]
    public class PulseSocialSegments : PulseService<PulseSocialSegmentModel>
    {
        public override bool IsMatch(System.Security.Principal.IPrincipal principal, HttpContextBase httpContext)
        {
            var session = httpContext.Session;

            if (session != null)
            {
                if (session["pulse_segments"] != null)
                {
                    string[] pulse_segments = (string[]) session["pulse_segments"];
                    bool retval = pulse_segments.Contains(Model.SegmentId);
                    return retval;
                }
            }
            return false;
        }
    }
}
