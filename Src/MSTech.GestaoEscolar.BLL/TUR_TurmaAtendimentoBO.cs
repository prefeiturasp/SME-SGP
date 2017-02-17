using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.BLL
{
    public class TUR_TurmaAtendimentoBO : BusinessBase<TUR_TurmaAtendimentoDAO, TUR_TurmaAtendimento>    
    {
        /// <summary>
        /// Retorna um datatable contendo todos os registros 
        /// da tabela de relacionamento TUR_TurmAtendimento filtrados
        /// por tur_id
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as entidades</returns>
        static public DataTable GetSelectBy_tur_id
        (
            long tur_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            try
            {
                TUR_TurmaAtendimentoDAO dao = new TUR_TurmaAtendimentoDAO();
                return dao.SelectBy_tur_id(tur_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Cria List de Turma Atendimento, cada entidade recebe um registro do DataTable.
        /// </summary>
        /// <param name="dtTurmaAtendimento">DataTable de Turma Atendimento</param>
        /// <returns>List Entidade Turma Atendimento</returns>
        static public List<TUR_TurmaAtendimento> CriaList_Entities_TurmaAtendimento(DataTable dtTurmaAtendimento, long tur_id)
        {
            try
            {
                //cria List
                List<TUR_TurmaAtendimento> lt = new List<TUR_TurmaAtendimento>();
                for (int i = 0; i < dtTurmaAtendimento.Rows.Count; i++)
                {
                    //cria entidade
                    TUR_TurmaAtendimento entityTurmaAtendimento = new TUR_TurmaAtendimento { };
                    //verifica se registro do DataTable é um novo registro
               
                    entityTurmaAtendimento.tur_id = tur_id;
                    entityTurmaAtendimento.tat_id = Convert.ToInt32(dtTurmaAtendimento.Rows[i]["tat_id"]);
                    entityTurmaAtendimento.IsNew = true;
                    //adiciona entidade na List
                    lt.Add(entityTurmaAtendimento);
                }
                //retorna List
                return lt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorno booleano na qual verifica se ID de Atendimento das entidades são iguais.
        /// </summary>
        /// <param name="entity1">Entidade de atendimento</param>
        /// <param name="entity2">Entidade de atendimento</param>
        /// <returns>True - caso seja mesmo nome/False - caso não seja mesmo nome</returns>
        public static bool VerificaMesmoAtendimento(MSTech.GestaoEscolar.Entities.TUR_TurmaAtendimento entity1, MSTech.GestaoEscolar.Entities.TUR_TurmaAtendimento entity2)
        {
            try
            {
                if (entity1.tat_id == entity2.tat_id)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Retorno booleano, de delete dos regsitros filtrados por tur_id
        /// </summary>
        /// <param name="entity">Entidade TurmaAtendimento</param>
        /// <returns>True/False</returns>
        public static bool DeleteBy_tur_id(MSTech.GestaoEscolar.Entities.TUR_TurmaAtendimento entity)
        {
            try
            {
                TUR_TurmaAtendimentoDAO dal = new TUR_TurmaAtendimentoDAO();
                return dal.DeleteBy_tur_id(entity.tur_id);
            }
            catch
            {
                throw;
            }
        }
    }
}
