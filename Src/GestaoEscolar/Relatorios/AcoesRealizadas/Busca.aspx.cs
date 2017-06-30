using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.AcoesRealizadas
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioNAAPA)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioNAAPA)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    if (filtros.ContainsKey("VS_Ordenacao"))
                    {
                        filtros["VS_Ordenacao"] = value;
                        __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioNAAPA, Filtros = filtros };
                    }
                }

            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioNAAPA)
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

            set
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioNAAPA)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    if (filtros.ContainsKey("VS_SortDirection"))
                    {
                        filtros["VS_SortDirection"] = value.ToString();
                        __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioNAAPA, Filtros = filtros };
                    }
                }

            }
        }

        /// <summary>
        /// Retorna o alu_id do registro que esta sendo editado.
        /// </summary>
        public long EditItemAluId
        {
            get
            {
                return Convert.ToInt64(grvResultados.DataKeys[grvResultados.EditIndex]["alu_id"]);
            }
        }


        /// <summary>
        /// Retorna o tur_id do registro que esta sendo editado.
        /// </summary>
        public long EditItemTurId
        {
            get
            {
                return Convert.ToInt64(grvResultados.DataKeys[grvResultados.EditIndex]["tur_id"]);
            }
        }

        /// <summary>
        /// Retorna o cal_id do registro que esta sendo editado.
        /// </summary>
        public int EditItemCalId
        {
            get
            {
                return Convert.ToInt32(grvResultados.DataKeys[grvResultados.EditIndex]["cal_id"]);
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
            UCCQtdePaginacao.IndexChanged += UCCQtdePaginacao_IndexChanged;

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsRelatorioAcoesRealizadas.js"));
            }

            if (!IsPostBack)
            {
                try
                {
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

                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(grvResultados);

                SelecionaDados();
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

        #endregion

        #region Métodos

        /// <summary>
        /// Retorna os dados de alunos
        /// </summary>
        /// <returns></returns>
        private void SelecionaDados()
        {
            DataTable dt = ACA_AlunoBO.BuscaAlunos_Documentos
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
                    UCCQtdePaginacao.Valor,
                    grvResultados.PageIndex,
                    (int)VS_SortDirection,
                    VS_Ordenacao,
                    false
                );

            grvResultados.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
            grvResultados.DataSource = dt;
            grvResultados.DataBind();
        }

        /// <summary>
        /// Salva a busca realizada
        /// </summary>
        private void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

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

            filtros.Add("VS_Ordenacao", VS_Ordenacao);
            filtros.Add("VS_SortDirection", VS_SortDirection.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioAcoesRealizadas, Filtros = filtros };
        }

        /// <summary>
        /// Realiza busca salva
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioNAAPA)
            {
                DateTime data;
                string valor, valor2, valor3;

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

            }

            if (UCCCalendario.Valor > 0)
            {
                Pesquisar();
            }
        }

        /// <summary>
        /// Realiza a pesquisa de alunos
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                pnlResultados.Visible = false;

                SalvaBusca();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                UCCQtdePaginacao.Valor = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                grvResultados.PageIndex = 0;
                grvResultados.PageSize = UCCQtdePaginacao.Valor;
                SelecionaDados();

                UCCQtdePaginacao.Visible = grvResultados.Rows.Count > 0;

                pnlResultados.Visible = true;

                updResultados.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Inicializa a tela
        /// </summary>
        private void InicializarTela()
        {
            pnlBusca.Visible = true;
            pnlResultados.Visible = false;
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

            UCCQtdePaginacao.GridViewRelacionado = grvResultados;

            updFiltros.Update();
            updResultados.Update();
        }

        /// <summary>
        /// Retorna a direção de ordenação da coluna.
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        private SortDirection SortDir(string sortExpression)
        {
            SortDirection sDir = SortDirection.Ascending;

            if (VS_Ordenacao == sortExpression)
                sDir = VS_SortDirection == SortDirection.Ascending
                    ? SortDirection.Descending
                    : SortDirection.Ascending;
            else
                VS_Ordenacao = sortExpression;

            VS_SortDirection = sDir == SortDirection.Ascending ? SortDirection.Ascending : SortDirection.Descending;

            return sDir;
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
            SelecionaDados();
        }

        protected void grvResultados_Sorting(object sender, GridViewSortEventArgs e)
        {
            VS_SortDirection = SortDir(e.SortExpression);
            VS_Ordenacao = e.SortExpression;
            SelecionaDados();
        }

        protected void grvResultados_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvResultados.EditIndex = e.NewEditIndex;
        }

        protected void btnGerarRelatrorioAcoesRealizadas_Click(object sender, EventArgs e)
        {
            string report, parametros;
            string alu_ids = String.Empty;
            string tur_ids = String.Empty;

            foreach (GridViewRow row in grvResultados.Rows)
            {
                CheckBox chkSelecionar = (CheckBox)row.FindControl("_chkSelecionar");
                if (chkSelecionar.Checked)
                {
                    alu_ids = (String.IsNullOrEmpty(alu_ids) ? "" : alu_ids + ",") + grvResultados.DataKeys[row.RowIndex].Values["alu_id"];
                    tur_ids = (String.IsNullOrEmpty(tur_ids) ? "" : tur_ids + ",") + grvResultados.DataKeys[row.RowIndex].Values["tur_id"];
                }
            }

           report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioAcoesRealizadas).ToString();
            parametros = "alu_ids=" + alu_ids +
                         "&tur_ids=" + tur_ids +
                         "&dre=" + UCCUAEscola.TextoComboUA +
                         "&escola=" + UCCUAEscola.TextoComboEscola +
                         "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria");
                         

            MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
        }


        #endregion
    }
}