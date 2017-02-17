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
	/// Classe abstrata de CLS_TurmaAula.
	/// </summary>
	public abstract class Abstract_CLS_TurmaAulaDAO : Abstract_DAL<CLS_TurmaAula>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaAula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			Param.Value = entity.tau_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAula entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			Param.Value = entity.tau_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			Param.Value = entity.tpc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_sequencia";
			Param.Size = 4;
				if(entity.tau_sequencia > 0 )
				{
					Param.Value = entity.tau_sequencia;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tau_data";
			Param.Size = 20;
				if(entity.tau_data!= new DateTime())
				{
					Param.Value = entity.tau_data;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_numeroAulas";
			Param.Size = 4;
				if(entity.tau_numeroAulas > 0 )
				{
					Param.Value = entity.tau_numeroAulas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_planoAula";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_planoAula))
				{
					Param.Value = entity.tau_planoAula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_diarioClasse";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_diarioClasse))
				{
					Param.Value = entity.tau_diarioClasse;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_conteudo";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_conteudo))
				{
					Param.Value = entity.tau_conteudo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_efetivado";
			Param.Size = 1;
				Param.Value = entity.tau_efetivado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tau_atividadeCasa";
			Param.Size = 2147483646;
				if(!string.IsNullOrEmpty(entity.tau_atividadeCasa))
				{
					Param.Value = entity.tau_atividadeCasa;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_situacao";
			Param.Size = 1;
			Param.Value = entity.tau_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tau_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tau_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdt_posicao";
			Param.Size = 1;
			Param.Value = entity.tdt_posicao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pro_id";
			Param.Size = 16;
				Param.Value = entity.pro_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tau_sintese";
			Param.Size = 2147483646;
				if(!string.IsNullOrEmpty(entity.tau_sintese))
				{
					Param.Value = entity.tau_sintese;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_reposicao";
			Param.Size = 1;
			Param.Value = entity.tau_reposicao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_id";
			Param.Size = 16;
				Param.Value = entity.usu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_idDocenteAlteracao";
			Param.Size = 16;
				Param.Value = entity.usu_idDocenteAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusFrequencia";
			Param.Size = 1;
			Param.Value = entity.tau_statusFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusAtividadeAvaliativa";
			Param.Size = 1;
			Param.Value = entity.tau_statusAtividadeAvaliativa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusAnotacoes";
			Param.Size = 1;
			Param.Value = entity.tau_statusAnotacoes;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusPlanoAula";
			Param.Size = 1;
			Param.Value = entity.tau_statusPlanoAula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_checadoAtividadeCasa";
			Param.Size = 1;
				Param.Value = entity.tau_checadoAtividadeCasa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataUltimaSincronizacao";
			Param.Size = 16;
				if(entity.tau_dataUltimaSincronizacao!= new DateTime())
				{
					Param.Value = entity.tau_dataUltimaSincronizacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			Param.Value = entity.tau_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			Param.Value = entity.tpc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_sequencia";
			Param.Size = 4;
				if(entity.tau_sequencia > 0 )
				{
					Param.Value = entity.tau_sequencia;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tau_data";
			Param.Size = 20;
				if(entity.tau_data!= new DateTime())
				{
					Param.Value = entity.tau_data;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_numeroAulas";
			Param.Size = 4;
				if(entity.tau_numeroAulas > 0 )
				{
					Param.Value = entity.tau_numeroAulas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_planoAula";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_planoAula))
				{
					Param.Value = entity.tau_planoAula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_diarioClasse";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_diarioClasse))
				{
					Param.Value = entity.tau_diarioClasse;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tau_conteudo";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.tau_conteudo))
				{
					Param.Value = entity.tau_conteudo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_efetivado";
			Param.Size = 1;
				Param.Value = entity.tau_efetivado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tau_atividadeCasa";
			Param.Size = 2147483646;
				if(!string.IsNullOrEmpty(entity.tau_atividadeCasa))
				{
					Param.Value = entity.tau_atividadeCasa;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_situacao";
			Param.Size = 1;
			Param.Value = entity.tau_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tau_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tau_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdt_posicao";
			Param.Size = 1;
			Param.Value = entity.tdt_posicao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pro_id";
			Param.Size = 16;
				Param.Value = entity.pro_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tau_sintese";
			Param.Size = 2147483646;
				if(!string.IsNullOrEmpty(entity.tau_sintese))
				{
					Param.Value = entity.tau_sintese;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_reposicao";
			Param.Size = 1;
			Param.Value = entity.tau_reposicao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_id";
			Param.Size = 16;
				Param.Value = entity.usu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_idDocenteAlteracao";
			Param.Size = 16;
				Param.Value = entity.usu_idDocenteAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusFrequencia";
			Param.Size = 1;
			Param.Value = entity.tau_statusFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusAtividadeAvaliativa";
			Param.Size = 1;
			Param.Value = entity.tau_statusAtividadeAvaliativa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusAnotacoes";
			Param.Size = 1;
			Param.Value = entity.tau_statusAnotacoes;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tau_statusPlanoAula";
			Param.Size = 1;
			Param.Value = entity.tau_statusPlanoAula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tau_checadoAtividadeCasa";
			Param.Size = 1;
				Param.Value = entity.tau_checadoAtividadeCasa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tau_dataUltimaSincronizacao";
			Param.Size = 16;
				if(entity.tau_dataUltimaSincronizacao!= new DateTime())
				{
					Param.Value = entity.tau_dataUltimaSincronizacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			Param.Value = entity.tau_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAula entity)
		{
			if (entity != null & qs != null)
            {
return true;
			}

			return false;
		}		
	}
}