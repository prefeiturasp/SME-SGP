using System;
using System.Data;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.UserControlLibrary.Combos;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Cidade_UCCadastroCidade : MSTech.CoreSSO.UserControlLibrary.Buscas.Abstract_UCBusca
{
    #region Delegate

    public delegate void SelecionaCidade(Guid cid_id, string cid_nome);

    /// <summary>
    /// Evento que seleciona a cidade que acabou de ser cadastrada na tela.
    /// </summary>
    public SelecionaCidade _SelecionaCidade;

    #endregion

    #region Métodos

    private void CarregaComboEstado()
    {
        UCComboUnidadeFederativa1.Inicialize(true
            , "Estado *"
            , String.Format("Estado é obrigatório.")
            , "*"
            , String.Empty
            , true
            , new UCComboSelectItemMessage("-- Selecione um estado --", "-1")
            , true
            , new UCComboItemNotFoundMessage("-- Selecione um estado --", "-1"));
        UCComboUnidadeFederativa1._Combo.Enabled = true;
        UCComboUnidadeFederativa1._EnableValidator = true;
        UCComboUnidadeFederativa1._ValidationGroup = "vlgPais";
        UCComboUnidadeFederativa1._Combo.Focus();
    }

    private void UCComboPais1__IndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (UCComboPais1._Combo.SelectedValue == SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL))
            {
                CarregaComboEstado();
            }
            else
            {
                UCComboUnidadeFederativa1.Inicialize(true
                                                     , "Estado"
                                                     , String.Format("Estado é obrigatório.")
                                                     , "*"
                                                     , String.Empty
                                                     , true
                                                     , new UCComboSelectItemMessage("-- Selecione um estado --", "-1")
                                                     , true
                                                     , new UCComboItemNotFoundMessage("-- Selecione um estado --", "-1"));
                UCComboUnidadeFederativa1._Combo.Enabled = false;
                UCComboUnidadeFederativa1._EnableValidator = false;
                UCComboUnidadeFederativa1._Combo.SelectedValue = Guid.Empty.ToString();
                _txtDDD.Text = "";
                _txtCidade.Text = "";
                _txtCidade.Focus();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Limpa campos da tela.
    /// </summary>
    public void _Limpar()
    {
        UCComboUnidadeFederativa1._Combo.SelectedValue = Guid.Empty.ToString();
        UCComboPais1._Combo.SelectedValue = Guid.Empty.ToString();
        UCComboUnidadeFederativa1._Combo.Enabled = false;
        _txtCidade.Text = string.Empty;
        _txtDDD.Text = string.Empty;
    }

    /// <summary>
    /// Carrega combos da tela.
    /// </summary>
    public void CarregarCombos()
    {
        try
        {
            UCComboPais1.CancelaSelect = false;
            UCComboPais1.Inicialize(true
                        , "País *"
                        , String.Format("País é obrigatório.")
                        , "*"
                        , String.Empty
                        , true
                        , new UCComboSelectItemMessage("-- Selecione um país --", "-1")
                        , true
                        , new UCComboItemNotFoundMessage("-- Selecione um país --", "-1"));
            UCComboPais1._EnableValidator = true;
            UCComboPais1._ValidationGroup = "vlgPais";
            UCComboPais1._Load(0);
            UCComboPais1._Combo.SelectedValue = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL);
           
            

            UCComboUnidadeFederativa1.CancelarBinding = false;
            UCComboUnidadeFederativa1.Inicialize(true
                                                 , "Estado"
                                                 , String.Format("Estado é obrigatório.")
                                                 , "*"
                                                 , String.Empty
                                                 , true
                                                 , new UCComboSelectItemMessage("-- Selecione um estado --", "-1")
                                                 , true
                                                 , new UCComboItemNotFoundMessage("-- Selecione um estado --", "-1"));
            UCComboUnidadeFederativa1._EnableValidator = false;
            UCComboUnidadeFederativa1._ValidationGroup = "vlgPais";
            UCComboUnidadeFederativa1._Load(Guid.Empty, 0);
            UCComboUnidadeFederativa1._Combo.Enabled = false;

            CarregaComboEstado();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Valida dados na tela.
    /// </summary>
    /// <returns>Se dados estão válidos</returns>
    private bool _Validar()
    {
        if ((UCComboPais1._Combo.SelectedValue == Guid.Empty.ToString()) || (UCComboPais1._Combo.SelectedValue == null))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("País é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }
        if (String.IsNullOrEmpty(_txtCidade.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Cidade é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }
        if (SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL) == UCComboPais1._Combo.SelectedValue)
        {
            if ((UCComboUnidadeFederativa1._Combo.SelectedValue == Guid.Empty.ToString()) || (UCComboUnidadeFederativa1._Combo.SelectedValue == null))
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Estado é obrigatório para este país.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }
        _lblMessage.Visible = false;
        return true;
    }

    /// <summary>
    /// Salva os dados da cidade.
    /// </summary>
    /// <returns>Se salvou com sucesso.</returns>
    private bool _Salvar()
    {
        try
        {
            END_Cidade entityCidade = new END_Cidade
                                          {
                                              pai_id = new Guid(UCComboPais1._Combo.SelectedValue)
                ,
                                              unf_id = (new Guid(UCComboUnidadeFederativa1._Combo.SelectedValue))
                ,
                                              cid_nome = _txtCidade.Text
                ,
                                              cid_ddd = string.IsNullOrEmpty(_txtDDD.Text) ? string.Empty : _txtDDD.Text
                ,
                                              cid_situacao = Convert.ToByte(1)
                                          };

            if (END_CidadeBO.Save(entityCidade, Guid.Empty, Guid.Empty, null))
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "cid_id: " + entityCidade.cid_id);
                _lblMessage.Text = UtilBO.GetErroMessage("Cidade incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);

                if (_SelecionaCidade != null)
                    _SelecionaCidade(entityCidade.cid_id, entityCidade.cid_nome);

                return true;
            }

            return false;
        }
        catch (ValidationException ex)
        {            
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            return false;
        }
        catch (ArgumentException ex)
        {            
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            return false;
        }
        catch (DuplicateNameException ex)
        {            
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            return false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar incluir a cidade.", UtilBO.TipoMensagem.Erro);
            return false;
        }
    }

    /// <summary>
    /// Seta o foco da tela no cadastro de cidades.
    /// </summary>
    public void SetaFoco()
    {
        Page.Form.DefaultFocus = UCComboPais1._Combo.ClientID;
        Page.Form.DefaultButton = _btnSalvar.UniqueID;
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
        }

        UCComboPais1.OnSelectedIndexChange = UCComboPais1__IndexChanged;
        _btnCancelar.OnClientClick = "$('#" + ContainerName +  "').dialog('close'); return false;";
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (_Validar())
        {
            if (_Salvar())
            {
                _Limpar();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CloseDialog", "$('#" + ContainerName + "').dialog('close');", true);
            }
        }

        _lblMessage.Visible = true;
    }

    #endregion
}
