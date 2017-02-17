using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoAvisoTextoGeral : MotherUserControl
    {
        #region PROPRIEDADADES

        /// <summary>
        /// Atribui estados para o combo
        /// </summary>
        public DropDownList Combo
        {
            get
            {
                return ddlAviso;
            }
            set
            {
                ddlAviso = value;
            }
        }

        /// <summary>
        /// Atribui valores para o label
        /// </summary>
        public Label Label
        {
            get
            {
                return lblTipoAviso;
            }
            set
            {
                lblTipoAviso = value;
            }
        }

        public int indexComboTipoAviso
        {
            set
            {
                ddlAviso.SelectedIndex = value;
            }
            get
            {
                return ddlAviso.SelectedIndex;
            }
        }

        /// <summary>
        /// Valor selecionado no combo de tipo(atg_id)
        /// </summary>
        public int ValorCombo
        {
            get
            {
                return Convert.ToInt32(ddlAviso.SelectedValue);
            }

            set
            {
                ddlAviso.SelectedValue = value.ToString();
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
                    AdicionaAsteriscoObrigatorio(lblTipoAviso);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTipoAviso);

                }

                cpvTipo.Visible = value;
            }
        }


        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvTipo.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// Por padrao, ele listara o cabecalho
        /// </summary>
        public bool MostrarTipoCabecalho
        {
            set
            {
                ddlAviso.Items.Insert(ddlAviso.Items.Count, new ListItem("Cabecalho", "4"));
                ddlAviso.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// Por padrao, ele listara o cabecalho
        /// </summary>
        public bool MostrarTipoDeclaracao
        {
            set
            {
                ddlAviso.Items.Insert(ddlAviso.Items.Count, new ListItem("Declaração por aluno", "5"));
                ddlAviso.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// Por padrao, ele listara o cabecalho relatorio
        /// </summary>
        public bool MostrarTipoCabecalhoRelatorio
        {
            set
            {
                ddlAviso.Items.Insert(ddlAviso.Items.Count, new ListItem("CabecalhoRelatorio", "6"));
                ddlAviso.AppendDataBoundItems = value;
            }
        }

        /// <summary>
        /// Configura título do combo de tipo.
        /// </summary>
        public string TituloCombo
        {
            set
            {
                lblTipoAviso.Text = value;
                cpvTipo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// ClientID do combo tipo.
        /// </summary>
        public string ComboTipo_ClientID
        {
            get
            {
                return ddlAviso.ClientID;
            }
        }

        /// <summary>
        /// Habilita/Desabilita o combo tipo.
        /// </summary>
        public bool Enabled
        {
            set
            {
                ddlAviso.Enabled = value;
            }
        }

        /// <summary>
        /// Habilita/Desabilita o AutoPostBack
        /// </summary>
        public bool EnabledAutoPostBack
        {
            set
            {
                ddlAviso.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Seta o foco no combo de campos auxiliares.
        /// </summary>
        public bool MostraCombo
        {
            set
            {
                ddlAviso.Visible = value;
                lblTipoAviso.Visible = value;
            }
        }

        /// <summary>
        /// Adciona e remove a mensagem "Selecione um Tipo de aviso e texto" do dropdownlist.
        /// Por padrão é false e a mensagem não é exibida.
        /// </summary>
        public bool MostrarMessageSelecioneTipo
        {
            set
            {
                string textoTipo = string.Empty;

                if ((value) && (ddlAviso.Items.FindByValue("-1") == null))
                    if (lblTipoAviso.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                        textoTipo = lblTipoAviso.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                    else if (lblTipoAviso.Text.EndsWith("*"))
                        textoTipo = lblTipoAviso.Text.Replace("*", "");
                    else
                        textoTipo = lblTipoAviso.Text;
                ddlAviso.Items.Insert(0, new ListItem
                    (
                        String.Concat("-- Selecione um ", textoTipo.ToLower(), " --")
                        , "-1"
                        , true
                    ));
                ddlAviso.AppendDataBoundItems = value;
            }
        }

        #endregion

        #region Delegates

        public delegate void OnSelectedIndexChangedTA();

        public event OnSelectedIndexChangedTA IndexChangedTA;

        #endregion

        #region Page Lyfe Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlAviso.AutoPostBack |= (IndexChangedTA != null);
        }

        #endregion

        #region Eventos

        protected void ddlAviso_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTAIndexChanged();
        }

        #endregion

        #region Métodos

        protected void ddlTAIndexChanged()
        {
            if (IndexChangedTA != null)
                IndexChangedTA();
        }
        #endregion
    }

}