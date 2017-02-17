/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ESC_EscolaClassificacaoVigenciaDAO : AbstractESC_EscolaClassificacaoVigenciaDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        public DataTable SelecionaAtivas(int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_EscolaClassificacaoVigencia_SelecionaAtiva", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@ecv_dataCriacao");
            qs.Parameters["@ecv_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Insere os valores da classe em um registro ja existente.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(ESC_EscolaClassificacaoVigencia entity)
        {
            __STP_UPDATE = "NEW_ESC_EscolaClassificacaoVigencia_UPDATE";
            return base.Alterar(entity);
        }


        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@ecv_id";
            Param.Size = 8;
            Param.Value = entity.ecv_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ecv_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ecv_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Exclui logicamente um registro do banco.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem apagados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(ESC_EscolaClassificacaoVigencia entity)
        {
            __STP_DELETE = "NEW_ESC_EscolaClassificacaoVigencia_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}