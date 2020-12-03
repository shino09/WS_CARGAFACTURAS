using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;

namespace ClaseDatos
{
    public class ClassProce
    {

        SqlQuery sqlquery = new SqlQuery();
        public int idTrans { get; set; }
        public string mensaje { get; set; }

        public ArrayList ObtenerTipoParticipante()
        {
            DataSet ds;
            string sqlstr = "";
            ArrayList lista = new ArrayList();

            try
            {
                sqlstr = "P_GET_PAR_TIP_CNSMR";
                ds = sqlquery.ExecuteDataSet(sqlstr);
                Console.WriteLine(ds);


               foreach (DataRow dtRow in ds.Tables[0].Rows)

                {
                    lista.Add(dtRow);
                }

                /*if (ds.Tables[0].Rows.Count > 0)
                {
                    lista.Add(ds.Tables[0].Rows[0]);

                }*/
                return lista;

            }

            
            catch (Exception)
            {
                //ds = null;
                //return ds;
                        return lista;

            }

            //return ds;


        }
    }

    public class Parametro
    {
        public string codigo { set; get; }
        public string descripcion { set; get; }

    }

    public class InfoFacturas2
    {
        public long providerRuc { get; set; }
        public string series { get; set; }
        public int numeration { get; set; }
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

    public class InfoCuotas2
    {
        public long providerRuc { get; set; }
        public string series { get; set; }
        public int numeration { get; set; }
        public int number { set; get; }
        public Double netAmount { set; get; }
        public DateTime paymentDate { set; get; }
    }
}



     
        
