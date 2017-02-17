/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	
	/// <summary>
	/// Classe abstrata de MTR_MovimentacaoDadosAdicionais
	/// </summary>
	public abstract class Abstract_MTR_MovimentacaoDadosAdicionaisDAO : Abstract_DAL<MTR_MovimentacaoDadosAdicionais>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MovimentacaoDadosAdicionais entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mda_id";
			Param.Size = 4;
			Param.Value = entity.mda_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MovimentacaoDadosAdicionais entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mda_id";
			Param.Size = 4;
			Param.Value = entity.mda_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
			if( entity.eco_id > 0  )
				Param.Value = entity.eco_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cid_id";
			Param.Size = 16;
            if (entity.cid_id != Guid.Empty)
				Param.Value = entity.cid_id;
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@unf_id";
			Param.Size = 16;
            if (entity.unf_id != Guid.Empty)
                Param.Value = entity.unf_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_avaliacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mda_avaliacao) )
				Param.Value = entity.mda_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_observacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mda_observacao) )
				Param.Value = entity.mda_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@hop_id";
			Param.Size = 4;
			if( entity.hop_id > 0  )
				Param.Value = entity.hop_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mda_situacao";
			Param.Size = 1;
			Param.Value = entity.mda_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mda_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mda_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mda_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mda_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MovimentacaoDadosAdicionais entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mda_id";
			Param.Size = 4;
			Param.Value = entity.mda_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
			if( entity.eco_id > 0  )
				Param.Value = entity.eco_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@cid_id";
            Param.Size = 16;
            if (entity.cid_id != Guid.Empty)
                Param.Value = entity.cid_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@unf_id";
            Param.Size = 16;
            if (entity.unf_id != Guid.Empty)
                Param.Value = entity.unf_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_avaliacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mda_avaliacao) )
				Param.Value = entity.mda_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_observacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mda_observacao) )
				Param.Value = entity.mda_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@hop_id";
			Param.Size = 4;
			if( entity.hop_id > 0  )
				Param.Value = entity.hop_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mda_situacao";
			Param.Size = 1;
			Param.Value = entity.mda_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mda_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mda_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mda_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mda_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MovimentacaoDadosAdicionais entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mda_id";
			Param.Size = 4;
			Param.Value = entity.mda_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MovimentacaoDadosAdicionais entity)
		{
            entity.mda_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.mda_id > 0);
		}		
	}
}

