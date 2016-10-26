using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Reportes_Planeacion.Volumenes.Datos;

namespace Reportes_Planeacion.Volumenes.Negocio
{
    public class Cls_Rpt_Plan_Volumenes_Negocio
    {
        #region Variables_Publicas
        public String P_Rpu { get; set; }
        public String P_No_Cuenta { get; set; }
        public String P_Estimado { get; set; }
        public Int32 P_Anio { get; set; }
        public Int32 P_Mes { get; set; }
        public String P_Str_Nombre_Mes { get; set; }
        public String P_Giro_Id { get; set; }
        public String P_Id { get; set; }
        public DataRow P_Dr_Registro { get; set; }
        public String P_Str_Usuario { get; set; }
        public String  P_Str_Accion { get; set; }

        #endregion

        #region Consultas
        public DataTable Consultar_Tarifas_Giro() { return Cls_Rpt_Plan_Volumenes_Datos.Consultar_Tarifas_Giro(this); }
        public DataTable Consultar_Volumenes() { return Cls_Rpt_Plan_Volumenes_Datos.Consultar_Volumenes(this); }
        public DataTable Consultar_Volumenes_Pipa() { return Cls_Rpt_Plan_Volumenes_Datos.Consultar_Volumenes_Pipa(this); }

        public DataTable Consultar_Si_Existe_Registro_Volumen() { return Cls_Rpt_Plan_Volumenes_Datos.Consultar_Si_Existe_Registro_Volumen(this); }
        public void Insertar_Registro_Volumenes() { Cls_Rpt_Plan_Volumenes_Datos.Insertar_Registro_Volumenes(this); }
        public void Actualizar_Registro_Volumenes() { Cls_Rpt_Plan_Volumenes_Datos.Actualizar_Registro_Volumenes(this); }
        public DataTable Consultar_Tabla_Historicos_Volumenes() { return Cls_Rpt_Plan_Volumenes_Datos.Consultar_Tabla_Historicos_Volumenes(this); }
       

        #endregion
    }
}