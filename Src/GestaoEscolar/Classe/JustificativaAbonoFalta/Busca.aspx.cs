using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Classe.JustificativaAbonoFalta
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Curso do GridView grvAluno.
        /// </summary>
        protected const int cellCurso = 5;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula estadual do GridView grvAluno.
        /// </summary>
        protected const int columnMatricula = 1;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula do GridView grvAluno.
        /// </summary>
        protected const int columnMatriculaEstadual = 0;

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// Id do aluno selecionado no grid.
        /// </summary>
        public long EditItem_AluId
        {
            get
            {
                return Convert.ToInt64(grvAluno.DataKeys[grvAluno.EditIndex].Values["alu_id"] ?? 0);
            }
        }

        /// <summary>
        /// Id da matrícula do aluno selecionado no grid.
        /// </summary>
        public int EditItem_MtuId
        {
            get
            {
                return Convert.ToInt32(grvAluno.DataKeys[grvAluno.EditIndex].Values["mtu_id"] ?? 0);
            }
        }

        /// <summary>
        /// Id da disciplina selecionada no combo.
        /// </summary>
        public long EditItem_TudId
        {
            get
            {
                return UCComboTurmaDisciplina.Valor;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaAbonoFalta)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaAbonoFalta)
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
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                }

                if (!IsPostBack)
                {
                    InicializarTela();
                    CarregarBusca();
                }

                UCBuscaDocenteTurma.IndexChanged_Turma += UCBuscaDocenteTurma_IndexChanged_Turma;
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                string message = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(message))
                    lblMessage.Text = message;
                // Inserção do título do relatório
                pnlBusca.GroupingText = Modulo.mod_nome;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            //Se for usuário administrador ou gestor permite gerar o relatório sem filtrar por escola
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao ||
                __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
            {
                UCBuscaDocenteTurma._VS_PermiteSemEscola = true;
                UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = false;
            }

            UCBuscaDocenteTurma._VS_AnosAnteriores = true;
            UCBuscaDocenteTurma._VS_MostarComboEscola = (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual);
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioUA = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = true;
            UCBuscaDocenteTurma.ComboCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
            UCComboTurmaDisciplina.Obrigatorio = true;

            UCBuscaDocenteTurma.InicializaCamposBusca();

            UCBuscaDocenteTurma.ComboEscola.FocusUA();

            UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
            UCComboTurmaDisciplina.VS_MostraRegencia = true;

            // Carrega o nome referente ao parametro de matricula estadual.
            string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

            grvAluno.Columns[columnMatricula].Visible = !mostraMatriculaEstadual;
            grvAluno.Columns[columnMatriculaEstadual].Visible = mostraMatriculaEstadual;
            grvAluno.Columns[columnMatriculaEstadual].HeaderText = nomeMatriculaEstadual;

            UCCamposBuscaAluno1.MostrarMatriculaEstadual = mostraMatriculaEstadual;
            UCCamposBuscaAluno1.TituloMatriculaEstadual = nomeMatriculaEstadual;

            grvAluno.Columns[cellCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        /// <summary>
        /// Método para salvar os filtros última busca realizada
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
            filtros.Add("tud_id", UCComboTurmaDisciplina.Valor.ToString());
            filtros.Add("tipoBusca", UCCamposBuscaAluno1.TipoBuscaNomeAluno);
            filtros.Add("pes_nome", UCCamposBuscaAluno1.NomeAluno);
            filtros.Add("pes_dataNascimento", UCCamposBuscaAluno1.DataNascAluno);
            filtros.Add("pes_nomeMae", UCCamposBuscaAluno1.NomeMaeAluno);
            filtros.Add("alc_matricula", UCCamposBuscaAluno1.MatriculaAluno);
            filtros.Add("alc_matriculaEstadual", UCCamposBuscaAluno1.MatriculaEstadualAluno);

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.JustificativaAbonoFalta, Filtros = filtros };
        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaAbonoFalta)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2, valor3;

                long doc_id = -1;

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
                    UCBuscaDocenteTurma.ComboEscola.Inicializar();

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

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tud_id", out valor);
                UCComboTurmaDisciplina.Valor = Convert.ToInt64(valor);
                
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

                updFiltros.Update();
            }
        }

        /// <summary>
        /// Pesquisa os alunos de acordo com os filtros de busca definidos.
        /// </summary>
        protected void Pesquisar(int pageIndex)
        {
            ACA_AlunoBO.numeroCursosPeja = 0;

            int qtdeLinhasPorPagina = Convert.ToInt32(ddlQtPaginado.SelectedValue);

            grvAluno.DataSource = ACA_AlunoBO.BuscaAlunos_JustificativaAbonoFalta
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
                        VS_Ordenacao
                    );

            // atribui essa quantidade para o grid
            grvAluno.PageSize = qtdeLinhasPorPagina;
            grvAluno.PageIndex = pageIndex;
            grvAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

            //fdsResultados.Style.Remove("display");
            fdsResultados.Visible = true;

            grvAluno.DataBind();

            divQtdPaginacao.Visible = grvAluno.Rows.Count > 0;
        }

        #endregion Métodos

        #region Eventos

        protected void ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!grvAluno.Rows.Count.Equals(0))
                {
                    Pesquisar(0);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Pesquisar(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvAluno, VS_Ordenacao, VS_SortDirection);
        }

        protected void grvAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnAbonoFalta = (ImageButton)e.Row.FindControl("btnAbonoFalta");
                    if (btnAbonoFalta != null)
                    {
                        btnAbonoFalta.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnAbonoFalta.CommandArgument = e.Row.RowIndex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_Sorting(object sender, GridViewSortEventArgs e)
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
                    PaginaBusca = PaginaGestao.JustificativaAbonoFalta
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
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
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
                    throw new ValidationException(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ValidacaoDataNascimento"));
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCBuscaDocenteTurma_IndexChanged_Turma()
        {
            try
            {
                UCComboTurmaDisciplina.Valor = -1;
                UCComboTurmaDisciplina.PermiteEditar = false;

                if (UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[1] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[2] > 0)
                {
                    if (UCBuscaDocenteTurma._VS_doc_id <= 0)
                        UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], true, 0, true);
                    else
                        UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id, 0, true);

                    UCComboTurmaDisciplina.SetarFoco();
                    UCComboTurmaDisciplina.PermiteEditar = UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0;
                }                
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }

        }

        protected void grvAluno_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvAluno.EditIndex = e.NewEditIndex;
        }

        #endregion Eventos
    }
}