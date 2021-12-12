using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary
{
    public class MyCreditCard
    {
        private static Lazy<MyCreditCard> lazy = new Lazy<MyCreditCard>(() => new MyCreditCard());

        public static MyCreditCard Instance { get { return lazy.Value; } }

        private MyCreditCard()
        {

        }

        #region Classes
        public sealed class CreditCard
        {
            public string CardNo { get; set; }
            public string CardType { get; set; }
            public string HesapNo { get; set; }
        } 
        #endregion

        public CreditCard GetCreditCard(string cardNo)
        {
            string yeniNo = string.Empty;
            foreach (char item in cardNo)
            {
                try
                {
                    int deger = Convert.ToInt32(item.ToString()) % 1;
                    yeniNo += item;
                }
                catch (Exception)
                {

                }
            }
            if (yeniNo.Length != 16)
                throw new Exception("Kart Numarası Eksik.");
            CreditCard card = new CreditCard();
            if (MyExtension.KontrolSistemi(yeniNo, MyExtension.KontrolTipleri.KrediKarti))
            {
                switch (yeniNo.Substring(0, 1))
                {
                    case "1":
                    case "2":
                        card.CardType = "Havayolları";
                        break;
                    case "3":
                        card.CardType = "Seyahat veya eğlence kartı";
                        break;
                    case "4":
                    case "5":
                        card.CardType = "Hesap kartı";
                        break;
                    case "6":
                    case "7":
                        card.CardType = "Akaryakıt kartı";
                        break;
                    case "8":
                        card.CardType = "Haberleşme(telekominikasyon) kartı";
                        break;
                    case "9":
                        card.CardType = "Uluslararası kart";
                        break;
                }
                card.HesapNo = yeniNo.Substring(6, 9);
                card.CardNo = $"{yeniNo.Substring(0, 4)}-{yeniNo.Substring(4, 4)}-{yeniNo.Substring(8, 4)}-{yeniNo.Substring(12, 4)}";
            }
            else
                throw new Exception("Kart Numarası Hatalı.");
            return card;
        }
    }
}
