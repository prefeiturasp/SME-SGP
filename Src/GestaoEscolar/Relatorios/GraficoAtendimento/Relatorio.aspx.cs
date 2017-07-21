namespace GestaoEscolar.Relatorios.GraficoAtendimento
{
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Security.Cryptography;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Relatorio : MotherPageLogado
    {
        public int gra_id
        {
            get { return Convert.ToInt32(ViewState["gra_id"] ?? "-1"); }
            set { ViewState["gra_id"] = value; }
        }

        public int esc_id
        {
            get { return Convert.ToInt32(ViewState["esc_id"] ?? "-1"); }
            set { ViewState["esc_id"] = value; }
        }

        public int uni_id
        {
            get { return Convert.ToInt32(ViewState["uni_id"] ?? "-1"); }
            set { ViewState["uni_id"] = value; }
        }

        public int cur_id
        {
            get { return Convert.ToInt32(ViewState["cur_id"] ?? "-1"); }
            set { ViewState["cur_id"] = value; }
        }
        public int crr_id
        {
            get { return Convert.ToInt32(ViewState["crr_id"] ?? "-1"); }
            set { ViewState["crr_id"] = value; }
        }

        public int crp_id
        {
            get { return Convert.ToInt32(ViewState["crp_id"] ?? "-1"); }
            set { ViewState["crp_id"] = value; }
        }

        protected string Usuario
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.Usuario.usu_login;
            }
        }

        protected string Entidade
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString();
            }
        }

        protected string Grupo
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString();
            }
        }

        protected string Token
        {
            get { return ViewState["Token"] as string ?? string.Empty; }
            set { ViewState["Token"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    PreviousPage.SalvarBusca();
                    string gra_titulo = PreviousPage.gra_titulo;
                    string cabecalho = string.Empty;
                    if (!string.IsNullOrEmpty(PreviousPage.uadSuperior_nome))
                    {
                        cabecalho += string.Format("<b>Diretoria Regional de Educação: </b>{0}<br />", PreviousPage.uadSuperior_nome);
                    }

                    cabecalho += string.Format("<b>Escola: </b>{0}", PreviousPage.esc_nome);

                    if (!string.IsNullOrEmpty(PreviousPage.cur_nome))
                    {
                        cabecalho += string.Format("<br /><b>Curso: </b>{0}", PreviousPage.cur_nome);
                    }

                    if (!string.IsNullOrEmpty(PreviousPage.crp_descricao))
                    {
                        cabecalho += string.Format("<br /><b>Período do curso: </b>{0}", PreviousPage.crp_descricao);
                    }

                    gra_id = PreviousPage.gra_id;
                    esc_id = PreviousPage.esc_id;
                    uni_id = PreviousPage.uni_id;
                    cur_id = PreviousPage.cur_id;
                    crr_id = PreviousPage.crr_id;
                    crp_id = PreviousPage.crp_id;

                    UCGraficoAtendimento.Carregar(gra_id, esc_id, uni_id, cur_id, crr_id, crp_id, gra_titulo, cabecalho, "/Relatorios/GraficoAtendimento/Busca.aspx");
                }
            }
        }
    }
}