/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class MTR_DocumentoMatriculaDAO : Abstract_MTR_DocumentoMatriculaDAO
    {
        #region Métodos Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_DocumentoMatricula entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@dmt_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@dmt_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_DocumentoMatricula entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@dmt_dataCriacao");
            qs.Parameters["@dmt_dataAlteracao"].Value = DateTime.Now;
        }
                
        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(MTR_DocumentoMatricula entity)
        {
            __STP_UPDATE = "NEW_MTR_DocumentoMatricula_UPDATE";
            return base.Alterar(entity);
        }
       
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_DocumentoMatricula entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@dmt_id";
            Param.Size = 4;
            Param.Value = entity.dmt_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@dmt_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dmt_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(MTR_DocumentoMatricula entity)
        {
            __STP_DELETE = "NEW_MTR_DocumentoMatricula_UPDATEBy_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}