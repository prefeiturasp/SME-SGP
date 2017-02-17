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

namespace GestaoEscolar.Academico.OrientacaoCurricular
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// ID do curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// ID do currículo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// ID do período.
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        private int VS_tds_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tds_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tds_id"] = value;
            }
        }

        /// <summary>
        /// ID do calendário.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        private string VS_cur_nome
        {
            get
            {
                return (ViewState["VS_cur_nome"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_cur_nome"] = value;
            }
        }

        private string VS_crp_descricao
        {
            get
            {
                return (ViewState["VS_crp_descricao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_crp_descricao"] = value;
            }
        }

        private string VS_tds_nome
        {
            get
            {
                return (ViewState["VS_tds_nome"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_tds_nome"] = value;
            }
        }

        private string VS_cal_descricao
        {
            get
            {
                return (ViewState["VS_cal_descricao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_cal_descricao"] = value;
            }
        }

        /// <summary>
        /// ID do calendário que está sendo copiado para uma nova orientação curricular.
        /// </summary>
        private int Cal_id_Copia
        {
            get
            {
                return UCComboCalendario2.Valor;
            }
        }

        /// <summary>
        /// Lista de conteúdos cadastrados
        /// </summary>
        private List<ORC_Conteudo_Cadastro> listaConteudoCadastro;

        /// <summary>
        /// Índice do item que está sendo editado.
        /// </summary>
        private int VS_EditItem
        {
            get
            {
                if (ViewState["VS_EditItem"] == null)
                    return -1;

                return (int) ViewState["VS_EditItem"];
            }
            set
            {
                ViewState["VS_EditItem"] = value;
            }
        }

        protected bool editandoObjetivo
        {
            get
            {
                return VS_EditItem == rptObjetivos.Items.Count;
            }
        }

        protected bool editandoItemObjetivo;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega repeaters na tela.
        /// </summary>
        private void CarregarObjetivosConteudos()
        {
            try
            {
                DataTable dtObjetivos = ORC_ObjetivoBO.SelecionaPor_Curriculo_Disciplina(VS_cur_id, VS_crr_id, VS_crp_id, VS_tds_id, VS_cal_id);

                rptObjetivos.DataSource = dtObjetivos;
                rptObjetivos.DataBind();

                // Se não houver objetivo cadastrado, mostra a mensagem e esconde o repeater.
                if (dtObjetivos.Rows.Count == 0)
                {
                    rptObjetivos.Visible = false;
                    lblMsgRepeater.Visible = true;
                    lblMsgRepeater.Text = "Não foram encontrados objetivos no(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " selecionada.";
                    btnCopiar.Visible = false;
                }
                else
                {
                    rptObjetivos.Visible = true;
                    lblMsgRepeater.Visible = false;
                    btnCopiar.Visible = true;
                }


                // Mostra o label com os dados selecionados.
                lblInformacao.Text = "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_cur_nome + "<br/>";
                lblInformacao.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + VS_crp_descricao + "<br/>";
                lblInformacao.Text += "<b>" + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + ": </b>" + VS_tds_nome + "<br/>";
                lblInformacao.Text += "<b>Calendário escolar: </b>" + VS_cal_descricao + "<br/>";
                lblInformacao.Visible = true;
                divLimparPesquisa.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva os dados da linha do objetivo.
        /// </summary>
        /// <param name="item">Item que contém os dados do objetivo.</param>
        private void SalvarObjetivo(RepeaterItem item)
        {
            try
            {
                ORC_Objetivo_Cadastro itemCadastro = RetornaObjetivoCadastro(item);

                // Valida se já existe um objetivo cadastrado com a mesma descrição no mesmo curso, período e disciplina.
                if (ORC_ObjetivoBO.VerificaNomeExistente(itemCadastro.entObjetivo))
                    throw new ValidationException("Já existe um objetivo cadastrado com a descrição " + itemCadastro.entObjetivo.obj_descricao + ".");

                ORC_ObjetivoBO.Save(itemCadastro);

                VS_EditItem = -1;

                // Se não for uma ação da cópia de orientação curricular - Recarrega repeater.
                if(Cal_id_Copia <= 0)
                    CarregarObjetivosConteudos();

                if (itemCadastro.entObjetivo.IsNew)
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "obj_id: " + itemCadastro.entObjetivo.obj_id);
                else
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "obj_id: " + itemCadastro.entObjetivo.obj_id);

                lblMensagem.Text = UtilBO.GetErroMessage("Objetivo da orientação curricular salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

                btnAdicionarObjetivo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                btnAdicionarObjetivoCima.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar objetivo.", UtilBO.TipoMensagem.Erro);
            }
        }

        #region Retorna estruturas de cadastro

        /// <summary>
        /// Retorna um list da estrutura de cadastro, com os dados que estão nos itens do repeater.
        /// </summary>
        /// <returns></returns>
        private List<ORC_Objetivo_Cadastro> RetornaDadosCadastrados()
        {
            List<ORC_Objetivo_Cadastro> lista = new List<ORC_Objetivo_Cadastro>();

            foreach (RepeaterItem item in rptObjetivos.Items)
            {
                lista.Add(RetornaObjetivoCadastro(item));
            }

            return lista;
        }

        /// <summary>
        /// Retorna a estrutura de cadastro de objetivos.
        /// </summary>
        /// <param name="item">Item do repeater com os dados a salvar.</param>
        /// <returns></returns>
        private ORC_Objetivo_Cadastro RetornaObjetivoCadastro(RepeaterItem item)
        {
            Literal litObj_id = (Literal)item.FindControl("litObj_id");
            TextBox txtObjetivo = (TextBox)item.FindControl("txtObjetivo");

            int obj_id = 0;
            if(Cal_id_Copia <= 0)
                obj_id = Convert.ToInt32(litObj_id.Text);

            ORC_Objetivo entObjetivo = new ORC_Objetivo
            {
                cur_id = VS_cur_id
                ,
                crr_id = VS_crr_id
                ,
                crp_id = VS_crp_id
                ,
                tds_id = VS_tds_id
                ,
                cal_id = (Cal_id_Copia > 0 ? Cal_id_Copia : VS_cal_id)
                ,
                obj_id = obj_id
                ,
                obj_situacao = (byte)ORC_ObjetivoSituacao.Ativo
                ,
                IsNew = obj_id <= 0
                ,
                obj_descricao = txtObjetivo.Text
            };

            return new ORC_Objetivo_Cadastro
            {
                entObjetivo = entObjetivo
                ,
                listConteudos = RetornaConteudos(item)
            };
        }

        /// <summary>
        /// Retorna os conteúdos cadastrados dentro da linha do objetivo.
        /// </summary>
        /// <param name="container">Linha do objetivo</param>
        /// <returns></returns>
        private List<ORC_Conteudo_Cadastro> RetornaConteudos(RepeaterItem container)
        {
            List<ORC_Conteudo_Cadastro> lista = new List<ORC_Conteudo_Cadastro>();
            Repeater rptConteudos = (Repeater)container.FindControl("rptConteudos");

            foreach (RepeaterItem item in rptConteudos.Items)
            {
                
                ORC_Conteudo_Cadastro entConteudos = RetornaConteudoCadastro(item);
                if ((entConteudos.listItensConteudo.Count > 0) ||
                   (entConteudos.listHabilidades.Count > 0) ||
                   (entConteudos.listPeriodos.Count > 0))
                {
                    lista.Add(entConteudos);
                }
            }

            return lista;
        }

        /// <summary>
        /// Retorna a estrutura de cadastro de conteúdo, com os dados do item de repeater passado por 
        /// parâmetro.
        /// </summary>
        /// <param name="item">Item</param>        
        /// <returns></returns>
        private ORC_Conteudo_Cadastro RetornaConteudoCadastro(RepeaterItem item)
        {
            Literal litCtd_id = (Literal)item.FindControl("litCtd_id");
            Literal litObj_id = (Literal)item.FindControl("litObj_id");
            int ctd_id = 0;
            int obj_id = 0;

            // se não vier da ação de copiar orientação curricular, guarda os ids para alterar caso seja uma alteração.
            if (Cal_id_Copia <= 0)
            {
                obj_id = Convert.ToInt32(litObj_id.Text);
                ctd_id = Convert.ToInt32(litCtd_id.Text);
            }

            ORC_Conteudo entConteudo = new ORC_Conteudo
            {
                obj_id = obj_id
                ,
                ctd_id = ctd_id
            };

            List<ORC_ConteudoItem> listaItens = RetornaItensConteudo(item, obj_id, ctd_id);            
            List<ORC_Habilidades> listaHabilidades = RetornaHabilidades(item, obj_id, ctd_id);
            List<int> listaPeriodos = RetornaPeriodos(item);

            return new ORC_Conteudo_Cadastro
            {
                entConteudo = entConteudo
                ,
                listItensConteudo = listaItens
                ,
                listHabilidades = listaHabilidades
                ,
                listPeriodos = listaPeriodos
            };
        }

        /// <summary>
        /// Retorna os valores cadastrados dentro do repeater de itens de conteúdo.
        /// </summary>
        /// <param name="container">Linha em que está o repeater</param>
        /// <param name="obj_id">Id do objetivo da linha atual</param>
        /// <param name="ctd_id">Id do conteúdo da linha atual</param>
        /// <returns></returns>
        private List<ORC_ConteudoItem> RetornaItensConteudo(RepeaterItem container, int obj_id, int ctd_id)
        {
            List<ORC_ConteudoItem> lista = new List<ORC_ConteudoItem>();
            Repeater rptConteudoItem = (Repeater)container.FindControl("rptConteudoItem");

            foreach (RepeaterItem item in rptConteudoItem.Items)
            {                
                TextBox txtConteudo = (TextBox)item.FindControl("txtDescricao");
                if (!string.IsNullOrEmpty(txtConteudo.Text))
                {

                    Literal litCti_id = (Literal)item.FindControl("litCti_id");
                    int cti_id = 0;

                    // se não vier da ação de copiar orientação curricular, guarda os ids para alterar caso seja uma alteração.
                    if (Cal_id_Copia <= 0)
                    {
                        cti_id = Convert.ToInt32(litCti_id.Text);
                    }

                    ORC_ConteudoItem entConteudo = new ORC_ConteudoItem
                    {
                        obj_id = obj_id
                        ,
                        ctd_id = ctd_id
                        ,
                        cti_id = cti_id
                        ,
                        cti_descricao = txtConteudo.Text
                        ,
                        cti_situacao = (byte)ORC_ConteudoItemSituacao.Ativo
                        ,
                        IsNew = cti_id <= 0
                    };

                    lista.Add(entConteudo);
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Retorna os valores cadastrados dentro do repeater de habilidades.
        /// </summary>
        /// <param name="container">Linha em que está o repeater</param>
        /// <param name="obj_id">Id do objetivo da linha atual</param>
        /// <param name="ctd_id">Id do conteúdo da linha atual</param>
        /// <returns></returns>
        private List<ORC_Habilidades> RetornaHabilidades(RepeaterItem container, int obj_id, int ctd_id)
        {
            List<ORC_Habilidades> lista = new List<ORC_Habilidades>();
            Repeater rptHabilidades = (Repeater)container.FindControl("rptHabilidades");

            foreach (RepeaterItem item in rptHabilidades.Items)
            {
                TextBox txtHabilidade = (TextBox)item.FindControl("txtDescricao");
                if (!string.IsNullOrEmpty(txtHabilidade.Text))
                {

                    Literal litHbl_id = (Literal)item.FindControl("litHbl_id");
                    int hbl_id = 0;

                    // se não vier da ação de copiar orientação curricular, guarda os ids para alterar caso seja uma alteração.
                    if (Cal_id_Copia <= 0)
                    {
                        hbl_id = Convert.ToInt32(litHbl_id.Text);
                    }

                    ORC_Habilidades entConteudo = new ORC_Habilidades
                    {
                        obj_id = obj_id
                        ,
                        ctd_id = ctd_id
                        ,
                        hbl_id = hbl_id
                        ,
                        hbl_descricao = txtHabilidade.Text
                        ,
                        hbl_situacao = (byte)ORC_HabilidadesSituacao.Ativo
                        ,
                        IsNew = hbl_id <= 0
                    };

                    lista.Add(entConteudo);
                }
            }

            return lista;
        }

        /// <summary>
        /// Retorna os valores cadastrados dentro do repeater de tipos de períodos do calendário.
        /// </summary>
        /// <param name="container">Linha em que está o repeater</param>
        /// <returns></returns>
        private List<int> RetornaPeriodos(RepeaterItem container)
        {
            List<int> lista = new List<int>();
            Repeater rptTiposPeriodo = (Repeater)container.FindControl("rptTiposPeriodo");

            foreach (RepeaterItem item in rptTiposPeriodo.Items)
            {
                Literal lbltpc_id = (Literal)item.FindControl("lbltpc_id");

                CheckBox chkPeriodo = (CheckBox)item.FindControl("chkPeriodo");

                if (chkPeriodo.Checked)
                {
                    int tpc_id = Convert.ToInt32(lbltpc_id.Text);
                    lista.Add(tpc_id);
                }
            }

            return lista;
        }

        #endregion

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            pnlPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

            btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            btnAdicionarObjetivoCima.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            btnAdicionarObjetivo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }

        /// <summary>
        /// Retorna o RepeaterItem do controle informado, usando o NamingContainer.
        /// </summary>
        /// <param name="sender">Controle</param>
        /// <returns></returns>
        private RepeaterItem RetornaRepeaterItemBotao(object sender)
        {
            Control btnSender = (Control)sender;
            return (RepeaterItem)btnSender.NamingContainer;
        }

        /// <summary>
        /// Retorna o Repeater do controle informado, usando o NamingContainer.
        /// </summary>
        /// <param name="sender">Controle</param>
        /// <returns></returns>
        private Repeater RetornaRepeaterBotao(object sender)
        {
            RepeaterItem item = RetornaRepeaterItemBotao(sender);

            return (Repeater)item.NamingContainer;
        }

        /// <summary>
        /// Retorna uma estrutura de cadastro de conteúdo nova.
        /// </summary>
        /// <param name="obj_id">ID do objetivo a que pertence o conteúdo</param>
        /// <param name="ctd_id"></param>
        /// <returns></returns>
        private ORC_Conteudo_Cadastro CriaNovoConteudo(int obj_id, int ctd_id)
        {
            List<ORC_ConteudoItem> listItem = new List<ORC_ConteudoItem> { new ORC_ConteudoItem() };

            List<ORC_Habilidades> listHabilidades = new List<ORC_Habilidades> { new ORC_Habilidades() };

            return new ORC_Conteudo_Cadastro
            {
                entConteudo = new ORC_Conteudo { obj_id = obj_id, ctd_id = ctd_id }
                ,
                listHabilidades = listHabilidades
                ,
                listItensConteudo = listItem
            };
        }

        /// <summary>
        /// Retorna a estrutura de cadastro de conteúdo, buscando o container do botão (parâmetro "sender").
        /// Chamado nos eventos dos botões de adicionar Itens de conteúdo e Habilidades.
        /// </summary>
        /// <param name="sender">Botão que chamou o evento</param>
        /// <param name="rptItem">Out - repeater que contém o botão</param>
        /// <returns></returns>
        private ORC_Conteudo_Cadastro RetornaConteudo(object sender, out Repeater rptItem)
        {
            // Retorna repeater através do NamingContainer.
            rptItem = RetornaRepeaterBotao(sender);

            // Item do repeater de fora (Conteúdo).
            RepeaterItem itemConteudo = (RepeaterItem)rptItem.NamingContainer;

            // Retorna o conteúdo cadastrado.
            return RetornaConteudoCadastro(itemConteudo);
        }
        
        /// <summary>
        /// Dá o dataBind no repeater, e seta a propriedade Visible no botão de adicionar conteúdo.
        /// </summary>
        /// <param name="rptConteudos">Repeater de conteúdos</param>
        /// <param name="itemObjetivo">Item do repeater de objetivo</param>
        private void AtualizaRepeaterConteudos(Repeater rptConteudos, RepeaterItem itemObjetivo)
        {
            rptConteudos.DataBind();

            if (rptConteudos.Items.Count > 0)
            {
                // Seta visível o último botão de adicionar conteúdo.
                Button btnAdicionarConteudo =
                    (Button) rptConteudos.Items[rptConteudos.Items.Count - 1].FindControl("btnAdicionarConteudo");

                btnAdicionarConteudo.Visible = editandoItemObjetivo;
            }

            // Se não existir nenhum conteúdo, setar o botão que fora do repeater, no item do objetivo.
            HtmlTableCell tdAdicionarConteudo = (HtmlTableCell) itemObjetivo.FindControl("tdAdicionarConteudo");

            if (tdAdicionarConteudo != null)
                tdAdicionarConteudo.Visible = (rptConteudos.Items.Count <= 0 && editandoItemObjetivo);
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
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroOrientacaoCurricular.js"));
            }

            string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnLimparPesquisa.ClientID), String.Format("ATENÇÃO: Todas as informações não salvas serão perdidas. Deseja continuar?"));
            Page.ClientScript.RegisterStartupScript(GetType(), btnLimparPesquisa.ClientID, script, true);

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
                        VS_cur_id = PreviousPage.Edit_Cur_id;
                        VS_crr_id = PreviousPage.Edit_Crr_id;
                        VS_crp_id = PreviousPage.Edit_Crp_id;
                        VS_tds_id = PreviousPage.Edit_Tds_id;
                        VS_cal_id = PreviousPage.Edit_Cal_id;
                        VS_cur_nome = PreviousPage.Edit_Curso;
                        VS_crp_descricao = PreviousPage.Edit_Grupamento;
                        VS_tds_nome = PreviousPage.Edit_Disciplina;
                        VS_cal_descricao = PreviousPage.Edit_Calendario;
                    }

                    CarregarObjetivosConteudos();

                    // Carrega os combos.
                    UCComboCalendario2.CarregarCalendarioAnual();



                    pnlResultados.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptObjetivos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Carregar header com nomes tipos de período calendário.
                Repeater rptTiposPeriodo = (Repeater)e.Item.FindControl("rptTiposPeriodo");
                rptTiposPeriodo.DataSource = ORC_ConteudoTipoPeriodoCalendarioBO.SelecionaPor_Conteudo(-1, -1);
                rptTiposPeriodo.DataBind();
            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Item))
            {
                // Seta flag que indica se o item do Objetivo é o que está sendo editado.
                editandoItemObjetivo = VS_EditItem == e.Item.ItemIndex;

                int obj_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "obj_id"));
                
                // Carregar conteúdos do objetivo.
                DataTable dt = ORC_ConteudoBO.SelecionaPor_Objetivo(obj_id);

                HtmlTableCell tdObj = (HtmlTableCell) e.Item.FindControl("tdObj");
                tdObj.RowSpan = dt.Rows.Count;

                Repeater rptConteudos = (Repeater) e.Item.FindControl("rptConteudos");
                rptConteudos.DataSource = dt;
                AtualizaRepeaterConteudos(rptConteudos, e.Item);

                LinkButton Objetivo = (LinkButton)e.Item.FindControl("Objetivo");
                if (Objetivo != null)
                {
                    Objetivo.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                // Altera a visibilidade do botão de excluir conteúdo.
                ImageButton btnExcluirObjetivo = (ImageButton)e.Item.FindControl("btnExcluirObjetivo");
                if (btnExcluirObjetivo != null)
                {
                    btnExcluirObjetivo.Visible = !editandoItemObjetivo && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluirObjetivo.CommandArgument = obj_id.ToString();
                }
            }
        }

        protected void rptConteudos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Item))
            {
                int obj_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "obj_id"));
                int ctd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ctd_id"));

                // Carregar tipos de período calendário.
                Repeater rptTiposPeriodo = (Repeater)e.Item.FindControl("rptTiposPeriodo");
                rptTiposPeriodo.DataSource = ORC_ConteudoTipoPeriodoCalendarioBO.SelecionaPor_Conteudo(obj_id, ctd_id);
                rptTiposPeriodo.DataBind();

                // Carregar itens de conteúdo e habilidades.
                Repeater rptConteudoItem = (Repeater) e.Item.FindControl("rptConteudoItem");
                Repeater rptHabilidades = (Repeater) e.Item.FindControl("rptHabilidades");

                if (listaConteudoCadastro == null)
                {
                    // Se a lista da tela não foi carregada, carrega os dados do banco.
                    rptConteudoItem.DataSource = ORC_ConteudoItemBO.SelecionaPor_Conteudo(obj_id, ctd_id);
                    rptHabilidades.DataSource = ORC_HabilidadesBO.SelecionaPor_Conteudo(obj_id, ctd_id);
                }
                else
                {
                    // Se a lista foi carregada, significa que foi adicionado um item dinamicamente, carregar
                    // dados a partir da lista.
                    var x = from ORC_Conteudo_Cadastro item in listaConteudoCadastro
                            where
                                item.entConteudo.ctd_id == ctd_id &&
                                item.entConteudo.obj_id == obj_id
                            select item;

                    if (x.Count() > 0)
                    {
                        rptConteudoItem.DataSource = x.First().listItensConteudo;
                        rptHabilidades.DataSource = x.First().listHabilidades;
                    }
                }

                rptConteudoItem.DataBind();
                rptHabilidades.DataBind();

                // Altera a visibilidade do botão de excluir conteúdo.
                ImageButton btnExcluirConteudo = (ImageButton)e.Item.FindControl("btnExcluirConteudo");
                if (btnExcluirConteudo != null)
                {
                    btnExcluirConteudo.Visible = !editandoItemObjetivo && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluirConteudo.CommandArgument = obj_id + ";" + ctd_id;
                }
            }
        }

        protected void rptTiposPeriodo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Item))
            {               
                // Altera a visibilidade do checkbox de tipo período.
                CheckBox chkPeriodo = (CheckBox)e.Item.FindControl("chkPeriodo");
                if (chkPeriodo != null)
                {
                    if (chkPeriodo.Enabled)
                        chkPeriodo.Enabled = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }
        
        protected void btnSalvarObjetivo_Click(object sender, EventArgs e)
        {
            if (Cal_id_Copia > 0)
            {
                foreach (RepeaterItem item in rptObjetivos.Items)
                {
                    SalvarObjetivo(item);
                }
            }
            else
            {
                RepeaterItem item = RetornaRepeaterItemBotao(sender);
                SalvarObjetivo(item);
            }            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Seta o editItem = vazio, e recarrega os repeaters.
            VS_EditItem = -1;
            CarregarObjetivosConteudos();

            btnAdicionarObjetivo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            btnAdicionarObjetivoCima.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }

        protected void lkbObjetivo_Click(object sender, EventArgs e)
        {
            try
            {
                RepeaterItem item = RetornaRepeaterItemBotao(sender);

                VS_EditItem = item.ItemIndex;

                CarregarObjetivosConteudos();

                btnAdicionarObjetivo.Visible = false;
                btnAdicionarObjetivoCima.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar editar objetivo.",
                                                         UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAdicionarObjetivo_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o repeater não estava escondido.
                if (!rptObjetivos.Visible)
                {
                    rptObjetivos.Visible = true;
                    lblMsgRepeater.Visible = false;
                }

                // Adicionar novo conteúdo como novo item no repeater.
                List<ORC_Objetivo_Cadastro> list = RetornaDadosCadastrados();

                ORC_Objetivo_Cadastro entNovo = new ORC_Objetivo_Cadastro
                {
                    entObjetivo = new ORC_Objetivo()
                };

                list.Add(entNovo);

                var x = from ORC_Objetivo_Cadastro cad in list
                        select cad.entObjetivo;

                VS_EditItem = x.Count() - 1;

                rptObjetivos.DataSource = x;
                rptObjetivos.DataBind();

                btnAdicionarObjetivo.Visible = false;
                btnAdicionarObjetivoCima.Visible = false;                
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar item de conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnExcluirObjetivo_Click(object sender, EventArgs e)
        {
            try
            {
                // Pegar os ID's dos itens que serão excluídos.
                ImageButton btn = (ImageButton)sender;
                string obj_id = btn.CommandArgument.Split(';')[0];

                ORC_Objetivo entity = new ORC_Objetivo
                {
                    obj_id = Convert.ToInt32(string.IsNullOrEmpty(obj_id) ? "0" : obj_id)
                };

                if (entity.obj_id > 0)
                {
                    ORC_ObjetivoBO.Delete(entity);

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "obj_id: " + entity.obj_id);

                    VS_EditItem = -1;

                    // Recarregar repeater.
                    CarregarObjetivosConteudos();

                    lblMensagem.Text = UtilBO.GetErroMessage("Objetivo da orientação curricular excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Não foi possível excluir o objetivo da orientação curricular.", UtilBO.TipoMensagem.Alerta);
                }

            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o objetivo da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAdicionarConteudo_Click(object sender, EventArgs e)
        {
            try
            {
                // Utilizado na validação de item do conteúdo e de habilidade
                Button btn = (Button)sender;
                RepeaterItem rptItemConteudoValidacao = (RepeaterItem)btn.NamingContainer;
                Repeater rptItemValidacao = (Repeater)rptItemConteudoValidacao.FindControl("rptConteudoItem");
                foreach (RepeaterItem item in rptItemValidacao.Items)
                {
                    TextBox txtItem = (TextBox)item.FindControl("txtDescricao");
                    if (string.IsNullOrEmpty(txtItem.Text))
                        throw new ValidationException("Item do conteúdo é obrigatório.");
                }
                Repeater rptHabilidadeValidacao = (Repeater)rptItemConteudoValidacao.FindControl("rptHabilidades");
                foreach (RepeaterItem item in rptHabilidadeValidacao.Items)
                {
                    TextBox txtHabilidade = (TextBox)item.FindControl("txtDescricao");
                    if (string.IsNullOrEmpty(txtHabilidade.Text))
                        throw new ValidationException("Habilidade é obrigatório.");
                }

                // Adicionar item de conteúdo na lista de dados.
                Repeater rptContainer = RetornaRepeaterBotao(sender);
                List<ORC_Conteudo_Cadastro> lista = new List<ORC_Conteudo_Cadastro>();

                // Se o botão está dentro do repeater de objetivos - não tem item no repeater de conteúdo.
                if (!rptContainer.ID.Equals(rptObjetivos.ID))
                {
                    foreach (RepeaterItem item in rptContainer.Items)                                  
                        lista.Add(RetornaConteudoCadastro(item));
                }

                int obj_id = -1;

                if (lista.Count > 0)
                {
                    ORC_Conteudo ent = lista.FindLast(p => p.entConteudo != null).entConteudo;

                    if (ent != null)
                        obj_id = ent.obj_id;
                }

                // Verifica a qtde de conteúdos cadastrados
                var x2 = from ORC_Conteudo_Cadastro cad in lista
                        select cad.entConteudo;

                // Gera um novo id para o conteúdo (utilizado apenas na tela)
                int ctd_id = x2.Count() + 1;

                ORC_Conteudo_Cadastro novoItem = CriaNovoConteudo(obj_id, ctd_id);

                lista.Add(novoItem);

                listaConteudoCadastro = lista;

                var x = from ORC_Conteudo_Cadastro cad in lista
                        select cad.entConteudo;

                // Seta flag que indica se o item do Objetivo é o que está sendo editado.
                editandoItemObjetivo = true;

                Repeater rptConteudos = rptContainer;

                if (rptContainer.ID.Equals(rptObjetivos.ID))
                {
                    RepeaterItem itemObjetivo = RetornaRepeaterItemBotao(sender);
                    rptConteudos = (Repeater) itemObjetivo.FindControl("rptConteudos");
                }

                // Dá o dataBind no repeater, e seta a propriedade Visible no botão de adicionar conteúdo.
                rptConteudos.DataSource = x;
                AtualizaRepeaterConteudos(rptConteudos, (RepeaterItem)rptConteudos.NamingContainer);

                RepeaterItem rptItem = (RepeaterItem)rptConteudos.NamingContainer;

                // Seta flag que indica se o item do Objetivo é o que está sendo editado.
                editandoItemObjetivo = true;

                // Atualiza o rowSpan da coluna de objetivos.
                HtmlTableCell tdObj = (HtmlTableCell)rptItem.FindControl("tdObj");
                tdObj.RowSpan = x.Count() + 1;

                listaConteudoCadastro = null;
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar item de conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnExcluirConteudo_Click(object sender, EventArgs e)
        {
            try
            {                                
                // Pegar os ID's dos itens que serão excluídos.
                ImageButton btn = (ImageButton) sender;
                string obj_id = btn.CommandArgument.Split(';')[0];
                string ctd_id = btn.CommandArgument.Split(';')[1];

                ORC_Conteudo entity = new ORC_Conteudo {
                                                            obj_id = Convert.ToInt32(string.IsNullOrEmpty(obj_id) ? "0" : obj_id)
                                                            ,
                                                            ctd_id = Convert.ToInt32(string.IsNullOrEmpty(ctd_id) ? "0" : ctd_id)
                                                       };

                if (entity.obj_id > 0 && entity.ctd_id > 0)
                {
                    ORC_ConteudoBO.Delete(entity);

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "obj_id: " + entity.obj_id + "; ctd_id: " + entity.ctd_id);

                    VS_EditItem = -1;

                    // Recarregar repeater.
                    CarregarObjetivosConteudos();

                    lblMensagem.Text = UtilBO.GetErroMessage("Conteúdo do ojetivo da orientação curricular excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Não foi possível excluir o conteúdo do objetivo da orientação curricular.", UtilBO.TipoMensagem.Alerta);
                }

            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o conteúdo do objetivo da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoItemConteudo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Utilizado na validação de item do conteúdo.
                ImageButton btn = (ImageButton) sender;
                RepeaterItem rptItemValidacao = (RepeaterItem) btn.NamingContainer;
                Repeater rptValidacao = (Repeater) rptItemValidacao.NamingContainer;
                foreach (RepeaterItem item in rptValidacao.Items)
                {
                    TextBox txtItem = (TextBox)item.FindControl("txtDescricao");
                    if (string.IsNullOrEmpty(txtItem.Text))                                           
                        throw new ValidationException("Item do conteúdo é obrigatório.");                    
                }

                Repeater rptItem;
                ORC_Conteudo_Cadastro conteudo = RetornaConteudo(sender, out rptItem);
                List<ORC_ConteudoItem> listItensConteudo = conteudo.listItensConteudo;                           

                // Novo item.
                ORC_ConteudoItem novoItem = new ORC_ConteudoItem
                                                {
                                                    obj_id = conteudo.entConteudo.obj_id
                                                    ,
                                                    ctd_id = conteudo.entConteudo.ctd_id
                                                };

                listItensConteudo.Add(novoItem);

                // Seta flag que indica se o item do Objetivo é o que está sendo editado.
                editandoItemObjetivo = true;

                rptItem.DataSource = listItensConteudo;
                rptItem.DataBind();
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar item de conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoItemHabilidade_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Utilizado na validação de habilidade
                ImageButton btn = (ImageButton)sender;
                RepeaterItem rptItemValidacao = (RepeaterItem)btn.NamingContainer;
                Repeater rptValidacao = (Repeater)rptItemValidacao.NamingContainer;
                foreach (RepeaterItem item in rptValidacao.Items)
                {
                    TextBox txtHabilidade = (TextBox)item.FindControl("txtDescricao");
                    if (string.IsNullOrEmpty(txtHabilidade.Text))
                        throw new ValidationException("Habilidade é obrigatório.");
                }

                Repeater rptItem;
                ORC_Conteudo_Cadastro conteudo = RetornaConteudo(sender, out rptItem);
                List<ORC_Habilidades> listHabilidades = conteudo.listHabilidades;                                  

                // Novo item.
                ORC_Habilidades novoItem = new ORC_Habilidades
                {
                    obj_id = conteudo.entConteudo.obj_id
                    ,
                    ctd_id = conteudo.entConteudo.ctd_id
                };

                listHabilidades.Add(novoItem);

                // Seta flag que indica se o item do Objetivo é o que está sendo editado.
                editandoItemObjetivo = true;

                rptItem.DataSource = listHabilidades;
                rptItem.DataBind();
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar item de conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            // Adiciona uma nova linha de objetivo.
            rptObjetivos.DataSource = ORC_ObjetivoBO.SelecionaPor_Curriculo_Disciplina(VS_cur_id, VS_crr_id, VS_crp_id, VS_tds_id, VS_cal_id);
            rptObjetivos.DataBind();           
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/OrientacaoCurricular/Busca.aspx", false);
        }

        protected void btnCopiar_Click(object sender, EventArgs e)
        {
            bool NaoSalvouDados = false;

            foreach (RepeaterItem item in rptObjetivos.Items)
            {
                Button btnSalvarObj = (Button)item.FindControl("btnSalvarObjetivo");
                if (btnSalvarObj != null && btnSalvarObj.Visible)
                {
                    NaoSalvouDados = true;
                    break;
                }
            }

            if (NaoSalvouDados)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CopiarOrientacaoCurricular", "$(document).ready(function(){ $('#divMsgSalvarDados').dialog('open'); });", true);
            }
            else
            {
                UCComboCalendario2.SelectedIndex = 0;
                divMsgCopiarOrientacao.Visible = false;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CopiarOrientacaoCurricular", "$(document).ready(function(){ $('#divCopiarOrientacao').dialog('open'); });", true);
            }
        }

        protected void btnSalvarCopia_Click(object sender, EventArgs e)
        {
            if (VS_cal_id == UCComboCalendario2.Valor)
            {
                lblMsgErroCopia.Text = UtilBO.GetErroMessage("Não é possível copiar a orientação curricular para a mesma etapa escolar, grupamento de ensino e " +
                                                             GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + ".", UtilBO.TipoMensagem.Alerta);
                divMsgCopiarOrientacao.Visible = true;
            }
            else
            {
                divMsgCopiarOrientacao.Visible = false;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "CopiarOrientacaoCurricular", "$(document).ready(function(){ $('#divCopiarOrientacao').dialog('close'); });", true);
                updConfirm.Update();
                btnSalvarObjetivo_Click(sender, e);

                UCComboCalendario2.SelectedIndex = 0;

                CarregarObjetivosConteudos();
            }
        }

        #endregion
    }
}
