using EPiServer.Personalization;
using EPiServer.ServiceLocation;
using PulsePersonalizationApp.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace PulsePersonalizationApp.Helpers
{
    public class GeoLocationHelper
    {
        public static IGeolocationProvider GetProvider()
        {
            return ServiceLocator.Current.GetInstance<IGeolocationProvider>();
        }

        public static IGeolocationResult GetLocationFromIp(IPAddress IpAddress)
        {
            IGeolocationProvider provider = GetProvider();
            if (provider == null)
            {
                // We have no way to convert IP to lat/long. 
                Debug.WriteLine("GeoLocationHelper.GetProvider(): Pulse requires a Geolocation Provider in order to function. Please install the MaxMind Geolocation Provider for Episerver.");
                return null;
            }

            return provider.Lookup(IpAddress);
        }

        public static CoordinatesModel GetCoordinatesFromIp(IPAddress IpAddress)
        {
            IGeolocationResult location = GetLocationFromIp(IpAddress);

            if (location == null)
            {
                Debug.WriteLine("GeoLocationHelper.GetProvider(): Geolocation provider returned null for IP: " + IpAddress.ToString());
                return null;
            }

            return new CoordinatesModel { Latitude = location.Location.Latitude, Longitude = location.Location.Longitude };
        }

        public static CoordinatesModel GetCoordinates() {
            try { 
                var context = new HttpContextWrapper(HttpContext.Current);
                HttpRequestBase request = context.Request;
                IPAddress ipAddress = null;
                if (IPAddress.TryParse(IpHelper.GetIPAddress(request), out ipAddress))
                {
                    return GetCoordinatesFromIp(ipAddress);
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GeoLocationHelper.GetCoordinates(): Error: " + ex.Message);
            } 
            return null;
        }
    }
}