using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using System.Web.UI;

public partial class WebControls_ParametroFormacaoTurmas_UCParametroFormacaoTurmas : MotherUserControl
{
    #region Propriedades
    
    /// <summary>
    /// Id do parâmetro de formação de turmas.
    /// </summary>
    private int VS_pft_id
    {
        get
        {
            if (ViewState["VS_pft_id"] != null)
                return Convert.ToInt32(ViewState["VS_pft_id"]);
            return -1;
        }
        set
        {
            ViewState["VS_pft_id"] = value;
        }
    }

    /// <summary>
    /// Id do curso.
    /// </summary>
    private int VS_cur_id
    {
        get
        {
            if (ViewState["VS_cur_id"] != null)
                return Convert.ToInt32(ViewState["VS_cur_id"]);
            return -1;
        }
        set
        {
            ViewState["VS_cur_id"] = value;
        }
    }

    /// <summary>
    /// Id do currículo do curso.
    /// </summary>
    private int VS_crr_id
    {
        get
        {
            if (ViewState["VS_crr_id"] != null)
                return Convert.ToInt32(ViewState["VS_crr_id"]);
            return -1;
        }
        set
        {
            ViewState["VS_crr_id"] = value;
        }
    }

    /// <summary>
    /// Id do período do curso.
    /// </summary>
    private int VS_crp_id
    {
        get
        {
            if (ViewState["VS_crp_id"] != null)
                return Convert.ToInt32(ViewState["VS_crp_id"]);
            return -1;
        }
        set
        {
            ViewState["VS_crp_id"] = value;
        }
    }

    #endregion

    #region Page Life Cyle

    protected void Page_Init(object sender, EventArgs e)
    {
        this.lblCapacidadeComDeficiente.Text = "Capac. com " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
        this.lblQtdDeficiente.Text = "Qtd. máx. " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " *";
        this.rfvCapacidadeComDeficiente.ErrorMessage = "Capac. com " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " é obrigatório.";
        this.rfvQtdDeficiente.ErrorMessage = "Qtd. " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " é obrigatório.";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsParametroFormacaoTurmas.js"));
        }

