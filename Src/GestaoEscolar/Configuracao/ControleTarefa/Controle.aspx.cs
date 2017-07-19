using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSQuartz.Core.SchedulerProviders;
using MSQuartz.Core;
using Quartz;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.Configuracao.ControleTarefa
{
    public partial class Controle : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// Constante para armazenar o nome Default do Grupo que os jobs fazem parte.
        /// </summary>
        private const string GroupName = "DEFAULT";

        /// <summary>
        /// Constante para armazenar o Index da célula com nome do Job.
        /// </summary>
        private const int CellJobName = 0;

        /// <summary>
        /// Constante para armazenar o Index da célula com nome da Trigger.
        /// </summary>
        private const int CellTriggerName = 0;

        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade para armazenar o nome do Job selecionado.
        /// </summary>
        private string VSJobName
        {
            get
            {
                return Convert.ToString(ViewState["VSJobName"]);
            }
            set
            {
                ViewState["VSJobName"] = value;
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Carrega os dados do Scheduler na página.
        /// </summary>
        private void LoadPage()
        {
            lblName.Text = "Nome: " + ApplicationWEB.SchedulerDataProvider.Data.SchedulerName;
            lblStatus.Text = "Status: " + ApplicationWEB.SchedulerDataProvider.Data.Status;
            lblRunningSince.Text = "Executando desde: " + ApplicationWEB.SchedulerDataProvider.Data.RunningSince.Value.ToLocalTime().ToString("G");
            lblTotalJobs.Text = "Total de Jobs: " + ApplicationWEB.SchedulerDataProvider.Data.JobsTotal;
            lblJobsExecuted.Text = "Total de Jobs em execução: " + ApplicationWEB.SchedulerDataProvider.Data.JobsExecuted;
            lblInstanceID.Text = "Id da instância: " + ApplicationWEB.SchedulerDataProvider.Data.InstanceId;
            lblIsRemote.Text = "Remoto: " + ApplicationWEB.SchedulerDataProvider.Data.IsRemote;
            lblSchedulerType.Text = "Scheduler type: " + ApplicationWEB.SchedulerDataProvider.Data.SchedulerType;

            this.gvJobs.DataSource = ApplicationWEB.SchedulerDataProvider.GetJobsOfGroup(GroupName);

            this.gvJobs.DataBind();

            //Carrega as triggers do job caso tenha sido selecionado algum Job.
            if (!String.IsNullOrEmpty(this.VSJobName))
            {
                this.txtJobName.Text = this.VSJobName;
                this.gvTriggers.DataSource = ApplicationWEB.SchedulerDataProvider.GetTriggersOfJob(GroupName, this.VSJobName);
                this.gvTriggers.DataBind();
                this.divTriggers.Visible = true;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                LoadPage();
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }
                string script = @"function jsControle() {
                        createDialog('#divNewTrigger', 600, 0);                       
                    }
                    arrFNC.push(jsControle);
                    arrFNCSys.push(jsControle);";

                if (!this.Page.ClientScript.IsStartupScriptRegistered(this.Page.GetType(), "key1"))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "key1", script, true);
                }

            }
            catch (Exception ex)
            {

                this.divPainel.Visible = false;
                _lblMessage.Visible = true;
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar informações", UtilBO.TipoMensagem.Erro);
                MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(ex);

            }
        }

        protected void Page_Load(object source, EventArgs e)
        {
            if (__SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Grupo != null)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
                {
                    Response.Redirect("~/Index.aspx", false);
                }
            }
        }

        #endregion

        #region Eventos

        protected void gvJobs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewTrigger")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    this.VSJobName = this.gvJobs.Rows[index].Cells[CellJobName].Text;
                }
            }
            catch (Exception ex)
            {
                _lblMessage.Visible = true;
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar trigger!", UtilBO.TipoMensagem.Erro);
                MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(ex);
            }

        }

        protected void gvTriggers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string triggerName = this.gvTriggers.Rows[index].Cells[CellTriggerName].Text;
                string groupName = GroupName;

                if (e.CommandName == "PauseTrigger")
                {
                    ApplicationWEB.SchedulerDataProvider.PauseTrigger(new TriggerKey(triggerName, groupName));
                }
                else if (e.CommandName == "ResumeTrigger")
                {
                    ApplicationWEB.SchedulerDataProvider.ResumeTrigger(new TriggerKey(triggerName, groupName));
                }
                else if (e.CommandName == "DeleteTrigger")
                {
                    ApplicationWEB.SchedulerDataProvider.DeleteTrigger(new TriggerKey(triggerName, groupName));
                }
            }
            catch (Exception ex)
            {
                _lblMessage.Visible = true;
                _lblMessage.Text = UtilBO.GetErroMessage("Erro na operação " + e.CommandName + " trigger!", UtilBO.TipoMensagem.Erro);
                MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(ex);
            }
        }

        protected void btnAddTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string groupName = GroupName;
                    string jobName = this.VSJobName;
                    string cronExpression = this.txtAgendamento.Text.Trim();
                    string triggerName = this.txtTrigger.Text.Trim();

                    var jobKey = new JobKey(jobName, groupName);
                    ApplicationWEB.SchedulerDataProvider.ScheduleCronTriggerForJob(jobKey, triggerName, cronExpression);
                    _lblMessage.Visible = true;
                    _lblMessage.Text = UtilBO.GetErroMessage("Trigger salva com sucesso!", UtilBO.TipoMensagem.Sucesso);

                    this.txtAgendamento.Text =
                    this.txtTrigger.Text = "";
                }
            }
            catch (Exception ex)
            {
                _lblMessage.Visible = true;
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao salvar trigger!", UtilBO.TipoMensagem.Erro);
                MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion

    }
}
