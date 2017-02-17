using System;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using System.Linq;

namespace AreaAluno.Consulta.Documentos
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades       

        private int tne_id;

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDocumentos(__SessionWEB.__UsuarioWEB.alu_id, __SessionWEB.__UsuarioWEB.mtu_id);
            }
        }

        #endregion Page Life Cycle

        #region Métodos

        #region Documentos

        /// <summary>
        /// Carrega os tipos de áreas documentos ativos
        /// </summary>
        private void CarregarDocumentos(long alu_id, int mtu_id)
        {
            string mtuId = "";

            if (mtu_id > 0)
            {
                mtuId = Convert.ToString(mtu_id);
            }
            MTR_MatriculaTurma matriculaTurma = MTR_MatriculaTurmaBO.GetEntity(new MTR_MatriculaTurma { alu_id = __SessionWEB.__UsuarioWEB.alu_id, mtu_id = mtu_id });
            //TUR_Turma turma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = matriculaTurma.tur_id });
            TUR_TurmaCurriculo curriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(matriculaTurma.tur_id).FirstOrDefault();
            ACA_Curso curso = ACA_CursoBO.GetEntity(new ACA_Curso { cur_id = curriculo.cur_id });

            this.tne_id = curso.tne_id;


            DataTable dtArea = ACA_TipoAreaDocumentoBO.SelecionarAtivos();
            dtArea.Columns.Add("PPP");

            // Adiciona PPP
            DataRow drPPP = dtArea.NewRow();
            drPPP["tad_id"] = -1;
            drPPP["tad_nome"] = GetGlobalResourceObject("Mensagens", "MSG_PLANO_POLITICO_PEDAGOGICO").ToString();
            drPPP["PPP"] = true;
            drPPP["tad_cadastroEscolaBoolean"] = true;
            dtArea.Rows.Add(drPPP);

            rptAreas.DataSource = dtArea;
            rptAreas.DataBind();

            lblSemAreas.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.lblSemAreas.Text"),
                                                     UtilBO.TipoMensagem.Informacao);
            lblSemAreas.Visible = rptAreas.Items.Count <= 0;
        }

        #endregion Documentos

        #endregion Métodos

        #region Eventos


        #region Documentos

        protected void rptAreas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int tad_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tad_id"));
                int esc_id = !Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tad_cadastroEscolaBoolean")) ? 0 : __SessionWEB.__UsuarioWEB.esc_id;
                bool ppp = DataBinder.Eval(e.Item.DataItem, "PPP") != DBNull.Value && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PPP"));

                DataTable dtDocumentos = ACA_ArquivoAreaBO.GetSelectBy_Id_Dre_Escola(tad_id, esc_id, new Guid(), tne_id, ppp, ppp);

                if (dtDocumentos.Rows.Count > 0)
                {
                    Repeater rptDocumentos = (Repeater)e.Item.FindControl("rptDocumentos");
                    rptDocumentos.Visible = true;
                    rptDocumentos.DataSource = dtDocumentos;
                    rptDocumentos.DataBind();
                }
                else
                {
                    e.Item.Visible = false;
                    //Label lblSemDocumentos = (Label)e.Item.FindControl("lblSemDocumentos");
                    //lblSemDocumentos.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.lblSemDocumentos.Text"),
                    //                                              UtilBO.TipoMensagem.Informacao);
                    //lblSemDocumentos.Visible = true;
                }
            }
        }

        protected void rptDocumentos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hplDocumento = (HyperLink)e.Item.FindControl("hplDocumento");
                if (Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "aar_tipoDocumento")) == 1)
                {
                    string arq_id = DataBinder.Eval(e.Item.DataItem, "arq_id").ToString();
                    hplDocumento.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", arq_id);
                }
                else
                {
                    string httpLink = DataBinder.Eval(e.Item.DataItem, "aar_link").ToString();
                    if (!httpLink.Contains("://"))
                        httpLink = "http://" + httpLink;
                    hplDocumento.NavigateUrl = httpLink;
                    hplDocumento.Target = "_blank";
                }
            }
        }

        #endregion Documentos

        #endregion Eventos
    }
}