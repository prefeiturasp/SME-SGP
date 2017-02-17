using System;
using System.Collections.Generic;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

namespace AreaAluno.Consulta.Boletim
{
    using System.ComponentModel.DataAnnotations;

    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.Entities;
    using System.Data;

    public partial class Busca : MotherPageLogado
    {
        #region Metodos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarBoletim(__SessionWEB.__UsuarioWEB.alu_id, __SessionWEB.__UsuarioWEB.mtu_id);
            }
        }

        protected void CarregarBoletim(long alu_id, int mtu_id)
        {
            try
            {
                string mtuId = "";

                if (mtu_id > 0)
                {
                    mtuId = Convert.ToString(mtu_id);
                }

                int tpc_id = 0;
                MTR_MatriculaTurma matriculaTurma = MTR_MatriculaTurmaBO.GetEntity(new MTR_MatriculaTurma { alu_id = __SessionWEB.__UsuarioWEB.alu_id, mtu_id = mtu_id });
                TUR_Turma turma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = matriculaTurma.tur_id });
                DataTable dtAvaliacao = ACA_AvaliacaoBO.GetSelectBy_FormatoAvaliacao(turma.fav_id);

                tpc_id = (from DataRow row in dtAvaliacao.Rows
                          orderby Convert.ToInt32(row["tpc_ordem"])
                          select Convert.ToInt32(row["tpc_id"])).FirstOrDefault();

                List<ACA_AlunoBO.BoletimDadosAluno> lBoletimAluno = ACA_AlunoBO.BuscaBoletimAlunos(Convert.ToString(alu_id), mtuId, tpc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (lBoletimAluno != null && lBoletimAluno.Any())
                {
                    UCAlunoBoletimEscolar.ExibeBoletim(lBoletimAluno.FirstOrDefault());
                }
                else
                {
                    throw new ValidationException("O aluno não possui dados para o boletim.");
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir o Boletim Online do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}