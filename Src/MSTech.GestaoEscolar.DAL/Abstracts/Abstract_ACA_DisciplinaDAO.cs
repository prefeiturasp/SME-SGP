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
	/// Classe abstrata de ACA_Disciplina
	/// </summary>
	public abstract class Abstract_ACA_DisciplinaDAO : Abstract_DAL<ACA_Disciplina>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_id";
			Param.Size = 4;
			Param.Value = entity.dis_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			Param.Value = entity.tds_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_codigo";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.dis_codigo) )
				Param.Value = entity.dis_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nome";
			Param.Size = 200;
			Param.Value = entity.dis_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nomeAbreviado";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.dis_nomeAbreviado) )
				Param.Value = entity.dis_nomeAbreviado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nomeDocumentacao";
			Param.Size = 40;
			if( !string.IsNullOrEmpty(entity.dis_nomeDocumentacao) )
				Param.Value = entity.dis_nomeDocumentacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@dis_ementa";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_ementa) )
				Param.Value = entity.dis_ementa;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_objetivos";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_objetivos) )
				Param.Value = entity.dis_objetivos;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_habilidades";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_habilidades) )
				Param.Value = entity.dis_habilidades;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_metodologias";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_metodologias) )
				Param.Value = entity.dis_metodologias;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaTeorica";
			Param.Size = 4;
			if( entity.dis_cargaHorariaTeorica > 0  )
				Param.Value = entity.dis_cargaHorariaTeorica;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaPratica";
			Param.Size = 4;
			if( entity.dis_cargaHorariaPratica > 0  )
				Param.Value = entity.dis_cargaHorariaPratica;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaSupervisionada";
			Param.Size = 4;
			if( entity.dis_cargaHorariaSupervisionada > 0  )
				Param.Value = entity.dis_cargaHorariaSupervisionada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaExtra";
			Param.Size = 4;
			if( entity.dis_cargaHorariaExtra > 0  )
				Param.Value = entity.dis_cargaHorariaExtra;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaAnual";
			Param.Size = 4;
			if( entity.dis_cargaHorariaAnual > 0  )
				Param.Value = entity.dis_cargaHorariaAnual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@dis_situacao";
			Param.Size = 1;
			Param.Value = entity.dis_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@dis_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.dis_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@dis_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.dis_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Disciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_id";
			Param.Size = 4;
			Param.Value = entity.dis_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			Param.Value = entity.tds_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_codigo";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.dis_codigo) )
				Param.Value = entity.dis_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nome";
			Param.Size = 200;
			Param.Value = entity.dis_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nomeAbreviado";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.dis_nomeAbreviado) )
				Param.Value = entity.dis_nomeAbreviado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_nomeDocumentacao";
			Param.Size = 40;
			if( !string.IsNullOrEmpty(entity.dis_nomeDocumentacao) )
				Param.Value = entity.dis_nomeDocumentacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@dis_ementa";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_ementa) )
				Param.Value = entity.dis_ementa;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_objetivos";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_objetivos) )
				Param.Value = entity.dis_objetivos;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_habilidades";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_habilidades) )
				Param.Value = entity.dis_habilidades;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@dis_metodologias";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.dis_metodologias) )
				Param.Value = entity.dis_metodologias;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaTeorica";
			Param.Size = 4;
			if( entity.dis_cargaHorariaTeorica > 0  )
				Param.Value = entity.dis_cargaHorariaTeorica;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaPratica";
			Param.Size = 4;
			if( entity.dis_cargaHorariaPratica > 0  )
				Param.Value = entity.dis_cargaHorariaPratica;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaSupervisionada";
			Param.Size = 4;
			if( entity.dis_cargaHorariaSupervisionada > 0  )
				Param.Value = entity.dis_cargaHorariaSupervisionada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaExtra";
			Param.Size = 4;
			if( entity.dis_cargaHorariaExtra > 0  )
				Param.Value = entity.dis_cargaHorariaExtra;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_cargaHorariaAnual";
			Param.Size = 4;
			if( entity.dis_cargaHorariaAnual > 0  )
				Param.Value = entity.dis_cargaHorariaAnual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@dis_situacao";
			Param.Size = 1;
			Param.Value = entity.dis_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@dis_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.dis_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@dis_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.dis_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Disciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_id";
			Param.Size = 4;
			Param.Value = entity.dis_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
		{
			entity.dis_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.dis_id > 0);
		}		
	}
}

