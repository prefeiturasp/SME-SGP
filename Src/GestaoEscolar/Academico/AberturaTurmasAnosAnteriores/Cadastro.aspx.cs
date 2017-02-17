using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.AberturaTurmasAnosAnteriores
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Armazena o ID da abertura de anos anteriores
        /// </summary>
        private long VS_tab_id
        {
            get
            {
                if (ViewState["VS_tab_id"] != null)
                {
                    return Convert.ToInt64(ViewState["VS_tab_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tab_id"] = value;
            }
        }

        private DateTime VS_dataIncialAnterior
        {
            get
            {
                if (ViewState["VS_dataIncialAnterior"] != null)
                {
                    return Convert.ToDateTime(ViewState["VS_dataIncialAnterior"]);
                }
                return DateTime.MinValue;
            }
            set
            {
                ViewState["VS_dataIncialAnterior"] = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                }

                if (!IsPostBack)
                {
                    InicializarEscolas();

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        Carregar(PreviousPage.EditItem);
                    }

                    Page.Form.DefaultFocus = txtAno.ClientID;
                    Page.Form.DefaultButton = btnSalvar.UniqueID;
                    btnSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
                }

                ucComboUAEscola.IndexChangedUA += UCFiltroEscolas1__Selecionar;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);

            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    string mensagem = ValidarCampos();

                    if (string.IsNullOrEmpty(mensagem))
                    {
                        Salvar();
                    }
                    else
                    {
                        throw new ValidationException(mensagem);
                    }
                }
                catch (ValidationException ve)
                {
                    lblMessage.Text = UtilBO.GetMessage(ve.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os combos.
        /// </summary>
        public void InicializarEscolas()
        {
            try
            {
                ucComboUAEscola.FocusUA();
                ucComboUAEscola.Inicializar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        private string ValidarCampos()
        {
            string mensagem = string.Empty;

            int ano = string.IsNullOrEmpty(txtAno.Text) ? 0 : Convert.ToInt32(txtAno.Text);

            if (ano > 0 && ano < 2015)
            {
                mensagem = GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoAnoMaiorIgual").ToString();
            }

            if (ano > 0 && ano >= DateTime.Now.Year)
            {
                mensagem += string.IsNullOrEmpty(mensagem) ? GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoAnoMenorAnoAtual").ToString() : "<br/>" + GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoAnoMenorAnoAtual").ToString();
            }

            DateTime dtInicio = Convert.ToDateTime(txtDataInicial.Text);
            DateTime dtFim = string.IsNullOrEmpty(txtDataFinal.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataFinal.Text);

            if (dtInicio < DateTime.Now.Date && ((VS_tab_id > 0 && VS_dataIncialAnterior != dtInicio) || (VS_tab_id <= 0)))
            {
                mensagem += string.IsNullOrEmpty(mensagem) ? GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataInicialMaiorIgualDataAtual").ToString() : "<br/>" + GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataInicialMaiorIgualDataAtual").ToString();
            }

            if (dtFim != DateTime.MinValue && dtInicio > dtFim)
            {
                mensagem += string.IsNullOrEmpty(mensagem) ? GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataFinalMaiorIgualDataInicial").ToString() : "<br/>" + GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataFinalMaiorIgualDataInicial").ToString();
            }

            return mensagem;
        }

        /// <summary>
        /// Carrega dados a partir da entidade
        /// </summary>
        private void Carregar(long tab_id)
        {
            try
            {
                TUR_TurmaAberturaAnosAnteriores entity = new TUR_TurmaAberturaAnosAnteriores { tab_id = tab_id };
                TUR_TurmaAberturaAnosAnterioresBO.GetEntity(entity);

                VS_tab_id = entity.tab_id;
                txtAno.Text = entity.tab_ano.ToString();
                ucComboUAEscola.Uad_ID = entity.uad_idSuperior;

                UCFiltroEscolas1__Selecionar();

                ucComboUAEscola.SelectedValueEscolas = new[] { entity.esc_id, entity.uni_id };
                txtDataInicial.Text = entity.tab_dataInicio.ToString("dd/MM/yyyy");
                txtDataFinal.Text = entity.tab_dataFim != DateTime.MinValue ? entity.tab_dataFim.ToString("dd/MM/yyyy") : string.Empty;

                VS_dataIncialAnterior = entity.tab_dataInicio;

                if (entity.tab_status != (byte)TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresStatus.AguardandoExecucao)
                {
                    txtDataInicial.Enabled = false;
                }

                if (entity.tab_dataFim != DateTime.MinValue)
                {
                    txtDataFinal.Text = entity.tab_dataFim.ToString("dd/MM/yyyy");

                    if (entity.tab_dataFim <= DateTime.Now.Date)
                    {
                        txtDataFinal.Enabled = false;
                    }
                }

                txtAno.Enabled = false;
                ucComboUAEscola.PermiteAlterarCombos = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDadosAgendamentos").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere e altera os dados da abertura de anos anteriores
        /// </summary>
        public void Salvar()
        {
            try
            {
                TUR_TurmaAberturaAnosAnteriores entityTurmaAberturaAnosAnteriores = new TUR_TurmaAberturaAnosAnteriores
                {
                    tab_id = VS_tab_id,
                    tab_ano = Convert.ToInt32(txtAno.Text),
                    uad_idSuperior = ucComboUAEscola.Uad_ID,
                    esc_id = ucComboUAEscola.Esc_ID,
                    uni_id = ucComboUAEscola.Uni_ID,
                    tab_dataInicio = Convert.ToDateTime(txtDataInicial.Text),
                    tab_dataFim = string.IsNullOrEmpty(txtDataFinal.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataFinal.Text),
                    tab_status = (byte)TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresStatus.AguardandoExecucao,
                    tab_situacao = (byte)TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresSituacao.Ativo,
                    IsNew = (VS_tab_id > 0) ? false : true
                };

                if (TUR_TurmaAberturaAnosAnterioresBO.Save(entityTurmaAberturaAnosAnteriores))
                {
                    if (VS_tab_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tab_id: " + entityTurmaAberturaAnosAnteriores.tab_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.SucessoIncluir").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tab_id: " + entityTurmaAberturaAnosAnteriores.tab_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.SucessoAlterar").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect("Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
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
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCFiltroEscolas1__Selecionar()
        {
            try
            {
                ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                if (ucComboUAEscola.Uad_ID != Guid.Empty)
                {
                    ucComboUAEscola.FocoEscolas = true;
                    ucComboUAEscola.PermiteAlterarCombos = true;
                }

                ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}