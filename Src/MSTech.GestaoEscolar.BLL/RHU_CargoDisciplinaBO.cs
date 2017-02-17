using System.Collections.Generic;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System;

namespace MSTech.GestaoEscolar.BLL
{
	/// <summary>
	/// RHU_CargoDisciplina Business Object 
	/// </summary>
	public class RHU_CargoDisciplinaBO : BusinessBase<RHU_CargoDisciplinaDAO,RHU_CargoDisciplina>
	{
        /// <summary>
        /// Verifica as disciplinas do cargo
        /// </summary>
        /// <param name="crg_id">ID do cargo</param>
        /// <returns>Retorna disciplinas do cargo</returns>
        public static List<RHU_CargoDisciplina> RetornaDisciplinasCargo(int crg_id)
        {
            RHU_CargoDisciplinaDAO dao = new RHU_CargoDisciplinaDAO();
            return dao.CarregaCargoDisciplina(crg_id);
        }

        /// <summary>
        /// Retorna todos os tipos de disciplina não excluídos logicamente filtrados por cargo
        /// Sem paginação
        /// </summary>            
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCargoDisciplinaByCrgId(int crg_id, Guid ent_id)
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            RHU_CargoDisciplinaDAO dao = new RHU_CargoDisciplinaDAO();
            return dao.SelecionaCargoDisciplinaByCrgId(controlarOrdem, crg_id);
        }

        #region Método excluir

        /// <summary>
        /// Deleta as disciplinas do cargo
        /// </summary>
        /// <param name="entity">Entidade RHU_CargoDisciplina</param>
        /// <param name="banco">Transação</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DeleteByCargo
        (
            RHU_CargoDisciplina entity
            , TalkDBTransaction banco
        )
        {
            RHU_CargoDisciplinaDAO dao = new RHU_CargoDisciplinaDAO { _Banco = banco };
            return dao.DeleteBy_Cargo(entity);
        }

        #endregion Método excluir
    }	
}