/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_TipoAnotacaoAlunoDAO : Abstract_ACA_TipoAnotacaoAlunoDAO
	{
        /// <summary>
        /// Retorna informações dos tipos de anotacões do aluno
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <returns>Lista com os tipos</returns>
        public DataTable SelecionarTipoAnotacaoAluno_ent_id(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAnotacaoAluno_SelectBy_ent_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);
                
                #endregion PARAMETROS

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
        /// Retorna informações dos tipos de anotacões do aluno
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="tia_nome">nome do tipo de anotação do aluno</param>
        /// <returns>Lista com os tipos</returns>
        public DataTable SelecionarTipoAnotacaoAluno_ent_id_tia_nome(Guid ent_id, string tia_nome)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAnotacaoAluno_SelectBy_ent_id_tia_nome", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tia_nome";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tia_nome))
                    Param.Value = tia_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Seleciona os dados do tipo de anotacão do aluno de acordo com o id e entidade da mesma.
        /// </summary>
        /// <param name="tia_id">ID do tipo de anotacão do aluno</param>
        /// <param name="ent_id">id da entidade</param>  
        public DataTable GetEntity_tia_id_ent_id(int tia_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAnotacaoAluno_SelectBy_tia_id_ent_id", _Banco);
            
            try
            {
            
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tia_id";
                Param.Size = 4;
                if (tia_id > 0)
                    Param.Value = tia_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);
                
                #endregion PARAMETROS

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
        
    }
}