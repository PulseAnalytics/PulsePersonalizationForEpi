using Newtonsoft.Json;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Repositories;
using PulsePersonalizationApp.Segments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PulsePersonalizationApp.Helpers
{
    public class HttpHelper
    {
        static HttpClient _client = null;

        public static HttpClient GetClient()
        {
            if (_client == null)
                _client = CreateClient();

            return _client;
        }

        public static HttpClient CreateClient(bool defaultClient = true, string baseUrl = Global.AzureUrl, string contentType = "application/json")
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl),
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            if (defaultClient) { 
                string pulseAPIkey = DataStoreRepository.Instance.LoadData<PulseAdminModel>().PulseApiKey;

                if (!String.IsNullOrEmpty(pulseAPIkey))
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", pulseAPIkey);
                    return client;
                }

                Debug.WriteLine("HttpHelper.CreateClient(): Pulse API key not specified!");
                return null;
            }

            return client;
        }

        public static void RecreateClient() {
            _client = CreateClient();
        }

        public static HttpRequestMessage GetHttpRequestMessage(CoordinatesModel coordinates) { 
            var payload = new Dictionary<string, double> 
            {
                { "latitude", coordinates.Latitude  },
                { "longitude", coordinates.Longitude }
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.Indented);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Global.SegmentMembershipUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            return request;
        }

        public static string GetSegmentList(CoordinatesModel coordinates)
        {
            try
            {
                HttpClient httpClient = GetClient();

                if (httpClient == null) return null;

                var task = httpClient.SendAsync(GetHttpRequestMessage(coordinates));
                task.Wait();

                if (task.Result.IsSuccessStatusCode)
                {
                    var jsontask = task.Result.Content.ReadAsStringAsync();
                    jsontask.Wait();

                    PulseSegmentListModel segmentList = JsonConvert.DeserializeObject<PulseSegmentListModel>(jsontask.Result);

                    return string.Join(", ", segmentList.data);
                }
                return "Error while getting segment list. Check API key!";

            } catch (Exception ex) {
                Debug.WriteLine("HttpHelper.QueryPulse(): Error: " + ex.Message);
                return "Error while getting segment list. Check API key!";
            }
        }

        public static List<MarketSegmentModel> ListSegments() {
            try {
                // Get all Market Segments
                var httpClient = CreateClient(false, Global.PulseServicesUrl);
                var response = httpClient.GetAsync(Global.ListSegmentsUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("HttpHelper.ListSegments(): Status: " + response.StatusCode);
                    var result = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<MarketSegmentModel>>(result);
                }

                Debug.WriteLine("HttpHelper.ListSegments(): Error: {0}", response);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("HttpHelper.ListSegments(): Error: {0}", ex.Message);
                return null;
            }
        }
    }
}