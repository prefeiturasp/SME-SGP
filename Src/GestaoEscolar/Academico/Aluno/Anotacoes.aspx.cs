
using System;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class Academico_Aluno_Anotacoes : MotherPageLogado
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCAlunoAnotacoes.js"));
            }

            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    UCAlunoAnotacoes1._VS_alu_id = PreviousPage.EditItem;
                }
                else if (Session["alu_id_anotacoes"] != null)
                {
                    if (Session["PaginaRetorno_AnotacoesAluno"] != null)
                    {
                        UCAlunoAnotacoes1.VS_PaginaRetorno = Session["PaginaRetorno_AnotacoesAluno"].ToString();
                        Session.Remove("PaginaRetorno_AnotacoesAluno");

                        UCAlunoAnotacoes1.VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                        Session.Remove("DadosPaginaRetorno");

                        UCAlunoAnotacoes1.VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                        Session.Remove("VS_DadosTurmas");

                        UCAlunoAnotacoes1.VS_tud_id = Convert.ToInt64(Session["tud_id_anotacoes"] != null ? Session["tud_id_anotacoes"] : -1);
                        Session.Remove("tud_id_anotacoes");

                        UCAlunoAnotacoes1.VS_mtu_id = Convert.ToInt32(Session["mtu_id_anotacoes"] != null ? Session["mtu_id_anotacoes"] : -1);
                        Session.Remove("mtu_id_anotacoes");
                    }

                    UCAlunoAnotacoes1._VS_alu_id = Convert.ToInt32(Session["alu_id_anotacoes"]);
                    Session.Remove("alu_id_anotacoes");
                }
                else
                {
                    UCAlunoAnotacoes1.CancelaSelect = true;

                    Response.Redirect("Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }

            UCAlunoAnotacoes1.cancelar_Click += UCAlunoAnotacoes1_cancelar_Click;
            UCAlunoAnotacoes1.VS_exibicaoNomePessoa = eExibicaoNomePessoa.NomeSocial;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            UCAlunoAnotacoes1.mensagem = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void UCAlunoAnotacoes1_cancelar_Click()
    {
        if (!string.IsNullOrEmpty(UCAlunoAnotacoes1.VS_PaginaRetorno))
        {
            Session["DadosPaginaRetorno"] = UCAlunoAnotacoes1.VS_DadosPaginaRetorno;
            Session["VS_DadosTurmas"] = UCAlunoAnotacoes1.VS_DadosPaginaRetorno_MinhasTurmas;
            RedirecionarPagina(UCAlunoAnotacoes1.VS_PaginaRetorno);
        }
        else
        {
            RedirecionarPagina("Busca.aspx");
        }

        Session.Remove("alu_id_anotacoes");
        Session.Remove("PaginaRetorno_AnotacoesAluno");
    }
}
