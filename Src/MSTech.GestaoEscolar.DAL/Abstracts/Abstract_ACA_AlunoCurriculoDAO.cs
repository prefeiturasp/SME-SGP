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
	/// Classe abstrata de ACA_AlunoCurriculo.
	/// </summary>
	public abstract class AbstractACA_AlunoCurriculoDAO : Abstract_DAL<ACA_AlunoCurriculo>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_matricula";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.alc_matricula))
				{
					Param.Value = entity.alc_matricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_codigoInep";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.alc_codigoInep))
				{
					Param.Value = entity.alc_codigoInep;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_registroGeral";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.alc_registroGeral))
            {
                Param.Value = entity.alc_registroGeral;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataPrimeiraMatricula";
			Param.Size = 20;
				if(entity.alc_dataPrimeiraMatricula!= new DateTime())
				{
					Param.Value = entity.alc_dataPrimeiraMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataSaida";
			Param.Size = 20;
				if(entity.alc_dataSaida!= new DateTime())
				{
					Param.Value = entity.alc_dataSaida;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataColacao";
			Param.Size = 20;
				if(entity.alc_dataColacao!= new DateTime())
				{
					Param.Value = entity.alc_dataColacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_matriculaEstadual";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.alc_matriculaEstadual))
				{
					Param.Value = entity.alc_matriculaEstadual;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_qtdeImpressoesHistorico";
			Param.Size = 4;
				if(entity.alc_qtdeImpressoesHistorico > 0 )
				{
					Param.Value = entity.alc_qtdeImpressoesHistorico;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alc_situacao";
			Param.Size = 1;
			Param.Value = entity.alc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alc_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_matricula";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.alc_matricula))
				{
					Param.Value = entity.alc_matricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_codigoInep";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.alc_codigoInep))
				{
					Param.Value = entity.alc_codigoInep;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_registroGeral";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.alc_registroGeral))
            {
                Param.Value = entity.alc_registroGeral;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataPrimeiraMatricula";
			Param.Size = 20;
				if(entity.alc_dataPrimeiraMatricula!= new DateTime())
				{
					Param.Value = entity.alc_dataPrimeiraMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataSaida";
			Param.Size = 20;
				if(entity.alc_dataSaida!= new DateTime())
				{
					Param.Value = entity.alc_dataSaida;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@alc_dataColacao";
			Param.Size = 20;
				if(entity.alc_dataColacao!= new DateTime())
				{
					Param.Value = entity.alc_dataColacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alc_matriculaEstadual";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.alc_matriculaEstadual))
				{
					Param.Value = entity.alc_matriculaEstadual;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_qtdeImpressoesHistorico";
			Param.Size = 4;
				if(entity.alc_qtdeImpressoesHistorico > 0 )
				{
					Param.Value = entity.alc_qtdeImpressoesHistorico;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alc_situacao";
			Param.Size = 1;
			Param.Value = entity.alc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alc_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}