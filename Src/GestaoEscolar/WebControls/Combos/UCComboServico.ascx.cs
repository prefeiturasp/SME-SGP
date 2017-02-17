using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboServico : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna valor selecionado no combo
        /// </summary>
        public Int32 Valor
        {
            get
            {
                return Convert.ToInt32(ddlServico.SelectedValue);
            }
        }

        /// <summary>
        /// Retorna texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return ddlServico.SelectedItem.Text;
            }
        }

        #endregion

        #region Eventos

        protected void ddlServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion

        #region Métodos

        public void Carregar()
        {
            ddlServico.Items.Clear();
            ddlServico.DataSource = SYS_ServicosBO.SelecionaServicos();
            ddlServico.Items.Insert(0, new ListItem("-- Selecione um serviço --", "-1", true));
            ddlServico.DataBind();
        }

        #endregion
    }
}