using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class ComboNacionalidade : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged OnSeletedIndexChanged;

        #endregion

        #region Propriedades

        public Guid Valor
        {
            get
            {
                return new Guid(ddlCombo.SelectedValue);
            }
            set
            {                
                ddlCombo.SelectedValue = value.ToString();
            }
        }

        public bool MostrarMensagemSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um país --", Guid.Empty.ToString(), true));
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Traz todos os dados, e guarda em cache o resultado.
        /// </summary>
        public void Carregar()
        {
            ddlCombo.Items.Clear();
            object lista;

            const string chave = "Nacionalidade";
            object cache = Cache[chave];

            if (cache == null)
            {
                // Carrega do banco para guardar em cache.
                DataTable dt = END_PaisBO.GetSelect(Guid.Empty, "", "", 1, false, 1, 1);

                lista = (from DataRow dr in dt.Rows
                         select new
                         {
                             dataTextField = dr["pai_nome"].ToString()
                             ,
                             dataValueField = dr["pai_id"].ToString()
                         }).ToList();

                // Adiciona cache com validade de 1 dia.
                Cache.Insert(chave, lista, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                lista = cache;
            }

            ddlCombo.DataSource = lista;

            MostrarMensagemSelecione = true;

            ddlCombo.DataBind();
        }

        #endregion

        #region Eventos

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ddlCombo.AutoPostBack = OnSeletedIndexChanged != null;
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSeletedIndexChanged != null)
                OnSeletedIndexChanged();
        }

        #endregion
    }
}