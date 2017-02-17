using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;

public partial class Documentos_BoletimEscolarAluno_BoletimEscolarAluno : MotherPageLogado
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String sMensagemLog = "";

            try
            {
                Int64 alu_id = ACA_AlunoBO.SelectAlunoby_pes_id(__SessionWEB.__UsuarioWEB.Usuario.pes_id);
                if (alu_id == 0)
                {
                    sMensagemLog = "Usuário não autorizado a exibir Boletim: usu_id: " + __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString(); 
                    UCAlunoBoletimEscolar.ExibeBoletim = false;
                    
                    throw new ValidationException("Usuário não autorizado a exibir o boletim escolar de um aluno.");
                }

                DataTable dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);
                UCAlunoBoletimEscolar.CarregarBoletim(alu_id, dtCurriculo);

                sMensagemLog = "Boletim exibido do aluno: alu_id: " + alu_id.ToString();                   
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao exibir boletim escolar do aluno", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Query, sMensagemLog);
            }
        }
    }    
}
