using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboAvisoTextoGeralCampoAuxiliar : MotherUserControl
    {
        #region Propriedades Tipo aviso

        //================UCCOMBOTIPOAVISOTEXTO=================================
        /// <summary>
        /// Valor selecionado no combo de tipo(atg_id)
        /// </summary>
        public int ValorComboTipo
        {
            get
            {
                return UCComboTipoAvisoTextoGeral1.ValorCombo;
            }

            set
            {
                UCComboTipoAvisoTextoGeral1.ValorCombo = value;
            }
        }

        /// <summary>
        /// Propriedade que seta a label e a validação do combo
        /// </summary>
        public bool ObrigatorioTipo
        {
            set
            {
                    UCComboTipoAvisoTextoGeral1.Obrigatorio = value;
            }
        }

        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroupTipo
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// </summary>
        public bool MostrarTipoCabecalho
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.MostrarTipoCabecalho = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// </summary>
        public bool MostrarTipoCabecalhoRelatorio
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.MostrarTipoCabecalhoRelatorio = value;
            }
        }

        /// <summary>
        /// Configura validação do combo.
        /// </summary>
        public bool MostrarTipoDeclaracao
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.MostrarTipoDeclaracao = value;
            }
        }

        /// <summary>
        /// Configura título do combo de tipo.
        /// </summary>
        public string TituloComboTipo
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.TituloCombo = value;
            }
        }

        /// <summary>
        /// Habilita/Desabilita o combo tipo.
        /// </summary>
        public bool EnabledComboTipo
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.Enabled = value;
            }
        }
         
        /// <summary>
        /// Seta o foco no combo de campos auxiliares.
        /// </summary>
        public bool MostraComboTipoAviso
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.MostraCombo = value;
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
                UCComboTipoAvisoTextoGeral1.MostrarMessageSelecioneTipo = value;
            }
        }

        /// <summary>
        /// Configura index do combo de tipo aviso.
        /// </summary>
        public int indexComboTipoAviso
        {
            set
            {
                UCComboTipoAvisoTextoGeral1.indexComboTipoAviso = value;
            }
            get
            {
                return UCComboTipoAvisoTextoGeral1.indexComboTipoAviso;
            }
        }
        //==============================END UCCOMBOTIPOAVISOTEXTO

        #endregion
    }
}