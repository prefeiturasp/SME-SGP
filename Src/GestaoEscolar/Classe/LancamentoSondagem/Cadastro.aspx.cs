using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web;
using System.Web.UI;

namespace GestaoEscolar.Classe.LancamentoSondagem
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de snd_id.
        /// </summary>
        private int VS_snd_id
        {
            get
            {
                if (ViewState["VS_snd_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_snd_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_snd_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de sda_id.
        /// </summary>
        private int VS_sda_id
        {
            get
            {
                if (ViewState["VS_sda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_sda_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_sda_id"] = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack && (PreviousPage.EditItem[0] > 0 || PreviousPage.SelectedItem[0] > 0))
                    {
                        bool responder = false;

                        if (PreviousPage.EditItem[0] > 0)
                        {
                            VS_snd_id = PreviousPage.EditItem[0];
                            VS_sda_id = PreviousPage.EditItem[1];
                            responder = true;
                        }
                        else
                        {
                            VS_snd_id = PreviousPage.SelectedItem[0];
                            VS_sda_id = PreviousPage.SelectedItem[1];
                        }

                        bntSalvar.Visible = responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.btnVoltar.Text").ToString();

                        ACA_Sondagem entity = ACA_SondagemBO.GetEntity(new ACA_Sondagem { snd_id = VS_snd_id });
                        lblDadosSondagem.Text = string.Format(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblDadosSondagem.Titulo").ToString(), entity.snd_titulo);
                        if (!string.IsNullOrEmpty(entity.snd_descricao))
                        {
                            lblDadosSondagem.Text += string.Format(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblDadosSondagem.Descricao").ToString(), entity.snd_descricao);
                        }
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.SelecioneSondagem").ToString(), UtilBO.TipoMensagem.Alerta);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Classe/LancamentoSondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    Page.Form.DefaultFocus = bntSalvar.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        /// <summary>
        /// Carregar os alunos da turma
        /// </summary>
        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                   
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Classe/LancamentoSondagem/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
                //Salvar();
        }

        #endregion
    }
}