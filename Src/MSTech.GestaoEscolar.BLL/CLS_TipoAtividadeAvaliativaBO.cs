/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{

    #region Enumerador

    /// <summary>
    /// Situações do tipo de atividade avaliativa
    /// </summary>
    public enum CLS_TipoAtividadeAvaliativaSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
        ,
        Inativo = 4
    }

    #endregion

    /// <summary>
    /// CLS_TipoAtividadeAvaliativa Business Object 
    /// </summary>
    public class CLS_TipoAtividadeAvaliativaBO : BusinessBase<CLS_TipoAtividadeAvaliativaDAO, CLS_TipoAtividadeAvaliativa>
    {
        #region Consultas

        /// <summary>
        /// Retorna todos os tipos de atividades avaliativas não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TipoAtividadeAvaliativa> SelecionaTipoAtividadeAvaliativa(int appMinutosCacheLongo = 0)
        {
            List<CLS_TipoAtividadeAvaliativa> dados = null;

            Func<List<CLS_TipoAtividadeAvaliativa>> retorno = delegate ()
            {
                CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();
                return dados = dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords)
                            .Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TipoAtividadeAvaliativa())).ToList();
            };

            dados = retorno();

            return dados;
        }

        /// <summary>
        /// Retorna os tipos de atividades avaliativas não excluídos logicamente
        /// </summary>
        /// <param name="apenasAtivos">True - retorna apenas ativos | False - retorna todos</param>
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TipoAtividadeAvaliativa> SelecionaTipoAtividadeAvaliativa(bool apenasAtivos, int appMinutosCacheLongo = 0)
        {
            return apenasAtivos
                ? SelecionaAtivos(appMinutosCacheLongo)
                : SelecionaTipoAtividadeAvaliativa(appMinutosCacheLongo);
        }

        /// <summary>
        /// Seleciona todos os tipos de atividades avaliativas não excluídos logicamente
        /// e ativos.
        /// </summary>
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TipoAtividadeAvaliativa> SelecionaAtivos(int appMinutosCacheLongo = 0)
        {
            List<CLS_TipoAtividadeAvaliativa> dados = null;

            Func<List<CLS_TipoAtividadeAvaliativa>> retorno = delegate()
            {
                CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();
                return dados = dao.SelecionaAtivos();
            };

            dados = retorno();

            return dados;
        }
        
        /// <summary>
        /// Carrega os dados na edição
        /// </summary>
        /// <param name="tav_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CLS_TipoAtividadeAvaliativa CarregaDados(int tav_id)
        {
            CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();

            CLS_TipoAtividadeAvaliativa dados = dao.CarregaDados(tav_id);

            return dados;
        }

        /// <summary>
        /// Seleciona todos os tipos de atividades avaliativas não excluídos logicamente
        /// e ativos.
        /// </summary>
        /// <param name="appMinutosCacheLongo">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TipoAtividadeAvaliativa> SelecionaTiposAtividadesAvaliativasAtivosBy_TurmaDisciplina(long tud_id, int appMinutosCacheLongo = 0)
        {
            List<CLS_TipoAtividadeAvaliativa> dados = null;

            Func<List<CLS_TipoAtividadeAvaliativa>> retorno = delegate()
            {
                CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();
                return dao.SelecionaAtivosBy_TurmaDisciplina(tud_id);
            };

            dados = retorno();

            return dados;
        }

        /// <summary>
        /// Retorna o ID do tipo de atividade avaliativa relacionado.
        /// </summary>
        /// <param name="caaId"></param>
        /// <param name="qatId"></param>
        /// <returns></returns>
        public static int SelecionaTipoAtividadeAvaliativaRelacionado(int caaId, int qatId, TalkDBTransaction banco)
        {
            CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();
            dao._Banco = banco;
            return dao.SelecionaTipoAtividadeAvaliativaRelacionado(caaId, qatId);
        }

        #endregion
    }
}