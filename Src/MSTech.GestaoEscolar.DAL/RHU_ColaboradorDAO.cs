using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Data.SqlClient;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_ColaboradorDAO : Abstract_RHU_ColaboradorDAO
    {
        #region Métodos Consulta

        /// <summary>
        /// Retorna os docentes e colaboradores com atribuiçao esporádica cadastrada para aquela escola
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        public DataTable PesquisaAtribuicaoEsporadica_PorFiltros
        (
            int esc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_PesquisaAtribuicaoEsporadica_PorFiltros", _Banco);

            #region PARAMETROS
            
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

        /// <summary>
        /// Retorna os docentes e colaboradores para atribuiçao esporádica
        /// qualquer docente pelos filtros;
        /// outros colaboradores nao docentes que possuam cargo base de docente
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="coc_matricula"></param>
        /// <returns></returns>
        public DataTable PesquisaPorFiltros_AtribuicaoEsporadica
        (
            string coc_matricula
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_PesquisaPorFiltros_AtribuicaoEsporadica", _Banco);

            #region PARAMETROS
            
            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@coc_matricula";
            Param.Value = coc_matricula;
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

        /// <summary>
        /// Retorna um datatable contendo todos os colaboladores
        /// que não foram excluídos logicamente
        /// </summary>
        /// <param name="pes_nome">Id da tabela PES_Pessoa do bd</param>
        /// <param name="tipo_cpf"></param>
        /// <param name="tipo_rg"></param>
        /// <param name="crg_id">Id da tabela RHU_Cargo do bd</param>
        /// <param name="fun_id">Id da tabela RHU_Funcao do bd</param>
        /// <param name="uad_id"></param>
        /// <param name="adm"></param>
        /// <param name="ent_id">Id da tabela SYS_Entidade do bd</param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os colaboradores</returns>
        public DataTable SelectBy_Pesquisa
        (
            string pes_nome
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , int fun_id
            , Guid uad_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tipo_documentacao_cpf";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_cpf))
                    Param.Value = tipo_cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tipo_documentacao_rg";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_rg))
                    Param.Value = tipo_rg;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (fun_id > 0)
                    Param.Value = fun_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@uad_id";
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != Guid.Empty)
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != Guid.Empty)
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna todos os colaboradores sem considerar permissão, se assim for configurado
        /// </summary>
        /// <param name="pes_nome">Nome do colaborador</param>
        /// <param name="coc_matricula">Matricula do colaborador</param>
        /// <param name="tipo_cpf">CPF do colaborador</param>
        /// <param name="tipo_rg">RG do colaborador</param>
        /// <param name="crg_id">Cargo do colaborador</param>
        /// <param name="fun_id">Função do colaborador</param>
        /// <param name="uad_id">UA do cargo/função do colaborador</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="todosColaboradores">Indica se vai considerar a permissão ou não</param>
        /// <param name="usu_id">Usuário logado</param>
        /// <param name="gru_id">Grupo do usuário logado</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        public DataTable SelectBy_Pesquisa_PermissaoTotal
        (
            string pes_nome
            , string coc_matricula
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , int fun_id
            , Guid uad_id
            , Guid uad_idSuperior
            , Guid ent_id
            , bool todosColaboradores
            , Guid usu_id
            , Guid gru_id
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_SelectBy_Pesquisa_PermissaoTotal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tipo_documentacao_cpf";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_cpf))
                    Param.Value = tipo_cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tipo_documentacao_rg";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_rg))
                    Param.Value = tipo_rg;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (fun_id > 0)
                    Param.Value = fun_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@uad_id";
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@uad_idSuperior";
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@todosColaboradores";
                Param.Size = 1;
                Param.Value = todosColaboradores;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != Guid.Empty)
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != Guid.Empty)
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona o colaborador, que não esteja
        /// excluído logicamente, pelo documento.
        /// </summary>
        /// <param name="psd_numero">Número do documento</param>
        /// <param name="tdo_id">Id do tipo de documento</param>
        /// <param name="entityColaborador"></param>
        /// <returns>True = se encontrou o colaborador com aquele documento / False = se não encontrou</returns>
        public bool SelectBy_Documento
        (
             Guid tdo_id
             , string psd_numero
             , RHU_Colaborador entityColaborador
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_SelectBy_Documento", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@psd_numero";
                Param.Size = 20;
                Param.Value = psd_numero;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entityColaborador.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_id";
                Param.Size = 16;
                Param.Value = tdo_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
                {
                    DataRowToEntity(qs.Return.Rows[0], entityColaborador, false);
                    return true;
                }
                return false;
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

        public DataTable BuscaPessoas(string pes_nome, string cpf, string rg, bool permiteAlunos)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_BuscaPessoa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cpf";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(cpf))
                    Param.Value = cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@rg";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(rg))
                    Param.Value = rg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@permiteAlunos";
                Param.Size = 1;
                Param.Value = permiteAlunos;
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

        public DataTable SelecionaDocentesPorNomeMatricula(string pes_nome, string coc_matricula)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_SelecionaDocentePorNome_Matricula", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
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
        /// Seleciona os dados do colaborador/docente da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro">Parâmetro: Nome do colaborador/docente OU matrícula</param>
        /// <returns>Retorna dados do colaborador/docente</returns>
        public DataSet SelecionaVisualizaConteudo(string parametro)
        {
            //Grava em DataSet pois retorna vários selects
            DataSet dsRetorno = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(_Banco.GetConnection.ConnectionString);

            adapter.SelectCommand = new SqlCommand("NEW_RHU_Colaborador_VisualizaConteudo", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            adapter.SelectCommand.Parameters.Add("parametro", SqlDbType.VarChar, 200);
            adapter.SelectCommand.Parameters["parametro"].Value = parametro;

            adapter.Fill(dsRetorno);

            return dsRetorno;
        }

        /// <summary>
        /// retorna os registros de colaborador com os dados de pessoa, docente, colaboradorCargo e colaboradorFuncao
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public DataTable SelecionarColaboradoresPorEscolaMatricula(int esc_id, string matricula)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_SelecionarPorEscolaMatricula", _Banco);
            try
            {

                #region Parametros

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@matricula";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(matricula))
                    Param.Value = matricula;
                else
                    Param.Value = DBNull.Value;
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

        #region Métodos verificação

        /// <summary>
        /// Verifica se existe outro colaborador com um vínculo que possua o mesmo número de matrícula.
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>
        /// <param name="matricula">Matrícula do vínculo do colaborador</param>
        /// <param name="ent_id">ID da entidade do usuário</param>
        /// <returns>True se existe.</returns>
        public bool VerificaMatriculaExistente
        (
            long col_id,
            string matricula,
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Colaborador_VerificaMatriculaExistente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 8;
                Param.Value = col_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@matricula";
                Param.Size = 30;
                Param.Value = matricula;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }        

        #endregion

        #region Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_Colaborador entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@col_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@col_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@col_dataAdmissao"].DbType = DbType.Date;
            qs.Parameters["@col_dataDemissao"].DbType = DbType.Date;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_Colaborador entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@col_dataCriacao");
            qs.Parameters["@col_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@col_dataAdmissao"].DbType = DbType.Date;
            qs.Parameters["@col_dataDemissao"].DbType = DbType.Date;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade RHU_Colaborador</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(RHU_Colaborador entity)
        {
            __STP_UPDATE = "NEW_RHU_Colaborador_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_Colaborador entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = entity.col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@col_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@col_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade RHU_Colaborador</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(RHU_Colaborador entity)
        {
            __STP_DELETE = "NEW_RHU_Colaborador_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}