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
	/// Classe abstrata de ACA_SondagemAgendamento.
	/// </summary>
	public abstract class Abstract_ACA_SondagemAgendamentoDAO : Abstract_DAL<ACA_SondagemAgendamento>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_SondagemAgendamento entity)
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
			Param.ParameterName = "@sda_id";
			Param.Size = 4;
			Param.Value = entity.sda_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_SondagemAgendamento entity)
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
			Param.ParameterName = "@sda_id";
			Param.Size = 4;
			Param.Value = entity.sda_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataInicio";
			Param.Size = 16;
			Param.Value = entity.sda_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataFim";
			Param.Size = 16;
			Param.Value = entity.sda_dataFim;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@sda_idRetificada";
            Param.Size = 4;
            if (entity.sda_idRetificada > 0)
                Param.Value = entity.sda_idRetificada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
                
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            if (entity.esc_id > 0)
                Param.Value = entity.esc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            if (entity.uni_id > 0)
                Param.Value = entity.uni_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@sda_situacao";
			Param.Size = 1;
			Param.Value = entity.sda_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.sda_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.sda_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_SondagemAgendamento entity)
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
			Param.ParameterName = "@sda_id";
			Param.Size = 4;
			Param.Value = entity.sda_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataInicio";
			Param.Size = 16;
			Param.Value = entity.sda_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataFim";
			Param.Size = 16;
			Param.Value = entity.sda_dataFim;
			qs.Parameters.Add(Param);
                
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@sda_idRetificada";
            Param.Size = 4;
            if (entity.sda_idRetificada > 0)
                Param.Value = entity.sda_idRetificada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
                
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            if (entity.esc_id > 0)
                Param.Value = entity.esc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            if (entity.uni_id > 0)
                Param.Value = entity.uni_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@sda_situacao";
			Param.Size = 1;
			Param.Value = entity.sda_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.sda_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@sda_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.sda_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_SondagemAgendamento entity)
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
			Param.ParameterName = "@sda_id";
			Param.Size = 4;
			Param.Value = entity.sda_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_SondagemAgendamento entity)
		{
			if (entity != null & qs != null)
            {
                entity.sda_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.sda_id > 0);
            }

			return false;
		}		
	}
}