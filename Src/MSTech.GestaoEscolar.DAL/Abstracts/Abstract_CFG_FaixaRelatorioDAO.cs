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
	/// Classe abstrata de CFG_FaixaRelatorio.
	/// </summary>
	public abstract class AbstractCFG_FaixaRelatorioDAO : Abstract_DAL<CFG_FaixaRelatorio>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_FaixaRelatorio entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@far_id";
			Param.Size = 4;
			Param.Value = entity.far_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlt_id";
			Param.Size = 4;
			Param.Value = entity.rlt_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_FaixaRelatorio entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlt_id";
			Param.Size = 4;
			Param.Value = entity.rlt_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@far_descricao";
			Param.Size = 200;
			Param.Value = entity.far_descricao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@far_inicio";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_inicio))
                Param.Value = entity.far_inicio;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@far_fim";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_fim))
                Param.Value = entity.far_fim;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            if (entity.esa_id > 0)
                Param.Value = entity.esa_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@eap_id";
            Param.Size = 4;
            if (entity.eap_id > 0)
                Param.Value = entity.eap_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@far_cor";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_cor))
                Param.Value = entity.far_cor;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@far_situacao";
			Param.Size = 1;
			Param.Value = entity.far_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@far_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.far_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@far_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.far_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CFG_FaixaRelatorio entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@far_id";
			Param.Size = 4;
			Param.Value = entity.far_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlt_id";
			Param.Size = 4;
			Param.Value = entity.rlt_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@far_descricao";
			Param.Size = 200;
			Param.Value = entity.far_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@far_inicio";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_inicio))
                Param.Value = entity.far_inicio;
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@far_fim";
			Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_fim))
                Param.Value = entity.far_fim;
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            if (entity.esa_id > 0)
                Param.Value = entity.esa_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@eap_id";
            Param.Size = 4;
            if (entity.eap_id > 0)
                Param.Value = entity.eap_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@far_cor";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.far_cor))
                Param.Value = entity.far_cor;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@far_situacao";
			Param.Size = 1;
			Param.Value = entity.far_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@far_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.far_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@far_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.far_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CFG_FaixaRelatorio entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@far_id";
			Param.Size = 4;
			Param.Value = entity.far_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlt_id";
			Param.Size = 4;
			Param.Value = entity.rlt_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_FaixaRelatorio entity)
		{
			if (entity != null & qs != null)
            {
			entity.far_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.far_id > 0);
			}

			return false;
		}		
	}
}