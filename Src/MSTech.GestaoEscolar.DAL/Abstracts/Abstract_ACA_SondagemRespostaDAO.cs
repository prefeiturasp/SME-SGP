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
	/// Classe abstrata de ACA_SondagemResposta.
	/// </summary>
	public abstract class Abstract_ACA_SondagemRespostaDAO : Abstract_DAL<ACA_SondagemResposta>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_SondagemResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@snd_id";
			Param.Size = 4;
			Param.Value = entity.snd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_id";
			Param.Size = 4;
			Param.Value = entity.sdr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_SondagemResposta entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@snd_id";
			Param.Size = 4;
			Param.Value = entity.snd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_id";
			Param.Size = 4;
			Param.Value = entity.sdr_id;
			qs.Parameters.Add(Param);
                
			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@sdr_sigla";
			Param.Size = 20;
			Param.Value = entity.sdr_sigla;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@sdr_descricao";
			Param.Size = 250;
			Param.Value = entity.sdr_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_ordem";
			Param.Size = 4;
			Param.Value = entity.sdr_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@sdr_situacao";
			Param.Size = 1;
			Param.Value = entity.sdr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sdr_dataCriacao";
			Param.Size = 16;
				if(entity.sdr_dataCriacao!= new DateTime())
				{
					Param.Value = entity.sdr_dataCriacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sdr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.sdr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_SondagemResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@snd_id";
			Param.Size = 4;
			Param.Value = entity.snd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_id";
			Param.Size = 4;
			Param.Value = entity.sdr_id;
			qs.Parameters.Add(Param);
                
			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@sdr_sigla";
			Param.Size = 20;
			Param.Value = entity.sdr_sigla;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@sdr_descricao";
			Param.Size = 250;
			Param.Value = entity.sdr_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_ordem";
			Param.Size = 4;
			Param.Value = entity.sdr_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@sdr_situacao";
			Param.Size = 1;
			Param.Value = entity.sdr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sdr_dataCriacao";
			Param.Size = 16;
				if(entity.sdr_dataCriacao!= new DateTime())
				{
					Param.Value = entity.sdr_dataCriacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sdr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.sdr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_SondagemResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@snd_id";
			Param.Size = 4;
			Param.Value = entity.snd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@sdr_id";
			Param.Size = 4;
			Param.Value = entity.sdr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_SondagemResposta entity)
		{
			if (entity != null & qs != null)
            {
                entity.sdr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.sdr_id > 0);
            }

			return false;
		}		
	}
}