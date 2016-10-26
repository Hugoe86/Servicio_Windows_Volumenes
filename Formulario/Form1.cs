using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reportes_Planeacion.Volumenes.Negocio;
using SIAC.Metodos_Generales;

namespace Formulario
{
    public partial class Frm_Prueba : Form
    { //*******************************************************************************
        //NOMBRE DE LA Frm_Prueba
        //DESCRIPCIÓN: 
        //PARAMETROS: 
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 07/Abril/2016
        //MODIFICO:
        //FECHA_MODIFICO:
        //CAUSA_MODIFICACIÓN:
        //*******************************************************************************
        public Frm_Prueba()
        {
            InitializeComponent();
        }
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN:Btn_Prueba_Click
        //DESCRIPCIÓN: 
        //PARAMETROS: 
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 07/Abril/2016
        //MODIFICO:
        //FECHA_MODIFICO:
        //CAUSA_MODIFICACIÓN:
        //*******************************************************************************
        private void Btn_Prueba_Click(object sender, EventArgs e)
        {
            try
            {
                Consultar_Informacion();
                MessageBox.Show("Proceso exitos", "Mensaje", MessageBoxButtons.OK);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Eror:   " + Ex.Message, "Mensaje", MessageBoxButtons.OK);
            }
        }


        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN:Consultar_Informacion
        //DESCRIPCIÓN: Metodo que permite llenar el Grid con la informacion de la consulta
        //PARAMETROS: 
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 07/Abril/2016
        //MODIFICO:
        //FECHA_MODIFICO:
        //CAUSA_MODIFICACIÓN:
        //*******************************************************************************
        public void Consultar_Informacion()
        {
            Cls_Rpt_Plan_Volumenes_Negocio Rs_Consulta = new Cls_Rpt_Plan_Volumenes_Negocio();
            DataTable Dt_Consulta = new DataTable();
            DataTable Dt_Tarifas = new DataTable();
            DataTable Dt_Reporte = new DataTable();
            DataTable Dt_Auxiliar = new DataTable();
            DataTable Dt_Resumen = new DataTable();
            DataRow Dr_Nuevo_Elemento;
            String Str_Nombre_Mes = "";
            Dictionary<Int32, String> Dic_Meses;
            Double Db_Total = 0;
            Decimal Dc_Total = 0;
            DataTable Dt_Existencia = new DataTable();

            try
            {
                Dic_Meses = Cls_Metodos_Generales.Crear_Diccionario_Meses();
                Rs_Consulta.P_Anio = DateTime.Now.Year;
                Rs_Consulta.P_Mes = DateTime.Now.Month;
                Dt_Tarifas = Rs_Consulta.Consultar_Tarifas_Giro();
                Dt_Reporte = Crear_Tabla_Reporte();



                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  Se ingresan los encabezados para las tomas 
                foreach (DataRow Registro in Dt_Tarifas.Rows)
                {
                    Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                    Dr_Nuevo_Elemento["tarifa_Id"] = Registro["giro_id"].ToString();
                    Dr_Nuevo_Elemento["Id"] = Registro["giro_id"].ToString();
                    Dr_Nuevo_Elemento["Accion"] = 1;
                    Dr_Nuevo_Elemento["Concepto"] = "Volumen facturado medido a " + Registro["Nombre_Giro"].ToString() + " (" + Registro["Clave"].ToString() + ")";


                    Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);
                }

                //   se consultan las tomas que se realizaron en el año de consulta
                Rs_Consulta.P_Estimado = "NO";
                Dt_Consulta = Rs_Consulta.Consultar_Volumenes();


                foreach (DataRow Registro in Dt_Reporte.Rows)
                {
                    Registro.BeginEdit();

                    Db_Total = 0;

                    for (int Cont_For = 1; Cont_For <= 12; Cont_For++)
                    {

                        Str_Nombre_Mes = "";
                        if (Dic_Meses.ContainsKey(Cont_For) == true)
                        {
                            Str_Nombre_Mes = Dic_Meses[Cont_For];

                        }//    fin de la validacion del diccionario


                        Dc_Total = (from ord in Dt_Consulta.AsEnumerable()
                                    where ord.Field<String>("giro_id") == Registro["tarifa_id"].ToString()
                                    && ord.Field<Int32>("bimestre") == Cont_For
                                    && ord.Field<Int32>("anio") == Rs_Consulta.P_Anio
                                    select ord.Field<Decimal>("Consumo")
                                              ).Sum();

                        Registro[Str_Nombre_Mes] = Convert.ToDouble(Dc_Total);
                        Db_Total = Db_Total + Convert.ToDouble(Dc_Total);

                    }

                    Registro["Total"] = Db_Total;
                    Db_Total = 0;

                    Registro.EndEdit();
                    Registro.AcceptChanges();

                }


                //  se borraran las tarifas id, ya que comenzara con el tercer proceso
                foreach (DataRow Registro_Reporte in Dt_Reporte.Rows)
                {
                    Registro_Reporte.BeginEdit();
                    Registro_Reporte["Tarifa_Id"] = "";
                    Registro_Reporte.EndEdit();
                    Registro_Reporte.AcceptChanges();

                }


                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  Se ingresan los encabezados para las tomas 
                foreach (DataRow Registro in Dt_Tarifas.Rows)
                {
                    Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                    Dr_Nuevo_Elemento["tarifa_Id"] = Registro["giro_id"].ToString();
                    Dr_Nuevo_Elemento["Id"] = Registro["giro_id"].ToString();
                    Dr_Nuevo_Elemento["Accion"] = 2;
                    Dr_Nuevo_Elemento["Concepto"] = "Volumen facturado estimado " + Registro["Nombre_Giro"].ToString() + " (" + Registro["Clave"].ToString() + ")";


                    Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);
                }

