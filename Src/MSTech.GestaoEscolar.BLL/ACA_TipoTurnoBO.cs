using System;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TipoTurnoBO : BusinessBase<ACA_TipoTurnoDAO, ACA_TipoTurno>
    {
        #region Enumerador

        /// <summary>
        /// Enumerador de Tipo de turno.
        /// </summary>
        public enum TipoTurno : byte
        {
            Manha = 1,

            Tarde = 2,

            Noite = 3,

            Integral = 4,

            Intermediario = 5
        }

        #endregion Enumerador

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_TipoTurnoPorTurma(Int64 tur_id)
        {
            return string.Format("Cache_TipoTurnoPorTurma_{0}", tur_id.ToString());
        }

        /// <summary>
        /// Busca as informações do turno, de acordo com a turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns>Entidade turno.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoTurnoPorTurma
        (
            Int64 tur_id
        )
        {
            ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
            return dao.SelectBy_Turma(tur_id);
        }

        /// <summary>
        /// Busca as informações do turno, de acordo com a turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns>Entidade turno.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_TipoTurno SelecionaEntTipoTurnoPorTurma
        (
            Int64 tur_id,
            int appMinutosCacheLongo = 0
        )
        {
            ACA_TipoTurno entity = null;

            Func<ACA_TipoTurno> retorno = delegate()
            {
                ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
                DataTable dt = dao.SelectBy_Turma(tur_id);
                entity = dt.Rows.Count > 0 ? dao.DataRowToEntity(dt.Rows[0], new ACA_TipoTurno()) : null;
                return entity;
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.TIPO_TURNO_TURMA_MODEL_KEY, tur_id);               

                entity = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
            }
            else
            {
                entity = retorno();
            }

            return entity;           
            
        }

        /// <summary>
        /// Busca as informações do turno, de acordo com a entidade do turno.
        /// </summary>
        /// <param name="ent_id"></param>
        /// <returns>Entidade turno.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorEntidadeTurno
        (
            Guid ent_id
        )
        {
            ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
            return dao.SelectBy_EntidadeTurno(ent_id);
        }
        
        /// <summary>
        /// Retorna todos os tipos de turno não excluídos logicamente
        /// Com paginação
        /// </summary>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoTurnoPaginado
        (
            int currentPage
            , int pageSize
        )
        {
            ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
            return dao.SelectBy_Pesquisa(true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de turno não excluídos logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoTurno()
        {
            ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }
        
        /// <summary>
        ///  Verifica se existe o tipo de turno com o mesmo nome,
        ///  escola, curso, currículo e currículo escola e
        ///  preenche a entidade.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoTurno.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade adm.</param>
        /// <param name="ces_id">Id do currículo escola.</param>
        /// <returns>True = Encontrou o tipo de turno / False = Não encontrou.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCurriculoTurno
        (
            ACA_TipoTurno entity
            , int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , int ces_id
        )
        {
            ACA_TipoTurnoDAO dao = new ACA_TipoTurnoDAO();
            return dao.SelectBy_Nome_CurriculoTurno(entity, cur_id, crr_id, esc_id, uni_id, ces_id);
        }
    }
}