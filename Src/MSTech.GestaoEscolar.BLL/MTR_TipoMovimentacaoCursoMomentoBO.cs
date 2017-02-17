/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Estrutura para salvar os cursos momento do parâmetro de movimentação
    /// </summary>
    public struct MTR_TipoMovimentacaoCursoMomento_Cadastro
    {
        public MTR_TipoMovimentacaoCursoMomento entityCursoMomento;
        public List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> listMomentoCalendarioPeriodo;
    }

    /// <summary>
    /// MTR_TipoMovimentacaoCursoMomento Business Object 
    /// </summary>
    public class MTR_TipoMovimentacaoCursoMomentoBO : BusinessBase<MTR_TipoMovimentacaoCursoMomentoDAO, MTR_TipoMovimentacaoCursoMomento>
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna todos os momentos do parâmetro de movimentação não excluídos logicamente
        /// </summary>                
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>     
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaMomentosPorParametroMovimentacao
        (
            int tmo_id
        )
        {
            MTR_TipoMovimentacaoCursoMomentoDAO dao = new MTR_TipoMovimentacaoCursoMomentoDAO();
            return dao.SelectBy_tmo_id(tmo_id);
        }

        /// <summary>
        /// Retorna todos os momentos do parâmetro de movimentação não excluídos logicamente
        /// </summary>                
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>     
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MTR_TipoMovimentacaoCursoMomento> SelecionaListaMomentosPorParametroMovimentacao
        (
            int tmo_id
        )
        {
            MTR_TipoMovimentacaoCursoMomentoDAO dao = new MTR_TipoMovimentacaoCursoMomentoDAO();
            DataTable dt = dao.SelectBy_tmo_id(tmo_id);

            List<MTR_TipoMovimentacaoCursoMomento> lista = new List<MTR_TipoMovimentacaoCursoMomento>();
            foreach (DataRow dr in dt.Rows)
            {

                MTR_TipoMovimentacaoCursoMomento ent = new MTR_TipoMovimentacaoCursoMomento();
                ent = dao.DataRowToEntity(dr, ent);

                lista.Add(ent);
            }

            return lista;
        }

        /// <summary>
        /// Retorna os momentos do parâmetro de movimentação por tipo de movimentção e ano letivo dos momentos de movimentação.
        /// </summary>
        /// <param name="tmo_id">ID do tipo de movimentação.</param>
        /// <param name="mom_ano">Ano letivo do momento de movimentação.</param>
        /// <param name="mom_id">ID do momento de movimentação.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTipoMovimentacaoMomentoAno(int tmo_id, int mom_ano, int mom_id, Guid ent_id)
        {
            return new MTR_TipoMovimentacaoCursoMomentoDAO().SelecionaPorTipoMovimentacaoMomentoAno(tmo_id, mom_ano, mom_id, ent_id);
        }

        /// <summary>
        /// Retorna os momentos do parâmetro de movimentação por tipo de movimentção e ano letivo dos momentos de movimentação.
        /// </summary>
        /// <param name="tmo_id">ID do tipo de movimentação.</param>
        /// <param name="mom_ano">Ano letivo do momento de movimentação.</param>
        /// <param name="mom_id">ID do momento de movimentação.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MTR_TipoMovimentacaoCursoMomento> SelecionaListaPorTipoMovimentacaoMomentoAno(int tmo_id, int mom_ano, int mom_id, Guid ent_id)
        {
            MTR_TipoMovimentacaoCursoMomentoDAO dao = new MTR_TipoMovimentacaoCursoMomentoDAO();
            return dao.SelecionaPorTipoMovimentacaoMomentoAno(tmo_id, mom_ano, mom_id, ent_id).Rows
                       .Cast<DataRow>()
                       .Select(dr => dao.DataRowToEntity(dr, new MTR_TipoMovimentacaoCursoMomento())).ToList();
        }

        #endregion Métodos de consulta
    }
}
