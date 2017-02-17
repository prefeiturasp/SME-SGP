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
	/// Classe abstrata de RHU_CargaHoraria
	/// </summary>
	public abstract class Abstract_RHU_CargaHorariaDAO : Abstract_DAL<RHU_CargaHoraria>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, RHU_CargaHoraria entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_id";
			Param.Size = 4;
			Param.Value = entity.chr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_CargaHoraria entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@chr_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.chr_descricao) )
				Param.Value = entity.chr_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@chr_padrao";
			Param.Size = 1;
			Param.Value = entity.chr_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@chr_especialista";
			Param.Size = 1;

            if (entity.chr_especialista == null)
                Param.Value = DBNull.Value;
            else
                Param.Value = entity.chr_especialista;

			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			if( entity.crg_id > 0  )
				Param.Value = entity.crg_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_cargaHorariaSemanal";
			Param.Size = 4;
			Param.Value = entity.chr_cargaHorariaSemanal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_temposAula";
			Param.Size = 4;
			if( entity.chr_temposAula > 0  )
				Param.Value = entity.chr_temposAula;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_horasAula";
			Param.Size = 4;
			if( entity.chr_horasAula > 0  )
				Param.Value = entity.chr_horasAula;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_horasComplementares";
			Param.Size = 4;
			if( entity.chr_horasComplementares > 0  )
				Param.Value = entity.chr_horasComplementares;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@chr_situacao";
			Param.Size = 1;
			Param.Value = entity.chr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@chr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.chr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@chr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.chr_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, RHU_CargaHoraria entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_id";
			Param.Size = 4;
			Param.Value = entity.chr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@chr_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.chr_descricao) )
				Param.Value = entity.chr_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@chr_padrao";
			Param.Size = 1;
			Param.Value = entity.chr_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@chr_especialista";
			Param.Size = 1;

            if (entity.chr_especialista == null)
                Param.Value = DBNull.Value;                
            else
                Param.Value = entity.chr_especialista;

			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			if( entity.crg_id > 0  )
				Param.Value = entity.crg_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_cargaHorariaSemanal";
			Param.Size = 4;
			Param.Value = entity.chr_cargaHorariaSemanal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_temposAula";
			Param.Size = 4;
			if( entity.chr_temposAula > 0  )
				Param.Value = entity.chr_temposAula;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_horasAula";
			Param.Size = 4;
			if( entity.chr_horasAula > 0  )
				Param.Value = entity.chr_horasAula;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_horasComplementares";
			Param.Size = 4;
			if( entity.chr_horasComplementares > 0  )
				Param.Value = entity.chr_horasComplementares;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@chr_situacao";
			Param.Size = 1;
			Param.Value = entity.chr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@chr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.chr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@chr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.chr_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, RHU_CargaHoraria entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_id";
			Param.Size = 4;
			Param.Value = entity.chr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_CargaHoraria entity)
		{
			entity.chr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.chr_id > 0);
		}		
	}
}

