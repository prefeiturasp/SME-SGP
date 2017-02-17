/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;

namespace MSTech.GestaoEscolar.BLL
{

    /// <summary>
    /// ACA_CurriculoDisciplinaEletiva Business Object 
    /// </summary>
    public class ACA_CurriculoDisciplinaEletivaBO : BusinessBase<ACA_CurriculoDisciplinaEletivaDAO, ACA_CurriculoDisciplinaEletiva>
    {
        /// <summary>
        /// Retorna um datatable contendo todos as disciplinas eletivas do curriculo/curso
        /// que não foram excluídos logicamente        
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , int crr_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_CurriculoDisciplinaEletivaDAO dao = new ACA_CurriculoDisciplinaEletivaDAO();

            return dao.SelectBy_cur_id_crr_id(cur_id, crr_id, paginado, currentPage / pageSize, pageSize, out totalRecords);

        }

        /// <summary>
        /// Seleciona pelos filtros passados por parametro, com paginação
        /// </summary>
        /// <param name="cur_id">cur_id</param>
        /// <param name="crr_id">crr_id</param>
        /// <param name="crp_id">crp_id</param>
        /// <param name="paginado">Se vai pagina</param>
        /// <param name="currentPage">Numero da pagina a retornar</param>
        /// <param name="pageSize">Tamanho da pagina</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_CurriculoDisciplinaEletiva> GetSelect
        (
            int cur_id
            , int crr_id
            , int crp_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_CurriculoDisciplinaEletivaDAO dao = new ACA_CurriculoDisciplinaEletivaDAO();

            return
                (from DataRow dr in
                     dao.SelectBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id, paginado, currentPage / pageSize, pageSize,
                                                out totalRecords).Rows
                 select dao.DataRowToEntity(dr, new ACA_CurriculoDisciplinaEletiva())).ToList();
        }

        /// <summary>
        /// Inclui uma nova disciplina eletiva para o curriculo/curso/periodo
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoDisciplinaEletiva</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoDisciplinaEletiva entity
            , Data.Common.TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_CurriculoDisciplinaEletivaDAO dao = new ACA_CurriculoDisciplinaEletivaDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new Validation.Exceptions.ValidationException(entity.PropertiesErrorList[0].Message);
        }
    }
}