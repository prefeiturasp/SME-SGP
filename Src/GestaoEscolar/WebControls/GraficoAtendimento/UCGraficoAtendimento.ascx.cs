using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.GraficoAtendimento
{
    public partial class UCGraficoAtendimento : System.Web.UI.UserControl
    {
        public void Carregar(int gra_id, int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, string gra_titulo, string cabecalho, string urlRetorno)
        {
            Session["gra_id_GraficoAtendimento"] = gra_id;
            Session["esc_id_GraficoAtendimento"] = esc_id;
            Session["uni_id_GraficoAtendimento"] = uni_id;
            Session["cur_id_GraficoAtendimento"] = cur_id;
            Session["crr_id_GraficoAtendimento"] = crr_id;
            Session["crp_id_GraficoAtendimento"] = crp_id;
            Session["gra_titulo_GraficoAtendimento"] = gra_titulo;
            Session["cabecalho_GraficoAtendimento"] = cabecalho;
        }
    }
}