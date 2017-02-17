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
	/// Classe abstrata de MTR_ParametroFormacaoTurma
	/// </summary>
	public abstract class Abstract_MTR_ParametroFormacaoTurmaDAO : Abstract_DAL<MTR_ParametroFormacaoTurma>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_id";
			Param.Size = 4;
			Param.Value = entity.pft_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_id";
			Param.Size = 4;
			Param.Value = entity.pft_id;
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
			if( entity.crp_id > 0  )
				Param.Value = entity.crp_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pft_tipo";
			Param.Size = 1;
			Param.Value = entity.pft_tipo;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pft_tipoControleCapacidade";
            Param.Size = 1;
            Param.Value = entity.pft_tipoControleCapacidade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_capacidade";
            Param.Size = 4;
            Param.Value = entity.pft_capacidade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pft_tipoControleDeficiente";
            Param.Size = 1;
            Param.Value = entity.pft_tipoControleDeficiente;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_qtdDeficiente";
			Param.Size = 4;
			if( entity.pft_qtdDeficiente > 0  )
				Param.Value = entity.pft_qtdDeficiente;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_capacidadeComDeficiente";
			Param.Size = 4;
			if( entity.pft_capacidadeComDeficiente > 0  )
				Param.Value = entity.pft_capacidadeComDeficiente;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pft_prefixoCodigoTurma";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.pft_prefixoCodigoTurma) )
				Param.Value = entity.pft_prefixoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_qtdDigitoCodigoTurma";
			Param.Size = 4;
			if( entity.pft_qtdDigitoCodigoTurma > 0  )
				Param.Value = entity.pft_qtdDigitoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_tipoDigitoCodigoTurma";
			Param.Size = 1;
			if( entity.pft_tipoDigitoCodigoTurma > 0  )
				Param.Value = entity.pft_tipoDigitoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_cargaHorariaSemanal";
			Param.Size = 4;
			if( entity.pft_cargaHorariaSemanal > 0  )
				Param.Value = entity.pft_cargaHorariaSemanal;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_id";
			Param.Size = 4;
			Param.Value = entity.cal_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pft_docenteEspecialista";
			Param.Size = 1;
			Param.Value = entity.pft_docenteEspecialista;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pft_situacao";
			Param.Size = 1;
			Param.Value = entity.pft_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pft_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pft_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pft_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pft_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ParametroFormacaoTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_id";
			Param.Size = 4;
			Param.Value = entity.pft_id;
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
			if( entity.crp_id > 0  )
				Param.Value = entity.crp_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pft_tipo";
			Param.Size = 1;
			Param.Value = entity.pft_tipo;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pft_tipoControleCapacidade";
            Param.Size = 1;
            Param.Value = entity.pft_tipoControleCapacidade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_capacidade";
            Param.Size = 4;
            Param.Value = entity.pft_capacidade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pft_tipoControleDeficiente";
            Param.Size = 1;
            Param.Value = entity.pft_tipoControleDeficiente;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_qtdDeficiente";
			Param.Size = 4;
			if( entity.pft_qtdDeficiente > 0  )
				Param.Value = entity.pft_qtdDeficiente;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_capacidadeComDeficiente";
			Param.Size = 4;
			if( entity.pft_capacidadeComDeficiente > 0  )
				Param.Value = entity.pft_capacidadeComDeficiente;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pft_prefixoCodigoTurma";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.pft_prefixoCodigoTurma) )
				Param.Value = entity.pft_prefixoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_qtdDigitoCodigoTurma";
			Param.Size = 4;
			if( entity.pft_qtdDigitoCodigoTurma > 0  )
				Param.Value = entity.pft_qtdDigitoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_tipoDigitoCodigoTurma";
			Param.Size = 1;
			if( entity.pft_tipoDigitoCodigoTurma > 0  )
				Param.Value = entity.pft_tipoDigitoCodigoTurma;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_cargaHorariaSemanal";
			Param.Size = 4;
			if( entity.pft_cargaHorariaSemanal > 0  )
				Param.Value = entity.pft_cargaHorariaSemanal;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_id";
			Param.Size = 4;
			Param.Value = entity.cal_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pft_docenteEspecialista";
			Param.Size = 1;
			Param.Value = entity.pft_docenteEspecialista;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pft_situacao";
			Param.Size = 1;
			Param.Value = entity.pft_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pft_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pft_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pft_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pft_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ParametroFormacaoTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pft_id";
			Param.Size = 4;
			Param.Value = entity.pft_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurma entity)
		{
            entity.pft_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.pft_id > 0);
		}		
	}
}

