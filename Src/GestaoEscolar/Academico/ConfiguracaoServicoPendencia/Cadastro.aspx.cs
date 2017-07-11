using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Academico_ConfiguracaoServicoPendencia_Cadastro : MotherPageLogado
{
    #region Propriedades
    private bool ParametroPermanecerTela
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }
    
    private int VS_csp_id
    {
        get
        {
            return Convert.ToInt32(ViewState["VS_csp_id"] ?? -1);
        }
        set
        {
            ViewState["VS_csp_id"] = value;
        }
    }

    private int VS_tne_id
    {
        get
        {
            return Convert.ToInt32(ViewState["VS_tne_id"] ?? -1);
        }
        set
        {
            ViewState["VS_tne_id"] = value;
        }
    }

    private int VS_tme_id
    {
        get
        {
            return Convert.ToInt32(ViewState["VS_tme_id"] ?? -1);
        }
        set
        {
            ViewState["VS_tme_id"] = value;
        }
    }

    private byte VS_tur_tipo
    {
        get
        {
            return Convert.ToByte(ViewState["VS_tur_tipo"] ?? 0);
        }
        set
        {
            ViewState["VS_tur_tipo"] = value;
        }
    }
    #endregion

    #region Métodos
    private void Carregar()
    {
        try
        {
            ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia
            {
                csp_id = VS_csp_id
                ,tne_id = VS_tne_id
                ,tme_id = VS_tme_id
                ,tur_tipo = VS_tur_tipo
            };
            ACA_ConfiguracaoServicoPendenciaBO.GetEntity(entity);


            chkDisciplinaSemAula.Checked = entity.csp_disciplinaSemAula;
            chkSemNota.Checked = entity.csp_semNota;
            chkSemParecer.Checked = entity.csp_semParecer;
            chkSemPlanejamento.Checked = entity.csp_semPlanejamento;
            chkSemResultadoFinal.Checked = entity.csp_semResultadoFinal;
            chkSemSintese.Checked = entity.csp_semSintese;
            chkSemPlanoAula.Checked = entity.csp_semPlanoAula;

            foreach (ListItem item in cblSemRelatorioAtendimento.Items)
            {
                eConfiguracaoServicoPendenciaSemRelatorioAtendimento valor = (eConfiguracaoServicoPendenciaSemRelatorioAtendimento)Enum.Parse(typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento), item.Value);
                item.Selected = ((eConfiguracaoServicoPendenciaSemRelatorioAtendimento)entity.csp_semRelatorioAtendimento).HasFlag(valor);
            }

            UCComboTipoNivelEnsino.Valor = entity.tne_id > 0 ? entity.tne_id : -1;
            UCComboTipoNivelEnsino.PermiteEditar = false;

            UCComboTipoModalidadeEnsino.Valor = entity.tme_id > 0 ? entity.tme_id : -1;
            UCComboTipoModalidadeEnsino.PermiteEditar = false;

            UCComboTipoTurma.Valor = entity.tur_tipo > 0 ? entity.tur_tipo : Convert.ToByte(0);
            UCComboTipoTurma.PermiteEditar = false;
            
            VS_csp_id = entity.csp_id;
            VS_tne_id = entity.tne_id;
            VS_tme_id = entity.tme_id;
            VS_tur_tipo = entity.tur_tipo;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }

    private void Salvar()
    {
        try
        {
            ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia
            {
                csp_id = VS_csp_id,
            };

            ACA_ConfiguracaoServicoPendenciaBO.GetEntity(entity);

            entity.tne_id = UCComboTipoNivelEnsino.Valor;
            entity.tme_id = UCComboTipoModalidadeEnsino.Valor;
            entity.tur_tipo = UCComboTipoTurma.Valor;
            entity.csp_disciplinaSemAula = chkDisciplinaSemAula.Checked;
            entity.csp_semNota = chkSemNota.Checked;
            entity.csp_semParecer = chkSemParecer.Checked;
            entity.csp_semPlanejamento = chkSemPlanejamento.Checked;
            entity.csp_semResultadoFinal = chkSemResultadoFinal.Checked;
            entity.csp_semSintese = chkSemSintese.Checked;
            entity.csp_semPlanoAula = chkSemPlanoAula.Checked;
            entity.IsNew = (VS_csp_id > 0) ? false : true;

            var semRelatorioAtendimento = from ListItem item in cblSemRelatorioAtendimento.Items
                                          where item.Selected
                                          select (eConfiguracaoServicoPendenciaSemRelatorioAtendimento)Enum.Parse(typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento), item.Value);

            entity.csp_semRelatorioAtendimento = (int)semRelatorioAtendimento.Aggregate(eConfiguracaoServicoPendenciaSemRelatorioAtendimento.Nenhum, (acumulado, item) => acumulado | item);

            if (ACA_ConfiguracaoServicoPendenciaBO.SelectBy_VerificaConfiguracaoServicoPendencia(entity, null))
                    throw new ACA_ConfiguracaoServicoPendenciaDuplicateException(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroDuplicacao").ToString());
            
            if (ACA_ConfiguracaoServicoPendenciaBO.Save(entity))
            {
                string message = "";
                if (VS_csp_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "csp_id: " + entity.csp_id);
                    message = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.SucessoCadastrar").ToString(), UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "csp_id: " + entity.csp_id);
                    message = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.SucessoAlterar").ToString(), UtilBO.TipoMensagem.Sucesso);
                }
                if (ParametroPermanecerTela)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = message;
                    VS_csp_id = entity.csp_id;
                    VS_tne_id = entity.tne_id;
                    VS_tme_id = entity.tme_id;
                    VS_tur_tipo = entity.tur_tipo;
                    if (VS_csp_id > 0)
                        Carregar();
                }
                else
                {
                    __SessionWEB.PostMessages = message;
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }           
        }
        catch (ACA_ConfiguracaoServicoPendenciaDuplicateException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }
    #endregion

    #region Eventos
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();

                GestaoEscolarUtilBO.CarregarComboEnum<eConfiguracaoServicoPendenciaSemRelatorioAtendimento>(cblSemRelatorioAtendimento.Items, true);

                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    VS_csp_id = PreviousPage.Edit_csp_id;
                    if (VS_csp_id > 0)
                        Carregar();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroSistema").ToString(), UtilBO.TipoMensagem.Erro);
            }
            Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;
        }
    }
    
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        bool peloMenosUmChecado = chkDisciplinaSemAula.Checked ||
                        chkSemNota.Checked ||
                        chkSemParecer.Checked ||
                        chkSemPlanejamento.Checked ||
                        chkSemPlanoAula.Checked ||
                        chkSemResultadoFinal.Checked ||
                        chkSemSintese.Checked ||
                        cblSemRelatorioAtendimento.Items.Cast<ListItem>().Any(p => p.Selected);

        if (!(UCComboTipoNivelEnsino.Valor > 0 || UCComboTipoModalidadeEnsino.Valor > 0 || UCComboTipoTurma.Valor > 0))
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Configuracao.ErroSelecione").ToString(), UtilBO.TipoMensagem.Alerta);
        }
        else if (!peloMenosUmChecado)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.ErroChecar").ToString(), UtilBO.TipoMensagem.Alerta);
        }
        else
            Salvar();
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    #endregion
}
