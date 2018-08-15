using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace front_end.API
{
    class LightState
    {
        public string id { get; set; }
        public bool state { get; set; }

        public override string ToString()
        {
            return $"{id} : {(state ? "on" : "off")}";
        }
    }
}