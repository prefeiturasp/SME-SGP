using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebControls_Combos_UCComboMes : MotherUserControl
{

    #region [Propriedades]

    /// <summary>
    /// Atribui valores para o combo mes inicio
    /// </summary>
    public bool exibirMesFim
    {
        set
        {
            divFinal.Visible = value;
        }

    }

    /// <summary>
    /// Informa se vai dar postback na seleção de mes
    /// </summary>
    public bool autoPostBack
    {
        set
        {
            ddlMesInicio.AutoPostBack = value;
            ddlMesFim.AutoPostBack = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo mes inicio
    /// </summary>
    public string mesInicio
    {
        get
        {
            if (!(Convert.ToInt32(ddlMesInicio.SelectedValue) <= 0))
                return ddlMesInicio.SelectedValue;
            else
                return null;
        }
        set
        {
            ddlMesInicio.SelectedValue = value;
        }
    }

    /// <summary>
    /// mes inicio
    /// </summary>
    public string mesInicioTexto
    {
        get
        {
            return ddlMesInicio.SelectedItem.Text;
        }
    }

    /// <summary>
    /// Atribui valores para o combo mes fim
    /// </summary>
    public string mesFim
    {
        get
        {
            if (!(Convert.ToInt32(ddlMesFim.SelectedValue) <= 0))
                return ddlMesFim.SelectedValue;
            else
                return null;
        }
        set
        {
            ddlMesFim.SelectedValue = value;
        }
    }

    /// <summary>
    /// mes fim
    /// </summary>
    public string mesFimTexto
    {
        get
        {
            return ddlMesFim.SelectedItem.Text;
        }
    }


    /// <summary>
    /// Atribui valores para o label mes inicio
    /// </summary>
    public string lblMesInicial
    {
        get
        {
            return lblMesInicio.Text;
        }
        set
        {
            lblMesInicio.Text = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label mes fim
    /// </summary>
    public string lblMesFinal
    {
        get
        {
            return lblMesFim.Text;
        }
        set
        {
            lblMesFim.Text = value;
        }
    }

    /// <summary>
    /// Informa se o mês inicial é obrigatório
    /// </summary>
    public bool obrigatorioMesInicio
    {
        set
        {
            cpvComboInicial.Visible = value;
        }
    }

    /// <summary>
    /// Informa se o mês final é obrigatório
    /// </summary>
    public bool obrigatorioMesFim
    {
        set
        {
            cpvComboFinal.Visible = value;
        }
    }

    /// <summary>
    /// Mensagem do mês início obrigatório
    /// </summary>
    public string ErrorMessageInicial
    {
        set
        {
            cpvComboInicial.ErrorMessage = value;
        }
    }

    /// <summary>
    /// Mensagem do mês final obrigatório
    /// </summary>
    public string ErrorMessageFim
    {
        set
        {
            cpvComboFinal.ErrorMessage = value;
        }
    }
    #endregion [Propriedades]

    #region [Eventos]

    /// <summary>
    /// Seta um titulo diferente do padrão para o combo
    /// </summary>
    public string Titulo
    {
        set
        {
            
            lblMesFim.Text = value;
            cpvComboFinal.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
        get
        {
            return lblMesFim.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblMesFim);
                AdicionaAsteriscoObrigatorio(lblMesInicio);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblMesFim);
                RemoveAsteriscoObrigatorio(lblMesInicio);
            }
            cpvComboFinal.Visible = value;
            cpvComboInicial.Visible = value;
        }
    }

    /// <summary>
    /// Seta um titulo diferente do padrão para o combo
    /// </summary>
    public string _Titulo
    {
        set
        {

            lblMesInicio.Text = value;
            cpvComboInicial.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
        get
        {
            return lblMesInicio.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
        }
    }

 

    protected void ddlMesInicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i;

        if (divFinal.Visible)
        {
            for (i = 1; i <= 12; i++)
            {
                if (i < Convert.ToInt32(ddlMesInicio.SelectedValue))
                    ddlMesFim.Items[i].Enabled = false;
                else
                    ddlMesFim.Items[i].Enabled = true;
            }
        }
    }

    #endregion [Eventos]

}