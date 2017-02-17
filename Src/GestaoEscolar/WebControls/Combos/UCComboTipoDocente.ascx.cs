using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoDocente : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// </summary>
        public int Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                    return -1;

                return Convert.ToInt32(ddlCombo.SelectedValue);
            }
            set
            {
                ddlCombo.SelectedValue = value.ToString();
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega o combo a partir do enumerador.
        /// </summary>
        /// <typeparam name="T">Tipo do dado do Enumerador</typeparam>
        /// <param name="cbo">Combo</param>
        public static void CarregarComboEnum<T>(DropDownList cbo)
        {
            cbo.Items.Clear();
            Type objType = typeof(T);
            FieldInfo[] propriedades = objType.GetFields();

            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    cbo.Items.Add(new ListItem(attributes[0].Description, objField.GetRawConstantValue().ToString()));
            }

            // Seleciona por default o Titular
            byte posicao_titular = (byte)EnumTipoDocente.Titular;
            ListItem itemCombo = cbo.Items.FindByValue(posicao_titular.ToString());

            if (itemCombo != null)
                cbo.SelectedValue = itemCombo.Value;


            if (cbo.Items.Count == 0)
            {
                cbo.Items.Insert(0, new ListItem("-- Selecione um tipo de docente --", "-1", true));
            }
        }

        public void CarregarComboTipoDocente()
        {
            CarregarComboEnum<EnumTipoDocente>(ddlCombo);
            ddlCombo.DataBind();
        }

        #endregion

        #region Eventos

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
            {
                IndexChanged();
            }
        }

        #endregion

    }
}