using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Collections.Generic;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboUAEscola : MotherUserControl
    {
        #region Delegates

        public delegate void OnSelectedIndexChangedUA();

        public event OnSelectedIndexChangedUA IndexChangedUA;

        public delegate void OnSelectedIndexChangedUA_Sender(UCComboUAEscola sender);

        public event OnSelectedIndexChangedUA_Sender IndexChangedUA_Sender;

        public delegate void OnSelectedIndexChanged();

        public event OnSelectedIndexChanged IndexChangedUnidadeEscola;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Propriedade que retorna se está configurado para filtrar por UA.
        /// </summary>
        public bool FiltroEscola
        {
            get
            {
                if (ViewState["FiltroEscola"] != null)
                {
                    return Convert.ToBoolean(ViewState["FiltroEscola"]);
                }
                return false;
            }
            set
            {
                ViewState["FiltroEscola"] = value;
            }
        }

        /// <summary>
        /// Propriedade que retorna se está configurado para filtrar
        /// pela permissão do usuário.
        /// </summary>
        private bool PermissaoUsuario
        {
            get
            {
                if (ViewState["PermissaoUsuario"] != null)
                {
                    return Convert.ToBoolean(ViewState["PermissaoUsuario"]);
                }
                return true;
            }
            set
            {
                ViewState["PermissaoUsuario"] = value;
            }
        }

        /// <summary>
        /// Propriedade que retorna se o parametro de controle de escolas será utilizado, e se caso for utilizado retornará o seu valor
        /// se o seu valor retornado for null a busca será indiferente em relação as escolas controladas
        /// se o seu valor retornado for true a busca deverá retornar as escolas controladas pelo sistema
        /// se o seu valor retornado for false a busca deverá retornar as escolas que não são controladas pelo sistema
        /// </summary>
        public Nullable<bool> FiltroEscolasControladas
        {
            get
            {
                if (ViewState["_VS_FiltroEscolasControladas"] != null)
                {
                    return Convert.ToBoolean(ViewState["_VS_FiltroEscolasControladas"]);
                }

                return null;
            }
            set
            {
                ViewState["_VS_FiltroEscolasControladas"] = value;
            }
        }

        /// <summary>
        /// Propriedade usada para carregar os combos apenas com Escolas ativas. True = Mostr Apenas Ativas / False ou NULL = Mostra Todas
        /// </summary>
        public bool MostraApenasAtivas
        {
            get
            {
                if (ViewState["MostraApenasAtivas"] != null)
                    return Convert.ToBoolean(ViewState["MostraApenasAtivas"]);
                return false;
            }
            set
            {
                ViewState["MostraApenasAtivas"] = value;
            }
        }

        /// <summary>
        /// Configura validação do combo de unidade administrativa.
        /// </summary>
        public bool ObrigatorioUA
        {
            get
            {
                if (ViewState["ObrigatorioUA"] != null)
                    return Convert.ToBoolean(ViewState["ObrigatorioUA"]);
                return false;
            }
            set
            {
                ViewState["ObrigatorioUA"] = value;
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblUA);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblUA);
                }
                cpvUA.Visible = value;
            }
        }

        public List<int> VS_EscolasPapeis
        {
            get
            {
                if (ViewState["VS_EscolasPapeis"] == null)
                    ViewState["VS_EscolasPapeis"] = new List<int>();
                return (List<int>)ViewState["VS_EscolasPapeis"];
            }
            set
            {
                ViewState["VS_EscolasPapeis"] = value;
            }
        }

        /// <summary>
        /// Configura validação do combo de escola.
        /// </summary>
        public bool ObrigatorioEscola
        {
            get
            {
                if (ViewState["ObrigatorioEscola"] != null)
                    return Convert.ToBoolean(ViewState["ObrigatorioEscola"]);
                return false;
            }
            set
            {
                ViewState["ObrigatorioEscola"] = value;
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblEscola);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblEscola);
                }
                cpvEscola.Visible = value;
            }
        }

        /// <summary>
        /// Propriedade que seta asterísco no label, porém sem o Compare Validator
        /// </summary>
        public bool AsteriscoObg
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblEscola);
                    AdicionaAsteriscoObrigatorio(lblUA);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblEscola);
                    RemoveAsteriscoObrigatorio(lblUA);
                }
            }
        }

        /// <summary>
        /// Retorna o valor da propriedade Visible do combo de UA
        /// </summary>
        public bool VisibleUA
        {
            get
            {
                return ddlUA.Visible;
            }
        }

        /// <summary>
        /// Retorna o valor da propriedade Visible do combo de Escolas
        /// </summary>
        public bool VisibleEscolas
        {
            get
            {
                return ddlUnidadeEscola.Visible;
            }
        }

        /// <summary>
        /// Configura título do combo de unidade adminsitrativa.
        /// </summary>
        public string LabelUA
        {
            get
            {
                return lblUA.Text;
            }
            set
            {
                lblUA.Text = value;
                cpvUA.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Retorna o título da Unidade Administrativa superior.
        /// </summary>
        public string TituloUA
        {
            get
            {
                return lblUA.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "").Replace("*", "");
            }
        }

        /// <summary>
        /// Retorna o título da Unidade Administrativa superior.
        /// </summary>
        public string TituloEscola
        {
            get
            {
                return lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "").Replace("*", "");
            }
        }

        /// <summary>
        /// Configura título do combo de escola.
        /// </summary>
        public string LabelEscola
        {
            set
            {
                lblEscola.Text = value;
                cpvEscola.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        public bool ExibeComboEscola
        {
            set
            {
                DdlEscola.Visible = value;
                lblEscola.Visible = value;
            }
            get
            {
                return DdlEscola.Visible;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione uma Escola / Unidade" do dropdownlist.
        /// Por padrão é false e a mensagem "Selecione uma Escola / Unidade" não é exibida.
        /// </summary>
        public bool MostrarMessageSelecioneEscola
        {
            set
            {
                string textoEscola = string.Empty;

                if ((value) && (ddlUnidadeEscola.Items.FindByValue("-1;-1") == null))
                    if (lblEscola.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                        textoEscola = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    else if (lblEscola.Text.EndsWith("*"))
                        textoEscola = lblEscola.Text.Replace("*", "");
                    else
                        textoEscola = lblEscola.Text;

                if (!string.IsNullOrEmpty(textoEscola))
                {
                    ddlUnidadeEscola.Items.Insert(0, new ListItem
                        (
                            String.Concat("-- Selecione uma ", textoEscola.ToLower(), " --")
                            , "-1;-1"
                            , true
                        ));
                }
                ddlUnidadeEscola.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione uma Unidade Adm." do dropdownlist.
        /// Por padrão é false e a mensagem "Selecione uma Unidade Adm." não é exibida.
        /// </summary>
        public bool MostrarMessageSelecioneUA
        {
            set
            {
                string textoUA = string.Empty;

                if (value)
                    if (lblUA.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                        textoUA = lblUA.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    else if (lblUA.Text.EndsWith("*"))
                        textoUA = lblUA.Text.Replace("*", "");
                    else
                        textoUA = lblUA.Text;

                ddlUA.Items.Insert(0, new ListItem
                    (
                        String.Concat("-- Selecione um(a) ", textoUA, " --")
                        , Guid.Empty.ToString()
                        , true
                    ));
                ddlUA.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Seta o foco no combo de escolas.
        /// </summary>
        public bool FocoEscolas
        {
            set
            {
                if (value)
                {
                    ddlUnidadeEscola.Focus();
                }
            }
        }

        /// <summary>
        /// ClientID do combo de UA
        /// </summary>
        public string ComboUA_ClientID
        {
            get
            {
                return ddlUA.ClientID;
            }
        }

        /// <summary>
        /// ClientID do combo de UA
        /// </summary>
        public string ComboEscola_ClientID
        {
            get
            {
                return ddlUnidadeEscola.ClientID;
            }
        }

        /// <summary>
        /// Propriedade Enable do combo de escolas
        /// </summary>
        public bool EnableEscolas
        {
            set
            {
                ddlUnidadeEscola.Enabled = value;
            }
        }

        /// <summary>
        /// Seta a propriedade Enabled nos combos da tela.
        /// </summary>
        public bool PermiteAlterarCombos
        {
            set
            {
                ddlUA.Enabled = value;
                ddlUnidadeEscola.Enabled = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo de Escolas.
        /// </summary>
        public int WidthComboEscolas
        {
            set
            {
                ddlUnidadeEscola.Width = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo de UA.
        /// </summary>
        public int WidthComboUA
        {
            set
            {
                ddlUA.Width = value;
            }
        }

        /// <summary>
        /// Recupera ou seta o valor da Unidade administrativa superior selecionada no combo.
        /// </summary>
        public Guid Uad_ID
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlUA.SelectedValue))
                    return new Guid(ddlUA.SelectedValue);

                return Guid.Empty;
            }
            set
            {
                ddlUA.SelectedValue = value.ToString();

                // Carrega combo de escolas de acordo com UA selecionada.
                CarregaEscolaPorUASuperiorSelecionada();
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedValue do Combo.
        /// Valor[0] = Esc_ID
        /// Valor[1] = Uni_id
        /// </summary>
        public int[] SelectedValueEscolas
        {
            set
            {
                ListItem itemDDL = ddlUnidadeEscola.Items.FindByValue(value[0] + ";" + value[1]);

                if (itemDDL != null)
                    ddlUnidadeEscola.SelectedValue = itemDDL.Value;
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedIndex do Combo Ua.
        /// </summary>
        public int SelectedIndexUa
        {
            set
            {
                ddlUA.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedIndex do Combo Escola.
        /// </summary>
        public int SelectedIndexEscolas
        {
            set
            {
                ddlUnidadeEscola.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Propriedade que verifica quantos items existem no combo de escolas
        /// </summary>
        public int QuantidadeItemsComboEscolas
        {
            get
            {
                return ddlUnidadeEscola.Items.Count;
            }
        }

        /// <summary>
        /// Propriedade que verifica quantos items existem no combo de uas
        /// </summary>
        public int QuantidadeItemsComboUAs
        {
            get
            {
                return ddlUA.Items.Count;
            }
        }

        /// <summary>
        /// Retorna o Esc_ID selecionado no combo.
        /// </summary>
        public int Esc_ID
        {
            get
            {
                string[] str = ddlUnidadeEscola.SelectedValue.Split(';');

                if (str.Length == 2)
                    return Convert.ToInt32(str[0]);

                return -1;
            }
        }

        /// <summary>
        /// Retorna Uni_ID selecionado no combo.
        /// </summary>
        public int Uni_ID
        {
            get
            {
                string[] str = ddlUnidadeEscola.SelectedValue.Split(';');

                if (str.Length == 2)
                    return Convert.ToInt32(str[1]);

                return -1;
            }
        }

        /// <summary>
        /// Seta o validation group dos combos.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvUA.ValidationGroup = value;
                cpvEscola.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Seta o validation group da UA.
        /// </summary>
        public string ValidationGroupUa
        {
            set
            {
                cpvUA.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Seta o validation group da escola.
        /// </summary>
        public string ValidationGroupEscola
        {
            set
            {
                cpvEscola.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Seta se o combo de escola será carregado automaticamente pela Unidade Administrativa
        /// superior, quando selecionada.
        /// </summary>
        public bool CarregarEscolaAutomatico
        {
            get
            {
                return ddlUA.AutoPostBack;
            }

            set
            {
                ddlUA.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Propriedade que retorna o valor selecionado no combo escola.
        /// </summary>
        public string ValorComboEscola
        {
            get
            {
                return ddlUnidadeEscola.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Propriedade que retorna o valor em inteiro selecionado no combo escola.
        /// </summary>
        public string ValorComboEscolaSelectedValue
        {
            get
            {
                return ddlUnidadeEscola.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Propriedade que retorna o valor selecionado no combo UA.
        /// </summary>
        public string ValorComboUA
        {
            get
            {
                return ddlUA.SelectedItem.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public DropDownList DdlUA
        {
            get { return ddlUA; }
        }

        /// <summary>
        ///
        /// </summary>
        public DropDownList DdlEscola
        {
            get { return ddlUnidadeEscola; }
        }

        #endregion Propriedades

        #region Eventos

        #region Page Lyfe Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlUA.AutoPostBack |= (IndexChangedUA != null);
            ddlUnidadeEscola.AutoPostBack = (IndexChangedUnidadeEscola != null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FiltroEscolasControladas = true;
            }
        }

        #endregion Page Lyfe Cycle

        protected void ddlUA_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUAIndexChanged();
        }

        protected void ddlUnidadeEscola_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChangedUnidadeEscola != null)
                IndexChangedUnidadeEscola();
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Retorna uma flag indicando se a escola informada existe nas opções do combo.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <returns></returns>
        public bool ExisteOpcaoEscola(int esc_id, int uni_id)
        {
            return ddlUnidadeEscola.Items.FindByValue(esc_id + ";" + uni_id) != null;
        }

        /// <summary>
        /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos
        /// conforme a configuração.
        /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
        /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro
        /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
        /// </summary>
        public void Inicializar()
        {
            try
            {
                lblUA.Visible = false;
                ddlUA.Visible = false;

                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa && PermissaoUsuario) ||
                    !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    FiltroEscola = false;

                    cpvUA.Visible = false;
                    CarregaUnidadesEscolas();
                }
                else
                {
                    FiltroEscola = true;
                    EnableEscolas = false;

                    Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    SYS_TipoUnidadeAdministrativa entityTipoUnidadeAdministrativa = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(entityTipoUnidadeAdministrativa);

                    LabelUA = String.IsNullOrEmpty(entityTipoUnidadeAdministrativa.tua_nome) ? "Unidade Administrativa" : entityTipoUnidadeAdministrativa.tua_nome;

                    CarregaUnidadesAdministrativasSuperior(tua_id);

                    lblUA.Visible = true;
                    ddlUA.Visible = true;
                    cpvUA.Visible = ObrigatorioUA;
                }

                if ((ObrigatorioUA) && (!lblUA.Text.Contains("*")))
                {
                    lblUA.Text += " *";
                }

                if ((ObrigatorioEscola) && (!lblEscola.Text.Contains("*")))
                {
                    lblEscola.Text += " *";
                }

                if (!ObrigatorioUA)
                {
                    lblUA.Text = lblUA.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblUA.Text = lblUA.Text.Replace(" *", "");
                }

                if (!ObrigatorioEscola)
                {
                    lblEscola.Text = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblEscola.Text = lblEscola.Text.Replace(" *", "");
                }

                cpvEscola.Visible = ObrigatorioEscola;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Carrega o combo de escola a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        public void InicializarVisaoAluno(Int64 alu_id, Guid ent_id)
        {
            try
            {
                lblUA.Visible = false;
                ddlUA.Visible = false;

                FiltroEscola = false;

                cpvUA.Visible = false;

                if (!alu_id.Equals(0) && !ent_id.Equals(Guid.Empty))
                    CarregaUnidadesEscolasVisaoAluno(alu_id, ent_id);

                if ((ObrigatorioEscola) && (!lblEscola.Text.Contains("*")))
                {
                    lblEscola.Text += " *";
                }

                if (!ObrigatorioEscola)
                {
                    lblEscola.Text = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblEscola.Text = lblEscola.Text.Replace(" *", "");
                }

                cpvEscola.Visible = ObrigatorioEscola;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Carrega o combo de escola a partir do código do responsável e entidade. Usado na visão individual
        /// Utilizado nas telas: Mensagens.
        /// </summary>
        /// <param name="pes_id">Código da pessoa do responsável</param>
        /// <param name="ent_id">Código da entidade</param>
        public void InicializarVisaoResponsavel(Guid pes_id, Guid ent_id)
        {
            try
            {
                lblUA.Visible = false;
                ddlUA.Visible = false;

                FiltroEscola = false;

                cpvUA.Visible = false;

                if (!pes_id.Equals(Guid.Empty) && !ent_id.Equals(Guid.Empty))
                    CarregaUnidadesEscolasVisaoResponsavel(pes_id, ent_id);

                if ((ObrigatorioEscola) && (!lblEscola.Text.Contains("*")))
                {
                    lblEscola.Text += " *";
                }

                if (!ObrigatorioEscola)
                {
                    lblEscola.Text = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblEscola.Text = lblEscola.Text.Replace(" *", "");
                }

                cpvEscola.Visible = ObrigatorioEscola;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Carrega o combo de escola a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        public void InicializarVisaoIndividual(Int64 doc_id, Guid ent_id)
        {
            InicializarVisaoIndividual(doc_id, ent_id, 0);
        }

        /// <summary>
        /// Carrega o combo de escola a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <param name="vinculoColaboradorCargo">1 -- Busca as escolas do docente pelo vinculo de colaborador cargo
        ///                                       0 -- Busca as escolas pelas atribuições de turma docente do docente
        ///                                       2 -- Busca escolas do docente com vigência na escola
        ///                                       3 -- Busca as escolas pelas atribuições de turma docente do docente com situaçao diferente 3</param>
        public void InicializarVisaoIndividual(Int64 doc_id, Guid ent_id, Byte vinculoColaboradorCargo)
        {
            try
            {
                lblUA.Visible = false;
                ddlUA.Visible = false;

                FiltroEscola = false;

                cpvUA.Visible = false;

                if (!doc_id.Equals(0) && !ent_id.Equals(Guid.Empty))
                    CarregaUnidadesEscolasVisaoIndividual(doc_id, ent_id, vinculoColaboradorCargo);

                if ((ObrigatorioEscola) && (!lblEscola.Text.Contains("*")))
                {
                    lblEscola.Text += " *";
                }

                if (!ObrigatorioEscola)
                {
                    lblEscola.Text = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblEscola.Text = lblEscola.Text.Replace(" *", "");
                }

                cpvEscola.Visible = ObrigatorioEscola;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Carrega o combo de escola a partir do cargo do colaborador. Usado na visão individual.
        /// Utilizado na tela: Atribuição do docente.
        /// </summary>
        /// <param name="col_id">Id do colaborador.</param>
        /// <param name="crg_id">Id do cargo.</param> 
        /// <param name="coc_id">Id do cargo do colaborador.</param>
        public void InicializarVisaoIndividualPorCargo(long col_id, int crg_id, int coc_id)
        {
            try
            {
                lblUA.Visible = false;
                ddlUA.Visible = false;

                FiltroEscola = false;

                cpvUA.Visible = false;

                if (!col_id.Equals(0) && !crg_id.Equals(0) && !coc_id.Equals(0))
                {
                    CarregaEscolasPorColaboradorCargoComHierarquia(col_id, crg_id, coc_id);
                }

                if ((ObrigatorioEscola) && (!lblEscola.Text.Contains("*")))
                {
                    lblEscola.Text += " *";
                }

                if (!ObrigatorioEscola)
                {
                    lblEscola.Text = lblEscola.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    lblEscola.Text = lblEscola.Text.Replace(" *", "");
                }

                cpvEscola.Visible = ObrigatorioEscola;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos
        /// conforme a configuração.
        /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
        /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro
        /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
        /// Não verifica permissão do usuário para carregar as unidades administrativas.
        /// </summary>
        public void InicializarTodos()
        {
            PermissaoUsuario = false;
            Inicializar();
        }

        public void InicializarDocente()
        {
        }

        /// <summary>
        /// Verifica o método de carregar escolas automaticamente, e dispara o evento, quando configurado.
        /// Verifica o delegate, e dispara também, quando configurado.
        /// </summary>
        protected void ddlUAIndexChanged()
        {
            if (ddlUA.AutoPostBack)
            {
                CarregaUnidadesEscolaresPorUASuperior(Uad_ID);
                if (Uad_ID == Guid.Empty)
                {
                    EnableEscolas = false;
                    SelectedValueEscolas = new[] { -1, -1 };//Nescessario quando existe somente um registro de escola.
                }
                else
                {
                    EnableEscolas = true;
                    FocoEscolas = true;
                }
            }

            if (IndexChangedUA != null)
                IndexChangedUA();

            if (IndexChangedUA_Sender != null)
                IndexChangedUA_Sender(this);
        }

        /// <summary>
        /// Carrega unidades escolares.
        /// </summary>
        private void CarregaUnidadesEscolas()
        {
            List<sComboUAEscola> dt;

            if (PermissaoUsuario)
            {
                if (FiltroEscolasControladas.HasValue)
                {
                    dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladas(
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                            FiltroEscolasControladas.Value,
                            ApplicationWEB.AppMinutosCacheLongo);
                }
                else
                {
                    dt = ESC_UnidadeEscolaBO.GetSelect_Cache(
                            0,
                            0,
                            0,
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                            ApplicationWEB.AppMinutosCacheLongo);
                }
            }
            else
            {
                dt = ESC_UnidadeEscolaBO.GetSelectPermissaoTotal_Cache(__SessionWEB.__UsuarioWEB.Usuario.ent_id, false, null, ApplicationWEB.AppMinutosCacheLongo);
            }

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            // Configura combo de escolas, caso possui apenas uma opção
            if (QuantidadeItemsComboEscolas == 2)
            {
                ddlUnidadeEscola.SelectedValue = ddlUnidadeEscola.Items[1].Value;
                if (IndexChangedUnidadeEscola != null)
                    IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Carrega unidades escolares a partir do código docente e entidade
        /// Utilizado nas telas: Mensagens. 
        /// </summary>
        /// <param name="alu_id">Código do aluno</param>
        /// <param name="ent_id">Código da entidade</param> 
        private void CarregaUnidadesEscolasVisaoAluno(Int64 alu_id, Guid ent_id)
        {
            List<sComboUAEscola> dt = ESC_UnidadeEscolaBO.GetSelectEscolas_VisaoAluno(alu_id, ent_id, ApplicationWEB.AppMinutosCacheLongo);

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            // Configura combo de escolas, caso possui apenas uma opção
            SelecionaPrimeiroItemEscola();
        }

        /// <summary>
        /// Carrega unidades escolares a partir do código responsavel e entidade
        /// Utilizado nas telas: Mensagens. 
        /// </summary>
        /// <param name="pes_id">Código da pessoa do responsável</param>
        /// <param name="ent_id">Código da entidade</param> 
        private void CarregaUnidadesEscolasVisaoResponsavel(Guid pes_id, Guid ent_id)
        {
            List<sComboUAEscola> dt = ESC_UnidadeEscolaBO.GetSelectEscolas_VisaoResponsavel(pes_id, ent_id, ApplicationWEB.AppMinutosCacheLongo);

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            // Configura combo de escolas, caso possui apenas uma opção
            SelecionaPrimeiroItemEscola();
        }

        /// <summary>
        /// Carrega unidades escolares a partir do código docente e entidade
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências. 
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param> 
        /// <param name="vinculoColaboradorCargo">0 - Busca as escolas pelas atribuições de turma docente do docente
        ///                                       1 - Busca as escolas do docente pelo vinculo de colaborador cargo
        ///                                       2 - Busca escolas do docente com vigência na escola</param>
        private void CarregaUnidadesEscolasVisaoIndividual(Int64 doc_id, Guid ent_id, Byte vinculoColaboradorCargo)
        {
            List<sComboUAEscola> dt = ESC_UnidadeEscolaBO.GetSelectEscolas_VisaoIndividual(doc_id, ent_id, vinculoColaboradorCargo, ApplicationWEB.AppMinutosCacheLongo);

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            // Configura combo de escolas, caso possui apenas uma opção
            SelecionaPrimeiroItemEscola();
        }

        /// <summary>
        /// Carrega o combo de escola a partir do cargo do colaborador. Usado na visão individual.
        /// Utilizado na tela: Atribuição do docente.
        /// </summary>
        /// <param name="col_id">Id do colaborador.</param>
        /// <param name="crg_id">Id do cargo.</param> 
        /// <param name="coc_id">Id do cargo do colaborador.</param>
        public void CarregaEscolasPorColaboradorCargoComHierarquia(long col_id, int crg_id, int coc_id)
        {
            List<sComboUAEscola> dt = ESC_EscolaBO.SelecionaPorColaboradorCargoComHierarquia(col_id, crg_id, coc_id, ApplicationWEB.AppMinutosCacheLongo);

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            // Configura combo de escolas, caso possui apenas uma opção
            SelecionaPrimeiroItemEscola();
        }

        /// <summary>
        /// Carrega as unidades de escola de acordo com a UA selecionada no combo de UA.
        /// </summary>
        public void CarregaEscolaPorUASuperiorSelecionada()
        {
            CarregaUnidadesEscolaresPorUASuperior(Uad_ID);
        }

        /// <summary>
        /// Carrega unidades escolares apartir
        /// do tipo de unidade administrativa superior passada por parâmetro.
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        public void CarregaUnidadesEscolaresPorUASuperior(Guid uad_idSuperior)
        {
            List<sComboUAEscola> dt;
            byte uni_Situacao = MostraApenasAtivas ? (byte)1 : (byte)0;

            // Verifica se obrigatório filtrar por permissão de usuário.
            if (PermissaoUsuario)
            {
                // Verifica se configurado para filtrar por escolas controladas.
                if (FiltroEscolasControladas.HasValue)
                {
                    dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperior(
                        uad_idSuperior,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        uni_Situacao,
                        FiltroEscolasControladas.Value,
                        ApplicationWEB.AppMinutosCacheLongo);
                }
                else
                {
                    dt = ESC_UnidadeEscolaBO.GetSelectByUASuperiorSituacao(
                        uad_idSuperior,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        uni_Situacao,
                        ApplicationWEB.AppMinutosCacheLongo);
                }
            }
            else
            {
                // Verifica se configurado para filtrar por escolas controladas.
                if (FiltroEscolasControladas.HasValue)
                {
                    dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperiorPermissaoTotal(
                        uad_idSuperior,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        true,
                        FiltroEscolasControladas.Value, uni_Situacao,
                        ApplicationWEB.AppMinutosCacheLongo);
                }
                else
                {
                    dt = ESC_UnidadeEscolaBO.GetSelectByUASuperiorPermissaoTotal(
                        uad_idSuperior,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        true, uni_Situacao,
                        ApplicationWEB.AppMinutosCacheLongo);
                }
            }

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.DataTextField = "esc_uni_nome";

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            SelecionaPrimeiroItemEscola();
        }


        /// <summary>
        /// Carrega unidades escolares sem acesso
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tpc_id">Periodo</param>
        public void CarregaUnidadesEscolaresSemAcesso(Guid uad_idSuperior, int cal_id, int tpc_id)
        {
            List<sComboUAEscola> dt;
            byte uni_Situacao = MostraApenasAtivas ? (byte)1 : (byte)0;

            // Verifica se obrigatório filtrar por permissão de usuário.
            if (PermissaoUsuario)
            {
                dt = ESC_UnidadeEscolaBO.GetSelectBySemAcesso(
                    uad_idSuperior,
                    cal_id,
                    tpc_id,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                    uni_Situacao,
                    ApplicationWEB.AppMinutosCacheLongo);
            }
            else
            {
                dt = ESC_UnidadeEscolaBO.GetSelectBySemAcessoPermissaoTotal(
                    uad_idSuperior,
                    cal_id,
                    tpc_id,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    true, uni_Situacao,
                    ApplicationWEB.AppMinutosCacheLongo);
            }

            if (VS_EscolasPapeis.Any())
                dt = dt.Where(p => VS_EscolasPapeis.Any(e => e == p.esc_id)).ToList();

            ddlUnidadeEscola.DataTextField = "esc_uni_nome";

            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.DataSource = dt;
            MostrarMessageSelecioneEscola = true;
            ddlUnidadeEscola.DataBind();

            SelecionaPrimeiroItemEscola();
        }

        /// <summary>
        /// Seleciona o primeiro item no combo de escolas, caso possua apenas uma opção.
        /// </summary>
        public void SelecionaPrimeiroItemEscola()
        {
            if (QuantidadeItemsComboEscolas == 2)
            {
                ddlUnidadeEscola.SelectedValue = ddlUnidadeEscola.Items[1].Value;
                if (IndexChangedUnidadeEscola != null)
                    IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Carrega unidades administrativas apartir
        /// do tipo de unidade administrativa passada por parâmetro.
        /// </summary>
        /// <param name="tua_id">Id do tipo de unidade administrativa</param>
        private void CarregaUnidadesAdministrativasSuperior(Guid tua_id)
        {
            List<sComboUAEscola> dt;

            // Verifica se obrigatório filtrar por permissãod e usuário.
            if (PermissaoUsuario)
            {
                dt = ESC_UnidadeEscolaBO.GetSelectBy_Pesquisa_PermissaoUsuario_Cache(
                        tua_id,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        0,
                        Guid.Empty,
                        ApplicationWEB.AppMinutosCacheLongo);
            }
            else
            {
                dt = ESC_UnidadeEscolaBO.GetSelectBy_PesquisaTodos_Cache(
                        tua_id,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        ApplicationWEB.AppMinutosCacheLongo);
            }

            ddlUA.Items.Clear();
            ddlUA.DataSource = dt;
            MostrarMessageSelecioneUA = true;
            ddlUA.DataBind();

            SelecionaPrimeiroItemUA();
        }

        /// <summary>
        /// Seleciona o primeiro item no combo de UA, caso possua somente um.
        /// </summary>
        public void SelecionaPrimeiroItemUA()
        {
            if (QuantidadeItemsComboUAs == 2)
            {
                ddlUA.SelectedValue = ddlUA.Items[1].Value;

                // Dispara os eventos do indexChanged configurados.
                ddlUAIndexChanged();
            }
        }

        /// <summary>
        /// Seta o foco no combo de unidade administrativa.
        /// </summary>
        public void FocusUA()
        {
            ddlUA.Focus();
        }

        /// <summary>
        /// Seta o foco no combo de escolas.
        /// </summary>
        public void FocusEscolas()
        {
            ddlUnidadeEscola.Focus();
        }

        #endregion Métodos
    }
}