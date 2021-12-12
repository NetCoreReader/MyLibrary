using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MyLibrary
{
    public sealed class MyGarbagegCollector
    {
        private static readonly Lazy<MyGarbagegCollector> lazy = new Lazy<MyGarbagegCollector>(() => new MyGarbagegCollector());
        public static MyGarbagegCollector Instance { get { return lazy.Value; } }
        private MyGarbagegCollector()
        {

        }

        /// <summary>
        /// Belleği Temizler
        /// </summary>
        public void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        /// <summary>
        /// Programın bellekte tahsis etmiş olduğu alanı byte cinsinden verir
        /// </summary>
        /// <param name="deger">Parametre true olarak atanırsa değer döndürülmeden önce GC mekanizması devreye girer ve değeri döndürmeden önce belleği temizler.
        /// Parametre olarak false atanırsa hiçbir işlem yapılmadan sadece o anki bellekte tahsis edilmiş alan boyutunu bize döndürür.</param>
        /// <returns></returns>
        public string PrograminKapladigiAlan(bool deger)
        {
            return GC.GetTotalMemory(deger) + " byte";
        }
    }
}
