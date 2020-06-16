using System.Web;

namespace PulsePersonalizationApp
{
    public class Global
    {
        public const string AddonName = "PulsePersonalizationApp";
        public const string ImagesFolderPath = AddonName + "/ClientResources/Images/";
        public const string StylesFolderPath = AddonName + "/ClientResources/Styles/";
        public const string ScriptsFolderPath = AddonName + "/ClientResources/Scripts/";

        public static string PublicRootPath = EPiServer.Shell.Configuration.EPiServerShellSection.GetSection().PublicModules.RootPath;
        public static string ProtectedRootPath = EPiServer.Shell.Configuration.EPiServerShellSection.GetSection().ProtectedModules.RootPath;

        public const string AzureUrl = "https://pulse-api.azure-api.net/";
        public static string SegmentMembershipUrl = "SegmentDataFunctions/GetSegmentMembershipByLocation";
        public const string PulseServicesUrl = "http://pulse-services-prod.pulse.app/";
        public static string ListSegmentsUrl = "list_segments";

        public static string GetPublicStylesUrl()
        {
            return GetPublicEPIUrl() + StylesFolderPath;
        }

        public static string GetProtectedStylesUrl()
        {
            return GetProtectedEPIUrl() + StylesFolderPath;
        }

        public static string GetPublicScriptsUrl()
        {
            return GetPublicEPIUrl()  + ScriptsFolderPath;
        }

        public static string GetProtectedScriptsUrl()
        {
            return GetProtectedEPIUrl() + ScriptsFolderPath;
        }

        private static string GetPublicEPIUrl()
        {
            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/") + PublicRootPath.Replace("~/", "");    
        }

        private static string GetProtectedEPIUrl()
        {
            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/") + ProtectedRootPath.Replace("~/", "");
        }
    }
}