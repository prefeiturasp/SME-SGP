using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class RHU_TipoVinculoBO : BusinessBase<RHU_TipoVinculoDAO, RHU_TipoVinculo>
    {
        /// <summary>
        /// Retorna todos os tipos de vínculo não excluídos logicamente
        /// </summary>                        
        /// <param name="tvi_nome">Nome do tipo de vínculo</param> 
        /// <param name="tvi_descricao">Descrição do tipo de vínculo</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>             
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoVinculo
        (            
            string tvi_nome
            , string tvi_descricao                        
            , Guid ent_id
        )
        {
            totalRecords = 0;

            RHU_TipoVinculoDAO dao = new RHU_TipoVinculoDAO();
            return dao.SelectBy_Pesquisa(tvi_nome, tvi_descricao, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de movimentação não excluídos logicamente
        /// Sem paginação
        /// </summary>       
        /// <param name="ent_id">Entidade do usuário logado</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoVinculo
        (
            Guid ent_id
        )
        {
            RHU_TipoVinculoDAO dao = new RHU_TipoVinculoDAO();
            return dao.SelectBy_Pesquisa(string.Empty, string.Empty, ent_id, out totalRecords);
        }
    }
}
