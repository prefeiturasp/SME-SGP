using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;


namespace GestaoEscolar.Academico.MatrizHabilidades
{
    public partial class Cadastro : MotherPageLogado
    {
       
        #region Propriedades

        /// <summary>
        /// ID da matriz.
        /// </summary>
        private int VS_mat_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mat_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_mat_id"] = value;
            }
        }
     
        #endregion

        #region Métodos

        /// <summary>
        /// Salva os dados da linha do objetivo.
        /// </summary>
        /// <param name="item">Item que contém os dados do objetivo.</param>
        private void Salvar()
        {
            try
            {
                ORC_MatrizHabilidades entMatriz = new ORC_MatrizHabilidades();
                entMatriz.mat_id = VS_mat_id;
                ORC_MatrizHabilidadesBO.GetEntity(entMatriz);

                entMatriz.ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                entMatriz.mat_nome = txtNome.Text;
                entMatriz.mat_padrao = ckbPadrao.Checked;
                entMatriz.mat_situacao = 1;


                if (ORC_MatrizHabilidadesBO.Salvar(entMatriz))
                {
                    if (VS_mat_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "VS_mat_id: " + entMatriz.mat_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.SucessoInclusao").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "VS_mat_id: " + entMatriz.mat_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.SucessoAlteracao").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect("~/Academico/MatrizHabilidades/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);                    
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            pnlCadastro.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

            btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }

        /// <summary>
        /// Carrega as informações da matriz que esta sendo alterada
        /// </summary>
        private void CarregaDados()
        {
            ORC_MatrizHabilidades entity = new ORC_MatrizHabilidades();
            entity.mat_id = VS_mat_id;
            ORC_MatrizHabilidadesBO.GetEntity(entity);

            txtNome.Text = entity.mat_nome;
            ckbPadrao.Checked = entity.mat_padrao;
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            }


            try
            {
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    VerificaPermissaoUsuario();

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_mat_id = PreviousPage.Edit_Mat_id;
                        CarregaDados();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Cadastro.lblMensagem.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/MatrizHabilidades/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}
