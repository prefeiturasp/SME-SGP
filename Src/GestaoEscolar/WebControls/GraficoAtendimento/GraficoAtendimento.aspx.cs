

namespace GestaoEscolar.WebControls.GraficoAtendimento
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

    public partial class GraficoAtendimento : MotherPageLogado
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

        protected string URL_Retorno
        {
            get { return ViewState["URL_Retorno"] as string ?? string.Empty; }
            set { ViewState["URL_Retorno"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gra_id = Convert.ToInt32(Session["gra_id_GraficoAtendimento"] ?? "-1");
                esc_id = Convert.ToInt32(Session["esc_id_GraficoAtendimento"] ?? "-1");
                uni_id = Convert.ToInt32(Session["uni_id_GraficoAtendimento"] ?? "-1");
                cur_id = Convert.ToInt32(Session["cur_id_GraficoAtendimento"] ?? "-1");
                crr_id = Convert.ToInt32(Session["crr_id_GraficoAtendimento"] ?? "-1");
                crp_id = Convert.ToInt32(Session["crp_id_GraficoAtendimento"] ?? "-1");

                pnlGrafico.GroupingText = (Session["gra_titulo_GraficoAtendimento"] ?? "Gráfico de atendimento").ToString();
                lblNomeGrafico.Text = pnlGrafico.GroupingText;
                lblCabecalho.Text = (Session["cabecalho_GraficoAtendimento"] ?? string.Empty).ToString();

                URL_Retorno = (Session["URL_Retorno_GraficoAtendimento"] ?? "/Relatorios/GraficoAtendimento/Busca.aspx").ToString();

                Session.Remove("gra_id_GraficoAtendimento");
                Session.Remove("esc_id_GraficoAtendimento");
                Session.Remove("uni_id_GraficoAtendimento");
                Session.Remove("cur_id_GraficoAtendimento");
                Session.Remove("crr_id_GraficoAtendimento");
                Session.Remove("crp_id_GraficoAtendimento");
                Session.Remove("gra_titulo_GraficoAtendimento");
                Session.Remove("cabecalho_GraficoAtendimento");
                Session.Remove("URL_Retorno_GraficoAtendimento");
            }

            Token = CreateToken(Usuario, Entidade, Grupo);
        }

        [WebMethod]
        public static string CreateToken(string usuario, string entidade, string grupo)
        {
            var jwtKey = System.Configuration.ConfigurationManager.AppSettings["jwtKey"];
            SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
            var currentTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var expTime = DateTime.UtcNow.AddMinutes(30).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Login", usuario);
            dic.Add("Entity", entidade);
            dic.Add("Group", grupo);
            dic.Add("iat", currentTime);
            dic.Add("exp", expTime);
            return JWT.JsonWebToken.Encode(dic, sa.Decrypt(jwtKey), JWT.JwtHashAlgorithm.HS256);
        }
    }
}