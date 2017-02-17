using System;
using System.Web;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;

public partial class Configuracao_TipoEquipamentoDeficiente_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Armazena o ID do tipo de equipamento para deficiente.
    /// </summary>
    private int VS_ted_id
    {
        get
        {
            if (ViewState["VS_ted_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_ted_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_ted_id"] = value;
        }
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                Carregar(PreviousPage.EditItem);
            else
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

            Page.Form.DefaultButton = btnSalvar.UniqueID;
            Page.Form.DefaultFocus = txtTipoEquipamentoDeficiente.ClientID;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Carrega dados do tipo de equipamento para deficiente.
    /// </summary>
    /// <param name="ted_id">Id do tipo de equipamento para deficiente</param>
    private void Carregar(int ted_id)
    {
        try
        {
            ACA_TipoEquipamentoDeficiente TipoEquipamentoDeficiente = new ACA_TipoEquipamentoDeficiente { ted_id = ted_id };
            ACA_TipoEquipamentoDeficienteBO.GetEntity(TipoEquipamentoDeficiente);
            VS_ted_id = TipoEquipamentoDeficiente.ted_id;
            txtTipoEquipamentoDeficiente.Text = TipoEquipamentoDeficiente.ted_nome;

        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            lblMessage.Text = UtilBO.GetErroMessage(
                GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Cadastro.ErroAoTentarCarregar").ToString()
                , UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere e altera o tipo de equipamento para deficientes.
    /// </summary>
    public void Salvar()
    {
        try
        {
            ACA_TipoEquipamentoDeficiente TipoEquipamentoDeficiente = new ACA_TipoEquipamentoDeficiente
            {
                ted_id = VS_ted_id
                ,
                ted_nome = txtTipoEquipamentoDeficiente.Text            
                ,
                IsNew = (VS_ted_id > 0) ? false : true
            };

            if (ACA_TipoEquipamentoDeficienteBO.Save(TipoEquipamentoDeficiente))
            {
                if (VS_ted_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "ted_id: " + TipoEquipamentoDeficiente.ted_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(
                        GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Cadastro.SucessoAoIncluir").ToString()
                        , UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "ted_id: " + TipoEquipamentoDeficiente.ted_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(
                        GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Cadastro.SucessoAoAlterar").ToString()
                        , UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect("~/Configuracao/TipoEquipamentoDeficiente/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(
                    GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Cadastro.ErroAoTentarSalvar").ToString()
                    , UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            lblMessage.Text = UtilBO.GetErroMessage(
                GetGlobalResourceObject("Configuracao", "TipoEquipamentoDeficiente.Cadastro.ErroAoTentarSalvar").ToString()
                , UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracao/TipoEquipamentoDeficiente/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    #endregion
}


