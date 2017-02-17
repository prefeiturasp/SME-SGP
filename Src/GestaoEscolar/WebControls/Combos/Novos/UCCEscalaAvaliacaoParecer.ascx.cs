using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer : MotherUserControl
{
    #region Constantes

    private const string valorSelecione = "-1;-1;-1";
    private const string valorSemParecer = "0;0;0";

    #endregion

    #region Proriedades

    /// <summary>
    /// Retorna o eap_valor selecionado no combo
    /// </summary>
    public string eap_valor
    {
        get
        {
            string valor = "";

            if (ddlCombo.SelectedIndex > 0)
            {
                //TODO: selecionar a ordem pelo eap_id
                return valor;
            }

            return valor;
        }
    }

    /// <summary>
    /// ClientID do combo
    /// </summary>
    public string ClientID_Combo
    {
        get
        {
            return ddlCombo.ClientID;
        }
    }

    /// <summary>
    /// ClientID do validator
    /// </summary>
    public string ClientID_Validator
    {
        get
        {
            return cpvCombo.ClientID;
        }
    }

    /// <summary>
    /// ClientID do label
    /// </summary>
    public string ClientID_Label
    {
        get
        {
            return lblTitulo.ClientID;
        }
    }

    /// <summary>
    /// Coloca na primeira linha a mensagem de selecione um item.
    /// </summary>
    public bool MostrarMensagemSelecione
    {
        get
        {
            if (ViewState["MostrarMensagemSelecione"] != null)
                return Convert.ToBoolean(ViewState["MostrarMensagemSelecione"]);
            return true;
        }
        set
        {
            ViewState["MostrarMensagemSelecione"] = value;
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
                AdicionaAsteriscoObrigatorio(lblTitulo);
            else
                RemoveAsteriscoObrigatorio(lblTitulo);

            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        get
        {
            return ddlCombo.Enabled;
        }
        set
        {
            ddlCombo.Enabled = value;
        }
    }

    /// <summary>
    /// Propriedade que verifica quantos items existem no combo
    /// </summary>
    public int QuantidadeItensCombo
    {
        get
        {
            return ddlCombo.Items.Count;
        }
    }

    /// <summary>
    /// Quando carrega o combo adiciona o item "Sem parecer"
    /// </summary>
    public bool AdicionaValorSemParecer
    {
        get
        {
            if (ViewState["SemParecer"] != null)
                return Convert.ToBoolean(ViewState["SemParecer"]);
            return false;
        }
        set
        {
            ViewState["SemParecer"] = value;
        }
    }

    /// <summary>
    /// Quando carrega o combo adiciona o item "Sem parecer"
    /// </summary>
    public int VS_esa_id
    {
        get
        {
            if (ViewState["VS_esa_id"] != null)
                return Convert.ToInt32(ViewState["VS_esa_id"]);
            return 0;
        }
        set
        {
            ViewState["VS_esa_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade que seta o SelectedIndex do Combo.       
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlCombo.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Seta um titulo diferente do padrão para o combo
    /// </summary>
    public string Titulo
    {
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
        get
        {
            return lblTitulo.Text;
        }
    }

    /// <summary>
    /// propriedade que seta se o titulo será visível.
    /// </summary>
    public bool mostraTitulo
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// valor[0] = esa_id
    /// valor[1] = eap_id
    /// valor[2] = eap_ordem
    /// </summary>
    public int[] Valor
    {
        get
        {
            string[] s = ddlCombo.SelectedValue.Split(';');

            if (s.Length == 3)
                return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]) };

            return new[] { -1, -1, -1 };
        }
        set
        {
            string s;
            if (value.Length == 3)
                s = value[0] + ";" + value[1] + ";" + value[2];
            else
                s = valorSelecione;

            if (ddlCombo.Items.FindByValue(s) != null)
                ddlCombo.SelectedValue = s;
        }
    }

    /// <summary>
    /// Propriedade visible da label do nome do combo
    /// </summary>
    public bool Visible_Label
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta o Width do combo.   
    /// </summary>
    public Int32 Width_Combo
    {
        set
        {
            ddlCombo.Width = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Adiciona item "Sem Parecer" no combo
    /// </summary>
    private void AdicionaSemParecer()
    {
        if (AdicionaValorSemParecer && (ddlCombo.Items.FindByValue(valorSemParecer) == null))
            ddlCombo.Items.Insert(0, new ListItem("Sem " + lblTitulo.Text.Replace("*", "").ToLower(), VS_esa_id.ToString() + ";0;0", true));

        ddlCombo.AppendDataBoundItems = AdicionaValorSemParecer;
    }

    /// <summary>
    /// Carrega a mensagem de selecione de acordo com o parâmetro
    /// </summary>
    public void CarregarMensagemSelecione()
    {
        if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + lblTitulo.Text.Replace("*", "").ToLower() + " --", valorSelecione, true));

        ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
    }

    /// <summary>
    /// Carrega o combo
    /// </summary>
    /// <param name="dataSource">Dados a serem inseridos no combo</param>
    public void CarregarCombo(object dataSource)
    {
        try
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = dataSource;

            AdicionaSemParecer();
            CarregarMensagemSelecione();
            ddlCombo.DataBind();
        }
        catch (Exception)
        {
            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Carrega todos os pareceres não excluídos logicamente
    /// filtrados por esacala de avaliação
    /// </summary>
    public void CarregarPorEscala(int esa_id)
    {
        VS_esa_id = esa_id;
        CarregarCombo(ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id)
                      .Select(p => new
                      {
                          descricao = p.eap_valor + " - " + p.eap_descricao,
                          esa_eap_ordem = p.esa_id + ";" + p.eap_id + ";" + p.eap_ordem
                      }));
    }

    #endregion

    #region Eventos

    #region Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                            lblTitulo.Text.EndsWith(" *");

        cpvCombo.ValueToCompare = valorSelecione;

        Obrigatorio = obrigatorio;
    }

    #endregion

    #endregion
}