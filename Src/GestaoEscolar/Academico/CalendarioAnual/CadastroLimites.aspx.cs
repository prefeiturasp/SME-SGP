using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.CalendarioAnual
{
    public partial class CadastroLimites : MotherPageLogado
    {
        protected class sLimite
        {
            public string tev_nome { get; set; }
            public string tpc_nome { get; set; }

            public int tev_id { get; set; }
            public int evl_id { get; set; }

            public ACA_EventoLimite evl { get; set; }
        }

        #region Propriedades

        /// <summary>
        /// Id do calendário selecionado na página de busca.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                if (ViewState["VS_cal_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_cal_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        protected string ValidationGroup { get { return "CadastroLimites"; } }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroEventoLimite.js"));
            }

            if (!IsPostBack)
            {
                cpvDataInicio.ValueToCompare = DateTime.Now.ToString("dd/MM/yyyy");

                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    VS_cal_id = PreviousPage.SelectedItem;

                    var entityCalendario = new ACA_CalendarioAnual() { cal_id = VS_cal_id };
                    ACA_CalendarioAnualBO.GetEntity(entityCalendario);

                    lblCalendarioAno.Text = string.Format("{0} (Ano {1})", entityCalendario.cal_descricao, entityCalendario.cal_ano);

                    UCCTipoEvento.CarregarLiberacao();
                    UCCPeriodoCalendario.CarregarPorCalendario(VS_cal_id, entityCalendario.cal_permiteLancamentoRecesso);
                    UCComboUAEscola.Inicializar();

                    //btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                    Pesquisar();
                }
                else
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Selecione um calendário para definir os limites para data de criação de eventos.", UtilBO.TipoMensagem.Alerta);
                    Response.Redirect("Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        protected void grvLimitesCalendario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var data = e.Row.DataItem as sLimite;

                var ltlAlcance = e.Row.FindControl("ltlAlcance") as ITextControl;
                if (ltlAlcance != null)
                {
                    if (data.evl.esc_id > 0)
                    {
                        var escola = new ESC_Escola() { esc_id = data.evl.esc_id };
                        ESC_EscolaBO.GetEntity(escola);

                        ltlAlcance.Text = escola.esc_nome;
                    }
                    else if(data.evl.uad_id != Guid.Empty)
                    {
                        var dre = new SYS_UnidadeAdministrativa { uad_id = data.evl.uad_id, ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id };
                        SYS_UnidadeAdministrativaBO.GetEntity(dre);

                        ltlAlcance.Text = dre.uad_nome;
                    }
                    else
                        ltlAlcance.Text = "Toda a rede";
                }

                var ltlUsuario = e.Row.FindControl("ltlUsuario") as ITextControl;
                if (ltlUsuario != null)
                {
                    var usuario = new SYS_Usuario() { usu_id = data.evl.usu_id };
                    SYS_UsuarioBO.GetEntity(usuario);

                    var pessoa = new PES_Pessoa() { pes_id = usuario.pes_id };
                    PES_PessoaBO.GetEntity(pessoa);

                    ltlUsuario.Text = pessoa.pes_nome ?? usuario.usu_login;
                }

                var ltlVigencia = e.Row.FindControl("ltlVigencia") as ITextControl;
                if (ltlVigencia != null)
                {
                    ltlVigencia.Text = string.Format("{0} - {1}",
                                                     data.evl.evl_dataInicio.ToString("dd/MM/yyyy"),
                                                     data.evl.evl_dataFim.ToString("dd/MM/yyyy"));
                }

                //var btnExcluir = e.Row.FindControl("btnExcluir") as WebControl;
                //if (btnExcluir != null)
                //{
                //    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                //}
            }
        }

        protected void grvLimitesCalendario_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                e.Cancel = true;

                var tev_id = Convert.ToInt32(e.Keys["tev_id"].ToString());
                var evl_id = Convert.ToInt32(e.Keys["evl_id"].ToString());

                var evl = new ACA_EventoLimite() { cal_id = VS_cal_id, tev_id = tev_id, evl_id = evl_id };
                ACA_EventoLimiteBO.GetEntity(evl);

                if (ACA_EventoLimiteBO.Delete(evl))
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Limite excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    Pesquisar();
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Não foi possível excluir.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCTipoEvento_IndexChanged()
        {
            var entityTipoEvento = new ACA_TipoEvento()
            {
                tev_id = UCCTipoEvento.Valor,
                tev_periodoCalendario = false
            };

            if (UCCTipoEvento.Valor > 0)
            {
                ACA_TipoEventoBO.GetEntity(entityTipoEvento);
            }

            UCCPeriodoCalendario.Visible = entityTipoEvento.tev_periodoCalendario;
            UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
        }

        protected void ddlAlcance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAlcance.SelectedValue == "-1" || ddlAlcance.SelectedValue == "1")
            {
                UCComboUAEscola.Visible = false;
                UCComboUAEscola.ObrigatorioEscola = false;
                UCComboUAEscola.ObrigatorioUA = false;
            }
            else if (ddlAlcance.SelectedValue == "2")
            {
                UCComboUAEscola.Visible = true;
                UCComboUAEscola.ExibeComboEscola = true;
                UCComboUAEscola.Uad_ID = Guid.Empty;
                UCComboUAEscola.EnableEscolas = false;

                UCComboUAEscola.ObrigatorioEscola = true;
            }
            else if (ddlAlcance.SelectedValue == "3")
            {
                UCComboUAEscola.Visible = true;

                UCComboUAEscola.ExibeComboEscola = false;
                UCComboUAEscola.ObrigatorioEscola = false;
                UCComboUAEscola.ObrigatorioUA = true;
            }
        }

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
            {
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola.EnableEscolas = false;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    if (ddlAlcance.SelectedValue == "-1")
                        throw new ValidationException("Alcance é obrigatório.");
                    else if (ddlAlcance.SelectedValue == "2" && UCComboUAEscola.Esc_ID <= 0)
                        throw new ValidationException("Escola é obrigatória.");
                    else if (ddlAlcance.SelectedValue == "3" && UCComboUAEscola.Uad_ID == Guid.Empty)
                        throw new ValidationException("DRE é obrigatória.");

                    #region Preenche entity

                    var entity = new ACA_EventoLimite()
                    {
                        cal_id = VS_cal_id,
                        tev_id = UCCTipoEvento.Valor,
                        evl_dataInicio = Convert.ToDateTime(txtDataInicio.Text),
                        evl_dataFim = Convert.ToDateTime(txtDataFim.Text),
                        usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    };

                    if (UCCPeriodoCalendario.Visible)
                        entity.tpc_id = UCCPeriodoCalendario.Valor[0];

                    if (UCComboUAEscola.ExibeComboEscola)
                    {
                        entity.esc_id = UCComboUAEscola.Esc_ID;
                        entity.uni_id = UCComboUAEscola.Uni_ID;
                    }
                    else if (UCComboUAEscola.VisibleUA)
                        entity.uad_id = UCComboUAEscola.Uad_ID;

                    #endregion

                    ACA_EventoLimiteBO.Save(entity);
                    lblMessage.Text = UtilBO.GetErroMessage("Dados salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Pesquisar();

                    #region Limpar formulário

                    UCCTipoEvento.Valor = -1;
                    UCCTipoEvento_IndexChanged();
                    txtDataInicio.Text = string.Empty;
                    txtDataFim.Text = string.Empty;
                    ddlAlcance.SelectedIndex = 0;
                    ddlAlcance_SelectedIndexChanged(null, null);
                    UCComboUAEscola.DdlUA.SelectedIndex = 0;
                    UCComboUAEscola.Visible = false;

                    #endregion
                }
                catch (ValidationException ex)
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
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Metodos

        private void Pesquisar()
        {
            var lstLimite = ACA_EventoLimiteBO.GetSelectByCalendario(VS_cal_id);
            var tblTipoEvento = ACA_TipoEventoBO.SelecionaTipoEvento(0, Guid.Empty);
            var lstTipoPeriodo = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);

            int tpc_idRecesso = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            string tcp_nomeRecesso = string.Empty;
            if (tpc_idRecesso > 0)
            {
                ACA_TipoPeriodoCalendario tpc = new ACA_TipoPeriodoCalendario { tpc_id = tpc_idRecesso };
                ACA_TipoPeriodoCalendarioBO.GetEntity(tpc);
                tcp_nomeRecesso = tpc.tpc_nome;
            }

            var source = lstLimite.Join(tblTipoEvento.Rows.Cast<DataRow>(),
                                        evl => evl.tev_id,
                                        tev => Convert.ToInt32(tev["tev_id"]),
                                        (evl, tev) => new sLimite()
                                        {
                                            tev_nome = tev["tev_nome"].ToString(),
                                            tpc_nome = (evl.tpc_id > 0
                                                                        ? (evl.tpc_id == tpc_idRecesso ? tcp_nomeRecesso : lstTipoPeriodo.First(tpc => tpc.tpc_id == evl.tpc_id).tpc_nome)
                                                                        : string.Empty),
                                            tev_id = evl.tev_id,
                                            evl_id = evl.evl_id,
                                            evl = evl
                                        })
                                  .GroupBy(evl => string.Format("{0}{1}{2}",
                                                                evl.tev_nome,
                                                                (string.IsNullOrEmpty(evl.tpc_nome) ? string.Empty : " - "),
                                                                evl.tpc_nome))
                                  .Select(grp => new
                                  {
                                      TipoEventoBimestre = grp.Key,
                                      Limites = grp.ToList()
                                  })
                                  .OrderBy(grp => grp.TipoEventoBimestre);
            rptLimitesCalendario.DataSource = source;
            rptLimitesCalendario.DataBind();
        }

        #endregion
    }
}