using System;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_ReuniaoResponsavel_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
    /// </summary>
    private bool usuarioPermissao
    {
        get
        {
            return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    /// <summary>
    /// Parâmetro acadêmico que indica se o cadastro de reunião de responáveis é por período do calendário.
    /// </summary>
    private bool cadastroReunioesPorPeriodo
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CADASTRAR_REUNIOES_POR_PERIODO_CALENDARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    /// <summary>
    /// ViewState que armazena o valor de crn_id da tabela ACA_CursoReunioes
    /// </summary>
    private int VS_crn_id
    {
        get
        {
            return Convert.ToInt32(ViewState["VS_crn_id"] ?? "-1");
        }

        set
        {
            ViewState["VS_crn_id"] = value;
        }
    }

    #endregion Propriedades

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                _UCComboCursoCurriculo.Obrigatorio = true;
                _UCComboCursoCurriculo.CarregarCursoCurriculo();
                _UCComboCalendario.PermiteEditar = false;
                UCComboPeriodoCalendario.Obrigatorio = UCComboPeriodoCalendario.Visible = cadastroReunioesPorPeriodo;
                UCComboPeriodoCalendario.PermiteEditar = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
        }

        _UCComboCursoCurriculo.IndexChanged += _UCComboCursoCurriculo_IndexChanged;
        _UCComboCalendario.IndexChanged += _UCComboCalendario_IndexChanged;
        UCComboPeriodoCalendario.IndexChanged += UCComboPeriodoCalendario_IndexChanged;
    }

    #endregion Eventos Page Life Cycle

    #region Eventos

    protected void _UCComboCursoCurriculo_IndexChanged()
    {
        UCComboPeriodoCalendario.Valor = new[] { -1, -1 };
        _UCComboCalendario.Valor = -1;
        UCComboPeriodoCalendario.PermiteEditar = false;
        _UCComboCalendario.PermiteEditar = false;

        if (_UCComboCursoCurriculo.Valor[0] > 0)
        {
            _UCComboCalendario.CarregarCalendarioAnualPorCurso(_UCComboCursoCurriculo.Valor[0]);
            _UCComboCalendario.PermiteEditar = true;
            _UCComboCalendario.SetarFoco();
        }

        Pesquisar();
    }

    protected void _UCComboCalendario_IndexChanged()
    {
        Pesquisar();
    }

    protected void UCComboPeriodoCalendario_IndexChanged()
    {
        Pesquisar();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (Salvar())
            Atualizar();
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Atualizar();
    }

    #endregion Eventos

    #region Métodos

    /// <summary>
    /// Reinicia os dados da tela.
    /// </summary>
    private void Atualizar()
    {
        _UCComboCursoCurriculo.CarregarCursoCurriculo();
        _UCComboCursoCurriculo.SetarFoco();
        _UCComboCalendario.Valor = -1;
        _UCComboCalendario.PermiteEditar = false;
        UCComboPeriodoCalendario.Valor = new[] { -1, -1 };
        UCComboPeriodoCalendario.PermiteEditar = false;
        _lblQtde.Visible = false;
        _txtQtde.Visible = false;
        _divBotoes.Visible = false;
    }

    /// <summary>
    /// Pesquisa se já existem reuniões cadastradas para o Curso e o Calendário escolhidos.
    /// </summary>
    private void Pesquisar()
    {
        try
        {
            if ((_UCComboCursoCurriculo.Valor[0] > 0) && (_UCComboCalendario.Valor > 0) && (UCComboPeriodoCalendario.Cap_ID > 0 || !cadastroReunioesPorPeriodo))
            {
                _lblQtde.Visible = true;
                _txtQtde.Visible = true;
                _divBotoes.Visible = true;

                // Desabilita quantidade de reuniões por período do calendário se existe outros registros ligados a ele.
                _txtQtde.Enabled = !CLS_FrequenciaReuniaoResponsaveisBO.VerificaFrequenciaPorCalendario(_UCComboCalendario.Valor, UCComboPeriodoCalendario.Cap_ID, _UCComboCursoCurriculo.Valor[0], _UCComboCursoCurriculo.Valor[1], cadastroReunioesPorPeriodo);

                ACA_CursoReunioes entityCursoReunioes = ACA_CursoReunioesBO.SelecionaPorCursoCalendarioPeriodo
                (
                    _UCComboCursoCurriculo.Valor[0]
                    ,
                    _UCComboCursoCurriculo.Valor[1]
                    ,
                    _UCComboCalendario.Valor
                    ,
                    cadastroReunioesPorPeriodo ? UCComboPeriodoCalendario.Cap_ID : -1
                );

                VS_crn_id = entityCursoReunioes.crn_id;

                _txtQtde.Text = entityCursoReunioes.crn_id <= 0 ? string.Empty : entityCursoReunioes.crn_qtde.ToString();
                _txtQtde.Enabled = _btnSalvar.Visible = usuarioPermissao;
            }
            else
            {
                _lblQtde.Visible = false;
                _txtQtde.Visible = false;
                _divBotoes.Visible = false;
                UCComboPeriodoCalendario.PermiteEditar = false;
                UCComboPeriodoCalendario.Valor = new[] { -1, -1 };

                if(_UCComboCursoCurriculo.Valor[0] > 0 && _UCComboCalendario.Valor > 0 && cadastroReunioesPorPeriodo)
                {
                    UCComboPeriodoCalendario.PermiteEditar = true;
                    UCComboPeriodoCalendario.CarregarTipoPeriodoCalendarioPorCalendario_Cap_id(_UCComboCalendario.Valor);
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Salva a quantidade de reuniões de responsáveis.
    /// </summary>
    private bool Salvar()
    {
        try
        {
            ACA_CursoReunioes entityCursoReunioes = new ACA_CursoReunioes
            {
                cur_id = _UCComboCursoCurriculo.Valor[0]
                ,
                crr_id = _UCComboCursoCurriculo.Valor[1]
                ,
                cal_id = _UCComboCalendario.Valor
                ,
                cap_id = cadastroReunioesPorPeriodo ? UCComboPeriodoCalendario.Cap_ID : -1
                ,
                crn_id = VS_crn_id
                ,
                IsNew = VS_crn_id <= 0
            };

            ACA_CursoReunioesBO.GetEntity(entityCursoReunioes);

            entityCursoReunioes.crn_qtde = String.IsNullOrEmpty(_txtQtde.Text) ? 0 : Convert.ToInt32(_txtQtde.Text);
            entityCursoReunioes.crn_situacao = 1;

            if (ACA_CursoReunioesBO.Salvar(entityCursoReunioes, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                if (entityCursoReunioes.IsNew)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "cur_id: " + entityCursoReunioes.cur_id + "; crr_id: " + entityCursoReunioes.crr_id + "; cal_id: " + entityCursoReunioes.cal_id + (cadastroReunioesPorPeriodo ? "; cap_id: " + entityCursoReunioes.cap_id : string.Empty));
                    _lblMessage.Text = UtilBO.GetMessage("Quantidade de reuniões de responsável incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    _updCadastroReuniao.Update();
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cur_id: " + entityCursoReunioes.cur_id + "; crr_id: " + entityCursoReunioes.crr_id + "; cal_id: " + entityCursoReunioes.cal_id + (cadastroReunioesPorPeriodo ? "; cap_id: " + entityCursoReunioes.cap_id : string.Empty));
                    _lblMessage.Text = UtilBO.GetMessage("Quantidade de reuniões de responsável alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    _updCadastroReuniao.Update();
                }

                return true;
            }

            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar reuniões de responsável.", UtilBO.TipoMensagem.Erro);
            return false;
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            return false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar reuniões de responsável.", UtilBO.TipoMensagem.Erro);
            return false;
        }
    }

    #endregion Métodos
}