        // O padrão da tela é sem controle para o tipo de controle de capacidade.
        ConfigurarCamposCapacidade(String.IsNullOrEmpty(ddlControleCapacidade.SelectedValue)
            ? ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.SemControle).ToString() : ddlControleCapacidade.SelectedValue);

        // O padrão da tela é sem alunos para os tipos de deficiências.
        ConfigurarCamposTipoDeficiencia(String.IsNullOrEmpty(ddlTiposDeficienciaAlunoIncluidos.SelectedValue)
            ? ((byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.SemAlunos).ToString() : ddlTiposDeficienciaAlunoIncluidos.SelectedValue);
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Reestringe a edição dos campos de acordo com o parâmetro permissão.
    /// </summary>
    /// <param name="permissao">Verdadeiro para permitir editar os campos</param>
    private void PermitirEditar(bool permissao)
    {
        UCComboCalendario.PermiteEditar =
        UCComboFormatoAvaliacao.PermiteEditar =
        ddlTipoDigitoCodigoTurma.Enabled =
        txtPrefixoCodigoTurma.Enabled =
        txtQtdDigitoCodigoTurma.Enabled =
        ddlControleCapacidade.Enabled =
        ddlTiposDeficienciaAlunoIncluidos.Enabled =
        txtCapacidade.Enabled =
        txtCapacidadeComDeficiente.Enabled =
        txtQtdDeficiente.Enabled =
        chkDocenteEspecialista.Enabled =
        cblTurnos.Enabled =
        cblTiposDeficiencia.Enabled = permissao;
    }

    /// <summary>
    /// Ativa ou desativa a validação dos campos do formulário.
    /// </summary>
    /// <param name="ativo">Verdadeiro para validar os campos</param>
    private void ConfigurarValidacao(bool ativo)
    {
        if (ativo)
        {
            UCComboCalendario.ValidationGroup =
            UCComboFormatoAvaliacao.ValidationGroup = "Parametro";
        }
        else
        {
            UCComboCalendario.ValidationGroup =
            UCComboFormatoAvaliacao.ValidationGroup = "";
        }
        cpvTipoDigitoCodigoTurma.Visible =
        rvQtdDigitoCodigoTurma.Visible =
        rfvQtdDigitoCodigoTurma.Visible =
        rfvCapacidade.Visible =
        rfvCapacidadeComDeficiente.Visible =
        rfvQtdDeficiente.Visible = ativo;
    }

    /// <summary>
    /// Configura visibilidade e validação dos campos referentes ao tipo de controle de capacidade com alunos deficientes.
    /// </summary>
    /// <param name="tipo">Enumerador do tipo de controle.</param>
    private void ConfigurarCamposCapacidade(string tipo)
    {
        if (!String.IsNullOrEmpty(tipo))
        {
            switch (Convert.ToByte(tipo))
            {
                case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.SemControle:
                    lblCapacidade.Style.Add("display", "none");
                    txtCapacidade.Style.Add("display", "none");
                    lblQtdDeficiente.Style.Add("display", "none");
                    txtQtdDeficiente.Style.Add("display", "none");
                    lblCapacidadeComDeficiente.Style.Add("display", "none");
                    txtCapacidadeComDeficiente.Style.Add("display", "none");
                    divCapacidades.Style.Add("display", "none");
                    rfvCapacidade.Enabled = false;
                    rfvQtdDeficiente.Enabled = false;
                    rfvCapacidadeComDeficiente.Enabled = false;
                    break;
                case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormal:
                    lblCapacidade.Style.Add("display", "");
                    txtCapacidade.Style.Add("display", "");
                    lblQtdDeficiente.Style.Add("display", "");
                    txtQtdDeficiente.Style.Add("display", "");
                    lblCapacidadeComDeficiente.Style.Add("display", "");
                    txtCapacidadeComDeficiente.Style.Add("display", "");
                    divCapacidades.Style.Add("display", "none");
                    rfvCapacidade.Enabled = true;
                    rfvQtdDeficiente.Enabled = true;
                    rfvCapacidadeComDeficiente.Enabled = true;
                    break;
                case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual:
                    lblCapacidade.Style.Add("display", "");
                    txtCapacidade.Style.Add("display", "");
                    lblQtdDeficiente.Style.Add("display", "");
                    txtQtdDeficiente.Style.Add("display", "");
                    lblCapacidadeComDeficiente.Style.Add("display", "none");
                    txtCapacidadeComDeficiente.Style.Add("display", "none");
                    divCapacidades.Style.Add("display", "");
                    rfvCapacidade.Enabled = true;
                    rfvQtdDeficiente.Enabled = true;
                    rfvCapacidadeComDeficiente.Enabled = false;
                    break;
            }
        }
    }

    /// <summary>
    /// Configura visibilidade dos campos referentes ao tipo de deficiência com aluno incluído.
    /// </summary>
    /// <param name="tipo">Enumerador do tipo de deficiência com aluno incluído.</param>
    private void ConfigurarCamposTipoDeficiencia(string tipo)
    {
        if (!String.IsNullOrEmpty(tipo))
        {
            this.fdsTipoDeficiencia.Style.Add("display", Convert.ToByte(tipo) == (byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.Escolher ? "inline-block" : "none");
        }
    }

    /// <summary>
    /// Carrega os campos com os dados do parametro de formação de turmas.
    /// </summary>
    /// <param name="cur_id">Id do curso</param>
    /// <param name="pfi_id">Id do processo de fechamento/início do ano letivo</param>
    /// <param name="parametroFormacaoTurmas">Estrutura do parâmetro de formação de turmas</param>
    /// <param name="turnos">DatTable de turnos</param>
    /// <param name="permiteEditar">Indica se os campos serão habilitados</param>
    /// <param name="exclusivoDeficientes">Indica se curso é exclusivo para deficientes</param>
    public void CarregarParametro(int cur_id, int pfi_id, ParametroFormacaoTurmas parametroFormacaoTurmas, DataTable turnos, bool permiteEditar, bool exclusivoDeficientes)
    {
        bool habilitarCampos = true;

        VS_cur_id = parametroFormacaoTurmas.entityParametroPeriodo.cur_id;
        VS_crr_id = parametroFormacaoTurmas.entityParametroPeriodo.crr_id;
        VS_crp_id = parametroFormacaoTurmas.entityParametroPeriodo.crp_id;

        CarregarCombos(pfi_id, parametroFormacaoTurmas, turnos, exclusivoDeficientes);

        ConfigurarValidacao(false);

        // Configurar se os controles serão habilitados.
        this.PermitirEditar(permiteEditar);

        // Preencher os campos apenas se houver registro gravado.
        if (parametroFormacaoTurmas.entityParametroPeriodo.pft_id > 0)
        {
            this.VS_pft_id = parametroFormacaoTurmas.entityParametroPeriodo.pft_id;
            this.UCComboCalendario.Valor = parametroFormacaoTurmas.entityParametroPeriodo.cal_id;
            this.UCComboFormatoAvaliacao.Valor = parametroFormacaoTurmas.entityParametroPeriodo.fav_id;

            this.ddlTipoDigitoCodigoTurma.SelectedValue = parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoDigitoCodigoTurma.ToString();
            //Se o dropdowlist estiver checao com "Sem controle automatico", vai carregar as informações porem vai esconder a div.
            if(Convert.ToInt32(ddlTipoDigitoCodigoTurma.SelectedValue) == Convert.ToInt32(MTR_ParametroFormacaoTurmaTipoDigito.SemControleAutomatico))
            {
                DivPrefixoCodigoTurma.Visible = false;
                DivQtdDigitoCodigoTurma.Visible = false;
            }
            this.txtPrefixoCodigoTurma.Text = parametroFormacaoTurmas.entityParametroPeriodo.pft_prefixoCodigoTurma;

            if (parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDigitoCodigoTurma == 0)
            {
                this.txtQtdDigitoCodigoTurma.Text = null;
            }
            else
            {
                this.txtQtdDigitoCodigoTurma.Text = parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDigitoCodigoTurma.ToString();
            }

            this.txtCapacidade.Text = parametroFormacaoTurmas.entityParametroPeriodo.pft_capacidade.ToString();
            this.txtCapacidadeComDeficiente.Text = parametroFormacaoTurmas.entityParametroPeriodo.pft_capacidadeComDeficiente.ToString();
            this.txtQtdDeficiente.Text = parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDeficiente.ToString();
            this.chkDocenteEspecialista.Checked = parametroFormacaoTurmas.entityParametroPeriodo.pft_docenteEspecialista;
            ddlControleCapacidade.SelectedValue = parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleCapacidade.ToString();
            ddlTiposDeficienciaAlunoIncluidos.SelectedValue = parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleDeficiente.ToString();

            // Configurar os turnos.
            if (this.cblTurnos.Items.Count > 0)
            {
                foreach (ListItem item in this.cblTurnos.Items)
                {
                    int trn_id = Convert.ToInt32(item.Value);
                    item.Selected = parametroFormacaoTurmas.listaParametroPeriodoTurno.Exists(p => p.trn_id == trn_id);
                }
            }

            // Configurar os campos de capacidade de acordo com o tipo de controle de capacidade.
            if (parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleCapacidade == (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual)
            {
                if (parametroFormacaoTurmas.listaCapacidadeDeficiente != null)
                {
                    this.hdnIdCapac.Value = "";
                    this.hdnCapacidades.Value = "";
                    foreach (MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity in parametroFormacaoTurmas.listaCapacidadeDeficiente)
                    {
                        this.hdnIdCapac.Value += entity.pfc_id.ToString() + ";";
                        this.hdnCapacidades.Value += entity.pfc_capacidadeComDeficiente.ToString() + ";";
                    }
                }
            }
            this.ConfigurarCamposCapacidade(this.ddlControleCapacidade.SelectedValue);

            // Configurar os tipos de deficiência
            if (parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleDeficiente == (byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.Escolher)
            {
                foreach (ListItem item in this.cblTiposDeficiencia.Items)
                {
                    Guid tde_id = new Guid(item.Value);
                    item.Selected = parametroFormacaoTurmas.listaTipoDeficiencia.Exists(p => p.tde_id == tde_id);
                }
            }
            this.ConfigurarCamposTipoDeficiencia(this.ddlTiposDeficienciaAlunoIncluidos.SelectedValue);

            // Configurar validação.
            this.ConfigurarValidacao(true);

            // Se houver turma cadastrada com os parametros de formação de turmas bloqueia campos.
            if (permiteEditar && TUR_TurmaBO.VerificaExisteTurmaParametroFormacao(parametroFormacaoTurmas.entityParametroPeriodo.cal_id
                                     , parametroFormacaoTurmas.entityParametroPeriodo.cur_id
                                     , parametroFormacaoTurmas.entityParametroPeriodo.crr_id
                                     , parametroFormacaoTurmas.entityParametroPeriodo.crp_id
                                     , TUR_TurmaTipo.Normal))
            {
                PermitirEditar(false);

                this.ddlTipoDigitoCodigoTurma.Enabled =
                this.txtPrefixoCodigoTurma.Enabled =
                this.txtQtdDigitoCodigoTurma.Enabled =
                this.ddlTiposDeficienciaAlunoIncluidos.Enabled =
                this.cblTiposDeficiencia.Enabled = true;

                habilitarCampos = false;
            }

        }

        CarregarFuncoesJS(habilitarCampos);

        // Carrega o nome do período na mensagem de erro.
        cpvTipoDigitoCodigoTurma.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
        rvQtdDigitoCodigoTurma.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
        rfvQtdDigitoCodigoTurma.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
        rfvCapacidade.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
        rfvCapacidadeComDeficiente.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
        rfvQtdDeficiente.ErrorMessage += " (" + parametroFormacaoTurmas.crp_descricao + ")";
    }

    /// <summary>
    /// Carrega os combos da tela de acordo com os dados.
    /// </summary>
    /// <param name="pfi_id">Id do processo fechamento/início.</param>
    /// <param name="parametroFormacaoTurmas">Estrutura de formação de turmas.</param>
    /// <param name="turnos">Tabela com os turnos.</param>
    /// <param name="exclusivoDeficientes">Indica se curso é exclusivo para deficientes.</param>
    private void CarregarCombos(int pfi_id, ParametroFormacaoTurmas parametroFormacaoTurmas, DataTable turnos, bool exclusivoDeficientes)
    {
        CarregarTurno(turnos);
        UCComboCalendario.CarregarCalendarioAnualPorCursoAnoInicio(VS_cur_id, pfi_id);
        UCComboCalendario.SelectedIndex = 0;
        UCComboFormatoAvaliacao.CarregarPorRegrasCurso
        (
            -1
            , parametroFormacaoTurmas.entityParametroPeriodo.cur_id
            , parametroFormacaoTurmas.entityParametroPeriodo.crr_id
            , parametroFormacaoTurmas.entityParametroPeriodo.crp_id
            , parametroFormacaoTurmas.entityParametroPeriodo.pft_docenteEspecialista
        );

        cblTiposDeficiencia.DataSource = PES_TipoDeficienciaBO.GetSelect();
        cblTiposDeficiencia.DataBind();

        ddlControleCapacidade.Items.Clear();
        ddlControleCapacidade.Items.Add(new ListItem("Sem controle", ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.SemControle).ToString()));
        ddlControleCapacidade.Items.Add(new ListItem("Capacidade normal e "
            + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()
                , ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormal).ToString()));
        ddlControleCapacidade.Items.Add(new ListItem("Capacidade normal e individual por "
            + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()
                , ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual).ToString()));

        ddlTiposDeficienciaAlunoIncluidos.Items.Clear();
        if (!exclusivoDeficientes)
        {
            ddlTiposDeficienciaAlunoIncluidos.Items.Add(new ListItem("Nenhum", ((byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.SemAlunos).ToString()));
        }

        ddlTiposDeficienciaAlunoIncluidos.Items.Add(new ListItem("Todos", ((byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.TodosTipos).ToString()));
        ddlTiposDeficienciaAlunoIncluidos.Items.Add(new ListItem(
             GetGlobalResourceObject("WebControls", "ParametroFormacaoTurmas.UCParametroFormacaoTurmas.ddlTiposDeficienciaAlunoIncluidos.Mensagem").ToString()
            , ((byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.Escolher).ToString()));
    }

    /// <summary>
    /// Carrega o checkBoxList de turnos.
    /// </summary>
    /// <param name="turnos">DataTable contendo os turnos</param>
    private void CarregarTurno(DataTable turnos)
    {
        if (turnos.Rows.Count > 0)
        {
            cblTurnos.DataSource = turnos;
            cblTurnos.DataBind();
            divTurnos.Visible = true;
            ltrMensagemVazio.Visible = false;
        }
        else
        {
            divTurnos.Visible = false;
            ltrMensagemVazio.Visible = true;
            ltrMensagemVazio.Text = UtilBO.GetMessage("Não há turnos definidos para o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " selecionado(a).", UtilBO.TipoMensagem.Alerta);
        }
    }

    /// <summary>
    /// Carregar as funções em javascript para os eventos dos campos de alunos deficientes.
    /// </summary>
    private void CarregarFuncoesJS(bool habilitarCampos)
    {
        ddlControleCapacidade.Attributes.Add("onchange", "javascript:HabilitaCapacidade('"
            + ddlControleCapacidade.ClientID
            + "','" + lblCapacidade.ClientID
            + "','" + txtCapacidade.ClientID
            + "','" + lblQtdDeficiente.ClientID
            + "','" + txtQtdDeficiente.ClientID
            + "','" + lblCapacidadeComDeficiente.ClientID
            + "','" + txtCapacidadeComDeficiente.ClientID
            + "','" + divCapacidades.ClientID
            + "','" + rfvCapacidade.ClientID
            + "','" + rfvQtdDeficiente.ClientID
            + "','" + rfvCapacidadeComDeficiente.ClientID
            + "','" + hdnIdCapac.ClientID
            + "','" + hdnCapacidades.ClientID
            + "','" + ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.SemControle).ToString()
            + "','" + ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormal).ToString()
            + "','" + ((byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual).ToString()
            + "','" + VS_crp_id.ToString()
            + "','" + habilitarCampos + "'); return false;");

        ddlTiposDeficienciaAlunoIncluidos.Attributes.Add("onchange", "javascript:HabilitaTipoDeficiencia('"
            + ddlTiposDeficienciaAlunoIncluidos.ClientID
            + "','" + fdsTipoDeficiencia.ClientID + "' , '"
            + ((byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.Escolher).ToString() + "'); return false;");

        txtQtdDeficiente.Attributes.Add("onblur", "javascript:ConfiguraCamposIncluidos('" + divCapacidades.ClientID + "','" + txtQtdDeficiente.ClientID + "','" + hdnIdCapac.ClientID + "','" + hdnCapacidades.ClientID + "','" + VS_crp_id.ToString() + "','" + habilitarCampos + "'); return false;");
    }

    /// <summary>
    /// Retorna os dados de parâmetro de formação de turmas que estão nos campos.
    /// </summary>
    /// <param name="parametroFormacaoTurmas">Estrutura de parâmetro de formação de turmas</param>
    public bool RetornarParametro(out ParametroFormacaoTurmas parametroFormacaoTurmas)
    {
        bool sucesso = false;

        parametroFormacaoTurmas = new ParametroFormacaoTurmas
        {
            entityParametroPeriodo = new MTR_ParametroFormacaoTurma()
            ,
            listaParametroPeriodoTurno = new List<MTR_ParametroFormacaoTurmaTurno>()
            ,
            listaCapacidadeDeficiente = new List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente>()
            ,
            listaTipoDeficiencia = new List<MTR_ParametroFormacaoTurmaTipoDeficiencia>()
        };

        if ((UCComboCalendario.Valor != -1) && (UCComboFormatoAvaliacao.Valor != -1))
        {
            parametroFormacaoTurmas.entityParametroPeriodo.pft_id = VS_pft_id;
            parametroFormacaoTurmas.entityParametroPeriodo.crp_id = VS_crp_id;
            parametroFormacaoTurmas.entityParametroPeriodo.cal_id = UCComboCalendario.Valor;
            parametroFormacaoTurmas.entityParametroPeriodo.fav_id = UCComboFormatoAvaliacao.Valor;

            parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoDigitoCodigoTurma = Convert.ToByte(ddlTipoDigitoCodigoTurma.SelectedValue);

            if (Convert.ToInt32(ddlTipoDigitoCodigoTurma.SelectedValue) == Convert.ToByte(MTR_ParametroFormacaoTurmaTipoDigito.SemControleAutomatico))
            {
                parametroFormacaoTurmas.entityParametroPeriodo.pft_prefixoCodigoTurma = null;
                parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDigitoCodigoTurma = 0;
            }
            else
            {
                parametroFormacaoTurmas.entityParametroPeriodo.pft_prefixoCodigoTurma = txtPrefixoCodigoTurma.Text;
                parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDigitoCodigoTurma = String.IsNullOrEmpty(txtQtdDigitoCodigoTurma.Text) ? 0 : Convert.ToInt32(txtQtdDigitoCodigoTurma.Text);
            }

            parametroFormacaoTurmas.entityParametroPeriodo.pft_capacidade = String.IsNullOrEmpty(txtCapacidade.Text) ? 0 : Convert.ToInt32(txtCapacidade.Text);
            parametroFormacaoTurmas.entityParametroPeriodo.pft_capacidadeComDeficiente = String.IsNullOrEmpty(txtCapacidadeComDeficiente.Text) ? 0 : Convert.ToInt32(txtCapacidadeComDeficiente.Text);
            parametroFormacaoTurmas.entityParametroPeriodo.pft_qtdDeficiente = String.IsNullOrEmpty(txtQtdDeficiente.Text) ? 0 : Convert.ToInt32(txtQtdDeficiente.Text);
            parametroFormacaoTurmas.entityParametroPeriodo.pft_docenteEspecialista = chkDocenteEspecialista.Checked;
            parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleCapacidade = Convert.ToByte(ddlControleCapacidade.SelectedValue);
            parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleDeficiente = Convert.ToByte(ddlTiposDeficienciaAlunoIncluidos.SelectedValue);
            parametroFormacaoTurmas.entityParametroPeriodo.IsNew = VS_pft_id <= 0;

            foreach (ListItem item in cblTurnos.Items)
            {
                if (item.Selected)
                {
                    MTR_ParametroFormacaoTurmaTurno entityParametroPeriodoTurno = new MTR_ParametroFormacaoTurmaTurno
                    {
                        trn_id = Convert.ToInt32(item.Value)
                    };

                    parametroFormacaoTurmas.listaParametroPeriodoTurno.Add(entityParametroPeriodoTurno);
                }
            }

            if (parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleCapacidade == (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual)
            {
                parametroFormacaoTurmas.listaCapacidadeDeficiente = RetornarCapacidadeDeficiente();
            }

            if (parametroFormacaoTurmas.entityParametroPeriodo.pft_tipoControleDeficiente == (byte)MTR_ParametroFormacaoTurmaTiposDeficienciaAlunoIncluidos.Escolher)
            {
                parametroFormacaoTurmas.listaTipoDeficiencia = RetornarTipoDeficiencia();
            }

            sucesso = true;
        }

        return sucesso;
    }

    /// <summary>
    /// Retorna uma lista de entidade MTR_ParametroFormacaoTurmaTipoDeficiencia com todos os tipos de deficiência selecionados.
    /// </summary>
    /// <returns>Lista de entidade MTR_ParametroFormacaoTurmaTipoDeficiencia.</returns>
    private List<MTR_ParametroFormacaoTurmaTipoDeficiencia> RetornarTipoDeficiencia()
    {
        return (from ListItem li in this.cblTiposDeficiencia.Items
                where li.Selected
                select new MTR_ParametroFormacaoTurmaTipoDeficiencia { tde_id = new Guid(li.Value) }).ToList();
    }

    /// <summary>
    /// Retorna uma lista de entidade MTR_ParametroFormacaoTurmaCapacidadeDeficiente com todos os valores de capacidades com alunos incluídos.
    /// </summary>
    /// <returns>Lista de entidade MTR_ParametroFormacaoTurmaCapacidadeDeficiente.</returns>
    private List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente> RetornarCapacidadeDeficiente()
    {
        List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente> capacidadeDeficiente = new List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente>();

        if (!String.IsNullOrEmpty(hdnCapacidades.Value))
        {
            string[] valoresCapacidade = hdnCapacidades.Value.Substring(0, hdnCapacidades.Value.Length - 1).Split(';');
            string[] idsCapacidade = hdnIdCapac.Value.Substring(0, hdnIdCapac.Value.Length - 1).Split(';');

            for (int indice = 0; indice < valoresCapacidade.Length; indice++)
            {
                if (!String.IsNullOrEmpty(valoresCapacidade[indice]))
                {
                    MTR_ParametroFormacaoTurmaCapacidadeDeficiente entityCapacidadeDeficiente = new MTR_ParametroFormacaoTurmaCapacidadeDeficiente
                    {
                        pfc_id = String.IsNullOrEmpty(idsCapacidade[indice].ToString()) ? 0 : Convert.ToInt32(idsCapacidade[indice])
                        ,
                        pfc_qtdDeficiente = indice + 1
                        ,
                        pfc_capacidadeComDeficiente = Convert.ToInt32(valoresCapacidade[indice])
                    };

                    capacidadeDeficiente.Add(entityCapacidadeDeficiente);
                }
            }
        }

        return capacidadeDeficiente;
    }

    #endregion

    #region Eventos

    protected void ddlTipoDigitoCodigoTurma_SelectedIndexChanged(object sender, EventArgs e)
    {
        DivPrefixoCodigoTurma.Visible = Convert.ToInt32(ddlTipoDigitoCodigoTurma.SelectedValue) != Convert.ToInt32(MTR_ParametroFormacaoTurmaTipoDigito.SemControleAutomatico);
        DivQtdDigitoCodigoTurma.Visible = DivPrefixoCodigoTurma.Visible;
    }

    protected void UCComboCalendario_IndexChanged()
    {
        ConfigurarValidacao(UCComboCalendario.Valor != -1);
    }

    protected void UCComboFormatoAvaliacao_IndexChanged()
    {
        ConfigurarValidacao(UCComboFormatoAvaliacao.Valor != -1);
    }

    protected void chkDocenteEspecialista_CheckedChanged(object sender, EventArgs e)
    {
        UCComboFormatoAvaliacao.CarregarPorRegrasCurso
        (
            -1
            , VS_cur_id
            , VS_crr_id
            , VS_crp_id
            , chkDocenteEspecialista.Checked
        );
    }

    #endregion
}
