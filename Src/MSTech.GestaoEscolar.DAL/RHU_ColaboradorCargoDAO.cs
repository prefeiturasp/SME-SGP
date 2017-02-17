using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_ColaboradorCargoDAO : Abstract_RHU_ColaboradorCargoDAO
    {

        /// <summary>
        /// Retorna os cargos dos colaboradores com os mesmos dados: unidade, cargo e vigencia.
        /// </summary>
        public DataTable PesquisaAtribuicoesIguais
        (
            long col_id
            , int crg_id
            , int coc_id
            , Guid ent_id
            , Guid uad_id
            , DateTime coc_vigenciaInicio
            , DateTime coc_vigenciaFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorCargo_PesquisaDadosIguais", this._Banco);
            
            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = col_id;
            qs.Parameters.Add(Param);

            AdicionaParametroInt("crg_id", crg_id, qs);
            AdicionaParametroInt("coc_id", coc_id, qs);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_id";
            Param.Size = 16;
            Param.Value = uad_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Date;
            Param.ParameterName = "@coc_vigenciaInicio";
            Param.Size = 20;
            Param.Value = coc_vigenciaInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Date;
            Param.ParameterName = "@coc_vigenciaFim";
            Param.Size = 20;
            Param.Value = coc_vigenciaFim;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            
            return qs.Return;
        }

        /// <summary>
        /// Adiciona um parâmetro à procedure do tipo inteiro.
        /// </summary>
        private void AdicionaParametroInt(string nome, int valor, QuerySelectStoredProcedure qs)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@" + nome;
            Param.Size = 4;
            Param.Value = valor;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cargos e funções
        /// dos colaboradores que não foram excluídas logicamente, filtrados por 
        /// id do colaborador        
        /// </summary>
        /// <param name="col_id">Id da tabela RHU_Colaborador do bd</param>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os cargos e funções dos colaboradores</returns>        
        public DataTable SelectBy_Pesquisa
        (
            long col_id
            , bool MostraCodigoEscola
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorCargoFuncao_SelectBy_Pesquisa", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
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
        /// Retorna o coc_id      
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>                
        /// <param name="crg_id">ID do cargo</param> 
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="esc_id">ID da escola</param>                
        public int SelectColaboradorCargoID
        (
             long col_id
            , int crg_id
            , Guid ent_id
            , int esc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorCargo_SELECTBY_col_id_crg_id_ent_id_esc_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 4;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToInt32(qs.Return.Rows[0][0]);
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
        /// Retorna o coc_id      
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>                
        /// <param name="crg_id">ID do cargo</param> 
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="esc_id">ID da escola</param>                
        public int SelectColaboradorCargoID
        (
             long col_id
            , int crg_id
            , string coc_matricula
            , bool coc_complementacaoCargaHoraria
            , bool coc_vinculoExtra
            , Guid ent_id
            , Guid uad_id
            , TalkDBTransaction banco
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorCargo_SELECTBY_col_id_crg_id_coc_matricula_ent_id_esc_id", banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@coc_matricula";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(coc_matricula))
                    Param.Value = coc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@coc_complementacaoCargaHoraria";
                Param.Size = 1;
                Param.Value = coc_complementacaoCargaHoraria;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@coc_vinculoExtra";
                Param.Size = 1;
                Param.Value = coc_vinculoExtra;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 4;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 4;
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0][0]);
                else
                    return 0;
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
        /// Verifica o código do último cargo cadastrado por colaborador/cargo
        /// filtradas por colaborador e cargo
        /// </summary>        
        /// <param name="col_id">Id da tabela RHU_Colaborador do bd</param>
        /// <param name="crg_id">Id da tabela RHU_ColaboradorCargo do bd</param>
        /// <returns>coc_id + 1</returns>
        public int SelectBy_col_id_crg_id_top_one
        (
            long col_id
            , int crg_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorCargo_SelectBy_col_id_crg_id_top_one", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["coc_id"].ToString()) + 1;
                else
                    return 1;
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

        #region Métodos Sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity"></param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_ColaboradorCargo entity)
        {
            entity.coc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.coc_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_ColaboradorCargo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@coc_vigenciaInicio"].DbType = DbType.Date;
            qs.Parameters["@coc_vigenciaFim"].DbType = DbType.Date;
            qs.Parameters["@coc_dataInicioMatricula"].DbType = DbType.Date;

            qs.Parameters["@coc_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@coc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_ColaboradorCargo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@coc_vigenciaInicio"].DbType = DbType.Date;
            qs.Parameters["@coc_vigenciaFim"].DbType = DbType.Date;
            qs.Parameters["@coc_dataInicioMatricula"].DbType = DbType.Date;

            qs.Parameters.RemoveAt("@coc_dataCriacao");
            qs.Parameters["@coc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade RHU_ColaboradorCargo</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(RHU_ColaboradorCargo entity)
        {
            __STP_UPDATE = "NEW_RHU_ColaboradorCargo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_ColaboradorCargo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = entity.col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            Param.Value = entity.crg_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@coc_id";
            Param.Size = 4;
            Param.Value = entity.coc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@coc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@coc_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade RHU_ColaboradorCargo</param>
        /// <returns>true = sucesso | false = fracasso</returns>       
        public override bool Delete(RHU_ColaboradorCargo entity)
        {
            __STP_DELETE = "NEW_RHU_ColaboradorCargo_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}
