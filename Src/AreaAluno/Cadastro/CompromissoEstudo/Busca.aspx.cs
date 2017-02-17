using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace AreaAluno.Cadastro.CompromissoEstudo
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        public int EditItem
        {
            get
            {
                return Convert.ToInt32(grvCompromissoEstudo.DataKeys[grvCompromissoEstudo.EditIndex].Values["cpe_id"]);
            }
        }

        public int VSAnoLetivo
        {
            get
            {
                return Convert.ToInt32((ViewState["VSAnoLetivo"] ?? (ViewState["VSAnoLetivo"] = ACA_CalendarioAnualBO.SelecionaAnoLetivoCorrente(__SessionWEB.__UsuarioWEB.Usuario.ent_id))));
            }
        }

        #endregion Propriedades

        #region Constantes

        private const int columnExcluir = 3;

        #endregion
        
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (!IsPostBack)
            {
                Int64 alu_id = __SessionWEB.__UsuarioWEB.alu_id;

                // somente terá acesso se o flag exibirBoletim do cadastro de "Tipo de ciclo" estiver marcado
                if (!ACA_TipoCicloBO.VerificaSeExibeCompromissoAluno(alu_id))
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Este usuário não tem permissão de acesso a esta página.", UtilBO.TipoMensagem.Alerta);
                    fdsCompromissoEstudo.Visible = false;
                    return;
                }

                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMensagem.Text = message;

                UCComboAnosLetivos.CarregarComboAnosLetivos(__SessionWEB.__UsuarioWEB.alu_id, 0);

                UCComboAnosLetivos.Ano = VSAnoLetivo;
                AnosLetivos_SelectedIndexChanged();
            }

            UCComboAnosLetivos.IndexChanged += AnosLetivos_SelectedIndexChanged;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            if (UCComboAnosLetivos.Ano != VSAnoLetivo)
            {
                lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.SalvarApenasAnoCorrente"), UtilBO.TipoMensagem.Alerta);
                return;
            }
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void grvCompromissoEstudo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();

                grvCompromissoEstudo.Columns[columnExcluir].Visible = UCComboAnosLetivos.Ano == VSAnoLetivo;

                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                    btnAlterar.Visible = UCComboAnosLetivos.Ano == VSAnoLetivo;

                Label lblBimestre = (Label)e.Row.FindControl("lblBimestre");
                if (lblBimestre != null)
                    lblBimestre.Visible = UCComboAnosLetivos.Ano != VSAnoLetivo && VSAnoLetivo > 0;
            }
        }

        protected void grvCompromissoEstudo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    if (UCComboAnosLetivos.Ano != VSAnoLetivo)
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.SalvarApenasAnoCorrente"), UtilBO.TipoMensagem.Alerta);
                        return;
                    }

                    int index = int.Parse(e.CommandArgument.ToString());
                    Int64 alu_id = __SessionWEB.__UsuarioWEB.alu_id;

                    DataTable dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);
                    if (dtCurriculo.Rows.Count == 0)
                    {
                        throw new ValidationException("Aluno não possui dados para a Área do Aluno.");
                    }

                    Int64 tur_id = Convert.ToInt64(dtCurriculo.Rows[0]["tur_id"]);
                    int cal_id = Convert.ToInt32(dtCurriculo.Rows[0]["cal_id"]);

                    ACA_CompromissoEstudo compromisso = new ACA_CompromissoEstudo
                    {
                        alu_id = alu_id,
                        cpe_id = Convert.ToInt32(grvCompromissoEstudo.DataKeys[index].Value)
                    };
                    ACA_CompromissoEstudoBO.GetEntity(compromisso);

                    List<Cache_EventosEfetivacaoTodos> listEventosCalendario = ACA_EventoBO.Select_EventoEfetivacaoTodos
                        (cal_id, tur_id, compromisso.tpc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

                    if (!listEventosCalendario.Any())
                    {
                        throw new ValidationException("Não é possível excluir, pois o bimestre já está fechado.");
                    }

                    compromisso.cpe_situacao = 3;
                    compromisso.cpe_dataAlteracao = DateTime.Now;

                    if (ACA_CompromissoEstudoBO.Save(compromisso))
                    {
                        CarregarCompromissoEstudoAlunoAnoLetivo(alu_id, UCComboAnosLetivos.Ano);

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "cpe_id: " + compromisso.cpe_id);
                        lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Busca.ExcluirSucesso"), UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Busca.ExcluirErro"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvCompromissoEstudo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
        }

        protected void AnosLetivos_SelectedIndexChanged()
        {
            try
            {
                btnNovo.Visible = UCComboAnosLetivos.Ano == VSAnoLetivo;

                Int64 alu_id = __SessionWEB.__UsuarioWEB.alu_id;

                CarregarCompromissoEstudoAlunoAnoLetivo(alu_id, UCComboAnosLetivos.Ano);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir o Boletim Online do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

        #region Metodos

        /// <summary>
        /// Carrega o compromisso pelo valor selecionado no combo de ano letivo.
        /// </summary>
        private void CarregarCompromissoEstudoAlunoAnoLetivo(long alu_id, int ano)
        {
            grvCompromissoEstudo.DataSource = ACA_CompromissoEstudoBO.GetSelectCompromissoAlunoBy_alu_id(alu_id, ano);
            grvCompromissoEstudo.DataBind();
        }
       
        #endregion Metodos

    }
}