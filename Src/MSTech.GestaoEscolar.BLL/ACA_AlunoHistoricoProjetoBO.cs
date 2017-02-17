/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System;
    using MSTech.Validation.Exceptions;
    using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.Data.Common;
	
	/// <summary>
	/// Description: ACA_AlunoHistoricoProjeto Business Object. 
	/// </summary>
	public class ACA_AlunoHistoricoProjetoBO : BusinessBase<ACA_AlunoHistoricoProjetoDAO, ACA_AlunoHistoricoProjeto>
	{
        /// <summary>
        /// Seleciona os projetos do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns></returns>
        public static DataTable SelectBy_Aluno(long alu_id)
        {
            ACA_AlunoHistoricoProjetoDAO dao = new ACA_AlunoHistoricoProjetoDAO();
            return dao.SelectBy_Aluno(alu_id);
        }

        /// <summary>
        /// Marca o projeto do aluno como excluido
        /// </summary>
        /// <param name="ahp"></param>
        /// <returns></returns>
        public static bool Excluir(ACA_AlunoHistoricoProjeto ahp)
        {
            ACA_AlunoHistoricoProjetoDAO dao = new ACA_AlunoHistoricoProjetoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (VerificaHistorico(ahp.alu_id, ahp.ahp_id))
                    throw new ValidationException((string)CustomResource.GetGlobalResourceObject("BLL", "ACA_AlunoHistoricoProjetoBO.ProjetoEmUsoHistorico"));

                return Save(ahp, dao._Banco);
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se o ahp_id está sendo usado em um histórico ativo
        /// </summary>
        /// <param name="ahp_id"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool VerificaHistorico(Int64 alu_id, int ahp_id, TalkDBTransaction banco)
        {
            ACA_AlunoHistoricoProjetoDAO dao = new ACA_AlunoHistoricoProjetoDAO();
            return dao.VerificaHistorico(alu_id, ahp_id, banco);
        }

        /// <summary>
        /// Verifica se o ahp_id está sendo usado em um histórico ativo
        /// </summary>
        /// <param name="ahp_id"></param>
        /// <returns></returns>
        public static bool VerificaHistorico(Int64 alu_id, int ahp_id)
        {
            ACA_AlunoHistoricoProjetoDAO dao = new ACA_AlunoHistoricoProjetoDAO();
            return VerificaHistorico(alu_id, ahp_id, dao._Banco);
        }
    }
}