using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;

namespace GestaoEscolar.WebControls.AlunoDeficiente
{
    public partial class UCAlunoDeficiente : MotherUserControl
    {
        #region Propriedades

        /// <summary>
        /// Propriedade que configura o alu_id do UC.
        /// </summary>
        public long VS_alu_id
        {
            get
            {
                if (ViewState["VS_alu_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_alu_id"].ToString());
                }
                return -1;
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAlunoMatricula.js"));
            }
        }

        #endregion

        #region Métodos

        public void Inicializar()
        {

            //Tipo Equipamento Deficiente
            chlEquipamentos.DataSource = ACA_TipoEquipamentoDeficienteBO.SelecionaTipoEquipamentoDeficiente();
            chlEquipamentos.DataBind();
            
            fsEquipamentos.Visible = chlEquipamentos.Items.Count > 0;
        }

        public void CarregarAluno(long alu_id)
        {
            if (chlEquipamentos.Items.Count > 0)
            {
                DataTable dtAlunoEquipamentos = ACA_AlunoEquipamentoDeficienteBO.SelecionaPorAluno(alu_id);

                List<int> ted_ids = (from DataRow dr in dtAlunoEquipamentos.Rows
                                     select Convert.ToInt32(dr["ted_id"])).ToList();

                foreach (ListItem item in chlEquipamentos.Items)
                {
                    item.Selected = ted_ids.Exists(p => p == Convert.ToInt32(item.Value));
                }
            }
        }

        /// <summary>
        /// Retorna todos id dos itens selecionado em chlEquipamentos
        /// </summary>
        /// <returns></returns>
        public List<int> RetornaEquipamentosSelcionados()
        {
            List<int> SelectedValues = new List<int>();

            foreach (ListItem item in chlEquipamentos.Items)
            {
                if (item.Selected)
                    SelectedValues.Add(Convert.ToInt32(item.Value));
            }

            return SelectedValues;
        }
        
        public void RetiraSelecao()
        {
            foreach (ListItem item in chlEquipamentos.Items)
            {
                item.Selected = false;
            }
        }

        #endregion
    }
}