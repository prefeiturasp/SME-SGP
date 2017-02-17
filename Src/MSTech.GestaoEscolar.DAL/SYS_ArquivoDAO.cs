/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data.Common;

    /// <summary>
    ///
    /// </summary>
    public class SYS_ArquivoDAO : Abstract_SYS_ArquivoDAO
    {
        #region Sobrescritos

        /// <summary>
        /// Override da string de conexão com banco.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_Arquivo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@arq_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@arq_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, SYS_Arquivo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@arq_dataCriacao");
            qs.Parameters["@arq_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ORC_Objetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(SYS_Arquivo entity)
        {
            __STP_UPDATE = "NEW_SYS_Arquivo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, SYS_Arquivo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@arq_id";
            Param.Size = 4;
            Param.Value = entity.arq_id;
            qs.Parameters.Add(Param);

            if (!__STP_DELETE.Equals("STP_SYS_Arquivo_DELETE"))
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@arq_situacao";
                Param.Size = 1;
                Param.Value = 3;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@arq_dataAlteracao";
                Param.Size = 8;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade SYS_Arquivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(SYS_Arquivo entity)
        {
            __STP_DELETE = "NEW_SYS_Arquivo_UPDATE_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Método que faz a exclusão física.
        /// </summary>
        /// <param name="entity"> Entidade SYS_Arquivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public bool ExcluiFisicamente(SYS_Arquivo entity)
        {
            __STP_DELETE = "STP_SYS_Arquivo_DELETE";
            return base.Delete(entity);
        }

        /// <summary>
        /// Método sobrescrito para retirar o limite do timeout
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <returns>True em caso de sucesso</returns>
        public override bool Carregar(SYS_Arquivo entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure(this.__STP_LOAD, this._Banco);
            qs.TimeOut = 0;

            try
            {
                ParamCarregar(qs, entity);
                qs.Execute();
                if (qs.Return.Rows.Count > 0)
                {
                    entity = this.DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }
                return false;
            }
            catch (DbException err)
            {
                throw err;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os dados do arquivo por arq_id
        /// </summary>
        /// <param name="arq_id"></param>
        /// <returns>entity SYS_Arquivo</returns>
        public SYS_Arquivo NEW_SYS_Arquivo_SelecionaPorArquivo(int arq_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_Arquivo_SelecionaPorArquivo", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@Arq_id";
                Param.Size = 8;
                Param.Value = arq_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return DataRowToEntity(qs.Return.Rows[0], new SYS_Arquivo());    
                }
                else
                {
                    return new SYS_Arquivo();
                }
                
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

        #endregion Sobrescritos
    }
}