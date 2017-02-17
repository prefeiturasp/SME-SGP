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

public partial class Configuracao_ParametroFaixaRelatorio : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(grvRelatorioFaixa.DataKeys[grvRelatorioFaixa.EditIndex].Value);
        }
    }
  
    #endregion

    #region ENUM

    public enum RelatoriosFaixa
    {
        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.DocDctGraficoAtividadeAvaliativa")]
        DocDctGraficoAtividadeAvaliativa = 244,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoConsolidadoAtividadeAvaliativa")]
        GraficoConsolidadoAtividadeAvaliativa = 252,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular")]
        GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular = 262,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas")]
        GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas = 263,
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
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parâmetros por faixa.", UtilBO.TipoMensagem.Erro);
            }
        }        
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Carrega os relatórios faixa
    /// </summary>
    private void carregarRelatorios()
    {
        Array populaGrid = Enum.GetValues(typeof(RelatoriosFaixa));

        DataTable dt = new DataTable();
        dt.Columns.Add("rlt_id");
        dt.Columns.Add("rlt_nome");

        Type objType = typeof(RelatoriosFaixa);
        FieldInfo[] propriedades = objType.GetFields();
        foreach (FieldInfo objField in propriedades)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                dt.Rows.Add(Convert.ToString(objField.GetRawConstantValue()), GetGlobalResourceObject("Enumerador", attributes[0].Description));
            }
        }
        grvRelatorioFaixa.DataSource = dt;
        grvRelatorioFaixa.DataBind();        
    }
   
    #endregion

    #region Eventos

    //Método para que seja herdado na tela de cadastro o EditIndex da tela de busca.
    protected void grvRelatorioFaixa_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;        
    }    
    
    #endregion  
}
