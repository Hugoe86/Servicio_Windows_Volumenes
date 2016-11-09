using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reportes_Planeacion.Volumenes.Negocio;
using System.Data;
using SIAC.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace Reportes_Planeacion.Volumenes.Datos
{
    public class Cls_Rpt_Plan_Volumenes_Datos
    {
        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Tarifas_Giro
        //DESCRIPCION: Metodo que Consulta las cuentas congeladas con estatus de cobranza
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Tarifas_Giro(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select *";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  from **********************************************************************************************************************************
                Str_My_Sql += " from Cat_Cor_Giros";

                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta


        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Volumenes
        //DESCRIPCION: Metodo que Consulta las cuentas congeladas con estatus de cobranza
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Volumenes(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select ";
                Str_My_Sql += " g.GIRO_ID";
                Str_My_Sql += ", g.Nombre_Giro ";
                Str_My_Sql += ", isnull(sum(f.Consumo), 0) as Consumo";

                Str_My_Sql += ", MONTH(f.Fecha_Emision) as Bimestre";
                Str_My_Sql += ", year(f.Fecha_Emision) as Anio";

                //Str_My_Sql += ", CASE" +
                //                    " WHEN month(f.Fecha_Emision) = 1 THEN" +
                //                        " 12" +
                //                    " else" +
                //                        " f.Bimestre " +
                //                    " end" +
                //                " as Bimestre";

                //Str_My_Sql += ", CASE " +
                //                    " WHEN month(f.Fecha_Emision) = 1 THEN" +
                //                        " f.Anio - 1" +
                //                    " ELSE" +
                //                        " f.Anio" +
                //                    " END" +
                //                " as Anio";

                Str_My_Sql += ", tp.CUOTA_ID";
        
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  from **********************************************************************************************************************************
                Str_My_Sql += " from Ope_Cor_Facturacion_Recibos f"; 
                Str_My_Sql += " join Cat_Cor_Predios p on p.Predio_ID = f.Predio_ID";
                Str_My_Sql += " JOIN Cat_Cor_Tarifas t ON p.Tarifa_ID = t.Tarifa_ID";
                Str_My_Sql += " JOIN CAT_COR_TIPOS_CUOTAS tp ON tp.CUOTA_ID = t.Cuota_ID";
                Str_My_Sql += " JOIN Cat_Cor_Giros_Actividades ga ON ga.Actividad_Giro_ID = p.Giro_Actividad_ID";
                Str_My_Sql += " JOIN Cat_Cor_Giros g ON g.GIRO_ID = ga.Giro_ID";


                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  where **********************************************************************************************************************************
                Str_My_Sql += " where";
                Str_My_Sql += " year(f.fecha_emision)  in (" + Datos.P_Anio + ")";
                Str_My_Sql += " and month(f.fecha_emision)  in (" + Datos.P_Mes + ")";

                if (Datos.P_Estimado == "NO")
                {
                    Str_My_Sql += " and tp.CUOTA_ID = '00002'";
                }
                else
                {
                    Str_My_Sql += " and tp.CUOTA_ID = '00001'";
                }

            
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  GROUP BY **********************************************************************************************************************************
                Str_My_Sql += " GROUP by";
                Str_My_Sql += " g.GIRO_ID";
                Str_My_Sql += ", g.Nombre_Giro";
                Str_My_Sql += ", tp.CUOTA_ID ";
                Str_My_Sql += ", f.Fecha_Emision";


                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ORDER BY **********************************************************************************************************************************
                Str_My_Sql += " ORDER BY";
                Str_My_Sql += "  year(f.Fecha_Emision) ";
                Str_My_Sql += ", g.GIRO_ID";
                Str_My_Sql += ", month(f.Fecha_Emision)";
                
                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta

        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Volumenes
        //DESCRIPCION: Metodo que Consulta las cuentas congeladas con estatus de cobranza
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Volumenes_Pipa(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select ";
                Str_My_Sql += " isnull(isnull(sum(dd.Cantidad), 0) * isnull(cc.m3_pipa, 0) , 0) as M3";
                Str_My_Sql += ", MONTH(d.Fecha_Creo) as Mes";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  from **********************************************************************************************************************************
                Str_My_Sql += " from Cat_Cor_Conceptos_Cobros cc";
                Str_My_Sql += " join Ope_Cor_Diversos_Detalles dd on cc.Concepto_ID = dd.Concepto_ID";
                Str_My_Sql += " join Ope_Cor_Diversos d on d.No_Diverso = dd.No_Diverso";


                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  where **********************************************************************************************************************************
                Str_My_Sql += " where";
                Str_My_Sql += " cc.Nombre like ('%pipa%') and d.estatus  ='PAGADO'";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  group by **********************************************************************************************************************************
                Str_My_Sql += " GROUP BY	";
                Str_My_Sql += "m3_pipa";
                Str_My_Sql += ", d.Fecha_Creo";

                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta

        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Pagos_A_Facturacion_Planeacion
        //DESCRIPCION: Metodo que los pagos realizados a la facturacion
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Si_Existe_Registro_Volumen(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select ";
                Str_My_Sql += " * ";


                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  from **********************************************************************************************************************************
                Str_My_Sql += " from Ope_Cor_Plan_Volumenes";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  where **********************************************************************************************************************************
                Str_My_Sql += " where";
                Str_My_Sql += " Año = " + Datos.P_Anio;
                Str_My_Sql += " and giro_Id = '" + Datos.P_Giro_Id + "'";

                if (!String.IsNullOrEmpty(Datos.P_Str_Accion))
                {
                    Str_My_Sql += " and accion = " + Datos.P_Str_Accion;
                }

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  **********************************************************************************************************************************

                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta



        //*******************************************************************************
        //NOMBRE_FUNCION:  Modifica_Notificacion
        //DESCRIPCION: Metodo que ingresa la informacion de los montos de la facturacion
        //PARAMETROS : 1.- Cls_Rpt_Plan_Montos_Negocio Clase_Negocios, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 25-Octubre-2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static void Insertar_Registro_Volumenes(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            //Declaración de las variables
            SqlTransaction Obj_Transaccion = null;
            SqlConnection Obj_Conexion = new SqlConnection(Cls_Constantes.Str_Conexion);
            SqlCommand Obj_Comando = new SqlCommand();
            String Mi_SQL = "";

            try
            {
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;


                #region montos_facturacion


                Mi_SQL = "INSERT INTO  Ope_Cor_Plan_Volumenes (";
                Mi_SQL += "  Giro_Id";                          //  1
                Mi_SQL += ", Concepto";                         //  2
                Mi_SQL += ", Año";                              //  3
                Mi_SQL += ", " + Datos.P_Str_Nombre_Mes;        //  4
                Mi_SQL += ", Fecha_Creo";                       //  5
                Mi_SQL += ", Usuario_Creo";                     //  6

                if (!String.IsNullOrEmpty(Datos.P_Str_Accion))
                {
                    Mi_SQL += ", accion ";                      //  7
                }

                Mi_SQL += ")";
                //***************************************************************************
                Mi_SQL += " values ";
                //***************************************************************************
                Mi_SQL += "(";
                Mi_SQL += "  '" + Datos.P_Giro_Id + "'";                                            //  1
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Concepto"].ToString() + "'";                 //  2
                Mi_SQL += ",  " + Convert.ToDouble(Datos.P_Anio).ToString(new CultureInfo("es-MX")) + "";                                                //  3
                Mi_SQL += ",  " + Convert.ToDouble(Datos.P_Dr_Registro[Datos.P_Str_Nombre_Mes].ToString()).ToString(new CultureInfo("es-MX")) + "";      //  4
                Mi_SQL += ",  getdate()";                                                           //  5
                Mi_SQL += ", '" + Datos.P_Str_Usuario + "'";                                        //  6


                if (!String.IsNullOrEmpty(Datos.P_Str_Accion))
                {
                    Mi_SQL += ", " + Datos.P_Str_Accion;                                   //  7
                }


                Mi_SQL += ")";

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                #endregion Fin montos_facturacion

                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //ejecucion de la transaccion    ***********************************************************************************
                Obj_Transaccion.Commit();


            }
            catch (SqlException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Obj_Conexion.Close();
            }


        }// fin del metodo



        //*******************************************************************************
        //NOMBRE_FUNCION:  Actualizar_Registro_Volumenes
        //DESCRIPCION: Metodo que ingresa la informacion de los montos de la facturacion
        //PARAMETROS : 1.- Cls_Rpt_Plan_Montos_Negocio Clase_Negocios, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 25-Octubre-2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static void Actualizar_Registro_Volumenes(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            //Declaración de las variables
            SqlTransaction Obj_Transaccion = null;
            SqlConnection Obj_Conexion = new SqlConnection(Cls_Constantes.Str_Conexion);
            SqlCommand Obj_Comando = new SqlCommand();
            String Mi_SQL = "";

            try
            {
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;


                #region montos_facturacion


                Mi_SQL = "update  Ope_Cor_Plan_Volumenes set ";
                Mi_SQL += "  " + Datos.P_Str_Nombre_Mes + " = " + Convert.ToDouble(Datos.P_Dr_Registro[Datos.P_Str_Nombre_Mes].ToString()).ToString(new CultureInfo("es-MX"));
                Mi_SQL += ", fecha_modifico = getdate()";
                Mi_SQL += ", usuario_modifico = '" + Datos.P_Str_Usuario + "'";
                Mi_SQL += " where id = '" + Datos.P_Id + "'";

                if (!String.IsNullOrEmpty(Datos.P_Str_Accion))
                {
                    Mi_SQL += " and accion = " + Datos.P_Str_Accion ;
                }

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                #endregion Fin montos_facturacion

                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //ejecucion de la transaccion    ***********************************************************************************
                Obj_Transaccion.Commit();


            }
            catch (SqlException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Obj_Conexion.Close();
            }


        }// fin del metodo




        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Tabla_Historicos_Montos_Pagos
        //DESCRIPCION: Metodo que los pagos realizados a la facturacion
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Tabla_Historicos_Volumenes(Cls_Rpt_Plan_Volumenes_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();

            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select ";
                Str_My_Sql += " * ";


                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  from **********************************************************************************************************************************
                Str_My_Sql += " from Ope_Cor_Plan_Volumenes";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  where **********************************************************************************************************************************
                Str_My_Sql += " where";
                Str_My_Sql += " Año = " + Datos.P_Anio;

                if (!String.IsNullOrEmpty(Datos.P_Giro_Id))
                {
                    Str_My_Sql += " and giro_Id = '" + Datos.P_Giro_Id + "'";
                }

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  **********************************************************************************************************************************

                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta




    }
}