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
	/// Classe abstrata de ACA_Curso.
	/// </summary>
	public abstract class Abstract_ACA_CursoDAO : Abstract_DAL<ACA_Curso>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Curso entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Curso entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_padrao";
			Param.Size = 1;
			Param.Value = entity.cur_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_codigo";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.cur_codigo))
				{
					Param.Value = entity.cur_codigo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_nome";
			Param.Size = 200;
			Param.Value = entity.cur_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_nome_abreviado";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.cur_nome_abreviado))
				{
					Param.Value = entity.cur_nome_abreviado;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_concluiNivelEnsino";
			Param.Size = 1;
				Param.Value = entity.cur_concluiNivelEnsino;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_exclusivoDeficiente";
			Param.Size = 1;
			Param.Value = entity.cur_exclusivoDeficiente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_idProximo";
			Param.Size = 4;
				if(entity.tne_idProximo > 0 )
				{
					Param.Value = entity.tne_idProximo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Date;
			Param.ParameterName = "@cur_vigenciaInicio";

			Param.Value = entity.cur_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Date;
			Param.ParameterName = "@cur_vigenciaFim";

				if(entity.cur_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.cur_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_efetivacaoSemestral";
			Param.Size = 1;
			Param.Value = entity.cur_efetivacaoSemestral;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_conflitoTurnoSR";
			Param.Size = 1;
			Param.Value = entity.cur_conflitoTurnoSR;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cur_situacao";
			Param.Size = 1;
			Param.Value = entity.cur_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@cur_cargaHoraria";
			Param.Value = entity.cur_cargaHoraria;
			qs.Parameters.Add(Param);
			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Curso entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_padrao";
			Param.Size = 1;
			Param.Value = entity.cur_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_codigo";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.cur_codigo))
				{
					Param.Value = entity.cur_codigo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_nome";
			Param.Size = 200;
			Param.Value = entity.cur_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@cur_nome_abreviado";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.cur_nome_abreviado))
				{
					Param.Value = entity.cur_nome_abreviado;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_concluiNivelEnsino";
			Param.Size = 1;
				Param.Value = entity.cur_concluiNivelEnsino;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_exclusivoDeficiente";
			Param.Size = 1;
			Param.Value = entity.cur_exclusivoDeficiente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_idProximo";
			Param.Size = 4;
				if(entity.tne_idProximo > 0 )
				{
					Param.Value = entity.tne_idProximo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Date;
			Param.ParameterName = "@cur_vigenciaInicio";

			Param.Value = entity.cur_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Date;
			Param.ParameterName = "@cur_vigenciaFim";

				if(entity.cur_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.cur_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_efetivacaoSemestral";
			Param.Size = 1;
			Param.Value = entity.cur_efetivacaoSemestral;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cur_conflitoTurnoSR";
			Param.Size = 1;
				Param.Value = entity.cur_conflitoTurnoSR;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cur_situacao";
			Param.Size = 1;
			Param.Value = entity.cur_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cur_dataCriacao";

			Param.Value = entity.cur_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cur_dataAlteracao";

			Param.Value = entity.cur_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@cur_cargaHoraria";

				Param.Value = entity.cur_cargaHoraria;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Curso entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Curso entity)
		{
			if (entity != null & qs != null)
            {
			entity.cur_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.cur_id > 0);
			}

			return false;
		}		
	}
}