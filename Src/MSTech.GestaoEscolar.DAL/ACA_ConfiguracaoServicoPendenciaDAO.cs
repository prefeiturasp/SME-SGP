/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_ConfiguracaoServicoPendenciaDAO : Abstract_ACA_ConfiguracaoServicoPendenciaDAO
    {
        /// <summary>
        /// Verifica se já existe uma Configuração do serviço de pendência cadastrada com os mesmos dados
        /// e com a situação "Ativo"
        /// </summary>
        /// <param name="entity">Entidade configuração do serviço de pendência</param>
        /// <returns>True/False</returns>
        public bool SelectBy_VerificaConfiguracaoServicoPendencia
        (
            ACA_ConfiguracaoServicoPendencia entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ConfiguracaoServicoPendencia_SelectBy_VerificaConfiguracaoServicoPendencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (entity.tne_id > 0)
                {
                    Param.Value = entity.tne_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                if (entity.tme_id > 0)
                {
                    Param.Value = entity.tme_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tur_tipo";
                Param.Size = 1;
                if (entity.tur_tipo > 0)
                {
                    Param.Value = entity.tur_tipo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@csp_semPlanoAula";
                Param.Size = 1;
                Param.Value = entity.csp_semPlanoAula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@csp_semRelatorioAtendimento";
                Param.Size = 4;
                Param.Value = entity.csp_semRelatorioAtendimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@csp_semObjetoConhecimento";
                Param.Size = 1;
                Param.Value = entity.csp_semObjetoConhecimento;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
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
        /// Retorna as configurações de serviço de pendência não excluídas logicamente, de acordo com tipo de nível de ensino,
        /// tipo de modalidade de ensino e tipo de turma.
        /// </summary>   
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tur_tipo">Enum do tipo de turma</param>
        public DataTable SelectBy_tne_id_tme_id_tur_tipo(
            int tne_id
            , int tme_id
            , int tur_tipo
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ConfiguracaoServicoPendencia_SelectBy_tne_id_tme_id_tur_tipo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)                
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                if (tme_id > 0)
                    Param.Value = tme_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_tipo";
                Param.Size = 4;
                if (tur_tipo > 0)
                    Param.Value = tur_tipo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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

        /// <summary>
        /// Retorna as configurações de serviço de pendência não excluídas logicamente, de acordo com tipo de nível de ensino,
        /// tipo de modalidade de ensino e tipo de turma.
        /// </summary>   
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tur_tipo">Enum do tipo de turma</param>
        public DataTable SelectTodasBy_tne_id_tme_id_tur_tipo(
            int tne_id
            , int tme_id
            , int tur_tipo
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ConfiguracaoServicoPendencia_SelectTodasBy_tne_id_tme_id_tur_tipo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                if (tme_id > 0)
                    Param.Value = tme_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_tipo";
                Param.Size = 4;
                if (tur_tipo > 0)
                    Param.Value = tur_tipo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@csp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@csp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@csp_dataCriacao");
            qs.Parameters["@csp_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_ConfiguracaoServicoPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_ConfiguracaoServicoPendencia entity)
        {
            __STP_UPDATE = "NEW_ACA_ConfiguracaoServicoPendencia_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ConfiguracaoServicoPendencia entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@csp_id";
            Param.Size = 4;
            Param.Value = entity.csp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@csp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@csp_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_ConfiguracaoServicoPendencia</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_ConfiguracaoServicoPendencia entity)
        {
            __STP_DELETE = "NEW_ACA_ConfiguracaoServicoPendencia_UpdateSituacao";
            return base.Delete(entity);
        }
    }
}