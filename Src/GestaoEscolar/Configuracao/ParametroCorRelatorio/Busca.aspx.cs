using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Reflection;

public partial class Configuracao_ParametroCorRelatorio : MotherPageLogado
{
    #region Propriedades

    public int EditItem
    {
        get
        {           
            return Convert.ToInt32(grvCor.DataKeys[grvCor.EditIndex].Value);
        }
    }
  
    #endregion

    #region ENUM

    public enum RelatoriosCor
    {
        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.DocDctGraficoAtividadeAvaliativa")]
        DocDctGraficoAtividadeAvaliativa = 244,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoConsolidadoAtividadeAvaliativa")]
        GraficoConsolidadoAtividadeAvaliativa = 252,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular")]
        GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular = 262,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas")]
        GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas = 263,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.AcompanhamentoIndividualdeNotas_ConceitoPorComponente")]
        AcompanhamentoIndividualdeNotas_ConceitoPorComponente = 270                
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaParametroAlerta.js"));
        }
        if (!IsPostBack)
        {
            try
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                {
                    lblMensagem.Text = message;
                }
                carregarRelatorios();
            }
            catch (Exception ex)
             {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros de cor.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    #endregion

    #region Métodos

    private void carregarRelatorios()
    {
        Array populaGrid = Enum.GetValues(typeof(RelatoriosCor));

        DataTable dt = new DataTable();
        dt.Columns.Add("rlt_id");
        dt.Columns.Add("rlt_nome");

        Type objType = typeof(RelatoriosCor);
        FieldInfo[] propriedades = objType.GetFields();
        foreach (FieldInfo objField in propriedades)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                dt.Rows.Add(Convert.ToString(objField.GetRawConstantValue()), GetGlobalResourceObject("Enumerador", attributes[0].Description));
            }
        }

        grvCor.DataSource = dt;
        grvCor.DataBind();        
    }
   
    #endregion

    #region Eventos

    //Método para que seja herdado na tela de cadastro o EditIndex da tela de busca.
    protected void grvCor_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;        
    }    
    
    #endregion  
}
