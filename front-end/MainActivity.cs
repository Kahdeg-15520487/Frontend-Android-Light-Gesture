﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.OS;
using front_end.Views;
using front_end.Models;

namespace front_end
{
    [Activity(Label = "front_end", MainLauncher = true)]
    public class MainActivity : Activity
    {
        static API.RestService restService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;

            // Use subclassed WebViewClient to intercept hybrid native calls
            webView.SetWebViewClient(new HybridWebViewClient());

            // Render the view from the type generated from RazorView.cshtml
            var model = new Model1() { Text = "Text goes here" };
            var template = new RazorView();
            var page = template.GenerateString();

            // Load the rendered HTML into the view with a base URL 
            // that points to the root of the bundled Assets folder
            webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);

            restService = new API.RestService();
        }

        private class HybridWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView webView, string url)
            {

                // If the URL is not our own custom scheme, just let the webView load the URL as usual
                var scheme = "hybrid:";

                if (!url.StartsWith(scheme))
                    return false;

                // This handler will treat everything between the protocol and "?"
                // as the method name.  The querystring has all of the parameters.
                var resources = url.Substring(scheme.Length).Split('?');
                var method = resources[0];
                var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]);

                //if (method == "UpdateLabel")
                //{
                //    var textbox = parameters["textbox"];

                //    // Add some text to our string here so that we know something
                //    // happened on the native part of the round trip.
                //    var prepended = string.Format("C# says \"{0}\"", textbox);

                //    // Build some javascript using the C#-modified result
                //    var js = string.Format("SetLabelText('{0}');", prepended);

                //    webView.LoadUrl("javascript:" + js);
                //}

                if (method == "Toggle")
                {
                    var api_url = parameters["textbox_url"];
                    var id = parameters["textbox"];

                    restService.SetUri(api_url);
                    
                    var ls = restService.GetLightState(id).GetAwaiter().GetResult();

                    var js = string.Format("SetLabelText('{0}');", ls);

                    webView.LoadUrl("javascript:" + js);
                }

                return true;
            }
        }
    }
}

