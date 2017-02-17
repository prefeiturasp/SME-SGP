/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class ACA_AlunoHistoricoDisciplinaDAO : Abstract_ACA_AlunoHistoricoDisciplinaDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna um datatable contendo todas as Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        public DataTable SelectBy_alu_id
        (
            long alu_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_SelectBy_alu_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            if (alu_id > 0)
                Param.Value = alu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e ano letivo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_anoLetivo">Ano letivo do historico</param>  
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        public DataTable SelectBy_alu_id_alh_anoLetivo
        (
            long alu_id
            , int alh_anoLetivo
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_SelectBy_alu_id_alh_anoLetivo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alh_anoLetivo";
            Param.Size = 4;
            Param.Value = alh_anoLetivo;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e tipo curriculo periodo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="tcp_id">Id da table ACA_TipoCurriculoPeriodo</param>  
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        public DataTable SelectBy_alu_id_tcp_id
        (
            long alu_id
            , int tcp_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_SelectBy_alu_id_tcp_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tcp_id";
            Param.Size = 4;
            Param.Value = tcp_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e historico id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_id">Id do historico do aluno</param>  
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        public DataTable SelectBy_alu_id_alh_id
        (
            long alu_id
            , int alh_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_SelectBy_alu_id_alh_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alh_id";
            Param.Size = 4;
            Param.Value = alh_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        #endregion Métodos

        #region Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alh_id";
            Param.Size = 4;
            Param.Value = entity.alh_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ahd_id";
            Param.Size = 4;
            Param.Value = entity.ahd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ahd_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoHistoricoDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(ACA_AlunoHistoricoDisciplina entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoHistoricoDisciplina_Update_Situacao";
            return base.Delete(entity);
        }
        
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #endregion Sobrescritos
    }
}