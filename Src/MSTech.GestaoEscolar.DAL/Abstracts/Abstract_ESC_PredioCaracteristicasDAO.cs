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
	/// Classe abstrata de ESC_PredioCaracteristicas.
	/// </summary>
	public abstract class AbstractESC_PredioCaracteristicasDAO : Abstract_DAL<ESC_PredioCaracteristicas>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_PredioCaracteristicas entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_id";
			Param.Size = 4;
			Param.Value = entity.prc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_PredioCaracteristicas entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_id";
			Param.Size = 4;
			Param.Value = entity.prc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncPredioEscolar";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncPredioEscolar;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncTemploIgreja";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncTemploIgreja;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncSalasEmpresa";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncSalasEmpresa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncCasaProfessor";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncCasaProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncSalasOutraEscola";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncSalasOutraEscola;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncGalpao";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncGalpao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncUnidadeInternacaoPrisional";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncUnidadeInternacaoPrisional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncOutros";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_formaOcupacaoPredio";
			Param.Size = 1;
				if(entity.prc_formaOcupacaoPredio > 0 )
				{
					Param.Value = entity.prc_formaOcupacaoPredio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_aguaConsumida";
			Param.Size = 1;
				if(entity.prc_aguaConsumida > 0 )
				{
					Param.Value = entity.prc_aguaConsumida;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaPocoArtesiano";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaPocoArtesiano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaCacimba";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaCacimba;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaFonte";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaFonte;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaGerador";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaGerador;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaOutros";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoFossa";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoFossa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoColeta";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoColeta;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoQueima";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoQueima;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoJoga";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoJoga;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoRecicla";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoRecicla;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoEnterra";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoEnterra;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoOutros";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@prc_situacao";
			Param.Size = 1;
			Param.Value = entity.prc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.prc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.prc_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_PredioCaracteristicas entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_id";
			Param.Size = 4;
			Param.Value = entity.prc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncPredioEscolar";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncPredioEscolar;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncTemploIgreja";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncTemploIgreja;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncSalasEmpresa";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncSalasEmpresa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncCasaProfessor";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncCasaProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncSalasOutraEscola";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncSalasOutraEscola;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncGalpao";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncGalpao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncUnidadeInternacaoPrisional";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncUnidadeInternacaoPrisional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_localFuncOutros";
			Param.Size = 1;
				Param.Value = entity.prc_localFuncOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_formaOcupacaoPredio";
			Param.Size = 1;
				if(entity.prc_formaOcupacaoPredio > 0 )
				{
					Param.Value = entity.prc_formaOcupacaoPredio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_aguaConsumida";
			Param.Size = 1;
				if(entity.prc_aguaConsumida > 0 )
				{
					Param.Value = entity.prc_aguaConsumida;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaPocoArtesiano";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaPocoArtesiano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaCacimba";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaCacimba;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaFonte";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaFonte;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasAguaInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_abasAguaInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaGerador";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaGerador;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaOutros";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_abasEnergiaInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_abasEnergiaInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoRedePublica";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoRedePublica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoFossa";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoFossa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_esgotoInexistente";
			Param.Size = 1;
				Param.Value = entity.prc_esgotoInexistente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoColeta";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoColeta;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoQueima";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoQueima;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoJoga";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoJoga;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoRecicla";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoRecicla;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoEnterra";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoEnterra;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prc_destLixoOutros";
			Param.Size = 1;
				Param.Value = entity.prc_destLixoOutros;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@prc_situacao";
			Param.Size = 1;
			Param.Value = entity.prc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.prc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.prc_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_PredioCaracteristicas entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prc_id";
			Param.Size = 4;
			Param.Value = entity.prc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_PredioCaracteristicas entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}