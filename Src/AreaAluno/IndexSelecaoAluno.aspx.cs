using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno
{
    public partial class IndexSelecaoAluno : MotherPageLogado
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Pes_Id_Responsavel"] != null)
                {
                    DataTable dtAlunosDoResponsavel = ACA_AlunoResponsavelBO.SelecionaAlunosPorResponsavel(new Guid(Session["Pes_Id_Responsavel"].ToString()));
                    //Session["Pes_Id_Responsavel"] = null;

                    grvAluno.DataSource = dtAlunosDoResponsavel;
                    grvAluno.DataBind();
                }
            }
        }

        protected void grvAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkBtn = (LinkButton)e.Row.FindControl("btnAluno");
                if (lkBtn != null)
                {
                    lkBtn.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvAluno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Encaminhar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvAluno.DataKeys[index].Values["alu_id"]);
                    Guid pes_id = new Guid(grvAluno.DataKeys[index].Values["pes_idAluno"].ToString());

                    __SessionWEB.__UsuarioWEB.alu_id = alu_id;
                    __SessionWEB.__UsuarioWEB.pes_idAluno = pes_id;

                    Response.Redirect("~/Index.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir a Área do Aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion
    }
}