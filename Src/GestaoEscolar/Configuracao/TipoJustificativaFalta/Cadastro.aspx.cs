using System;
using System.Data;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Configuracao_TipoJustificativaFalta_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Armazena o ID do tipo de justificativa de falta
    /// </summary>
    private int VS_tjf_id
    {
        get
        {
            if (ViewState["VS_tjf_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tjf_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["VS_tjf_id"] = value;
        }
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ckbAbonaFaltas.Visible = false;
        if (!IsPostBack)
        {
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                Carregar(PreviousPage.EditItem);
                ckbAbonaFaltas.Enabled = false;
                ckbAbonaFaltas.Visible = false;
                chkStuacao.Visible = true;
            }
            else
            {
                chkStuacao.Visible = false;
            }

            Page.Form.DefaultFocus = txtJustificativaFalta.ClientID;
            Page.Form.DefaultButton = bntSalvar.UniqueID;
            bntSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
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
    private void Carregar(int tjf_id)
    {
        try
        {
            ACA_TipoJustificativaFalta entity = new ACA_TipoJustificativaFalta { tjf_id = tjf_id };
            ACA_TipoJustificativaFaltaBO.GetEntity(entity);

            VS_tjf_id = entity.tjf_id;
            txtJustificativaFalta.Text = entity.tjf_nome;
            txtCodigo.Text = entity.tjf_codigo;
            ckbAbonaFaltas.Checked = entity.tjf_abonaFalta;

            if ((ACA_TipoJustificativaFaltaBO.ACA_TipoJustificativaFaltaSituacao)entity.tjf_situacao == ACA_TipoJustificativaFaltaBO.ACA_TipoJustificativaFaltaSituacao.Inativo)
                chkStuacao.Checked = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de justificativa de falta.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere e altera os dados da justificativa de falta
    /// </summary>
    public void Salvar()
    {
        try
        {
            ACA_TipoJustificativaFalta entityTipoJustificativaFalta = new ACA_TipoJustificativaFalta
            {
                tjf_id = VS_tjf_id
                ,
                tjf_nome = txtJustificativaFalta.Text
                ,
                tjf_codigo = txtCodigo.Text
                ,
                tjf_situacao = chkStuacao.Checked ? (byte)ACA_TipoJustificativaFaltaBO.ACA_TipoJustificativaFaltaSituacao.Inativo : (byte)ACA_TipoJustificativaFaltaBO.ACA_TipoJustificativaFaltaSituacao.Ativo
                ,
                tjf_abonaFalta = ckbAbonaFaltas.Checked 
                ,
                IsNew = (VS_tjf_id > 0) ? false : true
            };

            if (ACA_TipoJustificativaFaltaBO.Save(entityTipoJustificativaFalta))
            {
                if (VS_tjf_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tjf_id: " + entityTipoJustificativaFalta.tjf_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de justificativa de falta incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tjf_id: " + entityTipoJustificativaFalta.tjf_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de justificativa de falta alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de justificativa de falta.", UtilBO.TipoMensagem.Erro);
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
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de justificativa de falta.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion
}



