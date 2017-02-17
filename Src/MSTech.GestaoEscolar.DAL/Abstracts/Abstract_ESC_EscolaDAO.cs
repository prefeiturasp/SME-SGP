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
	/// Classe abstrata de ESC_Escola
	/// </summary>
	public abstract class Abstract_ESC_EscolaDAO : Abstract_DAL<ESC_Escola>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_Escola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_Escola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_id";
			Param.Size = 16;
			Param.Value = entity.uad_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_codigo";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.esc_codigo) )
				Param.Value = entity.esc_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_nome";
			Param.Size = 200;
			Param.Value = entity.esc_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_codigoInep";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.esc_codigoInep) )
				Param.Value = entity.esc_codigoInep;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_codigoNumeroMatricula";
			Param.Size = 4;
			if( entity.esc_codigoNumeroMatricula > 0  )
				Param.Value = entity.esc_codigoNumeroMatricula;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tre_id";
			Param.Size = 4;
			Param.Value = entity.tre_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_funcionamentoInicio";
			Param.Size = 20;
			if( entity.esc_funcionamentoInicio!= new DateTime() )
				Param.Value = entity.esc_funcionamentoInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_funcionamentoFim";
			Param.Size = 20;
			if( entity.esc_funcionamentoFim!= new DateTime() )
				Param.Value = entity.esc_funcionamentoFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@esc_fundoVerso";
            Param.Size = 260;
            if (!string.IsNullOrEmpty(entity.esc_fundoVerso))
                Param.Value = entity.esc_fundoVerso;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@esc_situacao";
			Param.Size = 1;
			Param.Value = entity.esc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.esc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.esc_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@esc_controleSistema";
			Param.Size = 1;
			Param.Value = entity.esc_controleSistema;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_autorizada";
            Param.Size = 1;
            if (entity.esc_autorizada > 0)
                Param.Value = entity.esc_autorizada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_atoCriacao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.esc_atoCriacao) )
				Param.Value = entity.esc_atoCriacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_dataPublicacaoDiarioOficial";
			Param.Size = 20;
			if( entity.esc_dataPublicacaoDiarioOficial!= new DateTime() )
				Param.Value = entity.esc_dataPublicacaoDiarioOficial;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_idSuperiorGestao";
            Param.Size = 16;
            Param.Value = entity.uad_idSuperior;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@esc_terceirizada";
            Param.Size = 1;
            Param.Value = entity.esc_terceirizada;
            qs.Parameters.Add(Param);
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_Escola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_id";
			Param.Size = 16;
			Param.Value = entity.uad_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_codigo";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.esc_codigo) )
				Param.Value = entity.esc_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_nome";
			Param.Size = 200;
			Param.Value = entity.esc_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_codigoInep";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.esc_codigoInep) )
				Param.Value = entity.esc_codigoInep;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_codigoNumeroMatricula";
			Param.Size = 4;
			if( entity.esc_codigoNumeroMatricula > 0  )
				Param.Value = entity.esc_codigoNumeroMatricula;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tre_id";
			Param.Size = 4;
			Param.Value = entity.tre_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_funcionamentoInicio";
			Param.Size = 20;
			if( entity.esc_funcionamentoInicio!= new DateTime() )
				Param.Value = entity.esc_funcionamentoInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_funcionamentoFim";
			Param.Size = 20;
			if( entity.esc_funcionamentoFim!= new DateTime() )
				Param.Value = entity.esc_funcionamentoFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@esc_fundoVerso";
            Param.Size = 260;
            if (!string.IsNullOrEmpty(entity.esc_fundoVerso))
                Param.Value = entity.esc_fundoVerso;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@esc_situacao";
			Param.Size = 1;
			Param.Value = entity.esc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.esc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.esc_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@esc_controleSistema";
			Param.Size = 1;
			Param.Value = entity.esc_controleSistema;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_autorizada";
            Param.Size = 1;
            if (entity.esc_autorizada > 0)
                Param.Value = entity.esc_autorizada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esc_atoCriacao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.esc_atoCriacao) )
				Param.Value = entity.esc_atoCriacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@esc_dataPublicacaoDiarioOficial";
			Param.Size = 20;
			if( entity.esc_dataPublicacaoDiarioOficial!= new DateTime() )
				Param.Value = entity.esc_dataPublicacaoDiarioOficial;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_idSuperiorGestao";
            Param.Size = 16;
            Param.Value = entity.uad_idSuperior;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@esc_terceirizada";
            Param.Size = 1;
            Param.Value = entity.esc_terceirizada;
            qs.Parameters.Add(Param);
        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_Escola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_Escola entity)
		{
			entity.esc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.esc_id > 0);
		}		
	}
}

