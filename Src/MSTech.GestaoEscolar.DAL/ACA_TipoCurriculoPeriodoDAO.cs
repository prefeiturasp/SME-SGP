/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
	using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: Classe tipo curriculo período.
	/// </summary>
	public class ACA_TipoCurriculoPeriodoDAO : Abstract_ACA_TipoCurriculoPeriodoDAO
    {       
        #region Consultas

        /// <summary>
        /// Retorna todos os tipos de currículo período pelo ano letivo e tipo nivel ensino
        /// </summary>        
        /// <param name="chp_anoLetivo">Ano letivo</param>
        /// <param name="tne_id">ID do tipo nível de ensino</param>
        public System.Data.DataTable SelecionaPorAnoLetivoNivelEnsino(int chp_anoLetivo, int tne_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoCurriculoPeriodo_SelectBy_AnoLetivoNivelEnsino", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@chp_anoLetivo";
                Param.Size = 4;
                Param.Value = chp_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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

        /// <summary>
        /// Retorna os tipos de período do curso.
        /// </summary>
        /// <param name="totalRecords">Total de registros da consulta</param>
        /// <returns>Lista com os tipos</returns>
        public DataTable SelectByPesquisa
        (            
            int tne_id
            , int tme_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoCurriculoPeriodo_SelectBy_NivelEnsino_ModalidadeEnsino", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                if (tme_id > 0)
                    Param.Value = tme_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

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
        /// Verifica o maior número de ordem cadastado de tipo de curriculo periodo
        /// </summary>     
        public int Select_MaiorOrdem()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoCurriculoPeriodo_Select_MaiorOrdem", _Banco);
            try
            {
                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0]) : 0;
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
        
        #endregion Consultas
    }
}