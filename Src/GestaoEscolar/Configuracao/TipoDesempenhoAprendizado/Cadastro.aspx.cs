using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.TipoDesempenhoAprendizado
{
    public partial class Cadastro : MotherPageLogado
    {

        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de ifm_id (ID do informativo)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tda_id
        {
            get
            {
                if (ViewState["VS_tda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tda_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_tda_id"] = value;
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
            UCCCurriculoPeriodo.IndexChanged += UCCCurriculoPeriodo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    // Inicializa componentes
                    UCCCalendario.Carregar();
                    UCComboTipoDisciplina.CarregarTipoDisciplina();
                    UCComboTipoDisciplina._Combo.SelectedValue = "-1";

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_tda_id = PreviousPage.Edit_tda_id;
                        Carregar(PreviousPage.Edit_tda_id);
                    }

                    Page.Form.DefaultFocus = UCCCalendario.ClientID;
                    Page.Form.DefaultButton = btnSalvar.UniqueID;

                    btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos Page Life Cycle

        #region Métodos

        /// <summary>
        /// Método para carregar um registro de tipo de desempenho, a fim de atualizar suas informações.
        /// Recebe dados referente ao tipo de desempenho para realizar busca.
        /// </summary>
        /// <param name="tda_id">ID do tipo de desempenho</param>
        public void Carregar(int tda_id)
        {
            try
            {
                // Armazena valor ID do informativo a ser alterada.
                VS_tda_id = tda_id;

                // Busca do informativo baseado no ID do informativo.
                ACA_TipoDesempenhoAprendizado entTipoDesempenho = new ACA_TipoDesempenhoAprendizado { tda_id = tda_id };
                ACA_TipoDesempenhoAprendizadoBO.GetEntity(entTipoDesempenho);

                // Tras os campos preenchidos
                // Valores

                int[] valorComboCurso = { entTipoDesempenho.cur_id, entTipoDesempenho.crr_id };
                int[] valorComboCurriculo = { entTipoDesempenho.cur_id, entTipoDesempenho.crr_id, entTipoDesempenho.crp_id };


                // Calendario
                UCCCalendario.Carregar();
                UCCCalendario.Valor = entTipoDesempenho.cal_id;
                UCCCalendario.PermiteEditar = true;

                // Curso Curriculo
                if (UCCCalendario.PermiteEditar)
                {
                    UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(-1, -1, UCCCalendario.Valor, 1);
                    UCCCursoCurriculo.Valor = valorComboCurso;
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                // Curriculo Periodo
                if (UCCCursoCurriculo.PermiteEditar)
                {
                    UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                    UCCCurriculoPeriodo.Valor = valorComboCurriculo;
                    UCCCurriculoPeriodo.PermiteEditar = true;
                }

                if (UCCCurriculoPeriodo.PermiteEditar)
                {
                    // Tipo disciplina
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodo(UCCCurriculoPeriodo.Valor[0], UCCCurriculoPeriodo.Valor[1], UCCCurriculoPeriodo.Valor[2]);
                    UCComboTipoDisciplina.Valor = entTipoDesempenho.tds_id;
                    UCComboTipoDisciplina.PermiteEditar = true;
                }

                // Descricao
                txtDescricao.Text = entTipoDesempenho.tda_descricao;

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar um informativo.
        /// </summary>
        private void Salvar()
        {
            bool isGravouDisciplina = false;
            try
            {
                if (UCComboTipoDisciplina.Valor == 0)  // opcao Todos
                {
                    foreach(ListItem lst in UCComboTipoDisciplina._Combo.Items)
                    {
                        if (Convert.ToInt32(lst.Value) > 0)  // diferente da opcao "todos = 0" gravo as disciplinas validas
                        {
                            ACA_TipoDesempenhoAprendizado entTipoDesempenho = new ACA_TipoDesempenhoAprendizado();

                            MovimentaCampos(entTipoDesempenho, Convert.ToInt32(lst.Value));

                            if (ACA_TipoDesempenhoAprendizadoBO.Save(entTipoDesempenho))
                            {
                                ApplicationWEB._GravaLogSistema(VS_tda_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tda_id: " + entTipoDesempenho.tda_id);
                                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + (VS_tda_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);
                            }
                            isGravouDisciplina = true;
                        }
                    }
                    if (isGravouDisciplina)
                    {
                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {   // caso o combo não contenha nenhuma disciplina, mostro mensagem
                        throw new ValidationException("Nenhuma disciplina foi encontrada para gravar o tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".");
                    }
                }
                else
                {   // nesse caso gravo apenas a disciplina que foi selecionada no combo.
                    ACA_TipoDesempenhoAprendizado entTipoDesempenho = new ACA_TipoDesempenhoAprendizado();

                    MovimentaCampos(entTipoDesempenho, UCComboTipoDisciplina.Valor);

                    if (ACA_TipoDesempenhoAprendizadoBO.Save(entTipoDesempenho))
                    {
                        ApplicationWEB._GravaLogSistema(VS_tda_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tda_id: " + entTipoDesempenho.tda_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + (VS_tda_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Movimenta os valores dos combos para a entidade
        /// </summary>
        /// <param name="entTipoDesempenho"></param>
        /// <param name="id_Disciplina"></param>
        private void MovimentaCampos(ACA_TipoDesempenhoAprendizado entTipoDesempenho, int id_Disciplina)
        {
            entTipoDesempenho.tda_id = VS_tda_id;

            entTipoDesempenho.cur_id = UCCCursoCurriculo.Valor[0];
            entTipoDesempenho.crr_id = UCCCursoCurriculo.Valor[1];
            entTipoDesempenho.crp_id = UCCCurriculoPeriodo.Valor[2];
            entTipoDesempenho.cal_id = UCCCalendario.Valor;
            entTipoDesempenho.tda_descricao = txtDescricao.Text;
            entTipoDesempenho.IsNew = VS_tda_id < 0;
            entTipoDesempenho.tds_id = id_Disciplina;
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
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos

        #region Delegates

        protected void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCCursoCurriculo.PermiteEditar = false;

                UCCCursoCurriculo.Valor = new int[2] { -1, -1 };

                if (UCCCalendario.Valor > 0)
                {
                    UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(-1, -1, UCCCalendario.Valor, 1);

                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do calendario.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCCCurriculoPeriodo.Valor = new[] { -1, -1 };

                UCCCurriculoPeriodo.PermiteEditar = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
                {
                    UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);

                    UCCCurriculoPeriodo.SetarFoco();
                    UCCCurriculoPeriodo.PermiteEditar = true;

                }

                UCCCurriculoPeriodo_IndexChanged();
            }
            catch (Exception ex)
            {

                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do(a) " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCurriculoPeriodo_IndexChanged()
        {
            try
            {
                UCComboTipoDisciplina.Valor = -1;
                UCComboTipoDisciplina.PermiteEditar = false;

                if (UCCCurriculoPeriodo.Valor[0] > 0)
                {
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodo(UCCCurriculoPeriodo.Valor[0], UCCCurriculoPeriodo.Valor[1], UCCCurriculoPeriodo.Valor[2]);
                    UCComboTipoDisciplina.SetarFoco();
                    UCComboTipoDisciplina.PermiteEditar = true;

                    // Se for inserir um registro novo, traz a opção todos, senão mostra o selecione
                    if (UCComboTipoDisciplina._Combo.Items.Count > 1 && VS_tda_id <= 0)
                    {   // somente adiciono essa opcao caso exista pelo menos uma disciplina cadastrada.
                        UCComboTipoDisciplina._Combo.Items.RemoveAt(0); // remove o selecione
                        UCComboTipoDisciplina._Combo.Items.Insert(0, new ListItem("Todos", "0", true));
                    }
                }
                //UCCCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do(a) " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_PERIODO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

    }
}