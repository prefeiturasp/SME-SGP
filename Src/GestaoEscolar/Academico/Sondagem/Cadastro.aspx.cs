using System;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;

namespace GestaoEscolar.Academico.Sondagem
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
        /// </summary>
        private bool ParametroPermanecerTela
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de snd_id
        /// no caso de atualização de um registro ja existente.
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

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados da sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        private void _LoadFromEntity(int snd_id)
        {
            try
            {
                ACA_Sondagem snd = new ACA_Sondagem { snd_id = snd_id };
                ACA_SondagemBO.GetEntity(snd);

                VS_snd_id = snd.snd_id;

                txtTitulo.Text = snd.snd_titulo;
                txtTitulo.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                txtDescricao.Text = snd.snd_descricao;
                txtDescricao.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ckbBloqueado.Checked = !snd.snd_situacao.Equals(1);
                ckbBloqueado.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                ckbBloqueado.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.ErroCarregarSondagem").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere ou altera a sondagem
        /// </summary>
        public void Salvar()
        {
            try
            {
                ACA_Sondagem snd = new ACA_Sondagem
                {
                    snd_id = VS_snd_id
                    ,
                    snd_titulo = txtTitulo.Text
                    ,
                    snd_descricao = txtDescricao.Text
                    ,
                    snd_situacao = (ckbBloqueado.Checked ? Convert.ToByte(2) : Convert.ToByte(1))
                    ,
                    snd_dataCriacao = DateTime.Now
                    ,
                    snd_dataAlteracao = DateTime.Now
                    ,
                    IsNew = (VS_snd_id > 0) ? false : true
                };

                if (ACA_SondagemBO.Salvar(snd))
                {
                    string message = "";
                    if (VS_snd_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "snd_id: " + snd.snd_id);
                        message = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.SondagemIncluidaSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "snd_id: " + snd.snd_id);
                        message = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.SondagemAlteradaSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }

                    if (ParametroPermanecerTela)
                    {
                        lblMessage.Text = message;
                        VS_snd_id = snd.snd_id;
                        _LoadFromEntity(VS_snd_id);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = message;
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.ErroSalvarSondagem").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (MSTech.Validation.Exceptions.ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.ErroSalvarSondagem").ToString(), UtilBO.TipoMensagem.Erro);
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
                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Academico", "Sondagem.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Academico", "Sondagem.Cadastro.btnVoltar.Text").ToString();

                        _LoadFromEntity(PreviousPage.EditItem);
                    }
                    else
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ?
                                           GetGlobalResourceObject("Academico", "Sondagem.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Academico", "Sondagem.Cadastro.btnVoltar.Text").ToString();
                        ckbBloqueado.Visible = false;
                    }

                    Page.Form.DefaultFocus = txtTitulo.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Cadastro.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        #endregion
    }
}