                //   se consultan las tomas que se realizaron en el año de consulta
                Rs_Consulta.P_Estimado = "SI";
                Dt_Consulta = Rs_Consulta.Consultar_Volumenes();


                foreach (DataRow Registro in Dt_Reporte.Rows)
                {
                    if (!String.IsNullOrEmpty(Registro["tarifa_Id"].ToString()))
                    {
                        Registro.BeginEdit();

                        Db_Total = 0;

                        for (int Cont_For = 1; Cont_For <= 12; Cont_For++)
                        {
                            Str_Nombre_Mes = "";
                            if (Dic_Meses.ContainsKey(Cont_For) == true)
                            {
                                Str_Nombre_Mes = Dic_Meses[Cont_For];

                            }//    fin de la validacion del diccionario


                            Dc_Total = (from ord in Dt_Consulta.AsEnumerable()
                                        where ord.Field<String>("giro_id") == Registro["tarifa_id"].ToString()
                                        && ord.Field<Int32>("bimestre") == Cont_For
                                        && ord.Field<Int32>("anio") == Rs_Consulta.P_Anio
                                        select ord.Field<Decimal>("Consumo")
                                                ).Sum();

                            Registro[Str_Nombre_Mes] = Convert.ToDouble(Dc_Total);
                            Db_Total = Db_Total + Convert.ToDouble(Dc_Total);

                        }

                        Registro["Total"] = Db_Total;
                        Db_Total = 0;

                        Registro.EndEdit();
                        Registro.AcceptChanges();
                    }
                }

                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  Volumen de agua distribuido en pipas********************************************************************************************************************
                Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                Dr_Nuevo_Elemento["tarifa_Id"] = "X";
                Dr_Nuevo_Elemento["Id"] = "PIPA";
                Dr_Nuevo_Elemento["Concepto"] = "Volumen de agua distribuido en pipas";
                Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);

                Dt_Consulta.Clear();
                Dt_Consulta = Rs_Consulta.Consultar_Volumenes_Pipa();


