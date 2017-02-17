/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data;
    using System;
    using System.Data.SqlClient;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO : Abstract_CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO
    {
        #region Métodos

        /// <summary>
        /// Busca o planejamento orientação curricular diagnostico atravéz da turma disciplina.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> BuscaPlanejamentoOrientacaoCurricularDiagnostico
        (
           long tur_id
        )
        {
            List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> lt = new List<CLS_PlanejamentoOrientacaoCurricularDiagnostico>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaPlanejamentoOrientacaoCurricularDiagnostico", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;

                qs.Parameters.Add(Param);



                #endregion

                qs.Execute();

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CLS_PlanejamentoOrientacaoCurricularDiagnostico())).ToList<CLS_PlanejamentoOrientacaoCurricularDiagnostico>();
                return lt;



            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca o planejamento orientação curricular diagnostico atravéz da turma disciplina.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable BuscaPlanejamentoOrientacaoCurricularDiagnosticoDT
        (
           int tud_id, int tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaPlanejamentoOrientacaoCurricularDiagnosticoPor_Turmas", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;


            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Salva os dados do diagnóstico em lote.
        /// </summary>
        /// <param name="dtPlanejamentoOrientacaoCurricularDiagnostico">DataTable de dados do diagnóstico.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable dtPlanejamentoOrientacaoCurricularDiagnostico)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_PlanejamentoOrientacaoCurricularDiagnostico_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbPlanejamentoOrientacaoCurricularDiagnostico";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_PlanejamentoOrientacaoCurricularDiagnostico";
                sqlParam.Value = dtPlanejamentoOrientacaoCurricularDiagnostico;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Sobrecritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_PlanejamentoOrientacaoCurricularDiagnostico entity)
        {
            return true;
        }		

        #endregion
    }
}