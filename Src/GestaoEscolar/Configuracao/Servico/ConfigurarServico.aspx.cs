using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using Quartz;
using MSTech.GestaoEscolar.Entities;
using System.Data;

namespace GestaoEscolar.Configuracao.Servico
{
    public partial class ConfigurarServico : MotherPageLogado
    {
        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(message))
                {
                    lblMessage.Text = message;
                }

                try
                {
                    UCComboServico1.Carregar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    fdsConfigurarServico.Visible = false;
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                UCFrequenciaServico1.ObrigatorioFrequencia = true;
                UCFrequenciaServico1.ObrigatorioHorario = true;

            }

            UCComboServico1.IndexChanged += UCComboServico1_IndexChanged;
        }

        #endregion

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void btnDispararAgora_Click(object sender, EventArgs e)
        {
            Disparar();
        }

        protected void chkDesativar_CheckedChanged(object sender, EventArgs e)
        {
            divCampos.Visible = !chkDesativar.Checked;
        }

        #endregion

        #region Delegates

        private void UCComboServico1_IndexChanged()
        {
            divDetalhes.Visible = divServico.Visible = UCComboServico1.Valor > 0;

            try
            {
                if (divServico.Visible)
                {
                    LimparCampos();
                    string expressao;

                    DataTable dt = SYS_ServicosBO.SelecionaProcedimentoPorNome(UCComboServico1.Texto);
                    string trigger = String.Format("Trigger_{0}", dt.Rows[0]["ser_nomeProcedimento"].ToString());

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ser_descricao"].ToString()))
                    {
                        _lblMensagem.Text = dt.Rows[0]["ser_descricao"].ToString();
                        _lblMensagem.Font.Bold = true;
                        divDetalhes.Visible = true;
                    }
                    else
                    {
                        _lblMensagem.Text = string.Empty;
                        divDetalhes.Visible = false;
                    }
                    
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
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Limpar os campos dos dados do serviço.
        /// </summary>
        private void LimparCampos()
        {
            UCFrequenciaServico1.TipoFrequencia = 0;
            UCFrequenciaServico1.DiaMesSelectedValue = "1";
            UCFrequenciaServico1.LimpaCheckboxList();
            UCFrequenciaServico1.LimpaRadioButtonList();
            UCFrequenciaServico1.Horario = string.Empty;
            UCFrequenciaServico1.LimpaRepeater();
            UCFrequenciaServico1.AtualizaDivs();
            chkDesativar.Checked = false;
            divCampos.Visible = true;
        }

        /// <summary>
        /// Método para salvar as configurações do serviço.
        /// </summary>
        private void Salvar()
        {
            try
            {
                SalvarTriggerQuartz(UCComboServico1.Texto, chkDesativar.Checked ? (byte)GestaoEscolarServicosBO.eServicoAtivo.Desabilitado : (byte)GestaoEscolarServicosBO.eServicoAtivo.Ativo);

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Serviço: " + UCComboServico1.Texto);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Configuração do serviço salva com sucesso.", UtilBO.TipoMensagem.Sucesso);

                Response.Redirect("ConfigurarServico.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a configuração do serviço.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para disparar agora o serviço selecionado
        /// </summary>
        private void Disparar()
        {
            try
            {
                SYS_Servicos servico = SYS_ServicosBO.GetEntity(new SYS_Servicos() { ser_id = Convert.ToInt16(UCComboServico1.Valor) });
                ApplicationWEB.SchedulerProvider.Scheduler.TriggerJob(new JobKey(servico.ser_nomeProcedimento));
                lblMessage.Text = UtilBO.GetErroMessage("Serviço disparado com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao disparar o serviço.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva as configurações do serviço na tabela do quartz (serviço do windows que executa os serviços de integração).
        /// </summary>
        /// <param name="srv_id">ID do serviço.</param>
        /// <param name="src_situacao">Situação do serviço.</param>
        private void SalvarTriggerQuartz(string ser_nome, byte ser_ativo)
        {
            const string groupName = "DEFAULT";
            string jobName = SYS_ServicosBO.SelecionaProcedimentoPorNome(ser_nome).Rows[0]["ser_nomeProcedimento"].ToString();
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

        #endregion
    }
}