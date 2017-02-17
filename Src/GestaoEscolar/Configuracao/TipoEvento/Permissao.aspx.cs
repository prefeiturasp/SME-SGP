using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.TipoEvento
{
    public partial class Permissao : MotherPageLogado
    {

        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tev_id, vindo da PreviousPage
        /// </summary>
        private int VS_tev_id
        {
            get
            {
                if (ViewState["VS_tev_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tev_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tev_id"] = value;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VS_tev_id = PreviousPage.EditItem;
                CarregaGrupos(VS_tev_id);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os Grupos de usuários do Core. Verifica para quais grupos o tipo de evento estará visível
        /// </summary>
        /// <param name="tev_id">ID do tipo de evento</param>
        private void CarregaGrupos(int tev_id)
        {
            lblMensagem.Text = UtilBO.GetErroMessage("Selecione os grupos em que o tipo de evento estará visível.", UtilBO.TipoMensagem.Informacao);

            cblGrupos.DataSource = SYS_GrupoBO.ConsultarPorSistema(__SessionWEB.__UsuarioWEB.Grupo.sis_id);
            cblGrupos.DataValueField = "gru_id";
            cblGrupos.DataTextField = "gru_nome";
            cblGrupos.DataBind();

            foreach (ACA_TipoEventoGrupo item in ACA_TipoEventoGrupoBO.SelectByTipoEvento(tev_id))
            {
                ListItem list = cblGrupos.Items.FindByValue(item.gru_id.ToString());

                if (list != null)
                    list.Selected = true;

            }
        }

        #endregion

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ACA_TipoEventoGrupoBO.DeleteByTipoEvento(VS_tev_id);

                foreach (ListItem item in cblGrupos.Items)
                {
                    if (item.Selected)
                    {
                        ACA_TipoEventoGrupoBO.Save(new ACA_TipoEventoGrupo
                        {
                            tev_id = VS_tev_id,
                            gru_id = new Guid(item.Value)
                        });
                    }
                }

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Permissões do tipo de evento alteradas com sucesso.", UtilBO.TipoMensagem.Sucesso);
                RedirecionarPagina("Busca.aspx");

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de evento.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("Busca.aspx");
        }

        #endregion

    }
}