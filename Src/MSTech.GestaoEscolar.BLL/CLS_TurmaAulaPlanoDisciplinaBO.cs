/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System;
    using System.Linq;

    /// <summary>
    /// Description: CLS_TurmaAulaPlanoDisciplina Business Object. 
    /// </summary>
    public class CLS_TurmaAulaPlanoDisciplinaBO : BusinessBase<CLS_TurmaAulaPlanoDisciplinaDAO, CLS_TurmaAulaPlanoDisciplina>
	{
        /// <summary>
        /// Deleta os relacionamentos do tud_id e tau_id informados
        /// </summary>
        /// <param name="tud_id"></param>
        public static void DeleteBy_aulaDisciplina(long tud_id, int tau_id, TalkDBTransaction banco = null)
        {
            try
            {
                CLS_TurmaAulaPlanoDisciplinaDAO dao = new CLS_TurmaAulaPlanoDisciplinaDAO();
                dao._Banco = banco == null ? dao._Banco : banco;
                dao.DeleteBy_aulaDisciplina(tud_id, tau_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna um dataRow com os dados de CLS_TurmaAUlaPlanoDisciplinat
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <param name="tuf_dataAlteracao"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaPlanoDiciplinaToDataRow(CLS_TurmaAulaPlanoDisciplina entity, DataRow dr)
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["tud_idPlano"] = entity.tud_idPlano;

            return dr;
        }

        /// <summary>
        /// Deleta os relacionamentos na tabela
        /// </summary>
        /// <param name="lstTurmaAulaPlanoDisc"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool DeletarAulasPlanos(List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletar, TalkDBTransaction banco = null)
        {
            try
            {
                DataTable dtTurmaAula = CLS_TurmaAulaPlanoDisciplina.TipoTabela_TurmaAulaPlanoDisciplina();

                lstTurmaAulaPlanoDiscDeletar.ForEach(p =>
                {
                    dtTurmaAula.Rows.Add(CLS_TurmaAulaPlanoDisciplinaBO.TurmaAulaToDataRow(p, dtTurmaAula.NewRow()));
                });

                return banco == null ?
                    new CLS_TurmaAulaPlanoDisciplinaDAO().DeletarAulas(dtTurmaAula) :
                    new CLS_TurmaAulaPlanoDisciplinaDAO { _Banco = banco }.DeletarAulas(dtTurmaAula);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona todos os relacionamentos pelo tud_id da regência
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaPlanoDisciplina> SelectBy_tud_id(long tud_id)
        {
            try
            {
                CLS_TurmaAulaPlanoDisciplinaDAO dao = new CLS_TurmaAulaPlanoDisciplinaDAO();
                return dao.SelectBy_tud_id(tud_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona todos os relacionamentos pelo tud_id e tau_id da regência
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaPlanoDisciplina> SelectBy_aulaDisciplina(long tud_id, int tau_id)
        {
            try
            {
                CLS_TurmaAulaPlanoDisciplinaDAO dao = new CLS_TurmaAulaPlanoDisciplinaDAO();
                return dao.SelectBy_aulaDisciplina(tud_id, tau_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva os relacionamentos na tabela
        /// </summary>
        /// <param name="lstTurmaAulaPlanoDisc"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarAulasPlanos(List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDisc, TalkDBTransaction banco = null)
        {
            try
            {
                DataTable dtTurmaAula = CLS_TurmaAulaPlanoDisciplina.TipoTabela_TurmaAulaPlanoDisciplina();

                lstTurmaAulaPlanoDisc.ForEach(p =>
                {
                    dtTurmaAula.Rows.Add(CLS_TurmaAulaPlanoDisciplinaBO.TurmaAulaToDataRow(p, dtTurmaAula.NewRow()));
                });

                return banco == null ?
                    new CLS_TurmaAulaPlanoDisciplinaDAO().SalvarAulas(dtTurmaAula) :
                    new CLS_TurmaAulaPlanoDisciplinaDAO { _Banco = banco }.SalvarAulas(dtTurmaAula);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva o plano de aula
        /// </summary>
        /// <param name="lstTurmaAula"></param>
        /// <param name="lstTurmaAulaPlanoDisc"></param>
        /// <param name="lstTurmaAulaPlanoDiscDeletar"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<CLS_TurmaAula> lstTurmaAula
                                          , List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDisc
                                          , List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletar)
        {
            CLS_TurmaAulaPlanoDisciplinaDAO dao = new CLS_TurmaAulaPlanoDisciplinaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                DataTable dtTurmaAula = CLS_TurmaAula.TipoTabela_TurmaAula();
                DataTable dtTurmaAulaPlanoDeletar = CLS_TurmaAulaPlanoDisciplina.TipoTabela_TurmaAulaPlanoDisciplina();
                DataTable dtTurmaAulaPlanoSalvar = CLS_TurmaAulaPlanoDisciplina.TipoTabela_TurmaAulaPlanoDisciplina();

                lstTurmaAula.ForEach(p =>
                {
                    if (CLS_TurmaAulaBO.ValidarAula(p, new List<sDadosAulaProtocolo>(), new List<CLS_TurmaAula>()))
                        dtTurmaAula.Rows.Add(CLS_TurmaAulaBO.TurmaAulaToDataRow(p, dtTurmaAula.NewRow()));
                });

                if (lstTurmaAulaPlanoDisc != null)
                {
                    lstTurmaAulaPlanoDiscDeletar.ForEach(p =>
                    {
                        dtTurmaAulaPlanoDeletar.Rows.Add(CLS_TurmaAulaPlanoDisciplinaBO.TurmaAulaToDataRow(p, dtTurmaAulaPlanoDeletar.NewRow()));
                    });

                    lstTurmaAulaPlanoDisc.ForEach(p =>
                    {
                        dtTurmaAulaPlanoSalvar.Rows.Add(CLS_TurmaAulaPlanoDisciplinaBO.TurmaAulaToDataRow(p, dtTurmaAulaPlanoSalvar.NewRow()));
                    });
                }

                bool retorno = dao.SalvarEmLote(dtTurmaAula, dtTurmaAulaPlanoDeletar, dtTurmaAulaPlanoSalvar);
                
                return retorno;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade da aula.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaToDataRow(CLS_TurmaAulaPlanoDisciplina entity, DataRow dr)
        {
            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["tud_idPlano"] = entity.tud_idPlano;

            return dr;
        }

        /// <summary>
        /// Retorna se o registro existe no banco de dados.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool VerificaExisteRegistro(CLS_TurmaAulaPlanoDisciplina entity, TalkDBTransaction banco)
        {
            return banco == null ?
                   new CLS_TurmaAulaPlanoDisciplinaDAO().VerificaExisteRegistro(entity) :
                   new CLS_TurmaAulaPlanoDisciplinaDAO { _Banco = banco }.VerificaExisteRegistro(entity); 
        }

    }
}