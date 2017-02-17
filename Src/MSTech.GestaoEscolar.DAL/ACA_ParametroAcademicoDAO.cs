/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_ParametroAcademicoDAO : Abstract_ACA_ParametroAcademicoDAO
	{
        /// <summary>
        /// Retorna os parâmetros ativos e vigentes.
        /// </summary>
        /// <returns></returns>
        public List<ACA_ParametroAcademico> SelectBy_ParametrosVigente()
        {
            List<ACA_ParametroAcademico> lt = new List<ACA_ParametroAcademico>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_SelectBy_Vigente", _Banco);
            try
            {
                qs.Execute();

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new ACA_ParametroAcademico())).ToList<ACA_ParametroAcademico>();
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os parâmetros encontrados para a chave, que possuem o mesmo valor passado.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>        
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param> 
        /// <param name="pac_valor"></param>
        /// <returns></returns>
        public DataTable SelectBy_Chave_Valor
        (
            Guid ent_id
            , string pac_chave
            , string pac_valor
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_SelectBy_Chave_Valor", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pac_chave";
                Param.Value = pac_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pac_valor";
                Param.Value = pac_valor;
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
        /// Retorna os Tipos de Unidade Administrativa cadastrados no parâmetro de tipo
        /// de Ua padrão para as escolas.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>        
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param> 
        /// <returns></returns>
        public DataTable Select_TipoUABy_Chave
        (
            Guid ent_id
            , string pac_chave
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_Select_TipoUABy_Chave", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pac_chave";
                Param.Value = pac_chave;
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
        /// Seleciona um parametro por entidade e chave.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param>        
        /// <returns></returns>
        public ACA_ParametroAcademico LoadBy_pac_chave
        (
            Guid ent_id
            , string pac_chave
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_SelectBy_pac_chave", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pac_chave";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(pac_chave))
                    Param.Value = pac_chave;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return DataRowToEntity(qs.Return.Rows[0], new ACA_ParametroAcademico());

                return new ACA_ParametroAcademico();
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
        /// Retorna o último parâmetro cadastrado que possua vigência fim, da entidade e chave informada. 
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param>        
        /// <returns></returns>
        public ACA_ParametroAcademico Load_UltimoVigenciaFim_By_Chave
        (
            Guid ent_id
            , string pac_chave
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_Load_Ultimo_VigenciaFim_By_Chave", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pac_chave";
                Param.Size = 100;
                Param.Value = pac_chave;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return DataRowToEntity(qs.Return.Rows[0], new ACA_ParametroAcademico());

                return new ACA_ParametroAcademico();
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
        /// Retorna um datatable contendo todos os parametros cadastrados no BD
        /// não excluidos logicamente.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="paginado">Se é paginado ou não.</param>
        /// <param name="currentPage">Pagina atual</param>
        /// <param name="pageSize">Tamanho da pagina</param>
        /// <param name="totalRecord">Total de registros na tabela original</param>
        /// <returns>Datatable com parametros</returns>
        public DataTable Select
        (
            Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_Select", _Banco);
            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parametros

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
        /// Retorna um Datatable com todos os valores cadastrados para um parametro nas quais não foram excluidas logicamente.
        /// filtrados por ent_id e pac_chave
        /// </summary>     
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param>        
        /// <param name="paginado">Se é paginado ou não.</param>
        /// <param name="currentPage">Pagina atual</param>
        /// <param name="pageSize">Tamanho da pagina</param>
        /// <param name="totalRecord">Total de registros na tabela original</param>
        /// <returns>Datatable com parametros</returns>
        public DataTable SelectBy_pac_chave2
        (
            Guid ent_id
            , string pac_chave
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_SelectBy_pac_chave2", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;                
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pac_chave";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(pac_chave))
                    Param.Value = pac_chave;
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
        /// Retorna um Booleano na qual faz verificação de existencia de vigencia conflitante com relação as datas de vigencia
        /// na entidade do parametro.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param>
        /// <param name="pac_vigenciaInicio">Campo par_vigenciaInicio da tabela ACA_ParametroAcademico do bd</param>        
        /// <param name="pac_vigenciaFim">Campo par_vigenciaFim da tabela ACA_ParametroAcademico do bd</param>        
        /// <param name="pac_obrigatorio">Campo par_obrigatorio da tabela ACA_ParametroAcademico do bd</param> 
        /// <returns>True - caso exista uma vigencia em conflito;</returns> 
        public bool SelectBy_Vigencia
        (
            Guid ent_id
            , string pac_chave
            , DateTime pac_vigenciaInicio
            , DateTime pac_vigenciaFim
            , Boolean pac_obrigatorio
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_SelectBy_Vigencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pac_chave";
                Param.Size = 100;
                Param.Value = pac_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pac_vigenciaInicio";
                Param.Size = 3;
                Param.Value = pac_vigenciaInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pac_vigenciaFim";
                Param.Size = 3;
                if (!pac_vigenciaFim.Equals(new DateTime()))
                    Param.Value = pac_vigenciaFim;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@pac_obrigatorio";
                Param.Size = 1;
                Param.Value = pac_obrigatorio;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um Booleano na qual faz atualização/adequação da data de vigencia final do ultimo parametro (anterior)
        /// ao parametro a ser inserido. Executado somente para parametros obrigatórios;
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <param name="pac_chave">Campo par_chave da tabela ACA_ParametroAcademico do bd</param>
        /// <param name="pac_vigenciaFim">Campo par_vigenciaFim da tabela ACA_ParametroAcademico do bd</param>        
        /// <returns>True - caso realize a atualização;</returns>
        public bool Update_VigenciaFim
        (
            Guid ent_id
            , string pac_chave
            , DateTime pac_vigenciaFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroAcademico_UPDATE_VigenciaFim", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pac_chave";
                Param.Size = 100;
                Param.Value = pac_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pac_vigenciaFim";
                Param.Size = 3;
                Param.Value = pac_vigenciaFim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pac_dataAlteracao";
                Param.Size = 8;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ParametroAcademico entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@pac_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@pac_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ParametroAcademico entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@pac_dataCriacao");
            qs.Parameters["@pac_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_ParametroAcademico</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_ParametroAcademico entity)
        {
            __STP_UPDATE = "NEW_ACA_ParametroAcademico_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ParametroAcademico entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pac_id";
            Param.Size = 4;
            Param.Value = entity.pac_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pac_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pac_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_ParametroAcademico</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_ParametroAcademico entity)
        {
            __STP_DELETE = "NEW_ACA_ParametroAcademico_Update_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar
        /// </ssummary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ParametroAcademico entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            if (entity.ent_id != Guid.Empty)
                Param.Value = entity.ent_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pac_id";
            Param.Size = 4;
            Param.Value = entity.pac_id;
            qs.Parameters.Add(Param);
        }



	}
}