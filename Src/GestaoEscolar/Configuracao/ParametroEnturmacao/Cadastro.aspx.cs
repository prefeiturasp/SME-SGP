using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL.GestaoEscolar;
using MSTech.GestaoEscolar.Entities.GestaoEscolar;

namespace GestaoEscolar.Configuracao.ParametroEnturmacao
{
    public partial class Cadastro : MotherPageLogado
    {
        private int _VS_tem_id
        {
            get
            {
                if (ViewState["_VS_tem_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_tem_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["_VS_tem_id"] = value;
            }
        }

        private int _VS_fav_id
        {
            get
            {
                if (ViewState["_VS_fav_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_fav_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["_VS_fav_id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                Carregar();
            }
        }

        private void Carregar()
        {
            _VS_tem_id = 1;
            _VS_fav_id = 998;

            STM_Temporario entity = new STM_Temporario();
            entity.tem_id = _VS_tem_id;
            entity.fav_id = _VS_fav_id;
            STM_TemporarioBO.GetEntity(entity);

            if(!entity.IsNew)
            {
                ComboTurma.SelectedValue = entity.aux1;
                DropDownList1.SelectedValue = entity.aux2;
                RadioButtonList1.SelectedValue = entity.aux3;
                RadioButtonList2.SelectedValue = entity.aux4;
                if (entity.aux5 != "" && entity.aux5 != null)
                {
                    TextBox1.Visible = true;
                    TextBox1.Text = entity.aux5;
                }
                if (entity.aux6 != "" && entity.aux6 != null)
                {
                    TextBox2.Text = entity.aux6;
                    TextBox2.Visible = true;
                }
                if (entity.aux7 != "" && entity.aux7 != null)
                {
                    TextBox3.Text = entity.aux7;
                    TextBox3.Visible = true;
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                STM_Temporario entity = new STM_Temporario();
                entity.tem_id = _VS_tem_id;
                entity.fav_id = _VS_fav_id;
                STM_TemporarioBO.GetEntity(entity);
                entity.aux1 = ComboTurma.SelectedValue;
                entity.aux2 = DropDownList1.SelectedValue;
                entity.aux3 = RadioButtonList1.SelectedValue;
                entity.aux4 = RadioButtonList2.SelectedValue;
                entity.aux5 = TextBox1.Text;
                entity.aux6 = TextBox2.Text;
                entity.aux7 = TextBox3.Text;

                STM_TemporarioBO.Save(entity);
                
                lblMessage.Text = UtilBO.GetErroMessage("Configuração salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a configuração.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedIndex == 1)
            {
                TextBox3.Visible = 
                rfvTextBox3.Enabled = true;
            }
            else
            {
                TextBox3.Visible = 
                rfvTextBox3.Enabled = false;
                TextBox3.Text = "";
            }
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList2.SelectedIndex == 1)
            {
                TextBox1.Visible =
                TextBox2.Visible = 
                rfvTextBox2.Enabled =
                rfvTextBox1.Enabled = true;
            }
            else
            {
                TextBox1.Visible =
                TextBox2.Visible =
                rfvTextBox2.Enabled =
                rfvTextBox1.Enabled = false;
                TextBox1.Text = 
                TextBox2.Text = "";
            }
        }
    }
}