using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Cargo_UCCadastroCargo : MotherUserControl
{
    #region Delegates

    public delegate void onInclui(DataTable dt);

    public event onInclui _Incluir;

    public void _IncluirCargo(DataTable dt)
    {
        if (_Incluir != null)
            _Incluir(_VS_cargos);
    }

    public delegate void onSeleciona();

    public event onSeleciona _Selecionar;

    public void _SelecionarUA()
    {
        if (_Selecionar != null)
        {
            _Selecionar();
        }
    }

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// ViewState com datatable de cargos
    /// Retorno e atribui valores para o DataTable de cargos
    /// </summary>
    public DataTable _VS_cargos
    {
        get
        {
            if (ViewState["_VS_cargos"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crg_id");
                dt.Columns.Add("seqcrg_id");
                dt.Columns.Add("fun_id");
                dt.Columns.Add("seqfun_id");
                dt.Columns.Add("uad_id");
                dt.Columns.Add("situacao_id");
                dt.Columns.Add("vigenciaini");
                dt.Columns.Add("vigenciafim");
                dt.Columns.Add("coc_matricula");
                dt.Columns.Add("cof_responsavelUA");
                dt.Columns.Add("cargofuncao");
                dt.Columns.Add("uad_nome");
                dt.Columns.Add("vigencia");
                dt.Columns.Add("situacao");
                dt.Columns.Add("observacao");
                dt.Columns.Add("chr_id");
                dt.Columns.Add("coc_vinculoSede");
                dt.Columns.Add("coc_vinculoExtra");
                dt.Columns.Add("coc_dataInicioMatricula");
                dt.Columns.Add("coc_readaptado");
                dt.Columns.Add("coc_complementacaoCargaHoraria");
                dt.Columns.Add("controladoIntegracao");

                ViewState["_VS_cargos"] = dt;
            }

            return (DataTable)ViewState["_VS_cargos"];
        }

        set
        {
            ViewState["_VS_cargos"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda a tabela de cargos/disciplinas
    /// no ViewState.
    /// </summary>
    public DataTable _VS_cargosDisciplinas
    {
        get
        {
            if (ViewState["_VS_cargosDisciplinas"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crg_id");
                dt.Columns.Add("coc_id");
                dt.Columns.Add("tds_id");

                dt.PrimaryKey = new[] { dt.Columns["crg_id"], dt.Columns["coc_id"], dt.Columns["tds_id"] };

                ViewState["_VS_cargosDisciplinas"] = dt;
            }

            return (DataTable)ViewState["_VS_cargosDisciplinas"];
        }

        set
        {
            ViewState["_VS_cargosDisciplinas"] = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Cargo.
    /// </summary>
    public int ComboCargoValor
    {
        get
        {
            return UCComboCargo1.Valor;
        }

        set
        {
            UCComboCargo1.Valor = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Cargo.
    /// </summary>
    public bool ComboCargoPermiteEditar
    {
        set
        {
            UCComboCargo1.PermiteEditar = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox Matricula.
    /// </summary>
    public TextBox _txtMatricula
    {
        get
        {
            return txtMatricula;
        }

        set
        {
            txtMatricula = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox UA.
    /// </summary>
    public TextBox _txtUA
    {
        get
        {
            return txtUA;
        }

        set
        {
            txtUA = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox Vigencia Inicial.
    /// </summary>
    public TextBox _txtVigenciaIni
    {
        get
        {
            return txtVigenciaIni;
        }

        set
        {
            txtVigenciaIni = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox Data de início da matrícula.
    /// </summary>
    public string txtDataInicioMatricula
    {
        get
        {
            return _txtDataInicioMatricula.Text;
        }

        set
        {
            _txtDataInicioMatricula.Text = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox Vigencia Final.
    /// </summary>
    public TextBox _txtVigenciaFim
    {
        get
        {
            return txtVigenciaFim;
        }

        set
        {
            txtVigenciaFim = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o TextBox Observacao.
    /// </summary>
    public TextBox _txtObservacao
    {
        get
        {
            return txtObservacao;
        }

        set
        {
            txtObservacao = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o Button Incluir.
    /// </summary>
    public Button _Botao
    {
        get
        {
            return _btnIncluir;
        }

        set
        {
            _btnIncluir = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o Button Cancelar.
    /// </summary>
    public Button _BotaoCancelar
    {
        get
        {
            return _btnCancelar;
        }

        set
        {
            _btnCancelar = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o Button Pesquisar UA.
    /// </summary>
    public ImageButton _btnPesquisarUA
    {
        get
        {
            return btnPesquisarUA;
        }

        set
        {
            btnPesquisarUA = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updCadastroCargo
    {
        get
        {
            return updCadastroCargos;
        }

        set
        {
            updCadastroCargos = value;
        }
    }

    public CheckBox _ckbVinculoSede
    {
        get
        {
            return ckbVinculoSede;
        }

        set
        {
            ckbVinculoSede = value;
        }
    }

    public CheckBox _ckbVinculoExtra
    {
        get
        {
            return ckbVinculoExtra;
        }

        set
        {
            ckbVinculoExtra = value;
        }
    }

    public CheckBox _ckbColaboradorReadaptado
    {
        get
        {
            return ckbColaboradorReadaptado;
        }

        set
        {
            ckbColaboradorReadaptado = value;
        }
    }

    public CheckBox _ckbComplementacaoCargaHoraria
    {
        get
        {
            return ckbComplementacaoCargaHoraria;
        }

        set
        {
            ckbComplementacaoCargaHoraria = value;
        }
    }

    /// <summary>
    /// Armazena o id da UA.
    /// </summary>
    public Guid _VS_uad_id
    {
        get
        {
            if (ViewState["_VS_uad_id"] != null)
            {
                return new Guid(ViewState["_VS_uad_id"].ToString());
            }

            return Guid.Empty;
        }

        set
        {
            ViewState["_VS_uad_id"] = value;
        }
    }

    /// <summary>
    /// Guarda o ID do colaborador.
    /// </summary>
    public long VS_col_id
    {
        get
        {
            if (ViewState["VS_col_id"] != null)
            {
                return Convert.ToInt64(ViewState["VS_col_id"]);
            }

            return 0;
        }

        set
        {
            ViewState["VS_col_id"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de cargo.
    /// </summary>
    public bool _VS_IsNew
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNew"]);
        }

        set
        {
            if (!value)
            {
                divCadastro.Visible = true;
            }

            ViewState["_VS_IsNew"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid.
    /// </summary>
    public int _VS_id
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
    /// Armazena o sequencial para alteração no DataTable.
    /// </summary>
    public int _VS_Coc_ID
    {
        get
        {
            if (ViewState["_VS_seq"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_seq"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    /// <summary>
    /// Armazena a flag controladoIntegracao para alteração no DataTable.
    /// </summary>
    public bool VS_coc_controladoIntegracao
    {
        get
        {
            if (ViewState["VS_coc_controladoIntegracao"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_coc_controladoIntegracao"]);
            }

            return false;
        }

        set
        {
            ViewState["VS_coc_controladoIntegracao"] = value;
        }
    }

    /// <summary>
    /// Armazena a data de admissão do colaborador.
    /// </summary>
    public DateTime _VS_dataAdmissao
    {
        get
        {
            if (ViewState["_VS_dataAdmissao"] != null)
            {
                return Convert.ToDateTime(ViewState["_VS_dataAdmissao"]);
            }

            return new DateTime();
        }

        set
        {
            ViewState["_VS_dataAdmissao"] = value;
        }
    }

    /// <summary>
    /// Armazena a data de demissão do colaborador.
    /// </summary>
    public DateTime _VS_dataDemissao
    {
        get
        {
            if (ViewState["_VS_dataDemissao"] != null)
            {
                return Convert.ToDateTime(ViewState["_VS_dataDemissao"]);
            }

            return new DateTime();
        }

        set
        {
            ViewState["_VS_dataDemissao"] = value;
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo de situação.
    /// </summary>
    public int ValorSituacao
    {
        get
        {
            return Convert.ToInt32(ddlCargoSituacao.SelectedValue);
        }

        set
        {
            ddlCargoSituacao.SelectedValue = value.ToString();
        }
    }

    /// <summary>
    /// Adiciona a classe css ao botao Incluir.
    /// </summary>
    public string AdicionaClasseCss
    {
        set
        {
            if (!Convert.ToString(_btnIncluir.CssClass).Contains(value))
            {
                _btnIncluir.CssClass += " " + value;
            }
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo de carga horária.
    /// </summary>
    public int ValorCargaHoraria
    {
        get
        {
            return UCComboCargaHoraria1.Valor;
        }

        set
        {
            UCComboCargaHoraria1.Valor = value;
        }
    }

    /// <summary>
    /// Armazena a flag se for cadastro de docente = true, se não, false.
    /// </summary>
    public bool VS_IsDocente
    {
        get
        {
            if (ViewState["VS_IsDocente"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_IsDocente"]);
            }

            return false;
        }

        set
        {
            ViewState["VS_IsDocente"] = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Carrega o combo de cargo de acordo com o filtro.
    /// null - Carrega todos os cargos não excluídos logicamente
    /// true - Carrega todos os cargos não excluídos logicamente com crg_cargoDocente = true
    /// false - Carrega todos os cargos não excluídos logicamente com crg_cargoDocente = false
    /// </summary>
    /// <param name="crg_cargoDocente">Indica se é cargo docente (pode ser nulo)</param>
    public void ComboCargoCarregar(Nullable<bool> crg_cargoDocente)
    {
        if (!crg_cargoDocente.HasValue)
        {
            UCComboCargo1.CarregarCargo();
        }
        else if (crg_cargoDocente == false)
        {
            UCComboCargo1.CarregarCargoNormal();
        }
        else if (crg_cargoDocente == true)
        {
            UCComboCargo1.CarregarCargoDocente();
        }
    }

    /// <summary>
    /// Carrega o combo de cargo de acordo com o filtro.    
    /// </summary>
    /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
    public void ComboCargoCarregarVerificandoControleIntegracao(bool crg_controleIntegracao)
    {
        UCComboCargo1.CarregarCargoVerificandoControleIntegracao(crg_controleIntegracao);
    }

    /// <summary>
    /// Carrega o combo de cargo de acordo com o filtro.    
    /// </summary>
    /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
    public void ComboCargoCarregarVerificandoControleIntegracaoDocente(bool crg_controleIntegracao)
    {
        UCComboCargo1.CarregarCargoDocenteVerificandoControleIntegracao(crg_controleIntegracao);
    }

    /// <summary>
    /// Seta o foco do combo de cargo.
    /// </summary>
    public void ComboCargoSetarFoco()
    {
        UCComboCargo1.SetarFoco();
    }

    /// <summary>
    /// Carrega o combo de carga horária.
    /// </summary>
    /// <param name="cargo">Entidade do cargo</param>
    public void ComboCargaHorariaCarregar(RHU_Cargo cargo)
    {
        bool controlarCargaHorariaDocente = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_CARGA_HORARIA_DOCENTE
            , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        UCComboCargaHoraria1.Obrigatorio = cargo.crg_cargoDocente && controlarCargaHorariaDocente;
        UCComboCargaHoraria1.CarregarCargaHoraria(cargo.crg_id, cargo.crg_especialista);

        UCComboCargaHoraria1.PermiteEditar = UCComboCargo1.Valor > 0;
    }

    /// <summary>
    /// Evento IndexChanged do Combo de cargos.
    /// </summary>
    private void SelecionarCargo()
    {
        try
        {
            if (UCComboCargo1.Valor > 0)
            {
                RHU_Cargo cargo = new RHU_Cargo
                {
                    crg_id = UCComboCargo1.Valor
                };
                RHU_CargoBO.GetEntity(cargo);

                if (VS_IsDocente)
                {
                    DataTable dtTipoDisciplina = RHU_CargoDisciplinaBO.SelecionaCargoDisciplinaByCrgId(cargo.crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    if(dtTipoDisciplina.Rows.Count > 0)
                    {
                        cblDisciplinasPossiveis.DataSourceID = string.Empty;
                        cblDisciplinasPossiveis.DataSource = dtTipoDisciplina;
                    }
                    else
                    {
                        List<sTipoDisciplina> lstTipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        cblDisciplinasPossiveis.DataSource = lstTipoDisciplina;                       
                    }                    
                }
                else
                {
                    List<sTipoDisciplina> lstTipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    cblDisciplinasPossiveis.DataSource = lstTipoDisciplina;                   
                }
                cblDisciplinasPossiveis.DataBind();

                if (cargo.crg_cargoDocente)
                {
                    fsDisciplinas.Visible = !cargo.crg_especialista;
                }
                else
                {
                    fsDisciplinas.Visible = false;
                }

                ComboCargaHorariaCarregar(cargo);
            }
            else
            {
                UCComboCargaHoraria1.Valor = -1;
                UCComboCargaHoraria1.PermiteEditar = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Limpa os campos da tela.
    /// </summary>
    public void _LimparCampos()
    {
        _VS_IsNew = true;
        _VS_id = 0;
        _VS_uad_id = Guid.Empty;
        HabilitaControles(Controls, true);
        _btnIncluir.Text = "Incluir";
        _btnIncluir.Visible = true;
        _btnCancelar.Text = "Cancelar";
        _lblMessage.Visible = false;

        UCComboCargo1.Valor = -1;

        foreach (ListItem li in cblDisciplinasPossiveis.Items)
        {
            li.Selected = false;
        }

        fsDisciplinas.Visible = false;

        txtMatricula.Text = string.Empty;
        txtUA.Text = string.Empty;
        btnPesquisarUA.Enabled = true;
        txtVigenciaIni.Text = string.Empty;
        txtVigenciaFim.Text = string.Empty;
        txtObservacao.Text = string.Empty;

        UCComboCargaHoraria1.Valor = -1;
        UCComboCargaHoraria1.PermiteEditar = false;

        ddlCargoSituacao.SelectedValue = "-1";
        ckbVinculoSede.Checked = false;
        ckbVinculoExtra.Checked = false;
        ckbColaboradorReadaptado.Checked = false;
        ckbComplementacaoCargaHoraria.Checked = false;
        _txtDataInicioMatricula.Text = string.Empty;
        rfvMatricula.Visible = false;
        LabelMatricula.Text = "Matrícula";

        UCComboCargo1.PermiteEditar = true;
        divCadastro.Visible = true;
    }

    /// <summary>
    /// Carrega as disciplinas.
    /// </summary>
    /// <param name="crg_id">ID do cargo</param>
    /// <param name="coc_id">ID do COC</param>
    /// <param name="col_id"></param>
    public void CarregarDisciplinas(int crg_id, int coc_id, long col_id)
    {
        RHU_Cargo cargo = new RHU_Cargo
        {
            crg_id = crg_id
        };
        RHU_CargoBO.GetEntity(cargo);

        fsDisciplinas.Visible = cargo.crg_cargoDocente && !cargo.crg_especialista;

        if (VS_IsDocente)
        {
            DataTable dtTipoDisciplina = RHU_CargoDisciplinaBO.SelecionaCargoDisciplinaByCrgId(cargo.crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (dtTipoDisciplina.Rows.Count > 0)
            {
                cblDisciplinasPossiveis.DataSourceID = string.Empty;
                cblDisciplinasPossiveis.DataSource = dtTipoDisciplina;
            }
            else
            {
                List<sTipoDisciplina> lstTipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                cblDisciplinasPossiveis.DataSource = lstTipoDisciplina;
            }            
        }
        else
        {
            List<sTipoDisciplina> lstTipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            cblDisciplinasPossiveis.DataSource = lstTipoDisciplina;            
        }
        cblDisciplinasPossiveis.DataBind();

        if (cargo.crg_cargoDocente)
        {
            if (!cargo.crg_especialista)
            {
                foreach (ListItem li in cblDisciplinasPossiveis.Items)
                {
                    int tds_id = Convert.ToInt32(li.Value);

                    // Encontra linha pela chave primaria da tabela.
                    var x = from DataRow dr in _VS_cargosDisciplinas.Rows
                            where dr.RowState != DataRowState.Deleted &&
                                Convert.ToInt32(dr["crg_id"]) == crg_id &&
                                Convert.ToInt32(dr["coc_id"]) == coc_id &&
                                Convert.ToInt32(dr["tds_id"]) == tds_id
                            select dr;

                    li.Selected = x.Count() > 0;
                }
            }
        }

        VS_col_id = col_id;
    }

    /// <summary>
    /// Faz as validações necessárias.
    /// </summary>
    /// <returns></returns>
    private bool _Validar()
    {
        if (UCComboCargo1.Valor <= 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Cargo é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (string.IsNullOrEmpty(txtUA.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Unidade administrativa é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (string.IsNullOrEmpty(txtVigenciaIni.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (RHU_ColaboradorBO.VerificaMatriculaExistente(VS_col_id, txtMatricula.Text, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Outro docente/colaborador já possui um vínculo com esta matrícula.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (ExisteCargoComMesmasInformacoes())
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Já existe um vínculo com as mesmas informações.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            && (ckbVinculoSede.Checked && ckbComplementacaoCargaHoraria.Checked))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Não é permitido selecionar as opções 'Vínculo sede' e 'Complementação de carga horária' no mesmo vínculo.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        cvVigenciIni.Validate();

        if (!cvVigenciIni.IsValid)
        {
            return false;
        }

        DateTime dtVigenciaInicio = Convert.ToDateTime(txtVigenciaIni.Text);
        DateTime dtVigenciaFim = string.IsNullOrEmpty(txtVigenciaFim.Text) ? new DateTime() : Convert.ToDateTime(txtVigenciaFim.Text);

        if (dtVigenciaFim != new DateTime())
        {
            cvVigenciaFim.Validate();

            if (!cvVigenciaFim.IsValid)
            {
                return false;
            }

            if (dtVigenciaInicio > dtVigenciaFim)
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser maior que a vigência final.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        if (_VS_dataAdmissao != new DateTime())
        {
            if (dtVigenciaInicio < _VS_dataAdmissao)
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser menor que a data de admissão.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (dtVigenciaFim != new DateTime())
            {
                if (dtVigenciaFim < _VS_dataAdmissao)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Vigência final não pode ser menor que a data de admissão.", UtilBO.TipoMensagem.Alerta);
                    return false;
                }
            }
        }

        if (_VS_dataDemissao != new DateTime())
        {
            if (dtVigenciaInicio > _VS_dataDemissao)
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser maior que a data de demissão.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (!string.IsNullOrEmpty(txtVigenciaFim.Text))
            {
                if (Convert.ToDateTime(txtVigenciaFim.Text) > _VS_dataDemissao)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Vigência final não pode ser maior que a data de demissão.", UtilBO.TipoMensagem.Alerta);
                    return false;
                }
            }
        }

        if (ddlCargoSituacao.SelectedValue == "-1")
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Situação é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessage.Visible = false;
        return true;
    }

    /// <summary>
    /// Verifica se existe cargos cadastrados com as mesmas informações
    /// </summary>
    /// <returns></returns>
    private bool ExisteCargoComMesmasInformacoes()
    {
        for (int i = 0; i < _VS_cargos.Rows.Count; i++)
        {
            if (_VS_cargos.Rows[i].RowState != DataRowState.Deleted)
            {
                bool cargoDiferente = _VS_cargos.Rows[i]["seqcrg_id"].ToString() != Convert.ToString(_VS_Coc_ID);

                if (((!_VS_IsNew && cargoDiferente) || (_VS_IsNew)) &&
                    _VS_cargos.Rows[i]["crg_id"].ToString() == Convert.ToString(UCComboCargo1.Valor) &&
                    _VS_cargos.Rows[i]["coc_matricula"].ToString() == Convert.ToString(txtMatricula.Text) &&
                    _VS_cargos.Rows[i]["uad_id"].ToString() == Convert.ToString(_VS_uad_id) &&
                    Convert.ToByte(_VS_cargos.Rows[i]["situacao_id"]) != (byte)RHU_ColaboradorCargoSituacao.Desativado &&
                    Convert.ToBoolean(_VS_cargos.Rows[i]["coc_complementacaoCargaHoraria"]) == Convert.ToBoolean(ckbComplementacaoCargaHoraria.Checked))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Inclui novo cargo.
    /// </summary>
    private void _IncluirCargo()
    {
        DataRow dr = _VS_cargos.NewRow();

        if (_VS_Coc_ID == -1)
        {
            _VS_Coc_ID = 1;
        }
        else
        {
            _VS_Coc_ID = _VS_Coc_ID + 1;
        }

        int crg_id = UCComboCargo1.Valor;

        dr["seqcrg_id"] = _VS_Coc_ID;
        dr["crg_id"] = crg_id;
        dr["uad_id"] = _VS_uad_id;
        dr["vigenciaini"] = txtVigenciaIni.Text;
        dr["vigenciafim"] = txtVigenciaFim.Text;
        dr["coc_matricula"] = txtMatricula.Text;
        dr["cargofuncao"] = UCComboCargo1.Texto;
        dr["uad_nome"] = txtUA.Text;
        dr["coc_dataInicioMatricula"] = _txtDataInicioMatricula.Text;
        dr["coc_readaptado"] = ckbColaboradorReadaptado.Checked;
        dr["controladoIntegracao"] = false;

        if (string.IsNullOrEmpty(txtVigenciaFim.Text.Trim()))
        {
            dr["vigencia"] = txtVigenciaIni.Text + " - *";
        }
        else
        {
            if (txtVigenciaIni.Text == txtVigenciaFim.Text)
            {
                dr["vigencia"] = txtVigenciaIni.Text;
            }
            else
            {
                dr["vigencia"] = txtVigenciaIni.Text + " - " + txtVigenciaFim.Text;
            }

            if (Convert.ToDateTime(txtVigenciaFim.Text) < DateTime.Now.Date)
            {
                ddlCargoSituacao.SelectedValue = ((byte)RHU_ColaboradorCargoSituacao.Desativado).ToString();
            }
        }

        dr["situacao_id"] = ddlCargoSituacao.SelectedValue;
        dr["situacao"] = ddlCargoSituacao.SelectedItem;
        dr["observacao"] = txtObservacao.Text;
        dr["chr_id"] = UCComboCargaHoraria1.Valor;
        dr["coc_vinculoSede"] = ckbVinculoSede.Checked;
        dr["coc_vinculoExtra"] = ckbVinculoExtra.Checked;
        dr["coc_complementacaoCargaHoraria"] = ckbComplementacaoCargaHoraria.Checked;

        _VS_cargos.Rows.Add(dr);

        SalvarDisciplinas(UCComboCargo1.Valor, _VS_Coc_ID);

        _IncluirCargo(_VS_cargos);
    }

    /// <summary>
    /// Altera o cargo.
    /// </summary>
    /// <param name="crg_id">ID do cargo</param>
    /// <param name="seqcrg_id">ID sequencial do cargo</param>
    private void _AlterarCargo(int crg_id, int seqcrg_id)
    {
        for (int i = 0; i < _VS_cargos.Rows.Count; i++)
        {
            if (_VS_cargos.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_cargos.Rows[i]["seqcrg_id"].ToString() == Convert.ToString(seqcrg_id) &&
                    _VS_cargos.Rows[i]["crg_id"].ToString() == Convert.ToString(crg_id))
                {
                    _VS_cargos.Rows[i]["uad_id"] = _VS_uad_id;
                    _VS_cargos.Rows[i]["vigenciaini"] = txtVigenciaIni.Text;
                    _VS_cargos.Rows[i]["vigenciafim"] = txtVigenciaFim.Text;
                    _VS_cargos.Rows[i]["cargofuncao"] = UCComboCargo1.Texto;
                    _VS_cargos.Rows[i]["coc_matricula"] = txtMatricula.Text;
                    _VS_cargos.Rows[i]["uad_nome"] = txtUA.Text;

                    if (string.IsNullOrEmpty(txtVigenciaFim.Text.Trim()))
                    {
                        _VS_cargos.Rows[i]["vigencia"] = txtVigenciaIni.Text + " - *";
                    }
                    else
                    {
                        if (txtVigenciaIni.Text == txtVigenciaFim.Text)
                        {
                            _VS_cargos.Rows[i]["vigencia"] = txtVigenciaIni.Text;
                        }
                        else
                        {
                            _VS_cargos.Rows[i]["vigencia"] = txtVigenciaIni.Text + " - " + txtVigenciaFim.Text;
                        }

                        if (Convert.ToDateTime(txtVigenciaFim.Text) < DateTime.Now.Date)
                        {
                            ddlCargoSituacao.SelectedValue = ((byte)RHU_ColaboradorCargoSituacao.Desativado).ToString();
                        }
                    }

                    _VS_cargos.Rows[i]["situacao_id"] = ddlCargoSituacao.SelectedValue;
                    _VS_cargos.Rows[i]["situacao"] = ddlCargoSituacao.SelectedItem;
                    _VS_cargos.Rows[i]["observacao"] = txtObservacao.Text;
                    _VS_cargos.Rows[i]["chr_id"] = UCComboCargaHoraria1.Valor;
                    _VS_cargos.Rows[i]["coc_vinculoSede"] = ckbVinculoSede.Checked;
                    _VS_cargos.Rows[i]["coc_vinculoExtra"] = ckbVinculoExtra.Checked;
                    _VS_cargos.Rows[i]["coc_readaptado"] = ckbColaboradorReadaptado.Checked;
                    _VS_cargos.Rows[i]["coc_complementacaoCargaHoraria"] = ckbComplementacaoCargaHoraria.Checked;
                    _VS_cargos.Rows[i]["coc_dataInicioMatricula"] = _txtDataInicioMatricula.Text;

                    SalvarDisciplinas(crg_id, seqcrg_id);

                    _Incluir(_VS_cargos);
                }
            }
        }
    }

    /// <summary>
    /// Salva as disciplinas.
    /// </summary>
    /// <param name="crg_id">ID do cargo</param>
    /// <param name="coc_id">ID do COC</param>
    private void SalvarDisciplinas(int crg_id, int coc_id)
    {
        if (fsDisciplinas.Visible)
        {
            foreach (ListItem li in cblDisciplinasPossiveis.Items)
            {
                DataRow linha = null;

                foreach (DataRow dr in _VS_cargosDisciplinas.Rows)
                {
                    if ((dr.RowState != DataRowState.Deleted) &&
                        (Convert.ToInt32(dr["crg_id"]) == crg_id) &&
                        (Convert.ToInt32(dr["coc_id"]) == coc_id) &&
                        (dr["tds_id"].ToString() == li.Value))
                    {
                        linha = dr;
                        break;
                    }
                }

                if (li.Selected)
                {
                    if (linha == null)
                    {
                        DataRow dr = _VS_cargosDisciplinas.NewRow();

                        dr["crg_id"] = crg_id;
                        dr["coc_id"] = coc_id;
                        dr["tds_id"] = li.Value;

                        _VS_cargosDisciplinas.Rows.Add(dr);
                    }
                }
                else if (linha != null)
                {
                    linha.Delete();
                }
            }
        }
    }

    /// <summary>
    /// Carrega na tabela do ViewState os relacionamentos Cargos/Disciplinas cadastrados
    /// para o colaborador.
    /// </summary>
    /// <param name="col_id"></param>
    public void CarregarCargosDisciplinas(long col_id)
    {
        DataTable dt = RHU_ColaboradorCargoDisciplinaBO.GetSelectBy_Colaborador(col_id);

        _VS_cargosDisciplinas = dt.Rows.Count > 0 ? dt : null;
    }

    /// <summary>
    /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
    /// passado.
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="enabled"></param>
    protected void HabilitaControles(ControlCollection controls, bool enabled)
    {
        foreach (Control c in controls)
        {
            if (c.Controls.Count > 0)
                HabilitaControles(c.Controls, enabled);

            WebControl wb = c as WebControl;

            if (wb != null)
                wb.Enabled = enabled;
        }

        txtUA.Enabled = false;
    }

    /// <summary>
    /// Configura a tela conforme parâmetros acadêmicos referentes ao controle de integração.
    /// </summary>
    public void ConfiguraCadastro()
    {

        if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(
            eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) || ddlCargoSituacao.SelectedValue.Equals("6"))
        {
            HabilitaControles(Controls, false);
            UCComboCargaHoraria1.PermiteEditar = false;
            UCComboCargo1.PermiteEditar = false;
        }
        else if (!VS_coc_controladoIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(
            eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL_MATRICULA_OBRIGATORIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            ckbVinculoExtra.Enabled = false;

            rfvMatricula.Visible = true;
            LabelMatricula.Text = "Matrícula *";

            if (_VS_IsNew)
                ddlCargoSituacao.SelectedValue = "1";

            ddlCargoSituacao.Enabled = false;
        }
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
        }

        UCComboCargo1.IndexChanged += SelecionarCargo;

        if (!IsPostBack)
        {
            LabelMatricula.Text = "Matrícula";
            rfvMatricula.Visible = false;
            cvVigenciIni.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência inicial");
            cvVigenciaFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência final");

            UCComboCargo1.Obrigatorio = true;
            UCComboCargo1.ValidationGroup = "Cargo";

            UCComboCargaHoraria1.MostrarMessageSelecione = true;
        }
        lblLegend.Text = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString();
    }

    protected void _btnIncluir_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && _Validar())
        {
            if (_VS_IsNew)
            {
                _IncluirCargo();
            }
            else
            {
                _AlterarCargo(_VS_id, _VS_Coc_ID);
            }

            divCadastro.Visible = false;
        }
        else
        {
            _lblMessage.Visible = true;
            updCadastroCargos.Update();
        }
    }

    protected void btnPesquisarUA_Click(object sender, ImageClickEventArgs e)
    {
        _SelecionarUA();
    }

    #endregion Eventos
}