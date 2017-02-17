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
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_TipoDocenteDAO : Abstract_ACA_TipoDocenteDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona todos os tipos de docentes ativos.
        /// </summary>
        /// <returns></returns>
        public List<ACA_TipoDocente> SelecionaAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDocente_SelecionaAtivos", _Banco);

            try
            {
                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new ACA_TipoDocente())).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a posição do tipo de docente.
        /// </summary>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <returns>Posição do docente.</returns>
        public byte SelecionaPosicaoPorTipoDocente(byte tdc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_TipoDocente_SelecionaPosicaoPorTipoDocente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToByte(qs.Return);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna o tipo de docente pela posição.
        /// </summary>
        /// <param name="tdc_posicao">Posição do docente.</param>
        /// <returns>Tipo do docente.</returns>
        public byte SelecionaTipoDocentePorPosicao(byte tdc_posicao)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_TipoDocente_SelecionaTipoPorPosicao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_posicao";
                Param.Size = 1;
                Param.Value = tdc_posicao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToByte(qs.Return);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retornar se existe outro tipo de docente cadastrado na mesma posição (return true) ou não (false)
        /// </summary>
        /// <param name="tdc_id">id do tipo do docente</param>
        /// <param name="tdc_posicao">posicao cadastrada para o tipo de docente</param>
        public bool VerificaDuplicidadePorPosicao(byte tdc_id, byte tdc_posicao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDocente_VerificaDuplicidadePorPosicao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_posicao";
                Param.Size = 1;
                Param.Value = tdc_posicao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                
                return qs.Return.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }

        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoDocente entity)
        {
            entity.tdc_dataCriacao = entity.tdc_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoDocente entity)
        {
            entity.tdc_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tdc_dataCriacao");
        }

        protected override bool Alterar(ACA_TipoDocente entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoDocente_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoDocente entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tdc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ACA_TipoDocente entity)
        {
            __STP_DELETE = "NEW_ACA_TipoDocente_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoDocente entity)
        {
            //entity.tdc_id = Convert.ToByte(qs.Return.Rows[0][0]);
            return entity.tdc_id > 0;
        }

        /// <summary>
        /// Verifica se já existe um tipo de docente cadastrado com a mesma descrição
        /// </summary>
        /// <param name="tdc_id">ID do tipo de docente</param>
        /// <param name="tdc_descricao">Nome do tipo de docente</param>  
        public bool SelectBy_TipoDocente
        (
            int tdc_id
            //, string tdc_descricao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDocente_SelectBy_TipoDocente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tdc_id";
                Param.Size = 4;
                if (tdc_id > 0)
                    Param.Value = tdc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

               #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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


        /// <summary>
        /// Verifica se existe um tipo de docente cadastrado com situação de excluído (3) 
        /// </summary>
        /// <param name="tdc_id">ID do tipo de docente</param>
        public bool SelectBy_TipoDocente_Situacao
        (
            int tdc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDocente_SelectBy_Situacao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tdc_id";
                Param.Size = 4;
                if (tdc_id > 0)
                    Param.Value = tdc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
    }
}