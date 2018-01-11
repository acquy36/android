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
using Android.Database;

namespace QLSX.Resources.Model
{
    public class ThanhPham
    {
        //thuoc tinh
        public string MaTP { get; set; }
        public string TenTP { get; set; }
        public string DVTTP { get; set; }

    }
}