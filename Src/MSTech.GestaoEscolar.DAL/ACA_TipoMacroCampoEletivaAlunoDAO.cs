/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	/// <summary>
    /// Classe ACA_TipoMacroCampoEletivaAlunoDAO.
	/// </summary>
	public class ACA_TipoMacroCampoEletivaAlunoDAO : Abstract_ACA_TipoMacroCampoEletivaAlunoDAO
	{
        /// <summary>
        /// Retorna todos os tipos de macro-campo disciplina não excluídos logicamente.
        /// </summary>        
        /// <param name="totalRecords">Total de registros retornado na busca</param> 
        /// <returns>DataTable contendo os tipos de macro-campo disciplina</returns>  
        public DataTable SelectBy_Pesquisa(out int totalRecords)
        {
            totalRecords = 0;

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoMacroCampoEletivaAluno_SelectBy_Pesquisa", _Banco);
            try
            {
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um tipo de macro-campo disciplina cadastrado com o mesmo nome.
        /// </summary>
        /// <param name="tea_id">ID do tipo de macro-campo disciplina</param>
        /// <param name="tea_nome">Nome do tipo de macro-campo disciplina</param>   
        /// <returns>True | False</returns>     
        public bool SelectBy_Nome(int tea_id, string tea_nome)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoMacroCampoEletivaAluno_SelectBy_Nome", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tea_nome";
                Param.Size = 100;
                Param.Value = tea_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tea_id";
                Param.Size = 4;
                if (tea_id > 0)
                    Param.Value = tea_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um tipo de macro-campo disciplina cadastrado com o mesma sigla.
        /// </summary>
        /// <param name="tea_id">ID do tipo de macro-campo disciplina</param>
        /// <param name="tea_sigla">Sigla do tipo de macro-campo disciplina</param>
        /// <returns>True | False</returns>     
        public bool SelectBy_Sigla( int tea_id, string tea_sigla)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoMacroCampoEletivaAluno_SelectBy_Sigla", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tea_sigla";
                Param.Size = 10;
                Param.Value = tea_sigla;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tea_id";
                Param.Size = 4;
                if (tea_id > 0)
                    Param.Value = tea_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os macro-campos não associados a disciplina.
        /// </summary>        
        /// <param name="dis_id">ID da disciplina</param>
        /// <returns>DataTable de macro-campos</returns>
        public DataTable SelectMacroCampoNaoAssociado(int dis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_DisciplinaMacroCampoEletivaAluno_SelectMacroCampoNaoAssociado", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                Param.Value = dis_id;
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
        /// Retorna os macro-campos associados a disciplina.
        /// </summary>        
        /// <param name="dis_id">ID da disciplina</param>
        /// <returns>DataTable de macro-campos</returns>
        public DataTable SelectMacroCampoAssociado(int dis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_DisciplinaMacroCampoEletivaAluno_SelectMacroCampoAssociado", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                Param.Value = dis_id;
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

        #region Métodos Sobrescritos

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

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoMacroCampoEletivaAluno entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tea_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tea_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoMacroCampoEletivaAluno entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tea_dataCriacao");
            qs.Parameters["@tea_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_TipoMacroCampoEletivaAluno entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoMacroCampoEletivaAluno_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_TipoMacroCampoEletivaAluno entity)
        {
            __STP_DELETE = "NEW_ACA_TipoMacroCampoEletivaAluno_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}