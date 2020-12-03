using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.IO;
using ClaseDatos;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using dim.rutinas;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
namespace WS_TEST

{
    
    /// <summary>
    /// Descripción breve de WS_TEST
    /// </summary>
    [WebService(Namespace = "http://cesion.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]

    [Serializable]
    public class WS_TEST : System.Web.Services.WebService
    {
        RutinasGenerales rg = new RutinasGenerales();
        ClassProce cls = new ClassProce();
        SqlQuery sq = new SqlQuery();
        #region VARIABLES
        Boolean exito=false;

        public string mensaje { get; set; }
        public RespuestaFacturas respuestaFacturas = new RespuestaFacturas();
        #endregion

        [WebMethod(Description = "Obtener Tipo de participante")]
        public List<Parametro> GetTipoParticipante()
        {
            List<Parametro> lista = new List<Parametro>();
            DataSet ds;
            string squery = "";

            squery = "EXEC P_GET_PAR_TIP_CNSMR";

            ds = sq.ExecuteDataSet(squery);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int cont=0;cont< ds.Tables[0].Rows.Count; cont++)
                    {
                        Parametro par = new Parametro();
                        par.codigo = ds.Tables[0].Rows[cont]["PAR_TIP_CNSMR_COD"].ToString();
                        par.descripcion = ds.Tables[0].Rows[cont]["PAR_TIP_CNSMR_DET"].ToString();
                        lista.Add(par);

                    }
                }
            }


            return lista;
        }


