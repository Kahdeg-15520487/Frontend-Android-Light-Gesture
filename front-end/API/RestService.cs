using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace front_end.API
{
    class RestService
    {
        static HttpClient HttpClient;
        static Uri Uri;

        public RestService(string uri)
        {
            HttpClient = new HttpClient();
            Uri = new Uri(uri);
        }

        public RestService()
        {
            HttpClient = new HttpClient();
            Uri = null;
        }

        public void SetUri(string uri)
        {
            Uri = new Uri(uri);
        }

        public async Task<Dictionary<string, bool>> GetLightStates()
        {
            Dictionary<string, bool> result = null;

            var response = await HttpClient.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Dictionary<string, bool>>(content);
            }

            return result;
        }

        public async Task<LightState> GetLightState(string id)
        {
            var uri = new Uri(Uri, id);

            LightState result = null;

            var response = HttpClient.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = await ReadHttpContent(response); //response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<LightState>(content);
            }

            return result;
        }

        public async Task<LightState> ToggleLight(string id)
        {
            var uri = new Uri(Uri, $"/toggle/{id}/");

            LightState result = null;

            var response = HttpClient.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<LightState>(content);
            }

            return result;
        }

        public async Task<string> ReadHttpContent(HttpResponseMessage response)
        {
            var ms = new MemoryStream();
            await response.Content.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            var sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
    }
}