                foreach (DataRow Registro in Dt_Reporte.Rows)
                {
                    if (Registro["tarifa_id"] == "X")
                    {
                        Registro.BeginEdit();
                        Db_Total = 0;

                        Registro[Str_Nombre_Mes] = Convert.ToDouble(Dc_Total);
                        Db_Total = Db_Total + Convert.ToDouble(Dc_Total);


                        //  se recorreran los meses del año que se busca
                        for (int Cont_For = 1; Cont_For <= 12; Cont_For++)
                        {
                            Str_Nombre_Mes = "";
                            Dc_Total = 0;

                            if (Dic_Meses.ContainsKey(Cont_For) == true)
                            {
                                Str_Nombre_Mes = Dic_Meses[Cont_For];

                            }//    fin de la validacion del diccionario


                            Dc_Total = (from ord in Dt_Consulta.AsEnumerable()
                                        where ord.Field<Int32>("mes") == Cont_For
                                        select ord.Field<Decimal>("M3")
                                                  ).Sum();
                            Db_Total = Db_Total + Convert.ToDouble(Dc_Total);
                            Registro[Str_Nombre_Mes] = Dc_Total;

                        }

                        Registro["Total"] = Db_Total;
                        Registro.EndEdit();
                        Registro.AcceptChanges();
                    }
                }