        [WebMethod(Description = "Recibe listado con los datos adicionales de las facturas")]
        public string GuardarInfoAdiccional(List<InfoFacturas> listaInfoFacturas, List<InfoCuotas> listaInfoCuotas)
        {
            DataSet dsTrans = null;
            DataSet dsExisteFactura = null;
            DataSet dsFactura = null;
            DataSet dsExisteCuota = null;            
            DataSet dsCuota = null;

            string strg_sql_trans = "";
            string strg_sql_facturas = "";
            string strg_sql_cuotas = "";
            string strgSqlExisteFactura = "";
            string strgSqlExisteCuota = "";
            string providerRuc, numeration, series, number;
            string idCARGAR_INFO_04003_TRANS = null;
            string idCARGAR_INFO_04003_FACTURAS = null;
            Console.Write(listaInfoFacturas);
            Console.Write(listaInfoCuotas);
            bool existeFactura = false;
            bool existeCuota = false;
            try
            {
              
                strg_sql_trans = "EXEC P_INS_CARGAR_INFO_04003_TRANS ";

                dsTrans = sq.ExecuteDataSet(strg_sql_trans);

                if (dsTrans != null)
                {
                    idCARGAR_INFO_04003_TRANS= dsTrans.Tables[0].Rows[0]["idCARGAR_INFO_04003_TRANS"].ToString();

                    //RECORRER LA LISTA DE LAS FACTURAS
                    for (int cont = 0; cont < listaInfoFacturas.Count; cont++)
            {
                InfoFacturas elementoInfoFactura = new InfoFacturas();
                elementoInfoFactura = listaInfoFacturas[cont];

                        //buscamos el ruc de la factura
                        providerRuc = elementoInfoFactura.providerRuc.ToString();
                        //buscamos el nro de factura - serie+numeracion
                        series = elementoInfoFactura.series;
                        numeration = elementoInfoFactura.numeration.ToString();

                        //var existe = (from c in listaInfoFacturas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == long.Parse(numeration)) select c).Any();

                        //strgSqlExisteFactura = "SELECT idCARGAR_INFO_04003_FACTURAS FROM CARGAR_INFO_04003_FACTURAS WHERE CARGAR_INFO_04003_FACTURAS_providerRuc=" + "'" + providerRuc + "' and CARGAR_INFO_04003_FACTURAS_series='" + series  + "' and CARGAR_INFO_04003_FACTURAS_numeration='" + numeration + "'";
                        strgSqlExisteFactura = "EXEC 	P_GET_CARGAR_INFO_04003_EXISTEFACTURAS" + "'" + idCARGAR_INFO_04003_TRANS + "','" + providerRuc + "','" + series + "','" + numeration + "'";  

                        dsExisteFactura = sq.ExecuteDataSet(strgSqlExisteFactura);
                        if (dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString() != null && dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString() != "")
                        {
                            existeFactura = true;
                            idCARGAR_INFO_04003_FACTURAS = dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString();
                        }
                        else
                        {
                            existeFactura = false;
                        }

                        if (!existeFactura)
                        {
                            strg_sql_facturas = "EXEC P_INS_CARGAR_INFO_04003_FACTURAS " + "'" + elementoInfoFactura.providerRuc + "','" + elementoInfoFactura.series + "','" + elementoInfoFactura.numeration + "','"
                    + elementoInfoFactura.authorizationNumber + "','" + elementoInfoFactura.invoiceType + "' ,'" + elementoInfoFactura.expirationDate + "','" + elementoInfoFactura.department + "','" + elementoInfoFactura.province + " ','" + elementoInfoFactura.district + " ','" + elementoInfoFactura.addressSupplier + "' ,'" + elementoInfoFactura.acqDepartment
                    + "','" + elementoInfoFactura.acqProvince + "','" + elementoInfoFactura.acqDistrict + "' ,'" + elementoInfoFactura.addressAcquirer + "' ,'" + elementoInfoFactura.typePayment + "','" + elementoInfoFactura.numberQuota + "' ,'"
                    + elementoInfoFactura.deliverDateAcq + "','" + elementoInfoFactura.aceptedDate + "' ,'" + elementoInfoFactura.paymentDate + "' ,'" + rg.ComasXpuntosMontos(elementoInfoFactura.netAmount.ToString()) + "' ,'" + elementoInfoFactura.other1 + "' ,'"
                    + elementoInfoFactura.other2 + "','" + elementoInfoFactura.additionalField1 + "' ,'" + elementoInfoFactura.additionalField2 + "' ,'" + idCARGAR_INFO_04003_TRANS + "'";

                            dsFactura = sq.ExecuteDataSet(strg_sql_facturas);
                            idCARGAR_INFO_04003_FACTURAS = dsFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString();
                        }


                        //return true;

                        //cuotas


                        List<InfoCuotas> listaInfoCuotas_aux = new List<InfoCuotas>();

                        listaInfoCuotas_aux = (from c in listaInfoCuotas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == int.Parse(numeration)) select c).ToList();
                        

                        for (int cont2 = 0; cont2 < listaInfoCuotas_aux.Count; cont2++)
                            {
                                InfoCuotas elementoInfoCuotas = new InfoCuotas();
                                elementoInfoCuotas = listaInfoCuotas_aux[cont2];
                                number = elementoInfoCuotas.number.ToString();
                                //existeCuota = (from c in listaInfoCuotas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == int.Parse(numeration) & c.number == int.Parse(number)) select c).Any();

                                //strgSqlExisteCuota = "SELECT COUNT(*) FROM CARGAR_INFO_04003_CUOTAS WHERE idCARGAR_INFO_04003_FACTURAS=" + "'" + idCARGAR_INFO_04003_FACTURAS + "' and CARGAR_INFO_04003_CUOTAS_number='" + number  + "'";
                                strgSqlExisteCuota = "EXEC 	P_GET_CARGAR_INFO_04003_EXISTECUOTAS" + "'" + idCARGAR_INFO_04003_TRANS + "','" + Convert.ToInt64(providerRuc) + "','" + series + "','" + Convert.ToInt64(numeration) + "','" + number + "'";

                                dsExisteCuota = sq.ExecuteDataSet(strgSqlExisteCuota);
                                if (dsExisteCuota.Tables[0].Rows[0]["CONTADOR"].ToString() != "0")
                                {
                                    existeCuota = true;
                                }
                                else
                                {
                                    existeCuota = false;

                                }
                                if (!existeCuota)
                                {
                                    strg_sql_cuotas = "EXEC P_INS_CARGAR_INFO_04003_CUOTAS " + "'" + elementoInfoCuotas.number + "' ,'" + rg.ComasXpuntosMontos(elementoInfoCuotas.netAmount.ToString()) + "', '" + elementoInfoCuotas.paymentDate + "' ,'" + idCARGAR_INFO_04003_FACTURAS + "'";
                                    dsCuota = sq.ExecuteDataSet(strg_sql_cuotas);
                                //listaInfoCuotas.RemoveAt(cont2);
                                //return true
                            }
                            }

                      
                    }

            }


                if (dsCuota != null)
                {
                    return idCARGAR_INFO_04003_TRANS;
                }
            }
            catch (Exception e)
            {
                return idCARGAR_INFO_04003_TRANS;
            }
            Console.Write(strg_sql_facturas);
            Console.Write(strg_sql_cuotas);


