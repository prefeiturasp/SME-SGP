using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Web.UI.HtmlControls;

public partial class Academico_Curso_Cadastro : MotherPageLogado
{
    #region Constantes

    /// <summary>
    /// Índice da aba cursos.
    /// </summary>
    private const string IndexAbaCursos = "0";

    /// <summary>
    /// Índice da aba componentes.
    /// </summary>
    private const string IndexAbaComponentes = "1";

    /// <summary>
    /// Posição da coluna "Périodos do curso" no grid de "Eletivas do aluno".
    /// </summary>
    private const int PosPeriodoCursoEletivasAlunos = 1;

    #endregion Constantes

    #region Propriedades

    /// <summary>
    /// Armazena o id do curso.
    /// </summary>
    private int _VS_cur_id
    {
        get
        {
            if (ViewState["_VS_cur_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_cur_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_cur_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do currículo.
    /// </summary>
    private int _VS_crr_id
    {
        get
        {
            if (ViewState["_VS_crr_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_crr_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_crr_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o valor do metodo VerificaAlunoCurriculo
    /// </summary>
    private bool _VS_VerificaAlunoCurriculo
    {
        get
        {
            if (ViewState["_VS_VerificaAlunoCurriculo"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_VerificaAlunoCurriculo"]);
            }

            return false;
        }

        set
        {
            ViewState["_VS_VerificaAlunoCurriculo"] = value;
        }
    }

    /// <summary>
    /// Armazena o valor do metodo VerificaTurma
    /// </summary>
    private bool _VS_VerificaTurma
    {
        get
        {
            if (ViewState["_VS_VerificaTurma"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_VerificaTurma"]);
            }

            return false;
        }

        set
        {
            ViewState["_VS_VerificaTurma"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid
    /// </summary>
    private int _VS_id
    {
        get
        {
            if (ViewState["_VS_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid
    /// </summary>
    private int _VS_id2
    {
        get
        {
            if (ViewState["_VS_id2"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_id2"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_id2"] = value;
        }
    }

    /// <summary>
    /// Armazena o nome da disciplina
    /// </summary>
    private string _VS_dis_nome
    {
        get
        {
            if (ViewState["_VS_dis_nome"] != null)
            {
                return Convert.ToString(ViewState["_VS_dis_nome"]);
            }

            return string.Empty;
        }

        set
        {
            ViewState["_VS_dis_nome"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de periodos
    /// Retorno e atribui valores para o DataTable de periodos
    /// </summary>
    private DataTable _VS_periodos
    {
        get
        {
            if (ViewState["_VS_periodos"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crr_id");
                dt.Columns.Add("crp_id");
                dt.Columns.Add("crp_ordem");
                dt.Columns.Add("crp_descricao");
                dt.Columns.Add("crp_idadeIdealAnoInicio");
                dt.Columns.Add("crp_idadeIdealMesInicio");
                dt.Columns.Add("crp_idadeIdealAnoFim");
                dt.Columns.Add("crp_idadeIdealMesFim");
                dt.Columns.Add("crp_controleTempo");
                dt.Columns.Add("crp_qtdeDiasSemana");
                dt.Columns.Add("crp_qtdeTemposDia");
                dt.Columns.Add("crp_qtdeHorasDia");
                dt.Columns.Add("crp_qtdeMinutosDia");
                dt.Columns.Add("crp_qtdeEletivasAlunos");
                dt.Columns.Add("crp_fundoFrente");
                dt.Columns.Add("tci_id");
                dt.Columns.Add("tcp_id");
                dt.Columns.Add("crp_turmaAvaliacao");
                dt.Columns.Add("crp_nomeAvaliacao");
                dt.Columns.Add("crp_concluiNivelEnsino");

                ViewState["_VS_periodos"] = dt;
            }

            return (DataTable)ViewState["_VS_periodos"];
        }

        set
        {
            ViewState["_VS_periodos"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable de Periodo
    /// </summary>
    private int _VS_seqPeriodo
    {
        get
        {
            if (ViewState["_VS_seqPeriodo"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_seqPeriodo"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_seqPeriodo"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de disciplinas
    /// Retorno e atribui valores para o DataTable de disciplinas
    /// </summary>
    private DataTable _VS_disciplinas
    {
        get
        {
            if (ViewState["_VS_disciplinas"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("tds_id");
                dt.Columns.Add("tds_nome");
                dt.Columns.Add("tds_base");
                dt.Columns.Add("tds_baseDescricao");
                dt.Columns.Add("dis_codigo");
                dt.Columns.Add("dis_nome");
                dt.Columns.Add("dis_nomeAbreviado");

                ViewState["_VS_disciplinas"] = dt;
            }

            return (DataTable)ViewState["_VS_disciplinas"];
        }

        set
        {
            ViewState["_VS_disciplinas"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de disciplinas do periodo
    /// </summary>
    private DataTable _VS_DisciplinasPeriodo
    {
        get
        {
            if (ViewState["_VS_DisciplinasPeriodo"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crp_descricao");
                dt.Columns.Add("crp_concluiNivelEnsino");
                dt.Columns.Add("crp_id");
                dt.Columns.Add("tds_id");
                dt.Columns.Add("tds_nome");
                dt.Columns.Add("tds_base");
                dt.Columns.Add("tds_baseDescricao");
                dt.Columns.Add("dis_id");
                dt.Columns.Add("dis_codigo");
                dt.Columns.Add("dis_nome");
                dt.Columns.Add("dis_nomeAbreviado");
                dt.Columns.Add("dis_ementa");
                dt.Columns.Add("dis_cargaHorariaTeorica");
                dt.Columns.Add("dis_cargaHorariaAnual");
                dt.Columns.Add("dis_cargaHorariaTotal");
                dt.Columns.Add("crd_tipo");
                dt.Columns.Add("dis_nomeDocumentacao");
                dt.Columns.Add("dis_habilidades");
                dt.Columns.Add("dis_objetivos");
                dt.Columns.Add("dis_metodologias");

                ViewState["_VS_DisciplinasPeriodo"] = dt;
            }

            return (DataTable)ViewState["_VS_DisciplinasPeriodo"];
        }

        set
        {
            ViewState["_VS_DisciplinasPeriodo"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de disciplinas eletivas
    /// </summary>
    private DataTable _VS_DisciplinasEletivas
    {
        get
        {
            if (ViewState["_VS_DisciplinasEletivas"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crp_id");
                dt.Columns.Add("cde_id");
                dt.Columns.Add("tds_id");
                dt.Columns.Add("tds_nome");
                dt.Columns.Add("dis_nome");
                dt.Columns.Add("cde_nome");
                ViewState["_VS_DisciplinasEletivas"] = dt;
            }

            return (DataTable)ViewState["_VS_DisciplinasEletivas"];
        }

        set
        {
            ViewState["_VS_DisciplinasEletivas"] = value;
        }
    }

    /// <summary>
    /// Armazena o valor do campo crp_controleTempo
    /// </summary>
    private byte _VS_crp_controleTempo
    {
        get
        {
            if (ViewState["_VS_crp_controleTempo"] != null)
            {
                return Convert.ToByte(ViewState["_VS_crp_controleTempo"]);
            }

            return 0;
        }

        set
        {
            ViewState["_VS_crp_controleTempo"] = value;
        }
    }

    /// <summary>
    /// Armazena o valor do campo crd_tipo
    /// </summary>
    private byte _VS_crd_tipo
    {
        get
        {
            if (ViewState["_VS_crd_tipo"] != null)
            {
                return Convert.ToByte(ViewState["_VS_crd_tipo"]);
            }

            return 0;
        }

        set
        {
            ViewState["_VS_crd_tipo"] = value;
        }
    }

    /// <summary>
    /// Armazena as disciplinas eletivas do aluno.
    /// </summary>
    private List<ACA_CurriculoDisciplina_Cadastro> VS_List_ACA_CurriculoDisciplina_Cadastro
    {
        get
        {
            if (ViewState["VS_List_ACA_CurriculoDisciplina_Cadastro"] == null)
            {
                ViewState["VS_List_ACA_CurriculoDisciplina_Cadastro"] = new List<ACA_CurriculoDisciplina_Cadastro>();
            }

            return (List<ACA_CurriculoDisciplina_Cadastro>)ViewState["VS_List_ACA_CurriculoDisciplina_Cadastro"];
        }

        set
        {
            ViewState["VS_List_ACA_CurriculoDisciplina_Cadastro"] = value;
        }
    }

    /// <summary>
    /// Armazena o id da disciplina.
    /// </summary>
    private int VS_dis_id
    {
        get
        {
            if (ViewState["VS_dis_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_dis_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["VS_dis_id"] = value;
        }
    }

    /// <summary>
    /// Indica se o usuário tem permissão total de alteração na tela
    /// </summary>
    private bool alteracaoTotalUsuarioAdmin
    {
        get
        {
            return __SessionWEB.__UsuarioWEB.Usuario.usu_login.ToUpper() == "ADMIN" && ApplicationWEB.LiberarAlteracaoTotalAdmin;
        }
    }

    #endregion Propriedades

    #region Page life cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCurso.js"));
        }

        if (!IsPostBack)
        {
            _VS_crr_id = 1;
            _VS_disciplinas = null;

            try
            {
                chkExclusivoDeficiente.Text = string.Format("{0} exclusivo(a) para aluno(s) {1}", GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id), ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower());
                chknaoCausaConflitoSR.Text = "Não causa conflito de horário do turno com matrícula em " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();

                UCComboTipoNivelEnsino1.Obrigatorio = true;
                UCComboTipoNivelEnsino1.ValidationGroup = "Curso";
                UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();

                UCComboTipoModalidadeEnsino1.Obrigatorio = true;
                UCComboTipoModalidadeEnsino1.ValidationGroup = "Curso";
                UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();

                UCComboTipoNivelEnsino2.Titulo = "Próximo nível de ensino";
                UCComboTipoNivelEnsino2.CarregarTipoNivelEnsino();

                UCComboTipoCiclo.Carregar();

                UCComboTipoCurriculoPeriodo.Obrigatorio = false;
                UCComboTipoCurriculoPeriodo.Titulo = "Tipo de" + " " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
                UCComboTipoCurriculoPeriodo.MostrarMessageSelecione = true;
                UCComboTipoCurriculoPeriodo.PermiteEditar = true;

                // Se o parâmetro disciplina eletiva do aluno não estiver setado, não exibe essa aba de cadastro
                string stds_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_DISCIPLINA_ELETIVA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (string.IsNullOrEmpty(stds_id))
                {
                    _lblEletivasAlunos.Visible = false;
                    _txtEletivasAlunos.Visible = false;
                    aDisciplinasEletivasAlunos.Visible = false;
                    divGridEletivasAlunos.Visible = false;
                    divDadosEletivasAlunos.Visible = false;
                }

                divFundoFrente.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_FUNDO_CARTEIRINHA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
            {
                _VS_cur_id = PreviousPage.EditItem;
                _LoadFromEntity();
            }
            else
            {
                UCComboTipoDisciplina1.Obrigatorio = true;
                UCComboTipoDisciplina1.ValidationGroup = "Disciplina";
                UCComboTipoDisciplina1.CarregarTipoDisciplina();

                _VS_VerificaTurma = false;
                _VS_VerificaAlunoCurriculo = false;

                VS_List_ACA_CurriculoDisciplina_Cadastro = null;

                grvEletivasAlunos.DataSource = new DataTable();
                grvEletivasAlunos.DataBind();
            }

            #region SetaNomeCurso_e_CursoPeriodo

            aDadosCurso.InnerText = "Dados do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            LabelCodigo.Text = "Código do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            LabelNome.Text = "Nome do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
            _rfvNomeCurso.ErrorMessage = "Nome do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " é obrigatório.";

            LabelPeriodosNormal.Text = "Quantidade normal de " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
            _rfvPeriodosNormal.ErrorMessage = LabelPeriodosNormal.Text.Replace("*", string.Empty) + " é obrigatório.";
            _cpvPeriodosNormal.ErrorMessage = "Quantidade normal de " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " não pode ser maior do que 255.";
            pnlCursosRelacionados.GroupingText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " equivalentes";
            
            divPeriodo.Attributes["title"] = "Cadastro de " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();

            divDisciplina.Attributes["title"] = "Cadastro de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA");
            divDisciplinaPeriodo.Attributes["title"] = "Cadastro de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " para o período";

            LabelTipoDisciplina.Text = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " para o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
            _cpvTipoDisciplinaTemposAula.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " para o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " é obrigatório.";

            LabelLegenda.Text = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " para o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ":";

            _lblOrdemPeriodo.Text = "Ordem do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
            _rfvOrdemPeriodo.ErrorMessage = "Ordem do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " é obrigatório.";
            cpvOrdemPeriodo.ErrorMessage = "Ordem do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " não pode ser maior do que 999.";

            pnlPeriodosCursoEletivasAlunos.GroupingText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            grvEletivasAlunos.Columns[PosPeriodoCursoEletivasAlunos].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();

            #endregion SetaNomeCurso_e_CursoPeriodo
        }

        _LoadGridCurriculo();

        UCComboTipoDisciplina1.IndexChanged += UCComboTipoDisciplina1__IndexChanged;
        UCComboTipoNivelEnsino1.IndexChanged += UCComboTipoNivelEnsino1_IndexChanged;

        cpvEletivasAlunos.ErrorMessage = "Quantidade de tempos de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " eletivos(as) não pode ser maior do que 255.";

        HabilitaControles(_updCadastroPeriodo.Controls, false);
        HabilitaControles(_updCadastroDisciplina.Controls, false);
        HabilitaControles(_updCadastroDisciplinaPeriodo.Controls, false);
        HabilitaControles(updCadastroEletivasAlunos.Controls, false);
        HabilitaControles(_updDadosCurso.Controls, false);
        btnCancelarEletivasAlunos.Enabled = _btnCancelarDisciplina.Enabled = _btnCancelar.Enabled = _btnCancelarDisciplinaPeriodo.Enabled = _btnCancelarPeriodo.Enabled = true;
    }

    #endregion Page life cycle

    #region Eventos

    #region PERIODOS

    protected void _ddlControleTempo_SelectedIndexChanged(object sender, EventArgs e)
    {
        _txtQtdeTemposDia.Text = string.Empty;
        _txtCargaHorariaHora.Text = string.Empty;
        _txtCargaHorariaMinuto.Text = string.Empty;

        divTemposAula.Visible = false;
        divHoras.Visible = false;

        if (Convert.ToInt32(_ddlControleTempo.SelectedValue) == Convert.ToInt32(ACA_CurriculoPeriodoControleTempo.TemposAula))
        {
            divTemposAula.Visible = true;
        }
        else if (Convert.ToInt32(_ddlControleTempo.SelectedValue) == Convert.ToInt32(ACA_CurriculoPeriodoControleTempo.Horas))
        {
            divHoras.Visible = true;
        }

        _updCadastroPeriodo.Update();
    }

    protected void _btnCancelarPeriodo_Click(object sender, EventArgs e)
    {
        _LimparCamposPeriodos();
    }

    #endregion PERIODOS

    #region DISCIPLINAS

    protected void _grvDisciplinas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            ImageButton btnDelete = (ImageButton)e.Row.FindControl("_btnExcluirDisciplina");
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = e.Row.Cells[0].Text;
            }
        }
    }

    #endregion DISCIPLINAS

    #region DISCIPLINASPERIODOS

    protected void _ddlTipoDisciplina_SelectedIndexChanged(object sender, EventArgs e)
    {
        fdsEletivas.Visible = false;
        chkDisciplinasEletivas.Items.Clear();

        _rfvCargaHorariaSemanal.Visible = false;
        LabelCargaHorariaSemanal.Text = "Carga horária semanal";
        divCargaHoraria.Visible = true;

        _txtNomeGrupoEletivas.Text = string.Empty;
        _txtCargaHorariaSemanal.Text = string.Empty;
        _txtCargaHorariaAnual.Text = string.Empty;

        // Esconde os campos carga horária semanal e carga horária anual se o tipo for Disciplina principal
        if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal))
        {
            divCargaHoraria.Visible = false;
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.Eletiva))
        {
            // Carrega as disciplinas eletivas do tipo Eletiva
            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipoGrid = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipoGrid == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.Eletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        chkDisciplinasEletivas.Items.Add(li);

                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva))
        {
            // Carrega as disciplinas eletivas do tipo Docente da turma e docente específico – eletiva
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";

            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipoGrid = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipoGrid == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        chkDisciplinasEletivas.Items.Add(li);

                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva))
        {
            // Carrega as disciplinas eletivas do tipo Depende da disponibilidade de professor – eletiva
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";

            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipoGrid = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipoGrid == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        chkDisciplinasEletivas.Items.Add(li);

                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorObrigatoria)
            || Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DocenteTurmaObrigatoria))
        {
            // Deixa obrigatório o campo Carga Horária Semanal se o tipo for
            // Docente da turma e docente específico – obrigatória ou Depende da disponibilidade de professor – obrigatória
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.Regencia))
        {
            divCargaHoraria.Visible = false;
        }

        _ddlTipoDisciplina.Focus();
    }

    protected void _grvCurriculo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "hide";
            e.Row.Cells[1].CssClass = "hide";

            DataTable dt;
            if (_VS_periodos.Rows.Count > 0)
            {
                _VS_periodos.DefaultView.Sort = "crp_ordem";
                dt = _VS_periodos.DefaultView.ToTable();
            }
            else
            {
                dt = _VS_periodos.Copy();
            }

            dt.AcceptChanges();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted)
                {
                    LinkButton periodo = new LinkButton
                                             {
                                                 CommandArgument = dt.Rows[i]["crp_id"].ToString(),
                                                 CommandName = "AlterarPeriodo",
                                                 Text = dt.Rows[i]["crp_descricao"].ToString(),
                                                 CausesValidation = false,
                                                 ForeColor = System.Drawing.Color.White
                                             };

                    e.Row.Cells[i + 4].Controls.Add(periodo);
                    e.Row.Cells[i + 4].CssClass = "center";
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].CssClass = "hide";
            e.Row.Cells[1].CssClass = "hide";

            DataTable dt;
            if (_VS_periodos.Rows.Count > 0)
            {
                _VS_periodos.DefaultView.Sort = "crp_ordem";
                dt = _VS_periodos.DefaultView.ToTable();
            }
            else
            {
                dt = _VS_periodos.Copy();
            }

            dt.AcceptChanges();

            string dis_nome = Convert.ToString(DataBinder.Eval(e.Row.DataItem, GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString())).Trim();
            string stds_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tds_id")).Trim();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted)
                {
                    if (!string.IsNullOrEmpty(stds_id))
                    {
                        LinkButton disciplinaperiodo = new LinkButton
                        {
                            CommandArgument = e.Row.Cells[0].Text + ";" + dt.Rows[i]["crp_id"] + ";" + dis_nome.Trim(),
                            CommandName = "AlterarDisciplinaPeriodo",
                            Text = e.Row.Cells[i + 4].Text,
                            CausesValidation = false,
                            ToolTip = "Alterar a ligação do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " " + dis_nome + " com o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " " + dt.Rows[i]["crp_descricao"] + "."
                        };

                        int tds_id = Convert.ToInt32(stds_id);
                        int crp_id = Convert.ToInt32(dt.Rows[i]["crp_id"].ToString());

                        var x = from DataRow dr in _VS_DisciplinasPeriodo.Rows
                                where (dr.RowState != DataRowState.Deleted && Convert.ToInt32(dr["tds_id"]) == tds_id && Convert.ToInt32(dr["crp_id"]) == crp_id && dr["tds_nome"].ToString().Trim() + " - " + dr["dis_nome"].ToString().Trim() == dis_nome.Trim())
                                select Convert.ToByte(dr["crd_tipo"].ToString());

                        byte crd_tipo;
                        if (x.Count() > 0)
                        {
                            crd_tipo = x.First();
                        }
                        else
                        {
                            crd_tipo = 0;
                        }

                        if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Obrigatoria))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DodgerBlue;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Optativa))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Eletiva))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkCyan;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Fuchsia;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenteTurmaObrigatoria))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.LimeGreen;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkGoldenrod;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorObrigatoria))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Purple;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Salmon;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkOrange;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Maroon;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenteEspecificoComplementacaoRegencia))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkGreen;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DisciplinaMultisseriada))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Sienna;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.MultisseriadaAluno))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkKhaki;
                        }
                        else if (crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada))
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.DarkBlue;
                        }
                        else
                        {
                            disciplinaperiodo.ForeColor = System.Drawing.Color.Black;
                        }

                        e.Row.Cells[i + 4].Controls.Add(disciplinaperiodo);
                        e.Row.Cells[i + 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }

            LinkButton disciplina = new LinkButton
            {
                CommandArgument = stds_id + ";" + dis_nome.Trim(),
                CommandName = "AlterarDisciplina",
                Text = e.Row.Cells[3].Text,
                CausesValidation = false
            };

            e.Row.Cells[3].Controls.Add(disciplina);
        }
    }

    protected void _grvCurriculo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AlterarPeriodo")
        {
            _VS_id = int.Parse(e.CommandArgument.ToString());
            UCComboTipoCurriculoPeriodo.CarregarPorNivelEnsinoModalidade(UCComboTipoNivelEnsino1.Valor, UCComboTipoModalidadeEnsino1.Valor);
            _AlterarPeriodoGrid();
        }
        else if (e.CommandName == "AlterarDisciplina")
        {
            _VS_id = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
            _VS_dis_nome = e.CommandArgument.ToString().Split(';')[1].Trim();

            _AlterarDisciplinaGrid();
        }
        else if (e.CommandName == "AlterarDisciplinaPeriodo")
        {
            _VS_id = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
            _VS_id2 = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
            _VS_dis_nome = e.CommandArgument.ToString().Split(';')[2].Trim();

            _AlterarDisciplinaPeriodoGrid();
        }
    }

    #endregion DISCIPLINASPERIODOS

    #region ELETIVAS ALUNOS

    protected void grvEletivasAlunos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ACA_CurriculoDisciplina_Cadastro cad = (ACA_CurriculoDisciplina_Cadastro)e.Row.DataItem;

            LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
            if (btnAlterar != null)
            {
                btnAlterar.CommandArgument = cad.entityDisciplina.dis_id.ToString();
                btnAlterar.Text = cad.entityDisciplina.dis_nome;
            }

            Label lblPeriodos = (Label)e.Row.FindControl("lblPeriodos");
            if (lblPeriodos != null)
            {
                lblPeriodos.Text = string.Empty;
                foreach (ACA_CurriculoDisciplina crd in cad.listCurriculoDisciplina)
                {
                    var x = from DataRow dr in _VS_periodos.Rows
                            where dr.RowState != DataRowState.Deleted && Convert.ToInt32(dr["crp_id"]) == crd.crp_id
                            select dr["crp_descricao"].ToString();

                    if (x.Count() > 0)
                    {
                        lblPeriodos.Text += x.First() + "<BR />";
                    }
                }
            }

            Label lblSituacao = (Label)e.Row.FindControl("lblSituacao");
            if (lblSituacao != null)
            {
                lblSituacao.Text = cad.entityDisciplina.dis_situacaoDescricao;
            }

            ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
            if (btnExcluir != null)
            {
                btnExcluir.CommandArgument = cad.entityDisciplina.dis_id.ToString();
            }
        }
    }

    protected void grvEletivasAlunos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        VS_dis_id = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName == "Alterar")
        {
            AlterarEletivasAlunosGrid();
        }
    }
    
    #endregion ELETIVAS ALUNOS

    protected void _ddlRegimeMatricula_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            _txtQtdeAvaliacoes.Text = string.Empty;
            _txtNomeAvaliacao.Text = string.Empty;

            if (Convert.ToInt32(_ddlRegimeMatricula.SelectedValue) == (int)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                divQtdeAvaliacoes.Visible = true;
                divTurmaAvaliacao.Visible = true;
                chkTurmaAvaliacao.Visible = true;
            }
            else
            {
                divQtdeAvaliacoes.Visible = false;
                divTurmaAvaliacao.Visible = false;
                chkTurmaAvaliacao.Visible = false;

                for (int i = 0; i < _VS_periodos.Rows.Count; i++)
                {
                    if (_VS_periodos.Rows[i].RowState != DataRowState.Deleted)
                    {
                        _VS_periodos.Rows[i]["crp_nomeAvaliacao"] = string.Empty;
                        _VS_periodos.Rows[i]["crp_turmaAvaliacao"] = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar alterar o regime de matrícula.", UtilBO.TipoMensagem.Erro);
            _lblMessage.Visible = true;
            ApplicationWEB._GravaErro(ex);
        }
        finally
        {
            _updDadosCurso.Update();
        }
    }

    protected void chkTurmaAvaliacao_CheckedChanged(object sender, EventArgs e)
    {
        _txtNomeAvaliacao.Text = string.Empty;
        divTurmaAvaliacao.Visible = chkTurmaAvaliacao.Checked;
        _updCadastroPeriodo.Update();
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Curso/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion Eventos

    #region Delegates

    private void UCComboTipoDisciplina1__IndexChanged()
    {
        _txtNomeDisciplina.Text = UCComboTipoDisciplina1.Valor > 0 ? UCComboTipoDisciplina1.Texto : string.Empty;

        UCComboTipoDisciplina1.SetarFoco();
    }

    private void UCComboTipoNivelEnsino1_IndexChanged()
    {
        UCComboTipoDisciplina1.CarregarTipoDisciplinaPorNivelEnsino(UCComboTipoNivelEnsino1.Valor);
        UCComboTipoNivelEnsino1.SetarFoco();

        LoadCursoRelacionado(UCComboTipoNivelEnsino1.Valor);
    }

    #endregion Delegates

    #region Métodos

    #region LOAD / SAVE

    /// <summary>
    /// Carrega os dados do curso nos controles caso seja alteração.
    /// </summary>
    private void _LoadFromEntity()
    {
        try
        {
            // Carrega Cursos
            ACA_Curso cur = new ACA_Curso { cur_id = _VS_cur_id };
            ACA_CursoBO.GetEntity(cur);

            if (cur.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(string.Format("O(A) {0} não pertence à entidade na qual você está logado.", GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Curso/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            UCComboTipoNivelEnsino1.Valor = cur.tne_id == 0 ? -1 : cur.tne_id;
            UCComboTipoModalidadeEnsino1.Valor = cur.tme_id == 0 ? -1 : cur.tme_id;

            _txtNome.Text = cur.cur_nome;
            _txtNomeAbreviado.Text = cur.cur_nome_abreviado;
            _txtCodigo.Text = cur.cur_codigo;
            _chkConcluiNivelEnsino.Checked = cur.cur_concluiNivelEnsino;
            chkExclusivoDeficiente.Checked = cur.cur_exclusivoDeficiente;
            chknaoCausaConflitoSR.Checked = cur.cur_conflitoTurnoSR;
            UCComboTipoNivelEnsino2.Valor = cur.tne_idProximo == 0 ? -1 : cur.tne_idProximo;
            _txtVigenciaIni.Text = cur.cur_vigenciaInicio.ToString("dd/MM/yyy");
            _txtVigenciaFim.Text = cur.cur_vigenciaFim == new DateTime() ? string.Empty : cur.cur_vigenciaFim.ToString("dd/MM/yyy");
            _ddlCursoSituacao.SelectedValue = cur.cur_situacao.ToString();
            txtCargaHoraria.Text = cur.cur_cargaHoraria.ToString();

            chkEfetivacaoSemestral.Checked = cur.cur_efetivacaoSemestral;

            UCComboTipoDisciplina1.Obrigatorio = true;
            UCComboTipoDisciplina1.ValidationGroup = "Disciplina";
            UCComboTipoDisciplina1.CarregarTipoDisciplinaPorNivelEnsino(cur.tne_id);

            ACA_Curriculo crr = new ACA_Curriculo { cur_id = _VS_cur_id, crr_id = _VS_crr_id };
            ACA_CurriculoBO.GetEntity(crr);

            _ddlRegimeMatricula.SelectedValue = crr.crr_regimeMatricula.ToString();
            _txtPeriodosNormal.Text = crr.crr_periodosNormal.ToString();
            _txtDiasLetivos.Text = crr.crr_diasLetivos.ToString();

            if (crr.crr_regimeMatricula == (int)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                divQtdeAvaliacoes.Visible = true;
                divTurmaAvaliacao.Visible = true;
                chkTurmaAvaliacao.Visible = true;
                _txtQtdeAvaliacoes.Text = crr.crr_qtdeAvaliacaoProgressao.ToString();
            }
            else
            {
                divQtdeAvaliacoes.Visible = false;
                divTurmaAvaliacao.Visible = false;
                chkTurmaAvaliacao.Visible = false;
                _txtQtdeAvaliacoes.Text = string.Empty;
            }

            LoadCursoRelacionado(cur.tne_id);

            // Carrega Periodos do Curriculo
            DataTable dtPeriodos = ACA_CurriculoPeriodoBO.GetSelect(_VS_cur_id, _VS_crr_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, false, 1, 1);
            if (dtPeriodos.Rows.Count == 0)
            {
                dtPeriodos = null;
            }

            _VS_periodos = dtPeriodos;
            _VS_seqPeriodo = ACA_CurriculoPeriodoBO.VerificaUltimoPeriodoCadastrado(_VS_cur_id, _VS_crr_id) - 1;

            // Carrega Disciplinas
            DataTable dtDisciplinas = ACA_CurriculoDisciplinaBO.GetSelect_Disciplinas(_VS_cur_id, _VS_crr_id, false, 1, 1);
            if (dtDisciplinas.Rows.Count == 0)
            {
                dtDisciplinas = null;
            }

            _VS_disciplinas = dtDisciplinas;

            // Carrega Disciplinas do Curriculo
            DataTable dtDisciplinasCurriculo = ACA_CurriculoDisciplinaBO.GetSelect(_VS_cur_id, _VS_crr_id, false, 1, 1);
            if (dtDisciplinasCurriculo.Rows.Count == 0)
            {
                dtDisciplinasCurriculo = null;
            }

            _VS_DisciplinasPeriodo = dtDisciplinasCurriculo;

            // Carrega Disciplinas Eletivas do Curriculo
            DataTable dtDisciplinasEletivas = ACA_CurriculoDisciplinaEletivaBO.GetSelect(_VS_cur_id, _VS_crr_id, false, 1, 1);
            if (dtDisciplinasEletivas.Rows.Count == 0)
            {
                dtDisciplinasEletivas = null;
            }

            _VS_DisciplinasEletivas = dtDisciplinasEletivas;

            _VS_VerificaAlunoCurriculo = ACA_CursoBO.VerificaAlunoCurriculo(_VS_cur_id, _VS_crr_id);
            _VS_VerificaTurma = ACA_CursoBO.VerificaTurma(_VS_cur_id, _VS_crr_id);

            // Se existir turma ligada ao curso deixa alterar apenas
            // nome, código, nome abreviado, próximo nível de ensino, vigência final e situação
            if (_VS_VerificaTurma && !alteracaoTotalUsuarioAdmin)
            {
                _ddlRegimeMatricula.Enabled = false;
                _txtQtdeAvaliacoes.Enabled = false;
                _txtPeriodosNormal.Enabled = false;
                _txtDiasLetivos.Enabled = false;
            }

            // Se existir aluno currículo não deixa alterar o campo
            // exclusivo aluno deficiente.
            if (_VS_VerificaAlunoCurriculo && !alteracaoTotalUsuarioAdmin)
            {
                chkExclusivoDeficiente.Enabled = false;
            }

            // Desabilita campos.
            _txtVigenciaIni.Enabled = cur.cur_vigenciaInicio.Date > DateTime.Now.Date;
            UCComboTipoNivelEnsino1.PermiteEditar = false;
            UCComboTipoModalidadeEnsino1.PermiteEditar = false;

            grvEletivasAlunos.Columns[PosPeriodoCursoEletivasAlunos].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();

            // Carrega as disciplinas eletivas do aluno
            VS_List_ACA_CurriculoDisciplina_Cadastro = ACA_CurriculoDisciplinaBO.SelecionaDisciplinasEletivasAlunos(_VS_cur_id, _VS_crr_id);

            grvEletivasAlunos.DataSource = VS_List_ACA_CurriculoDisciplina_Cadastro;
            grvEletivasAlunos.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar o(a) {0}.", GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Metodo para carregar os cursos com o mesmo nível de ensino.
    /// </summary>
    /// <param name="tne_id">Id do tipo_nivel_ensino.</param>
    private void LoadCursoRelacionado(int tne_id)
    {
        try
        {
            DataTable dtCursoRel = ACA_CursoBO.SelectCursoRelacionadoNivelEnsino(_VS_cur_id, _VS_crr_id, tne_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            List<string> listRel = ACA_CursoRelacionadoBO.SelecionaCursosRelacionados(_VS_cur_id, _VS_crr_id);

            cklCursoEquivalente.DataSource = dtCursoRel;
            cklCursoEquivalente.DataBind();

            foreach (ListItem index in cklCursoEquivalente.Items)
            {
                if (listRel.Contains(index.Value))
                {
                    index.Selected = true;
                }
            }

            pnlCursosRelacionados.Visible = dtCursoRel.Rows.Count > 0;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar a lista de {0} equivalentes.", GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion LOAD / SAVE

    #region PERIODOS

    /// <summary>
    /// limpa os campos da area de curriculo periodo
    /// </summary>
    private void _LimparCamposPeriodos()
    {
        _lblMessagePeriodo.Visible = false;
        divTurmaAvaliacao.Visible = false;

        _txtDescricaoPeriodo.Text = string.Empty;
        chkCrpConcluiNivelEnsino.Checked = false;
        _txtOrdemPeriodo.Text = string.Empty;
        _txtIdadeIdealAnoInicio.Text = string.Empty;
        _txtIdadeIdealMesInicio.Text = string.Empty;
        _txtIdadeIdealAnoFim.Text = string.Empty;
        _txtIdadeIdealMesFim.Text = string.Empty;
        _txtQtdeDiasSemana.Text = string.Empty;
        _txtQtdeTemposDia.Text = string.Empty;
        _txtCargaHorariaHora.Text = string.Empty;
        _txtCargaHorariaMinuto.Text = string.Empty;
        _txtEletivasAlunos.Text = string.Empty;
        _txtNomeAvaliacao.Text = string.Empty;
        chkTurmaAvaliacao.Checked = false;
        txtFundoFrente.Text = string.Empty;

        _ddlControleTempo.SelectedValue = "-1";
        _ddlControleTempo.Enabled = true;

        UCComboTipoCiclo.SelectedValue = "-1";

        divTemposAula.Visible = false;
        divHoras.Visible = false;
    }

    /// <summary>
    /// Altera o curriculo perido no grid.
    /// </summary>
    private void _AlterarPeriodoGrid()
    {
        _ddlControleTempo.Enabled = true;

        divTemposAula.Visible = false;
        divHoras.Visible = false;

        // Carrega os dados do período
        for (int i = 0; i < _VS_periodos.Rows.Count; i++)
        {
            if (_VS_periodos.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_periodos.Rows[i]["crp_id"].ToString() == Convert.ToString(_VS_id))
                {
                    _txtDescricaoPeriodo.Text = _VS_periodos.Rows[i]["crp_descricao"].ToString();
                    chkCrpConcluiNivelEnsino.Checked = Convert.ToBoolean(_VS_periodos.Rows[i]["crp_concluiNivelEnsino"].ToString());
                    _txtOrdemPeriodo.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_ordem"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_ordem"].ToString();
                    _txtIdadeIdealAnoInicio.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_idadeIdealAnoInicio"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_idadeIdealAnoInicio"].ToString();
                    _txtIdadeIdealMesInicio.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_idadeIdealMesInicio"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_idadeIdealMesInicio"].ToString();
                    _txtIdadeIdealAnoFim.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_idadeIdealAnoFim"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_idadeIdealAnoFim"].ToString();
                    _txtIdadeIdealMesFim.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_idadeIdealMesFim"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_idadeIdealMesFim"].ToString();
                    _ddlControleTempo.SelectedValue = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_controleTempo"].ToString()) || _VS_periodos.Rows[i]["crp_controleTempo"].ToString() == "0" ? "-1" : _VS_periodos.Rows[i]["crp_controleTempo"].ToString();
                    _txtQtdeDiasSemana.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_qtdeDiasSemana"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_qtdeDiasSemana"].ToString();
                    _txtQtdeTemposDia.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_qtdeTemposDia"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_qtdeTemposDia"].ToString();
                    _txtCargaHorariaHora.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_qtdeHorasDia"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_qtdeHorasDia"].ToString();
                    _txtCargaHorariaMinuto.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_qtdeMinutosDia"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_qtdeMinutosDia"].ToString();
                    _txtEletivasAlunos.Text = string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_qtdeEletivasAlunos"].ToString()) || _VS_periodos.Rows[i]["crp_qtdeEletivasAlunos"].ToString() == "0" ? string.Empty : _VS_periodos.Rows[i]["crp_qtdeEletivasAlunos"].ToString();
                    txtFundoFrente.Text = _VS_periodos.Rows[i]["crp_fundoFrente"].ToString();
                    UCComboTipoCiclo.SelectedValue = string.IsNullOrEmpty(_VS_periodos.Rows[i]["tci_id"].ToString()) ? "-1" : _VS_periodos.Rows[i]["tci_id"].ToString();
                    UCComboTipoCurriculoPeriodo.Valor = string.IsNullOrEmpty(_VS_periodos.Rows[i]["tcp_id"].ToString()) ? -1 : Convert.ToInt32(_VS_periodos.Rows[i]["tcp_id"]);
                    _txtNomeAvaliacao.Text = _VS_periodos.Rows[i]["crp_nomeAvaliacao"].ToString();
                    chkTurmaAvaliacao.Checked = Convert.ToBoolean(_VS_periodos.Rows[i]["crp_turmaAvaliacao"].ToString());

                    if (Convert.ToInt32(_ddlControleTempo.SelectedValue) == Convert.ToInt32(ACA_CurriculoPeriodoControleTempo.TemposAula))
                    {
                        divTemposAula.Visible = true;
                    }
                    else if (Convert.ToInt32(_ddlControleTempo.SelectedValue) == Convert.ToInt32(ACA_CurriculoPeriodoControleTempo.Horas))
                    {
                        divHoras.Visible = true;
                    }

                    divTurmaAvaliacao.Visible = chkTurmaAvaliacao.Checked;

                    break;
                }
            }
        }

        // Verifica se já existe alguma ligação da disciplina com o período
        var x = from DataRow dr in _VS_DisciplinasPeriodo.Rows
                where dr.RowState != DataRowState.Deleted && Convert.ToInt32(dr["crd_tipo"]) > 0 && Convert.ToInt32(dr["crp_id"]) == _VS_id
                select Convert.ToInt32(dr["crp_id"].ToString());

        if (x.Count() > 0)
        {
            _ddlControleTempo.Enabled = false;
        }

        // Se existir turma ligada ao curso ou aluno curriculo
        // não deixa alterar a matriz de curso/disciplina
        if ((_VS_VerificaTurma || _VS_VerificaAlunoCurriculo) && !alteracaoTotalUsuarioAdmin)
        {
            _txtDescricaoPeriodo.Enabled = false;
            _txtIdadeIdealAnoInicio.Enabled = false;
            _txtIdadeIdealMesInicio.Enabled = false;
            _txtIdadeIdealAnoFim.Enabled = false;
            _txtIdadeIdealMesFim.Enabled = false;
            _ddlControleTempo.Enabled = false;
            _txtQtdeDiasSemana.Enabled = false;
            _txtQtdeTemposDia.Enabled = false;
            _txtCargaHorariaHora.Enabled = false;
            _txtCargaHorariaMinuto.Enabled = false;
            chkTurmaAvaliacao.Enabled = false;
        }
        
        _txtDescricaoPeriodo.Focus();
        _updCadastroPeriodo.Update();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroTurnoAlterar", "$('.divPeriodo').dialog('open');", true);
    }

    #endregion PERIODOS

    #region DISCIPLINAS

    /// <summary>
    /// Altera a disciplina do grid.
    /// </summary>
    private void _AlterarDisciplinaGrid()
    {
        UCComboTipoDisciplina1.PermiteEditar = false;

        for (int i = 0; i < _VS_disciplinas.Rows.Count; i++)
        {
            if (_VS_disciplinas.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplinas.Rows[i]["tds_id"].ToString() == _VS_id.ToString() &&
                    string.Format("{0} - {1}", _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim(), _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim()) == _VS_dis_nome.Trim())
                {
                    UCComboTipoDisciplina1.Valor = Convert.ToInt32(_VS_disciplinas.Rows[i]["tds_id"].ToString());
                    _txtCodigoDisciplina.Text = _VS_disciplinas.Rows[i]["dis_codigo"].ToString();
                    _txtNomeDisciplina.Text = _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim();
                    _txtNomeAbreviadoDisciplina.Text = _VS_disciplinas.Rows[i]["dis_nomeAbreviado"].ToString().Trim();

                    break;
                }
            }
        }

        // Se existir turma ligada ao curso ou aluno curriculo
        // não deixa alterar a matriz de curso/disciplina
        if ((_VS_VerificaTurma || _VS_VerificaAlunoCurriculo) && !alteracaoTotalUsuarioAdmin)
        {
            UCComboTipoDisciplina1.PermiteEditar = false;
            _txtNomeDisciplina.Enabled = false;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                _txtNomeAbreviadoDisciplina.Enabled = false;
                _txtCodigoDisciplina.Enabled = false;
            }
        }

