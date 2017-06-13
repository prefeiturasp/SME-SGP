namespace GestaoEscolar.WebControls.RelatorioPedagogico
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Security.Cryptography;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class RelatorioPedagogico : MotherPageLogado
    {
        public long alu_id
        {
            get { return Convert.ToInt64(ViewState["alu_id"] ?? "-1"); }
            set { ViewState["alu_id"] = value; }
        }

        public int ano
        {
            get { return Convert.ToInt32(ViewState["ano"] ?? "-1"); }
            set { ViewState["ano"] = value; }
        }

        public int mtu_id
        {
            get { return Convert.ToInt32(ViewState["mtu_id"] ?? "-1"); }
            set { ViewState["mtu_id"] = value; }
        }

        public int tpc_id
        {
            get { return Convert.ToInt32(ViewState["tpc_id"] ?? "-1"); }
            set { ViewState["tpc_id"] = value; }
        }

        public string URLRetorno
        {
            get { return ViewState["URLRetorno"].ToString(); }
            set { ViewState["URLRetorno"] = value; }
        }

        protected override void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);
            Page.Theme = "IntranetSMEBootStrap";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery-2.0.3.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/bootstrap/bootstrap.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/scrolling.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Charts/Chart.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/angular-charts/angular-chart.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/angular-filter/angular-filter.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/angular.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/module.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/Boletim.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/calendario.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/sondagem.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/anotacao.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/justificativaFalta.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/movimentacao.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/matriculaTurma.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/conselhoClasse.controller.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.mCustomScrollbar.concat.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/angular-directives/JQmCustomScrollbar.js"));
            }

            if (!IsPostBack)
            {
                if (Session["alu_id"] != null)
                {
                    alu_id = Convert.ToInt32(Session["alu_id"]);
                    Session.Remove("alu_id");
                    if (Session["URLRetorno"] != null)
                    {
                        URLRetorno = Session["URLRetorno"].ToString();
                        Session.Remove("URLRetorno");
                    }
                    else
                    {
                        URLRetorno = Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/Aluno/Busca.aspx");
                    }

                    using (DataTable dt = MTR_MatriculaTurmaBO.GetSelectAnoMatricula(alu_id))
                    {
                        ano = dt.Select().Select(p => Convert.ToInt32(p["cal_ano"])).Max();
                        mtu_id = dt.Select(string.Format("cal_ano = {0}", ano)).Select(p => Convert.ToInt32(p["mtu_id"])).First();
                        tpc_id = dt.Select(string.Format("cal_ano = {0}", ano)).Select(p => Convert.ToInt32(p["tpc_id"])).First();
                    }

                    Token = CreateToken(Usuario, Entidade, Grupo);
                }
            }
        }
    }
}