                //  se borraran las tarifas id, ya que comenzara con el tercer proceso
                foreach (DataRow Registro_Reporte in Dt_Reporte.Rows)
                {
                    Registro_Reporte.BeginEdit();
                    Registro_Reporte["Tarifa_Id"] = "";
                    Registro_Reporte.EndEdit();
                    Registro_Reporte.AcceptChanges();

                }

                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  Volumen de agua distribuido en bebedero 1********************************************************************************************************************
                Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                Dr_Nuevo_Elemento["tarifa_Id"] = "X";
                Dr_Nuevo_Elemento["Id"] = "BEB1";
                Dr_Nuevo_Elemento["Concepto"] = "Volumen de agua distribuido en bebedero 1";
                Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);

                Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                Dr_Nuevo_Elemento["tarifa_Id"] = "X";
                Dr_Nuevo_Elemento["Id"] = "BEB2";
                Dr_Nuevo_Elemento["Concepto"] = "Volumen de agua distribuido en bebedero 2";
                Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);


                Dr_Nuevo_Elemento = Dt_Reporte.NewRow();
                Dr_Nuevo_Elemento["tarifa_Id"] = "X";
                Dr_Nuevo_Elemento["Id"] = "BEB3";
                Dr_Nuevo_Elemento["Concepto"] = "Volumen de agua distribuido en bebedero 3";
                Dt_Reporte.Rows.Add(Dr_Nuevo_Elemento);


                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  llenan los espacios vacion con cero 0********************************************************************************************************************
                foreach (DataRow Registro_Reporte in Dt_Reporte.Rows)
                {
                    Registro_Reporte.BeginEdit();
                    Registro_Reporte["Tarifa_Id"] = "";

                    for (int Cont_For = 1; Cont_For <= 12; Cont_For++)
                    {
                        Str_Nombre_Mes = "";
                        if (Dic_Meses.ContainsKey(Cont_For) == true)
                        {
                            Str_Nombre_Mes = Dic_Meses[Cont_For];

                        }//    fin de la validacion del diccionario

                        if (String.IsNullOrEmpty(Registro_Reporte[Str_Nombre_Mes].ToString()))
                        {
                            Registro_Reporte[Str_Nombre_Mes] = 0;
                        }
                    }

                    if (String.IsNullOrEmpty(Registro_Reporte["Total"].ToString()))
                    {
                        Registro_Reporte["Total"] = 0;
                    }
                    Registro_Reporte.EndEdit();
                    Registro_Reporte.AcceptChanges();

                }


                //********************************************************************************************************************
                //********************************************************************************************************************
                //********************************************************************************************************************
                //  se ingresara la informacion

                //  se realizara la insercion de la informacion
                foreach (DataRow Registro in Dt_Reporte.Rows)
                {
                    Dt_Existencia.Clear();

                    Str_Nombre_Mes = "";
                    Str_Nombre_Mes = Dic_Meses[DateTime.Now.Month];
                    Rs_Consulta.P_Str_Nombre_Mes = Str_Nombre_Mes;
                    Rs_Consulta.P_Giro_Id = Registro["id"].ToString();
                    Rs_Consulta.P_Anio = DateTime.Now.Year;
                    Rs_Consulta.P_Dr_Registro = Registro;
                    Rs_Consulta.P_Str_Usuario = "Servicio";

                    if (!String.IsNullOrEmpty(Registro["Accion"].ToString()))
                    {
                        Rs_Consulta.P_Str_Accion = Registro["Accion"].ToString();
                    }
                    else
                    {
                        Rs_Consulta.P_Str_Accion = "";
                    }


                    Dt_Existencia = Rs_Consulta.Consultar_Si_Existe_Registro_Volumen();

                    //  validacion de la consulta
                    if (Dt_Existencia != null && Dt_Existencia.Rows.Count > 0)
                    {
                        //  actualizacion
                        Rs_Consulta.P_Id = Dt_Existencia.Rows[0]["ID"].ToString();
                        Rs_Consulta.Actualizar_Registro_Volumenes();

                    }// fin del if
                    else
                    {
                        //  insercion
                        Rs_Consulta.Insertar_Registro_Volumenes();

                    }// fin el else

                }// fin foreach


            }
            catch (Exception Ex)
            {
                MessageBox.Show("Eror:   " + Ex.Message, "Mensaje", MessageBoxButtons.OK);
            }
        }// fin




        /////*******************************************************************************************************
        ///// <summary>
        ///// genera un datatable nuevo con los campos para la 
        ///// </summary>
        ///// <returns>un datatable con los campos para mostrar accesos e ingresos por año y mes</returns>
        ///// <creo>Hugo Enrique Ramírez Aguilera</creo>
        ///// <fecha_creo>13-Enero-2016</fecha_creo>
        ///// <modifico></modifico>
        ///// <fecha_modifico></fecha_modifico>
        ///// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        private DataTable Crear_Tabla_Reporte()
        {
            DataTable Dt_Reporte = new DataTable();

            try
            {
                Dt_Reporte.Columns.Add("tarifa_Id");
                Dt_Reporte.Columns.Add("Id");
                Dt_Reporte.Columns.Add("Accion", typeof(System.Double));
                Dt_Reporte.Columns.Add("Concepto");
                Dt_Reporte.Columns.Add("Enero", typeof(System.Double));
                Dt_Reporte.Columns.Add("Febrero", typeof(System.Double));
                Dt_Reporte.Columns.Add("Marzo", typeof(System.Double));
                Dt_Reporte.Columns.Add("Abril", typeof(System.Double));
                Dt_Reporte.Columns.Add("Mayo", typeof(System.Double));
                Dt_Reporte.Columns.Add("Junio", typeof(System.Double));
                Dt_Reporte.Columns.Add("Julio", typeof(System.Double));
                Dt_Reporte.Columns.Add("Agosto", typeof(System.Double));
                Dt_Reporte.Columns.Add("Septiembre", typeof(System.Double));
                Dt_Reporte.Columns.Add("Octubre", typeof(System.Double));
                Dt_Reporte.Columns.Add("Noviembre", typeof(System.Double));
                Dt_Reporte.Columns.Add("Diciembre", typeof(System.Double));
                Dt_Reporte.Columns.Add("Total", typeof(System.Double));
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error:   " + Ex.Message, "Mensaje", MessageBoxButtons.OK);
            }

            return Dt_Reporte;
        }

    }
}
