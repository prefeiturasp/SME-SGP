/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	using System;
	using System.Data;
	using MSTech.Data.Common;
	using MSTech.Data.Common.Abstracts;
	using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Classe abstrata de ACA_TipoAnotacaoAluno.
	/// </summary>
	public abstract class Abstract_ACA_TipoAnotacaoAlunoDAO : Abstract_DAL<ACA_TipoAnotacaoAluno>
	{
        /// <summary>
		/// ConnectionString.
		/// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoAnotacaoAluno entity)
		{
			if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tia_id";
                Param.Size = 4;
                Param.Value = entity.tia_id;
                qs.Parameters.Add(Param);
			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoAnotacaoAluno entity)
		{
			if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tia_nome";
                Param.Size = 50;
                Param.Value = entity.tia_nome;
                qs.Parameters.Add(Param);

			    Param = qs.NewParameter();
			    Param.DbType = DbType.AnsiString;
			    Param.ParameterName = "@tia_codigo";
			    Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.tia_codigo))
				{
					Param.Value = entity.tia_codigo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			    qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tia_situacao";
                Param.Size = 1;
                Param.Value = entity.tia_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tia_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tia_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tia_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tia_dataAlteracao;
                qs.Parameters.Add(Param);

			    Param = qs.NewParameter();
			    Param.DbType = DbType.Guid;
			    Param.ParameterName = "@ent_id";
			    Param.Size = 16;
			    Param.Value = entity.ent_id;
			    qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoAnotacaoAluno entity)
		{
			if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tia_id";
                Param.Size = 4;
                Param.Value = entity.tia_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tia_nome";
                Param.Size = 50;
                Param.Value = entity.tia_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tia_codigo";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(entity.tia_codigo))
                {
                    Param.Value = entity.tia_codigo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tia_situacao";
                Param.Size = 1;
                Param.Value = entity.tia_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tia_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tia_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tia_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tia_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoAnotacaoAluno entity)
		{
			if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tia_id";
                Param.Size = 4;
                Param.Value = entity.tia_id;
                qs.Parameters.Add(Param);
			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoAnotacaoAluno entity)
		{
			if (entity != null & qs != null)
            {
                entity.tia_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.tia_id > 0);
			}

			return false;
		}		
	}
}