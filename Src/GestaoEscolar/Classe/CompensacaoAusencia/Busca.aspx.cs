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

namespace GestaoEscolar.Classe.CompensacaoAusencia
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        private const int gvCompAusenciaIndiceAlterar = 3;
        private const int gvCompAusenciaIndiceExcluir = 4;

        #endregion

        #region Propriedades

        /// <summary>
        /// Salva o valor de cpa_id quando edicao.
        /// </summary>
        /// <value>
        /// edit_atg_id.
        /// </value>
        public long[] Edit_cpa_id
        {
            get
            {
                return new long[] { Convert.ToInt64(gvCompAusencia.DataKeys[gvCompAusencia.EditIndex].Values[0] ?? 0),
                                    Convert.ToInt64(gvCompAusencia.DataKeys[gvCompAusencia.EditIndex].Values[1] ?? 0) };
            }
        }

        /// <summary>
        /// Guarda os filtros
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CompensacaoAusencia)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de doc_id
        /// </summary>
        private long _VS_doc_id
        {
            get
            {
                if (ViewState["_VS_doc_id"] != null)
                    return Convert.ToInt64(ViewState["_VS_doc_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_doc_id"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CompensacaoAusencia)
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

        /// <summary>
        /// Flag que indica se a disciplina é oferecia para alunos de libras.
        /// </summary>
        private bool VS_DisciplinaEspecial
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_DisciplinaEspecial"] ?? false);
            }

            set
            {
                ViewState["VS_DisciplinaEspecial"] = value;
            }
        }

        /// <summary>
        /// Guarda em viewstate se na disciplina que está sendo pesquisada o docente possui atribuição ativa.
        /// Se não permitir, fica em modo consulta a tela.
        /// </summary>
        private bool VS_AtribuicaoAtiva
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_AtribuicaoAtiva"] ?? false);
            }
            set
            {
                ViewState["VS_AtribuicaoAtiva"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible);

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo calendário
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (_VS_doc_id > 0)
                {
                    if (UCComboUAEscola.Esc_ID > 0)
                    {
                        ddlTurma.Enabled = true;

                        UCComboCalendario.CarregarCalendarioAnual();

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;
                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;

                        UCCTurmaDisciplina1.Valor = -1;
                        UCCTurmaDisciplina1.PermiteEditar = false;

                        UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                        UCCPeriodoCalendario.PermiteEditar = false;

                        UCComboCalendario.Valor = -1;
                        UCComboCalendario.PermiteEditar = false;
                    }
                }
                else
                {
                    UCComboCalendario.Valor = -1;
                    UCComboCalendario.PermiteEditar = false;

                    if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        UCComboCalendario.CarregarCalendarioAnual();

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;
                    }

                    UCComboCalendario_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo calendario e trata o combo Curso curriculo.
        /// </summary>
        protected void UCComboCalendario_IndexChanged()
        {
            try
            {
                if (_VS_doc_id > 0)
                {
                    if (UCComboCalendario.Valor > 0)
                    {
                        UCComboCalendario.PermiteEditar = true;
                        ddlTurma.Enabled = true;
                        InicializaCamposBuscaVisaoIndividual(UCComboUAEscola.Esc_ID);
                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;

                        UCCTurmaDisciplina1.Valor = -1;
                        UCCTurmaDisciplina1.PermiteEditar = false;

                        UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                        UCCPeriodoCalendario.PermiteEditar = false;
                    }
                }
                else
                {

                    UCCCursoCurriculo.Valor = new[] { -1, -1 };
                    UCCCursoCurriculo.PermiteEditar = false;

                    if (UCComboCalendario.Valor > 0 && UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        // Permite carregar cursos ativos ou encerrados (turmas histórico).
                        UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario.Valor, 0);

                        UCCCursoCurriculo.SetarFoco();
                        UCCCursoCurriculo.PermiteEditar = true;
                    }

                    UCCCursoCurriculo_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                ddlTurma.SelectedValue = "-1";
                ddlTurma.Enabled = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0 || _VS_doc_id > 0)
                {
                    ddlTurma.Items.Clear();
                    ddlTurma.DataTextField = "tur_codigo";

                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

                    ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                                          UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID,
                                                                                          UCComboCalendario.Valor, _VS_doc_id > 0 ? 0 : UCCCursoCurriculo.Valor[0],
                                                                                          _VS_doc_id > 0 ? 0 : UCCCursoCurriculo.Valor[1], -1,
                                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, 0,
                                                                                          ApplicationWEB.AppMinutosCacheLongo)
                                          .GroupBy(p => new { tur_id = p.tur_id, tur_codigo = p.tur_codigo }).Select(p => p.Key).ToList(); ;
                    ddlTurma.DataBind();

                    ddlTurma.Focus();
                    ddlTurma.Enabled = true;
                }

                this.ddlTurma_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboTurmaDisciplina_IndexChanged()
        {
            try
            {
                UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                UCCPeriodoCalendario.PermiteEditar = false;

                if (UCCTurmaDisciplina1.Valor > -1)
                {
                    long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);

                    TUR_Turma entTur = new TUR_Turma { tur_id = tur_id };
                    TUR_TurmaBO.GetEntity(entTur);

                    UCCPeriodoCalendario.CarregarPorCalendario(entTur.cal_id);

                    UCCPeriodoCalendario.SetarFoco();
                    UCCPeriodoCalendario.PermiteEditar = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Trata o numero de linhas por pagina da grid.
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            gvCompAusencia.PageSize = UCComboQtdePaginacao1.Valor;
            gvCompAusencia.PageIndex = 0;
            // atualiza o grid
            gvCompAusencia.DataBind();
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaCamposBusca()
        {
            //Carrega os campos
            UCComboUAEscola.FiltroEscolasControladas = true;
            UCComboUAEscola.Inicializar();

            //if (UCComboUAEscola.VisibleUA)
            UCComboUAEscola_IndexChangedUA();
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca para visão individual
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        private void InicializaCamposBuscaVisaoIndividual(int esc_id)
        {
            //Carrega os campos
            int posicaoDocenteCompatilhado = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.POSICAO_DOCENCIA_COMPARTILHADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlTurma.Items.Clear();
            ddlTurma.DataTextField = "tur_esc_nome";

            ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

            ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Docente_TodosTipos_Posicao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_doc_id, posicaoDocenteCompatilhado, esc_id, UCComboCalendario.Valor, false, false, ApplicationWEB.AppMinutosCacheLongo)
                                  .GroupBy(p => new { tur_id = p.tur_id, tur_esc_nome = p.tur_esc_nome }).Select(p => p.Key).ToList();
            ddlTurma.DataBind();

            //if (UCComboUAEscola.VisibleUA)
            ddlTurma_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Realiza a consulta pelos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                bool permiteConsultar = true;
                List<sPermissaoDocente> ltPermissao = new List<sPermissaoDocente>();
                EnumTipoDocente tipoDocente = EnumTipoDocente.Titular;
                if (_VS_doc_id > 0)
                {
                    long tud_id = UCCTurmaDisciplina1.Valor;
                    bool AtribuicaoAtiva;
                    // Traz a última atribuição que o docente teve naquela disciplina, sendo ativa ou inativa.
                    byte tdt_posicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma_ComInativos
                        (_VS_doc_id, tud_id, out AtribuicaoAtiva, ApplicationWEB.AppMinutosCacheLongo);
                    VS_AtribuicaoAtiva = AtribuicaoAtiva;
                    ltPermissao = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(tdt_posicao, (byte)EnumModuloPermissao.Compensacoes);
                    permiteConsultar = ltPermissao.Any(p => p.pdc_permissaoConsulta);

                    TUR_TurmaDisciplina entityTurmaDisciplina = new TUR_TurmaDisciplina { tud_id = tud_id };
                    TUR_TurmaDisciplinaBO.GetEntity(entityTurmaDisciplina);
                    VS_DisciplinaEspecial = entityTurmaDisciplina.tud_disciplinaEspecial;

                    tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);
                }
                else
                {
                    VS_AtribuicaoAtiva = true;
                }

                if (permiteConsultar)
                {
                    gvCompAusencia.EmptyDataText = GetGlobalResourceObject("Classe", "CompensacaoAusencia.Busca.SemCompensacaoAusencia").ToString();
                    gvCompAusencia.PageIndex = 0;
                    odsCompAusencia.SelectMethod = _VS_doc_id > 0 && VS_DisciplinaEspecial ? "SelectByPesquisaFiltroDeficiencia" : "SelectByPesquisa";
                    odsCompAusencia.SelectParameters.Clear();
                    odsCompAusencia.SelectParameters.Add("uad_idSuperior", DbType.Guid, UCComboUAEscola.Uad_ID.ToString());
                    odsCompAusencia.SelectParameters.Add("esc_id", DbType.Int32, UCComboUAEscola.Esc_ID.ToString());
                    odsCompAusencia.SelectParameters.Add("uni_id", DbType.Int32, UCComboUAEscola.Uni_ID.ToString());
                    odsCompAusencia.SelectParameters.Add("cur_id", DbType.Int32, UCCCursoCurriculo.Valor[0].ToString());
                    odsCompAusencia.SelectParameters.Add("crr_id", DbType.Int32, UCCCursoCurriculo.Valor[1].ToString());
                    odsCompAusencia.SelectParameters.Add("cap_id", DbType.Int32, UCCPeriodoCalendario.Valor[1].ToString());
                    odsCompAusencia.SelectParameters.Add("tud_id", DbType.Int64, UCCTurmaDisciplina1.Valor.ToString());
                    odsCompAusencia.SelectParameters.Add("tur_id", DbType.Int64, ddlTurma.SelectedValue);

                    // Filtra pela visão do usuário.
                    odsCompAusencia.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                    odsCompAusencia.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                    odsCompAusencia.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual).ToString());
                    odsCompAusencia.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                    if (_VS_doc_id > 0 && VS_DisciplinaEspecial)
                    {
                        odsCompAusencia.SelectParameters.Add("tipoDocente", tipoDocente.ToString());
                    }

                    odsCompAusencia.DataBind();

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                    gvCompAusencia.Sort(VS_Ordenacao, VS_SortDirection);

                    #region Salvar busca realizada com os parâmetros do ODS.

                    foreach (Parameter param in odsCompAusencia.SelectParameters)
                    {
                        filtros.Add(param.Name, param.DefaultValue);
                    }

                    filtros.Add("tpc_id", UCCPeriodoCalendario.Valor[0].ToString());
                    filtros.Add("cal_id", UCComboCalendario.Valor.ToString());

                    __SessionWEB.BuscaRealizada = new BuscaGestao
                    {
                        PaginaBusca = PaginaGestao.CompensacaoAusencia
                        ,
                        Filtros = filtros
                    };

                    #endregion

                    // mostra essa quantidade no combobox
                    UCComboQtdePaginacao1.Valor = itensPagina;
                    // atribui essa quantidade para o grid
                    gvCompAusencia.PageSize = itensPagina;
                    // atualiza o grid
                    gvCompAusencia.DataBind();

                    fdsResultados.Visible = true;

                    if (_VS_doc_id > 0)
                    {
                        gvCompAusencia.Columns[gvCompAusenciaIndiceAlterar].Visible =
                        gvCompAusencia.Columns[gvCompAusenciaIndiceExcluir].Visible = ltPermissao.Any(p => p.pdc_permissaoEdicao);
                    }
                    else
                    {
                        gvCompAusencia.Columns[gvCompAusenciaIndiceAlterar].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        gvCompAusencia.Columns[gvCompAusenciaIndiceExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    }
                }
                else
                {
                    string msg = String.Format("O docente não possui permissão para consultar compensações de ausência do(a) {0} selecionado(a).", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_MIN"));
                    lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as compensações de ausências.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CompensacaoAusencia)
            {
                // Recuperar busca realizada e pesquisar automaticamente

                string valor, valor2;

                if (_VS_doc_id <= 0)
                {
                    //CRE
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    UCComboUAEscola.Uad_ID = new Guid(valor);
                    UCComboUAEscola_IndexChangedUA();
                }

                //Escola
                UCComboUAEscola.MostraApenasAtivas = true;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2);
                UCComboUAEscola.SelectedValueEscolas = new int[2] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                UCComboUAEscola_IndexChangedUnidadeEscola();

                //Calendario
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCComboCalendario.Valor = Convert.ToInt32(valor);
                UCComboCalendario_IndexChanged();

                if (_VS_doc_id <= 0)
                {
                    //Etapa de ensino
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                    UCCCursoCurriculo.Valor = new int[2] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                    UCCCursoCurriculo_IndexChanged();
                }

                //Turma
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);

                ddlTurma.SelectedValue = valor;
                ddlTurma_SelectedIndexChanged(null, null);

                //Disciplina
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tud_id", out valor);
                UCCTurmaDisciplina1.Valor = Convert.ToInt64(valor);
                UCComboTurmaDisciplina_IndexChanged();

                //Periodo Calendario
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor2);

                UCCPeriodoCalendario.Valor = new int[2] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };

                Pesquisar();
            }
            else
            {
                fdsResultados.Visible = false;
            }
        }

        #region Métodos usados no grid

        /// <summary>
        /// Concatena a string recebida entre parenteses com "+"
        /// </summary>
        /// <param name="numero">The numero.</param>
        /// <returns></returns>
        public string ConcatenaNumeroAlunos(string numero)
        {
            return String.Concat("( +", numero, ")");
        }

        /// <summary>
        /// Se a atividade tiver mais de 100 caracteres retorna os 100 primeiros caracteres da string de atividade com "..."
        /// </summary>
        /// <param name="atividade">Parametro atividade.</param>
        /// <returns></returns>
        public string InsereRetColunaAtividades(string atividade)
        {
            if (atividade.Length > 100)
                return String.Concat(atividade.Substring(0, 100), "...");

            return atividade;
        }

        #endregion Métodos usados no grid

        /// <summary>
        /// Carregars the ausencias.
        /// </summary>
        /// <param name="cpa_id">Parametro cpa_id.</param>
        /// <param name="tud_id">Parametro tud_id.</param>
        /// <param name="tpc_id">Parametro tpc_id.</param>
        private void CarregarAusencias(int cpa_id, long tud_id, int tpc_id)
        {
            try
            {
                odsQtdeAlunosCompensados.SelectParameters.Clear();
                odsQtdeAlunosCompensados.SelectParameters.Add("tud_id", DbType.Int64, tud_id.ToString());
                odsQtdeAlunosCompensados.SelectParameters.Add("tpc_id", DbType.Int32, tpc_id.ToString());
                odsQtdeAlunosCompensados.SelectParameters.Add("cpa_id", DbType.Int32, cpa_id.ToString());
                odsQtdeAlunosCompensados.SelectParameters.Add("doc_id", DbType.Int64, _VS_doc_id.ToString());
                odsQtdeAlunosCompensados.SelectParameters.Add("documentoOficial", "false");
                grvCompensacaoAulas.DataBind();

                fdsCompensacaoAusencia.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar alunos com compensações de ausências.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                upnCompensacao.Update();
            }
        }

        #endregion Métodos

        #region Eventos

        /// <summary>
        /// Load da pagina
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaCompensacaoAusencia.js"));
            }

            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
            UCComboCalendario.IndexChanged += UCComboCalendario_IndexChanged;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCTurmaDisciplina1.IndexChanged += UCComboTurmaDisciplina_IndexChanged;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                gvCompAusencia.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsEscola.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsEscola.ClientID)), true);
                    }

                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    {
                        // Busca o doc_id do usuário logado.
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                        {
                            //Seta o docente
                            _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                            //Esconde os campos não visíveis para docentes
                            UCCCursoCurriculo.Visible = false;
                            ddlTurma.Enabled = false;

                            //Carrega as escolas no combo
                            UCComboUAEscola.InicializarVisaoIndividual(_VS_doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 3);

                            if (UCComboUAEscola.QuantidadeItemsComboEscolas == 2)
                            {
                                ddlTurma.Enabled = true;
                                InicializaCamposBuscaVisaoIndividual(UCComboUAEscola.Esc_ID);
                            }
                            else
                                InicializaCamposBuscaVisaoIndividual(0);
                        }
                        else
                        {
                            divPesquisa.Visible = false;
                            lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                        }
                    }
                    else
                    {
                        //Inicializa os campos de busca
                        InicializaCamposBusca();
                    }

                    //Carrega uma busca salva na memoria
                    VerificaBusca();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = UCComboUAEscola.ComboUA_ClientID;

                // Permissões da pagina
                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// Limpa os campos de pesquisa
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("~/Classe/CompensacaoAusencia/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Chama tela cadastro para insercao de nova compensação de ausencia.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Classe/CompensacaoAusencia/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Chama metodo Pesquisa para buscar valores
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Pesquisar();
        }

        /// <summary>
        /// Controle do databound da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvCompAusencia_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CLS_CompensacaoAusenciaBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(gvCompAusencia);

            if ((!string.IsNullOrEmpty(gvCompAusencia.SortExpression)) &&
                (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CompensacaoAusencia))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = gvCompAusencia.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", gvCompAusencia.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = gvCompAusencia.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", gvCompAusencia.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.CompensacaoAusencia
                    ,
                    Filtros = filtros
                };
            }
        }

        /// <summary>
        /// Controle do row databound da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvCompAusencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int periodoAberto = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "periodoAberto"));

                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && periodoAberto > 0
                        && VS_AtribuicaoAtiva;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnAlterar = (ImageButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && periodoAberto > 0
                        && VS_AtribuicaoAtiva;
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnDetalharCompensacao = (ImageButton)e.Row.FindControl("btnDetalharCompensacao");
                if (btnDetalharCompensacao != null)
                {
                    btnDetalharCompensacao.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        /// <summary>
        /// Controle do rowcommand da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvCompAusencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int cpa_id = Convert.ToInt32(gvCompAusencia.DataKeys[index].Values[0]);
                    long tud_id = Convert.ToInt32(gvCompAusencia.DataKeys[index].Values[1]);

                    CLS_CompensacaoAusencia entity = new CLS_CompensacaoAusencia
                    {
                        cpa_id = cpa_id,
                        tud_id = tud_id
                    };
                    CLS_CompensacaoAusenciaBO.GetEntity(entity);

                    if (CLS_CompensacaoAusenciaBO.Delete(entity))
                    {
                        gvCompAusencia.PageIndex = 0;
                        gvCompAusencia.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "cpa_id: " + cpa_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Compensação de ausência excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir compensação de ausência.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "DetalharCompensacao")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int cpa_id = Convert.ToInt32(gvCompAusencia.DataKeys[index].Values[0]);
                    long tud_id = Convert.ToInt32(gvCompAusencia.DataKeys[index].Values[1]);
                    int tpc_id = Convert.ToInt32(gvCompAusencia.DataKeys[index].Values[2]);

                    if (cpa_id > 0 && tud_id > 0)
                    {
                        CarregarAusencias(cpa_id, tud_id, tpc_id);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CompensacaoDetalhes", "$(document).ready(function() { $('#divCompensacaoDetalhes').dialog('open'); });", true);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar detalhes de compensação de ausência.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        /// <summary>
        /// Carrega as disciplinas da turma
        /// </summary>
        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);

                if (_VS_doc_id <= 0)
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id);
                else
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id, _VS_doc_id);

                UCCTurmaDisciplina1.PermiteEditar = false;

                if (tur_id > 0)
                {
                    UCCTurmaDisciplina1.SetarFoco();
                    UCCTurmaDisciplina1.PermiteEditar = true;
                }

                UCComboTurmaDisciplina_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}