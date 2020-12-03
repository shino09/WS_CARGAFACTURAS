using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseDatos
{
    public class InfoFacturas
    {


        public long providerRuc { get; set; }
        public string series { get; set; }
        public long numeration { get; set; }
        public string authorizationNumber { get; set; }
        public string invoiceType { get; set; }
        public DateTime expirationDate { get; set; }
        public string department { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string addressSupplier { get; set; }
        public string acqDepartment { get; set; }
        public string acqProvince { get; set; }
        public string acqDistrict { get; set; }
        public string addressAcquirer { get; set; }
        public int typePayment { get; set; }
        public int numberQuota { get; set; }
        public DateTime deliverDateAcq { get; set; }
        public DateTime aceptedDate { get; set; }
        public DateTime paymentDate { get; set; }
        public Decimal netAmount { get; set; }
        public string other1 { get; set; }
        public string other2 { get; set; }
        public string additionalField1 { get; set; }
        public string additionalField2 { get; set; }

    }
}
