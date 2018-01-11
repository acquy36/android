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
    public class NhanVien
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string DiachiNV { get; set; }
        public string SoDTNV { get; set; }
        public string EmailNV { get; set; }
       
    }
}