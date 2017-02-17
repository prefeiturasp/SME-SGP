using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoAtividadeAvaliativa : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();

    public event SelectedIndexChanged IndexChanged;

    #endregion DELEGATES

    #region PROPRIEDADES

    /// <summary>
    /// Seta a propriedade IsValid no validator do combo.
    /// </summary>
    public bool Validator_IsValid
    {
        set { cpvCombo.IsValid = value; }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public int Valor
    {
        get
        {
            if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                return -1;

            return Convert.ToInt32(ddlCombo.SelectedValue);
        }
        set
        {
            ddlCombo.SelectedValue = value.ToString();
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlCombo.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblTitulo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTitulo);
            }

            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            ddlCombo.Enabled = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Outro tipo de atividade..." do dropdownlist.
    /// Por padrão é false e a mensagem "Outro tipo de atividade..." não é exibida.
    /// </summary>
    public bool MostrarMessageOutros;

    /// <summary>
    /// Evento Click javascript do combo.
    /// </summary>
    public string Combo_OnChange
    {
        set
        {
            ddlCombo.Attributes["onchange"] = value;
        }
    }

    /// <summary>
    /// Propriedade ClientID do combo.
    /// </summary>
    public string Combo_ClientID
    {
        get
        {
            return ddlCombo.ClientID;
        }
    }

    /// <summary>
    /// Retorna o ClientID do validator ligado ao combo.
    /// </summary>
    public string Validator_ClientID
    {
        get { return cpvCombo.ClientID; }
    }

    #endregion PROPRIEDADES

    #region METODOS

    /// <summary>
    /// Seta o foco no combo
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Mostra os tipos de atividades não excluídos logicamente no dropdownlist
    /// </summary>
    public void CarregarTipoAtividadeAvaliativa(bool apenasAtivos, long tud_id = -1)
    {
        ddlCombo.Items.Clear();
        if (tud_id <= 0)
        {
            ddlCombo.DataSource = CLS_TipoAtividadeAvaliativaBO.SelecionaTipoAtividadeAvaliativa(apenasAtivos);
        }
        else
        {
            List<CLS_TipoAtividadeAvaliativa> list = CLS_TipoAtividadeAvaliativaBO.SelecionaTiposAtividadesAvaliativasAtivosBy_TurmaDisciplina(tud_id, ApplicationWEB.AppMinutosCacheLongo);
            ddlCombo.DataSource = list.Where(p => p.qat_id != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.LicaoDeCasa
                            && p.qat_id != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDaAtividadeDiversificada
                            && p.qat_id != (int)CLS_QualificadorAtividadeBO.EnumTipoQualificadorAtividade.RecuperacaoDoInstrumentoDeAvaliacao);      
        }
        ddlCombo.Items.Insert(0, new ListItem(string.Format("-- Selecione um tipo de {0} --", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), "-1", true));
        if (MostrarMessageOutros)
            ddlCombo.Items.Insert(ddlCombo.Items.Count, new ListItem(string.Format("Outro tipo de {0}...", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), "0", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os tipos de atividades não excluídos logicamente no dropdownlist
    /// </summary>
    /// <param name="apenasAtivos">Apenas os ativos ou não</param>
    /// <param name="tav_id">Id de um tipo para ser retornado</param>
    public void CarregaTipoAtividadeAvaliativaAtivosMaisInativo(bool apenasAtivos, int tav_id, long tud_id = -1)
    {
        List<CLS_TipoAtividadeAvaliativa> list;
        if (tud_id <= 0)
        {
            list = CLS_TipoAtividadeAvaliativaBO.SelecionaTipoAtividadeAvaliativa(apenasAtivos, ApplicationWEB.AppMinutosCacheLongo);
        }
        else
        {
            list = CLS_TipoAtividadeAvaliativaBO.SelecionaTiposAtividadesAvaliativasAtivosBy_TurmaDisciplina(tud_id, ApplicationWEB.AppMinutosCacheLongo);
        }

        //Ve se o id não esta na lista, se nao estiver, adiciona ele
        if (!list.Any(p => p.tav_id == tav_id))
        {
            CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa { tav_id = tav_id };
            CLS_TipoAtividadeAvaliativaBO.GetEntity(entity);

            if (entity.tav_id > 0 && entity.tav_situacao != (byte)CLS_TipoAtividadeAvaliativaSituacao.Excluido)
            {
                list.Add(new CLS_TipoAtividadeAvaliativa
                {
                    tav_id = entity.tav_id,
                    tav_nome = entity.tav_nome
                });
            }
        }

        ddlCombo.Items.Clear();
        ddlCombo.DataSource = list;
        ddlCombo.Items.Insert(0, new ListItem(string.Format("-- Selecione um tipo de {0} --", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), "-1", true));
        if (MostrarMessageOutros)
            ddlCombo.Items.Insert(ddlCombo.Items.Count, new ListItem(string.Format("Outro tipo de {0}...", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), "0", true));
        ddlCombo.DataBind();
    }

    #endregion METODOS

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        string nomeAtividade = GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();

        if (!IsPostBack)
        {
            //Altera o Label para o nome padrão de atividade no sistema
            lblTitulo.Text = string.Format("Tipo de {0}", nomeAtividade);
            if (cpvCombo.Visible)
                AdicionaAsteriscoObrigatorio(lblTitulo);
            else
                RemoveAsteriscoObrigatorio(lblTitulo);
        }

        //Altera a mensagem de validação para o nome padrão de atividade no sistema
        cpvCombo.ErrorMessage = string.Format("Tipo de {0} é obrigatório.", nomeAtividade);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion EVENTOS
}