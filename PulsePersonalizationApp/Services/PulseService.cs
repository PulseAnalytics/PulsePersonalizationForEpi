using System.Web;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.Segments;
using System.Threading;
using PulsePersonalizationApp.Helpers;
using PulsePersonalizationApp.Models;
using System.Net;

namespace PulsePersonalizationApp.Services
{
    public abstract class PulseService<T>: CriterionBase<T>
        where T : class, ICriterionModel, new()
    {
        public override void Subscribe(ICriterionEvents criterionEvents)
        {
            base.Subscribe(criterionEvents);

            criterionEvents.StartSession += PulseSessionStartHandler;
        }
        public override void Unsubscribe(ICriterionEvents criterionEvents)
        {
            criterionEvents.StartSession -= PulseSessionStartHandler;
        }

        private void PulseSessionStartHandler(object sender, CriterionEventArgs e)
        {
            var locker = LockingSingleton.GetInstance();

            if (Monitor.TryEnter(locker.lockObj))
            {
                try
                {
                    if (!locker.isLocked)
                    {
                        locker.isLocked = true;

                        Debug.WriteLine("PulseSessionStartHandler(): START");

                        string ip = IpHelper.GetIPAddress(new HttpRequestWrapper(HttpContext.Current.Request));

                        Debug.WriteLine("PulseSessionStartHandler(): IP:" + ip);

                        CoordinatesModel coordinates = GeoLocationHelper.GetCoordinatesFromIp(IPAddress.Parse(ip));
                        if (coordinates == null)
                        {
                            e.HttpContext.Session["pulse_segments"] = new string[0];
                            locker.isLocked = false;
                            return;
                        }

                        QueryPulse(coordinates, e.HttpContext.Session, locker);

                        Debug.WriteLine("PulseSessionStartHandler(): END");
                    }
                    else {
                        Debug.WriteLine("PulseSessionStartHandler(): Another thread already started");
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    Monitor.Exit(locker.lockObj);
                    Debug.WriteLine("PulseSessionStartHandler(): Lock released");
                }
            }
        }

        public static void QueryPulse(CoordinatesModel coordinates, HttpSessionStateBase sessionBase, LockingSingleton locker)
        {
            Debug.WriteLine("QueryPulse(): START");

            HttpClient httpClient = HttpHelper.GetClient();
            HttpSessionStateBase session = sessionBase;

            if (httpClient == null) {
                locker.isLocked = false;
                return;
            }

            httpClient.SendAsync(HttpHelper.GetHttpRequestMessage(coordinates)).ContinueWith((task) =>
            {
                var response = task.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("QueryPulse(): Status: " + response.StatusCode);

                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();

                    PulseSegmentListModel ps = JsonConvert.DeserializeObject<PulseSegmentListModel>(jsonTask.Result);
                    session["pulse_segments"] = ps.data;

                    Debug.WriteLine("QueryPulse(): Data: " + string.Join(", ", ps.data));
                }
                else
                {
                    Debug.WriteLine("QueryPulse(): Error: {0}", response);
                }

                locker.isLocked = false;
            });

            Debug.WriteLine("QueryPulse(): END");
            return;
        }
    }
}