/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;

    /// <summary>
    /// 
    /// </summary>
    public class ACA_AlunoCurriculoDAO : AbstractACA_AlunoCurriculoDAO
    {
        #region Métodos

        /// <summary>
        /// Atualiza a situação dos dados de matrícula do aluno para inativo.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="dataSaida">Data de saída do aluno</param>
        public void InativaDadosMatricula
        (
            Int64 alu_id
            , DateTime dataSaida
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_InativaDadosMatricula", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataSaida";
                Param.Size = 16;
                Param.Value = dataSaida;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
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
        /// Retorna as matrículas do aluno que estejam na situação 
        /// 1 - Ativo ou 7 - Em matrícula
        /// Só traz matrículas das escolas que o usuário informado possua acesso.
        /// </summary>
        public DataTable SelectBy_Aluno_PermissaoUsuario
        (
            Int64 alu_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_SelectBy_Aluno_PermissaoUsuario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);


                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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
        /// Retorna o último curriculo do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="entity">Entidade ACA_AlunoCurriculo</param>
        /// <returns>DataTable com os dados</returns>
        public bool SelectBy_UltimoCurriculo(long alu_id, out ACA_AlunoCurriculo entity)
        {
            entity = new ACA_AlunoCurriculo();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_SelectBy_UltimoCurriculo", _Banco);
            
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    entity = DataRowToEntity(dr, entity);
                    return true;
                }
                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna o último curriculo INATIVO do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="entity">Entidade ACA_AlunoCurriculo</param>
        /// <returns>DataTable com os dados</returns>
        public bool SelectBy_UltimoCurriculoInativo(long alu_id, out ACA_AlunoCurriculo entity)
        {
            entity = new ACA_AlunoCurriculo();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_SelectBy_UltimoCurriculoInativo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    entity = DataRowToEntity(dr, entity);
                    return true;
                }
                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a última matrícula ativa dos alunos informados.
        /// </summary>
        /// <param name="alu_id">IDs dos alunos</param>
        /// <returns></returns>
        public DataTable SelecionaUltimaMatriculaAtiva_Alunos
        (
            string alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_UltimaMatriculaAtiva_Alunos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                Param.Value = alu_id;
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
        /// Retorna os currículos ativos e em matricula do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>        
        /// <returns>DataTable com os dados</returns>
        public DataTable SelecionaDadosUltimaMatriculaAtiva
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_UltimaMatriculaAtiva", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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
        /// Retorna o último currículo do aluno (Ativo ou inativo)
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>        
        /// <returns>DataTable com os dados</returns>
        public DataTable SelecionaDadosUltimaMatricula
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_UltimaMatricula", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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
        /// Verifica se o aluno curriculo não está sendo utilizado na matricula turma
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="alc_id"></param>        
        /// <returns>True/False</returns>
        public bool SelectBy_VerificaMatriculaTurma
        (
            long alu_id
            , int alc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_SelectBy_VerificaMatriculaTurma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alc_id";
                Param.Size = 4;
                Param.Value = alc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
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
        /// Verifica se já existe um AlunoCurriculo cadastrado com os mesmos dados
        /// (escola, curso, currículo e período)
        /// e com a situação "Ativo" ou "Em matrícula"
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns>True/False</returns>
        public bool SelectBy_VerificaAlunoCurriculo
        (
            long alu_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_SelectBy_VerificaAlunoCurriculo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                if (alu_id > 0)
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
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

        public bool Update_Situacao_FechamentoMatricula
        (
            ACA_AlunoCurriculo entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_Update_Situacao_FechamentoMatricula", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alc_id";
                Param.Size = 4;
                Param.Value = entity.alc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@alc_situacao";
                Param.Size = 1;
                Param.Value = entity.alc_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alc_dataAlteracao";
                Param.Size = 16;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return true;
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
        /// Altera somente os dados referentes à matrícula.
        /// </summary>
        /// <param name="entity">Entidade com dados necessários</param>
        /// <returns></returns>
        public bool AlterarDadosMatricula(ACA_AlunoCurriculo entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_UPDATE_DadosMatricula", _Banco);
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alc_id";
            Param.Size = 4;
            Param.Value = entity.alc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_matricula";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.alc_matricula))
                Param.Value = entity.alc_matricula;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_codigoInep";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alc_codigoInep))
                Param.Value = entity.alc_codigoInep;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_registroGeral";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.alc_registroGeral))
                Param.Value = entity.alc_registroGeral;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_matriculaEstadual";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.alc_matriculaEstadual))
                Param.Value = entity.alc_matriculaEstadual;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            qs.Execute();

            return true;
        }

        /// <summary>
        /// Retorna os currículos do aluno (inativo) de acordo com o mtu_id
        /// usado no UCBoletimCompletoAluno
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>        
        /// <param name="mtu_id">id da MatriculaTurma do aluno</param>
        /// <param name="tpc_id">id do bimestre/coc</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable SelecionaDadosMatriculaTurma
        (
            long alu_id,
            int mtu_id,
            int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_MatriculaTurma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 8;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
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
        /// Retorna os currículos ativos/inativos e em matricula do aluno
        /// usado no UCBoletimCompletoAluno
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>        
        /// <param name="tpc_id">Id do bimestre/coc</param>        
        /// <returns>DataTable com os dados</returns>
        public DataTable SelecionaDadosUltimaMatriculaAtivaPeriodoAvaliacao
        (
            long alu_id,
            int tpc_id
        )
        {                                                       
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculo_UltimaMatricula_PeriodoAvaliacao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
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
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
        {
            if (entity != null & qs != null)
            {
                entity.alc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.alc_id > 0);
            }

            return false;
        }		

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@alc_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@alc_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@alc_dataPrimeiraMatricula"].DbType = DbType.Date;
            qs.Parameters["@alc_dataSaida"].DbType = DbType.Date;
            qs.Parameters["@alc_dataColacao"].DbType = DbType.Date;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@alc_dataCriacao");
            qs.Parameters["@alc_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@alc_dataPrimeiraMatricula"].DbType = DbType.Date;
            qs.Parameters["@alc_dataSaida"].DbType = DbType.Date;
            qs.Parameters["@alc_dataColacao"].DbType = DbType.Date;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoCurriculo</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_AlunoCurriculo entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoCurriculo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alc_id";
            Param.Size = 4;
            Param.Value = entity.alc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@alc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoCurriculo</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ACA_AlunoCurriculo entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoCurriculo_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_AlunoCurriculo entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_AlunoCurriculo entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_AlunoCurriculo entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_AlunoCurriculo entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoCurriculo entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_AlunoCurriculo entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_AlunoCurriculo> Select()
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
        //public override IList<ACA_AlunoCurriculo> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoCurriculo entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_AlunoCurriculo DataRowToEntity(DataRow dr, ACA_AlunoCurriculo entity)
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
        //public override ACA_AlunoCurriculo DataRowToEntity(DataRow dr, ACA_AlunoCurriculo entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}