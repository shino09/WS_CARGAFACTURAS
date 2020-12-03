using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseDatos
{
    public class InfoCuotas
    {
        public long providerRuc { get; set; }
        public string series { get; set; }
        public int numeration { get; set; }
        public int number { set; get; }
        public Decimal netAmount { set; get; }
        public DateTime paymentDate { set; get; }


    }
}
