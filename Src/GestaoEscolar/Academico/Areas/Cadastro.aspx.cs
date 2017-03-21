namespace GestaoEscolar.Academico.Areas
{
    using System;
    using System.Data;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;

    public partial class Cadastro : MotherPageLogado
    {
        #region Constantes

        private string nameSpaceResource = "Academico";
        private string chaveResource = "Areas.Cadastro.{0}";

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tad_id (ID do tipo area)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tad_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tad_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tad_id"] = value;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_tad_id = PreviousPage.Edit_tad_id;
                        Carregar();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroAoTentarCarregar"), UtilBO.TipoMensagem.Erro);
                }

                //Permissões da pagina
                Page.Form.DefaultButton = btnSalvar.UniqueID;
                btnSalvar.Visible = txtNome.Enabled = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao &&
                        (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
                Page.Form.DefaultFocus = txtNome.ClientID;

                chkCadastroEscola.Enabled &= btnSalvar.Visible;
            }
        }

        #endregion Page Life Cycle

        #region Métodos

        /// <summary>
        /// O método retorna o valor de um resource.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        private string RetornaResource(string chave)
        {
            return GetGlobalResourceObject(nameSpaceResource, String.Format(chaveResource, chave)).ToString();
        }

        /// <summary>
        /// Método para carregar um registro de tipo de area, a fim de atualizar suas informações.
        /// Recebe dados referente ao tipo de area para realizar a busca.
        /// </summary>
        private void Carregar()
        {
            ACA_TipoAreaDocumento entity = new ACA_TipoAreaDocumento { tad_id = VS_tad_id };
            ACA_TipoAreaDocumentoBO.GetEntity(entity);

            txtNome.Text = entity.tad_nome;
            chkCadastroEscola.Checked = entity.tad_cadastroEscola;

            bool integridade = ACA_TipoAreaDocumentoBO.VerificarIntegridade("tad_id", VS_tad_id.ToString(), "ACA_TipoAreaDocumento");

            chkCadastroEscola.Enabled = !integridade;

            lblMsgInfo.Text = integridade && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao ? UtilBO.GetErroMessage(RetornaResource("lblMsgInfo.Text"), UtilBO.TipoMensagem.Informacao) : string.Empty;
        }

        /// <summary>
        /// Método para salvar um tipo de area.
        /// </summary>
        private void Salvar()
        {
            try
            {
                ACA_TipoAreaDocumento entity = new ACA_TipoAreaDocumento { tad_id = VS_tad_id };
                ACA_TipoAreaDocumentoBO.GetEntity(entity);

                entity.tad_nome = txtNome.Text;
                entity.IsNew = VS_tad_id < 0;
                entity.tad_cadastroEscola = chkCadastroEscola.Checked;

                if (ACA_TipoAreaDocumentoBO.SalvarArea(entity))
                {
                    ApplicationWEB._GravaLogSistema(VS_tad_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tad_id: " + entity.tad_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Documento " + (VS_tad_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    RedirecionarPagina("~/Academico/Areas/Busca.aspx");
                }
            }
            catch (ValidationException e)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException e)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroAoTentarSalvar"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/Areas/Busca.aspx");
        }

        #endregion Eventos
    }
}