using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.ComponentModel;
using System.Data;
using System;
using MSTech.Data.Common;
using System.Collections.Generic;
using System.Linq;
using MSTech.GestaoEscolar.BLL.Caching;
using System.Web;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da justificativa de pendência
    /// </summary>
    public enum CLS_FechamentoJustificativaPendenciaSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enumeradores


    /// <summary>
	/// Description: CLS_FechamentoJustificativaPendencia Business Object. 
	/// </summary>
	public class CLS_FechamentoJustificativaPendenciaBO : BusinessBase<CLS_FechamentoJustificativaPendenciaDAO, CLS_FechamentoJustificativaPendencia>
    {
        #region Consultas

        /// <summary>
        /// Seleciona as justificativas de pendência de acordo com os filtros de busca.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_Busca
        (
            int esc_id
            , int uni_id
            , int cal_id
            , long tud_id
            , int tpc_id
        )
        {
            totalRecords = 0;
            try
            {
                CLS_FechamentoJustificativaPendenciaDAO dao = new CLS_FechamentoJustificativaPendenciaDAO();
                return dao.SelectBy_Busca(
                            esc_id,
                            uni_id,
                            cal_id,
                            tud_id,
                            tpc_id,
                            out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona as justificativas de pendência cadastradas para a turma disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_FechamentoJustificativaPendencia> GetSelectBy_TurmaDisciplina
        (
            long tud_id
        )
        {
            try
            {
                CLS_FechamentoJustificativaPendenciaDAO dao = new CLS_FechamentoJustificativaPendenciaDAO();
                DataTable dt = dao.SelectBy_TurmaDisciplina(tud_id);
                return (from DataRow dr in dt.Rows
                        select dao.DataRowToEntity(dr, new CLS_FechamentoJustificativaPendencia())).ToList();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona as justificativas de pendência cadastradas para a turma disciplina e período do calendário.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="cal_id">Id do período do calendário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CLS_FechamentoJustificativaPendencia GetSelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , int cal_id
            , int tpc_id
            , int appMinutosCacheLongo = 0
        )
        {
            CLS_FechamentoJustificativaPendencia dados = null;
            Func<CLS_FechamentoJustificativaPendencia> retorno = delegate()
            {
                CLS_FechamentoJustificativaPendenciaDAO dao = new CLS_FechamentoJustificativaPendenciaDAO();
                return dao.SelectBy_TurmaDisciplinaPeriodo(tud_id, cal_id, tpc_id);
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.FECHAMENTO_JUSTIFICATIVA_PENDENCIA_MODEL_KEY, tud_id, cal_id, tpc_id);
                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        #endregion Consultas

        #region Saves

        /// <summary>
        /// Salva as justificativas de pendencia em lote.
        /// </summary>
        /// <param name="dtFechamentoJustificativaPendencia">Tabela com os dados das justificativas.</param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<CLS_FechamentoJustificativaPendencia> lstFechamentoJustificativaPendencia)
        {
            CLS_FechamentoJustificativaPendenciaDAO dao = new CLS_FechamentoJustificativaPendenciaDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                DataTable dtFechamentoJustificativaPendencia = CLS_FechamentoJustificativaPendencia.TipoTabela_FechamentoJustificativaPendencia();
                lstFechamentoJustificativaPendencia.ForEach(p =>
                {
                    DataRow drFechamentoJustificativaPendencia = dtFechamentoJustificativaPendencia.NewRow();
                    dtFechamentoJustificativaPendencia.Rows.Add(EntityToDataRow(p, drFechamentoJustificativaPendencia));
                });

                if (new CLS_FechamentoJustificativaPendenciaDAO{ _Banco = banco }.SalvarEmLote(dtFechamentoJustificativaPendencia)
                    // Se tiver registro de inserção na tabela, atualizo a lista de pendências no fechamento.
                    && lstFechamentoJustificativaPendencia.Any(p => p.fjp_id <= 0))
                {                    
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                        lstFechamentoJustificativaPendencia
                          .Select(p => new AlunoFechamentoPendencia
                          {
                              tud_id = p.tud_id,
                              tpc_id = p.tpc_id,
                              afp_frequencia = false,
                              afp_nota = false,
                              afp_processado = 2
                          })
                          .ToList()
                      , banco);
                }

                // Limpa o cache.
                try
                {
                    if (lstFechamentoJustificativaPendencia.Count > 0 && HttpContext.Current != null)
                    {
                        lstFechamentoJustificativaPendencia.ForEach(p => GestaoEscolarUtilBO.LimpaCache(string.Format(ModelCache.FECHAMENTO_JUSTIFICATIVA_PENDENCIA_MODEL_KEY, p.tud_id, p.cal_id, p.tpc_id)));
                    }
                }
                catch
                { }

                return true;
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        public static bool Excluir(CLS_FechamentoJustificativaPendencia fechamentoJustificativaPendencia)
        {
            CLS_FechamentoJustificativaPendenciaDAO dao = new CLS_FechamentoJustificativaPendenciaDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                if (CLS_FechamentoJustificativaPendenciaBO.Delete(fechamentoJustificativaPendencia, banco))
                {
                    // Atualizo a lista de pendências no fechamento.
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                        new List<AlunoFechamentoPendencia> {
                            new AlunoFechamentoPendencia
                            {
                                tud_id = fechamentoJustificativaPendencia.tud_id,
                                tpc_id = fechamentoJustificativaPendencia.tpc_id,
                                afp_frequencia = false,
                                afp_nota = false,
                                afp_processado = 2
                            }
                        }
                      , banco);
                }

                // Limpa o cache.
                try
                {
                    if (HttpContext.Current != null)
                    {
                        GestaoEscolarUtilBO.LimpaCache(string.Format(ModelCache.FECHAMENTO_JUSTIFICATIVA_PENDENCIA_MODEL_KEY, fechamentoJustificativaPendencia.tud_id, fechamentoJustificativaPendencia.cal_id, fechamentoJustificativaPendencia.tpc_id));
                    }
                }
                catch
                { }

                return true;
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        public static DataRow EntityToDataRow(CLS_FechamentoJustificativaPendencia entity, DataRow dr)
        {
            dr["tud_id"] = entity.tud_id;
            dr["cal_id"] = entity.cal_id;
            dr["tpc_id"] = entity.tpc_id;
            dr["fjp_id"] = entity.fjp_id;
            dr["fjp_justificativa"] = entity.fjp_justificativa;
            if (entity.usu_id != Guid.Empty)
            {
                dr["usu_id"] = entity.usu_id;
            }
            else
            {
                dr["usu_id"] = DBNull.Value;
            }
            if (entity.usu_idAlteracao != Guid.Empty)
            {
                dr["usu_idAlteracao"] = entity.usu_idAlteracao;
            }
            else
            {
                dr["usu_idAlteracao"] = DBNull.Value;
            }
            dr["fjp_situacao"] = entity.fjp_situacao;
            return dr;
        }

        #endregion Saves
    }
}