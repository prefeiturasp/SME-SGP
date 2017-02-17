/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL
{
	using MSTech.GestaoEscolar.DAL.Abstracts;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_AvisoTextoGeralDAO : AbstractACA_AvisoTextoGeralDAO
    {

        #region Métodos de consulta


        /// <summary>
        /// Selectby_atg_tipoes the specified atg_tipo.
        /// </summary>
        /// <param name="atg_tipo">The atg_tipo.</param>
        /// <returns></returns>
        public DataTable SelecionaPorTipoAviso(int atg_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AvisoTextoGeral_SelecionaPorTipoAviso", _Banco);
            try
            {

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@atg_tipo";
                Param.Size = 1;
                Param.Value = atg_tipo;
                qs.Parameters.Add(Param);

                #endregion


                qs.Execute();
                
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca cabecalho
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable SelecionaCabecalho(int esc_id, int uni_id, Guid fone, Guid email)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AvisoTextoGeral_Cabecalho", _Banco);
            try
            {

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@fone";
                Param.Size = 16;
                Param.Value = fone;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@email";
                Param.Size = 16;
                Param.Value = email;
                qs.Parameters.Add(Param);

                #endregion


                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        #endregion

        #region Sobrescritos

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ACA_AvisoTextoGeral entity)
        {
            entity.atg_dataCriacao = DateTime.Now;
            entity.atg_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ACA_AvisoTextoGeral entity)
        {
            entity.atg_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@atg_dataCriacao");
        }

        /// <summary>
        /// Alterars the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override bool Alterar(Entities.ACA_AvisoTextoGeral entity)
        {
            __STP_UPDATE = "NEW_ACA_AvisoTextoGeral_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.ACA_AvisoTextoGeral entity)
        {

            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@atg_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@atg_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override bool Delete(Entities.ACA_AvisoTextoGeral entity)
        {
            __STP_DELETE = "NEW_ACA_AvisoTextoGeral_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Sobrescritos

    }
}