using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_Aluno_Busca : MotherPageLogado
{
    #region Constantes

    private const int indiceColunaMatricula = 4;
    private const int indiceColunaMatriculaEstadual = 5;
    private const int indiceColunaAnotacoes = 8;
    private const int indiceColunaCapturarFoto = 9;
    private const int indiceColunaBoletim = 10;

    #endregion Constantes

    #region Propriedades

    public long EditItem
    {
        get
        {
            return Convert.ToInt64(_grvAluno.DataKeys[_grvAluno.EditIndex].Values["alu_id"]);
        }
    }

    public bool EditItem_Permissao
    {
        get
        {
            return Convert.ToBoolean(_grvAluno.DataKeys[_grvAluno.EditIndex].Values["permissao"]);
        }
    }

    /// <summary>
    /// Retorna se vai exibir o campo de gemeos
    /// </summary>
    private bool PermiteAlunoGemeo
    {
        get { return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_CAMPO_ALUNO_GEMEO, __SessionWEB.__UsuarioWEB.Usuario.ent_id); }
    }

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
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Alunos)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Alunos)
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
    /// Retorna se é pra mostrar só o nome, ou o código + o nome da escola no grid.
    /// </summary>
    private bool MostraCodigoComNomeEscola
    {
        get { return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id); }
    }

    #endregion Propriedades

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        _Pesquisar(0, false);
    }

    #endregion Delegates

    #region Métodos

    ///// <summary>
    ///// Atualiza parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
    ///// </summary>
    private void Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA(GridViewRowEventArgs index)
    {
        Label lblMatriculaSalaRecurso = (Label)index.Row.FindControl("lblMatriculaSalaRecurso");
        if (lblMatriculaSalaRecurso != null)
            lblMatriculaSalaRecurso.Text = "Matrícula em " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
    }

    /// <summary>
    /// Carrega os tipos de situação do aluno.
    /// </summary>
    private void CarregaComboSituacaoAluno()
    {
        ddlSituacao.Items.Clear();

        byte Ativo = Convert.ToByte(ACA_AlunoSituacao.Ativo);
        byte Inativo = Convert.ToByte(ACA_AlunoSituacao.Inativo);
        byte Excedente = Convert.ToByte(ACA_AlunoSituacao.Excedente);
        byte EmPreMatricula = Convert.ToByte(ACA_AlunoSituacao.EmPreMatricula);

        ddlSituacao.Items.Insert(0, new ListItem("-- Selecione uma situação --", "0", true));
        ddlSituacao.Items.Insert(1, new ListItem((string)GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Ativo"), Ativo.ToString()));
        ddlSituacao.Items.Insert(2, new ListItem((string)GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Inativo"), Inativo.ToString()));
        
        ddlSituacao.DataBind();
        ddlSituacao.SelectedIndex = 1;
    }

    /// <summary>
    /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
    /// realizada.
    /// </summary>
    private void SelecionarEscola()
    {
        string esc_id;
        string uni_id;

        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
        {
            uccUaEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
        }
    }

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    private void _Pesquisar(int pageIndex, bool alterarSessaoBusca)
    {
        try
        {
            string dataCriacao = string.IsNullOrEmpty(_txtDataCriacao.Text) ? new DateTime().ToString("yyyy-MM-dd") : Convert.ToDateTime(_txtDataCriacao.Text).ToString("yyyy-MM-dd");
            string dataAlteracao = string.IsNullOrEmpty(_txtDataAlteracao.Text) ? new DateTime().ToString("yyyy-MM-dd") : Convert.ToDateTime(_txtDataAlteracao.Text).ToString("yyyy-MM-dd");

            //Seta o tipo de busca por nome do aluno : 1 - Contém / 2 - Começa por / 3 - Fonética
            byte tipoBusca = Convert.ToByte(_rblEscolhaBusca.SelectedValue);
            
            bool apenasGemeo = PermiteAlunoGemeo && chkApenasGemeos.Checked;
            bool podeVisualizarTodos = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao
                                      && chkPesquisarTodasEscolas.Checked;

            _grvAluno.DataSource = _VS_doc_id > 0 ?
                ACA_AlunoBO.GetSelectBy_Docente
                    (
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        _VS_doc_id,
                        tipoBusca,
                        _txtNome.Text,
                        _txtMae.Text,
                        _txtDataNascimento.Text,
                        _txtMatricula.Text,
                        _txtMatriculaEstadual.Text,
                        Convert.ToDateTime(dataCriacao),
                        Convert.ToDateTime(dataAlteracao),
                        ckbApenasDeficiencia.Checked,
                        apenasGemeo,
                        UCComboTipoDeficiencia.Valor,
                        UCComboQtdePaginacao1.Valor,
                        pageIndex,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        false
                        ) :
                ACA_AlunoBO.BuscaAlunos_PorFiltroPreferencial
                    (
                        uccUaEscola.Uad_ID,
                        uccUaEscola.Esc_ID,
                        uccUaEscola.Uni_ID,
                        _txtNome.Text,
                        tipoBusca,
                        _txtDataNascimento.Text,
                        _txtMae.Text,
                        _txtMatricula.Text,
                        _txtMatriculaEstadual.Text,
                        Convert.ToByte(ddlSituacao.SelectedValue),
                        dataCriacao,
                        dataAlteracao,
                        ckbApenasDeficiencia.Checked,
                        UCComboTipoDeficiencia.Valor,
                        __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao,
                        podeVisualizarTodos,
                        false,
                        apenasGemeo,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        false,
                        false,
                        UCComboQtdePaginacao1.Valor,
                        pageIndex,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        false
                    );

            _grvAluno.PageIndex = pageIndex;
            _grvAluno.PageSize = UCComboQtdePaginacao1.Valor;
            _grvAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

            _grvAluno.DataBind();

            #region Salvar busca realizada com os parâmetros do ODS.

            if (alterarSessaoBusca)
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                if (_VS_doc_id > 0)
                {
                    filtros.Add("doc_id", _VS_doc_id.ToString());
                }
                else
                {
                    filtros.Add("uad_idSuperior", uccUaEscola.Uad_ID.ToString());
                    filtros.Add("esc_id", uccUaEscola.Esc_ID.ToString());
                    filtros.Add("uni_id", uccUaEscola.Uni_ID.ToString());
                    filtros.Add("alu_situacao", Convert.ToString(ddlSituacao.SelectedValue));
                    filtros.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
                    filtros.Add("podeVisualizarTodos", podeVisualizarTodos.ToString());
                    filtros.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                    filtros.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                    filtros.Add("apenasIntencaoTransferencia", "false");
                    filtros.Add("retornaExcedentes", "false");
                    filtros.Add("retornaPreMatricula", "false");
                }

                filtros.Add("pes_nome", _txtNome.Text);
                filtros.Add("pes_dataNascimento", _txtDataNascimento.Text);
                filtros.Add("pes_nomeMae", _txtMae.Text);
                filtros.Add("tipoBusca", tipoBusca.ToString());
                filtros.Add("alc_matricula", _txtMatricula.Text);
                filtros.Add("alc_matriculaEstadual", _txtMatriculaEstadual.Text);
                filtros.Add("alu_dataCriacao", dataCriacao);
                filtros.Add("alu_dataAlteracao", dataAlteracao);
                filtros.Add("apenasDeficiente", ckbApenasDeficiencia.Checked.ToString());
                filtros.Add("deficiencia", UCComboTipoDeficiencia.Valor.ToString());
                filtros.Add("apenasGemeo", apenasGemeo.ToString());
                filtros.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.Alunos
                    ,
                    Filtros = filtros
                };
            }

            #endregion Salvar busca realizada com os parâmetros do ODS.

            fdsResultados.Visible = true;

            //Mostra a legenda e itens por pagina caso forem retornados alunos pela pesquisa
            divLegenda.Visible = _grvAluno.Rows.Count > 0;
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Alunos)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
            _txtNome.Text = valor;

            if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_dataNascimento", out valor))
                _txtDataNascimento.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nomeMae", out valor);
            _txtMae.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
            _txtMatricula.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matriculaEstadual", out valor);
            _txtMatriculaEstadual.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
            _rblEscolhaBusca.SelectedValue = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alu_dataCriacao", out valor);
            if (!string.IsNullOrEmpty(valor) && Convert.ToDateTime(valor) != new DateTime())
                _txtDataCriacao.Text = Convert.ToDateTime(valor).ToString("dd/MM/yyyy");

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alu_dataAlteracao", out valor);
            if (!string.IsNullOrEmpty(valor) && Convert.ToDateTime(valor) != new DateTime())
                _txtDataAlteracao.Text = Convert.ToDateTime(valor).ToString("dd/MM/yyyy");

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alu_situacao", out valor);
            ddlSituacao.SelectedValue = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("podeVisualizarTodos", out valor);
            chkPesquisarTodasEscolas.Checked = string.IsNullOrEmpty(valor) ? false : Convert.ToBoolean(valor);
            ValidarCheckPesquisarTodasEscolas();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("apenasDeficiente", out valor);
            ckbApenasDeficiencia.Checked = string.IsNullOrEmpty(valor) ? false : Convert.ToBoolean(valor);
            if (ckbApenasDeficiencia.Checked)
                ckbApenasDeficiencia_CheckedChanged(null, null);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("deficiencia", out valor);
            UCComboTipoDeficiencia._Load(Guid.Empty, 1);
            UCComboTipoDeficiencia.Valor = string.IsNullOrEmpty(valor) ? Guid.Empty : new Guid(valor);
            upApenasDeficiencia.Update();

            if (uccUaEscola.FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                if (!string.IsNullOrEmpty(valor))
                {
                    uccUaEscola.Uad_ID = new Guid(valor);
                }

                uccUaEscola.EnableEscolas = (uccUaEscola.Uad_ID != Guid.Empty);

                if (uccUaEscola.Uad_ID != Guid.Empty)
                {
                    uccUaEscola.CarregaEscolaPorUASuperiorSelecionada();
                    SelecionarEscola();
                }
            }
            else
            {
                SelecionarEscola();
            }

            string dataCriacao = string.IsNullOrEmpty(_txtDataCriacao.Text) ? new DateTime().ToString("yyyy-MM-dd") : Convert.ToDateTime(_txtDataCriacao.Text).ToString("yyyy-MM-dd");
            string dataAlteracao = string.IsNullOrEmpty(_txtDataAlteracao.Text) ? new DateTime().ToString("yyyy-MM-dd") : Convert.ToDateTime(_txtDataAlteracao.Text).ToString("yyyy-MM-dd");

            if (!(uccUaEscola.Uad_ID == Guid.Empty
                && uccUaEscola.Esc_ID <= 0 && uccUaEscola.Uni_ID <= 0 && string.IsNullOrEmpty((_txtNome.Text ?? "").Trim())
                && string.IsNullOrEmpty((_txtMae.Text ?? "").Trim())
                && Convert.ToDateTime(string.IsNullOrEmpty(_txtDataNascimento.Text) ? new DateTime().ToString("yyyy-MM-dd") : Convert.ToDateTime(_txtDataNascimento.Text).ToString("yyyy-MM-dd")) == new DateTime() && string.IsNullOrEmpty((_txtMatricula.Text ?? "").Trim())
                && string.IsNullOrEmpty((_txtMatriculaEstadual.Text ?? "").Trim()) && Convert.ToDateTime(dataCriacao) == new DateTime()
                && Convert.ToDateTime(dataAlteracao) == new DateTime()
                ))
            {
                _Pesquisar(0, true);
            }
            else { fdsResultados.Visible = false; }
        }
        else
        {
            fdsResultados.Visible = false;
        }
    }

    private void ValidarCheckPesquisarTodasEscolas()
    {
        bool UA = uccUaEscola.VisibleUA;
        bool Escola = uccUaEscola.VisibleEscolas;

        if (chkPesquisarTodasEscolas.Checked)
        {
            uccUaEscola.PermiteAlterarCombos = false;
            if (UA)
            {
                uccUaEscola.Uad_ID = Guid.Empty;
            }
            if (Escola)
            {
                uccUaEscola.SelectedValueEscolas = new[] { -1, -1 };
            }
        }
        else
        {
            uccUaEscola.PermiteAlterarCombos = true;

            if (UA)
            {
                uccUaEscola.SelecionaPrimeiroItemUA();
            }
            else
            {
                uccUaEscola.SelecionaPrimeiroItemEscola();
            }
        }
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Init(object sender, EventArgs e)
    {
        HtmlTableCell cell = tbLegenda.Rows[0].Cells[0];
        if (cell != null)
            cell.BgColor = ApplicationWEB.AlunoInativo;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaAlunos.js"));
        }

        if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), fdsConsulta.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsConsulta.ClientID)), true);
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _grvAluno;

        if (!IsPostBack)
        {
            try
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    }
                    else
                    {
                        _lblMessage.Text = UtilBO.GetErroMessage("Este usuário não tem permissão de acesso a esta página.", UtilBO.TipoMensagem.Alerta);
                        fdsResultados.Visible = false;
                        fdsConsulta.Visible = false;
                        return;
                    }
                }

                cvDataNascimento.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de nascimento do aluno");
                cvData.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de cadastro");
                CustomValidator1.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data da última atualização");

                // Se for docente, não carrega as escolas.
                if (_VS_doc_id <= 0)
                {
                    uccUaEscola.FiltroEscolasControladas = true;
                    uccUaEscola.Inicializar();
                }

                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                // Carrega o nome referente ao parametro de matricula estadual.
                string nomeMatriculaEstadual =
                    ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                // Configura os campos de matrícula estadual e matrícula, de acordo com o parâmetro.
                _lblMatrEst.Text = nomeMatriculaEstadual;
                _lblMatrEst.Visible = mostraMatriculaEstadual;
                _txtMatriculaEstadual.Visible = mostraMatriculaEstadual;

                _grvAluno.Columns[indiceColunaMatriculaEstadual].HeaderText = nomeMatriculaEstadual;
                _grvAluno.Columns[indiceColunaMatriculaEstadual].Visible = mostraMatriculaEstadual;

                _grvAluno.Columns[indiceColunaMatricula].Visible = !mostraMatriculaEstadual;
                _lblMatricula.Visible = !mostraMatriculaEstadual;
                _txtMatricula.Visible = !mostraMatriculaEstadual;

                _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnLimparPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

                // Exibe coluna 'alterar foto' se parâmetro está definido como true,
                // ou se grupo do usuário tem permissão de alteração e usuário está logado com visão diferente da Individual.
                _grvAluno.Columns[indiceColunaCapturarFoto].Visible =
                    (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                    __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual) || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ALTERAR_FOTO_ALUNO_CONSULTA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                
                _grvAluno.Columns[indiceColunaAnotacoes].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ANOTACOES_BUSCA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                bool mostraBoletim = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MOSTRAR_COLUNA_BOLETIM_MANUTENCAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                _grvAluno.Columns[indiceColunaBoletim].Visible = mostraBoletim;

                chkApenasGemeos.Visible = PermiteAlunoGemeo;

                if (_VS_doc_id > 0)
                {
                    fdsResultados.Visible = true;
                    fdsConsulta.Visible = true;
                    divSituacao.Visible = false;
                }

                CarregaComboSituacaoAluno();

                VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            if (uccUaEscola.FiltroEscola)
                uccUaEscola.FocusUA();
            else
                uccUaEscola.FocusEscolas();

            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
        }

        if (_VS_doc_id > 0)
        {
            uccUaEscola.Visible = false;
        }
        else
            chkPesquisarTodasEscolas.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao;
    }

    protected void _grvAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        _Pesquisar(e.NewPageIndex, false);
    }

    protected void _grvAluno_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView grid = ((GridView)(sender));

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
                PaginaBusca = PaginaGestao.Alunos
                ,
                Filtros = filtros
            };
        }

        _Pesquisar(grid.PageIndex, false);
    }

    protected void _grvAluno_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();

        // seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvAluno, VS_Ordenacao, VS_SortDirection);
    }

    protected void _grvAluno_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            byte situacao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "alu_situacaoID"));
            long arq_idFoto = Convert.ToInt64((DataBinder.Eval(e.Row.DataItem, "arq_idFoto")) == DBNull.Value ? 0 : (DataBinder.Eval(e.Row.DataItem, "arq_idFoto")));
            bool possuiimagem = (arq_idFoto > 0);

            string tca_numeroAvaliacao = DataBinder.Eval(e.Row.DataItem, "tca_numeroAvaliacao").ToString();
            string crp_nomeAvaliacao = DataBinder.Eval(e.Row.DataItem, "crp_nomeAvaliacao").ToString();

            //Pinta a linha de acordo com a situação do aluno
            if ((ACA_AlunoSituacao)situacao == ACA_AlunoSituacao.Inativo)
                e.Row.Style["background-color"] = ApplicationWEB.AlunoInativo;

            Image _imgPossuiImagem = (Image)e.Row.FindControl("_imgPossuiImagem");
            if (_imgPossuiImagem != null)
                _imgPossuiImagem.Visible = possuiimagem;

            bool bloquearAlteracao = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao && !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BLOQUEAR_FICHA_ALUNO_USUARIOS_PERMISSAO_CONSULTA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = bloquearAlteracao;
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = !bloquearAlteracao;
            }

            // Só não mostra o botão de expandir se o aluno estiver excedente
            LinkButton btnExpandir = (LinkButton)e.Row.FindControl("btnExpandir");
            if (btnExpandir != null)
                btnExpandir.Visible = (ACA_AlunoSituacao)situacao != ACA_AlunoSituacao.Excedente &&
                                      (ACA_AlunoSituacao)situacao != ACA_AlunoSituacao.EmPreMatricula;

            Label lblEscola = (Label)e.Row.FindControl("lblEscola");
            string nomecampo = MostraCodigoComNomeEscola ? "Escola_CodigoNome" : "esc_nome";
            lblEscola.Text = "<b>Escola: </b>" + DataBinder.Eval(e.Row.DataItem, nomecampo) ?? "-";

            Label lblCurso = (Label)e.Row.FindControl("lblCurso");
            if (String.IsNullOrEmpty(lblCurso.Text))
                lblCurso.Text = "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>-";
            else
                lblCurso.Text = "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + lblCurso.Text;

            Label lblPeriodo = (Label)e.Row.FindControl("lblPeriodo");
            if (String.IsNullOrEmpty(lblPeriodo.Text))
                lblPeriodo.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>-";
            else
                lblPeriodo.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + lblPeriodo.Text;

            Label lblTurma = (Label)e.Row.FindControl("lblTurma");
            if (String.IsNullOrEmpty(lblTurma.Text))
                lblTurma.Text = "<b>Turma: </b>-";
            else
                lblTurma.Text = "<b>Turma: </b>" + lblTurma.Text;

            Label lblAvaliacao = (Label)e.Row.FindControl("lblAvaliacao");
            if (!string.IsNullOrEmpty(crp_nomeAvaliacao) && !string.IsNullOrEmpty(tca_numeroAvaliacao))
            {
                lblAvaliacao.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>" + crp_nomeAvaliacao + ": </b>" + tca_numeroAvaliacao;
                lblAvaliacao.Visible = true;
            }
            else
            {
                lblAvaliacao.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>" + crp_nomeAvaliacao + ": </b>-";
                lblAvaliacao.Visible = false;
            }

            Label lblNChamada = (Label)e.Row.FindControl("lblNChamada");
            int numeroChamada;
            Int32.TryParse(lblNChamada.Text, out numeroChamada);

            if (numeroChamada > 0)
                lblNChamada.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Nº Chamada: </b>" + lblNChamada.Text;
            else
                lblNChamada.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Nº Chamada: </b>-";

            //atualiza parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
            Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA(e);

            ImageButton _btnBoletim = (ImageButton)e.Row.FindControl("_btnBoletim");
            if (_btnBoletim != null)
            {
                _btnBoletim.CommandArgument = e.Row.RowIndex.ToString();
            }
            _btnBoletim = (ImageButton)e.Row.FindControl("_btnBoletim2");
            if (_btnBoletim != null)
            {
                _btnBoletim.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _grvAluno_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Boletim")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                long alu_id = Convert.ToInt64(_grvAluno.DataKeys[index].Value);
                int tpc_id = ACA_TipoPeriodoCalendarioBO.SelecionaPeriodoVigente();

                ucBoletim.Carregar(tpc_id, alu_id);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "BoletimCompletoAluno", "$(document).ready(function(){ $('#divBoletimCompleto').dialog('open'); });", true);
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o boletim completo do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _grvAluno_RowEditing(object sender, GridViewEditEventArgs e)
    {
        _grvAluno.EditIndex = e.NewEditIndex;
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect("Busca.aspx", false);
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;

            _Pesquisar(0, true);
        }
    }

    protected void chkPesquisarTodasEscolas_CheckedChanged(object sender, EventArgs e)
    {
        ValidarCheckPesquisarTodasEscolas();
    }

    protected void ckbApenasDeficiencia_CheckedChanged(object sender, EventArgs e)
    {
        UCComboTipoDeficiencia.PermiteEditar = UCComboTipoDeficiencia.Visible = ckbApenasDeficiencia.Checked;
        UCComboTipoDeficiencia.Valor = Guid.Empty;
    }
    
    #endregion Eventos
}