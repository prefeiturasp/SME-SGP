using System;
using System.ComponentModel;
using System.Data;
using System.Web;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Regime de Matricula
    /// </summary>
    public enum ACA_CurriculoRegimeMatricula : byte
    {
        Seriado = 1
        ,

        PorCreditos = 2
        ,

        SeriadoPorAvaliacoes = 3
    }

    /// <summary>
    /// Situações do curriculo
    /// </summary>
    public enum ACA_CurriculoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Desativado = 4
        ,

        EmAtivacao = 5
        ,

        EmDesativacao = 6
    }

    public class ACA_CurriculoBO : BusinessBase<ACA_CurriculoDAO, ACA_Curriculo>
    {
        #region Consultas

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_Curriculo GetEntity(ACA_Curriculo entity, TalkDBTransaction banco = null)
        {
            ACA_CurriculoDAO dao = banco == null ? new ACA_CurriculoDAO() : new ACA_CurriculoDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            dao.Carregar(entity);

            return entity;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_cur_id
        (
            int cur_id
        )
        {
            ACA_CurriculoDAO dao = new ACA_CurriculoDAO();
            return dao.SelectBy_cur_id(cur_id);
        }

        /// <summary>
        /// Consulta o currículo pelo código do curso.
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="cur_codigo">Código do curso</param>
        /// <param name="entityCurriculo">Entidade currículo</param>
        /// <returns>True/False</returns>
        public static bool ConsultarCodigoCurso
        (
            Guid ent_id
            , string cur_codigo
            , out ACA_Curriculo entityCurriculo
        )
        {
            ACA_CurriculoDAO dao = new ACA_CurriculoDAO();
            return dao.SelectBy_Curso_Codigo(ent_id, cur_codigo, out entityCurriculo);
        }

        #endregion Consultas

        #region Cache

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_Curriculo entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        public static void LimpaCache_PeloCurso(ACA_Curso entity)
        {
            if (HttpContext.Current != null)
            {
                GestaoEscolarUtilBO.LimpaCache(string.Format("ACA_Curriculo_GetEntity_{0}_", entity.cur_id));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_Curriculo entity)
        {
            return string.Format("ACA_Curriculo_GetEntity_{0}_{1}", entity.cur_id, entity.crr_id);
        }

        #endregion Cache

        #region Inclusão e Alteração

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_Curriculo entity
            , TalkDBTransaction banco = null
        )
        {
            if (entity.Validate())
            {
                LimpaCache(entity);

                ACA_CurriculoDAO dao = (banco == null ? new ACA_CurriculoDAO() : new ACA_CurriculoDAO { _Banco = banco });
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        #endregion Inclusão e Alteração
    }
}