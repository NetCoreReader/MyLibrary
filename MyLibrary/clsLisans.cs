using System;
using System.Collections.Generic;


namespace MyLibrary
{
    public sealed class clsLisans
    {
        private static readonly Lazy<clsLisans> lazy = new Lazy<clsLisans>(() => new clsLisans());

        public static clsLisans Instance { get { return lazy.Value; } }

        private clsLisans()
        {
            if (ssLisans().Contains(_Key) == false)
            {
                throw new Exception("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ligula tellus, egestas eu interdum lobortis, aliquam sit amet lacus. Morbi id faucibus ligula, sed sagittis mi. Integer vel sollicitudin nisl. Phasellus tincidunt porta dignissim.");
            }
        }

        private List<string> ssLisans()
        {
            DateTime _dt1 = DateTime.Now.AddDays(-1);
            DateTime _dt2 = DateTime.Now;
            DateTime _dt3 = DateTime.Now.AddDays(1);
            List<string> uc_ay = new List<string>();
            uc_ay.Add(MyCryptoOthers.Instance.Hash_SHA256(_dt1.Year.ToString() + _dt1.Month.ToString()));
            uc_ay.Add(MyCryptoOthers.Instance.Hash_SHA256(_dt2.Year.ToString() + _dt2.Month.ToString()));
            uc_ay.Add(MyCryptoOthers.Instance.Hash_SHA256(_dt3.Year.ToString() + _dt3.Month.ToString()));
            return uc_ay;
        }

        public static string _Key { get; set; }
    }
}
