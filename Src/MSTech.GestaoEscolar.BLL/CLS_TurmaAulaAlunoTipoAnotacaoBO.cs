/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;    
    using System.Collections.Generic;
    using System;
	
	/// <summary>
	/// Description: CLS_TurmaAulaAlunoTipoAnotacao Business Object. 
	/// </summary>
	public class CLS_TurmaAulaAlunoTipoAnotacaoBO : BusinessBase<CLS_TurmaAulaAlunoTipoAnotacaoDAO, CLS_TurmaAulaAlunoTipoAnotacao>
	{
				
        /// <summary>
        /// Busca os Tipo Anotações dos alunos da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaAlunoTipoAnotacao> SelecionaPorTurmaAula
        (
            Int64 tud_id
            , int tau_id
        )
        {
            CLS_TurmaAulaAlunoTipoAnotacaoDAO dao = new CLS_TurmaAulaAlunoTipoAnotacaoDAO();
            return dao.SelecionaPorTurmaAula(tud_id, tau_id);
        }

        /// <summary>
        /// Busca os Tipo Anotações de um aluno da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="mtd_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaAlunoTipoAnotacao> SelecionaPorTurmaAulaAluno
        (
            Int64 tud_id
            , int tau_id
            , Int64 alu_id
            , int mtu_id
            , int mtd_id
        )
        {
            CLS_TurmaAulaAlunoTipoAnotacaoDAO dao = new CLS_TurmaAulaAlunoTipoAnotacaoDAO();
            return dao.SelecionaPorTurmaAulaAluno(tud_id, tau_id, alu_id, mtu_id, mtd_id);
        }	
        /// <summary>
        /// Retorna um datarow com dados de uma anotação do aluno na aula.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaAlunoTipoAnotacaoToDataRow(CLS_TurmaAulaAlunoTipoAnotacao entity, DataRow dr)
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["alu_id"] = entity.alu_id;
            dr["mtu_id"] = entity.mtu_id;
            dr["mtd_id"] = entity.mtd_id;
            dr["tia_id"] = entity.tia_id;

            return dr;
        }

	}
}