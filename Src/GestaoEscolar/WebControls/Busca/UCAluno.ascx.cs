using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.Busca
{
    public partial class UCAluno : MotherUserControl
    {
        protected IDictionary<string, object> returns = new Dictionary<string, object>();

        #region Constantes

        private const int INDEX_COLUMN_DATA_NASCIMENTO = 1;
        private const int INDEX_COLUMN_NOME_MAE = 2;
        private const int INDEX_COLUMN_MATRICULA = 3;
        private const int INDEX_COLUMN_MATRICULA_ESTADUAL = 4;
        private const int INDEX_COLUMN_DATA_CRIACAO = 5;
        private const int INDEX_COLUMN_DATA_ALTERACAO = 6;
        private const int INDEX_COLUMN_NOME_ESCOLA = 7;
        private const int INDEX_COLUMN_CURSO = 8;
        private const int INDEX_COLUMN_PERIODO_CURSO = 9;
        private const int INDEX_COLUMN_CODIGO_TURMA = 10;

        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade que informa se o combo situacao deve ficar desabilitado,
        /// ao inves de escondido, quando a busca for para uma situacao específica.
        /// </summary>
        public bool DesabilitarSituacao
        {
            get;
            set;
        }

        /// <summary>
        /// Propriedade que informa se vai verificar a permissão do usuário na busca.
        /// </summary>
        public bool _VS_verificaPermissao
        {
            get
            {
                if (ViewState["_VS_verificaPermissao"] != null)
                    return Convert.ToBoolean(ViewState["_VS_verificaPermissao"]);
                return true;
            }
            set
            {
                ViewState["_VS_verificaPermissao"] = value;
            }
        }

        /// <summary>
        /// Propriedade que informa se a busca trará apenas alunos ativos.
        /// Por padrão traz alunos ativos e inativos.
        /// </summary>
        public bool BuscaSomenteAtivos
        {
            set
            {
                if (_ddlSituacao.Items.Count == 0)
                {
                    CarregaComboSituacaoAluno();
                }

                if (value)
                {
                    _ddlSituacao.SelectedValue = _ddlSituacao.Items[1].Value;
                    if (DesabilitarSituacao)
                    {
                        _ddlSituacao.Enabled = false;
                    }
                    else
                    {
                        _ddlSituacao.Visible = false;
                        _lblSituacao.Visible = false;
                    }
                    divLegenda.Visible = false;
                }
                else
                {
                    _ddlSituacao.SelectedValue = _ddlSituacao.Items[0].Value;
                    _ddlSituacao.Visible = true;
                    _lblSituacao.Visible = true;
                    divLegenda.Visible = true;
                }
            }
        }

        /// <summary>
        /// Propriedade que informa se a busca trará apenas alunos ativos.
        /// Por padrão traz alunos ativos e inativos.
        /// </summary>
        public bool BuscaSomenteInativos
        {
            set
            {
                if (_ddlSituacao.Items.Count == 0)
                {
                    CarregaComboSituacaoAluno();
                }

                if (value)
                {
                    _ddlSituacao.SelectedValue = _ddlSituacao.Items[2].Value;
                    if (DesabilitarSituacao)
                    {
                        _ddlSituacao.Enabled = false;
                    }
                    else
                    {
                        _ddlSituacao.Visible = false;
                        _lblSituacao.Visible = false;
                    }
                    divLegenda.Visible = false;
                }
                else
                {
                    _ddlSituacao.SelectedValue = _ddlSituacao.Items[0].Value;
                    _ddlSituacao.Visible = true;
                    _lblSituacao.Visible = true;
                    divLegenda.Visible = true;
                }
            }
        }

        /// <summary>
        /// Guarda o sortdirection do grid de alunos
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                return (SortDirection)(ViewState["VS_SortDirection"] ?? SortDirection.Ascending);
            }

            set
            {
                ViewState["VS_SortDirection"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortexpression do grid de alunos
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                return (ViewState["VS_Ordenacao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_Ordenacao"] = value;
            }
        }

        /// <summary>
        /// Nome do container onde foi colocado a busca.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Check se o é uma post back assincrono ou não (Ajax ou não).
        /// </summary>
        protected bool IsAsyncPostBack
        {
            get
            {
                bool retorno = false;
                var sm = ScriptManager.GetCurrent(Page);
                if (sm != null)
                    retorno = sm.IsInAsyncPostBack;
                return retorno;
            }
        }

        /// <summary>
        /// Indica se a exibição do nome do aluno é de documento oficial.
        /// </summary>
        public bool VS_DocumentoOficial
        {
            get
            {
                if (ViewState["VS_DocumentoOficial"] == null)
                {
                    ViewState["VS_DocumentoOficial"] = false;
                }

                return Convert.ToBoolean(ViewState["VS_DocumentoOficial"]);
            }

            set
            {
                ViewState["VS_DocumentoOficial"] = value;
            }
        }

        #endregion

        #region Delegates

        public delegate void OnReturnValues(IDictionary<string, object> parameters);
        public event OnReturnValues ReturnValues;

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _grvAluno.PageSize = ApplicationWEB._Paginacao;

                HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
                if (cell != null)
                    cell.BgColor = ApplicationWEB.AlunoInativo;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UCComboUAEscola1.Inicializar();
                CarregaComboSituacaoAluno();
                
                _ddlSituacao.SelectedIndex = 1;

                //Carrega o nome referente ao parametro de matricula estadual
                string _paramValor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                _lblMatrEst.Text = _paramValor;
                _lblMatrEst.Visible = !string.IsNullOrEmpty(_paramValor);
                _txtMatriculaEstadual.Visible = !string.IsNullOrEmpty(_paramValor);

                if (!string.IsNullOrEmpty(_paramValor))
                {
                    _grvAluno.Columns[INDEX_COLUMN_MATRICULA_ESTADUAL].Visible = true;
                    _grvAluno.Columns[INDEX_COLUMN_MATRICULA_ESTADUAL].HeaderText = _paramValor;
                    _lblMatricula.Visible = string.IsNullOrEmpty(_paramValor);
                    _txtMatricula.Visible = string.IsNullOrEmpty(_paramValor);
                }
                else
                {
                    _grvAluno.Columns[INDEX_COLUMN_MATRICULA].Visible = true;
                }
                _lblMatricula.Text = GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA").ToString();
            }
        }

        #endregion

        #region Eventos

        protected void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.CarregaUnidadesEscolaresPorUASuperior(UCComboUAEscola1.Uad_ID);
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola1.EnableEscolas = (UCComboUAEscola1.Uad_ID != Guid.Empty);
                UCComboUAEscola1.FocoEscolas = (UCComboUAEscola1.Uad_ID != Guid.Empty);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) unidade(s) administrativa(s).", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisa_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Pesquisar(0);
            }
        }

        protected void grvAluno_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                returns.Add(_grvAluno.DataKeyNames[0], _grvAluno.DataKeys[e.NewSelectedIndex].Values["alu_id"]);
                returns.Add(_grvAluno.DataKeyNames[1], _grvAluno.DataKeys[e.NewSelectedIndex].Values["pes_nome"]);

                // Solicitação de comparecimento
                returns.Add(_grvAluno.DataKeyNames[2], _grvAluno.DataKeys[e.NewSelectedIndex].Values["uad_id"]);
                returns.Add(_grvAluno.DataKeyNames[3], _grvAluno.DataKeys[e.NewSelectedIndex].Values["pes_nomeMae"]);
                //returns.Add(_grvAluno.DataKeyNames[5], _grvAluno.DataKeys[e.NewSelectedIndex].Values["nome_Responsavel"]);

                // Permissaõ do usuário
                returns.Add(_grvAluno.DataKeyNames[4], _grvAluno.DataKeys[e.NewSelectedIndex].Values["permissao"]);

                returns.Add(_grvAluno.DataKeyNames[5], _grvAluno.DataKeys[e.NewSelectedIndex].Values["alu_situacaoID"]);
                returns.Add(_grvAluno.DataKeyNames[6], _grvAluno.DataKeys[e.NewSelectedIndex].Values["pes_id"]);

                if (_grvAluno.Columns[INDEX_COLUMN_MATRICULA].Visible)
                {
                    returns.Add("matricula", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblMatricula")).Text);
                }
                else if (_grvAluno.Columns[INDEX_COLUMN_MATRICULA_ESTADUAL].Visible)
                {
                    returns.Add("matricula", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblMatriculaEstadual")).Text);
                }
                returns.Add("esc_nome", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblEscola")).Text);
                returns.Add("cur_nome", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblCurso")).Text);
                returns.Add("cpr_descricao", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblPeríodo")).Text);
                returns.Add("tur_codigo", ((Label)_grvAluno.Rows[e.NewSelectedIndex].FindControl("lblTurma")).Text);

                if (ReturnValues != null)
                    ReturnValues(returns);
                else
                    throw new NotImplementedException();

                if (!String.IsNullOrEmpty(ContainerName))
                    Close();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(_grvAluno, VS_Ordenacao, VS_SortDirection);
        }

        protected void grvAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte situacao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "alu_situacaoID"));

                //Pinta a linha de acordo com a situação do aluno            
                if ((ACA_AlunoSituacao)situacao == ACA_AlunoSituacao.Inativo)
                    e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;
            }
        }

        protected void _grvAluno_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                VS_SortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                VS_Ordenacao = e.SortExpression;
            }

            Pesquisar(grid.PageIndex);
        }

        protected void _grvAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Pesquisar(e.NewPageIndex);
        }

        #endregion

        #region Métodos

        public void Limpar()
        {
            UCComboUAEscola1.Inicializar();
            if (UCComboUAEscola1.Esc_ID > 0 && UCComboUAEscola1.QuantidadeItemsComboEscolas > 2)
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
            }
            _rblEscolhaBusca.SelectedIndex = 0;
            _txtNome.Text = "";
            _txtDataNascimento.Text = "";
            _txtMae.Text = "";
            _txtMatricula.Text = "";
            _txtMatriculaEstadual.Text = "";
            _ddlSituacao.SelectedIndex = 1;
            _grvAluno.SelectedIndex = -1;
            fdsResultado.Visible = false;
            divLegenda.Visible = false;

            if (UCComboUAEscola1.VisibleUA)
                UCComboUAEscola1.FocusUA();
            else
                UCComboUAEscola1.FocusEscolas();

        }

        /// <summary>
        /// Carrega os tipos de situação do aluno.
        /// </summary>
        private void CarregaComboSituacaoAluno()
        {
            _ddlSituacao.Items.Clear();

            byte Ativo = Convert.ToByte(ACA_AlunoSituacao.Ativo);
            byte Inativo = Convert.ToByte(ACA_AlunoSituacao.Inativo);
            byte Excedente = Convert.ToByte(ACA_AlunoSituacao.Excedente);

            _ddlSituacao.Items.Insert(0, new ListItem("-- Selecione uma situação --", "0", true));
            _ddlSituacao.Items.Insert(1, new ListItem("Ativo", Ativo.ToString()));
            _ddlSituacao.Items.Insert(2, new ListItem("Inativo", Inativo.ToString()));

            _ddlSituacao.DataBind();
        }

        /// <summary>
        /// Pesquisa os alunos
        /// </summary>
        /// <param name="pageIndex"></param>
        private void Pesquisar(int pageIndex)
        {
            try
            {
                string dataCriacao = new DateTime().ToString("yyyy-MM-dd");
                string dataAlteracao = new DateTime().ToString("yyyy-MM-dd");

                bool podeVisualizarTodos = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao;

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                _grvAluno.DataSource = ACA_AlunoBO.BuscaAlunos_PorFiltroPreferencial
                    (
                        UCComboUAEscola1.Uad_ID,
                        UCComboUAEscola1.Esc_ID,
                        UCComboUAEscola1.Uni_ID,
                        _txtNome.Text,
                        Convert.ToByte(_rblEscolhaBusca.SelectedValue),
                        _txtDataNascimento.Text,
                        _txtMae.Text,
                        _txtMatricula.Text,
                        _txtMatriculaEstadual.Text,
                        Convert.ToByte(_ddlSituacao.SelectedValue),
                        dataCriacao,
                        dataAlteracao,
                        __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao,
                        podeVisualizarTodos,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        false,
                        false,
                        itensPagina,
                        pageIndex,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        VS_DocumentoOficial
                    );

                _grvAluno.PageIndex = pageIndex;
                _grvAluno.PageSize = itensPagina;
                _grvAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

                _grvAluno.DataBind();

                fdsResultado.Visible = true;

                if (_ddlSituacao.Visible)
                    divLegenda.Visible = _grvAluno.Rows.Count > 0;
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

        /// <summary>
        /// Fecha o popup da consulta.
        /// </summary>
        private void Close()
        {
            _grvAluno.PageIndex = 0;
            if (this.IsAsyncPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseDialog", String.Format("$(\"#{0}\").dialog(\"close\");", this.ContainerName), true);
            }
            else
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("CloseDialog"))
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "CloseDialog", String.Format("$(\"#{0}\").dialog(\"close\");", this.ContainerName), true);
            }
        }

        /// <summary>
        /// Altera a visibilidade das colunas do grid.
        /// </summary>
        public void AlterarVisibilidadeColunas(bool mostrarDataNascimento
                                                , bool mostrarNomeMae
                                                , bool mostrarMatricula
                                                , bool mostrarDataCriacao
                                                , bool mostrarDataAlteracao
                                                , bool mostrarNomeEscola
                                                , bool mostrarCurso
                                                , bool mostrarPeriodoCurso
                                                , bool mostrarCodigoTurma)
        {
            _grvAluno.Columns[INDEX_COLUMN_DATA_NASCIMENTO].Visible = mostrarDataNascimento;
            _grvAluno.Columns[INDEX_COLUMN_NOME_MAE].Visible = mostrarNomeMae;
            _grvAluno.Columns[INDEX_COLUMN_MATRICULA].Visible &= mostrarMatricula;
            _grvAluno.Columns[INDEX_COLUMN_MATRICULA_ESTADUAL].Visible &= mostrarMatricula;
            _grvAluno.Columns[INDEX_COLUMN_DATA_CRIACAO].Visible = mostrarDataCriacao;
            _grvAluno.Columns[INDEX_COLUMN_DATA_ALTERACAO].Visible = mostrarDataAlteracao;
            _grvAluno.Columns[INDEX_COLUMN_NOME_ESCOLA].Visible = mostrarNomeEscola;
            _grvAluno.Columns[INDEX_COLUMN_CURSO].Visible = mostrarCurso;
            _grvAluno.Columns[INDEX_COLUMN_PERIODO_CURSO].Visible = mostrarPeriodoCurso;
            _grvAluno.Columns[INDEX_COLUMN_CODIGO_TURMA].Visible = mostrarCodigoTurma;
        }

        #endregion
    }
}
