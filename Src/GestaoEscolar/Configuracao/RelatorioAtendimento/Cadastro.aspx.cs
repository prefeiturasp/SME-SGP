using System;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.Validation.Exceptions;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

namespace GestaoEscolar.Configuracao.RelatorioAtendimento
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
        /// Armazena em viewstate o arquivo selecionado.
        /// </summary>
        private SYS_Arquivo VS_arquivo
        {
            get
            {
                return (SYS_Arquivo)(ViewState["VS_arquivo"] ?? new SYS_Arquivo());
            }

            set
            {
                ViewState["VS_arquivo"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rea_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_rea_id
        {
            get
            {
                if (ViewState["VS_rea_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rea_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_rea_id"] = value;
            }
        }
        
        /// <summary>
        /// Propriedade em ViewState que armazena valor de rea_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private List<CLS_RelatorioAtendimentoQuestionario> VS_lstQuestionarios
        {
            get
            {
                if (ViewState["VS_lstQuestionarios"] == null)
                    ViewState["VS_lstQuestionarios"] = new List<CLS_RelatorioAtendimentoQuestionario>();

                return (List<CLS_RelatorioAtendimentoQuestionario>)ViewState["VS_lstQuestionarios"];
            }
            set
            {
                ViewState["VS_lstQuestionarios"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados do relatório
        /// </summary>
        /// <param name="rea_id">ID do relatório</param>
        private void _LoadFromEntity(int rea_id)
        {
            try
            {
                VS_rea_id = rea_id;

                CLS_RelatorioAtendimento rea = new CLS_RelatorioAtendimento { rea_id = VS_rea_id };
                CLS_RelatorioAtendimentoBO.GetEntity(rea);

                txtTitulo.Text = rea.rea_titulo;
                txtTituloAnexo.Text = rea.rea_tituloAnexo;
                ddlPeriodicidade.SelectedValue = rea.rea_periodicidadePreenchimento.ToString();
                ddlTipo.SelectedValue = rea.rea_tipo.ToString();
                ddlTipo_SelectedIndexChanged(ddlTipo, new EventArgs());
                chkExibeHipotese.Checked = rea.rea_permiteEditarHipoteseDiagnostica;
                chkExibeRacaCor.Checked = rea.rea_permiteEditarRecaCor;
                hplAnexo.Visible = rea.arq_idAnexo > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                hplAnexo.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", rea.arq_idAnexo);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere ou altera o relatório
        /// </summary>
        private void Salvar()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTituloAnexo.Text) && !fupAnexo.HasFile && string.IsNullOrEmpty(hplAnexo.NavigateUrl))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.TituloAnexoSemArquivo").ToString());
                                
                CLS_RelatorioAtendimento rea = new CLS_RelatorioAtendimento
                {
                    rea_id = VS_rea_id,
                    rea_titulo = txtTitulo.Text,
                    rea_tipo = Convert.ToByte(ddlTipo.SelectedValue),
                    rea_permiteEditarRecaCor = Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE && chkExibeRacaCor.Checked,
                    rea_permiteEditarHipoteseDiagnostica = Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE && chkExibeHipotese.Checked,
                    tds_id = (Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.RP ? -1 : UCComboTipoDisciplina.Valor),
                    rea_periodicidadePreenchimento = Convert.ToByte(ddlPeriodicidade.SelectedValue),
                    rea_tituloAnexo = txtTituloAnexo.Text,
                    IsNew = VS_rea_id <= 0
                };

                if (fupAnexo.HasFile)
                {
                    if (GestaoEscolarUtilBO.VerificarArquivo(fupAnexo.PostedFile))
                    {
                        string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(fupAnexo.PostedFile.FileName);
                        string nomeArquivo = fupAnexo.PostedFile.FileName;
                        int tamanhoArquivo = fupAnexo.PostedFile.ContentLength;
                        string typeMime = fupAnexo.PostedFile.ContentType;

                        Stream arquivo = CopiarArquivo(fupAnexo.PostedFile.InputStream);

                        VS_arquivo = CriarAnexo(arquivo, nomeArquivo, tamanhoArquivo, typeMime);
                    }
                }

                if (!VS_lstQuestionarios.Any(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.NenhumQuestionarioAdicionado").ToString());

                List<CLS_RelatorioAtendimentoGrupo> lstGrupo = CarregaGruposPreenchidos();

                List<CLS_RelatorioAtendimentoCargo> lstCargo = CarregaCargosPreenchidos();

                if (!lstGrupo.Any(g => g.rag_permissaoAprovacao || g.rag_permissaoConsulta || g.rag_permissaoEdicao || g.rag_permissaoExclusao) &&
                    !lstCargo.Any(c => c.rac_permissaoAprovacao || c.rac_permissaoConsulta || c.rac_permissaoEdicao || c.rac_permissaoExclusao))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.NenhumaPermissao").ToString());

                if (CLS_RelatorioAtendimentoBO.Salvar(rea, lstGrupo, lstCargo, VS_lstQuestionarios, VS_arquivo))
                {
                    string message = "";
                    if (VS_rea_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "rea_id: " + rea.rea_id);
                        message = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.RelatorioIncluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "rea_id: " + rea.rea_id);
                        message = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.RelatorioAlteradoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                    if (ParametroPermanecerTela)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        lblMessage.Text = message;
                        VS_rea_id = rea.rea_id;
                        _LoadFromEntity(VS_rea_id);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = message;
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/RelatorioAtendimento/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// O método copia o stream do arquivo selecionado para uma MemoryStream, possibilitando sua leitura múltiplas vezes.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        private Stream CopiarArquivo(Stream inputStream)
        {
            long tamanho = inputStream.Length;
            byte[] buffer = new byte[tamanho];
            MemoryStream memoryStream = new MemoryStream();

            int cont = inputStream.Read(buffer, 0, (int)tamanho);

            while (cont > 0)
            {
                memoryStream.Write(buffer, 0, cont);
                cont = inputStream.Read(buffer, 0, (int)tamanho);
            }

            memoryStream.Position = 0;
            inputStream.Close();
            return memoryStream;
        }

        /// <summary>
        /// Monta uma entidade de arquivo de acordo com o documento passado.
        /// </summary>
        /// <returns>Entidade de arquivo.</returns>
        public SYS_Arquivo CriarAnexo(Stream arquivo, string nomeArquivo, int tamanho, string typeMime)
        {
            return !string.IsNullOrEmpty(nomeArquivo) ?
                   new SYS_Arquivo
                   {
                       arq_nome = Path.GetFileName(nomeArquivo)
                       ,
                       arq_tamanhoKB = tamanho
                       ,
                       arq_typeMime = typeMime
                       ,
                       arq_data = GetBytes(arquivo)
                       ,
                       arq_situacao = (byte)SYS_ArquivoSituacao.Ativo
                       ,
                       arq_dataCriacao = DateTime.Now
                       ,
                       arq_dataAlteracao = DateTime.Now
                   } : null;
        }

        /// <summary>
        /// Retorna o array de Bytes do arquivo
        /// </summary>
        /// <param name="arquivo">Stream do arquivo</param>
        /// <returns>Array de Bytes</returns>
        public byte[] GetBytes(Stream arquivo)
        {
            byte[] file = null;

            if (arquivo != null)
            {
                int tamanho = Convert.ToInt32(arquivo.Length);
                file = new byte[tamanho];

                if (arquivo.Length == 0)
                    throw new ValidationException("O arquivo tem 0 bytes, por isso ele não será anexado.");

                arquivo.Read(file, 0, tamanho);
            }

            return file;
        }

        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private void CarregaCargos()
        {
            gvCargo.DataSource = CLS_RelatorioAtendimentoCargoBO.SelectBy_rea_id(VS_rea_id);
            gvCargo.DataBind();
        }

        /// <summary>
        /// Carrega os grupos
        /// </summary>
        private void CarregaGrupos()
        {
            gvGrupo.DataSource = CLS_RelatorioAtendimentoGrupoBO.SelectBy_rea_id(VS_rea_id, __SessionWEB.__UsuarioWEB.Grupo.sis_id);
            gvGrupo.DataBind();
        }

        /// <summary>
        /// Carrega os questionarios
        /// </summary>
        private void CarregaQuestionarios()
        {
            VS_lstQuestionarios = CLS_RelatorioAtendimentoQuestionarioBO.SelectBy_rea_id(VS_rea_id);
            gvQuestionario.DataSource = VS_lstQuestionarios;
            gvQuestionario.DataBind();
        }

        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private List<CLS_RelatorioAtendimentoCargo> CarregaCargosPreenchidos()
        {
            List<CLS_RelatorioAtendimentoCargo> lstCargo = new List<CLS_RelatorioAtendimentoCargo>();

            foreach (GridViewRow item in gvCargo.Rows)
            {
                CheckBox chkpermissaoConsulta = (CheckBox)item.FindControl("chkpermissaoConsulta");
                CheckBox chkpermissaoEdicao = (CheckBox)item.FindControl("chkpermissaoEdicao");
                CheckBox chkpermissaoExclusao = (CheckBox)item.FindControl("chkpermissaoExclusao");
                CheckBox chkpermissaoAprovacao = (CheckBox)item.FindControl("chkpermissaoAprovacao");

                if (chkpermissaoConsulta != null && chkpermissaoEdicao != null && chkpermissaoExclusao != null && chkpermissaoAprovacao != null)
                {
                    if (!chkpermissaoConsulta.Checked && !chkpermissaoEdicao.Checked && !chkpermissaoExclusao.Checked && !chkpermissaoAprovacao.Checked)
                        continue;

                    lstCargo.Add(new CLS_RelatorioAtendimentoCargo
                    {
                        rea_id = VS_rea_id,
                        crg_id = Convert.ToInt32(gvCargo.DataKeys[item.RowIndex]["crg_id"].ToString()),
                        rac_permissaoConsulta = chkpermissaoConsulta.Checked,
                        rac_permissaoAprovacao = chkpermissaoAprovacao.Checked,
                        rac_permissaoExclusao = chkpermissaoExclusao.Checked,
                        rac_permissaoEdicao = chkpermissaoEdicao.Checked
                    });
                }
            }

            return lstCargo;
        }

        /// <summary>
        /// Carrega os grupos
        /// </summary>
        private List<CLS_RelatorioAtendimentoGrupo> CarregaGruposPreenchidos()
        {
            List<CLS_RelatorioAtendimentoGrupo> lstGrupo = new List<CLS_RelatorioAtendimentoGrupo>();

            foreach (GridViewRow item in gvGrupo.Rows)
            {
                CheckBox chkpermissaoConsulta = (CheckBox)item.FindControl("chkpermissaoConsulta");
                CheckBox chkpermissaoEdicao = (CheckBox)item.FindControl("chkpermissaoEdicao");
                CheckBox chkpermissaoExclusao = (CheckBox)item.FindControl("chkpermissaoExclusao");
                CheckBox chkpermissaoAprovacao = (CheckBox)item.FindControl("chkpermissaoAprovacao");

                if (chkpermissaoConsulta != null && chkpermissaoEdicao != null && chkpermissaoExclusao != null && chkpermissaoAprovacao != null)
                {
                    if (!chkpermissaoConsulta.Checked && !chkpermissaoEdicao.Checked && !chkpermissaoExclusao.Checked && !chkpermissaoAprovacao.Checked)
                        continue;

                    lstGrupo.Add(new CLS_RelatorioAtendimentoGrupo
                    {
                        rea_id = VS_rea_id,
                        gru_id = new Guid(gvGrupo.DataKeys[item.RowIndex]["gru_id"].ToString()),
                        rag_permissaoConsulta = chkpermissaoConsulta.Checked,
                        rag_permissaoAprovacao = chkpermissaoAprovacao.Checked,
                        rag_permissaoExclusao = chkpermissaoExclusao.Checked,
                        rag_permissaoEdicao = chkpermissaoEdicao.Checked
                    });
                }
            }

            return lstGrupo;
        }
        
        /// <summary>
        /// Inicializa os campos da tela
        /// </summary>
        private void Inicializar()
        {
            VS_arquivo = new SYS_Arquivo();
            VS_rea_id = -1;
            txtTitulo.Text = txtTituloAnexo.Text = "";
            ddlTipo.SelectedValue = "0";
            ddlPeriodicidade.SelectedValue = "0";
            chkExibeHipotese.Checked = chkExibeRacaCor.Checked = divDisciplina.Visible = false;
            UCComboTipoDisciplina.Valor = -1;
            hplAnexo.Text = "";
            divAdicionarQuestionario.Visible = false;
            UCComboQuestionario.CarregarQuestionario();
            UCComboTipoDisciplina.CarregarTipoDisciplinaTipo((byte)ACA_TipoDisciplinaBO.TipoDisciplina.RecuperacaoParalela);
            gvCargo.DataSource = new DataTable();
            gvGrupo.DataSource = new DataTable();
            gvQuestionario.DataSource = VS_lstQuestionarios;
            gvCargo.DataBind();
            gvGrupo.DataBind();
            gvQuestionario.DataBind();
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            }

            if (!IsPostBack)
            {
                try
                {
                    Inicializar();

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();

                        _LoadFromEntity(PreviousPage.EditItem);
                    }
                    else
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();
                        ckbBloqueado.Visible = false;
                    }

                    CarregaCargos();
                    CarregaGrupos();
                    CarregaQuestionarios();

                    Page.Form.DefaultFocus = txtTitulo.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/RelatorioAtendimento/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            divHipotese.Visible = Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE;
            divRacaCor.Visible = Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE;
            divDisciplina.Visible = Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.RP;

            if (Convert.ToByte(ddlTipo.SelectedValue) != (byte)CLS_RelatorioAtendimentoTipo.AEE)
                chkExibeHipotese.Checked = chkExibeRacaCor.Checked = false;
            if (Convert.ToByte(ddlTipo.SelectedValue) != (byte)CLS_RelatorioAtendimentoTipo.RP)
                UCComboTipoDisciplina.Valor = -1;
        }
        
        protected void btnAdicionarQuestionario_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCComboQuestionario.Valor <= 0)
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.QuestionarioObrigatorio").ToString());

                if (VS_lstQuestionarios.Any(q => q.qst_id == UCComboQuestionario.Valor && q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.QuestionarioJaAdicionado").ToString());

                int raq_id = (VS_lstQuestionarios.Any() ? VS_lstQuestionarios.Max(q => q.raq_id) + 1 : 1);
                while (VS_lstQuestionarios.Any(q => q.raq_id == raq_id))
                    raq_id++;

                VS_lstQuestionarios.Add(new CLS_RelatorioAtendimentoQuestionario
                {
                    rea_id = VS_rea_id,
                    raq_id = raq_id,
                    qst_id = UCComboQuestionario.Valor,
                    qst_titulo = UCComboQuestionario.Texto,
                    raq_ordem = (VS_lstQuestionarios.Any() ? VS_lstQuestionarios.Max(q => q.raq_ordem) + 1 : 1),
                    raq_situacao = 1,
                    IsNew = true
                });

                gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                gvQuestionario.DataBind();

                divAdicionarQuestionario.Visible = false;
                UCComboQuestionario.Valor = -1;
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroAdicionarQuestionario").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCacelarQuestionario_Click(object sender, EventArgs e)
        {
            divAdicionarQuestionario.Visible = false;
            UCComboQuestionario.Valor = -1;
        }

        protected void gvQuestionario_DataBound(object sender, EventArgs e)
        {
            GridView grv = (GridView)sender;
            if (grv.Rows.Count > 0)
            {
                ((ImageButton)grv.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)grv.Rows[grv.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }

        protected void gvQuestionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                }
                ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
                if (_btnSubir != null)
                {
                    _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                    _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
                if (_btnDescer != null)
                {
                    _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                    _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void gvQuestionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idDescer = Convert.ToInt32(gvQuestionario.DataKeys[index - 1]["raq_id"]);
                    int idSubir = Convert.ToInt32(gvQuestionario.DataKeys[index]["raq_id"]);
                    int ordemSubir = VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idSubir).First())].raq_ordem;
                    int ordemDescer = VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idDescer).First())].raq_ordem;

                    VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idSubir).First())].raq_ordem = ordemDescer;
                    VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idDescer).First())].raq_ordem = ordemSubir;

                    VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

                    gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                    gvQuestionario.DataBind();

                    if (gvQuestionario.Rows.Count > 0)
                    {
                        ((ImageButton)gvQuestionario.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)gvQuestionario.Rows[gvQuestionario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idDescer = Convert.ToInt32(gvQuestionario.DataKeys[index]["raq_id"]);
                    int idSubir = Convert.ToInt32(gvQuestionario.DataKeys[index + 1]["raq_id"]);
                    int ordemSubir = VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idSubir).First())].raq_ordem;
                    int ordemDescer = VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idDescer).First())].raq_ordem;

                    VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idSubir).First())].raq_ordem = ordemDescer;
                    VS_lstQuestionarios[VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idDescer).First())].raq_ordem = ordemSubir;

                    VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

                    gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                    gvQuestionario.DataBind();

                    if (gvQuestionario.Rows.Count > 0)
                    {
                        ((ImageButton)gvQuestionario.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)gvQuestionario.Rows[gvQuestionario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["raq_id"]);

                    if (idExcluir > 0 && VS_lstQuestionarios.Any(l => l.raq_id == idExcluir))
                    {
                        int ind = VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First());
                        int ordem = VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First().raq_ordem;

                        //Ajusta as ordens
                        for (int i = ind + 1; i < VS_lstQuestionarios.Count; i++)
                        {
                            VS_lstQuestionarios[i].raq_ordem = ordem;
                            ordem += 1;
                        }

                        VS_lstQuestionarios.RemoveAt(ind);
                    }
                    VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

                    gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                    gvQuestionario.DataBind();

                    if (gvQuestionario.Rows.Count > 0)
                    {
                        ((ImageButton)gvQuestionario.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)gvQuestionario.Rows[gvQuestionario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnNovoQuestionario_Click(object sender, EventArgs e)
        {
            divAdicionarQuestionario.Visible = true;
            UCComboQuestionario.Valor = -1;
        }

        #endregion
    }
}