namespace GestaoEscolar.Academico.Areas
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;

    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        private const int grvAreasIndiceColunaOrdem = 2;
        private const int grvAreasIndiceColunaExcluir = 4;

        private string nameSpaceResource = "Academico";
        private string chaveResource = "Areas.Busca.{0}";

        #endregion Constantes

        #region Propriedades

        public int Edit_tad_id
        {
            get
            {
                return Convert.ToInt32(grvAreas.DataKeys[grvAreas.EditIndex].Values["tad_id"] ?? 0);
            }
        }

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
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroArea.js"));
            }

            if (!IsPostBack)
            {
                string mensagem = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(mensagem))
                {
                    lblMensagem.Text = mensagem;
                }

                try
                {
                    //Permissões da pagina
                    grvAreas.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                    grvAreas.Columns[grvAreasIndiceColunaOrdem].Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao;
                    grvAreas.Columns[grvAreasIndiceColunaExcluir].Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao;

                    btnNovaArea.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao;

                    Pesquisar();

                    if (grvAreas.Rows.Count > 0)
                    {
                        ((ImageButton)grvAreas.Rows[0].FindControl("btnSubir")).Visible = false;
                        ((ImageButton)grvAreas.Rows[grvAreas.Rows.Count - 1].FindControl("btnDescer")).Visible = false;
                    }

                    updAreas.Update();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroCarregarDados"), UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = Page.Form.DefaultFocus = btnNovaArea.UniqueID;
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
        /// Seleciona os tipos de áreas de documentos.
        /// </summary>
        private void Pesquisar()
        {
            odsAreas.SelectParameters.Clear();
            odsAreas.SelectParameters.Add("admin", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());

            // atualiza o grid
            grvAreas.DataBind();

        }

        #endregion Métodos

        #region Eventos

        protected void btnNovaArea_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/Areas/Cadastro.aspx");
        }

        protected void odsAreas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void grvAreas_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ACA_TipoAreaDocumentoBO.GetTotalRecords();
        }

        protected void grvAreas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                    btnAlterar.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                ImageButton btnSubir = (ImageButton)e.Row.FindControl("btnSubir");
                if (btnSubir != null)
                {
                    btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                    btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton btnDescer = (ImageButton)e.Row.FindControl("btnDescer");
                if (btnDescer != null)
                {
                    btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                    btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void grvAreas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_tad_id = Convert.ToInt32(grvAreas.DataKeys[index].Values["tad_id"]);

                    lblPopUpExclusao.Text = ACA_TipoAreaDocumentoBO.VerificarIntegridade("tad_id", VS_tad_id.ToString(), "ACA_TipoAreaDocumento") ?
                        "Já existem links/documentos cadastrados por unidades escolares. Deseja realmente excluir?" :
                        "Confirma a exclusão?";

                    updMensagemExclusao.Update();

                    ScriptManager.RegisterStartupScript(Page
                                                          , typeof(Page)
                                                          , "AbrePopUpExclusaoVinculo"
                                                          , "$('#divConfirmaExclusao').dialog('open');"
                                                          , true);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroExcluirTipoArea"), UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tad_idDescer = Convert.ToInt32(grvAreas.DataKeys[index - 1]["tad_id"]);
                    int tad_ordemDescer = Convert.ToByte(grvAreas.DataKeys[index]["tad_ordem"]);
                    ACA_TipoAreaDocumento entityDescer = new ACA_TipoAreaDocumento { tad_id = tad_idDescer };
                    ACA_TipoAreaDocumentoBO.GetEntity(entityDescer);
                    entityDescer.tad_ordem = tad_ordemDescer;

                    int tad_idSubir = Convert.ToInt32(grvAreas.DataKeys[index]["tad_id"]);
                    int tad_ordemSubir = Convert.ToByte(grvAreas.DataKeys[index - 1]["tad_ordem"]);
                    ACA_TipoAreaDocumento entitySubir = new ACA_TipoAreaDocumento { tad_id = tad_idSubir };
                    ACA_TipoAreaDocumentoBO.GetEntity(entitySubir);
                    entitySubir.tad_ordem = tad_ordemSubir;

                    if (ACA_TipoAreaDocumentoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        grvAreas.DataBind();
                        if (grvAreas.Rows.Count > 0)
                        {
                            ((ImageButton)grvAreas.Rows[0].FindControl("btnSubir")).Visible = false;
                            ((ImageButton)grvAreas.Rows[grvAreas.Rows.Count - 1].FindControl("btnDescer")).Visible = false;
                        }

                        updAreas.Update();

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tad_id: " + tad_idSubir);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tad_id: " + tad_idDescer);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroSubir"), UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tad_idDescer = Convert.ToInt32(grvAreas.DataKeys[index]["tad_id"]);
                    int tad_ordemDescer = Convert.ToByte(grvAreas.DataKeys[index + 1]["tad_ordem"]);
                    ACA_TipoAreaDocumento entityDescer = new ACA_TipoAreaDocumento { tad_id = tad_idDescer };
                    ACA_TipoAreaDocumentoBO.GetEntity(entityDescer);
                    entityDescer.tad_ordem = tad_ordemDescer;

                    int tad_idSubir = Convert.ToInt32(grvAreas.DataKeys[index + 1]["tad_id"]);
                    int tad_ordemSubir = Convert.ToByte(grvAreas.DataKeys[index]["tad_ordem"]);
                    ACA_TipoAreaDocumento entitySubir = new ACA_TipoAreaDocumento { tad_id = tad_idSubir };
                    ACA_TipoAreaDocumentoBO.GetEntity(entitySubir);
                    entitySubir.tad_ordem = tad_ordemSubir;

                    if (ACA_TipoAreaDocumentoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        grvAreas.DataBind();

                        if (grvAreas.Rows.Count > 0)
                        {
                            ((ImageButton)grvAreas.Rows[0].FindControl("btnSubir")).Visible = false;
                            ((ImageButton)grvAreas.Rows[grvAreas.Rows.Count - 1].FindControl("btnDescer")).Visible = false;
                        }

                        updAreas.Update();

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tad_id: " + tad_idSubir);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tad_id: " + tad_idDescer);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroDescer"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ACA_TipoAreaDocumento entity = new ACA_TipoAreaDocumento { tad_id = VS_tad_id };

                if (ACA_TipoAreaDocumentoBO.Delete(entity))
                {
                    grvAreas.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tad_id: " + VS_tad_id);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("SucessoExclusaoPopup"), UtilBO.TipoMensagem.Sucesso);

                    if (grvAreas.Rows.Count > 0)
                    {
                        ((ImageButton)grvAreas.Rows[0].FindControl("btnSubir")).Visible = false;
                        ((ImageButton)grvAreas.Rows[grvAreas.Rows.Count - 1].FindControl("btnDescer")).Visible = false;
                    }

                    updAreas.Update();
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroExcluirTipoArea"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}