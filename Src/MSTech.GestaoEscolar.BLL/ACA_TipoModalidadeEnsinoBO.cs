using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TipoModalidadeEnsinoBO : BusinessBase<ACA_TipoModalidadeEnsinoDAO, ACA_TipoModalidadeEnsino>
    {
        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// Com paginação
        /// </summary>                        
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsinoPaginado
        (
            int currentPage
            , int pageSize
            , int tme_idSuperior
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_TipoModalidadeEnsinoDAO dao = new ACA_TipoModalidadeEnsinoDAO();
            return dao.SelectBy_Pesquisa(true, currentPage/pageSize, pageSize, tme_idSuperior, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsino()
        {
            ACA_TipoModalidadeEnsinoDAO dao = new ACA_TipoModalidadeEnsinoDAO();
            totalRecords = 0;
            return dao.SelectAtivos(out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsinoFilhos()
        {
            ACA_TipoModalidadeEnsinoDAO dao = new ACA_TipoModalidadeEnsinoDAO();
            totalRecords = 0;
            return dao.SelectFilhosAtivos(out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// de acordo com as atribuições do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsinoFilhosDocenteEventoAno(long doc_id, string eventosAbertos, int cal_ano)
        {
            return new ACA_TipoModalidadeEnsinoDAO().SelecionaTipoModalidadeEnsinoDocenteEvento(doc_id, eventosAbertos, cal_ano);
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// de acordo com as atribuições do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsinoFilhosDocenteEvento(long doc_id, string eventosAbertos)
        {
            return new ACA_TipoModalidadeEnsinoDAO().SelecionaTipoModalidadeEnsinoDocenteEvento(doc_id, eventosAbertos, 0);
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// Vinculados a escola informada.
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade escolar</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_idSuperior">Id da entidade superior</param>
        /// </summary>           
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoModalidadeEnsino_Por_Escola
        (   int esc_id,
            int uni_id,
            Guid ent_id,
            Guid uad_idSuperior
        )
        {
            ACA_TipoModalidadeEnsinoDAO dao = new ACA_TipoModalidadeEnsinoDAO();
            return dao.SelecionaTipoModalidadeEnsino_Por_Escola(esc_id, uni_id, ent_id, uad_idSuperior);
        }
    }
}
