using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Código de tipo da escala avaliação.
    /// </summary>
    public enum EscalaAvaliacaoTipo
    {
        Numerica = 1
       , Pareceres = 2
       , Relatorios = 3

    }

    public class ACA_EscalaAvaliacaoBO : BusinessBase<ACA_EscalaAvaliacaoDAO, ACA_EscalaAvaliacao>
    {
        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_EscalaAvaliacao GetEntity(ACA_EscalaAvaliacao entity, TalkDBTransaction banco = null)
        {
            // Chave padrão do cache - nome do método + parâmetros.
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                (
                    chave,
                    () =>
                    {
                        dao.Carregar(entity);
                        return entity;
                    },
                    GestaoEscolarUtilBO.MinutosCacheMedio
                ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_EscalaAvaliacao entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_EscalaAvaliacao entity)
        {
            return string.Format(ModelCache.ESCALA_AVALIACAO_MODEL_KEY, entity.esa_id);
        }

        /// <summary>
        /// Retorna as escala de avaliação não excluídas
        /// </summary>
        /// <param name="esa_nome">Nome da escala de avaliação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        /// <returns>Datatable com as escalas de avaliação</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscalaAvaliacao(string esa_nome, Guid ent_id)
        {
            totalRecords = 0;

            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            return dao.SelectBy_Pesquisa(esa_nome, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna as escala de avaliação não excluídas de acordo com o tipo da escala
        /// </summary>
        /// <param name="numerico">Indica se vai trazer as escalas do tipo numerico</param>
        /// <param name="parecer">Indica se vai trazer as escalas do tipo parecer</param>
        /// <param name="relatorio">Indica se vai trazer as escalas do tipo relatorio</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com as escalas de avaliação de acordo com o tipo da escala</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscalaAvaliacaoPorTipo(bool numerico, bool parecer, bool relatorio, Guid ent_id)
        {
            totalRecords = 0;

            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            return dao.SelectBy_TipoEscala(numerico, parecer, relatorio, ent_id);
        }

        /// <summary>
        /// Verifica se existe uma escala de avaliação com o mesmo nome na mesma entidade
        /// </summary>
        /// <param name="entity">Entidade ACA_EscalaAvaliacao</param>
        /// <returns>true: já existe a escala/false: ainda não existe a escala</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaNomeExistente(ACA_EscalaAvaliacao entity)
        {
            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            return dao.SelectBy_Nome(entity.esa_id, entity.esa_nome, entity.ent_id);
        }

        /// <summary>
        /// Seleciona as escalas de avaliação do tipo pareceres de todas as turmas dos filtros utilizados
        /// (Utilizada na tela de quadros totalizadores)
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do currículo período</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="adm">True - Administrador do sistema</param>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <returns>Retorna uma string com os esa_ids separados por vírgula</returns>
        public static string SelecionaPorTurmasFiltros
        (
            int cal_id
            , Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , long tur_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            return dao.SelectBy_TurmasFiltros(cal_id, uad_idSuperior, esc_id, uni_id, cur_id, crr_id, crp_id, tur_id, ent_id, adm, usu_id, gru_id);
        }

        /// <summary>
        /// Busca as escalas de avaliação ativas pela chave k1 da entidade.
        /// </summary>
        /// <param name="k1"></param>
        /// <returns></returns>
        public static DataTable BuscaEscalasAvaliacaoPorChaveK1(string k1)
        {
            if (string.IsNullOrEmpty(k1))
            {
                throw new k1VaziaException();
            }

            ACA_EscalaAvaliacaoDAO dao = new ACA_EscalaAvaliacaoDAO();
            return dao.BuscaEscalasAvaliacaoPorChaveK1(k1);
        }
    }
}
