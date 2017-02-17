using System;
using System.Data;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Configuracao_TipoJustificativaExclusaoAulas_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Armazena o ID do tipo de justificativa para exclusão de aulas
    /// </summary>
    private int VS_tje_id
    {
        get
        {
            if (ViewState["VS_tje_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tje_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_tje_id"] = value;
        }
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    Carregar(PreviousPage.EditItem);
                    chkStuacao.Visible = true;
                }
                else
                {
                    chkStuacao.Visible = false;
                }

                Page.Form.DefaultFocus = txtJustificativaExclusaoAulas.ClientID;
                Page.Form.DefaultButton = bntSalvar.UniqueID;
                bntSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);

        }
    }

    #endregion

    #region Eventos

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void bntSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// carrega dados a partir da entidade
    /// </summary>
    private void Carregar(int tje_id)
    {
        try
        {
            ACA_TipoJustificativaExclusaoAulas entity = new ACA_TipoJustificativaExclusaoAulas { tje_id = tje_id };
            ACA_TipoJustificativaExclusaoAulasBO.GetEntity(entity);

            VS_tje_id = entity.tje_id;
            txtJustificativaExclusaoAulas.Text = entity.tje_nome;
            txtCodigo.Text = entity.tje_codigo;

            if ((ACA_TipoJustificativaExclusaoAulasBO.ACA_TipoJustificativaExclusaoAulasSituacao)entity.tje_situacao == ACA_TipoJustificativaExclusaoAulasBO.ACA_TipoJustificativaExclusaoAulasSituacao.Inativo)
                chkStuacao.Checked = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.ErroCarregarDadosTipoJustificativaExclusaoAulas").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere e altera os dados da justificativa para exclusão de aulas
    /// </summary>
    public void Salvar()
    {
        try
        {
            ACA_TipoJustificativaExclusaoAulas entityTipoJustificativaExclusaoAulas = new ACA_TipoJustificativaExclusaoAulas
            {
                tje_id = VS_tje_id,
                tje_nome = txtJustificativaExclusaoAulas.Text,
                tje_codigo = txtCodigo.Text,
                tje_situacao = chkStuacao.Checked ? (byte)ACA_TipoJustificativaExclusaoAulasBO.ACA_TipoJustificativaExclusaoAulasSituacao.Inativo : (byte)ACA_TipoJustificativaExclusaoAulasBO.ACA_TipoJustificativaExclusaoAulasSituacao.Ativo,
                IsNew = (VS_tje_id > 0) ? false : true
            };

            if (ACA_TipoJustificativaExclusaoAulasBO.Save(entityTipoJustificativaExclusaoAulas))
            {
                if (VS_tje_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tje_id: " + entityTipoJustificativaExclusaoAulas.tje_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.SucessoIncluirTipoJustificativaExclusaoAulas").ToString(), UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tje_id: " + entityTipoJustificativaExclusaoAulas.tje_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.SucessoAlterarTipoJustificativaExclusaoAulas").ToString(), UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.ErroSalvarTipoJustificativaExclusaoAulas").ToString(), UtilBO.TipoMensagem.Erro);
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
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoJustificativaExclusaoAulas.Cadastro.ErroSalvarTipoJustificativaExclusaoAulas").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion
}



