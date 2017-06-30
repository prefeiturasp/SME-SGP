using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.Alertas
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade que armazena valor do id do alerta.
        /// </summary>
        private short VS_cfa_id
        {
            get
            {
                if (ViewState["VS_cfa_id"] != null)
                    return Convert.ToInt16(ViewState["VS_cfa_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_cfa_id"] = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        private void Carregar(short cfa_id)
        {
            UCFrequenciaServico1.LimparCampos();
            VS_cfa_id = cfa_id;

            CFG_Alerta alerta = CFG_AlertaBO.GetEntity(new CFG_Alerta { cfa_id = VS_cfa_id });
            txtNome.Text = alerta.cfa_nome;
            txtPeriodoAnalise.Text = alerta.cfa_periodoAnalise.ToString();
            txtPeriodoValidade.Text = alerta.cfa_periodoValidade.ToString();
            txtAssunto.Text = alerta.cfa_assunto;

            // Carrega os grupos
            grvGrupos.DataSource = CFG_AlertaGrupoBO.SelecionarGruposPorAlerta(VS_cfa_id, __SessionWEB.__UsuarioWEB.Grupo.sis_id);
            grvGrupos.DataBind();

            // Carrega configuração serviço
            string expressao;
            string trigger = string.Format("Trigger_{0}", alerta.cfa_nomeProcedimento);
            if (GestaoEscolarServicosBO.SelecionaExpressaoPorTrigger(trigger, out expressao))
            {
                UCFrequenciaServico1.ConfigurarFrequencia(expressao);
                chkDesativar.Visible = true;
            }
            else
            {
                chkDesativar.Visible = false;
            }
        }

        private void Salvar()
        {
            bool sucessoSalvar = true;
            bool sucessoAgendar = true;

            CFG_Alerta alerta = CFG_AlertaBO.GetEntity(new CFG_Alerta { cfa_id = VS_cfa_id });
            alerta.cfa_nome = txtNome.Text;
            alerta.cfa_periodoAnalise = string.IsNullOrEmpty(txtPeriodoAnalise.Text) ? 0 : Convert.ToInt32(txtPeriodoAnalise.Text);
            alerta.cfa_periodoValidade = string.IsNullOrEmpty(txtPeriodoValidade.Text) ? 0 : Convert.ToInt32(txtPeriodoValidade.Text);
            alerta.cfa_assunto = txtAssunto.Text;

            sucessoSalvar = CFG_AlertaBO.Salvar(alerta, CarregaGruposSelecionados());

            if (sucessoSalvar)
            {
                try
                {
                    SalvarTriggerQuartz(alerta.cfa_nomeProcedimento, chkDesativar.Checked ? (byte)GestaoEscolarServicosBO.eServicoAtivo.Desabilitado : (byte)GestaoEscolarServicosBO.eServicoAtivo.Ativo);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    sucessoAgendar = false;
                }
            }

            if (sucessoSalvar && sucessoAgendar)
            {
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("GestaoEscolar.Configuracao.Alertas.Cadastro", "mensagemSucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(sucessoSalvar ? "Erro ao tentar agendar o alerta." : "Erro ao tentar salvar alerta.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os grupos
        /// </summary>
        private List<CFG_AlertaGrupo> CarregaGruposSelecionados()
        {
            List<CFG_AlertaGrupo> lstGrupo = new List<CFG_AlertaGrupo>();
            foreach (GridViewRow item in grvGrupos.Rows)
            {
                CheckBox chkEnvio = (CheckBox)item.FindControl("chkEnvio");
                if (chkEnvio != null)
                {
                    if (!chkEnvio.Checked)
                        continue;

                    lstGrupo.Add(new CFG_AlertaGrupo
                    {
                        cfa_id = VS_cfa_id
                        , gru_id = new Guid(grvGrupos.DataKeys[item.RowIndex]["gru_id"].ToString())
                    });
                }
            }
            return lstGrupo;
        }

        /// <summary>
        /// Salva as configurações do serviço na tabela do quartz (serviço do windows que executa os serviços de integração).
        /// </summary>
        /// <param name="srv_id">ID do serviço.</param>
        /// <param name="src_situacao">Situação do serviço.</param>
        private void SalvarTriggerQuartz(string nomeProcedimento, byte ser_ativo)
        {
            const string groupName = "DEFAULT";
            string jobName = nomeProcedimento;
            string cronExpression = UCFrequenciaServico1.GeraCronExpression();
            string triggerName = "Trigger_" + jobName;

            var jobKey = new JobKey(jobName, groupName);

            // Verifica se a trigger já está criada 
            if (ApplicationWEB.SchedulerProvider.Scheduler.CheckExists(jobKey))
            {
                // Caso a trigger já exista, deleta para cria-lá com as configurações novas
                ApplicationWEB.SchedulerDataProvider.DeleteTrigger(new TriggerKey(triggerName, groupName));
            }

            // Verifica se o serviço está habilitado
            if (ser_ativo == (byte)GestaoEscolarServicosBO.eServicoAtivo.Ativo)
            {
                // Cria a trigger com as configurações novas
                ApplicationWEB.SchedulerDataProvider.ScheduleCronTriggerForJob(jobKey, triggerName, cronExpression);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAlerta.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        Carregar(PreviousPage.EditItem);

                        Page.Form.DefaultFocus = txtNome.ClientID;
                        btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                        {
                            HabilitaControles(fdsAlertas.Controls, false);
                        }
                    }
                    else
                    {
                        btnCancelar_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar alerta.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void chkDesativar_CheckedChanged(object sender, EventArgs e)
        {
            UCFrequenciaServico1.Visible = !chkDesativar.Checked;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Salvar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar alerta.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/Alertas/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos
    }
}