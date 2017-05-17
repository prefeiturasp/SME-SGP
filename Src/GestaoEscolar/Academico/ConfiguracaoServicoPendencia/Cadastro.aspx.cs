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

    private void Carregar()
    {
        try
        {
            ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia
            {
                csp_id = VS_csp_id
            };
            ACA_ConfiguracaoServicoPendenciaBO.GetEntity(entity);


            chkDisciplinaSemAula.Checked = entity.csp_disciplinaSemAula;
            chkSemNota.Checked = entity.csp_semNota;
            chkSemParecer.Checked = entity.csp_semParecer;
            chkSemPlanejamento.Checked = entity.csp_semPlanejamento;
            chkSemResultadoFinal.Checked = entity.csp_semResultadoFinal;
            chkSemSintese.Checked = entity.csp_semSintese;
            chkSemPlanoAula.Checked = entity.csp_semPlanoAula;

            UCComboTipoNivelEnsino.Valor = entity.tne_id > 0 ? entity.tne_id : -1;
            UCComboTipoNivelEnsino.PermiteEditar = false;

            UCComboTipoModalidadeEnsino.Valor = entity.tme_id > 0 ? entity.tme_id : -1;
            UCComboTipoModalidadeEnsino.PermiteEditar = false;

            UCComboTipoTurma.Valor = entity.tur_tipo > 0 ? entity.tur_tipo : Convert.ToByte(0);
            UCComboTipoTurma.PermiteEditar = false;
            
            VS_csp_id = entity.csp_id;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a configuração do serviço de pendência.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void Salvar()
    {
        try
        {
            ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia
            {
                csp_id = VS_csp_id,
                tne_id = UCComboTipoNivelEnsino.Valor,
                tme_id = UCComboTipoModalidadeEnsino.Valor,
                tur_tipo = UCComboTipoTurma.Valor,
                csp_disciplinaSemAula = chkDisciplinaSemAula.Checked,
                csp_semNota = chkSemNota.Checked,
                csp_semParecer = chkSemParecer.Checked,
                csp_semPlanejamento = chkSemPlanejamento.Checked,
                csp_semResultadoFinal = chkSemResultadoFinal.Checked,
                csp_semSintese = chkSemSintese.Checked,
                csp_semPlanoAula = chkSemPlanoAula.Checked,
                IsNew = (VS_csp_id > 0) ? false : true
            };

            if (ACA_ConfiguracaoServicoPendenciaBO.Save(entity))
            {
                string message = "";
                if (VS_csp_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "csp_id: " + entity.csp_id);
                    message = UtilBO.GetErroMessage("Configuração do serviço de pendência cadastrada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "csp_id: " + entity.csp_id);
                    message = UtilBO.GetErroMessage("Configuração do serviço de pendência alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                if (ParametroPermanecerTela)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = message;
                    VS_csp_id = entity.csp_id;
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a configuração do serviço de pendência.", UtilBO.TipoMensagem.Erro);
            }           
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a configuração do serviço de pendência.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();

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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
            Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;
        }
    }
    
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (UCComboTipoNivelEnsino.Valor>0 || UCComboTipoModalidadeEnsino.Valor>0 || UCComboTipoTurma.Valor>0)
            Salvar();
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            lblMessage.Text = UtilBO.GetErroMessage("Selecione o tipo de nível de ensino, o tipo de modalidade de ensino ou o tipo de turma.", UtilBO.TipoMensagem.Alerta);
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    
}
