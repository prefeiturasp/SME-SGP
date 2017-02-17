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
	/// Classe abstrata de ACA_CalendarioAnual
	/// </summary>
	public abstract class Abstract_ACA_CalendarioAnualDAO : Abstract_DAL<ACA_CalendarioAnual>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_id";
			Param.Size = 4;
			Param.Value = entity.cal_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cal_padrao";
			Param.Size = 1;
				Param.Value = entity.cal_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_ano";
			Param.Size = 4;
			Param.Value = entity.cal_ano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cal_descricao";
			Param.Size = 200;
			Param.Value = entity.cal_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataInicio";
			Param.Size = 16;
			Param.Value = entity.cal_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataFim";
			Param.Size = 16;
			Param.Value = entity.cal_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cal_situacao";
			Param.Size = 1;
			Param.Value = entity.cal_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cal_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cal_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@cal_permiteLancamentoRecesso";
            Param.Size = 1;
            Param.Value = entity.cal_permiteLancamentoRecesso;
            qs.Parameters.Add(Param);
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_id";
			Param.Size = 4;
			Param.Value = entity.cal_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cal_padrao";
			Param.Size = 1;
				Param.Value = entity.cal_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_ano";
			Param.Size = 4;
			Param.Value = entity.cal_ano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cal_descricao";
			Param.Size = 200;
			Param.Value = entity.cal_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataInicio";
			Param.Size = 16;
			Param.Value = entity.cal_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataFim";
			Param.Size = 16;
			Param.Value = entity.cal_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cal_situacao";
			Param.Size = 1;
			Param.Value = entity.cal_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cal_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cal_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cal_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@cal_permiteLancamentoRecesso";
            Param.Size = 1;
            Param.Value = entity.cal_permiteLancamentoRecesso;
            qs.Parameters.Add(Param);
        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_id";
			Param.Size = 4;
			Param.Value = entity.cal_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
		{
			entity.cal_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.cal_id > 0);
		}		
	}
}

