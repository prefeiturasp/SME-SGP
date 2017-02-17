using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.AvisosTextosGerais
{
    public partial class UCAvisosTextosGerais : MotherUserControl
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de atg_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        public int VS_atg_id
        {
            get
            {
                if (ViewState["VS_atg_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_atg_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_atg_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rad_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        public int VS_rda_id
        {
            get
            {
                if (ViewState["VS_rda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rda_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_rda_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rlt_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        public int VS_rlt_id
        {
            get
            {
                if (ViewState["VS_rlt_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rlt_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_rlt_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rlt_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        public int VS_pda_id
        {
            get
            {
                if (ViewState["VS_pda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_pda_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_pda_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que verifica se e cadastro
        /// de aviso ou do TipoAvisotextoGerais
        /// Padrao -1
        /// </summary>
        public int TipoAvisotextoGerais
        {
            get
            {
                if (ViewState["TipoAvisotextoGerais"] != null)
                {
                    return Convert.ToInt32(ViewState["TipoAvisotextoGerais"]);
                }
                return -1;
            }
            set
            {
                ViewState["TipoAvisotextoGerais"] = value;
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };

                UCComboUAEscola1.EnableEscolas = (UCComboUAEscola1.Uad_ID != Guid.Empty || !UCComboUAEscola1.DdlUA.Visible);

                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola1_IndexChangedUnidadeEscola()
        {
            try
            {
                UCComboCursoCurriculo.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo.PermiteEditar = false;

                if (UCComboUAEscola1.Esc_ID > 0 && UCComboUAEscola1.Uni_ID > 0)
                {
                    UCComboCursoCurriculo.CarregarPorEscolaSituacaoCurso(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, 1);

                    UCComboCursoCurriculo.SetarFoco();
                    UCComboCursoCurriculo.PermiteEditar = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        /// <summary>
        /// Load da pagina
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));

                //sm.Scripts.Add(new ScriptReference("~/Includes/redactor/redactor.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/ckeditor/ckeditor.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsAvisosTextosGerais.js"));
            }

            txtDescricao.config.toolbar = new object[]
                                        {
                                            new object[]
                                                {
                                                    "Cut", "Copy", "-", "Paste", "PasteText", "PasteFromWord", "-", "Undo",
                                                    "Redo", "-", "Find", "Replace", "-", "SelectAll", "RemoveFormat", "-",
                                                    "Table", "-", "Templates"
                                                },
                                            new object[]
                                                {
                                                    "Image", "Smiley", "SpecialChar", "-", "_dataAtual", "_diasAtraso",
                                                    "_nome", "_valorDivida", "-", "Bold", "Italic", "Underline", "Strike",
                                                    "TextColor", "BGColor", "-", "Subscript", "Superscript", "-",
                                                    "NumberedList", "BulletedList", "-", "Link", "Unlink", "Anchor",
                                                    "HorizontalRule"
                                                },
                                            "/", 
                                            new object[] {"Format", "Font", "FontSize"},
                                            new object[]
                                                {
                                                    "Outdent", "Indent", "-", "JustifyLeft", "JustifyCenter",
                                                    "JustifyRight", "JustifyBlock", "-", "Preview", "-", "About"
                                                },
                                        };


            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMessage.Text = message;

                    if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho
                        || TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio)
                    {
                        TrataFiltrosCabecalho();
                        CarregarCabecalho();
                    }
                    else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao)//Cadastro/Edicao de declaracao
                    {
                        CarregarRLT();
                        TrataFiltrosDeclaracao();
                    }
                    else
                    {
                        UCComboUAEscola1.Inicializar();

                        if (VS_atg_id > 0)//Edicao de aviso
                        {
                            Carregar();
                            TrataFiltrosAviso();
                        }
                        else//Cadastro de aviso
                        {
                            //Inicializa os campos de busca
                            InicializaCamposBusca();
                        }
                    }

                    divCampoAuxiliar.Visible = TipoAvisotextoGerais != 6;                    

                    Page.Form.DefaultFocus = UCComboUAEscola1.VisibleUA ? UCComboUAEscola1.ComboUA_ClientID : UCComboUAEscola1.ComboEscola_ClientID;
                    Page.Form.DefaultButton = btnSalvar.UniqueID;
                    UCComboCampoAuxiliar1.Focus();

                    btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método para carregar um registro de aviso texto geral, a fim de atualizar suas informações.
        /// Recebe dados referente ao aviso texto geral para realizar busca.
        /// </summary>
        /// <param name="atg_id">ID do aviso texto geral</param>
        public void CarregarCabecalho()
        {
            try
            {
                // Armazena valor ID da SAAI – Sala de apoio e acompanhamento a inclusão a ser alterada.
                VS_atg_id = ACA_AvisoTextoGeralBO.SelecionaPorTipoAviso(TipoAvisotextoGerais);

                // Busca da SAAI – Sala de apoio e acompanhamento a inclusão baseado no ID da SAAI – Sala de apoio e acompanhamento a inclusão.
                ACA_AvisoTextoGeral entAviso = new ACA_AvisoTextoGeral { atg_id = VS_atg_id };
                ACA_AvisoTextoGeralBO.GetEntity(entAviso);

                ESC_Escola entEscola = new ESC_Escola { esc_id = entAviso.esc_id };
                ESC_EscolaBO.GetEntity(entEscola);

                txtDescricao.Text = HttpUtility.HtmlDecode(entAviso.atg_descricao);
                //redactor_content.InnerText = HttpUtility.HtmlDecode(entAviso.atg_descricao);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os avisos textos gerais.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaCamposBusca()
        {
            UCComboUAEscola1.MostraApenasAtivas = true;

            UCComboUAEscola1.Inicializar();

            UCComboUAEscola1_IndexChangedUA();
        }

        /// <summary>
        /// Método para carregar um registro de aviso texto geral, a fim de atualizar suas informações.
        /// Recebe dados referente ao aviso texto geral para realizar busca.
        /// </summary>
        /// <param name="atg_id">ID do aviso texto geral</param>
        public void Carregar()
        {
            try
            {
                // Busca da SAAI – Sala de apoio e acompanhamento a inclusão baseado no ID da SAAI – Sala de apoio e acompanhamento a inclusão.
                ACA_AvisoTextoGeral entAviso = new ACA_AvisoTextoGeral { atg_id = VS_atg_id };
                ACA_AvisoTextoGeralBO.GetEntity(entAviso);

                ESC_Escola entEscola = new ESC_Escola { esc_id = entAviso.esc_id };
                ESC_EscolaBO.GetEntity(entEscola);

                if (UCComboUAEscola1.VisibleUA)
                {
                    // Buscar Unidade Administrativa Superior.
                    SYS_UnidadeAdministrativa entUA = new SYS_UnidadeAdministrativa { ent_id = entEscola.ent_id, uad_id = entEscola.uad_id };
                    SYS_UnidadeAdministrativaBO.GetEntity(entUA);

                    Guid uad_idSuperior = entEscola.uad_idSuperiorGestao.Equals(Guid.Empty) ? entUA.uad_idSuperior : entEscola.uad_idSuperiorGestao;

                    UCComboUAEscola1.Uad_ID = uad_idSuperior;

                    // Recarrega o combo de escolas com a uad_idSuperior.
                    UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();
                }

                //Carrega Escolas
                UCComboUAEscola1.MostraApenasAtivas = true;
                UCComboUAEscola1.SelectedValueEscolas = new[] { entEscola.esc_id, entAviso.uni_id };
                UCComboUAEscola1.PermiteAlterarCombos = true;

                //Carrega curso
                UCComboCursoCurriculo.CarregarPorEscolaSituacaoCurso(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, 1);
                UCComboCursoCurriculo.Valor = new int[] { entAviso.cur_id, entAviso.crr_id };

                //Carrega situacao
                cmbSituacao.SelectedValue = entAviso.atg_situacao.ToString();

                //Carrega titulo
                txtTitulo.Text = entAviso.atg_titulo;

                //Carrega check do cabecalho
                chkTimbre.Checked = entAviso.atg_timbreCabecalho;

                //Carrega tipo de aviso e campos auxiliares
                UCComboCampoAuxiliar1.ValorComboTipo = entAviso.atg_tipo;

                //Carrega text cin descricao
                txtDescricao.Text = HttpUtility.HtmlDecode(entAviso.atg_descricao);
                //redactor_content.InnerText = HttpUtility.HtmlDecode(entAviso.atg_descricao);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os avisos textos gerais.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para carregar um registro de aviso texto geral, a fim de atualizar suas informações.
        /// Recebe dados referente ao aviso texto geral para realizar busca.
        /// </summary>
        /// <param name="atg_id">ID do aviso texto geral</param>
        public void CarregarRLT()
        {
            try
            {
                //Carrega dados relatorio
                CFG_RelatorioDocumentoAluno entRlt = CarregaEntRelatorio();

                //Carrega dados parametro documento aluno
                CFG_ParametroDocumentoAluno entPDA = new CFG_ParametroDocumentoAluno
                {
                    pda_id = VS_pda_id,
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    rlt_id = VS_rlt_id
                };
                CFG_RelatorioDocumentoAlunoBO.GetEntity(entRlt);

                txtTitDeclaracao.Text = VS_rlt_id + " - " + entRlt.rda_nomeDocumento;

                //Carrega dados do aviso texto
                if (entRlt.atg_id > 0)
                {
                    VS_atg_id = Convert.ToInt32(entRlt.atg_id);
                    ACA_AvisoTextoGeral entAviso = new ACA_AvisoTextoGeral { atg_id = VS_atg_id };
                    ACA_AvisoTextoGeralBO.GetEntity(entAviso);

                    txtDescricao.Text = HttpUtility.HtmlDecode(entAviso.atg_descricao);
                    //redactor_content.InnerText = HttpUtility.HtmlDecode(entAviso.atg_descricao);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os avisos textos gerais.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Desabilita os filtros de escola, etc quando for cadastro de declaracao.
        /// </summary>
        private void TrataFiltrosDeclaracao()
        {
            //Esconde os filtros e parametros
            lblFdsMain.Text = "Cadastro da descrição de declarações";
            updFiltroAviso.Visible = false;

            //Esconde os chks que nao serao utilizados
            divChk.Visible = false;

            //Desabilita edicao do texto
            txtTitDeclaracao.Enabled = false;

            //Mostra div declaracao
            updDeclaracao.Visible = true;

            //Seta os campos do combo tipo aviso para cabecalho
            UCComboCampoAuxiliar1.MostrarTipoDeclaracao = true;
            UCComboCampoAuxiliar1.ValorComboTipo = TipoAvisotextoGerais;
            UCComboCampoAuxiliar1.MostraComboTipoAviso = true;
            UCComboCampoAuxiliar1.EnabledComboTipo = false;
        }

        /// <summary>
        /// Desabilita os filtros de escola, etc quando for cadastro de cabecalho.
        /// </summary>
        private void TrataFiltrosCabecalho()
        {
            //Esconde os filtros e parametros
            fdsFiltros.Visible = false;
            if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho)
            {
            lblFdsMain.Text = "Cadastro do cabeçalho de avisos e textos gerais";
                //Seta os campos do combo tipo aviso para cabecalho
                UCComboCampoAuxiliar1.MostrarTipoCabecalho = true;    
            }
            else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio) 
            {
                lblFdsMain.Text = "Cadastro do cabeçalho de relatórios";
                //Seta os campos do combo tipo aviso para cabecalho
                UCComboCampoAuxiliar1.MostrarTipoCabecalhoRelatorio = true;    
            }

            //Esconde os chks que nao serao utilizados
            divChk.Visible = false;

            //Seta os campos do combo tipo aviso para cabecalho
            UCComboCampoAuxiliar1.ValorComboTipo = TipoAvisotextoGerais;
           
            UCComboCampoAuxiliar1.MostraComboTipoAviso = false;
            
            //Esconde campos de declaracao
            updDeclaracao.Visible = false;
        }

        /// <summary>
        /// Trata os campos quando for edicao
        /// </summary>
        private void TrataFiltrosAviso()
        {
            UCComboUAEscola1.PermiteAlterarCombos =
            UCComboCursoCurriculo.PermiteEditar =
            updDeclaracao.Visible =
            UCComboCampoAuxiliar1.EnabledComboTipo = false;
        }

        /// <summary>
        /// Método para salvar um aviso texto geral.
        /// </summary>
        private void Salvar()
        {
            try
            {
                ACA_AvisoTextoGeral entAviso = new ACA_AvisoTextoGeral();

                if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho)
                {
                    entAviso.atg_titulo = "Cabeçalho";
                    entAviso.atg_timbreCabecalho = false;
                    entAviso.atg_anotacaoAula = false;
                    entAviso.atg_tipo = Convert.ToByte(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho);
                }
                else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao)
                {
                    entAviso.atg_titulo = "Declaração";
                    entAviso.atg_timbreCabecalho = false;
                    entAviso.atg_anotacaoAula = false;
                    entAviso.atg_tipo = Convert.ToByte(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao);
                }
                else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio)
                {
                    entAviso.atg_titulo = "Cabeçalho Relatório";
                    entAviso.atg_timbreCabecalho = false;
                    entAviso.atg_anotacaoAula = false;
                    entAviso.atg_tipo = Convert.ToByte(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio);
                }
                else
                {
                    entAviso.uni_id = UCComboUAEscola1.Uni_ID;
                    entAviso.esc_id = UCComboUAEscola1.Esc_ID;
                    entAviso.cur_id = UCComboCursoCurriculo.Valor[0];
                    entAviso.crr_id = UCComboCursoCurriculo.Valor[1];
                    entAviso.atg_titulo = txtTitulo.Text;
                    entAviso.atg_tipo = Convert.ToByte(UCComboCampoAuxiliar1.ValorComboTipo);
                    entAviso.atg_timbreCabecalho = chkTimbre.Checked;
                }

                entAviso.atg_anotacaoAula = false;
                entAviso.IsNew = VS_atg_id <= 0;
                //entAviso.atg_descricao = HttpUtility.HtmlEncode(redactor_content.InnerText);
                entAviso.atg_descricao = txtDescricao.Text; // HttpUtility.HtmlEncode(txtDescricao.Text);
                entAviso.atg_id = VS_atg_id;
                entAviso.atg_situacao = byte.Parse(cmbSituacao.SelectedValue);

                if (ACA_AvisoTextoGeralBO.Save(entAviso))
                {
                    ApplicationWEB._GravaLogSistema(VS_atg_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "atg_id: " + entAviso.atg_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(entAviso.atg_titulo + (VS_atg_id > 0 ? " alterado" : " incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao && VS_atg_id <= 0)//Adiciona atg_id na declaracao
                    {
                        CFG_RelatorioDocumentoAluno entRlt = CarregaEntRelatorio();
                        entRlt.atg_id = entAviso.atg_id;
                        entRlt.IsNew = false;

                        if (CFG_RelatorioDocumentoAlunoBO.Save(entRlt))
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "rda_id: " + entRlt.rda_id);
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage(entAviso.atg_titulo + " alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                    }

                    VoltarPagina();
                }
            }
            catch (ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar aviso texto geral.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Retorna a pagina de busca 
        /// </summary>
        private void VoltarPagina()
        {
            if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho
                || TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio)
            {
                Response.Redirect("~/Configuracao/CabecalhoAvisosTextosGerais/CadastroCabecalhoAvisosTextos.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao)
            {
                Response.Redirect("~/Configuracao/Declaracoes/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Academico/AvisosTextosGerais/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Carrega dados relatorio declaracao
        /// </summary>
        /// <returns></returns>
        private CFG_RelatorioDocumentoAluno CarregaEntRelatorio()
        {
            CFG_RelatorioDocumentoAluno entRlt = new CFG_RelatorioDocumentoAluno
            {
                rda_id = VS_rda_id,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                rlt_id = VS_rlt_id
            };
            CFG_RelatorioDocumentoAlunoBO.GetEntity(entRlt);

            return entRlt;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Handles the Click event of the btnSalvar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnNovo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/AvisosTextosGerais/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Handles the Click event of the btnCancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //VoltarPagina();
            if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Cabecalho
                || TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio)
            {
                Response.Redirect("~/Configuracao/CabecalhoAvisosTextosGerais/BuscaCabecalhoAvisosTextos.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (TipoAvisotextoGerais == (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao)
            {
                Response.Redirect("~/Configuracao/Declaracoes/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Academico/AvisosTextosGerais/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion
    }
}