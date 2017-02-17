using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboMotivoBaixaFrequencia : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public SelectedIndexChanged OnSelectedIndexChanged;

        #endregion Delegates

        #region PROPRIEDADES

        /// <summary>
        /// Retorna e seta o valor selecionado no combo
        /// </summary>
        public int Valor
        {
            get
            {
                return Convert.ToInt32(string.IsNullOrEmpty(hdnValor.Value) ? "-1" : hdnValor.Value);
            }
            set
            {
                hdnValor.Value = dtMotivosBaixaFreq.Select("mbf_id = '" + value.ToString() + "'").Length > 0 ? value.ToString() : "-1";
                CarregarCache();
                btnExpandir.Text = dtMotivosBaixaFreq.Select("mbf_id = '" + value.ToString() + "'").Length > 0 ? 
                                   dtMotivosBaixaFreq.Select("mbf_id = '" + value.ToString() + "'")[0]["siglaDescricao"].ToString() :
                                   "   -- Selecione um motivo de baixa frequência --   ";
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return btnExpandir.Text;
            }
        }

        public bool Visible
        {
            set
            {
                divCombo.Visible = value;
            }
        }

        public bool Enabled
        {
            set
            {
                btnExpandir.Enabled = tvMotivosBaixaFreq.Enabled = value;
            }
        }

        DataTable dtMotivosBaixaFreq;

        #endregion

        #region METODOS

        /// <summary>
        /// Seta o foco no combo    
        /// </summary>
        public void SetarFoco()
        {
            btnExpandir.Focus();
        }

        /// <summary>
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// </summary>
        public void CarregarCache()
        {
            dtMotivosBaixaFreq = ACA_MotivoBaixaFrequenciaBO.SelecionarAtivos(ApplicationWEB.AppMinutosCacheCurto);
        }

        private void CarregaTreeView(int mbf_idPai, TreeNode treeNode)
        {
            foreach (DataRow row in dtMotivosBaixaFreq.Select("mbf_idPai = '" + mbf_idPai + "'"))
            {
                TreeNode item = new TreeNode
                {
                    Text = row["siglaDescricao"].ToString(),
                    Value = row["mbf_id"].ToString(),
                    SelectAction = mbf_idPai == 0 ? TreeNodeSelectAction.Expand : TreeNodeSelectAction.Select,
                    ImageUrl = ""
                };
                if (mbf_idPai == 0)
                {
                    tvMotivosBaixaFreq.Nodes.Add(item);
                    CarregaTreeView(Convert.ToInt32(item.Value), item);
                }
                else
                    treeNode.ChildNodes.Add(item);
            }
        }

        #endregion

        #region EVENTOS

        protected void Page_Load(object sender, EventArgs e)
        {
            btnExpandir.CssClass = "comboHierarquicoButton";
            tvMotivosBaixaFreq.CssClass = "comboHierarquicoItens";
        }

        protected void btnExpandir_Click(object sender, EventArgs e)
        {
            if (tvMotivosBaixaFreq.Visible)
                tvMotivosBaixaFreq.Visible = false;
            else
            {
                CarregarCache();
                tvMotivosBaixaFreq.Visible = true;
                tvMotivosBaixaFreq.Nodes.Clear();
                CarregaTreeView(0, null);
            }
        }

        protected void tvMotivosBaixaFreq_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (!tvMotivosBaixaFreq.SelectedNode.Value.Equals("0"))
            {
                hdnValor.Value = tvMotivosBaixaFreq.SelectedNode.Value;
                btnExpandir.Text = tvMotivosBaixaFreq.SelectedNode.Text;
                tvMotivosBaixaFreq.Visible = false;

                if (OnSelectedIndexChanged != null)
                    OnSelectedIndexChanged();
            }
        }

        #endregion
    }
}