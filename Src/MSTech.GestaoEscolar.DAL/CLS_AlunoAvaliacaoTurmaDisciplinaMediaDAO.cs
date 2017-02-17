/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Data.SqlClient;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO : Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO
    {
        #region Metodos

        /// <summary>
        /// Busca as médias da turma salvas pra disicplina e período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do período</param>
        /// <returns></returns>
        public DataTable BuscaNotasFinaisTurma(long tud_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplinaMediaSelecionaPor_Turma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
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
        /// Busca as médias da turma salvas pra disicplina e período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do período</param>
        /// <returns></returns>
        public DataTable BuscaNotasFinaisTud(long tud_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplinaMediaSelecionaPor_Tud", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
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
        /// Salva os dados da média dos alunos.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplinaMedia">DataTable de médias dos alunos.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable dtAlunoAvaliacaoTurmaDisciplinaMedia)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplinaMedia_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbAlunoAvaliacaoTurmaDisciplinaMedia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoAvaliacaoTurmaDisciplinaMedia";
                sqlParam.Value = dtAlunoAvaliacaoTurmaDisciplinaMedia;
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

        #region Sobrescritos

        /// <summary>
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplinaMedia entity)
        {
            entity.atm_dataCriacao = DateTime.Now;
            entity.atm_dataAlteracao = DateTime.Now;

            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplinaMedia entity)
        {
            entity.atm_dataAlteracao = DateTime.Now;

            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@atm_dataCriacao");
        }

        /// <summary>
        /// Méotodo de update alterado para que não modificasse o valor do campo data da criação;
        /// </summary>
        /// <param name="entity">Entidade com dados preenchidos</param>
        /// <returns>True para alteração realizado com sucesso.</returns>
        protected override bool Alterar(CLS_AlunoAvaliacaoTurmaDisciplinaMedia entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoAvaliacaoTurmaDisciplinaMedia_UPDATE";
            return base.Alterar(entity);
        }

        #endregion
	}
}