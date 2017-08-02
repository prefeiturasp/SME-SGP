using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;
using System.Linq;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using System.Web;
using MSTech.Security.Cryptography;

namespace AreaAluno.WebControls.BoletimCompletoAluno
{
    public partial class UCBoletimAngular : MotherUserControl
    {
        protected string alu_ids
        {
            get { return ViewState["alu_ids"] as string ?? string.Empty; }
            set { ViewState["alu_ids"] = value; }
        }
        protected string mtu_ids
        {
            get { return ViewState["mtu_ids"] as string ?? string.Empty; }
            set { ViewState["mtu_ids"] = value; }
        }
        public string VS_nomeBoletim
        {
            get { return ViewState["VS_nomeBoletim"] as string ?? string.Empty; }
            set { ViewState["VS_nomeBoletim"] = value; }
        }
        protected bool infantil
        {
            get { return ViewState["infantil"] == null ? false : (bool)ViewState["infantil"]; }
            set { ViewState["infantil"] = value; }
        }
        protected int tpc_id { get; set; }

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
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/angular.js"));
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/module.js"));
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/Boletim.controller.js"));
            }

            if (!IsPostBack)
            {
                IDictionary<string, ICFG_Configuracao> configuracao;
                CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                if (configuracao.ContainsKey("AppURLAreaAlunoInfantil") && !string.IsNullOrEmpty(configuracao["AppURLAreaAlunoInfantil"].cfg_valor))
                {
                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    string configInfantil = configuracao["AppURLAreaAlunoInfantil"].cfg_valor;

                    infantil = url.Contains(configInfantil);
                    if (infantil)
                        VS_nomeBoletim = (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "MenuBoletimInfantil");
                    else
                        VS_nomeBoletim = ((string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "MenuBoletimOnline")).Replace(" Online", "");
                }

                var textoRodape = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETO").ToString();
                divRodape.Visible = !string.IsNullOrWhiteSpace(textoRodape);
                lblRodape.Text = textoRodape;

                var textoRodapeInfantil = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETOInfantil").ToString();
                divRodapeInfantil.Visible = !string.IsNullOrWhiteSpace(textoRodapeInfantil);
                lblRodapeInfantil.Text = textoRodapeInfantil;

                var textoRodapeFreqExterna = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_FREQUENCIA_EXTERNA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                lblFreqExterna.Text = !string.IsNullOrEmpty(textoRodapeFreqExterna) ? "* " + textoRodapeFreqExterna : textoRodapeFreqExterna;
                lblFreqExternaInfantil.Text = !string.IsNullOrEmpty(textoRodapeFreqExterna) ? "* " + textoRodapeFreqExterna : textoRodapeFreqExterna;
            }
        }

        public void CarregaBoletim(long aluId, int mtuId, int tpcId, bool mostrarPeriodos)
        {

            tpc_id = tpcId;
            alu_ids = aluId.ToString();
            mtu_ids = mtuId.ToString();

            Token = CreateToken(Usuario, Entidade, Grupo);

            if (aluId > 0)
            {
                var matriculas = ACA_AlunoBO.BuscarMatriculasPeriodos(new long[] { aluId }, new int[] { mtuId });

                int temp;
                if (tpc_id == 0 && int.TryParse(matriculas.Rows[0]["tpc_id"].ToString(), out temp))
                {
                    tpc_id = temp;
                }

                rptPeriodos.DataSource = matriculas;
                rptPeriodos.DataBind();

            }
        }

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