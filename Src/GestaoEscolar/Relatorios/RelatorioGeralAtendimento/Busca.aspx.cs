using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.RelatorioGeralAtendimento
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        private const string Ascending = "asc";
        private const string Descending = "desc";

        #endregion

        #region Propriedades

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return "pes_nome";
            }

            set
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    if (filtros.ContainsKey("VS_Ordenacao"))
                    {
                        filtros["VS_Ordenacao"] = value;
                        __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioGeralAtendimento, Filtros = filtros };
                    }
                }

            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return valor;
                    }
                }

                return Ascending;
            }

            set
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    if (filtros.ContainsKey("VS_SortDirection"))
                    {
                        filtros["VS_SortDirection"] = value.ToString();
                        __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioGeralAtendimento, Filtros = filtros };
                    }
                }

            }
        }

        /// <summary>
        /// Alunos selecionados no grid para impressao de seus documentos
        /// </summary>
        private SortedDictionary<long, bool> _VS_AlunosSelecionados
        {
            get
            {
                if (ViewState["_VS_AlunosSelecionados"] == null)
                    ViewState["_VS_AlunosSelecionados"] = new SortedDictionary<long, bool>();
                return (SortedDictionary<long, bool>)ViewState["_VS_AlunosSelecionados"];
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            UCCUAEscola.IndexChangedUA += UCCUAEscola_IndexChangedUA;
            UCCUAEscola.IndexChangedUnidadeEscola += UCCUAEscola_IndexChangedUnidadeEscola;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCurriculoPeriodo.IndexChanged += UCCCurriculoPeriodo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCCTipoRelatorioAtendimento.IndexChanged += UCCTipoRelatorioAtendimento_IndexChanged;
            UCCQtdePaginacao.IndexChanged += UCCQtdePaginacao_IndexChanged;

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaRelatorioGeralAtendimento.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        updFiltros.Visible = false;
                        lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "QuantitativoSugestoes.Busca.lblMessage.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                    }

                    string msg = __SessionWEB.PostMessages;

                    if (!string.IsNullOrEmpty(msg))
                    {
                        lblMensagem.Text = msg;
                        updMensagem.Update();
                    }

                    InicializarTela();
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }
        }

        #endregion

        #region Delegates

        private void UCCQtdePaginacao_IndexChanged()
        {
            if (grvResultados.Rows.Count > 0)
            {
                // atribui nova quantidade itens por página para o grid
                grvResultados.PageSize = UCCQtdePaginacao.Valor;
                grvResultados.PageIndex = 0;
                grvResultados.DataBind();
            }
        }

        private void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCTurma.Valor = new long[] { -1, -1, -1 };
                UCCTurma.PermiteEditar = false;

                if (UCCCalendario.Valor > 0)
                {
                    UCCTurma.CarregarPorEscolaCurriculoCalendario_TurmasNormais(UCCUAEscola.Esc_ID, UCCUAEscola.Uni_ID, UCCCurriculoPeriodo.Valor[0], UCCCurriculoPeriodo.Valor[1], UCCCurriculoPeriodo.Valor[2], UCCCalendario.Valor);
                    UCCTurma.PermiteEditar = true;
                    UCCTurma.Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) turma(s) do período.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        private void UCCCurriculoPeriodo_IndexChanged()
        {
            try
            {
                UCCCalendario.Valor = -1;

                if (UCCCurriculoPeriodo.Valor[0] > 0)
                {
                    UCCCalendario.CarregarPorCurso(UCCCursoCurriculo.Valor[0]);
                    UCCCalendario.PermiteEditar = true;
                    UCCCalendario.SetarFoco();
                }

                UCCCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        private void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCCCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                UCCCurriculoPeriodo.PermiteEditar = false;

                if (UCCCursoCurriculo.Valor[0] > 0)
                {
                    UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                    UCCCurriculoPeriodo.PermiteEditar = true;
                    UCCCurriculoPeriodo.Focus();
                }

                UCCCurriculoPeriodo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        private void UCCUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCursoCurriculo.Valor = new[] { -1, -1 };
                UCCCursoCurriculo.PermiteEditar = false;

                if (UCCUAEscola.Esc_ID > 0 && UCCUAEscola.Uni_ID > 0)
                {
                    UCCCursoCurriculo.CarregarVigentesPorEscola(UCCUAEscola.Esc_ID, UCCUAEscola.Uni_ID);
                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        private void UCCUAEscola_IndexChangedUA()
        {
            try
            {

                if (UCCUAEscola.Uad_ID == Guid.Empty)
                    UCCUAEscola.SelectedValueEscolas = new[] { -1, -1 };

                UCCUAEscola_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        private void UCCTipoRelatorioAtendimento_IndexChanged()
        {
            try
            {
                UCCRelatorioAtendimento.Valor = -1;
                UCCRelatorioAtendimento.PermiteEditar = false;

                if (UCCTipoRelatorioAtendimento.Valor > 0)
                {
                    UCCRelatorioAtendimento.CarregarPorPermissaoUuarioTipo((CLS_RelatorioAtendimentoTipo)UCCTipoRelatorioAtendimento.Valor);
                    UCCRelatorioAtendimento.PermiteEditar = true;
                    UCCRelatorioAtendimento.Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) relatório(s).", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
            finally
            {
                updFiltros.Update();
            }
        }

        #endregion

        #region Métodos

        private void InicializarTela()
        {
            UCCTipoRelatorioAtendimento.CarregarTipos();

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                pnlBusca.Visible = false;
                fdsResultados.Visible = true;
                Pesquisar();
            }
            else
            {
                pnlBusca.Visible = true;
                fdsResultados.Visible = false;
                UCCUAEscola.Inicializar();
                if (UCCUAEscola.VisibleUA)
                {
                    UCCUAEscola_IndexChangedUA();
                }

                // Carrega o nome referente ao parametro de matricula estadual.
                string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                UCCBuscaAluno.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                UCCBuscaAluno.TituloMatriculaEstadual = nomeMatriculaEstadual;

                VerificarBusca();
            }

            UCCQtdePaginacao.GridViewRelacionado = grvResultados;

            updFiltros.Update();
            updResultados.Update();
        }

        private void Pesquisar()
        {
            try
            {
                fdsResultados.Visible = false;

                SalvaBusca();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                UCCQtdePaginacao.Valor = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                grvResultados.PageIndex = 0;
                grvResultados.PageSize = UCCQtdePaginacao.Valor;

                grvResultados.DataBind();

                UCCQtdePaginacao.Visible = grvResultados.Rows.Count > 0;

                fdsResultados.Visible = true;

                updResultados.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
            {
                DateTime data;
                string valor, valor2, valor3;

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id <= 0)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCCUAEscola.Uad_ID = new Guid(valor);
                        UCCUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCCUAEscola.Uad_ID != Guid.Empty)
                        {
                            UCCUAEscola.FocoEscolas = true;
                            UCCUAEscola.PermiteAlterarCombos = true;
                        }
                        string esc_id;
                        string uni_id;

                        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                        {
                            UCCUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                            UCCUAEscola_IndexChangedUnidadeEscola();
                        }
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                UCCCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                UCCCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                UCCCurriculoPeriodo.Valor = new[] { UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                UCCCurriculoPeriodo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCCCalendario.Valor = Convert.ToInt32(valor);
                UCCCalendario_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
                UCCTurma.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
                UCCBuscaAluno.TipoBuscaNomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
                UCCBuscaAluno.NomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_dataNascimento", out valor);
                if (DateTime.TryParse(valor, out data))
                {
                    UCCBuscaAluno.DataNascAluno = valor;
                }
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nomeMae", out valor);
                UCCBuscaAluno.NomeMaeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
                UCCBuscaAluno.MatriculaAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matriculaEstadual", out valor);
                UCCBuscaAluno.MatriculaEstadualAluno = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("rea_tipo", out valor);
                UCCTipoRelatorioAtendimento.Valor = Convert.ToByte(valor);
                UCCTipoRelatorioAtendimento_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("rea_id", out valor);
                UCCRelatorioAtendimento.Valor = Convert.ToInt32(valor);

            }

            if (UCCTurma.Valor[0] > 0)
            {
                Pesquisar();
            }
        }

        private void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id <= 0)
            {
                filtros.Add("rea_tipo", UCCTipoRelatorioAtendimento.Valor.ToString());
                filtros.Add("rea_id", UCCRelatorioAtendimento.Valor.ToString());

                filtros.Add("uad_idSuperior", UCCUAEscola.Uad_ID.ToString());
                filtros.Add("esc_id", UCCUAEscola.Esc_ID.ToString());
                filtros.Add("uni_id", UCCUAEscola.Uni_ID.ToString());
                filtros.Add("cur_id", UCCCursoCurriculo.Valor[0].ToString());
                filtros.Add("crr_id", UCCCursoCurriculo.Valor[1].ToString());
                filtros.Add("crp_id", UCCCurriculoPeriodo.Valor[2].ToString());
                filtros.Add("cal_id", UCCCalendario.Valor.ToString());
                filtros.Add("tur_id", UCCTurma.Valor[0].ToString());
                filtros.Add("ttn_id", UCCTurma.Valor[2].ToString());
                filtros.Add("crp_idTurma", UCCTurma.Valor[1].ToString());
                filtros.Add("tipoBusca", UCCBuscaAluno.TipoBuscaNomeAluno);

                filtros.Add("pes_nome", UCCBuscaAluno.NomeAluno);
                filtros.Add("pes_dataNascimento", UCCBuscaAluno.DataNascAluno);
                filtros.Add("pes_nomeMae", UCCBuscaAluno.NomeMaeAluno);
                filtros.Add("alc_matriculaEstadual", UCCBuscaAluno.MatriculaEstadualAluno);
                filtros.Add("alc_matricula", UCCBuscaAluno.MatriculaAluno);
            }

            filtros.Add("VS_Ordenacao", "pes_nome");
            filtros.Add("VS_SortDirection", "asc");

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioGeralAtendimento, Filtros = filtros };
        }

        private DataTable SelecionaDados()
        {
            DataTable dt;

            if (UCCTipoRelatorioAtendimento.Valor == (byte)CLS_RelatorioAtendimentoTipo.AEE)
            {
                dt = ACA_AlunoBO.BuscaAlunosRelatoriosAEE
                                    (
                                        UCCCalendario.Valor,
                                        UCCUAEscola.Esc_ID,
                                        UCCUAEscola.Uni_ID,
                                        UCCCursoCurriculo.Valor[0],
                                        UCCCursoCurriculo.Valor[1],
                                        UCCCurriculoPeriodo.Valor[2],
                                        UCCTurma.Valor[0],
                                        Convert.ToByte(UCCBuscaAluno.TipoBuscaNomeAluno),
                                        UCCBuscaAluno.NomeAluno,
                                        Convert.ToDateTime(string.IsNullOrEmpty(UCCBuscaAluno.DataNascAluno) ? new DateTime().ToString() : UCCBuscaAluno.DataNascAluno),
                                        UCCBuscaAluno.NomeMaeAluno,
                                        UCCBuscaAluno.MatriculaAluno,
                                        UCCBuscaAluno.MatriculaEstadualAluno,
                                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                        UCCUAEscola.Uad_ID,
                                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                        false,
                                        ApplicationWEB.AppMinutosCacheLongo
                                    );
            }
            else
            {
                dt = ACA_AlunoBO.BuscaAlunosRelatorioGeralAtendimento
                                (
                                    UCCCalendario.Valor,
                                    UCCUAEscola.Esc_ID,
                                    UCCUAEscola.Uni_ID,
                                    UCCCursoCurriculo.Valor[0],
                                    UCCCursoCurriculo.Valor[1],
                                    UCCCurriculoPeriodo.Valor[2],
                                    UCCTurma.Valor[0],
                                    Convert.ToByte(UCCBuscaAluno.TipoBuscaNomeAluno),
                                    UCCBuscaAluno.NomeAluno,
                                    Convert.ToDateTime(string.IsNullOrEmpty(UCCBuscaAluno.DataNascAluno) ? new DateTime().ToString() : UCCBuscaAluno.DataNascAluno),
                                    UCCBuscaAluno.NomeMaeAluno,
                                    UCCBuscaAluno.MatriculaAluno,
                                    UCCBuscaAluno.MatriculaEstadualAluno,
                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                    UCCUAEscola.Uad_ID,
                                    (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                    __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                    false,
                                    ApplicationWEB.AppMinutosCacheLongo
                                );
            }
            
            dt.DefaultView.Sort = VS_Ordenacao + " " + VS_SortDirection;

            return dt;
        }

        private string SortDir(string sortExpression)
        {
            string sDir = Ascending;

            if (VS_Ordenacao == sortExpression)
                sDir = VS_SortDirection == Ascending
                    ? Descending
                    : Ascending;
            else
                VS_Ordenacao = sortExpression;

            VS_SortDirection = sDir == Ascending ? Ascending : Descending;

            return sDir;
        }

        private void GerarRelatório()
        {
            try
            {
                __SessionWEB.PostMessages = String.Empty;
                string report = String.Empty;
                string parametros = String.Empty;
                string esc_ids = String.Empty;
                string alu_ids_boletim = String.Empty;
                string tur_ids_boletim = String.Empty;
                string tur_ids = String.Empty;

                _VS_AlunosSelecionados.Clear();

                if (chkTodos.Checked)
                {
                    DataTable dtAlunos = SelecionaDados();

                    foreach (DataRow row in dtAlunos.Rows)
                    {
                        if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(row["alu_id"])))
                        {
                            _VS_AlunosSelecionados.Add(Convert.ToInt64(row["alu_id"]), true);
                        }

                        alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + row["alu_id"];
                        tur_ids_boletim = (String.IsNullOrEmpty(tur_ids_boletim) ? "" : tur_ids_boletim + ",") + row["tur_id"];
                    }
                }
                else // se só a pagina atual tem selecionados corre as linhas do grid pegando os alu_ids atuais
                {
                    foreach (GridViewRow row in grvResultados.Rows)
                    {
                        CheckBox chkSelecionar = (CheckBox)row.FindControl("chkSelecionar");

                        if (chkSelecionar.Checked)
                        {
                            if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(grvResultados.DataKeys[row.RowIndex].Values["alu_id"])))
                            {
                                _VS_AlunosSelecionados.Add(Convert.ToInt64(grvResultados.DataKeys[row.RowIndex].Values["alu_id"]), true);
                            }

                            alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + grvResultados.DataKeys[row.RowIndex].Values["alu_id"];
                            tur_ids_boletim = (String.IsNullOrEmpty(tur_ids_boletim) ? "" : tur_ids_boletim + ",") + grvResultados.DataKeys[row.RowIndex].Values["tur_id"];
                        }
                    }
                }

                if (_VS_AlunosSelecionados.Count > 0)
                {
                    string alu_ids = String.Empty;

                    foreach (KeyValuePair<long, bool> kvp in _VS_AlunosSelecionados)
                    {
                        alu_ids = !String.IsNullOrEmpty(alu_ids) ? String.Concat(alu_ids + ',', kvp.Key) : kvp.Key.ToString();
                    }

                    String sDeclaracaoHMTL = string.Empty;
                    String sReportDev = string.Empty;

                    sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, (int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioGeralAtendimento);
                    report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioGeralAtendimento).ToString();
                    parametros = "alu_id=" + alu_ids_boletim +
                                 "&tur_id=" + tur_ids_boletim +
                                 "&rea_id=" + UCCRelatorioAtendimento.Valor +
                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                 "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                 "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                 "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                      , ApplicationWEB.LogoRelatorioSSRS);

                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                    MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);

                }
                else
                {
                    throw new ValidationException("Selecione pelo menos um aluno para gerar relatório.");
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception err)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                ApplicationWEB._GravaErro(err);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && UCCBuscaAluno.IsValid)
            {
                Pesquisar();
            }
            else
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.", UtilBO.TipoMensagem.Alerta);
                updMensagem.Update();
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            RedirecionarPagina("Busca.aspx");
        }

        protected void grvResultados_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ACA_AlunoBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(grvResultados);
        }

        protected void grvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvResultados.PageIndex = e.NewPageIndex;
            grvResultados.DataBind();
        }

        protected void grvResultados_DataBinding(object sender, EventArgs e)
        {
            if (grvResultados.DataSource == null)
            {
                grvResultados.DataSource = SelecionaDados();
            }
        }

        protected void grvResultados_Sorting(object sender, GridViewSortEventArgs e)
        {
            VS_SortDirection = SortDir(e.SortExpression);
            VS_Ordenacao = e.SortExpression;
            grvResultados.DataBind();
        }

        protected void grvResultados_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvResultados.EditIndex = e.NewEditIndex;
        }

        protected void btnGerarRelatorioCima_Click(object sender, EventArgs e)
        {
            GerarRelatório();
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            GerarRelatório();
        }

        protected void grvResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelecionar = (CheckBox)e.Row.FindControl("chkSelecionar");

                if (chkSelecionar != null)
                {
                    chkSelecionar.Attributes.Add("index", e.Row.RowIndex.ToString());

                    if ((_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(chkSelecionar.Attributes["alu_id"]))))
                    {
                        chkSelecionar.Checked = true;
                        e.Row.Style.Add("background", "#F8F7CB");
                    }
                    else
                    {
                        chkSelecionar.Checked = false;
                        e.Row.Style.Remove("background");
                    }
                }
            }
        }

        #endregion

    }
}