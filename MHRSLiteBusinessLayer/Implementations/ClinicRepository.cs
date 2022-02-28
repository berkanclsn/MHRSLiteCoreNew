using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataLayer;
using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
   public class ClinicRepository:Repository<Clinic>, IClinicRepository
    {

        public ClinicRepository(MyContext myContext) : base(myContext)
        {


        }
        public void Deneme()
        {
            //Repositorylere kalıtım aldıkları yerdeki metotlar
            //yeterli gözüküyor. Ancak ilerleyen zamanlarda generic yapının karşılamadığı bir ihtiyaç olursa buraya bir metot eklenebilir. O metot _myContext'i kullanarak işlemi yapsın diye burada _myContext'i protected özelliğiyle kalıtım aldık.
            //Örn: Bir önceki projemizdeki CategoryRepository'de dashboard için ir ihtiyaç doğmuştu.
            //Örn; Sistem yöneticilerin ya da müdürlerin istediği raporlar
            //Örn; İstanbuldaki toplam Dahiliye klinik sayısı 
            // Aşağıdaki gibi kullanımlar yapabiliriz.
            //var x= from h in _myContext.Hospitals


        }

    }
}
