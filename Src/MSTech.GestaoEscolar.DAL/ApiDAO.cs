using System;
using System.Data;
using System.Data.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.DAL
{
    public class ApiDAO : Persistent
    {
        #region Atributos

        /// <summary>
        /// Gets or sets Param.
        /// </summary>
        protected DbParameter Param { get; set; }

        #endregion

        #region Propriedades

        /// <summary> 
        /// Indica o nome da conexão da classe.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #endregion

        #region Métodos

        #region Sistema Diário de classe

        /// <summary>
        /// retorna um datatable com registros pelo ent_id.
        /// quando a dataBase não for passado, apenas registros ativos serão retornados,
        /// caso contrario apenas registros criados ou alterados apos esta data.
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para criação/alteração dos registros</param>
        /// <returns></returns>
        public DataTable SelecionarTiposAnotacoesAlunoPorEntidade(int id, Guid ent_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAnotacaoAluno_EntidadeDataBase", _Banco);

            try
            {

                #region parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 32;
                Param.ParameterName = "@tia_id";

                if (id > 0)
                    Param.Value = id;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;

                if (Guid.Empty.Equals(ent_id))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 8;

                if (dataBase.Equals(new DateTime()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;

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
        /// Retorna lista de permissoes por tipo de docente.
        /// </summary>
        /// <returns></returns>
        public DataTable BuscarPermissoesPorTipoDocente(int tdc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_PermissaoDocente_TipoDocente", _Banco);

            try
            {
                #region Parametros
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 32;
                Param.ParameterName = "@tdc_id";
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona os dados para a sincronizacao do planejamento
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>                
        /// <returns></returns>
        public DataSet SelecionaRetornoPlanejamento(long tur_id, DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoCiclo_SelecionaAtivoPorEscola", _Banco);

            try
            {


                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                
                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;

                if (syncDate == new DateTime())
                    Param.Value = DBNull.Value;
                else
                    Param.Value = syncDate;

                qs.Parameters.Add(Param);


                #endregion Parâmetros

                return qs.Execute_DataSet();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna datatable com tipos de docentes.
        /// </summary>
        /// <returns></returns>
        public DataTable BuscarTiposDocente()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaPermissoesDocentes", _Banco);

            try
            {
                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna datatable com todos os arquivos vinculados a DCL_ArquivoBiblioteca ativas.
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaArquivosBiblioteca()
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_ArquivoBiblioteca_ListagemArquivos", _Banco);

            try
            {
                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Relacao de compensação de ausencias por escola.
        /// </summary>
        /// <param name="esc_id">Escola</param>
        /// <param name="syncDate">Data da ultima sincronização</param>
        /// <returns>DataTable de CLS_CompensacaoAusencia</returns>
        public DataTable BuscaCompensacaoFalta(int esc_id, Int64 tur_id, DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_Select_Escola_DataSincronizacao", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;

                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;

                if (syncDate == new DateTime())
                    Param.Value = DBNull.Value;
                else
                    Param.Value = syncDate;

                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }


        
        public DataTable BuscaEventos(int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaEventosFechamento", _Banco);

            try
            {
                #region Parametros

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
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Método que busca escola
        /// </summary>
        /// <param name="k1">Chave do sistema para a entidade</param>
        /// <param name="uad_codigo">Código da unidade administrativa</param>
        /// <returns>DataTable contendo ent_id, uad_id, uad_codigo e uad_nome</returns>
        public DataTable BuscaEscola(string k1, string uad_codigo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaEscola", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@k1";
                Param.Size = 100;
                Param.Value = k1;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uad_codigo";
                Param.Size = 20;
                Param.Value = uad_codigo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Método que busca escolas a partir do usuario do professor
        /// </summary>
        /// <param name="usu_login"></param>
        /// <returns></returns>
        public DataTable BuscaEscolasProfessor(string usu_login)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Service_BuscaEscolasPeloUsuarioDocente", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 100;
                Param.Value = usu_login;
                qs.Parameters.Add(Param);



                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna as turmas ativas do professor em determinada escola
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="doc_id">id do docente</param>
        /// <returns></returns>
        public DataTable SelecionarTurmasPorEscolaProfessor(int esc_id, int doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaTurmasPorEscolaDocente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;

                if (doc_id > 0)
                {
                    Param.Value = doc_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }

                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um dataTable com registros de TUR_TurmaDisciplina por turma.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        public DataTable SelecionarTurmaDisciplinaPorTurma(long tur_id)
        {
            try
            {
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                return dao.SelecionaPorTurmas(tur_id.ToString());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Método que busca dados das turmas e disciplinas das turmas
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="syncDate">Menor data de sincronização dos registros da turma (Opcional)</param>
        /// <param name="anoLetivo">Ano letivo</param>
        /// <returns>DataTable contendo os dados das turmas e disciplinas</returns>
        public DataTable BuscaTurmas(string esc_ids, Int64 tur_id, DateTime syncDate, int anoLetivo, string usu_login)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaTurmas", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_ids";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(esc_ids))
                    Param.Value = esc_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(usu_login))
                    Param.Value = usu_login;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@anoLetivo";
                Param.Size = 8;
                Param.Value = anoLetivo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Método que retorna os status dos protocolos
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="dtProtocolos">DataTable com os paramentros para buscar os status</param>
        /// <returns>DataTable contendo os status dos protocolos</returns>
        public DataTable BuscaStatusProtocolos(String protocolos)
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaStatusProtocolos", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.ParameterName = "@protocolos";
                Param.DbType = DbType.String;
                Param.Value = protocolos;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna registros de fotos dos alunos
        /// </summary>
        /// <param name="alu_ids">array de alunos por ;</param>
        /// <param name="syncDate">data da ultima sincronizacao</param>
        /// <returns></returns>
        public DataTable BuscaFotoAluno(string alu_ids, DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_FotosAlunos", this._Banco);

            try
            {
                #region parametros
                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";
                Param.Value = alu_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
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
        }


        /// <summary>
        /// Método que busca os alunos de uma determinada turma
        /// </summary>
        /// <param name="esc_id">Id da escola (Opcional)</param>
        /// <param name="tur_id">Id da turma (Opcional)</param>
        /// <param name="syncDate">Menor data de sincronização dos registros dos Alunos na Turma especificada (Opcional)</param>
        /// <returns>DataTable que contém os alunos da turma pesquisada</returns>
        public DataTable BuscaAlunosTurma(Int32 esc_id, Int64 tur_id, DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaAlunosTurma", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;

                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;

                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
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
        }

        /// <summary>
        /// retorna os dados de alunos com os dados de pessoa e usuario por escola
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <returns></returns>
        public DataTable SelecionarAlunosPorEscola(int esc_id, DateTime dataSincronizacao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                    Param.Value = esc_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataSincronizacao";
                Param.Size = 16;
                
                if (dataSincronizacao != new DateTime())
                    Param.Value = dataSincronizacao;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

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
        /// Método que busca usuários
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="usu_login">Login do usuário (Opcinal)</param>
        /// <param name="syncDate">Menor data de sincronização dos registros dos Alunos na Turma especificada (Opcional)</param>
        /// <param name="dtProfessores">DataTable contendo os doc_id dos professores</param>
        /// <param name="dtAdministradores">DataTable contendo os usu_id dos administradores</param>
        /// <returns>DataTable contendo os usuários</returns>
        public DataTable BuscaUsuarios(int esc_id, string usu_login, DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscarUsuarios", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(usu_login))
                    Param.Value = usu_login;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
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
        }

        /// <summary>
        /// Método que busca os dados do usuário
        /// </summary>        
        /// <param name="usu_login">Login do usuário</param>        
        /// <param name="esc_id">Escola que o tablet esta vinculada</param>
        /// <returns>DataTable contendo os dados do usuário</returns>
        public DataTable BuscaDadosUsuario(string usu_login, int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaDadosLogin", this._Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                Param.Value = usu_login;
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
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Método que carrega os dados de configuração
        /// </summary>
        /// <param name="esc_id">Is da escola</param>
        /// <returns>DataTable contendo os dados de configuração</returns>
        public DataTable DadosIniciais(int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaAgendaRequisicao", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca todos os tipos de atividade
        /// </summary>
        /// <returns>DataTable contendo os tipos de atividade</returns>
        public DataTable BuscaTipoAtividade(DateTime syncDate)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_Service_BuscaTipoAtividade", this._Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca os recursos aula
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaRecursosAula()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaRecursosAula", this._Banco);
            try
            {
                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna justificativas faltas
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaJustificativasFaltas()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaJustificativasFaltas", this._Banco);
            try
            {
                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Sistema SIG

        /// <summary>
        /// Retorna a quantidade de alunos aprovados no fechamento do bimestre.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaQuantidadeAlunosAprovados(string anosLetivos, decimal percentualFrequenciaPadrao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunosAprovados", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@anosLetivos";
                if (string.IsNullOrEmpty(anosLetivos))
                {
                    Param.Value = DBNull.Value;
                }
                else
                {
                    Param.Value = anosLetivos;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@percentualFrequenciaPadrao";
                Param.Size = 7;

                if (percentualFrequenciaPadrao > 0)
                    Param.Value = percentualFrequenciaPadrao;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de alunos por resuldado no fechamento do bimestre.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaQuantidadeAlunosResultados(int cal_ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunosResultados", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                Param.Value = cal_ano;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
       
        /// <summary>
        /// Retorna a quantidade de alunos e turmas por escola
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaQuantidadeAlunoTurmaPorEscola()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunoTurmaPorEscola", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de alunos que estão na idade ideal
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaQuantidadeAlunoIdadeIdeal()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunoIdadeIdeal", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de alunos que estão na idade ideal no ensino fundamental
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaQuantidadeAlunoIdadeIdealFundamental()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunoIdadeIdealFundamental", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de escolas que iniciaram o fechamento.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaEscolaFechamento()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeEscolasFechamento", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de alunos aprovados, reprovados e que tiveram movimentação de abandono por escola.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaTaxaRendimento()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_TaxaRendimento", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de turmas que iniciaram o fechamento.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaTurmasFechamento()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeTurmasFechamento", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de alunos de finalizaram o boletim.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaAlunosFechamento()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_QuantidadeAlunosFechamento", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de pais/responsáveis que acessaram o boletim online.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public DataTable SelecionaResponsaveisFechamento()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_ResponsaveisAcessoBoletimOnline", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Geral - Sistema Gestão Escolar

        #region ACA_AreaConhecimento

        /// <summary>
        /// Retorna a ACA_AreaConhecimento pelo aco_id
        /// </summary>
        /// <param name="aco_id">Id da área de conhecimento.</param>
        /// <returns>ACA_AreaConhecimento</returns>
        public ACA_AreaConhecimento SelecionarAreaConhecimentoPorId(int aco_id)
        {
            try
            {
                ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();

                ACA_AreaConhecimento areaConhecimento = new ACA_AreaConhecimento { aco_id = aco_id };

                dao.Carregar(areaConhecimento);

                return areaConhecimento.IsNew ? null : areaConhecimento;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ACA_CalendarioAnual


        /// <summary>
        /// Retorna a ACA_CalendarioAnual pelo esc_id.
        /// </summary>
        /// <param name="esc_id">Id da Escola</param>
        /// <returns>Datatable</returns>
        public DataTable SelecionarCalendarioAnualPorEscId(long esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_CalendarioCurso_CalendarioPeriodo_SelectBy_EscId", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                

                #endregion Parâmetros

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
        /// Retorna a ACA_CalendarioAnual pelo cal_id.
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable SelecionarCalendarioAnualPorId(int cal_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_CalId", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataBase";
                Param.Size = 20;
                if (dataBase != new DateTime())
                    Param.Value = dataBase;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                

                #endregion Parâmetros

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
        /// Carrega os calendários anuais pelo cal_ano.
        /// </summary>
        /// <param name="cal_ano">Ano do calendário</param>  
        /// <returns>Datatable com os dados ACA_Calendario, ACA_CalendarioCurso e ACA_CalendarioPeriodo</returns>
        public DataTable SelecionaCalendarioAnualPorAno(int cal_ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_CalAno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                if (cal_ano > 0)
                    Param.Value = cal_ano;
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

        #endregion ACA_CalendarioAnual

        #region ACA_CurriculoPeriodo

        /// <summary>
        /// Retorna uma lista de curriculos do periodo por escola. Se passado a dataBase
        /// irá retornar apenas os registros criados ou alterados após esta data, caso contrario
        /// apenas os registros ativos serão retornados.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionarCurriculosPorEscola(int esc_id, DateTime dataBase)
        {
            try
            {
                return new ACA_CurriculoPeriodoDAO().SelecionarCurriculosPorEscola(esc_id, dataBase);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ACA_TipoDisciplina

        /// <summary>
        /// Retorna a ACA_TipoDisciplina pelo tds_id.
        /// </summary>
        /// <returns>ACA_TipoDisciplina</returns>
        public DataTable SelecionarTipoDisciplinaPorId(int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDisciplina_SelectBy_TdsId", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                    Param.Value = tds_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);                

                #endregion Parâmetros

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
        /// Retorna a ACA_TipoDisciplina pelo tne_id.
        /// </summary>
        /// <returns>ACA_TipoDisciplina</returns>
        public DataTable SelecionarTipoDisciplinaPorNivelEnsino(int tne_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDisciplina_SelectBy_TneId", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                    Param.Value = tne_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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
        /// Retorna todos os tipos de disciplina não excluídos logicamente.
        /// </summary>               
        /// <param name="tne_id">ID do tipo de nível de ensino</param>          
        /// <param name="aco_id">ID da Area de Conhecimento</param>    
        /// <returns>Datatable</returns>
        public DataTable SelecionarTipoDisciplina
        (
            int tne_id
            , int aco_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoDisciplina_SelectBy_Pesquisa_TipoNivelEnsino_AreaConhecimento", _Banco);

            try
            {
                #region Parâmetros

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
                Param.ParameterName = "@aco_id";
                Param.Size = 4;
                if (aco_id > 0)
                    Param.Value = aco_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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

        #endregion ACA_TipoDisciplina

        #region ACA_TipoModalidadeEnsino

        /// <summary>
        /// Retorna a ACA_TipoModalidadeEnsino ativas.
        /// </summary>
        /// <returns>ACA_TipoModalidadeEnsino</returns>
        public DataTable SelecionarTipoModalidadeEnsinoAtivas()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelectAtivos", _Banco);

            try
            {
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

        #endregion ACA_TipoModalidadeEnsino

        #region ACA_TipoNivelEnsino

        /// <summary>
        /// Retorna a ACA_TipoNivelEnsino pelo tne_id.
        /// </summary>
        /// <returns>ACA_TipoNivelEnsino</returns>
        public DataTable SelecionarTipoNivelEnsinoPorId(int tne_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoNivelEnsino_SelectBy_tne_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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

        #endregion ACA_TipoNivelEnsino

        #region Usuário

        /// <summary>
        /// Seleciona os ids de aluno ou docente por usuário.
        /// </summary>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="ent_id">ID da entidade.</param>
        /// <returns></returns>
        public DataTable SelecionaAlunoDocentePorUsuario(Guid usu_id, string usu_login, Guid ent_id, long esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaAlunoDocentePorUsuario", _Banco);

            try
            {
                #region Parâmetros

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(usu_login))
                    Param.Value = usu_login;
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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 8;

                if (dataBase.Equals(new DateTime()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;
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

        #endregion Usuário

        #region ACA_Aluno

        /// <summary>
        /// retorna registro de aluno pelo id
        /// </summary>
        /// <param name="alu_id">id do aluno</param>
        /// <returns></returns>
        public ACA_Aluno SelecionarAlunoPorId(int alu_id)
        {
            try
            {
                ACA_AlunoDAO dao = new ACA_AlunoDAO();
                ACA_Aluno aluno = new ACA_Aluno
                {
                    alu_id = alu_id
                };

                dao.Carregar(aluno);

                if (aluno.IsNew)
                {
                    return null;
                }

                return aluno;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region MTR_Movimentacao

        /// <summary>
        /// Retorna todas as movimentações posterior a data informada.
        /// </summary>
        /// <param name="dataAlteracao">Data de alteração da movimentacao</param>
        /// <returns>Datatable</returns>
        public DataTable SelecionarMovimentacoesPorDataAlteracao(DateTime mov_dataAlteracao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelectBy_DataAlteracao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@mov_dataAlteracao";
                Param.Size = 20;
                Param.Value = mov_dataAlteracao;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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
        /// Retorna a movimentacao pelo alu_id e mov_id.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mov_id">Id da movimentacao.</param>
        /// <returns>MTR_Movimentacao</returns>
        public MTR_Movimentacao SelecionarMovimentacaoPorId(long alu_id, int mov_id)
        {
            try
            {
                MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();

                MTR_Movimentacao movimentacao = new MTR_Movimentacao { alu_id = alu_id, mov_id = mov_id };

                dao.Carregar(movimentacao);

                return movimentacao.IsNew ? null : movimentacao;
            }
            catch
            {
                throw;
            }
        }

        #endregion MTR_Movimentacao

        #region DCL_Protocolo

        /// <summary>
        /// retorna um registro de protocolo pelo id.
        /// </summary>
        /// <param name="pro_id">id do protocolo - Guid</param>
        /// <returns></returns>
        public DCL_Protocolo SelecionarProtocoloPorId(Guid pro_id)
        {
            try
            {
                DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
                DCL_Protocolo protocolo = new DCL_Protocolo
                {
                    pro_id = pro_id
                };

                dao.Carregar(protocolo);

                if (protocolo.IsNew)
                {
                    return null;
                }

                return protocolo;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region CLS_TipoAtividadeAvaliativa

        /// <summary>
        /// retorna o tipo de atividade avaliativa por id
        /// </summary>
        /// <param name="tav_id">id do tipo de atividade avaliativa</param>
        /// <returns></returns>
        public CLS_TipoAtividadeAvaliativa SelecionarTipoAtividadeAvaliativaPorId(int tav_id)
        {
            try
            {
                CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();
                CLS_TipoAtividadeAvaliativa tipoAtividade = new CLS_TipoAtividadeAvaliativa
                {
                    tav_id = tav_id
                };

                dao.Carregar(tipoAtividade);

                if (tipoAtividade.IsNew)
                {
                    return null;
                }

                return tipoAtividade;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna todos os tipos de atividades avaliativas ativas.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionarTipoAtividadeAvaliativaAtivas()
        {
            try
            {
                CLS_TipoAtividadeAvaliativaDAO dao = new CLS_TipoAtividadeAvaliativaDAO();

                int totalRecord;
                return new DataTable();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ACA_RecursosAula

        /// <summary>
        /// retorna um recurso pelo id
        /// </summary>
        /// <param name="rsa_id">id do recurso</param>
        /// <returns></returns>
        public ACA_RecursosAula SelecionarRecursoPorId(int rsa_id)
        {
            try
            {
                ACA_RecursosAulaDAO dao = new ACA_RecursosAulaDAO();
                ACA_RecursosAula recurso = new ACA_RecursosAula
                {
                    rsa_id = rsa_id
                };

                dao.Carregar(recurso);

                if (recurso.IsNew)
                {
                    return null;
                }

                return recurso;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna todos os registros de recursos da aula ativos.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionarRecursosAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_RecursosAula_All", _Banco);
            try
            {
                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ESC_Escola

        /// <summary>
        /// Retorna a escola pelo id.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <returns></returns>
        public ESC_Escola SelecionarEscolaPorId(int esc_id)
        {
            try
            {
                ESC_EscolaDAO dao = new ESC_EscolaDAO();

                ESC_Escola escola = new ESC_Escola { esc_id = esc_id };
                dao.Carregar(escola);

                if (escola.IsNew)
                {
                    return null;
                }

                return escola;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna as escolas pela entidade, qdo informado a data base o retorno é 
        /// apenas de escolas criadas ou alteradas apos esta data... caso contrario 
        /// apenas escolas ativas serão retornadas.
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para a seleção.</param>
        /// <returns></returns>
        public DataTable SelecionarEscolasPorEntidade(Int32 esc_id, string esc_codigo, Guid ent_id, DateTime dataBase)
        {
            try
            {
                return new ESC_EscolaDAO().SelecionarEscolaApi(esc_id, esc_codigo, ent_id, dataBase);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region TUR_Turma

        /// <summary>
        /// Retorna a turma pelo id
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public TUR_Turma SelecionarTurmaPorId(Int64 tur_id)
        {
            try
            {
                TUR_TurmaDAO dao = new TUR_TurmaDAO();
                TUR_Turma turma = new TUR_Turma { tur_id = tur_id };

                dao.Carregar(turma);

                if (turma.IsNew)
                {
                    return null;
                }

                return turma;

            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region TUR_TurmaDisciplina
        public TUR_TurmaDisciplina SelecionarTurmaDisciplinaPorId(long tud_id)
        {
            try
            {
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                TUR_TurmaDisciplina turmaDisciplina = new TUR_TurmaDisciplina { tud_id = tud_id };

                dao.Carregar(turmaDisciplina);

                if (turmaDisciplina.IsNew)
                {
                    return null;
                }

                return turmaDisciplina;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ACA_Docente

        /// <summary>
        /// retorna um registro de docente pelo id
        /// </summary>
        /// <param name="doc_id">id do docente</param>
        /// <returns></returns>
        public ACA_Docente SelecionarDocentePorId(int doc_id)
        {
            try
            {
                ACA_DocenteDAO dao = new ACA_DocenteDAO();
                ACA_Docente docente = new ACA_Docente
                {
                    doc_id = doc_id
                };

                dao.Carregar(docente);

                if (docente.IsNew)
                {
                    return null;
                }

                return docente;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna os registros de docente com os dados de pessoa e usuarios. se passado a ultimaModificacao
        /// retorna apenas os dados alterados/criados a partir desta data.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionarDocentesPorTurma(long tur_id, DateTime dataBase)
        {
            return new ACA_DocenteDAO().SelecionaDocentesPorTurma(tur_id, dataBase);
        }

        /// <summary>
        /// retorna uma lista de docentes por escola. se passado a ultimaModificacao
        /// retorna apenas os dados alterados/criados a partir desta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionarDocentesPorEscola(int esc_id, DateTime dataBase)
        {
            return new ACA_DocenteDAO().SelecionaDocentesPorEscola(esc_id, dataBase);
        }

        /// <summary>
        /// retorna os registros de colaborador com os dados de pessoa, docente, colaboradorCargo e colaboradorFuncao
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="esc_id">matricula do docente</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionarColaboradoresPorEscolaMatricula(int esc_id, string matricula)
        {
            return new RHU_ColaboradorDAO().SelecionarColaboradoresPorEscolaMatricula(esc_id, matricula);
        }

        #endregion

        #region ACA_FormatoAvaliacao

        /// <summary>
        /// Carrega um registro de formato de avaliacao pelo id
        /// </summary>
        /// <param name="fav_id">id do formato de avaliacao</param>
        /// <returns></returns>
        public ACA_FormatoAvaliacao BuscarFormatoAvaliacaoPorId(int fav_id)
        {
            try
            {
                ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
                ACA_FormatoAvaliacao formatoAvaliacao = new ACA_FormatoAvaliacao
                {
                    fav_id = fav_id
                };

                dao.Carregar(formatoAvaliacao);

                if (formatoAvaliacao.IsNew)
                {
                    return null;
                }

                return formatoAvaliacao;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ACA_EscalaAvaliacao

        /// <summary>
        /// Retorna um datatable com a escala de avaliacao e teu relacionamento com a escala numerica ou por parecer.
        /// </summary>
        /// <param name="esa_id">id da escala de avaliacao</param>
        /// <returns></returns>
        public DataTable BuscarEscalaAvaliacaoPorId(int esa_id)
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("New_aca_escalaavaliacao_selectby_id", _Banco);
            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esa_id";
                Param.Size = 4;
                Param.Value = esa_id;
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

        #region ORC_OrientacoesCurriculares

        /// <summary>
        /// Busca a orientacao curricular pelo ID
        /// </summary>
        /// <param name="ocr_id"></param>
        /// <returns></returns>
        public ORC_OrientacaoCurricular SelecionarOrientacaoPorId(Int64 ocr_id)
        {
            try
            {
                ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();

                ORC_OrientacaoCurricular orientacao = new ORC_OrientacaoCurricular { ocr_id = ocr_id };
                dao.Carregar(orientacao);

                if (orientacao.IsNew)
                {
                    return null;
                }

                return orientacao;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Retorna os alunos que estão ativos na escola ligada à unidade administrativa informada.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade administrativa da escola</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosAtivosPorUnidadeAdministrativaEscola(Guid ent_id, Guid uad_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaAlunosAtivosPor_UnidadeAdministrativa", _Banco);

            try
            {
                #region Parâmetros

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
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
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

        /// <summary>
        /// Retorna os alunos que estão ativos na escola ligada à unidade administrativa informada.
        /// Listagem detalhada
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uad_id">ID da escola (UAD)</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosDetalhadoPorUnidadeAdministrativaEscola(Guid ent_id, int esc_id, Guid uad_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaAlunosDetalhadoPor_UnidadeAdministrativa", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                if (uad_id.Equals(new Guid()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
        /// Retorna os colaboradores da unidade administrativa e cargo informados.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade administrativa da escola</param>
        /// <param name="crg_id">ID do cargo para filtrar colaboradores</param>
        /// <returns></returns>
        public DataTable SelecionaColaboradoresPorUnidade_Cargo(Guid ent_id, Guid uad_id, int crg_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaColaboradorPor_Unidade_Cargo", _Banco);

            try
            {
                #region Parâmetros

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
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
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

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// Busca também disciplinas eletivas que o aluno cursou (tem nota) mas que a turma atual não oferece.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public DataTable SelecionaBoletimAluno(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0001_SubBoletimEscolarAluno", _Banco);

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
            Param.Size = 4;
            Param.Value = mtu_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Plataforma de itens e avaliações

        /// <summary>
        /// Retorna dados do docente pelo pes_id
        /// </summary>
        /// <param name="ent_id">ID da pessoa</param>
        /// <returns></returns>
        public DataTable BuscaDadosDocente(Guid pes_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosDocentePor_Pessoa", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                Param.Value = pes_id;
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
        /// Retorna lista de escola que o usuario tem permissao
        /// </summary>
        /// <param name="ent_id">ID da pessoa</param>
        /// <returns></returns>
        public DataTable BuscaEscolasPorPermissaoUsuario(Guid ent_id, Guid usu_id, Guid gru_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaEscolasPor_PermissaoUsuario", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
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
        /// Retorna lista de escolas por entidade
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade superior</param>
        /// <returns></returns>
        public DataTable BuscaEscolasEntidade(Guid ent_id, Guid uad_idSuperior)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaEscolasPor_Entidade", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior.Equals(new Guid()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
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
        /// Retorna lista de escola que o usuario tem permissao e esta na unidade
        /// </summary>
        /// <param name="ent_id">ID da pessoa</param>
        /// <returns></returns>
        public DataTable BuscaEscolasPorPermissaoUsuarioUnidadeAdm(Guid ent_id, Guid usu_id, Guid gru_id, Guid uad_idSuperior)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaEscolasPor_Permissao_UnidadeAdm", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                Param.Value = uad_idSuperior;
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
        /// Retorna lista de escola que o usuario tem permissao e esta na unidade
        /// </summary>
        /// <param name="ent_id">ID da turma</param>
        /// <returns></returns>
        public DataTable BuscaAlunosMatriculadosTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaAlunosMatriculadosTurmaPor_Turma", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// Retorna tipos de justificativa falta
        /// </summary>
        /// <returns></returns>
        public DataTable BuscarTiposJustificativaFalta()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTipoJustificativaFalta_OrdenadaNome", _Banco);

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna tipos de justificativa falta
        /// </summary>
        /// <returns></returns>
        public DataTable BuscarTiposNivelEnsino()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTipoNivelEnsino", _Banco);

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna turma com ou nao permissao de usuario
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaDadosTurmas(Guid ent_id, int cal_id, Guid uad_idSuperior, int esc_id, int cur_id, int crr_id, int crp_id, int trn_id, string tur_codigo,
            Guid usu_id, Guid gru_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosTurmaPor_Escola_Permissao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 4;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna turma com ou nao permissao de usuario
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaDadosTurmasDocente(Guid ent_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosTurmaPor_Escola_PermissaoDocente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
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

        /// <summary>
        /// Retorna turma fitlrada
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaDadosTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosTurmaPor_Turma", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
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

        /// <summary>
        /// Retorna disciplinas da turma
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaTurmaDisciplinas(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTipoDisciplinaPor_Turma", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
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

        /// <summary>
        /// Retorna disciplinas da turma e docente
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaTurmaDocenteDisciplinas(long tur_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTipoDisciplinaPor_TurmaDocente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
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

        /// <summary>
        /// Retorna disciplinas da turma e docente
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaDocenteDisciplinas(long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTipoDisciplinaPor_Docente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
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

        /// <summary>
        /// Retorna usuario e nome de docentes da turma
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaDadosDocentePorTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDocentesPor_Turma", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
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

        /// <summary>
        /// Retorna usuario e nome de docentes da turma
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaTurnos(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaTurnosPor_Entidade", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
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

        /// <summary>
        /// Retorna dataset contendo nos datatables os dados dos cursos.
        /// [0] - Cursos ativos da Entidade informada.
        /// [1] - Curriculos dos cursos ativos da Entidade informada.
        /// [2] - CurriculosPeriodo dos cursos ativos da Entidade informada.
        /// [3] - Tipo nível de ensino dos cursos ativos da Entidade informada.
        /// </summary>
        /// <returns> DataSet </returns>
        public DataSet BuscaDadosCursosEntidade(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosCursos_Por_Entidade", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna dataset contendo nos datatables os dados dos cursos.
        /// [0] - Disciplinas para o tipo de nível de ensino.        
        /// [1] - Tipos de disciplina para o tipo de nível de ensino.
        /// </summary>
        /// <returns> DataSet </returns>
        public DataSet BuscaDadosDisciplinas(int tne_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaDadosDisciplinas_Por_TipoNivelEnsino", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
                qs.Parameters.Add(Param);

                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna lista de descrição da série
        /// </summary>
        /// <param name="tme_id">modalidade de ensino</param>
		/// <param name="tne_id">nível de ensino</param>
        /// <returns></returns>
        public DataTable SelecionarTipoCurriculoPeriodo(int tme_id, int tne_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaTipoCurriculoPeriodo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                Param.Value = tme_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna lista de docentes por vínculo de trabalho
        /// </summary>
        /// <param name="psd_numeroCPF">cpf do docente</param>
        /// <param name="psd_numeroRG">rg do docente</param>
        /// <param name="ent_id">entidade</param>
        /// <param name="coc_matricula">matrícula do docente</param>
        /// /// <param name="coc_vinculoSede">true se o docente possuir vínculo</param>
        /// <returns></returns>
        public DataTable SelecionarDocentesPorVinculoDeTrabalho(string psd_numeroCPF, string psd_numeroRG, Guid ent_id, string coc_matricula, bool coc_vinculoSede)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_SelecionaDocentePorVinculoDeTrabalho", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@psd_numeroCPF";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(psd_numeroCPF))
                    Param.Value = psd_numeroCPF;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@psd_numeroRG";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(psd_numeroRG))
                    Param.Value = psd_numeroRG;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 30;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@coc_matricula";
                Param.Size = 16;
                Param.Value = coc_matricula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@coc_vinculoSede";
                Param.Size = 1;
                Param.Value = coc_vinculoSede;
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

        #endregion Plataforma de itens e avaliações

        #endregion
    }
}