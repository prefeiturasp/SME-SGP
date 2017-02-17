using System;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_UnidadeEscolaContatoBO : BusinessBase<ESC_UnidadeEscolaContatoDAO, ESC_UnidadeEscolaContato>            
    {
        /// <summary>
        /// Retorna um datatable contendo todos os tipos de meio de contato
        /// que não foram excluídos logicamente. Filtrado por esc_id e uni_id.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <returns>DataTable com tipos de meio de contato.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaContatosDaEscola
        (
            int esc_id
            , int uni_id
        )
        {
            ESC_UnidadeEscolaContatoDAO dao = new ESC_UnidadeEscolaContatoDAO();
            return dao.SelecionaContatosDaEscola(esc_id, uni_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os contatos da unidade escola
        /// que não foram excluídos logicamente, filtrados por 
        /// id da entidade, id da unidade administrativa
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os contatos da unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id
            , int uni_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ESC_UnidadeEscolaContatoDAO dao = new ESC_UnidadeEscolaContatoDAO();
            return dao.SelectBy_esc_id_uni_id(esc_id, uni_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona o id do último contato cadastrado para a unidade escola + 1
        /// se não houver contato cadastrado para unidade escola retorna 1
        /// filtrados por esc_id, uni_id
        /// </summary>
        /// <param name="esc_id">Campo ent_id da tabela ESC_UnidadeEscolaContato do bd</param>        
        /// <param name="uni_id">Campo uni_id da tabela ESC_UnidadeEscolaContato do bd</param>        
        /// <returns>uec_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimoContatoCadastrado
        (
            int esc_id
            , int uni_id
        )
        {
            ESC_UnidadeEscolaContatoDAO dao = new ESC_UnidadeEscolaContatoDAO();
            return dao.SelectBy_esc_id_uni_id_top_one(esc_id, uni_id);
        }

        /// <summary>
        /// Inclui um novo contato para a unidade escola
        /// </summary>
        /// <param name="entity">Entidade ESC_UnidadeEscolaContato</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ESC_UnidadeEscolaContato entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ESC_UnidadeEscolaContatoDAO dao = new ESC_UnidadeEscolaContatoDAO {_Banco = banco};

                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}
