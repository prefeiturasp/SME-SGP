/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_FormatoAvaliacaoDAO : Abstract_ACA_FormatoAvaliacaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os formatos de avaliação padrão, de acordo com as regras para o currículo.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final.</param>
        /// <param name="seriadoAvaliacoes">Indica se o currículo do curso é seriado por avaliações.</param>
        /// <returns></returns>
        public DataTable SelectBy_RegrasCurriculo
        (
            int fav_id
            , Guid ent_id
            , int qtdeAvaliacaoPeriodica
            , bool seriadoAvaliacoes
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_RegrasCurriculo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtdeAvaliacaoPeriodica";
                Param.Size = 4;
                Param.Value = qtdeAvaliacaoPeriodica;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@seriadoAvaliacoes";
                Param.Size = 1;
                Param.Value = seriadoAvaliacoes;
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

        /// <summary>
        /// Retorna os formatos cadastrados para a escola ou que sejam padrão.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        public DataTable SelectBy_Escola_Padrao
        (
            int esc_id
            , int uni_id
            , Guid ent_id
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_Escola_Padrao", _Banco);
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
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

        /// <summary>
        /// Retorna os formatos de avaliação padrão de um formato específico ou ativo
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="ent_id">Id da entidade do usuário logado</param>
        /// <returns></returns>
        public DataTable SelectBy_FormatoPadraoAtivo
        (
            int fav_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_FormatoPadraoAtivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

        /// <summary>
        /// Retorna os formatos padrão, de um formato específico ou ativo
        /// e que tenham a quantidade de avaliações periodica ou periodica+final.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        /// <param name="seriadoAvaliacoes">Indica se o currículo do curso é seriado por avaliações</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public DataTable SelectBy_RegrasCurriculoPeriodo
        (
            int fav_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int qtdeAvaliacaoPeriodica
            , bool seriadoAvaliacoes
            , Nullable<bool> tur_docenteEspecialista
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_RegrasCurriculoPeriodo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@seriadoAvaliacoes";
                Param.Size = 1;
                Param.Value = seriadoAvaliacoes;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtdeAvaliacaoPeriodica";
                Param.Size = 4;
                Param.Value = qtdeAvaliacaoPeriodica;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tur_docenteEspecialista";
                Param.Size = 1;
                if (tur_docenteEspecialista == null)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tur_docenteEspecialista;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

        /// <summary>
        /// Verifica se o formato de avaliação está sendo utilizado em alguma turma do peja
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public bool SelectBy_VerificaTurmaPeja
        (
            int fav_id            
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_VerificaTurmaPeja", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna um datatable contendo todos os formatos de avaliação
        /// que não foram excluídos logicamente, filtrados por 
        /// id do formato de avaliação, nome do formato de avaliação e situação
        /// </summary>
        /// <param name="fav_id">Id da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="fav_nome">Campo fav_nome da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="fav_situacao">Campo fav_situacao da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os formatos de avaliação</returns>
        public DataTable SelectBy_fav_id_fav_nome
        (
            int fav_id
            , string fav_nome
            , int fav_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_fav_id_fav_nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                if (fav_id > 0)
                    Param.Value = fav_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fav_nome";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(fav_nome))
                    Param.Value = fav_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_situacao";
                Param.Size = 4;
                if (fav_situacao > 0)
                    Param.Value = fav_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna um datatable contendo todos os formatos de avaliação
        /// que não foram excluídos logicamente, filtrados por 
        /// id da escola, id da unidade e o nome do formato de avaliação.
        /// </summary>
        /// <param name="esc_id">Campo ID de escola da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="uni_id">Campo ID de unidade da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="fav_nome">Campo fav_nome da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com os formatos de avaliação</returns>
        public DataTable SelectBy_esc_id_uni_id_fav_nome
        (
            int esc_id
            , int uni_id
            , string fav_nome
            , Guid ent_id
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_esc_id_uni_id_fav_nome", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fav_nome";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(fav_nome))
                    Param.Value = fav_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
        /// Retorno booleano na qual verifica se existe um Formato de avaliação com o mesmo nome 
        /// cadastrado no banco com situação diferente de Excluido.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="fav_nome">Nome do formato de avaliação</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public bool SelectBy_Nome
        (
            int fav_id
            , string fav_nome
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                if (fav_id > 0)
                    Param.Value = fav_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fav_nome";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(fav_nome))
                    Param.Value = fav_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 32;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
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
        /// Retorna tabela com todos os id de formato de avaliacao que usam o esa_id do parametro em:
        /// esa_idConceitoGlobal , esa_idDocente ou esa_idPorDisciplina 
        /// </summary>
        /// <param name="esa_id">Id da Escala de avaliacao testada</param>
        /// <returns>Tabela com o campo fav_id</returns>
        public DataTable SelectBy_esa_id(int esa_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_Selectby_esa_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esa_id";
                if (esa_id > 0)
                    Param.Value = esa_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                
                #endregion

                qs.Execute();
                return (qs.Return);
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
        /// Seleciona o formato de avaliação de acordo com a disciplina da turma.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <returns>Formato de avaliação.</returns>
        public ACA_FormatoAvaliacao SelecionarPorTud(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelecionaPorTud", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                ACA_FormatoAvaliacao entity = new ACA_FormatoAvaliacao();
                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity);
                }

                return entity;
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
        /// Seleciona o formato de avaliação de acordo com a disciplina da turma.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <returns>Formato de avaliação.</returns>
        public ACA_FormatoAvaliacao SelecionarPorTur(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelecionaPorTur", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tur_id";
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                ACA_FormatoAvaliacao entity = new ACA_FormatoAvaliacao();
                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity);
                }

                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_FormatoAvaliacao entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_FormatoAvaliacao entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_FormatoAvaliacao entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_FormatoAvaliacao entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_FormatoAvaliacao entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_FormatoAvaliacao entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_FormatoAvaliacao entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_FormatoAvaliacao> Select()
        //{
        //    return base.Select();
        //}
        ///// <summary>
        ///// Realiza o select da tabela com paginacao
        ///// </summary>
        ///// <param name="currentPage">Pagina atual</param>
        ///// <param name="pageSize">Tamanho da pagina</param>
        ///// <param name="totalRecord">Total de registros na tabela original</param>
        ///// <returns>Lista com todos os registros da p�gina</returns>
        //public override IList<ACA_FormatoAvaliacao> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_FormatoAvaliacao DataRowToEntity(DataRow dr, ACA_FormatoAvaliacao entity)
        //{
        //    return base.DataRowToEntity(dr, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_FormatoAvaliacao DataRowToEntity(DataRow dr, ACA_FormatoAvaliacao entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}