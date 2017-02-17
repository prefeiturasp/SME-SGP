using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.ParametroResultado
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tpr_id (ID do tipo de resultado)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tpr_id
        {
            get
            {
                if (ViewState["VS_tpr_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tpr_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_tpr_id"] = value;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            // Seta delegates
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    // Inicializa componentes
                    UCCCursoCurriculo.CarregarPorSituacaoCurso(1);
                    MostrarTiposDisciplina(ddlTipoLancamento.SelectedValue == ((byte)EnumTipoLancamento.Disciplinas).ToString());

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_tpr_id = PreviousPage.Edit_tpr_id;
                        Carregar(VS_tpr_id);
                    }

                    btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de resultado.", UtilBO.TipoMensagem.Erro);
                }
            }

            Page.Form.DefaultFocus = UCCCursoCurriculo.ClientID_Combo;
            Page.Form.DefaultButton = btnSalvar.UniqueID;

        }

        #endregion Eventos Page Life Cycle

        #region Métodos

        /// <summary>
        /// Método para carregar um registro de tipo de resultado, a fim de atualizar suas informações.
        /// Recebe dados referente ao tipo de resultado para realizar busca.
        /// </summary>
        /// <param name="ifm_id">ID do informativo</param>
        public void Carregar(int tpr_id)
        {
            // Busca do informativo baseado no ID do informativo.
            ACA_TipoResultado entTpResultado = new ACA_TipoResultado { tpr_id = tpr_id };
            ACA_TipoResultadoBO.GetEntity(entTpResultado);

            DataTable dt = ACA_TipoResultadoCurriculoPeriodoBO.SelectBy_tpr_id(entTpResultado.tpr_id);

            int[] valorComboCurso = { Convert.ToInt32(dt.Rows[0]["cur_id"]), Convert.ToInt32(dt.Rows[0]["crr_id"]) };

            // Tipo de lancamento
            ddlTipoLancamento.SelectedValue = entTpResultado.tpr_tipoLancamento.ToString();

            //// Curso Curriculo
            UCCCursoCurriculo.Valor = valorComboCurso;
            UCCCursoCurriculo_IndexChanged();

            // Tipo de disciplina
            if (ddlTipoDisciplina.Visible && entTpResultado.tds_id > 0)
            {
                ddlTipoDisciplina.SelectedValue = entTpResultado.tds_id.ToString();
            }

            // Conceito final
            ddlConceitoFinal.SelectedValue = entTpResultado.tpr_resultado.ToString();

            // Nomenclatura
            txtNomenclatura.Text = entTpResultado.tpr_nomenclatura;

            // Series
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (ListItem item in cblPeriodos.Items)
                {
                    if (dt.Rows[i]["crp_id"].ToString() == item.Value)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Método para salvar um informativo.
        /// </summary>
        private void Salvar()
        {
            try
            {
                ACA_TipoResultado entTpResultado = new ACA_TipoResultado();

                entTpResultado.tpr_id = VS_tpr_id;
                entTpResultado.tpr_resultado = Convert.ToInt16(ddlConceitoFinal.SelectedValue);
                entTpResultado.tpr_nomenclatura = txtNomenclatura.Text;
                entTpResultado.tpr_tipoLancamento = Convert.ToInt16(ddlTipoLancamento.SelectedValue);
                entTpResultado.tds_id = ddlTipoDisciplina.Visible ? Convert.ToInt32(ddlTipoDisciplina.SelectedValue) : -1;
                entTpResultado.IsNew = VS_tpr_id < 0;

                IList<ACA_TipoResultadoCurriculoPeriodo> series = new List<ACA_TipoResultadoCurriculoPeriodo>();
                List<ListItem> selecionados = cblPeriodos.Items.Cast<ListItem>()
                                                .Where(li => li.Selected)
                                                .ToList();
                if (selecionados.Count > 0)
                {
                    foreach (ListItem item in selecionados)
                    {
                        ACA_TipoResultadoCurriculoPeriodo it = new ACA_TipoResultadoCurriculoPeriodo
                        {
                            cur_id = UCCCursoCurriculo.Valor[0],
                            crr_id = UCCCursoCurriculo.Valor[1],
                            crp_id = Convert.ToInt32(item.Value)
                        };
                        series.Add(it);
                    }
                }
                else
                    throw new ValidationException("Pelo menos uma série deve ser selecionada.");

                if (ACA_TipoResultadoBO.Save(entTpResultado, series))
                {
                    ApplicationWEB._GravaLogSistema(VS_tpr_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tpr_id: " + entTpResultado.tpr_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de resultado " + (VS_tpr_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Response.Redirect("~/Configuracao/ParametroResultado/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar tipo de resultado.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void MostrarTiposDisciplina(bool mostrar)
        {
            ddlTipoDisciplina.Visible = lblTipoDisciplina.Visible = mostrar;
            if (mostrar && ddlTipoDisciplina.Items.Count == 1)
            {
                // se tem curso/curriculo selecionado, carrego as disciplinas relacionadas
                if (UCCCursoCurriculo.Valor[0] != -1)
                {
                    ddlTipoDisciplina.Enabled = true;
                    DataTable dtDisciplinas = ACA_CurriculoDisciplinaBO.GetSelect_Disciplinas(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], false, 1, 1);
                    ddlTipoDisciplina.DataTextField = "tds_nome";
                    ddlTipoDisciplina.DataValueField = "tds_id";
                    ddlTipoDisciplina.DataSource = dtDisciplinas;
                    ddlTipoDisciplina.DataBind();
                }
                // senao, desabilito o combo para selecao de disciplina
                else
                {
                    ddlTipoDisciplina.Enabled = false;
                }
            }
        }

        #endregion

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ParametroResultado/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ParametroResultado/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void ddlTipoLancamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarTiposDisciplina(ddlTipoLancamento.SelectedValue == ((byte)EnumTipoLancamento.Disciplinas).ToString());
        }

        #endregion Eventos

        #region Delegates

        private void UCCCursoCurriculo_IndexChanged()
        {
            if (UCCCursoCurriculo.Valor[0] != -1)
            {
                cblPeriodos.Items.Clear();
                DataTable dt = ACA_CurriculoPeriodoBO.BuscaCurriculoPeriodoPorEntidadeCursoCurriculo(Ent_ID_UsuarioLogado, UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    ListItem item = new ListItem();
                    item.Text = row["crp_descricao"].ToString();
                    item.Value = row["crp_id"].ToString();
                    cblPeriodos.Items.Add(item);
                }
                pnlPeriodos.Visible = true;  
            }
            else
            {
                pnlPeriodos.Visible = false;
            }

            // Recarrega os tipos de disciplina
            ddlTipoDisciplina.Items.Clear();
            ddlTipoDisciplina.Items.Add(new ListItem(GetGlobalResourceObject("Configuracao", "ParametroResultado.Cadastro.ddlTipoDisciplina.valor0").ToString(), "-1"));
            MostrarTiposDisciplina(ddlTipoLancamento.SelectedValue == ((byte)EnumTipoLancamento.Disciplinas).ToString());
        }

        #endregion Delegates

    }
}