            //GENERO EL STRING DE INSERCION.

            return idCARGAR_INFO_04003_TRANS;
        }



        [WebMethod(Description = "Guarda el contenido del xls en la bd")]
        public int  GuardarContenidoExcel(string nombreArchivo, string nombreHoja , string rutaArchivo ,string rutaArchivoCompleta ,string extensionArchivo)
        {

            string versionExcel="", tipoExcel="";
            switch (extensionArchivo)
            {
                case "XLSX":
                    tipoExcel = "Microsoft.ACE.OLEDB.12.0";
                    versionExcel = "Excel 12.0";
                    break;
                case "xlsx":
                    tipoExcel = "Microsoft.ACE.OLEDB.12.0";
                    versionExcel = "Excel 12.0";
                    break;
                case "XLS":
                    tipoExcel = "Microsoft.Jet.OLEDB.4.0";
                    versionExcel = "Excel 8.0";
                    break;
                case "xls":
                    tipoExcel = "Microsoft.Jet.OLEDB.4.0";
                    versionExcel = "Excel 8.0";
                    break;
            }

            DataSet ds = null;
            string strg_sql = "";
            strg_sql = "P_INS_VALIDACIONES_EXCEL_4003 " + "'" + tipoExcel + "' ,'" + versionExcel + "', '" + rutaArchivoCompleta + "' ,'" + nombreHoja + "'";
            ds = sq.ExecuteDataSet(strg_sql);
            int id_Trans=0;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int cont = 0; cont < ds.Tables[0].Rows.Count; cont++)
                    {
                        id_Trans = Convert.ToInt32(ds.Tables[0].Rows[cont]["idTRANS"].ToString());

                    }
                    
                }
                return id_Trans;
            }
            else
            {
                return 0;
            }
        }


        [WebMethod(Description = "Validar el excel desde la BD")]
        public DataSet ValidarExcelBD(int id_Trans)
        {

            //DataSet ds_nulos_todos = null, ds_tipodatos_todos = null, ds_largo_todos = null, ds_nulos_cuotas_unico = null, ds_tipodatos_cuotas_unico = null,
            //    ds_largo_cuotas_unico = null, ds_nulos_cuotas = null, ds_tipodatos_cuotas = null, ds_largo_cuotas = null, ds_fechas = null;//, ds_final = null;
            string strg_sql_nulos_todos = "", strg_sql_tipodatos_todos = "", strg_sql_largo_todos = "", strg_sql_nulos_cuotas_unico = "", strg_sql_tipodatos_cuotas_unico = "",
                strg_sql_largo_cuotas_unico = "", strg_sql_nulos_cuotas = "", strg_sql_tipodatos_cuotas = "", strg_sql_largo_cuotas = "", strg_sql_fechas = "";
            int error_todos = 0, error_cuotas_unico = 0, error_cuotas = 0, error_fechas = 0;
            DataSet ds_final = new DataSet();
            DataSet ds_nulos_todos = new DataSet();
            DataSet ds_tipodatos_todos = new DataSet();
            DataSet ds_largo_todos = new DataSet();
            DataSet ds_nulos_cuotas_unico = new DataSet();
            DataSet ds_tipodatos_cuotas_unico = new DataSet();
            DataSet ds_largo_cuotas_unico = new DataSet();
            DataSet ds_nulos_cuotas = new DataSet();
            DataSet ds_tipodatos_cuotas = new DataSet();
            DataSet ds_largo_cuotas = new DataSet();
            DataSet ds_fechas = new DataSet();

            strg_sql_nulos_todos = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_NULOS_TODOS " + "'" + id_Trans + "'";
            ds_nulos_todos = sq.ExecuteDataSet(strg_sql_nulos_todos);

            if (ds_nulos_todos.Tables[0].Rows.Count == 0)
            {
                strg_sql_tipodatos_todos = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_TIPODATOS_TODOS " + "'" + id_Trans + "'";
                ds_tipodatos_todos = sq.ExecuteDataSet(strg_sql_tipodatos_todos);

                if (ds_tipodatos_todos.Tables[0].Rows.Count == 0)
                {
                    strg_sql_largo_todos = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_LARGO_TODOS " + "'" + id_Trans + "'";
                    ds_largo_todos = sq.ExecuteDataSet(strg_sql_largo_todos);


                    //ds_nulos_todos.Merge(ds_tipodatos_todos);
                    /*  if (ds_nulos_todos.Tables[0].Rows.Count > 0 || ds_tipodatos_todos.Tables[0].Rows.Count > 0 || ds_largo_todos.Tables[0].Rows.Count > 0)
                      {
                          error_todos = 1;
                      }*/



                    /* if (error_todos != 1)
                     {*/

                    if (ds_largo_todos.Tables[0].Rows.Count == 0)
                    {
                        strg_sql_nulos_cuotas_unico = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_NULO_CUOTAS_UNICO " + "'" + id_Trans + "'";
                        ds_nulos_cuotas_unico = sq.ExecuteDataSet(strg_sql_nulos_cuotas_unico);

                        if (ds_nulos_cuotas_unico.Tables[0].Rows.Count == 0)
                        {
                            strg_sql_tipodatos_cuotas_unico = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_TIPODATOS_CUOTAS_UNICO " + "'" + id_Trans + "'";
                            ds_tipodatos_cuotas_unico = sq.ExecuteDataSet(strg_sql_tipodatos_cuotas_unico);
                            if (ds_tipodatos_cuotas_unico.Tables[0].Rows.Count == 0)
                            {
                                strg_sql_largo_cuotas_unico = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_LARGO_CUOTAS_UNICO " + "'" + id_Trans + "'";
                                ds_largo_cuotas_unico = sq.ExecuteDataSet(strg_sql_largo_cuotas_unico);


                                /* if (ds_nulos_cuotas_unico != null || ds_tipodatos_cuotas_unico != null || ds_largo_cuotas_unico != null)
                                 {
                                     error_cuotas_unico = 1;
                                 }


                                 if (error_cuotas_unico != 1)
                                 {*/

                                if (ds_largo_cuotas_unico.Tables[0].Rows.Count == 0)
                                {
                                    strg_sql_nulos_cuotas = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_NULO_CUOTAS " + "'" + id_Trans + "'";
                                    ds_nulos_cuotas = sq.ExecuteDataSet(strg_sql_nulos_cuotas);
                                    if (ds_nulos_cuotas.Tables[0].Rows.Count == 0)
                                    {
                                        strg_sql_tipodatos_cuotas = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_TIPODATOS_CUOTAS " + "'" + id_Trans + "'";
                                        ds_tipodatos_cuotas = sq.ExecuteDataSet(strg_sql_tipodatos_cuotas);
                                        if (ds_tipodatos_cuotas.Tables[0].Rows.Count == 0)
                                        {
                                            strg_sql_largo_cuotas = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_LARGO_CUOTAS " + "'" + id_Trans + "'";
                                            ds_largo_cuotas = sq.ExecuteDataSet(strg_sql_largo_cuotas);


                                            /*
                            if (ds_nulos_cuotas != null || ds_tipodatos_cuotas != null || ds_largo_cuotas != null)
                            {
                                error_cuotas = 1;
                            }

                            if (error_cuotas != 1)
                            {*/
                                            if (ds_largo_cuotas.Tables[0].Rows.Count == 0)
                                            {
                                                strg_sql_fechas = "P_GET_ERRORES_VALIDACIONES_EXCEL_4003_FECHAS " + "'" + id_Trans + "'";
                                                ds_fechas = sq.ExecuteDataSet(strg_sql_fechas);
                                                /* if (ds_fechas != null)
                                                 {
                                                     error_fechas = 1;
                                                 }*/
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
                

            //DataSet ds_final = new DataSet();
            //ds_final.Tables.Add('dssdds');
            //ds_final.Tables[0].Rows[0]["idVALIDACIONES_EXCEL_4003_TRASPASO"] = '0';
            //ds_final.Tables[0].Rows[0]["MENSAJE"] = '0';

            if (ds_nulos_todos.Tables.Count > 0)
            {
                ds_final.Merge(ds_nulos_todos);
            }
            if (ds_tipodatos_todos.Tables.Count > 0)
            {
                ds_final.Merge(ds_tipodatos_todos);
            }
            if (ds_largo_todos.Tables.Count > 0)
            {
                ds_final.Merge(ds_largo_todos);
            }
            if (ds_nulos_cuotas_unico.Tables.Count > 0)
            {
                ds_final.Merge(ds_nulos_cuotas_unico);
            }
            if (ds_tipodatos_cuotas_unico.Tables.Count >0)
            {
                ds_final.Merge(ds_tipodatos_cuotas_unico);
            }
            if (ds_largo_cuotas_unico.Tables.Count >0)
            {
                ds_final.Merge(ds_largo_cuotas_unico);
            }
            if (ds_nulos_cuotas.Tables.Count > 0)
            {
                ds_final.Merge(ds_nulos_cuotas);
            }
            if (ds_tipodatos_cuotas.Tables.Count > 0)
            {
                ds_final.Merge(ds_tipodatos_cuotas);
            }
            if (ds_largo_cuotas.Tables.Count > 0)
            {
                ds_final.Merge(ds_largo_cuotas);
            }
           if (ds_fechas.Tables.Count > 0)
               {
              ds_final.Merge(ds_fechas);
                    }
                                   
                
            if (ds_final.Tables.Count > 0)
            {
                return ds_final;
            }
            else
            {
                return null;
            }
        }

        [WebMethod(Description = "Obtiene los errores de validacion de las Facturas")]
        public DataSet GetErroresValidacion(int idTrans)
        {
            List<InfoFacturas> lista = new List<InfoFacturas>();
            DataSet ds;
            string squery = "";

            squery = "EXEC P_GET_CARGAR_INFO_04003_FACTURAS_ERRORES_VALIDACION" + "'" + idTrans + "'";
            ds = sq.ExecuteDataSet(squery);
            return ds;
        }



        [WebMethod(Description = "Recibe listado con los datos adicionales de las facturas2")]
        public string GuardarInfoAdiccional2(List<InfoFacturas> listaInfoFacturas, List<InfoCuotas> listaInfoCuotas)
        {
            DataSet dsTrans = null;
            DataSet dsExisteFactura = null;
            DataSet dsFactura = null;
            DataSet dsExisteCuota = null;
            DataSet dsCuota = null;

            string strg_sql_trans = "";
            string strg_sql_facturas = "";
            string strg_sql_cuotas = "";
            string strgSqlExisteFactura = "";
            string strgSqlExisteCuota = "";
            string providerRuc, numeration, series, number;
            string idCARGAR_INFO_04003_TRANS = null;
            string idCARGAR_INFO_04003_FACTURAS = null;
            Console.Write(listaInfoFacturas);
            Console.Write(listaInfoCuotas);
            bool existeFactura = false;
            bool existeCuota = false;
            try
            {

                strg_sql_trans = "EXEC P_INS_CARGAR_INFO_04003_TRANS ";

                dsTrans = sq.ExecuteDataSet(strg_sql_trans);

                if (dsTrans != null)
                {
                    idCARGAR_INFO_04003_TRANS = dsTrans.Tables[0].Rows[0]["idCARGAR_INFO_04003_TRANS"].ToString();

                    //RECORRER LA LISTA DE LAS FACTURAS
                    for (int cont = 0; cont < listaInfoFacturas.Count; cont++)
                    {
                        InfoFacturas elementoInfoFactura = new InfoFacturas();
                        elementoInfoFactura = listaInfoFacturas[cont];

                        //buscamos el ruc de la factura
                        providerRuc = elementoInfoFactura.providerRuc.ToString();
                        //buscamos el nro de factura - serie+numeracion
                        series = elementoInfoFactura.series;
                        numeration = elementoInfoFactura.numeration.ToString();

                        //var existe = (from c in listaInfoFacturas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == long.Parse(numeration)) select c).Any();

                        //strgSqlExisteFactura = "SELECT idCARGAR_INFO_04003_FACTURAS FROM CARGAR_INFO_04003_FACTURAS WHERE CARGAR_INFO_04003_FACTURAS_providerRuc=" + "'" + providerRuc + "' and CARGAR_INFO_04003_FACTURAS_series='" + series  + "' and CARGAR_INFO_04003_FACTURAS_numeration='" + numeration + "'";
                        strgSqlExisteFactura = "EXEC 	P_GET_CARGAR_INFO_04003_EXISTEFACTURAS" + "'" + idCARGAR_INFO_04003_TRANS + "','" + providerRuc + "','" + series + "','" + numeration + "'";

                        dsExisteFactura = sq.ExecuteDataSet(strgSqlExisteFactura);
                        if (dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString() != null && dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString() != "")
                        {
                            existeFactura = true;
                            idCARGAR_INFO_04003_FACTURAS = dsExisteFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString();
                        }
                        else
                        {
                            existeFactura = false;
                        }

                        if (!existeFactura)
                        {
                            strg_sql_facturas = "EXEC P_INS_CARGAR_INFO_04003_FACTURAS " + "'" + elementoInfoFactura.providerRuc + "','" + elementoInfoFactura.series + "','" + elementoInfoFactura.numeration + "','"
                    + elementoInfoFactura.authorizationNumber + "','" + elementoInfoFactura.invoiceType + "' ,'" + elementoInfoFactura.expirationDate + "','" + elementoInfoFactura.department + "','" + elementoInfoFactura.province + " ','" + elementoInfoFactura.district + " ','" + elementoInfoFactura.addressSupplier + "' ,'" + elementoInfoFactura.acqDepartment
                    + "','" + elementoInfoFactura.acqProvince + "','" + elementoInfoFactura.acqDistrict + "' ,'" + elementoInfoFactura.addressAcquirer + "' ,'" + elementoInfoFactura.typePayment + "','" + elementoInfoFactura.numberQuota + "' ,'"
                    + elementoInfoFactura.deliverDateAcq + "','" + elementoInfoFactura.aceptedDate + "' ,'" + elementoInfoFactura.paymentDate + "' ,'" + rg.ComasXpuntosMontos(elementoInfoFactura.netAmount.ToString()) + "' ,'" + elementoInfoFactura.other1 + "' ,'"
                    + elementoInfoFactura.other2 + "','" + elementoInfoFactura.additionalField1 + "' ,'" + elementoInfoFactura.additionalField2 + "' ,'" + idCARGAR_INFO_04003_TRANS + "'";

                            dsFactura = sq.ExecuteDataSet(strg_sql_facturas);
                            idCARGAR_INFO_04003_FACTURAS = dsFactura.Tables[0].Rows[0]["idCARGAR_INFO_04003_FACTURAS"].ToString();
                        }


                        //return true;

                        //cuotas


                        List<InfoCuotas> listaInfoCuotas_aux = new List<InfoCuotas>();

                        listaInfoCuotas_aux = (from c in listaInfoCuotas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == int.Parse(numeration)) select c).ToList();


                        for (int cont2 = 0; cont2 < listaInfoCuotas_aux.Count; cont2++)
                        {
                            InfoCuotas elementoInfoCuotas = new InfoCuotas();
                            elementoInfoCuotas = listaInfoCuotas_aux[cont2];
                            number = elementoInfoCuotas.number.ToString();
                            //existeCuota = (from c in listaInfoCuotas where (c.providerRuc == long.Parse(providerRuc) & c.series == series & c.numeration == int.Parse(numeration) & c.number == int.Parse(number)) select c).Any();

                            //strgSqlExisteCuota = "SELECT COUNT(*) FROM CARGAR_INFO_04003_CUOTAS WHERE idCARGAR_INFO_04003_FACTURAS=" + "'" + idCARGAR_INFO_04003_FACTURAS + "' and CARGAR_INFO_04003_CUOTAS_number='" + number  + "'";
                            strgSqlExisteCuota = "EXEC 	P_GET_CARGAR_INFO_04003_EXISTECUOTAS" + "'" + idCARGAR_INFO_04003_TRANS + "','" + Convert.ToInt64(providerRuc) + "','" + series + "','" + Convert.ToInt64(numeration) + "','" + number + "'";

                            dsExisteCuota = sq.ExecuteDataSet(strgSqlExisteCuota);
                            if (dsExisteCuota.Tables[0].Rows[0]["CONTADOR"].ToString() != "0")
                            {
                                existeCuota = true;
                            }
                            else
                            {
                                existeCuota = false;

                            }
                            if (!existeCuota)
                            {
                                strg_sql_cuotas = "EXEC P_INS_CARGAR_INFO_04003_CUOTAS " + "'" + elementoInfoCuotas.number + "' ,'" + rg.ComasXpuntosMontos(elementoInfoCuotas.netAmount.ToString()) + "', '" + elementoInfoCuotas.paymentDate + "' ,'" + idCARGAR_INFO_04003_FACTURAS + "'";
                                dsCuota = sq.ExecuteDataSet(strg_sql_cuotas);
                                //listaInfoCuotas.RemoveAt(cont2);
                                //return true
                            }
                        }


                    }

                }


                if (dsCuota != null)
                {
                    return idCARGAR_INFO_04003_TRANS;
                }
            }
            catch (Exception e)
            {
                return idCARGAR_INFO_04003_TRANS;
            }
            Console.Write(strg_sql_facturas);
            Console.Write(strg_sql_cuotas);


            //GENERO EL STRING DE INSERCION.

            return idCARGAR_INFO_04003_TRANS;
        }

        [WebMethod(Description = "Obtiene los datos adicionales de las Facturas")]

        public DataSet GetInfoAdiccionalFacturas(int idTrans)
        {
            List<InfoFacturas> lista = new List<InfoFacturas>();
            DataSet ds;
            string squery = "";

            squery = "EXEC P_GET_CARGAR_INFO_04003_FACTURAS" + "'" + idTrans + "'";

            ds = sq.ExecuteDataSet(squery);
            return ds;
        }


        [WebMethod(Description = "Obtiene los datos adicionales de las Cuotas")]

        public DataSet GetInfoAdiccionalCuotas(int idFactura)
        {
            List<InfoCuotas> lista = new List<InfoCuotas>();
            DataSet ds;
            string squery = "";

            squery = "EXEC P_GET_CARGAR_INFO_04003_CUOTAS" + "'" + idFactura + "'";

            ds = sq.ExecuteDataSet(squery);
            
            return ds;
        }

        [WebMethod(Description = "Vaciar las tablas trans , facturas y cuotas")]

        public void VaciarTablas()
        {
            DataSet ds;
            string squery = "";
            squery = "EXEC P_DEL_CARGAR_INFO_04003_VACIAR_TABLAS";
            ds = sq.ExecuteDataSet(squery);
        }


        [WebMethod(Description = "Vaciar las tablas trans , traspaso")]

        public void VaciarTablasValidaciones(int idTrans)
        {
            DataSet ds;
            string squery = "";
            squery = "EXEC P_DEL_VALIDACIONES_04003_VACIAR_TABLAS" + "'" + idTrans + "'"; ;
            ds = sq.ExecuteDataSet(squery);
        }

        [WebMethod(Description = "Obtiene los errores de validacion de las Facturas")]
        public DataSet GetErroresValidacionVIEJO(int idTrans)
        {
            List<InfoFacturas> lista = new List<InfoFacturas>();
            DataSet ds;
            string squery = "";

            squery = "EXEC P_GET_CARGAR_INFO_04003_FACTURAS_ERRORES_VALIDACION" + "'" + idTrans + "'";
            ds = sq.ExecuteDataSet(squery);
            return ds;
        }

    }
}
