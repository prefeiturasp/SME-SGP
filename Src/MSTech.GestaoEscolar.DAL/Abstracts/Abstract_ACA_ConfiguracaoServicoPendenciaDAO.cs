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
	/// Classe abstrata de ACA_ConfiguracaoServicoPendencia.
	/// </summary>
	public abstract class Abstract_ACA_ConfiguracaoServicoPendenciaDAO : Abstract_DAL<ACA_ConfiguracaoServicoPendencia>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tur_tipo";
			Param.Size = 1;
			Param.Value = entity.tur_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_id";
			Param.Size = 4;
			Param.Value = entity.csp_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tur_tipo";
			Param.Size = 1;
			Param.Value = entity.tur_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_id";
			Param.Size = 4;
			Param.Value = entity.csp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semNota";
			Param.Size = 1;
			Param.Value = entity.csp_semNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semParecer";
			Param.Size = 1;
			Param.Value = entity.csp_semParecer;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_disciplinaSemAula";
			Param.Size = 1;
			Param.Value = entity.csp_disciplinaSemAula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semResultadoFinal";
			Param.Size = 1;
			Param.Value = entity.csp_semResultadoFinal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semPlanejamento";
			Param.Size = 1;
			Param.Value = entity.csp_semPlanejamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semSintese";
			Param.Size = 1;
			Param.Value = entity.csp_semSintese;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_situacao";
			Param.Size = 4;
			Param.Value = entity.csp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@csp_dataAlteracao";

			Param.Value = entity.csp_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@csp_dataCriacao";

			Param.Value = entity.csp_dataCriacao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tur_tipo";
			Param.Size = 1;
			Param.Value = entity.tur_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_id";
			Param.Size = 4;
			Param.Value = entity.csp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semNota";
			Param.Size = 1;
			Param.Value = entity.csp_semNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semParecer";
			Param.Size = 1;
			Param.Value = entity.csp_semParecer;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_disciplinaSemAula";
			Param.Size = 1;
			Param.Value = entity.csp_disciplinaSemAula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semResultadoFinal";
			Param.Size = 1;
			Param.Value = entity.csp_semResultadoFinal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semPlanejamento";
			Param.Size = 1;
			Param.Value = entity.csp_semPlanejamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@csp_semSintese";
			Param.Size = 1;
			Param.Value = entity.csp_semSintese;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_situacao";
			Param.Size = 4;
			Param.Value = entity.csp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@csp_dataAlteracao";

			Param.Value = entity.csp_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@csp_dataCriacao";

			Param.Value = entity.csp_dataCriacao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tur_tipo";
			Param.Size = 1;
			Param.Value = entity.tur_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@csp_id";
			Param.Size = 4;
			Param.Value = entity.csp_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
		{
			if (entity != null & qs != null)
            {
                entity.csp_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.csp_id > 0);
            }

            return false;
		}		
	}
}