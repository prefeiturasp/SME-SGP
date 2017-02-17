/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_PlanejamentoOrientacaoCurricularDAO : Abstract_CLS_PlanejamentoOrientacaoCurricularDAO
	{
        /// <summary>
        /// O método seleciona a hierarquia de orientações curriculares na tela de planejamento anual.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do péríodo.</param>
        /// <param name="crp_idAnterior">ID do período anterior.</param>
        /// <param name="tpc_id">ID do período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <param name="anoAnterior">Flag que indica se a busca será realizada para as orientações curriculares anteriores</param>
        /// <returns></returns>
        public DataTable SelecionaPorTurmaPeriodoDisciplina
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int crp_idAnterior,
            int tpc_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            bool anoAnterior,
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoOrientacaoCurricular_SelecionaPorTurmaPeriodoDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAnterior";
                Param.Size = 4;
                if (crp_idAnterior > 0)
                    Param.Value = crp_idAnterior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 4;
                if (tdt_posicao > 0)
                    Param.Value = tdt_posicao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@anoAnterior";
                Param.Size = 1;
                Param.Value = anoAnterior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// O método seleciona a hierarquia de orientações curriculares de uma avaliação.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do péríodo.</param>
        /// <param name="crp_idAnterior">ID do período anterior.</param>
        /// <param name="tpc_id">ID do período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <param name="anoAnterior">Flag que indica se a busca será realizada para as orientações curriculares anteriores</param>
        /// <param name="tnt_id">ID da turma nota</param>
        /// <returns></returns>
        public DataTable SelecionaPorTurmaPeriodoDisciplinaAvaliacao
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int crp_idAnterior,
            int tpc_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            bool anoAnterior,
            Guid ent_id,
            int tnt_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoOrientacaoCurricular_SelecionaPorTurmaPeriodoDisciplinaAvaliacao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAnterior";
                Param.Size = 4;
                if (crp_idAnterior > 0)
                    Param.Value = crp_idAnterior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 4;
                if (tdt_posicao > 0)
                    Param.Value = tdt_posicao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@anoAnterior";
                Param.Size = 1;
                Param.Value = anoAnterior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tnt_id;
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
        /// Busca o planejamento orientação curricular atravéz da turma 
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public List<CLS_PlanejamentoOrientacaoCurricular> BuscaPlanejamentoOrientacaoCurricular
        (          
           long tur_id          
        )
        {
            List<CLS_PlanejamentoOrientacaoCurricular> lt = new List<CLS_PlanejamentoOrientacaoCurricular>(); 
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaPlanejamentoOrientacaoCurricular", _Banco);

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

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CLS_PlanejamentoOrientacaoCurricular())).ToList<CLS_PlanejamentoOrientacaoCurricular>();
                return lt;


                
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca o planejamento orientação curricular atravéz da turma 
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable BuscaPlanejamentoOrientacaoCurricularDT
        (
           int tud_id, int tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaPlanejamentoOrientacaoCurricularPor_Turmas", _Banco);

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
        /// Salva os dados do planejamento em lote.
        /// </summary>
        /// <param name="dtPlanejamentoOrientacaoCurricular">DataTable de dados planejamento.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable dtPlanejamentoOrientacaoCurricular)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_PlanejamentoOrientacaoCurricular_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbPlanejamentoOrientacaoCurricular";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_PlanejamentoOrientacaoCurricular";
                sqlParam.Value = dtPlanejamentoOrientacaoCurricular;
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

        #region Sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_PlanejamentoOrientacaoCurricular entity)
        {
            return true;
        }		

        #endregion Sobrescritos
    }
}