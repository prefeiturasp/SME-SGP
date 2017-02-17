/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_QualificadorAtividadeDAO : AbstractCLS_QualificadorAtividadeDAO
    {
        #region Metodos

        /// <summary>
        /// Retorna todos os qualificadores de atividade não excluídos logicamente
        /// </summary>         
        public DataTable SelectAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QualificadorAtividade_SelectAtivos", _Banco);
            try
            {
                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        /// <summary>
        /// Retorna os qualificadores de atividade ativos configurados para a turma disciplina.
        /// </summary>         
        public DataTable SelectAtivosBy_TurmaDisciplina(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QualificadorAtividade_SelectAtivosBy_TurmaDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion
    }
}
