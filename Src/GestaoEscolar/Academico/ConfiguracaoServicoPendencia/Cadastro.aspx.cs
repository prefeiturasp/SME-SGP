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
    /// Propriedade em ViewState que armazena valor de csp_id (ID da configuração do serviço de pendência)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
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

            UCComboTipoNivelEnsino.Valor = entity.tne_id;
            UCComboTipoNivelEnsino.PermiteEditar = false;

            UCComboTipoModalidadeEnsino.Valor = entity.tme_id;
            UCComboTipoModalidadeEnsino.PermiteEditar = false;

            UCComboTipoTurma.Valor = entity.tur_tipo;
            UCComboTipoTurma.PermiteEditar = false;



            VS_csp_id = entity.csp_id;

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a configuração do serviço de pendência.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    /// <summary>
    /// Salva as informações da Configuração do serviço de pendência
    /// </summary>
    private void Salvar()
    {
        try
        {
            ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia {

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
                if (VS_csp_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Cadastro de configuração do serviço de pendência. csp_id" + entity.csp_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Configuração do serviço de pendência cadastrada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "fun_id: " + entity.csp_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Configuração do serviço de pendência alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                if (ParametroPermanecerTela)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = __SessionWEB.PostMessages;
                }
                else
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }            
        }
        catch (MSTech.Validation.Exceptions.ValidationException ex)
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
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a função.", UtilBO.TipoMensagem.Erro);
        }
    }
}