        _txtNomeDisciplina.Focus();
        _updCadastroDisciplina.Update();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroDisciplinaAlterar", "$('.divDisciplina').dialog('open');", true);
    }

    #endregion DISCIPLINAS

    #region DISCIPLINASPERIODOS

    /// <summary>
    /// Limpa os campos da disciplina no periodo
    /// </summary>
    private void _LimparCamposDisciplinasPeriodos()
    {
        _lblMessageDisciplinaPeriodo.Visible = false;

        _ddlTipoDisciplina.SelectedValue = "-1";
        _txtCargaHorariaSemanal.Text = string.Empty;
        _txtCargaHorariaAnual.Text = string.Empty;
        _txtEmenta.Text = string.Empty;
        _txtNomeGrupoEletivas.Text = string.Empty;
        _lblDisciplina.Text = string.Empty;

        fdsEletivas.Visible = false;
        chkDisciplinasEletivas.Items.Clear();

        _rfvCargaHorariaSemanal.Visible = false;
        LabelCargaHorariaSemanal.Text = "Carga horária semanal";
        divCargaHoraria.Visible = true;
    }
    
    /// <summary>
    /// Altera a disciplina do curriculo periodo no grid
    /// </summary>
    private void _AlterarDisciplinaPeriodoGrid()
    {
        _VS_crd_tipo = 0;
        _VS_crp_controleTempo = 0;
        byte tipoDisciplinaSelecionada = 0;
        divCargaHoraria.Visible = true;

        _LimparCamposDisciplinasPeriodos();

        // Carrega os dados da ligação da disciplina com o período
        for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
        {
            if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString() == _VS_id.ToString() &&
                    _VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString() == _VS_id2.ToString() &&
                    _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim() == _VS_dis_nome.Trim())
                {
                    _txtEmenta.Text = _VS_DisciplinasPeriodo.Rows[i]["dis_ementa"].ToString();
                    _txtCargaHorariaSemanal.Text = _VS_DisciplinasPeriodo.Rows[i]["dis_cargaHorariaTeorica"].ToString() == "0" ? string.Empty : _VS_DisciplinasPeriodo.Rows[i]["dis_cargaHorariaTeorica"].ToString();
                    _txtCargaHorariaAnual.Text = _VS_DisciplinasPeriodo.Rows[i]["dis_cargaHorariaAnual"].ToString() == "0" ? string.Empty : _VS_DisciplinasPeriodo.Rows[i]["dis_cargaHorariaAnual"].ToString();
                    tipoDisciplinaSelecionada = Convert.ToByte(string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? "0" : _VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    _VS_crd_tipo = Convert.ToByte(string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? "0" : _VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());

                    break;
                }
            }
        }

        // Carrega os dados do período
        for (int i = 0; i < _VS_periodos.Rows.Count; i++)
        {
            if (_VS_periodos.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_periodos.Rows[i]["crp_id"].ToString() == _VS_id2.ToString())
                {
                    _lblDisciplina.Text = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": " + _VS_periodos.Rows[i]["crp_descricao"];
                    _VS_crp_controleTempo = Convert.ToByte(string.IsNullOrEmpty(_VS_periodos.Rows[i]["crp_controleTempo"].ToString()) ? "0" : _VS_periodos.Rows[i]["crp_controleTempo"].ToString());

                    break;
                }
            }
        }

        // Carrega os dados da disciplina
        for (int i = 0; i < _VS_disciplinas.Rows.Count; i++)
        {
            if (_VS_disciplinas.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplinas.Rows[i]["tds_id"].ToString() == _VS_id.ToString() &&
                    _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim() == _VS_dis_nome.Trim())
                {
                    _lblDisciplina.Text += "<br /> " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                            ": " + _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim();

                    break;
                }
            }
        }

        // Exibe para o usuário apenas os tipo de acordo com o controle de tempo do período
        _ddlTipoDisciplina.Items.Clear();
        if (_VS_crp_controleTempo == Convert.ToByte(ACA_CurriculoPeriodoControleTempo.TemposAula))
        {
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("-- Selecione um tipo --", "-1", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Obrigatória", "1", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem(GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " principal", "5", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Optativa", "3", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Eletiva", "4", true));
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ITENS_REGENCIA_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Regência", "11", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Componente da regência", "12", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docente específico – Complementação da regência", "13", true));
            }

            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Disciplina multisseriada", "14", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Disciplina multisseriada do aluno", "16", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docência compartilhada", "17", true));
            _ddlTipoDisciplina.SelectedValue = tipoDisciplinaSelecionada == 0 ? "-1" : tipoDisciplinaSelecionada.ToString();
        }
        else if (_VS_crp_controleTempo == Convert.ToByte(ACA_CurriculoPeriodoControleTempo.Horas))
        {
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("-- Selecione um tipo --", "-1", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem(GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " principal", "5", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Optativa", "3", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docente da turma e docente específico – obrigatória", "6", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docente da turma e docente específico – eletiva", "7", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Depende da disponibilidade de professor – obrigatória", "8", true));
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Depende da disponibilidade de professor – eletiva", "9", true));
            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ITENS_REGENCIA_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Regência", "11", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Componente da regência", "12", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docente específico – Complementação da regência", "13", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Disciplina multisseriada", "14", true));
                _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Disciplina multisseriada do aluno", "16", true));
            }
            _ddlTipoDisciplina.Items.Insert(_ddlTipoDisciplina.Items.Count, new ListItem("Docência compartilhada", "17", true));
            _ddlTipoDisciplina.SelectedValue = tipoDisciplinaSelecionada == 0 ? "-1" : tipoDisciplinaSelecionada.ToString();
        }

        // Esconde os campos carga horária semanal e carga horária anual se o tipo for Disciplina principal
        if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal))
        {
            divCargaHoraria.Visible = false;
        }
        else if (_VS_crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Eletiva))
        {
            // Carrega as disciplinas eletivas do tipo Eletiva
            int cde_id = 0;

            // verifica se existe disciplina eletiva cadastrada
            // se existir, recupera o cde_id
            for (int i = 0; i < _VS_DisciplinasEletivas.Rows.Count; i++)
            {
                if (_VS_DisciplinasEletivas.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["crp_id"].ToString());
                    string dis_nome = _VS_DisciplinasEletivas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[i]["dis_nome"].ToString().Trim();

                    if (tds_id == _VS_id &&
                        crp_id == _VS_id2 &&
                        dis_nome.Trim() == _VS_dis_nome.Trim())
                    {
                        cde_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["cde_id"].ToString());
                        break;
                    }
                }
            }

            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipo = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Eletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        for (int j = 0; j < _VS_DisciplinasEletivas.Rows.Count; j++)
                        {
                            if (_VS_DisciplinasEletivas.Rows[j].RowState != DataRowState.Deleted)
                            {
                                if (li.Value.Split(';')[0] == _VS_DisciplinasEletivas.Rows[j]["tds_id"].ToString() &&
                                    dis_nome.Trim() == _VS_DisciplinasEletivas.Rows[j]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[j]["dis_nome"].ToString().Trim() &&
                                    _VS_id2.ToString() == _VS_DisciplinasEletivas.Rows[j]["crp_id"].ToString() &&
                                    cde_id.ToString() == _VS_DisciplinasEletivas.Rows[j]["cde_id"].ToString())
                                {
                                    _txtNomeGrupoEletivas.Text = _VS_DisciplinasEletivas.Rows[j]["cde_nome"].ToString();
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }

                        chkDisciplinasEletivas.Items.Add(li);
                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (_VS_crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva))
        {
            // Carrega as disciplinas eletivas do tipo Docente da turma e docente específico – eletiva
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";

            int cde_id = 0;

            // verifica se existe disciplina eletiva cadastrada
            // se existir, recupera o cde_id
            for (int i = 0; i < _VS_DisciplinasEletivas.Rows.Count; i++)
            {
                if (_VS_DisciplinasEletivas.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["crp_id"].ToString());
                    string dis_nome = _VS_DisciplinasEletivas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[i]["dis_nome"].ToString().Trim();

                    if (tds_id == _VS_id &&
                        crp_id == _VS_id2 &&
                        dis_nome.Trim() == _VS_dis_nome.Trim())
                    {
                        cde_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["cde_id"].ToString());
                        break;
                    }
                }
            }

            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipo = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenteTurmaEletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        for (int j = 0; j < _VS_DisciplinasEletivas.Rows.Count; j++)
                        {
                            if (_VS_DisciplinasEletivas.Rows[j].RowState != DataRowState.Deleted)
                            {
                                if (li.Value.Split(';')[0] == _VS_DisciplinasEletivas.Rows[j]["tds_id"].ToString() &&
                                    dis_nome.Trim() == _VS_DisciplinasEletivas.Rows[j]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[j]["dis_nome"].ToString().Trim() &&
                                    _VS_id2.ToString() == _VS_DisciplinasEletivas.Rows[j]["crp_id"].ToString() &&
                                    cde_id.ToString() == _VS_DisciplinasEletivas.Rows[j]["cde_id"].ToString())
                                {
                                    _txtNomeGrupoEletivas.Text = _VS_DisciplinasEletivas.Rows[j]["cde_nome"].ToString();
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }

                        chkDisciplinasEletivas.Items.Add(li);
                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (_VS_crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva))
        {
            // Carrega as disciplinas eletivas do tipo Depende da disponibilidade de professor – eletiva
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";

            int cde_id = 0;

            // verifica se existe disciplina eletiva cadastrada
            // se existir, recupera o cde_id
            for (int i = 0; i < _VS_DisciplinasEletivas.Rows.Count; i++)
            {
                if (_VS_DisciplinasEletivas.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["crp_id"].ToString());
                    string dis_nome = _VS_DisciplinasEletivas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[i]["dis_nome"].ToString().Trim();

                    if (tds_id == _VS_id &&
                        crp_id == _VS_id2 &&
                        dis_nome.Trim() == _VS_dis_nome.Trim())
                    {
                        cde_id = Convert.ToInt32(_VS_DisciplinasEletivas.Rows[i]["cde_id"].ToString());
                        break;
                    }
                }
            }

            for (int i = 0; i < _VS_DisciplinasPeriodo.Rows.Count; i++)
            {
                if (_VS_DisciplinasPeriodo.Rows[i].RowState != DataRowState.Deleted)
                {
                    int tds_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["tds_id"].ToString());
                    int crp_id = Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[i]["crp_id"].ToString());
                    byte crd_tipo = string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString()) ? Convert.ToByte(0) : Convert.ToByte(_VS_DisciplinasPeriodo.Rows[i]["crd_tipo"].ToString());
                    string dis_nome = _VS_DisciplinasPeriodo.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[i]["dis_nome"].ToString().Trim();

                    if ((tds_id != _VS_id || dis_nome.Trim() != _VS_dis_nome.Trim()) && crp_id == _VS_id2 && crd_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorEletiva))
                    {
                        ListItem li = new ListItem
                        {
                            Value = tds_id + ";" + dis_nome.Trim(),
                            Text = dis_nome.Trim()
                        };

                        for (int j = 0; j < _VS_DisciplinasEletivas.Rows.Count; j++)
                        {
                            if (_VS_DisciplinasEletivas.Rows[j].RowState != DataRowState.Deleted)
                            {
                                if (li.Value.Split(';')[0] == _VS_DisciplinasEletivas.Rows[j]["tds_id"].ToString() &&
                                    dis_nome.Trim() == _VS_DisciplinasEletivas.Rows[j]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasEletivas.Rows[j]["dis_nome"].ToString().Trim() &&
                                    _VS_id2.ToString() == _VS_DisciplinasEletivas.Rows[j]["crp_id"].ToString() &&
                                    cde_id.ToString() == _VS_DisciplinasEletivas.Rows[j]["cde_id"].ToString())
                                {
                                    _txtNomeGrupoEletivas.Text = _VS_DisciplinasEletivas.Rows[j]["cde_nome"].ToString();
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }

                        chkDisciplinasEletivas.Items.Add(li);
                        fdsEletivas.Visible = true;
                    }
                }
            }
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DependeDisponibilidadeProfessorObrigatoria)
            || Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DocenteTurmaObrigatoria))
        {
            // Deixa obrigatório o campo Carga Horária Semanal se o tipo for
            // Docente da turma e docente específico – obrigatória ou Depende da disponibilidade de professor – obrigatória
            _rfvCargaHorariaSemanal.Visible = true;
            LabelCargaHorariaSemanal.Text = "Carga horária semanal *";
        }
        else if (Convert.ToInt32(_ddlTipoDisciplina.SelectedValue) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.Regencia))
        {
            divCargaHoraria.Visible = false;
        }

        // Se existir turma ligada ao curso ou aluno curriculo
        // não deixa alterar a matriz de curso/disciplina
        if ((_VS_VerificaTurma || _VS_VerificaAlunoCurriculo) && !alteracaoTotalUsuarioAdmin)
        {
            _ddlTipoDisciplina.Enabled = false;
            _txtEmenta.Enabled = false;
            _txtCargaHorariaSemanal.Enabled = false;
            _txtCargaHorariaAnual.Enabled = false;
            _txtNomeGrupoEletivas.Enabled = false;
            chkDisciplinasEletivas.Enabled = false;
        }

        _ddlTipoDisciplina.Focus();
        _updCadastroDisciplinaPeriodo.Update();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroDisciplinaPeriodoAlterar", "$('.divDisciplinaPeriodo').dialog('open');", true);
    }

    /// <summary>
    /// Carrega a matriz de períodos e disciplinas
    /// </summary>
    private void _LoadGridCurriculo()
    {
        DataTable dt = new DataTable();
        _grvCurriculo.DataSource = dt;
        _grvCurriculo.DataBind();

        dt.Columns.Add("tds_id");
        dt.Columns.Add("tds_base");
        dt.Columns.Add("Base");
        dt.Columns.Add(GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString());

        DataTable dtOrdenado;
        if (_VS_periodos.Rows.Count > 0)
        {
            _VS_periodos.DefaultView.Sort = "crp_ordem";
            dtOrdenado = _VS_periodos.DefaultView.ToTable();
        }
        else
        {
            dtOrdenado = _VS_periodos.Copy();
        }

        for (int i = 0; i < dtOrdenado.Rows.Count; i++)
        {
            if (dtOrdenado.Rows[i].RowState != DataRowState.Deleted)
            {
                dt.Columns.Add(dtOrdenado.Rows[i]["crp_descricao"].ToString());
            }
        }

        int contDisciplinas = 0;
        if (_VS_disciplinas.Rows.Count > 0)
        {
            for (int i = 0; i < _VS_disciplinas.Rows.Count; i++)
            {
                if (_VS_disciplinas.Rows[i].RowState != DataRowState.Deleted)
                {
                    contDisciplinas = contDisciplinas + 1;
                    break;
                }
            }
        }

        if (contDisciplinas == 0)
        {
            DataView dv = new DataView(dt);
            _grvCurriculo.DataSource = dv;
            _FixarGridVazia(_grvCurriculo);
        }
        else
        {
            for (int i = 0; i < _VS_disciplinas.Rows.Count; i++)
            {
                if (_VS_disciplinas.Rows[i].RowState != DataRowState.Deleted)
                {
                    DataRow dr = dt.NewRow();
                    dr["tds_id"] = _VS_disciplinas.Rows[i]["tds_id"];
                    dr["tds_base"] = _VS_disciplinas.Rows[i]["tds_base"];
                    dr["Base"] = _VS_disciplinas.Rows[i]["tds_baseDescricao"];
                    //dr["Disciplinas"] = _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim();
                    dr[GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString()] = _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim();

                    for (int j = 0; j < dtOrdenado.Rows.Count; j++)
                    {
                        if (dtOrdenado.Rows[j].RowState != DataRowState.Deleted)
                        {
                            if (_VS_DisciplinasPeriodo.Rows.Count <= 0)
                            {
                                dr[dtOrdenado.Rows[j]["crp_descricao"].ToString()] = "-";
                            }

                            for (int k = 0; k < _VS_DisciplinasPeriodo.Rows.Count; k++)
                            {
                                if (_VS_DisciplinasPeriodo.Rows[k].RowState != DataRowState.Deleted)
                                {
                                    if (_VS_DisciplinasPeriodo.Rows[k]["tds_id"].ToString() == _VS_disciplinas.Rows[i]["tds_id"].ToString() &&
                                        _VS_DisciplinasPeriodo.Rows[k]["crp_id"].ToString() == dtOrdenado.Rows[j]["crp_id"].ToString() &&
                                        _VS_DisciplinasPeriodo.Rows[k]["tds_nome"].ToString().Trim() + " - " + _VS_DisciplinasPeriodo.Rows[k]["dis_nome"].ToString().Trim() == _VS_disciplinas.Rows[i]["tds_nome"].ToString().Trim() + " - " + _VS_disciplinas.Rows[i]["dis_nome"].ToString().Trim())
                                    {
                                        if (_VS_DisciplinasPeriodo.Rows[k]["dis_cargaHorariaTotal"].ToString() != "-1" && !string.IsNullOrEmpty(_VS_DisciplinasPeriodo.Rows[k]["dis_cargaHorariaTotal"].ToString()))
                                        {
                                            dr[dtOrdenado.Rows[j]["crp_descricao"].ToString()] = _VS_DisciplinasPeriodo.Rows[k]["dis_cargaHorariaTotal"];
                                        }
                                        else
                                        {
                                            dr[dtOrdenado.Rows[j]["crp_descricao"].ToString()] = "-";
                                        }

                                        if (Convert.ToInt32(_VS_DisciplinasPeriodo.Rows[k]["crd_tipo"].ToString()) == Convert.ToInt32(ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal))
                                        {
                                            dr[dtOrdenado.Rows[j]["crp_descricao"].ToString()] = "*";
                                        }

                                        break;
                                    }

                                    dr[dtOrdenado.Rows[j]["crp_descricao"].ToString()] = "-";
                                }
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }

            DataView dv = dt.DefaultView;
            //dv.Sort = "tds_base, Disciplinas";
            dv.Sort = "tds_base, " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString();

            _grvCurriculo.DataSource = dv;
            _grvCurriculo.DataBind();

            if (_VS_cur_id <= 0)
            {
                UCComboTipoNivelEnsino1.PermiteEditar = _grvCurriculo.Rows.Count <= 0;
            }
            else
            {
                UCComboTipoNivelEnsino1.PermiteEditar = false;
            }

            _updDadosCurso.Update();
        }

        _updGridPeriodo.Update();
    }

    /// <summary>
    /// Propriedade na qual verifica se o DataSource de uma grid é vazia. Nesse caso a Grid não é criada por estar vazia.
    /// Caso seja vazia, ele cria uma nova linha vazia para esta grid, e atribui valor de Visible false para linha.
    /// Permitindo que a grid seja criada apresentando seu Header, Footer.
    /// </summary>
    /// <param name="grdView">GridView do Dia da Semana</param>
    private void _FixarGridVazia(GridView grdView)
    {
        // normalmente executada depois do carregamento da grid (grid.Bind())
        if (grdView.Rows.Count == 0 && grdView.DataSource != null)
        {
            DataTable dt = null;

            // necessário clonar a fonte (DataSource) caso contrário será indiretamente adicionado na fonte (DataSource) original
            if (grdView.DataSource is DataView)
            {
                dt = ((DataView)grdView.DataSource).ToTable().Clone();
            }

            if (dt == null)
            {
                return;
            }

            // adicionando linha vazia
            dt.Rows.Add(dt.NewRow());

            grdView.DataSource = new DataView(dt);
            grdView.DataBind();

            // escondendo a linha
            grdView.Rows[0].Visible = false;
            grdView.Rows[0].Controls.Clear();
        }

        // normalmente executada em todos os postbacks
        if (grdView.Rows.Count == 1 && grdView.DataSource == null)
        {
            bool bIsGridEmpty = true;

            // checa se na primeira linha, todas as celulas estão vazias.
            for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(grdView.Rows[0].Cells[i].Text))
                {
                    bIsGridEmpty = false;
                }
            }

            // esconde a linha
            if (bIsGridEmpty)
            {
                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }
        }

        if (_VS_cur_id <= 0)
        {
            UCComboTipoNivelEnsino1.PermiteEditar = _grvCurriculo.Rows.Count <= 1;
        }
        else
        {
            UCComboTipoNivelEnsino1.PermiteEditar = false;
        }
    }

    #endregion DISCIPLINASPERIODOS

    #region ELETIVAS ALUNOS

    /// <summary>
    /// Limpa os campos das disciplinas eletivas.
    /// </summary>
    private void LimparCamposEletivasAlunos()
    {
        lblMessageEletivasAlunos.Visible = false;
        txtSigla.Text = string.Empty;
        ddlSituacaoEletivasAlunos.SelectedValue = "-1";
        txtNomeDisciplinaEletivasAlunos.Text = string.Empty;
        txtNomeDocumentacoes.Text = string.Empty;
        txtObjetivosEletivasAlunos.Text = string.Empty;
        txtHabilidadesEletivasAlunos.Text = string.Empty;
        txtMetodologiasEletivasAlunos.Text = string.Empty;
        txtQtdeTemposAulaEletivasAlunos.Text = string.Empty;

        chkPeriodosCursoEletivasAlunos.Items.Clear();
        rptCampos.DataSource = null;
        rptCampos.DataBind();

        var x = from DataRow dr in _VS_periodos.Rows
                where dr.RowState != DataRowState.Deleted && !string.IsNullOrEmpty(dr["crp_qtdeEletivasAlunos"].ToString()) && Convert.ToByte(dr["crp_qtdeEletivasAlunos"].ToString()) > 0
                select new { crp_id = dr["crp_id"].ToString(), crp_descricao = dr["crp_descricao"].ToString() };

        for (int i = 0; i < x.Count(); i++)
        {
            ListItem li = new ListItem
            {
                Value = x.ToList()[i].crp_id,
                Text = x.ToList()[i].crp_descricao
            };

            chkPeriodosCursoEletivasAlunos.Items.Add(li);
        }
    }

    /// <summary>
    /// Altera as disciplinas eletivas no grid.
    /// </summary>
    private void AlterarEletivasAlunosGrid()
    {
        LimparCamposEletivasAlunos();

        rptCampos.DataSource = ACA_TipoMacroCampoEletivaAlunoBO.SelecionaMacroCamposNaoAssociado(VS_dis_id);
        rptCampos.DataBind();

        foreach (ACA_CurriculoDisciplina_Cadastro cad in VS_List_ACA_CurriculoDisciplina_Cadastro)
        {
            if (cad.entityDisciplina.dis_id == VS_dis_id)
            {
                txtNomeDisciplinaEletivasAlunos.Text = cad.entityDisciplina.dis_nome;
                txtNomeDocumentacoes.Text = cad.entityDisciplina.dis_nomeDocumentacao;
                txtObjetivosEletivasAlunos.Text = cad.entityDisciplina.dis_objetivos;
                txtHabilidadesEletivasAlunos.Text = cad.entityDisciplina.dis_habilidades;
                txtMetodologiasEletivasAlunos.Text = cad.entityDisciplina.dis_metodologias;
                ddlSituacaoEletivasAlunos.SelectedValue = cad.entityDisciplina.dis_situacao.ToString();
                txtQtdeTemposAulaEletivasAlunos.Text = cad.entityDisciplina.dis_cargaHorariaTeorica > 0 ? cad.entityDisciplina.dis_cargaHorariaTeorica.ToString() : string.Empty;
                txtSigla.Text = cad.entityDisciplina.dis_codigo;
                foreach (ListItem li in chkPeriodosCursoEletivasAlunos.Items)
                {
                    var x2 = from ACA_CurriculoDisciplina crd in cad.listCurriculoDisciplina
                             where crd.crp_id == Convert.ToInt32(li.Value)
                             select crd.crp_id;

                    if (x2.Count() > 0)
                    {
                        li.Selected = true;
                    }
                }

                foreach (RepeaterItem item in rptCampos.Items)
                {
                    CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                    if (ckbCampo != null && ckbCampo.Checked)
                    {
                        HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                        if (hdnId != null)
                            ckbCampo.Checked = cad.listMacroCampo.Any(p => p.tea_id == Convert.ToInt32(hdnId));
                    }
                }

                break;
            }
        }

        txtNomeDisciplinaEletivasAlunos.Focus();
        updCadastroEletivasAlunos.Update();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroEletivasAlunosAlterar", "$('.divEletivasAlunos').dialog('open');", true);
    }

    #endregion ELETIVAS ALUNOS

    #endregion Métodos
}