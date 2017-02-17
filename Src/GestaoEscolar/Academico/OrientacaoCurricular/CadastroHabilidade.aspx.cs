using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.OrientacaoCurricular;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.OrientacaoCurricular
{
    public partial class CadastroHabilidade : MotherPageLogado
    {
        #region Constantes

        protected const string validationGroup = "Orientacao";
        protected const int grvNiveisAprendizado = 2;
        protected const int grvReplicar = 3;
        protected const int grvEditarFilhos = 6;
        protected const int grvRelacionarHabilidades = 7;

        #endregion

        #region Delegates

        private void UCConfirmacaoOperacao1_ConfimaOperacao()
        {
            if ((ORC_OrientacaoCurriculaTipoMensagem)VS_ConfirmacaoPadrao == ORC_OrientacaoCurriculaTipoMensagem.DadosReplicarOrientacao)
            {
                ReplicarOrientacao(false);
            }
        }

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1.CancelSelect = false;

                if (UCComboCursoCurriculo1.Valor[0] > 0)
                    UCComboCurriculoPeriodo1.CarregarPorQtdeNivelOrientacaoCurricular(maxNivel, UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], VS_cal_id, VS_tds_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                UCComboCurriculoPeriodo1.PermiteEditar = UCComboCursoCurriculo1.Valor[0] > 0;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// ID do curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// ID do currículo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// ID do período.
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        private int VS_tds_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tds_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tds_id"] = value;
            }
        }

        /// <summary>
        /// ID do calendário.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// ID da matriz de habilidades
        /// </summary>
        private long VS_mat_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_mat_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_mat_id"] = value;
            }
        }

        /// <summary>
        /// Nome do curso
        /// </summary>
        private string VS_cur_nome
        {
            get
            {
                return (ViewState["VS_cur_nome"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_cur_nome"] = value;
            }
        }

        /// <summary>
        /// Nome do grupamento
        /// </summary>
        private string VS_crp_descricao
        {
            get
            {
                return (ViewState["VS_crp_descricao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_crp_descricao"] = value;
            }
        }

        /// <summary>
        /// Nome da disciplina
        /// </summary>
        private string VS_tds_nome
        {
            get
            {
                return (ViewState["VS_tds_nome"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_tds_nome"] = value;
            }
        }

        /// <summary>
        /// Descrição do calendário.
        /// </summary>
        private string VS_cal_descricao
        {
            get
            {
                return (ViewState["VS_cal_descricao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_cal_descricao"] = value;
            }
        }

        /// <summary>
        /// Níveis de orientação curricular.
        /// </summary>
        private List<sNivelOrientacaoCurricular> VS_dtNivel
        {
            get
            {
                return (List<sNivelOrientacaoCurricular>)
                       (ViewState["VS_dtNivel"] ??
                       (ViewState["VS_dtNivel"] = ORC_NivelBO.SelecionaPorCursoGrupamentoCalendarioTipoDisciplina
                                                  (
                                                        VS_cur_id,
                                                        VS_crr_id,
                                                        VS_crp_id,
                                                        VS_cal_id,
                                                        VS_tds_id,
                                                        VS_mat_id,
                                                        ApplicationWEB.AppMinutosCacheLongo
                                                  )));
            }
        }

        /// <summary>
        /// Ordem máxima dos níveis de orientação.
        /// </summary>
        private int maxNivel
        {
            get
            {
                return VS_dtNivel.Select(row => row.nvl_ordem).Max();
            }
        }

        /// <summary>
        /// Lista hierarquica das orientações curriculares.
        /// </summary>
        private List<ORC_OrientacaoCurricularBO.OrientacaoCurricular> VS_ltOrientacaoCurricular
        {
            get
            {
                return (List<ORC_OrientacaoCurricularBO.OrientacaoCurricular>)
                       (ViewState["VS_ltOrientacaoCurricular"] ??
                       (ViewState["VS_ltOrientacaoCurricular"] =
                        ORC_OrientacaoCurricularBO.SelecionaPorCalendarioPeriodoDisciplina
                        (
                            VS_cal_id,
                            VS_cur_id,
                            VS_crr_id,
                            VS_crp_id,
                            VS_tds_id,
                            VS_mat_id
                        )));
            }

            set
            {
                ViewState["VS_ltOrientacaoCurricular"] = value;
            }
        }

        /// <summary>
        /// ID da orientação curricular superior.
        /// </summary>
        private long VS_ocr_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_ocr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_ocr_id"] = value;
            }
        }

        /// <summary>
        /// Orientação curricular superior da tela atual.
        /// </summary>
        private ORC_OrientacaoCurricularBO.OrientacaoCurricular OrientacaoAtual
        {
            get
            {
                bool achou = false;
                ORC_OrientacaoCurricularBO.OrientacaoCurricular orientacao = BuscaOrientacao(VS_ocr_id, VS_ltOrientacaoCurricular, ref achou);

                if (achou && VS_ocr_id > 0)
                {
                    orientacao.ltNivelAprendizadoOrientacaoFilho = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectNivelAprendizadoByOcrIdSuperior(VS_ocr_id);
                }

                return achou ? orientacao : new ORC_OrientacaoCurricularBO.OrientacaoCurricular();
            }
        }

        private DataTable dtniveisAprendizado;

        /// <summary>
        /// Armazena níveis de aprendizagem.
        /// </summary>
        private DataTable dtNiveisAprendizado
        {
            get
            {
                return dtniveisAprendizado ??
                    (dtniveisAprendizado = ORC_NivelAprendizadoBO.GetSelectNivelAprendizadoByCursoPeriodo(VS_cur_id, VS_crr_id, VS_crp_id));
            }
        }

        private int? nivel;

        /// <summary>
        /// Nível da orientação curricular anterior usado no repeater.
        /// </summary>
        private int Nivel
        {
            get
            {
                return Convert.ToInt32(nivel ?? 0);
            }

            set
            {
                nivel = value;
            }
        }

        /// <summary>
        /// Posição do index que teve alteração, para poder percorrer e preencher os checkbox dos níveis de aprendizado.
        /// </summary>
        private int indexAlteracao
        {
            get
            {
                return Convert.ToInt32(nivel ?? -1);
            }

            set
            {
                nivel = value;
            }
        }

        /// <summary>
        /// ID da orientação curricular que será replicada.
        /// </summary>
        private long VS_ocr_id_replicada
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_ocr_id_replicada"] ?? "-1");
            }

            set
            {
                ViewState["VS_ocr_id_replicada"] = value;
            }
        }

        /// <summary>
        /// Indica qual o método que chamou a confirmação padrão
        /// 1-Replicar orientação curricular e sobrepor a existente
        /// </summary>
        public byte VS_ConfirmacaoPadrao
        {
            get
            {
                if (ViewState["VS_ConfirmacaoPadrao"] != null)
                {
                    return Convert.ToByte(ViewState["VS_ConfirmacaoPadrao"]);
                }

                return 0;
            }

            set
            {
                ViewState["VS_ConfirmacaoPadrao"] = value;
            }
        }

        /// <summary>
        /// ID da orientação curricular que será replicada.
        /// </summary>
        private long VS_ocr_id_relacionamento
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_ocr_id_relacionamento"] ?? "-1");
            }

            set
            {
                ViewState["VS_ocr_id_relacionamento"] = value;
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference("~/Includes/jquery.treeview.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroHabilidade.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsLoader.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_cur_id = PreviousPage.Edit_Cur_id;
                        VS_crr_id = PreviousPage.Edit_Crr_id;
                        VS_crp_id = PreviousPage.Edit_Crp_id;
                        VS_tds_id = PreviousPage.Edit_Tds_id;
                        VS_cal_id = PreviousPage.Edit_Cal_id;
                        VS_cur_nome = PreviousPage.Edit_Curso;
                        VS_crp_descricao = PreviousPage.Edit_Grupamento;
                        VS_tds_nome = PreviousPage.Edit_Disciplina;
                        VS_cal_descricao = PreviousPage.Edit_Calendario;
                        VS_mat_id = PreviousPage.Edit_Mat_id;
                    }

                    string msg = string.Empty;
                    if (VS_dtNivel.Count() <= 0)
                    {
                        msg = "Verifique se os níveis de orientação curricular foram cadastrados.";
                    }
                    if (dtNiveisAprendizado.Rows.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(msg))
                            msg += "<br/>";
                        msg += "Verifique se os níveis de aprendizagem foram cadastrados.";
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
                        RedirecionarPagina("~/Academico/OrientacaoCurricular/Busca.aspx");
                    }

                    UCConfirmacaoOperacao1.Mensagem = "Já existe uma orientação curricular com esses filtros, portanto ela será sobreposta, confirma ação?";

                    CarregarCabecalho();
                    CarregarOrientacoesCurriculares();

                    UCComboCursoCurriculo1.MostrarMessageSelecione = UCComboCursoCurriculo1.Obrigatorio =
                        UCComboCurriculoPeriodo1._MostrarMessageSelecione = UCComboCurriculoPeriodo1.Obrigatorio =
                            UCComboCurriculoPeriodo1.CancelSelect = UCComboCursoCurriculo1.CancelSelect = true;

                    UCComboCursoCurriculo1.MostrarMessageSelecione = UCComboCursoCurriculo1.Obrigatorio = true;
                    UCComboCursoCurriculo1.CancelSelect = false;
                    UCComboCursoCurriculo1.CarregaCursos_Relacionados_Por_Escola(VS_cur_id, VS_crr_id, 0, 0, true);
                    UCComboCursoCurriculo1_IndexChanged();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                    updMessage.Update();
                }
            }

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCConfirmacaoOperacao1.ConfimaOperacao += UCConfirmacaoOperacao1_ConfimaOperacao;

            lblMensagemRelacionarHabilidade.Visible = false;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados do cabeçalho.
        /// </summary>
        private void CarregarCabecalho()
        {
            ORC_MatrizHabilidades entMatriz = new ORC_MatrizHabilidades();
            entMatriz.mat_id = VS_mat_id;
            ORC_MatrizHabilidadesBO.GetEntity(entMatriz);


            // Mostra o label com os dados selecionados.
            lblCabecalho.Text = "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_cur_nome + "<br/>";
            lblCabecalho.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_crp_descricao + "<br/>";
            lblCabecalho.Text += "<b>" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + ": </b>" + VS_tds_nome + "<br/>";
            lblCabecalho.Text += "<b>Calendário escolar: </b>" + VS_cal_descricao + "<br/>";
            lblCabecalho.Text += "<b>Nome da matriz: </b>" + entMatriz.mat_nome + "<br/>";
            lblCabecalho.Text += "<b>Matriz padrão: </b>" + (entMatriz.mat_padrao ? "Sim" : "Não") + "<br/>";

            string msgReplicar = String.Format("A replicação é possível apenas para um {0} com a mesma quantidade de níveis de orientação curricular.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower());
            lblInfoReplicar.Text = UtilBO.GetErroMessage(msgReplicar, UtilBO.TipoMensagem.Informacao);
        }

        /// <summary>
        /// Carrega as orientações curriculares
        /// </summary>
        private void CarregarOrientacoesCurriculares()
        {
            int nvl_ordem;

            if (VS_ocr_id > 0)
            {
                ORC_OrientacaoCurricularBO.OrientacaoCurricular orientacaoAtual = OrientacaoAtual;

                lblOrientacaoPai.Text = string.IsNullOrEmpty(orientacaoAtual.entOrientacao.ocr_codigo) ?
                                        string.Empty :
                                        "<b>Código superior:</b> " + orientacaoAtual.entOrientacao.ocr_codigo + "<br />";

                lblOrientacaoPai.Text += "<b>Descrição superior:</b> " + orientacaoAtual.entOrientacao.ocr_descricao;

                nvl_ordem = orientacaoAtual.nvl_ordem;

                grvOrientacaoCurricular.Columns[grvEditarFilhos].Visible = nvl_ordem < maxNivel;
            }
            else
            {
                if (VS_dtNivel.Count > 0)
                {
                    nvl_ordem = VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                          .Select(row => row.nvl_ordem).First();
                }
                else
                {
                    nvl_ordem = 0;
                }

                lblOrientacaoPai.Text = string.Empty;

                grvOrientacaoCurricular.Columns[grvEditarFilhos].Visible = nvl_ordem < maxNivel;
            }

            string nvl_nomeProximo = VS_ocr_id > 0 ?
                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                .Where(row => row.nvl_ordem > nvl_ordem)
                                                .Select(row => row.nvl_nome).FirstOrDefault() :
                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                .Where(row => row.nvl_ordem == nvl_ordem)
                                                .Select(row => row.nvl_nome).FirstOrDefault();

            pnlResuldados.GroupingText = String.Format("Cadastro de {0}", nvl_nomeProximo);
            btnVoltar.Visible = VS_ocr_id > 0;

            #region Mostra coluna níveis de aprendizado da orientação curricular

            string ordem = VS_ocr_id > 0 ?
                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                .Where(row => row.nvl_ordem > nvl_ordem)
                                                .Select(row => row.nvl_ordem.ToString()).FirstOrDefault() :
                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                .Where(row => row.nvl_ordem == nvl_ordem)
                                                .Select(row => row.nvl_ordem.ToString()).FirstOrDefault();

            int aux = !string.IsNullOrEmpty(ordem) ? Convert.ToInt32(ordem) : 0;

            // Só mostra coluna níveis de aprendizado quando for a orientação curricular do último nível.
            if (aux == maxNivel)
            {
                ORC_MatrizHabilidades entMatriz = new ORC_MatrizHabilidades();
                entMatriz.mat_id = VS_mat_id;
                ORC_MatrizHabilidadesBO.GetEntity(entMatriz);

                grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible = true;
                grvOrientacaoCurricular.Columns[grvRelacionarHabilidades].Visible = entMatriz.mat_padrao;
                MostraDivLegenda();
            }
            else
            {
                grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible =
                    grvOrientacaoCurricular.Columns[grvRelacionarHabilidades].Visible = false;
                divLegenda.Visible = false;
            }

            grvOrientacaoCurricular.DataBind();

            #endregion

            #region Mostra coluna Replicar no primeiro nível

            if (aux == 1)
            {
                grvOrientacaoCurricular.Columns[grvReplicar].Visible = true;
            }
            else
            {
                grvOrientacaoCurricular.Columns[grvReplicar].Visible = false;
            }

            #endregion

            if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
            {
                PreencheNiveisAprendizado();
            }

        }

        /// <summary>
        /// Busca por uma orientação curricular na lista de orientações (Busca recursiva)
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular procurada.</param>
        /// <param name="listaBusca">Lista para a busca</param>
        /// <param name="achou">Flag que indica se a orientação foi encontrada</param>
        /// <returns></returns>
        private ORC_OrientacaoCurricularBO.OrientacaoCurricular BuscaOrientacao(long ocr_id, List<ORC_OrientacaoCurricularBO.OrientacaoCurricular> listaBusca, ref bool achou)
        {
            if (listaBusca.Any(p => p.entOrientacao.ocr_id == ocr_id))
            {
                achou = true;
                return listaBusca.Where(p => p.entOrientacao.ocr_id == ocr_id).First();
            }

            ORC_OrientacaoCurricularBO.OrientacaoCurricular orientacao = new ORC_OrientacaoCurricularBO.OrientacaoCurricular();

            foreach (ORC_OrientacaoCurricularBO.OrientacaoCurricular item in listaBusca)
            {
                orientacao = BuscaOrientacao(ocr_id, item.ltOrientacaoCurricularFilho, ref achou);
                if (achou)
                    return orientacao;
            }

            return orientacao;
        }

        /// <summary>
        /// O método copia n vezes uma string e a concatena para si mesma.
        /// </summary>
        /// <param name="valor">String a ser copiado.</param>
        /// <param name="multiplicacao">Quantidade de vezes que o valor será replicado.</param>
        /// <returns></returns>
        private string MultiplicaString(string valor, int multiplicacao)
        {
            StringBuilder sb = new StringBuilder(multiplicacao * valor.Length);
            for (int i = 0; i < multiplicacao; i++)
            {
                sb.Append(valor);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Habilita/Desabilita checkbox dos níveis de aprendizado.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="habilita"></param>
        private void HabilitaCheckBoxNiveis(int row, bool habilita)
        {
            try
            {
                Repeater rptNiveis = (Repeater)grvOrientacaoCurricular.Rows[row].FindControl("rptNivelAprendizado");

                if (rptNiveis != null)
                {
                    foreach (RepeaterItem item in rptNiveis.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkNivel");
                        if (chk != null)
                        {
                            chk.Enabled = habilita;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar orientações curriculares.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void PreencheNiveisAprendizado()
        {
            try
            {
                ORC_OrientacaoCurricularNivelAprendizado nivelAprendizado;

                if (OrientacaoAtual.ltNivelAprendizadoOrientacaoFilho != null)
                {
                    foreach (GridViewRow row in grvOrientacaoCurricular.Rows)
                    {
                        Repeater rptNiveis = (Repeater)grvOrientacaoCurricular.Rows[row.RowIndex].FindControl("rptNivelAprendizado");
                        rptNiveis.DataSource = dtNiveisAprendizado;
                        rptNiveis.DataBind();

                        if (rptNiveis != null)
                        {
                            foreach (RepeaterItem item in rptNiveis.Items)
                            {
                                nivelAprendizado = new ORC_OrientacaoCurricularNivelAprendizado
                                {
                                    ocr_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[row.RowIndex].Values["entOrientacao"])).ocr_id,
                                    nap_id = 0
                                };
                                HiddenField hdn = (HiddenField)item.FindControl("hdnIdNivel");
                                if (hdn != null && !string.IsNullOrEmpty(hdn.Value))
                                {
                                    nivelAprendizado.nap_id = Convert.ToInt32(hdn.Value);
                                }

                                ORC_OrientacaoCurricularNivelAprendizado nivelApAux = new ORC_OrientacaoCurricularNivelAprendizado();
                                nivelApAux = OrientacaoAtual.ltNivelAprendizadoOrientacaoFilho.Find(p => p.ocr_id == nivelAprendizado.ocr_id && p.nap_id == nivelAprendizado.nap_id);

                                if (nivelApAux != null && nivelApAux.ocn_dataCriacao != new DateTime())
                                {
                                    CheckBox chk = (CheckBox)item.FindControl("chkNivel");
                                    if (chk != null)
                                    {
                                        chk.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar orientações curriculares.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Se for pra mostrar a div Legenda, busca a descrição e a sigla
        /// </summary>
        private void MostraDivLegenda()
        {
            rptLegenda.DataSource = dtNiveisAprendizado;
            rptLegenda.DataBind();

            if (rptLegenda.Items.Count > 0)
            {
                divLegenda.Visible = true;
            }
        }

        private void ReplicarOrientacao(bool verificarConfirmacacao)
        {
            if (verificarConfirmacacao)
            {
                VS_ConfirmacaoPadrao = (byte)ORC_OrientacaoCurriculaTipoMensagem.DadosReplicarOrientacao;
                UCConfirmacaoOperacao1.ObservacaoVisivel = false;
                UCConfirmacaoOperacao1.ObservacaoObrigatorio = false;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaMovimentacao", "$(document).ready(function(){ $('#divConfirmacao').dialog('open'); });", true);
            }
            else
            {
                ORC_OrientacaoCurricularBO.ReplicarOrientacaoCurricular(VS_cur_id, VS_crr_id, UCComboCurriculoPeriodo1.Valor[2], VS_crp_id, VS_cal_id, VS_tds_id, VS_ocr_id_replicada, UCComboCursoCurriculo1.Valor[0], VS_mat_id);

                // Fecha o popup e mostra mensagem salvo com sucesso
                ScriptManager.RegisterStartupScript(this, GetType(), "ReplicarOrientacao", "$(document).ready(function(){  $('#divReplicar').dialog('close');});", true);
                updReplicar.Update();

                lblMessage.Text = UtilBO.GetErroMessage("Orientação curricular replicada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                updMessage.Update();
            }
        }

        /// <summary>
        /// Carrega o pop-up de relacionamento de habilidades
        /// </summary>
        private void RelacionarHabilidades(ORC_OrientacaoCurricular entOrientacaoCurricular)
        {
            if (entOrientacaoCurricular.ocr_id > 0)
            {
                VS_ocr_id_relacionamento = entOrientacaoCurricular.ocr_id;

                ORC_MatrizHabilidades entMatriz = new ORC_MatrizHabilidades();
                entMatriz.mat_id = VS_mat_id;
                ORC_MatrizHabilidadesBO.GetEntity(entMatriz);

                // Mostra o label com os dados selecionados.
                lblHabilidadeSelecionada.Text = "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_cur_nome + "<br/>";
                lblHabilidadeSelecionada.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_crp_descricao + "<br/>";
                lblHabilidadeSelecionada.Text += "<b>" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + ": </b>" + VS_tds_nome + "<br/>";
                lblHabilidadeSelecionada.Text += "<b>Calendário escolar: </b>" + VS_cal_descricao + "<br/>";
                lblHabilidadeSelecionada.Text += "<b>Nome da matriz: </b>" + entMatriz.mat_nome + "<br/>";
                lblHabilidadeSelecionada.Text += "<b>" + GetGlobalResourceObject("Academico", "OrientacaoCurricular.CadastroHabilidade.lblHabilidadeSelecionada.text.habilidade") + ": </b>" + entOrientacaoCurricular.ocr_descricao + "<br/>";

                // Carrega o combo de matrizes curriculares
                CarregarMatrizesCurriculares();

                if (ddlMatrizCurricular.Items.Count == 2)
                {
                    ddlMatrizCurricular.SelectedIndex = 1;
                }

                // Carregar a arvore de habilidades da matriz selecionada
                CarregarMatrizRelacionadaSelecionada();
            }
            else
            {
                btnRelacionarHabilidades.Visible = false;
            }
        }

        /// <summary>
        /// Carrega o combo de matrizes curriculares
        /// </summary>
        private void CarregarMatrizesCurriculares()
        {
            ddlMatrizCurricular.Items.Clear();
            DataTable DtMatrizesCurriculares = ORC_MatrizHabilidadesBO.SelectMatrizHabilidades_ByCursoPeriodoDisciplinaPadrao(VS_cur_id, VS_crr_id, VS_crp_id, VS_cal_id, VS_tds_id, false,
                                                                                                                              __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ListItem li = new ListItem(GetGlobalResourceObject("Academico", "OrientacaoCurricular.CadastroHabilidade.ddlMatrizCurricular.valor0").ToString(), "-1", true);
            ddlMatrizCurricular.Items.Add(li);

            foreach (DataRow dr in DtMatrizesCurriculares.Rows)
            {
                li = new ListItem(dr["mat_nome"].ToString(), dr["mat_id"].ToString());
                ddlMatrizCurricular.Items.Add(li);
            }
        }

        private void CarregarMatrizRelacionadaSelecionada()
        {
            Int64 matriz_id = Convert.ToInt64(ddlMatrizCurricular.SelectedValue);
            Nivel = 0;
            if (matriz_id > 0)
            {
                rptMatrizRelacionada.DataSource = ORC_OrientacaoCurricularBO.SelecionaPorCalendarioPeriodoDisciplinaTreeview_ByMatriz
                                                    (
                                                        VS_cal_id,
                                                        VS_cur_id,
                                                        VS_crr_id,
                                                        VS_crp_id,
                                                        VS_tds_id,
                                                        matriz_id,
                                                        VS_ocr_id_relacionamento
                                                        );
            }
            else
            {
                rptMatrizRelacionada.DataSource = new DataTable();
            }
            rptMatrizRelacionada.DataBind();
            updRelacionarHabilidades.Update();

            pnlHabilidadesRelacionadas.Visible = matriz_id > 0;
            btnRelacionarHabilidades.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && matriz_id > 0;
        }

        #endregion

        #region Eventos

        protected void grvOrientacaoCurricular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int nvl_odem = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "nvl_ordem"));

                ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ImageButton imgEditarFilhos = (ImageButton)e.Row.FindControl("imgEditarFilhos");
                if (imgEditarFilhos != null)
                {
                    imgEditarFilhos.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    imgEditarFilhos.CommandArgument = e.Row.RowIndex.ToString();
                    imgEditarFilhos.Visible = nvl_odem < maxNivel;
                }

                ImageButton imgExcluir = (ImageButton)e.Row.FindControl("imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;

                ImageButton imgReplicar = (ImageButton)e.Row.FindControl("imgReplicar");
                if (imgReplicar != null)
                {
                    //imgReplicar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    imgReplicar.CommandArgument = e.Row.RowIndex.ToString();
                    //imgReplicar.Visible = nvl_odem < maxNivel;
                }

                ImageButton btnRelacionarHabilidades = (ImageButton)e.Row.FindControl("btnRelacionarHabilidades");
                if (btnRelacionarHabilidades != null)
                {
                    ORC_OrientacaoCurricular orientacaoCurricular = (ORC_OrientacaoCurricular)(DataBinder.Eval(e.Row.DataItem, "entOrientacao"));
                    btnRelacionarHabilidades.CommandArgument = e.Row.RowIndex.ToString();
                    btnRelacionarHabilidades.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && orientacaoCurricular.ocr_id > 0;
                    if (btnRelacionarHabilidades.Visible)
                    {
                        Image imgRelacionarHabilidadesSituacao = (Image)e.Row.FindControl("imgRelacionarHabilidadesSituacao");
                        imgRelacionarHabilidadesSituacao.Visible = orientacaoCurricular.Relacionada;
                    }
                }
            }
        }

        protected void grvOrientacaoCurricular_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;
            ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgEditar");
            if (imgEditar != null)
            {
                imgEditar.Visible = false;
                ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgCancelar");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;
            }
            ImageButton btnRelacionarHabilidades = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnRelacionarHabilidades");
            if (btnRelacionarHabilidades != null)
                btnRelacionarHabilidades.Visible = false;

            PreencheNiveisAprendizado();

            if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
            {
                HabilitaCheckBoxNiveis(e.NewEditIndex, true);
            }

            grv.Rows[e.NewEditIndex].Focus();

            indexAlteracao = e.NewEditIndex;
        }

        protected void grvOrientacaoCurricular_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                {

                    if (VS_ocr_id > 0)
                    {
                        grv.DataSource = OrientacaoAtual.ltOrientacaoCurricularFilho;
                        grv.DataBind();
                    }
                    else
                    {
                        grv.DataSource = VS_ltOrientacaoCurricular;
                        grv.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar orientações curriculares.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void grvOrientacaoCurricular_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);

                if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
                {
                    HabilitaCheckBoxNiveis(e.RowIndex, false);
                }

                // Se for um registro novo, exclui a linha
                if (VS_ocr_id > 0 && OrientacaoAtual.ltOrientacaoCurricularFilho[e.RowIndex].entOrientacao.ocr_id <= 0)
                {
                    OrientacaoAtual.ltOrientacaoCurricularFilho.RemoveAt(e.RowIndex);
                }
                else if (VS_ocr_id <= 0 && VS_ltOrientacaoCurricular[e.RowIndex].entOrientacao.ocr_id <= 0)
                {
                    VS_ltOrientacaoCurricular.RemoveAt(e.RowIndex);
                }

                grv.EditIndex = -1;
                grv.DataBind();

                PreencheNiveisAprendizado();

                //indexAlteracao = -1;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao cancelar operação.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void grvOrientacaoCurricular_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                long ocr_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[e.RowIndex].Values["entOrientacao"])).ocr_id;

                ORC_OrientacaoCurricular entity = new ORC_OrientacaoCurricular
                {
                    IsNew = ocr_id <= 0
                    ,
                    ocr_id = ocr_id
                    ,
                    ocr_idSuperior = VS_ocr_id
                    ,
                    nvl_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[e.RowIndex].Values["entOrientacao"])).nvl_id
                    ,
                    tds_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[e.RowIndex].Values["entOrientacao"])).tds_id
                    ,
                    ocr_situacao = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[e.RowIndex].Values["entOrientacao"])).ocr_situacao
                    ,
                    mat_id = VS_mat_id
                };

                TextBox txtCodigo = (TextBox)grv.Rows[e.RowIndex].FindControl("txtCodigo");
                if (txtCodigo != null)
                {
                    entity.ocr_codigo = txtCodigo.Text;
                }

                TextBox txtDescricao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtDescricao");
                if (txtDescricao != null)
                {
                    entity.ocr_descricao = txtDescricao.Text;
                }

                if (ORC_OrientacaoCurricularBO.Save(entity))
                {
                    VS_ltOrientacaoCurricular = null;

                    if (ocr_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "ocr_id: " + entity.ocr_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Orientação curricular incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "ocr_id: " + entity.ocr_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Orientação curricular alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    // Salva os níveis de aprendizado da orientação curricular.
                    if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
                    {
                        // Busca níveis de aprendizado das orientações curriculares filhos
                        List<ORC_OrientacaoCurricularNivelAprendizado> ltNivelAprendizado = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectNivelAprendizadoByOcrIdSuperior(VS_ocr_id);
                        List<ORC_OrientacaoCurricularNivelAprendizado> ltNivelAprendizadoTodos = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectTodosNivelAprendizadoByOcrId(entity.ocr_id);

                        Repeater rptNiveis = (Repeater)grvOrientacaoCurricular.Rows[e.RowIndex].FindControl("rptNivelAprendizado");

                        ORC_OrientacaoCurricularNivelAprendizado orientacaoNivelAprendizado;
                        ORC_OrientacaoCurricularNivelAprendizado orientacaoNivelAprendizadoAux;
                        ORC_OrientacaoCurricularNivelAprendizado orientacaoNivelAprendizadoAuxExcluido;

                        if (rptNiveis != null)
                        {
                            foreach (RepeaterItem item in rptNiveis.Items)
                            {
                                CheckBox chk = (CheckBox)item.FindControl("chkNivel");
                                if (chk != null)
                                {
                                    HiddenField hdnNapId = (HiddenField)item.FindControl("hdnIdNivel");
                                    int nap_id = (!string.IsNullOrEmpty(hdnNapId.Value) ? Convert.ToInt32(hdnNapId.Value) : 0);

                                    orientacaoNivelAprendizado = new ORC_OrientacaoCurricularNivelAprendizado
                                    {
                                        ocr_id = entity.ocr_id,
                                        nap_id = nap_id,
                                        ocn_id = -1,
                                        ocn_situacao = 1,
                                        ocn_dataCriacao = DateTime.Now,
                                        ocn_dataAlteracao = DateTime.Now,
                                        IsNew = true
                                    };

                                    orientacaoNivelAprendizadoAux = ltNivelAprendizado.Find(p => p.ocr_id == orientacaoNivelAprendizado.ocr_id && p.nap_id == orientacaoNivelAprendizado.nap_id);
                                    orientacaoNivelAprendizadoAuxExcluido = ltNivelAprendizadoTodos.Find(p => p.ocr_id == orientacaoNivelAprendizado.ocr_id && p.nap_id == orientacaoNivelAprendizado.nap_id);

                                    if (!chk.Checked && orientacaoNivelAprendizadoAux != null)
                                    {
                                        orientacaoNivelAprendizado.IsNew = false;
                                        orientacaoNivelAprendizado.ocn_id = orientacaoNivelAprendizadoAux.ocn_id;
                                        orientacaoNivelAprendizado.ocn_situacao = 3;
                                    }
                                    else if (chk.Checked && orientacaoNivelAprendizadoAuxExcluido != null)
                                    {
                                        orientacaoNivelAprendizado.IsNew = false;
                                        orientacaoNivelAprendizado.ocn_id = orientacaoNivelAprendizadoAuxExcluido.ocn_id;
                                    }
                                    else if ((!chk.Checked && orientacaoNivelAprendizadoAux == null) || (chk.Checked && orientacaoNivelAprendizadoAux != null))
                                    {
                                        continue;
                                    }
                                    ORC_OrientacaoCurricularNivelAprendizadoBO.Save(orientacaoNivelAprendizado);
                                }
                            }
                        }
                    }

                    indexAlteracao = e.RowIndex;

                    grv.EditIndex = -1;
                    grv.DataBind();

                    if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
                    {
                        PreencheNiveisAprendizado();
                        MostraDivLegenda();
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        protected void grvOrientacaoCurricular_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                long ocr_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[e.RowIndex].Values["entOrientacao"])).ocr_id;
                if (ocr_id > 0)
                {
                    ORC_OrientacaoCurricular entity = new ORC_OrientacaoCurricular
                    {
                        ocr_id = ocr_id
                    };

                    if (ORC_OrientacaoCurricularBO.DeletarHierarquia(entity))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "ocr_id: " + entity.ocr_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Orientação curricular excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        VS_ltOrientacaoCurricular = null;
                        grv.EditIndex = -1; 
                        grv.DataBind();
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        protected void grvOrientacaoCurricular_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditFilhos"))
            {
                grvOrientacaoCurricular.EditIndex = -1;

                int index = Convert.ToInt32(e.CommandArgument);
                VS_ocr_id = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[index].Values["entOrientacao"])).ocr_id;
                CarregarOrientacoesCurriculares();
            }
            else if (e.CommandName.Equals("Replicar"))
            {
                grvOrientacaoCurricular.EditIndex = -1;

                //UCComboCurriculoPeriodo1._Combo.Items.Clear();
                // Preenche o combo com os períodos que possuem a mesma quantidade de níveis do nível da orientação curricular atual
                //UCComboCurriculoPeriodo1.CarregarPorQtdeNivelOrientacaoCurricular(maxNivel, VS_cur_id, VS_crr_id, VS_cal_id, VS_tds_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                //UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(VS_cur_id, VS_crr_id, -1, -1);

                UCComboCursoCurriculo1.MostrarMessageSelecione = UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCursoCurriculo1.CancelSelect = false;
                UCComboCursoCurriculo1.CarregaCursos_Relacionados_Por_Escola(VS_cur_id, VS_crr_id, 0, 0, true);
                UCComboCursoCurriculo1_IndexChanged();

                lblMensagemReplicar.Text = "";

                int index = Convert.ToInt32(e.CommandArgument);
                VS_ocr_id_replicada = ((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[index].Values["entOrientacao"])).ocr_id;

                ScriptManager.RegisterStartupScript(this, GetType(), "ReplicarOrientacao", "$(document).ready(function(){  $('#divReplicar').dialog('open');});", true);
                updReplicar.Update();
            }
            else if (e.CommandName.Equals("RelacionarHabilidades"))
            {
                hdnRowRelacionamento.Value = e.CommandArgument.ToString();

                grvOrientacaoCurricular.EditIndex = -1;
                RelacionarHabilidades((ORC_OrientacaoCurricular)(grvOrientacaoCurricular.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["entOrientacao"]));

                ScriptManager.RegisterStartupScript(this, GetType(), "RelacionarHabilidades",
                "$(document).ready(function(){ $('#divRelacionarHabilidades').dialog('option', 'title', '" + GetGlobalResourceObject("Academico", "OrientacaoCurricular.CadastroHabilidade.divRelacionarHabilidades.title") + "');" +
                "$('#divRelacionarHabilidades').dialog('open');});", true);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            grvOrientacaoCurricular.EditIndex = -1;

            VS_ocr_id = OrientacaoAtual.entOrientacao.ocr_idSuperior;
            CarregarOrientacoesCurriculares();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/OrientacaoCurricular/Busca.aspx");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                int nvl_ordem = VS_ocr_id > 0 ? OrientacaoAtual.nvl_ordem :
                                                VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                          .Select(row => row.nvl_ordem).First();



                sNivelOrientacaoCurricular drNivel =  VS_ocr_id > 0 ?
                                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                                .Where(row => row.nvl_ordem > nvl_ordem).First() :
                                                      VS_dtNivel.OrderBy(row => row.nvl_ordem)
                                                                .Where(row => row.nvl_ordem == nvl_ordem).First();


                ORC_OrientacaoCurricularBO.OrientacaoCurricular orientacao =
                    new ORC_OrientacaoCurricularBO.OrientacaoCurricular
                    {
                        entOrientacao = new ORC_OrientacaoCurricular
                        {
                            ocr_id = -1
                            ,
                            ocr_idSuperior = VS_ocr_id
                            ,
                            IsNew = true
                            ,
                            ocr_codigo = string.Empty
                            ,
                            ocr_descricao = string.Empty
                            ,
                            nvl_id = drNivel.nvl_id
                            ,
                            tds_id = VS_tds_id
                            ,
                            ocr_situacao = 1
                            ,
                            mat_id = VS_mat_id
                            ,
                            Relacionada = false
                        }
                        ,
                        nvl_nome = drNivel.nvl_nome
                        ,
                        nvl_ordem = drNivel.nvl_ordem
                        ,
                        ltOrientacaoCurricularFilho = new List<ORC_OrientacaoCurricularBO.OrientacaoCurricular>()
                    };

                if (VS_ocr_id > 0)
                {
                    OrientacaoAtual.ltOrientacaoCurricularFilho.Add(orientacao);
                }
                else
                {
                    VS_ltOrientacaoCurricular.Add(orientacao);
                }

                int index = VS_ocr_id > 0 ? OrientacaoAtual.ltOrientacaoCurricularFilho.Count - 1 : VS_ltOrientacaoCurricular.Count - 1;
                grvOrientacaoCurricular.EditIndex = index;
                grvOrientacaoCurricular.DataSource = VS_ocr_id > 0 ? OrientacaoAtual.ltOrientacaoCurricularFilho : VS_ltOrientacaoCurricular;
                grvOrientacaoCurricular.DataBind();

                ImageButton imgEditar = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;

                ImageButton imgEditarFilhos = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgEditarFilhos");
                if (imgEditarFilhos != null)
                    imgEditarFilhos.Visible = false;

                ImageButton imgSalvar = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;
                ImageButton imgCancelar = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgCancelarOrientacao");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;

                ImageButton imgReplicar = (ImageButton)grvOrientacaoCurricular.Rows[index].FindControl("imgReplicar");
                if (imgReplicar != null)
                    imgReplicar.Visible = false;

                string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
                Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

                // Carrega checkbox dos niveis de aprendizado
                PreencheNiveisAprendizado();

                if (grvOrientacaoCurricular.Columns[grvNiveisAprendizado].Visible)
                {
                    HabilitaCheckBoxNiveis(index, true);
                }

                grvOrientacaoCurricular.Rows[index].Focus();

                indexAlteracao = -1;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova orientação curricular.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            Nivel = 0;

            rptOrientacoes.DataSource = ORC_OrientacaoCurricularBO.SelecionaPorCalendarioPeriodoDisciplinaTreeview_ByMatriz
                                        (
                                            VS_cal_id,
                                            VS_cur_id,
                                            VS_crr_id,
                                            VS_crp_id,
                                            VS_tds_id,
                                            VS_mat_id,
                                            -1
                                         );

            rptOrientacoes.DataBind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ExibirOrientacoes", "$(document).ready(function(){ $('#divOrientacoes').dialog('open'); });", true);
            updOrientacoes.Update();
        }

        protected void rptOrientacoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int nivelLinha = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "level_id"));
                bool folha = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "sheet")); ;
                string ocr_codigo = DataBinder.Eval(e.Item.DataItem, "ocr_codigo").ToString();
                string ocr_descricao = DataBinder.Eval(e.Item.DataItem, "ocr_descricao").ToString();

                Literal litCabecalho = (Literal)e.Item.FindControl("litCabecalho");
                Literal litConteudo = (Literal)e.Item.FindControl("litConteudo");
                Literal litRodape = (Literal)e.Item.FindControl("litRodape");

                string ul = String.Format("<ul class='treeview' style='display: {0};'>", nivelLinha == 1 ? "block" : "none");
                string li = nivelLinha == 1 ? "<li class='last'>" : folha ? "<li class='last'>" : "<li class='expandable'>";

                string cabecalho = nivelLinha == Nivel ? li :
                    (nivelLinha > Nivel ? ul + li : MultiplicaString("</li></ul>", Nivel - nivelLinha) + li);
                cabecalho += folha ? string.Empty : "<div class='hitarea expandable-hitarea'></div>";
                litCabecalho.Text = cabecalho;

                litConteudo.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                litRodape.Visible = folha || (nivelLinha == Nivel);
                litRodape.Text = folha ? "</li>" : string.Empty;

                Nivel = nivelLinha;
            }
        }

        protected void odsNivelAprendizado_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                if (e.Exception.InnerException is ValidationException)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(e.Exception.InnerException.Message, UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    ApplicationWEB._GravaErro(e.Exception.InnerException);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis de aprendizado.", UtilBO.TipoMensagem.Erro);
                }

                e.ExceptionHandled = true;
            }
        }

        protected void btnReplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (VS_cur_id == UCComboCursoCurriculo1.Valor[0] && VS_crp_id == UCComboCurriculoPeriodo1.Valor[2])
                {
                    throw new ValidationException(UCComboCursoCurriculo1.Titulo + " e " + UCComboCurriculoPeriodo1.Titulo.ToLower() + "é igual ao do cadastro.");
                }

                /* Se já existir uma orientação para o ano e disciplina selecionados -> msg p/ confirma operação */
                List<ORC_OrientacaoCurricularBO.OrientacaoCurricular> lt = ORC_OrientacaoCurricularBO.SelecionaPorCalendarioPeriodoDisciplina(VS_cal_id, UCComboCursoCurriculo1.Valor[0], VS_crr_id, UCComboCurriculoPeriodo1.Valor[2], VS_tds_id, VS_mat_id);

                bool achou = false;
                ORC_OrientacaoCurricularBO.OrientacaoCurricular orientacao = BuscaOrientacao(VS_ocr_id_replicada, VS_ltOrientacaoCurricular, ref achou);

                if (achou && lt.Any(p => (p.entOrientacao.ocr_codigo ?? string.Empty).Equals(orientacao.entOrientacao.ocr_codigo ?? string.Empty) &&
                                         p.entOrientacao.ocr_descricao.Equals(orientacao.entOrientacao.ocr_descricao) &&
                                         p.nvl_ordem == orientacao.nvl_ordem))
                {
                    ReplicarOrientacao(true);
                }
                else
                {
                    ReplicarOrientacao(false);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemReplicar.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMensagemReplicar.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemReplicar.Text = UtilBO.GetErroMessage("Erro ao tentar replicar a orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptMatrizRelacionada_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int nivelLinha = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "level_id"));
                bool folha = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "sheet")); ;
                string ocr_codigo = DataBinder.Eval(e.Item.DataItem, "ocr_codigo").ToString();
                string ocr_descricao = DataBinder.Eval(e.Item.DataItem, "ocr_descricao").ToString();

                Literal litCabecalho = (Literal)e.Item.FindControl("litCabecalho");
                Literal litRodape = (Literal)e.Item.FindControl("litRodape");
                Literal lblHabilidade = (Literal)e.Item.FindControl("lblHabilidade");

                string ul = String.Format("<ul class='treeview' style='display: {0};'>", nivelLinha == 1 ? "block" : "none");
                string li = nivelLinha == 1 ? "<li class='last'>" : folha ? "<li class='last' style='display: table; width:100%'>" : "<li class='expandable'>";

                string cabecalho = nivelLinha == Nivel ? li :
                    (nivelLinha > Nivel ? ul + li : MultiplicaString("</li></ul>", Nivel - nivelLinha) + li);
                cabecalho += folha ? string.Empty : "<div class='hitarea expandable-hitarea'></div>";
                litCabecalho.Text = cabecalho;

                lblHabilidade.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                litRodape.Visible = folha || (nivelLinha == Nivel);
                litRodape.Text = folha ? "</li>" : string.Empty;

                Nivel = nivelLinha;

                CheckBox chkRelacionada = (CheckBox)e.Item.FindControl("chkRelacionada");
                chkRelacionada.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
        }

        protected void ddlMatrizCurricular_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarMatrizRelacionadaSelecionada();
        }

        protected void btnRelacionarHabilidades_Click(object sender, EventArgs e)
        {
            try
            {
                List<ORC_OrientacaoCurricularRelacionamento> listaRelacionamentos = new List<ORC_OrientacaoCurricularRelacionamento>();
                foreach (RepeaterItem habilidade in rptMatrizRelacionada.Items)
                {
                    CheckBox chkRelacionada = (CheckBox)habilidade.FindControl("chkRelacionada");
                    HiddenField hdnChave = (HiddenField)habilidade.FindControl("hdnChave");
                    HiddenField hdnRelacionada = (HiddenField)habilidade.FindControl("hdnRelacionada");
                    if (chkRelacionada.Visible)
                    {
                        ORC_OrientacaoCurricularRelacionamento entity = new ORC_OrientacaoCurricularRelacionamento();
                        entity.isChecked = chkRelacionada.Checked;
                        entity.ocr_id = VS_ocr_id_relacionamento;
                        entity.ocr_idRelacionada = Convert.ToInt64(hdnChave.Value);
                        entity.IsNew = hdnRelacionada.Value == "0";
                        if (entity.isChecked || !entity.IsNew)
                        {
                            listaRelacionamentos.Add(entity);
                        }
                    }
                }
                ORC_OrientacaoCurricularRelacionamentoBO.Salvar(listaRelacionamentos);
                CarregarMatrizRelacionadaSelecionada();
                lblMensagemRelacionarHabilidade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "OrientacaoCurricular.CadastroHabilidade.lblMensagemRelacionarHabilidade.Text.Sucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                lblMensagemRelacionarHabilidade.Visible = true;

                // atualiza na tela a situacao do relacionamento da orientação curricular selecionada
                bool relacionada = ORC_OrientacaoCurricularBO.SelecionaPossuiRelacionamento(VS_ocr_id_relacionamento);
                grvOrientacaoCurricular.Rows[Convert.ToInt32(hdnRowRelacionamento.Value)].FindControl("imgRelacionarHabilidadesSituacao").Visible = relacionada;
                OrientacaoAtual.ltOrientacaoCurricularFilho.Find(p => p.entOrientacao.ocr_id == VS_ocr_id_relacionamento).entOrientacao.Relacionada = relacionada;
                updOrientacaoCurricular.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemRelacionarHabilidade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "OrientacaoCurricular.CadastroHabilidade.lblMensagemRelacionarHabilidade.Text.Erro").ToString(), UtilBO.TipoMensagem.Erro);
                lblMensagemRelacionarHabilidade.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "TopRelacionarHabilidades", "$('#divRelacionarHabilidades').animate({scrollTop:0}, 'fast');", true);
        }

        #endregion
    }
}