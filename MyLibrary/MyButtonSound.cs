using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary
{
    public sealed class MyButtonSound
    {
        private static Lazy<MyButtonSound> lazy = new Lazy<MyButtonSound>(() => new MyButtonSound());
        public static MyButtonSound Instance { get { return lazy.Value; } }

        public MyButtonSound()
        {

        }

        public enum MySounds
        {
            Hata,
            Click,
            Uyari,
            Soru
        }

        public void GetSound(MySounds s)
        {
            switch (s)
            {
                case MySounds.Hata:
                    System.Media.SystemSounds.Beep.Play();
                    break;
                case MySounds.Click:
                    new System.Media.SoundPlayer(Properties.Resources.Click).PlaySync();
                    break;
                case MySounds.Uyari:
                    break;
                case MySounds.Soru:
                    break;
                default:
                    break;
            }

        }    



    }



}
