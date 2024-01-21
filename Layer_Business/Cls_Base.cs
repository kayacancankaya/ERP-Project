using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layer_Business
{
    public abstract class Cls_Base
    {
        
        public enum DovizTipi
        {
            TL = 0,
            USD = 1,
            EUR = 2,
            GBP = 3,
        }
        public enum SiparisTipi
        {
            Yurtici = 1,
            Yurtdisi = 2,
         
        }
        public enum UretimDurum
        {
            Uretildi = 1,
            Uretilmedi = 2,
            Tamamlanmadı = 3,
        }

        public static Cls_Base.DovizTipi GetEnumDovizTipi(string dovizTipi)
        {
            Cls_Base.DovizTipi selectedDovizEnum;

            switch (dovizTipi)
            {
                case "TL":
                    selectedDovizEnum = Cls_Base.DovizTipi.TL;
                    break;
                case "USD":
                    selectedDovizEnum = Cls_Base.DovizTipi.USD;
                    break;
                case "EUR":
                    selectedDovizEnum = Cls_Base.DovizTipi.EUR;
                    break;
                case "GBP":
                    selectedDovizEnum = Cls_Base.DovizTipi.GBP;
                    break;
                default:
                    selectedDovizEnum = Cls_Base.DovizTipi.TL;
                    break;
            }
            return selectedDovizEnum;
        }
        public static Cls_Base.SiparisTipi GetEnumSiparisTipi(string siparisTipi)
        {
            Cls_Base.SiparisTipi selectedSiparisEnum;

            switch (siparisTipi)
            {
                case "Yurtici":
                    selectedSiparisEnum = Cls_Base.SiparisTipi.Yurtici;
                    break;
                case "Yurtdisi":
                    selectedSiparisEnum = Cls_Base.SiparisTipi.Yurtdisi;
                    break;
                default:
                    selectedSiparisEnum = Cls_Base.SiparisTipi.Yurtici;
                    break;
            }
            return selectedSiparisEnum;
        }
    }

}
