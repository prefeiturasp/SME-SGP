using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.ObjetoAprendizagem
{
    public partial class Busca : MotherPageLogado
    {
        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            
            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        uppPesquisa.Visible = false;
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessage.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                    }

                    Page.Form.DefaultFocus = UCComboUAEscola.ComboEscola_ClientID;
                    Page.Form.DefaultButton = btnGerarRel.UniqueID;

                    string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                    Inicializar();
                    VerificaBusca();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessage.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }

            UCComboAnoLetivo1.IndexChanged += UCComboAnoLetivo1_IndexChanged;
        }

        #endregion Page Life Cycle

        #region Eventos

        protected void btnGerarRel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                    GerarRel();
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessage.ErroGerarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void cvCiclos_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                if (ckbCampo != null && ckbCampo.Checked)
                {
                    args.IsValid = true;
                    break;
                }
            }
        }

        #endregion Eventos

        #region Delegates

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (UCComboAnoLetivo1.ano > 0)
                {
                    rptCampos.DataSource = ACA_TipoCicloBO.SelecionaTipoCicloAtivosEscolaAno(UCComboAnoLetivo1.ano, UCComboUAEscola.Esc_ID, UCComboUAEscola.Uad_ID, true, ApplicationWEB.AppMinutosCacheLongo);
                    rptCampos.DataBind();

                    lblMessageCiclo.Visible = false;
                    if (rptCampos.Items.Count <= 0)
                    {
                        lblMessageCiclo.Visible = true;
                        lblMessageCiclo.Text = GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessageCiclo.Text").ToString();
                    }

                    divCiclo.Visible = true;
                }
                else
                    divCiclo.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessage.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboAnoLetivo1_IndexChanged()
        {
            UCComboTipoDisciplina1.CarregarNivelEnsinoTipoDisciplinaObjetosAprendizagem(UCComboAnoLetivo1.ano);
            if (UCComboAnoLetivo1.ano > 0)
            {
                rptCampos.DataSource = ACA_TipoCicloBO.SelecionaTipoCicloAtivosEscolaAno(UCComboAnoLetivo1.ano, UCComboUAEscola.Esc_ID, UCComboUAEscola.Uad_ID, true, ApplicationWEB.AppMinutosCacheLongo);
                rptCampos.DataBind();

                lblMessageCiclo.Visible = false;
                if (rptCampos.Items.Count <= 0)
                {
                    lblMessageCiclo.Visible = true;
                    lblMessageCiclo.Text = GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessageCiclo.Text").ToString();
                }

                divCiclo.Visible = true;
            }
            else
                divCiclo.Visible = false;
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Inicializa os filtros da pagina.
        /// </summary>
        protected void Inicializar()
        {
            UCComboAnoLetivo1.CarregarAnoAtual();
            UCComboUAEscola.Inicializar();
            UCComboTipoDisciplina1.CarregarNivelEnsinoTipoDisciplinaObjetosAprendizagem(UCComboAnoLetivo1.ano);
            rptCampos.DataSource = ACA_TipoCicloBO.SelecionaTipoCicloAtivosEscolaAno(UCComboAnoLetivo1.ano, UCComboUAEscola.Esc_ID, UCComboUAEscola.Uad_ID, true, ApplicationWEB.AppMinutosCacheLongo);
            rptCampos.DataBind();

            lblMessageCiclo.Visible = false;
            if (rptCampos.Items.Count <= 0)
            {
                lblMessageCiclo.Visible = true;
                lblMessageCiclo.Text = GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.lblMessageCiclo.Text").ToString();
            }

            divCiclo.Visible = true;

            if (UCComboUAEscola.VisibleUA)
                UCComboUAEscola_IndexChangedUA();
            else
                UCComboUAEscola.ObrigatorioEscola = true;
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioObjetoAprendizagem)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                // Ano
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ano", out valor);
                UCComboAnoLetivo1.ano = Convert.ToInt32(valor);
                UCComboAnoLetivo1_IndexChanged();

                // UA Escola
                if (UCComboUAEscola.FiltroEscola)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola.Uad_ID = new Guid(valor);
                    }

                    UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty);

                    if (UCComboUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                        SelecionarEscola();
                    }
                }
                else
                {
                    SelecionarEscola();
                }
                UCComboUAEscola_IndexChangedUnidadeEscola();

                // Disciplina
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tds_id", out valor);
                UCComboTipoDisciplina1.Valor = Convert.ToInt32(valor);

                //Ciclos
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ciclosSelecionados", out valor);
                foreach (RepeaterItem item in rptCampos.Items)
                {
                    HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                    if (hdnId != null && valor.Split(',').Any(c => c.Equals(hdnId.Value)))
                    {
                        CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                        if (ckbCampo != null)
                            ckbCampo.Checked = true;
                    }
                }

                // Tipo de relatório
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoRel", out valor);
                ddlTipoRel.SelectedValue = valor;
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
        /// realizada.
        /// </summary>
        private void SelecionarEscola()
        {
            string esc_id;
            string uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
            }
        }

        /// <summary>
        /// Gera o relatorio com base nos filtros selecionados.
        /// </summary>
        private void GerarRel()
        {
            string ciclosSelecionados = "";
            string ciclosTexto = "";
            bool todosCiclos = true;
            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                if (ckbCampo != null)
                {
                    if (ckbCampo.Checked)
                    {
                        ciclosTexto += (string.IsNullOrEmpty(ciclosTexto) ? "" : ", ") + ckbCampo.Text;
                        HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                        if (hdnId != null)
                            ciclosSelecionados += (string.IsNullOrEmpty(ciclosSelecionados) ? "" : ",") + hdnId.Value;
                    }
                    else
                        todosCiclos = false;
                }
            }

            if (todosCiclos)
                ciclosTexto = "Todos";

            if (string.IsNullOrEmpty(ciclosSelecionados))
                throw new ValidationException(GetGlobalResourceObject("Relatorios", "ObjetoAprendizagem.Busca.cvCiclos.ErrorMessage").ToString());

            SalvarBusca(ciclosSelecionados);
            
            string parameter = string.Empty;
            string report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioObjetoAprendizagem).ToString();
            
            parameter = "uad_idSuperior=" + UCComboUAEscola.Uad_ID.ToString()
                        + "&esc_id=" + UCComboUAEscola.Esc_ID.ToString()
                        + "&uni_id=" + UCComboUAEscola.Uni_ID.ToString()
                        + "&cal_ano=" + UCComboAnoLetivo1.ano.ToString()
                        + "&ciclosSelecionados=" + ciclosSelecionados
                        + "&ciclosTexto=" + ciclosTexto
                        + "&tds_id=" + UCComboTipoDisciplina1.Valor.ToString()
                        + "&tds_nome=" + UCComboTipoDisciplina1.Texto
                        + "&tipoRel=" + ddlTipoRel.SelectedValue
                        + "&MostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                        + "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                , ApplicationWEB.LogoRelatorioSSRS)
                        + "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio")
                        + "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria")
                        + "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString()
                        + "&adm=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
                        + "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id
                        + "&gru_id=" + __SessionWEB.__UsuarioWEB.Grupo.gru_id;

            MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.CallReport("Relatorios", report, parameter, HttpContext.Current);
        }

        /// <summary>
        /// Salva os dados da busca realizada para ser carregada posteriormente.
        /// </summary>
        private void SalvarBusca(string ciclosSelecionados)
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("ano", UCComboAnoLetivo1.ano.ToString());
            filtros.Add("uad_idSuperior", UCComboUAEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCComboUAEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCComboUAEscola.Uni_ID.ToString());
            filtros.Add("tds_id", UCComboTipoDisciplina1.Valor.ToString());
            filtros.Add("ciclosSelecionados", ciclosSelecionados); 
            filtros.Add("tipoRel", ddlTipoRel.SelectedValue);

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioObjetoAprendizagem, Filtros = filtros };
        }

        #endregion Métodos
    }
}