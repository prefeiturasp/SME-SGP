using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using DevExpress.XtraReports.UI;
using ReportNameDocumentos = MSTech.GestaoEscolar.BLL.ReportNameDocumentos;
using MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo;

namespace GestaoEscolar.Relatorios.RelatoriosCP.GraficoIndividualNotas
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Curso do GridView grvDocumentoAluno.
        /// </summary>
        protected const int cellCurso = 6;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula estadual do GridView grvDocumentoAluno.
        /// </summary>
        protected const int columnMatricula = 2;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula do GridView grvDocumentoAluno.
        /// </summary>
        protected const int columnMatriculaEstadual = 1;

        #endregion Constantes

        #region Propriedades

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

        /// <summary>
        /// Indica se existe pesquisa realizada.
        /// </summary>
        private Boolean _VS_PesquisaSalva
        {
            get
            {
                return (Boolean)ViewState["_VS_PesquisaSalva"];
            }
            set
            {
                ViewState["_VS_PesquisaSalva"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosAluno)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosAluno)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                    }
                }

                return SortDirection.Ascending;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocumentosAluno.js"));
            }

            UCBuscaDocenteTurma._VS_AnosAnteriores = true;
            UCBuscaDocenteTurma.IndexChanged_Calendario += UCBuscaDocenteTurma_IndexChanged_Calendario;
            UCBuscaDocenteTurma.IndexChanged_Turma += UCBuscaDocenteTurma_IndexChanged_Turma;

            //btnGerarRelatorio.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            //btnGerarRelatorioCima.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            if (!IsPostBack)
            {
                _VS_PesquisaSalva = false;
                //_VS_SelecionarTodos = false;

                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        updPesquisa.Visible = false;
                        lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }

                    // Inserção do título do relatório
                    lgdTitulo.InnerText = Modulo.mod_nome;

                    Inicializar();

                    Page.Form.DefaultButton = btnPesquisar.UniqueID;

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

                    // ******************************

                    // Carrega o nome referente ao parametro de matricula estadual.
                    string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                    grvDocumentoAluno.Columns[columnMatricula].Visible = !mostraMatriculaEstadual;
                    grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = mostraMatriculaEstadual;
                    grvDocumentoAluno.Columns[columnMatriculaEstadual].HeaderText = nomeMatriculaEstadual;

                    UCCamposBuscaAluno1.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                    UCCamposBuscaAluno1.TituloMatriculaEstadual = nomeMatriculaEstadual;

                    grvDocumentoAluno.Columns[cellCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    //fdsResultados.Style.Add("display", "none");
                    fdsResultados.Visible = false;

                    CarregaBusca();

                    // adiciona atributo que controla se estao todas as paginas selecionavas
                    //divSeleciona.Style.Add("display", "inline");
                    //hdnSelecionaGrid.Value = "false";
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Page Life Cycle

        #region Métodos

        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        protected void Inicializar()
        {
            UCBuscaDocenteTurma._VS_MostarComboEscola = false;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio &= UCBuscaDocenteTurma._VS_doc_id > 0;
            UCCPeriodoCalendario.MostrarMensagemSelecione = true;
            UCCPeriodoCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioUA = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = true;
            UCBuscaDocenteTurma.ComboCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
            UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
            divPeriodoCalendario.Visible = false;
            UCBuscaDocenteTurma._VS_CarregarApenasTurmasNormais = true;

            UCBuscaDocenteTurma.Visible =
            UCCamposObrigatorios.Visible = true;

            UCBuscaDocenteTurma.InicializaCamposBusca();
            UCBuscaDocenteTurma.ComboEscola.FocusUA();
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        protected void CarregaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.GraficoIndividualNotasConceito)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2, valor3;
                long doc_id = -1;

                UCBuscaDocenteTurma.ComboEscola.Inicializar();

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    UCBuscaDocenteTurma.ComboEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID = new Guid(valor);
                        UCBuscaDocenteTurma.ComboEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCBuscaDocenteTurma.ComboEscola.Uad_ID != Guid.Empty)
                        {
                            UCBuscaDocenteTurma.ComboEscola.FocoEscolas = true;
                            UCBuscaDocenteTurma.ComboEscola.PermiteAlterarCombos = true;
                        }

                        string esc_id;
                        string uni_id;

                        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                        {
                            UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                            UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                        }
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCBuscaDocenteTurma.ComboCalendario.Valor = Convert.ToInt32(valor);
                UCBuscaDocenteTurma.UCCCalendario_IndexChanged();
                UCBuscaDocenteTurma_IndexChanged_Calendario();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                UCBuscaDocenteTurma.ComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                UCBuscaDocenteTurma.UCCCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                if (doc_id <= 0)
                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor = new[] { UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0], UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                else
                    UCBuscaDocenteTurma._VS_doc_id = doc_id;
                UCBuscaDocenteTurma.UCComboCurriculoPeriodo__OnSelectedIndexChange();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
                UCBuscaDocenteTurma.ComboTurma.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };
                UCBuscaDocenteTurma_IndexChanged_Turma();

                if (divPeriodoCalendario.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor2);
                    UCCPeriodoCalendario.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
                UCCamposBuscaAluno1.TipoBuscaNomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
                UCCamposBuscaAluno1.NomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_dataNascimento", out valor);
                UCCamposBuscaAluno1.DataNascAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nomeMae", out valor);
                UCCamposBuscaAluno1.NomeMaeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
                UCCamposBuscaAluno1.MatriculaAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matriculaEstadual", out valor);
                UCCamposBuscaAluno1.MatriculaEstadualAluno = valor;

                if (btnPesquisar.Visible)
                    Pesquisar(0);

                updPesquisa.Update();
            }
        }

        /// <summary>
        /// Salva os dados da busca realizada para ser carregada posteriormente.
        /// </summary>
        protected void SalvarBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCBuscaDocenteTurma.ComboEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCBuscaDocenteTurma.ComboEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCBuscaDocenteTurma.ComboEscola.Uni_ID.ToString());
            filtros.Add("cur_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0].ToString());
            filtros.Add("crr_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1].ToString());
            filtros.Add("crp_id", UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2].ToString());
            filtros.Add("cal_id", UCBuscaDocenteTurma.ComboCalendario.Valor.ToString());
            filtros.Add("tur_id", UCBuscaDocenteTurma.ComboTurma.Valor[0].ToString());
            filtros.Add("crp_idTurma", UCBuscaDocenteTurma.ComboTurma.Valor[1].ToString());
            filtros.Add("ttn_id", UCBuscaDocenteTurma.ComboTurma.Valor[2].ToString());

            if (divPeriodoCalendario.Visible)
            {
                filtros.Add("tpc_id", UCCPeriodoCalendario.Valor[0].ToString());
                filtros.Add("cap_id", UCCPeriodoCalendario.Valor[1].ToString());
            }

            filtros.Add("tipoBusca", UCCamposBuscaAluno1.TipoBuscaNomeAluno);
            filtros.Add("pes_nome", UCCamposBuscaAluno1.NomeAluno);
            filtros.Add("pes_dataNascimento", UCCamposBuscaAluno1.DataNascAluno);
            filtros.Add("pes_nomeMae", UCCamposBuscaAluno1.NomeMaeAluno);
            filtros.Add("alc_matricula", UCCamposBuscaAluno1.MatriculaAluno);
            filtros.Add("alc_matriculaEstadual", UCCamposBuscaAluno1.MatriculaEstadualAluno);

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.GraficoIndividualNotasConceito, Filtros = filtros };
        }

        /// <summary>
        /// Pesquisa os alunos de acordo com os filtros de busca definidos.
        /// </summary>
        protected void Pesquisar(int pageIndex)
        {
            ACA_AlunoBO.numeroCursosPeja = 0;

            int qtdeLinhasPorPagina = Convert.ToInt32(ddlQtPaginado.SelectedValue);

            grvDocumentoAluno.DataSource = ACA_AlunoBO.BuscaAlunos_Documentos_GraficoIndividualNotas
                    (
                        UCBuscaDocenteTurma.ComboCalendario.Valor,
                        UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                        UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                        UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                        UCBuscaDocenteTurma.ComboTurma.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                        UCCamposBuscaAluno1.NomeMaeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        UCCamposBuscaAluno1.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        qtdeLinhasPorPagina,
                        pageIndex,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        false
                    );

            // atribui essa quantidade para o grid
            grvDocumentoAluno.PageSize = qtdeLinhasPorPagina;
            grvDocumentoAluno.PageIndex = pageIndex;
            grvDocumentoAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

            //fdsResultados.Style.Remove("display");
            fdsResultados.Visible = true;

            grvDocumentoAluno.DataBind();
            chkTodos.Visible = !grvDocumentoAluno.Rows.Count.Equals(0);

            divQtdPaginacao.Visible = grvDocumentoAluno.Rows.Count > 0;
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarBusca();

                __SessionWEB.PostMessages = String.Empty;
                string esc_ids = String.Empty;
                string alu_ids_boletim = String.Empty;

                // Alu_ids não ordenado pelo id, e sim pelo nome
                string alu_ids_boletim_nao_ordenado = String.Empty;

                _VS_AlunosSelecionados.Clear();

                //bool bTodasPaginas = false;
                //Boolean.TryParse(hdnSelecionaGrid.Value, out bTodasPaginas);

                if (chkTodos.Checked)
                {
                    DataTable dtAlunos = ACA_AlunoBO.BuscaAlunos_Documentos_GraficoIndividualNotas
                    (
                        UCBuscaDocenteTurma.ComboCalendario.Valor,
                        UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                        UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                        UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                        UCBuscaDocenteTurma.ComboTurma.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                        UCCamposBuscaAluno1.NomeMaeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        UCCamposBuscaAluno1.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        0,
                        0,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        false
                    );

                    foreach (DataRow row in dtAlunos.Rows)
                    {
                        alu_ids_boletim_nao_ordenado = (String.IsNullOrEmpty(alu_ids_boletim_nao_ordenado) ? "" : alu_ids_boletim_nao_ordenado + ",") + row["alu_id"];

                        if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(row["alu_id"])))
                        {
                            esc_ids = !String.IsNullOrEmpty(esc_ids) ? String.Concat(esc_ids + ',', row["esc_id"].ToString()) : row["esc_id"].ToString();
                            _VS_AlunosSelecionados.Add(Convert.ToInt64(row["alu_id"]), true);
                        }
                    }
                }
                else // se só a pagina atual tem selecionados corre as linhas do grid pegando os alu_ids atuais
                {
                    foreach (GridViewRow row in grvDocumentoAluno.Rows)
                    {
                        CheckBox chkSelecionar = (CheckBox)row.FindControl("chkSelecionar");

                        if (chkSelecionar.Checked)
                        {
                            alu_ids_boletim_nao_ordenado = (String.IsNullOrEmpty(alu_ids_boletim_nao_ordenado) ? "" : alu_ids_boletim_nao_ordenado + ",") + grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"];

                            if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"])))
                            {
                                _VS_AlunosSelecionados.Add(Convert.ToInt64(grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"]), true);
                                esc_ids = !String.IsNullOrEmpty(esc_ids) ? String.Concat(esc_ids + ',', Convert.ToString(grvDocumentoAluno.DataKeys[row.RowIndex].Values["esc_id"])) : Convert.ToString(grvDocumentoAluno.DataKeys[row.RowIndex].Values["esc_id"]);
                            }
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

                    string report, parameter;
                    parameter = string.Empty;
                    report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.GraficoIndividualNotaComponente).ToString();
                    XtraReport DevReport = null;
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                    DevReport = new RelGrafIndividualNotaComponente
                                (UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                UCBuscaDocenteTurma.ComboCalendario.Valor,
                                UCCPeriodoCalendario.Valor[0],
                                UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                                UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                                UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                                alu_ids,
                                __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                                GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                                ApplicationWEB.LogoRelatorioDB);

                    GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                    Response.Redirect(String.Format("~/Documentos/RelatorioDev.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Selecione pelo menos um aluno para gerar documento.", UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCCamposBuscaAluno1.IsValid)
                {
                    SalvarBusca();
                    Pesquisar(0);
                }
                else
                    throw new ValidationException("Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.");
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!grvDocumentoAluno.Rows.Count.Equals(0))
                {
                    Pesquisar(0);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvDocumentoAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Pesquisar(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvDocumentoAluno_DataBound(object sender, EventArgs e)
        {
            chkTodos.Checked = false;

            UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);
        }

        protected void grvDocumentoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkTodos.Attributes.Remove("todososcursospeja");
                    chkTodos.Attributes.Add("todososcursospeja", (ACA_AlunoBO.numeroCursosPeja == ACA_AlunoBO.GetTotalRecords() ? "1" : "0"));
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelecionar = (CheckBox)e.Row.FindControl("chkSelecionar");

                    if (chkSelecionar != null)
                    {
                        chkSelecionar.Attributes.Add("index", e.Row.RowIndex.ToString());

                        if (_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(chkSelecionar.Attributes["alu_id"])))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grvDocumentoAluno_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                SortDirection sortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = e.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", e.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = sortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", sortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.DocumentosAluno
                    ,
                    Filtros = filtros
                };
            }

            try
            {
                Pesquisar(grid.PageIndex);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

        #region Delegates

        private void UCBuscaDocenteTurma_IndexChanged_Calendario()
        {     
            try
            {
                UCCPeriodoCalendario.CarregarPorCalendario(-1);
                if (UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                {
                    UCCPeriodoCalendario.CarregarPorCalendario(UCBuscaDocenteTurma.ComboCalendario.Valor);
                    UCCPeriodoCalendario.SetarFoco();
                    divPeriodoCalendario.Visible = UCCPeriodoCalendario.PermiteEditar = true;
                }
                else
                    divPeriodoCalendario.Visible = UCCPeriodoCalendario.PermiteEditar = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCBuscaDocenteTurma_IndexChanged_Turma()
        {
            try
            {
                UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                UCCPeriodoCalendario.PermiteEditar = false;

                if (UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[1] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[2] > 0)
                {
                    UCCPeriodoCalendario.CarregarPorTurma(UCBuscaDocenteTurma.ComboTurma.Valor[0]);

                    UCCPeriodoCalendario.SetarFoco();
                    UCCPeriodoCalendario.PermiteEditar = UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0;
                }
                else if (UCCPeriodoCalendario.Obrigatorio && UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                {
                    UCCPeriodoCalendario.CarregarPorCalendario(UCBuscaDocenteTurma.ComboCalendario.Valor);
                    UCCPeriodoCalendario.SetarFoco();

                    if (UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0)
                        UCCPeriodoCalendario.PermiteEditar = true;
                    else
                    {
                        UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                        UCCPeriodoCalendario.PermiteEditar = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

        }

        #endregion Delegates
    }
}