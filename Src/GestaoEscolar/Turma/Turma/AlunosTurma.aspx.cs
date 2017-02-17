using System;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.Security.Cryptography;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Academico_Turma_AlunosTurma : MotherPageLogado
{
    #region Estruturas

    /// <summary>
    /// Estrutura para armazenar os registros 
    /// dos alunos matriculados na turma pesquisada
    /// </summary>
    [Serializable]
    private struct AlunosTurma
    {
        public int mtu_numeroChamada { get; set; }
        public int numeroChamada { get; set; }
        public int numeroChamadaReal { get; set; }
        public string alunoNome { get; set; }
        public string numeroMatricula { get; set; }
        public int alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtuSituacao { get; set; }
        public string movimentacaoSaida { get; set; }
        public DateTime mtu_dataMatricula { get; set; }
        public DateTime mtu_dataSaida { get; set; }
    }

    #endregion

    #region Constantes

    /// <summary>
    /// Constante que indica a coluna do número de chamada do aluno.
    /// </summary>
    const int _dgvAlunos_ColunaNumeroChamada = 0;
    
    /// <summary>
    /// Constante que indica a coluna da situação da matrícula  da turma.
    /// </summary>
    const int _dgvAlunos_ColunaMtuSituacao = 5;
    
    /// <summary>
    /// Constante que indica a coluna do número de chamada real do aluno.
    /// </summary>
    const int _dgvAlunos_ColunaNumeroChamadaReal = 1;

    /// <summary>
    /// Constante que indica a coluna do número de matrícula do aluno.
    /// </summary>
    const int _dgvAlunos_ColunaNumeroMatricula = 3;

    /// <summary>
    /// Constante que indica a coluna da data de saída
    /// </summary>
    const int _dgvAlunos_ColunaDataSaida = 7;

    #endregion

    #region Propriedades

    /// <summary>
    /// Id da turma pesquisada
    /// </summary>
    private long _VS_tur_id
    {
        get
        {
            if (ViewState["_VS_tur_id"] != null)
                return Convert.ToInt64(ViewState["_VS_tur_id"]);

            return 0;
        }
        set
        { ViewState["_VS_tur_id"] = value; }
    }

    /// <summary>
    /// Índice do registro ativado
    /// </summary>
    private int _VS_index
    {
        get
        {
            if (ViewState["_VS_index"] != null)
                return Convert.ToInt32(ViewState["_VS_index"]);
            return 0;
        }
        set
        {
            ViewState["_VS_index"] = value;
        }
    }

    /// <summary>
    /// Situacao da turma pesquisada
    /// TRUE - Turma ativa
    /// FALSE - Situacao diferente de ativa
    /// </summary>
    private bool _VS_SituacaoAtiva
    {
        get
        {
            return (bool)ViewState["_VS_SituacaoAtiva"];
        }
        set
        {
            ViewState["_VS_SituacaoAtiva"] = value;
        }
    }

    /// <summary>
    /// Lista com os registros dos alunos matriculados na turma pesquisada
    /// </summary>
    private List<AlunosTurma> _VS_dadosAlunos
    {
        get
        {
            if (ViewState["_VS_dadosAlunos"] == null)
                ViewState["_VS_dadosAlunos"] = new List<AlunosTurma>();
            return (List<AlunosTurma>)ViewState["_VS_dadosAlunos"];
        }
        set
        {
            ViewState["_VS_dadosAlunos"] = value;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada.
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (ViewState["VS_Ordenacao"] != null)
                return ViewState["VS_Ordenacao"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["VS_Ordenacao"] = value;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada.
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (ViewState["VS_SortDirection"] != null)
                return (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["VS_SortDirection"].ToString());
            else
                return SortDirection.Ascending;
        }
        set
        {
            ViewState["VS_SortDirection"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Redireciona para a página de busca.
    /// </summary>
    private void VoltarBusca()
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    /// <summary>
    /// Carrega os dados na tela.
    /// </summary>
    public void carregaTela()
    {
        try
        {
            DataTable dtTurma = TUR_TurmaBO.SelectBY_tur_id(_VS_tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            
             int qtVagas, qtMatriculados;
            TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(_VS_tur_id, out qtVagas, out qtMatriculados);

            _lblTurma.Text = String.Format("Turma: <b>{0} </b><br/>", dtTurma.Rows[0]["tur_codigo"]);
            _lblEscola.Text = String.Format("Escola: <b>{0} </b><br/>", dtTurma.Rows[0]["tur_escolaUnidade"]);
            _lblCalendario.Text = String.Format("Calendário: <b>{0} </b><br/>", dtTurma.Rows[0]["tur_calendario"]);
            _lblCurso.Text = String.Format(GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": <b>{0} </b><br/>", dtTurma.Rows[0]["tur_curso"]);
            _lblTurno.Text = String.Format("Turno: <b>{0} </b><br/>", dtTurma.Rows[0]["tur_turno"]);
            _lblSituacao.Text = String.Format("Situação da turma: <b>{0} </b><br/>", dtTurma.Rows[0]["tur_situacao"]);
            _lblCapacidade.Text = String.Format("Capacidade da turma: <b>{0} </b><br/>", qtVagas);
            _lblQtdMatriculados.Text = String.Format("Quantidade de matriculados na turma: <b>{0} </b><br/>", qtMatriculados);

            PesquisaAlunos();
            divLegenda.Visible = _dgvAlunos.Rows.Count > 0;

            // verifica se o grupo tem permissão para alteracoes 
            _dgvAlunos.Columns[_dgvAlunos_ColunaNumeroChamada].Visible = false;

            _dgvAlunos.AllowSorting = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar alunos da turma.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Executa pesquisa e preenche grid com alunos da turma carregada
    /// </summary>
    private void PesquisaAlunos()
    {
        try
        {
            _dgvAlunos.DataSourceID = odsALunosTurma.ID;
            odsALunosTurma.SelectParameters.Clear();
            odsALunosTurma.SelectParameters.Add("tur_id", _VS_tur_id.ToString());

            _dgvAlunos.Sort(VS_Ordenacao, VS_SortDirection);

            _dgvAlunos.DataBind();

            DataTable dt = MTR_MatriculaTurmaBO.MatriculaTurmaAlunosNumeroChamada(_VS_tur_id);

            _VS_dadosAlunos = dt.Rows.Cast<DataRow>().Select(dr =>
                       new AlunosTurma
                       {
                           mtu_numeroChamada = Convert.ToInt32(dr["mtu_numeroChamada"]),
                           numeroChamada = Convert.ToInt32(dr["numeroChamada"]),
                           numeroChamadaReal = Convert.ToInt32(dr["numeroChamadaReal"]),
                           alunoNome = Convert.ToString(dr["alunoNome"]),
                           numeroMatricula = Convert.ToString(dr["numeroMatricula"]),
                           alu_id = Convert.ToInt32(dr["alu_id"]),
                           mtu_id = Convert.ToInt32(dr["mtu_id"]),
                           mtuSituacao = Convert.ToInt32(dr["mtuSituacao"]),
                           movimentacaoSaida = Convert.ToString(dr["movimentacaoSaida"]),
                           mtu_dataMatricula = string.IsNullOrEmpty(dr["mtu_dataMatricula"].ToString()) ? new DateTime() : Convert.ToDateTime(dr["mtu_dataMatricula"].ToString()),
                           mtu_dataSaida = string.IsNullOrEmpty(dr["mtu_dataSaida"].ToString()) ? new DateTime() : Convert.ToDateTime(dr["mtu_dataSaida"].ToString())
                       }).ToList();

            _dgvAlunos.AllowPaging = false;
            _dgvAlunos.AllowSorting = true;

            if (_VS_dadosAlunos.Where(p => p.numeroChamada > 0 && p.mtuSituacao == 1).Count() > 0)
                _dgvAlunos.Sort("mtu_numeroChamada", SortDirection.Ascending);
            else
                _dgvAlunos.Sort("alunoNome", SortDirection.Ascending);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar alunos da turma.", UtilBO.TipoMensagem.Erro);
        }
    }
    
    #endregion

    #region Eventos

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                _dgvAlunos.Columns[_dgvAlunos_ColunaNumeroMatricula].HeaderText = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
            if (cell != null)
                cell.BgColor = ApplicationWEB.AlunoInativo;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsAlunosTurma.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _VS_tur_id = PreviousPage.Edit_tur_id;
                    _VS_SituacaoAtiva = (PreviousPage.Edit_tur_situacao == "Ativo");

                    if (_VS_tur_id > 0)
                    {
                        carregaTela();
                    }
                    else
                    {
                        VoltarBusca();
                    }
                }
                else
                {
                    VoltarBusca();
                }

            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }
    
    protected void _dgvAlunos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSituacao = ((Label)e.Row.FindControl("lblSituacao"));
            if (lblSituacao != null)
            {
                if (Convert.ToInt32(e.Row.Cells[_dgvAlunos_ColunaMtuSituacao].Text) == Convert.ToInt32(MTR_MatriculaTurmaSituacao.Ativo))
                    lblSituacao.Text = "Ativo";
                else
                    if (Convert.ToInt32(e.Row.Cells[_dgvAlunos_ColunaMtuSituacao].Text) == Convert.ToInt32(MTR_MatriculaTurmaSituacao.Inativo))
                    {
                        e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;

                        lblSituacao.Text = "Inativo";

                        Label lblMovimentacaoSaida = ((Label)e.Row.FindControl("lblMovimentacaoSaida"));
                        if (lblMovimentacaoSaida != null && !(string.IsNullOrEmpty(lblMovimentacaoSaida.Text)))
                        {
                            lblSituacao.Text += " (" + lblMovimentacaoSaida.Text + ")";
                        }
                    }
                    else
                        if (Convert.ToInt32(e.Row.Cells[_dgvAlunos_ColunaMtuSituacao].Text) == Convert.ToInt32(MTR_MatriculaTurmaSituacao.Efetivado))
                            lblSituacao.Text = "Efetivado";
                        else
                            if (Convert.ToInt32(e.Row.Cells[_dgvAlunos_ColunaMtuSituacao].Text) == Convert.ToInt32(MTR_MatriculaTurmaSituacao.EmMatricula))
                                lblSituacao.Text = "Em matrícula";
            }

            // Muda o número de chamada -1 da coluna real para vazio.
            if ((Convert.ToInt32(e.Row.Cells[_dgvAlunos_ColunaNumeroChamadaReal].Text) <= 0))
                e.Row.Cells[_dgvAlunos_ColunaNumeroChamadaReal].Text = "";

            // Muda o número de chamada -1 da coluna real para vazio.     
            string dataSaida = DataBinder.Eval(e.Row.DataItem, "mtu_dataSaida").ToString();
            if (!string.IsNullOrEmpty(dataSaida))
            {
                if ((Convert.ToDateTime(e.Row.Cells[_dgvAlunos_ColunaDataSaida].Text) == new DateTime()))
                    e.Row.Cells[_dgvAlunos_ColunaDataSaida].Text = "";
            }
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Turma/Turma/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    
    protected void _dgvAlunos_DataBound(object sender, EventArgs e)
    {
        if (_dgvAlunos.Rows.Count > 0)
        {
            ConfiguraColunasOrdenacao(_dgvAlunos);
            if ((!String.IsNullOrEmpty(_dgvAlunos.SortExpression)))
            {

                VS_Ordenacao = _dgvAlunos.SortExpression;
                VS_SortDirection = _dgvAlunos.SortDirection;


                //__SessionWEB.BuscaRealizada = new BuscaGestao
                //{
                //    PaginaBusca = PaginaGestao.TurmaAlunos
                //    ,
                //    Filtros = filtros
                //};
            }

            ImageButton _btnDescer = (ImageButton)_dgvAlunos.Rows[_dgvAlunos.Rows.Count - 1].FindControl("_btnDescer");

            if (_btnDescer != null)
            {
                _btnDescer.Visible = false;
            }
        }
    }
    
    #endregion
}


