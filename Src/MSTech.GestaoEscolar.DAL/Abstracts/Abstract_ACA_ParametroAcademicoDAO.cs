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
	/// Classe abstrata de ACA_ParametroAcademico
	/// </summary>
	public abstract class Abstract_ACA_ParametroAcademicoDAO : Abstract_DAL<ACA_ParametroAcademico>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ParametroAcademico entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;            
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pac_id";
			Param.Size = 4;
			Param.Value = entity.pac_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ParametroAcademico entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_chave";
			Param.Size = 100;
			Param.Value = entity.pac_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_valor";
			Param.Size = 1000;
			Param.Value = entity.pac_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pac_descricao) )
				Param.Value = entity.pac_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pac_obrigatorio";
			Param.Size = 1;
				Param.Value = entity.pac_obrigatorio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pac_situacao";
			Param.Size = 1;
			Param.Value = entity.pac_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_vigenciaInicio";
			Param.Size = 16;
			Param.Value = entity.pac_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_vigenciaFim";
			Param.Size = 16;
			if( entity.pac_vigenciaFim!= new DateTime() )
				Param.Value = entity.pac_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pac_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pac_dataAlteracao;
			qs.Parameters.Add(Param);

		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ParametroAcademico entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;            
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pac_id";
			Param.Size = 4;
			Param.Value = entity.pac_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_chave";
			Param.Size = 100;
			Param.Value = entity.pac_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_valor";
			Param.Size = 1000;
			Param.Value = entity.pac_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pac_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pac_descricao) )
				Param.Value = entity.pac_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pac_obrigatorio";
			Param.Size = 1;
				Param.Value = entity.pac_obrigatorio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pac_situacao";
			Param.Size = 1;
			Param.Value = entity.pac_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_vigenciaInicio";
			Param.Size = 16;
			Param.Value = entity.pac_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_vigenciaFim";
			Param.Size = 16;
			if( entity.pac_vigenciaFim!= new DateTime() )
				Param.Value = entity.pac_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pac_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pac_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pac_dataAlteracao;
			qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ParametroAcademico entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);   
            
            Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pac_id";
			Param.Size = 4;
			Param.Value = entity.pac_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ParametroAcademico entity)
		{
			entity.pac_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.pac_id > 0);
		}		
	}
}

