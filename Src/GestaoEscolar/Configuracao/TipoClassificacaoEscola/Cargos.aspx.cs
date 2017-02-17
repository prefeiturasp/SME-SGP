using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoClassificacaoEscola_Cargos : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Armazena o ID do tipo de classificação de escola.
    /// </summary>
    public int VS_tce_id
    {
        get
        {
            if (ViewState["VS_tce_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tce_id"]);
            }

            return -1;
        }
        set
        {
            ViewState["VS_tce_id"] = value;
        }
    }

    /// <summary>
    /// Lista que armazena os cargos adicionados
    /// </summary>
    private List<ESC_TipoClassificacaoEscolaCargos> VS_ListaTipoClassificacaoEscolaCargos
    {
        get
        {
            if (ViewState["VS_ListaTipoClassificacaoEscolaCargos"] == null)
                ViewState["VS_ListaTipoClassificacaoEscolaCargos"] = new List<ESC_TipoClassificacaoEscolaCargos>();

            return (List<ESC_TipoClassificacaoEscolaCargos>)ViewState["VS_ListaTipoClassificacaoEscolaCargos"];
        }
        set
        {
            ViewState["VS_ListaTipoClassificacaoEscolaCargos"] = value;
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
            
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                VS_tce_id = PreviousPage.EditItem;
                CarregarCargos();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoClassificacaoEscola.Cargos.lblMensagem.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }
    
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracao/TipoClassificacaoEscola/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

    #region Métodos
    
    /// <summary>
    /// Carrega os cargos.
    /// </summary>
    private void CarregarCargos()
    {
        ESC_TipoClassificacaoEscola ESC_TipoClassificacaoEscola = new ESC_TipoClassificacaoEscola { tce_id = VS_tce_id };
        ESC_TipoClassificacaoEscolaBO.GetEntity(ESC_TipoClassificacaoEscola);

        ckbPermitirQualquerCargoEscola.Checked = ESC_TipoClassificacaoEscola.tce_permiteQualquerCargoEscola;

        lblTipoClassificacaoEscolaNome.Text = ESC_TipoClassificacaoEscola.tce_nome;

        VS_ListaTipoClassificacaoEscolaCargos = ESC_TipoClassificacaoEscolaCargosBO.SelectTipoClassificacaoEscolaCargosByTipoClassificacaoEscola(VS_tce_id);
        if (VS_ListaTipoClassificacaoEscolaCargos.Count > 0)
        {
            gdvCargos.DataSource = VS_ListaTipoClassificacaoEscolaCargos;
            gdvCargos.DataBind();
        }
    }
    
    #endregion
    
    protected void gdvCargos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex.ToString());

                int tcc_id = Convert.ToInt32(gdvCargos.DataKeys[index]["tcc_id"].ToString());
                
                Label lblVigenciaFinal = (Label)e.Row.FindControl("lblVigenciaFinal");

                DateTime vigenciaFinal = Convert.ToDateTime(lblVigenciaFinal.Text);

                if (vigenciaFinal == DateTime.MinValue)
                {
                    lblVigenciaFinal.Text = string.Empty;
                }
                else
                {
                    lblVigenciaFinal.Text = vigenciaFinal.ToString("dd/MM/yyyy");
                }
            }
        }
        catch (Exception erro)
        {
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "TipoClassificacaoEscola.Cargos.lblMensagem.ErroCarregarCargoAtribuicaoEsporadica").ToString(), UtilBO.TipoMensagem.Erro);
            ApplicationWEB._GravaErro(erro);
        }
    }
}