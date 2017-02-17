using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.NivelAprendizado
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Id nível aprendizado.
        /// </summary>
        private int VS_nap_id
        {
            get
            {
                if (ViewState["VS_nap_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_nap_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_nap_id"] = value;
            }
        }

        /// <summary>
        /// Id curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                if (ViewState["VS_cur_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_cur_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// Id curriculo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                if (ViewState["VS_crr_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_crr_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// Id período do curso (grupamento de ensino).
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                if (ViewState["VS_crp_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_crp_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Carrega os combos.
                UCComboCursoCurriculo1.CarregarCursoCurriculoSituacao(1);
                UCComboCurriculoPeriodo1._Combo.Enabled = false;
                UCComboCurriculoPeriodo1.CancelSelect = true;
                

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    VS_nap_id = PreviousPage.EditItem_Nap_id;
                    LoadFromEntity(VS_nap_id);
                    Page.Form.DefaultFocus = txtDescricao.ClientID;
                }

                Page.Form.DefaultButton = bntSalvar.UniqueID;
            }

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
        }

        private void LoadFromEntity(int nap_id)
        {
            ORC_NivelAprendizado nivel = new ORC_NivelAprendizado
            {
                nap_id = nap_id
            };
            ORC_NivelAprendizadoBO.GetEntity(nivel);

            VS_cur_id = nivel.cur_id;
            VS_crr_id = nivel.crr_id;
            VS_crp_id = nivel.crp_id;            

            txtDescricao.Text = nivel.nap_descricao;
            txtSigla.Text = nivel.nap_sigla;

            UCComboCursoCurriculo1.Valor = new[] { VS_cur_id, VS_crr_id };

            UCComboCurriculoPeriodo1.CancelSelect = false;
            UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], -1, -1);
            UCComboCurriculoPeriodo1.Valor = new[] { VS_cur_id, VS_crr_id, VS_crp_id };

            UCComboCursoCurriculo1.PermiteEditar = false;
            UCComboCurriculoPeriodo1.PermiteEditar = false;

            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/NivelAprendizado/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string desc = string.Empty;
                string sigla = string.Empty;

                desc = txtDescricao.Text;
                sigla = txtSigla.Text;

                ORC_NivelAprendizado nivel = new ORC_NivelAprendizado
                {
                    nap_id = VS_nap_id,
                    cur_id = VS_cur_id > 0 ? VS_cur_id : UCComboCursoCurriculo1.Valor[0],
                    crr_id = VS_crr_id > 0 ? VS_crr_id : UCComboCursoCurriculo1.Valor[1],
                    crp_id = VS_crp_id > 0 ? VS_crp_id : UCComboCurriculoPeriodo1.Valor[2],
                    nap_descricao = desc,
                    nap_sigla = sigla,
                    nap_situacao = 1,
                    IsNew = (VS_nap_id < 0)
                };

                if (ORC_NivelAprendizadoBO.Salvar(nivel, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    ApplicationWEB._GravaLogSistema((VS_nap_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert), "nap_id: " + nivel.nap_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(string.Format("Nível de aprendizado {0} com sucesso.", (VS_nap_id > 0 ? "alterado" : "incluído")), UtilBO.TipoMensagem.Sucesso);
                    Response.Redirect("~/Academico/NivelAprendizado/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (DuplicateNameException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                updMensagem.Update();
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                updMensagem.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar nível de aprendizado.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        #endregion

        #region Delegates

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
                UCComboCurriculoPeriodo1.PermiteEditar = true;

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1.CancelSelect = false;
                UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], -1, -1);

                UCComboCurriculoPeriodo1._Combo.Enabled = UCComboCursoCurriculo1.Valor[0] > 0;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}