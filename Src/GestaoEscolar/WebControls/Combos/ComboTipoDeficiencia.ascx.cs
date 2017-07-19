using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class ComboTipoDeficiencia : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged OnSeletedIndexChanged;

        #endregion Delegates

        #region Propriedades

        public bool Obrigatorio
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTitulo);
                }

                cpvCombo.Visible = value;
            }
        }

        public bool ExibeDeficienciaMultipla
        {
            set
            {
                foreach (ListItem def in ddlCombo.Items)
                {
                    if (CFG_DeficienciaFIlhaBO.SelectFilhaBy_Deficiencia(new Guid(def.Value)).Count > 0)
                    {
                        ddlCombo.Items.Remove(def);
                    }
                }
            }
        }

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

        /// <summary>
        /// Texto do título ao combo.
        /// </summary>
        public string Titulo
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedIndex do Combo.
        /// </summary>
        public int Combo_SelectedIndex
        {
            get
            {
                return ddlCombo.SelectedIndex;
            }
            set
            {
                ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
            }
        }

        public bool MostrarMensagemSelecione
        {
            /// + GestaoEscolarUtilBO.nomePadraoTipoDeficiencia(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --"
            set
            {
                if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoTipoDeficiencia
                        (__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() +
                        " --", Guid.Empty.ToString(), true));
            }
        }

        /// <summary>
        /// Atribui valores para o combo
        /// </summary>
        public DropDownList _Combo
        {
            get
            {
                return ddlCombo;
            }
            set
            {
                ddlCombo = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Traz todos os dados, e guarda em cache o resultado.
        /// </summary>
        public void Carregar()
        {
            ddlCombo.Items.Clear();
            object lista;

            const string chave = "TipoDeficiencia";
            object cache = Cache[chave];

            if (cache == null)
            {
                // Carrega do banco para guardar em cache.
                DataTable dt = PES_TipoDeficienciaBO.GetSelect(Guid.Empty, "", 1, false, 1, 1);

                lista = (from DataRow dr in dt.Rows
                         select new
                         {
                             dataTextField = dr["tde_nome"].ToString()
                             ,
                             dataValueField = dr["tde_id"].ToString()
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

            try
            {
                ddlCombo.DataBind();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                lblAviso.Visible = true;
                ApplicationWEB._GravaErro(ex);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion Métodos

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

        #endregion Eventos
    }
}