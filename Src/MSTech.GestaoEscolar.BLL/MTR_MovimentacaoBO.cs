using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Tipos de movimentação de aluno
    /// </summary>
    public enum MTR_MovimentacaoTipoMovimentacao : byte
    {
        AlterarTurma = 1,
        ReclassificarAluno = 2,
        AlterarCurso = 3,
        TransferirAlunoDentroRede = 4,
        TransferirAlunoForaRede = 5
    }

    /// <summary>
    /// Tipos de movimentação de aluno
    /// </summary>
    public enum MTR_MovimentacaoSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
        Retroativo = 4
    }

    #endregion Enumeradores

    #region Estruturas

    /// <summary>
    /// Estrutura para salvar a movimentação
    /// </summary>
    [Serializable]
    public struct MTR_Movimentacao_Cadastro
    {
        public MTR_Movimentacao entMovimentacao;
        public ACA_AlunoCurriculo entAluCurAnterior;
        public ACA_AlunoCurriculo entAluCurNovo;
        public MTR_MatriculaTurma entMatTurAnterior;
        public MTR_MatriculaTurma entMatTurNovo;
        public ACA_AlunoCurriculoAvaliacao entAluCurAvaAnterior;
        public ACA_AlunoCurriculoAvaliacao entAluCurAvaNovo;
        public MTR_MovimentacaoDadosAdicionais entDadosAdicionais;
        public DateTime dataMovimentacao;

        private FormacaoTurmaBO.ListasFechamentoMatricula _listasFechamentoMatricula;
        public FormacaoTurmaBO.ListasFechamentoMatricula listasFechamentoMatricula
        {
            get
            {
                if (_listasFechamentoMatricula == null)
                {
                    _listasFechamentoMatricula = new FormacaoTurmaBO.ListasFechamentoMatricula();
                }

                return _listasFechamentoMatricula;
            }
            set
            {
                _listasFechamentoMatricula = value;
            }
        }
    }

    #endregion Estruturas

    #region Excessões

    /// <summary>
    /// Classe de excessão referente à entidade MTR_Movimentacao.
    /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
    /// na entidade da movimentação.
    /// </summary>
    public class MTR_Movimentacao_ValidationException : ValidationException
    {
        public MTR_Movimentacao_ValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Classe de excessão, utilizada quando uma movimentação foi realizada
    /// recentemente por outro usuário
    /// </summary>
    public class RealizarMovimentacao_ValidationException : ValidationException
    {
        public RealizarMovimentacao_ValidationException(string message) : base(message) { }
    }

    public class MovimentacaoRetroativaException : Exception
    {
        private string _mensagem;

        public MovimentacaoRetroativaException()
        {
        }

        public MovimentacaoRetroativaException(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                {
                    _mensagem = base.Message;
                }

                return _mensagem;
            }
        }
    }

    /// <summary>
    /// Excessão que é disparada quando um curso não está no parâmetro de movimentação.
    /// </summary>
    public class CursoNaoParametrizado_ValidationException : ValidationException
    {
        public CursoNaoParametrizado_ValidationException(string message) : base(message) { }
    }

    #endregion Excessões

    public class MTR_MovimentacaoBO : BusinessBase<MTR_MovimentacaoDAO, MTR_Movimentacao>
    {
        #region Consultas

        /// <summary>
        /// Retorna as movimentações realizadas de acordo com os filtros
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo do curso</param>
        /// <param name="alc_matricula">Código da matrícula</param>
        /// <param name="alc_matriculaEstadual">Código da matrícula estadual</param>
        /// <param name="tur_codigo">Codigo da turma</param>
        /// <param name="tipoBusca">Tipo de busca por nome do aluno</param>
        /// <param name="nome_aluno">Nome do aluno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="dataInicio">Data inicial</param>
        /// <param name="dataFim">Data final</param>
        /// <returns>Tabela com movimentações do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaMovimentacao
        (
            Guid uad_idSuperior,
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            string alc_matricula,
            string alc_matriculaEstadual,
            string tur_codigo,
            byte tipoBusca,
            string nome_aluno,
            Guid ent_id,
            bool adm,
            Guid usu_id,
            Guid gru_id,
            DateTime dataInicio,
            DateTime dataFim
        )
        {
            totalRecords = 0;

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.SelectBy_Pesquisa(uad_idSuperior, esc_id, uni_id, cur_id, crr_id, alc_matricula, alc_matriculaEstadual, tur_codigo, tipoBusca, nome_aluno, ent_id, adm, usu_id, gru_id, dataInicio, dataFim, MostraCodigoEscola, out totalRecords);
        }

        /// <summary>
        /// Retorna as movimentações realizadas para o aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Tabela com movimentações do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaMovimentacaoAluno
        (
            long alu_id,
            Guid ent_id
        )
        {
            totalRecords = 0;

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.SelectBy_Aluno_Movimentacao(alu_id, ent_id, MostraCodigoEscola, out totalRecords);
        }

        /// <summary>
        /// Retorna os dados da movimentação de um aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>Entidade MTR_Movimentacao</returns>
        public static MTR_Movimentacao GetEntityBy_AlunoMovimentacaoRetroativa
        (
            long alu_id,
            TalkDBTransaction banco
        )
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_AlunoMovimentacaoRetroativa(alu_id);

            MTR_Movimentacao entity = new MTR_Movimentacao();

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna os dados de uma movimentação do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mov_id">Id da movimentação</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosMovimentacaoAluno
        (
            long alu_id,
            int mov_id
        )
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.SelectBy_alu_id_mov_id(alu_id, mov_id);
        }

        /// <summary>
        /// Retorna as movimentações realizadas para o aluno
        /// filtrando por tipo de movimento
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="tipoMovimento">Enum do tipo de movimento</param>
        /// <returns>Lista da entidade MTR_Movimentacao</returns>
        public static List<MTR_Movimentacao> SelecionaPorTipoMovimento(long alu_id, MTR_TipoMovimentacaoTipoMovimento tipoMovimento)
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.SelectBy_TipoMovimento(alu_id, Convert.ToByte(tipoMovimento));
        }

        /// <summary>
        /// Retorna os dados da última movimentação do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        public static MTR_Movimentacao SelecionaUltimaMovimentacaoAluno(long alu_id)
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            DataTable dt = dao.Select_UltimaMovimentacaoAluno(alu_id);

            MTR_Movimentacao entity = new MTR_Movimentacao();

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Seleciona as movimentações de saída realizadas em determinado ano em um dos dois meses do parâmetro.
        /// </summary>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="mes1">Mês 1</param>
        /// <param name="mes2">Mês 2</param>
        /// <param name="ano">Ano</param>
        /// <returns>Movimentação dos meses/ano</returns>
        public static List<MTR_Movimentacao> SelecionaMovimentacaoDeSaidaPorMesEAno(Guid ent_id, int mes1, int mes2, int ano)
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.SelecionaMovimentacaoDeSaidaPorMesEAno(ent_id, mes1, mes2, ano);
        }

        /// <summary>
        /// Seleciona as movimentações realizadas após a data informada.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade escolar.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="alc_matricula">Matrícula do aluno.</param>
        /// <param name="apenasAtivos">Flag para indicar se busca alunos inativos.</param>
        /// <param name="dataMovimentacao">Data referência da busca.</param>
        public static DataTable SelecionaPorData
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , string alc_matricula
            , bool apenasAtivos
            , DateTime dataMovimentacao
        )
        {
            totalRecords = 0;
            return new MTR_MovimentacaoDAO().SelecionaPorData(uad_idSuperior, esc_id, uni_id, cur_id, crr_id, crp_id, alc_matricula, apenasAtivos, dataMovimentacao, false, -1, -1, out totalRecords);
        }

        /// <summary>
        /// Verifica se o aluno possui movimentações por tipo e ano.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="tipoMovimentacao">Tipos de movimentações separados por ";"</param>
        /// <param name="ano">Ano do calendário escolar.</param>
        /// <returns></returns>
        public static bool VerificaAlunoPossuiMovimentacaoSaidaEscola(long alu_id, int mtu_id)
        {
            return new MTR_MovimentacaoDAO().VerificaAlunoPossuiMovimentacaoSaidaEscola(alu_id, mtu_id);
        }

        #endregion Consultas

        #region Validações

        /// <summary>
        /// Verifica se o turno da nova turma não entra em conflito com o turno da SAAI – Sala de apoio e acompanhamento a inclusão.
        /// </summary>
        /// <param name="alunoDeficiente">Aluno é deficiente</param>
        /// <param name="cadMov">Estrutura de cadastro de movimentação</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        private static void VerificarCursoExclusivoDeficiente(bool alunoDeficiente, MTR_Movimentacao_Cadastro cadMov, TalkDBTransaction bancoGestao, Guid ent_id)
        {
            if (!alunoDeficiente)
            {
                int cur_id = cadMov.entAluCurNovo.cur_id;

                ACA_Curso curso = null;

                if (cadMov.listasFechamentoMatricula.listCursos != null)
                {
                    // Se a lista de fechamento foi alimentada, buscar entidade da lista.
                    curso = cadMov.listasFechamentoMatricula.listCursos.Find(p => p.cur_id == cur_id);
                }

                if (curso == null)
                {
                    curso = ACA_CursoBO.GetEntity(new ACA_Curso { cur_id = cur_id }, bancoGestao);
                }

                if (curso.cur_exclusivoDeficiente)
                {
                    throw new ValidationException(String.Format("Este(a) {0} é exclusivo(a) para aluno(s) {1}. Favor verificar.",
                        GestaoEscolarUtilBO.nomePadraoCurso(ent_id).ToLower(),
                        ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TERMO_ALUNOS_DEFICIENCIA_TURMAS_NORMAIS, ent_id).ToLower()));
                }
            }
        }

        /// <summary>
        /// Retorna se a escola de origem vai ser responsável pelo lançamento de notas
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="dataMovimentacao"></param>
        /// <param name="ent_id"></param>
        /// <returns>
        /// True: Lançamento de notas será pela escola de origem
        /// False: Lançamento de notas será definido pelo usuário
        /// </returns>
        public static bool VerificaEfetivacaoEscolaOrigem(long tur_id, DateTime dataMovimentacao, Guid ent_id)
        {
            TUR_Turma tur = new TUR_Turma { tur_id = tur_id };
            TUR_TurmaBO.GetEntity(tur);

            int tmo_id = MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId((byte)MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede, ent_id);

            string sPrazoDias = MTR_ParametroTipoMovimentacaoBO.SelecionaValorParametroTipoMovimentacao(tmo_id, ChaveParametroTipoMovimentacao.PrazoDiasEfetivacaoEscolaOrigem);
            int PrazoDias = string.IsNullOrEmpty(sPrazoDias) ? 0 : Convert.ToInt32(sPrazoDias);

            DateTime dataFimPeriodoCalendario = ACA_TipoPeriodoCalendarioBO.SelecionaDataFinalPeriodoCalendarioAtual(tur.cal_id, dataMovimentacao);

            return dataFimPeriodoCalendario != new DateTime() &&
                   dataFimPeriodoCalendario.AddDays(PrazoDias * -1).Date <= dataMovimentacao;
        }

        /// <summary>
        /// Verifica se a movimentação pode ser alterada
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarAlteracaoMovimentacao(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Falecimento:
                case MTR_TipoMovimentacaoTipoMovimento.NecessidadeTrabalhar:
                case MTR_TipoMovimentacaoTipoMovimento.DoencaAnomaliaGrave:
                case MTR_TipoMovimentacaoTipoMovimento.Abandono:
                case MTR_TipoMovimentacaoTipoMovimento.DuplicidadeMatriculaExclusaoErroEscola:
                case MTR_TipoMovimentacaoTipoMovimento.TerminoPEJA:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Valida os dados necessários para realizar qualquer movimentação.
        /// Dispara uma ValidationException caso haja algum problema.
        /// </summary>
        /// <param name="cadMov">Entidade de cadastro de movimentação</param>
        /// <param name="alu">Entidade do aluno</param>
        /// <param name="tmo">Entidade tipo de movimentação</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        private static void ValidaDadosMovimentacao(MTR_Movimentacao_Cadastro cadMov, MTR_TipoMovimentacao tmo, ACA_Aluno alu, TalkDBTransaction bancoGestao)
        {
           bool isValidInicioMovimentacao;
            bool isValidFechamentoMovimentacao;

            // Valida se está no momento configurado na movimentação.
            MTR_TipoMovimentacaoBO.VerificaPeriodoValidoMovimentacao(cadMov.entMatTurAnterior,
                                                                     cadMov.entAluCurAnterior,
                                                                     cadMov.entMovimentacao.tmo_id,
                                                                     bancoGestao,
                                                                     cadMov.entMovimentacao.mov_dataRealizacao,
                                                                     out isValidInicioMovimentacao,
                                                                     out isValidFechamentoMovimentacao,
                                                                     cadMov.listasFechamentoMatricula);

            if (!isValidInicioMovimentacao)
                throw new ValidationException("Não é possível realizar esse tipo de movimentação nesse momento do calendário escolar.");

            if ((cadMov.entMatTurNovo != null) && (cadMov.entMatTurNovo.tur_id > 0))
            {
                // Verificar se a turma que o aluno vai entrar é do tipo 1-Normal.
                TUR_Turma entTurmaDestino = null;
                if (cadMov.listasFechamentoMatricula.listTurma != null)
                {
                    // Se a lista de fechamento foi alimentada, buscar entidade da lista.
                    entTurmaDestino = cadMov.listasFechamentoMatricula.listTurma.Find(p => p.tur_id == cadMov.entMatTurNovo.tur_id);
                }

                if (entTurmaDestino == null)
                {
                    entTurmaDestino = new TUR_Turma
                    {
                        tur_id = cadMov.entMatTurNovo.tur_id
                    };
                    TUR_TurmaBO.GetEntity(entTurmaDestino, bancoGestao);
                }                

                if (entTurmaDestino.tur_tipo != (byte)TUR_TurmaTipo.Normal)
                {
                    throw new ValidationException("Não é possível movimentar o aluno para a turma " +
                        entTurmaDestino.tur_codigo + ".");
                }
            }
        }

        /// <summary>
        /// Valida entidade aluno, se já está inativa.
        /// </summary>
        /// <param name="alu">Entidade a ser validada - obrigatório</param>
        private static void ValidaSituacaoInativo(ACA_Aluno alu)
        {
            if (alu.alu_situacao == Convert.ToByte(ACA_AlunoSituacao.Inativo))
                throw new ValidationException("Não foi possível realizar a movimentação. O aluno já está inativo no sistema.");
        }

        /// <summary>
        /// Validação da escola origem e destino.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        private static void ValidaEscolas(int esc_id)
        {
            ESC_Escola entityEscolaOrigem = new ESC_Escola
            {
                esc_id = esc_id
            };
            ESC_EscolaBO.GetEntity(entityEscolaOrigem);

            // Verificação caso a escola ORIGEM esteja com a situação = 4 "Desativado"
            if (entityEscolaOrigem.esc_situacao == Convert.ToByte(ESC_EscolaBO.ESC_EscolaSituacao.Desativado))
                throw new ValidationException("Não é possível salvar a movimentação, pois a escola " + entityEscolaOrigem.esc_nome + " esta desativada.");
        }

        /// <summary>
        /// Validação da data de movimentação, pois a data atual não pode ser anterior a última.
        /// </summary>
        /// <param name="cadMov">Estrutura de movimentação</param>
        /// <param name="tmo">Entidade tipo de movimentação</param>
        /// <param name="alu">Entidade do aluno</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados do gestão</param>
        /// <param name="listasFechamentoMatricula">Listas carregadas com dados do fechamento de matrícula</param>
        private static void ValidaDataMovimentacao
        (
            MTR_Movimentacao_Cadastro cadMov
            , MTR_TipoMovimentacao tmo
            , ACA_Aluno alu
            , TalkDBTransaction bancoGestao
            , FormacaoTurmaBO.ListasFechamentoMatricula listasFechamentoMatricula = null
        )
        {
            // Chama o método padrão para validar data da movimetação
            ValidaDataMovimentacao(cadMov.dataMovimentacao, alu.alu_id, alu.ent_id, tmo.tmo_id, bancoGestao, listasFechamentoMatricula);
        }

        /// <summary>
        /// Validação da data de movimentação, pois a data atual não pode ser anterior a última.
        /// </summary>
        /// <param name="dataMovimentacao">Data da movimentação</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tmo_id">Id do tipo de movimentação</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados</param>
        /// <param name="listasFechamentoMatricula">Listas carregadas com dados do fechamento de matrícula</param>
        public static void ValidaDataMovimentacao
        (
            DateTime dataMovimentacao,
            long alu_id,
            Guid ent_id,
            int tmo_id,
            TalkDBTransaction bancoGestao,
            FormacaoTurmaBO.ListasFechamentoMatricula listasFechamentoMatricula = null
        )
        {
            dataMovimentacao = dataMovimentacao == new DateTime() ? DateTime.Now : dataMovimentacao;

            // Recupera o último currículo do aluno, caso exista.
            ACA_AlunoCurriculo entityUltimoAlunoCurriculo;
            ACA_AlunoCurriculoBO.CarregaUltimoCurriculo(alu_id, out entityUltimoAlunoCurriculo);

            MTR_TipoMovimentacao tmo = null;
            if (listasFechamentoMatricula != null && listasFechamentoMatricula.listTipoMovimentacao != null)
            {
                // Se a lista de fechamento de matrícula foi passada, busca o tipo de movimentação dela ao invés de dar GetEntity.
                tmo = listasFechamentoMatricula.listTipoMovimentacao.
                        Find(p => p.tmo_id == tmo_id);
            }

            if (tmo == null)
            {
                tmo = new MTR_TipoMovimentacao
                {
                    tmo_id = tmo_id
                };
                MTR_TipoMovimentacaoBO.GetEntity(tmo, bancoGestao);
            }

            // Verifica a data da movimentação, necessário no caso  de ações retroativas.
            if ((entityUltimoAlunoCurriculo.alc_dataPrimeiraMatricula != new DateTime()) && (entityUltimoAlunoCurriculo.alc_dataPrimeiraMatricula.Date > dataMovimentacao.Date) ||
                (entityUltimoAlunoCurriculo.alc_dataSaida != new DateTime()) && (entityUltimoAlunoCurriculo.alc_dataSaida.Date > dataMovimentacao.Date))
            {
                throw new ValidationException("Data da movimentação não pode ser anterior à data da última matrícula do aluno.");
            }

            DateTime dataRealizacaoUltimaMovimentacao = MTR_TipoMovimentacaoBO.SelecionaDataRealizacaoUltimaMovimentacao(alu_id, bancoGestao);
            if (dataRealizacaoUltimaMovimentacao != new DateTime() && dataMovimentacao.Date < dataRealizacaoUltimaMovimentacao.Date)
                throw new ValidationException("Data da movimentação não pode ser anterior à data da última movimentação do aluno.");

            if ((MTR_TipoMovimentacaoTipoMovimento)tmo.tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.Falecimento && dataMovimentacao.Date > DateTime.Now.Date)
                throw new ValidationException("Data da movimentação deve ser menor ou igual à " + DateTime.Now.ToString("dd/MM/yyyy") + ".");
        }

        /// <summary>
        /// Verifica se vai gerar uma movimentação retroativa, pendende de informação
        /// </summary>
        /// <param name="dataMovimentacao">Data da movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="verificarDataAcaoRetroativa">"Indica se vai enviar mensagem para o usuário confirmando a movimentação retroativa"</param>
        /// <param name="bancoGestao">"Conexão aberta com o banco de dados"</param>
        /// <returns>true:realizar a movimentação retroativa / false:realizar a movimentação normal</returns>
        public static bool VerificarMovimentacaoRetroativa(DateTime dataMovimentacao, Guid ent_id, bool verificarDataAcaoRetroativa, TalkDBTransaction bancoGestao)
        {
            int prazoMovimentacao = MTR_MomentoAnoBO.SelecionaPrazoMovimentacaoPorEntidadeAno(ent_id, DateTime.Now.Year, bancoGestao);

            if (dataMovimentacao.Date > DateTime.Now.Date)
                throw new ValidationException("Data da movimentação deve ser menor ou igual à " + DateTime.Now.ToString("dd/MM/yyyy") + ".");

            if (dataMovimentacao.Date < DateTime.Now.AddDays(prazoMovimentacao * -1).Date)
            {
                if (verificarDataAcaoRetroativa)
                    throw new MovimentacaoRetroativaException("<b>Você está realizando uma movimentação para uma data anterior a " + prazoMovimentacao + " dias, o que irá gerar uma ação retroativa que dependerá de aprovação. <br /> Esta movimentação somente será efetuada no sistema após a aprovação.</b><br /><br /> Confirma a realização da movimentação?");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida e configura se a turma destino possui a mesma avaliação turma atual do aluno.
        /// </summary>
        /// <param name="cadMov">Estrutura de movimentação</param>
        /// <param name="progressaoPEJA">Verifica se o método é chamado pela tela de progressão</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        public static void ConfiguraTurmaAvaliacao(MTR_Movimentacao_Cadastro cadMov, bool progressaoPEJA, TalkDBTransaction bancoGestao)
        {
            List<ACA_CurriculoPeriodo> listPeriodosEquivalentes = ACA_CurriculoPeriodoBO.
                Seleciona_PeriodosRelacionados_Equivalentes(cadMov.entAluCurAnterior.cur_id, cadMov.entAluCurAnterior.crr_id, cadMov.entAluCurAnterior.crp_id);

            // Apenas valida a avaliação da turma caso seja o mesmo Curso/Período.
            // Também verifica se os cursos são equivalentes
            if (
                ((cadMov.entAluCurAnterior.cur_id == cadMov.entAluCurNovo.cur_id) &&
                (cadMov.entAluCurAnterior.crr_id == cadMov.entAluCurNovo.crr_id) &&
                (cadMov.entAluCurAnterior.crp_id == cadMov.entAluCurNovo.crp_id))
                || (listPeriodosEquivalentes.Count > 0 && listPeriodosEquivalentes.Exists(p => p.cur_id == cadMov.entAluCurNovo.cur_id &&
                                                                                          p.crr_id == cadMov.entAluCurNovo.crr_id &&
                                                                                          p.crp_id == cadMov.entAluCurNovo.crp_id)))
            {
                // Apenas valida a avaliação da turma caso definido a turma destino.
                if (cadMov.entMatTurNovo.tur_id > 0)
                {
                    ACA_CurriculoPeriodo entityCurriculoPeriodo = new ACA_CurriculoPeriodo { cur_id = cadMov.entAluCurAnterior.cur_id, crr_id = cadMov.entAluCurAnterior.crr_id, crp_id = cadMov.entAluCurAnterior.crp_id };
                    ACA_CurriculoPeriodoBO.GetEntity(entityCurriculoPeriodo, bancoGestao);

                    TUR_TurmaCurriculoAvaliacao entityTurmaCurriculoAvaliacaoAtual = new TUR_TurmaCurriculoAvaliacao { tur_id = cadMov.entAluCurAvaAnterior.tur_id, cur_id = cadMov.entAluCurAvaAnterior.cur_id, crr_id = cadMov.entAluCurAvaAnterior.crr_id, crp_id = cadMov.entAluCurAvaAnterior.crp_id, tca_id = cadMov.entAluCurAvaAnterior.tca_id };
                    TUR_TurmaCurriculoAvaliacaoBO.GetEntity(entityTurmaCurriculoAvaliacaoAtual, bancoGestao);
                    if (entityTurmaCurriculoAvaliacaoAtual.tca_id > 0)
                    {
                        TUR_TurmaCurriculoAvaliacao entityTurmaCurriculoAvaliacaoDestino;
                        string msg;

                        if (!progressaoPEJA)
                        {
                            // Verifica se a turma destino possui a avaliação da turma origem.
                            entityTurmaCurriculoAvaliacaoDestino =
                                TUR_TurmaCurriculoAvaliacaoBO.SelecionaAvaliacaoExistenteParaTurma(cadMov.entMatTurNovo.tur_id,
                                                                                                    cadMov.entAluCurNovo.cur_id,
                                                                                                    cadMov.entAluCurNovo.crr_id,
                                                                                                    cadMov.entAluCurNovo.crp_id,
                                                                                                    entityTurmaCurriculoAvaliacaoAtual.tca_numeroAvaliacao);
                            msg = "A turma destino do aluno deve possuir o(a) mesmo(a) " +
                                                                GestaoEscolarUtilBO.nomePadraoPeriodoAvaliacao(entityCurriculoPeriodo.crp_nomeAvaliacao) + " atual do aluno.";
                        }
                        else
                        {
                            entityTurmaCurriculoAvaliacaoDestino = new TUR_TurmaCurriculoAvaliacao
                                                                                                   {
                                                                                                       tur_id = cadMov.entMatTurNovo.tur_id
                                                                                                       ,
                                                                                                       cur_id = cadMov.entAluCurAvaNovo.cur_id
                                                                                                       ,
                                                                                                       crr_id = cadMov.entAluCurAvaNovo.crr_id
                                                                                                       ,
                                                                                                       crp_id = cadMov.entAluCurAvaNovo.crp_id
                                                                                                       ,
                                                                                                       tca_id = cadMov.entAluCurAvaNovo.tca_id
                                                                                                   };
                            TUR_TurmaCurriculoAvaliacaoBO.GetEntity(entityTurmaCurriculoAvaliacaoDestino, bancoGestao);
                            msg = "A turma destino do aluno não possui o(a) " +
                                                                GestaoEscolarUtilBO.nomePadraoPeriodoAvaliacao(entityCurriculoPeriodo.crp_nomeAvaliacao) + " configurada para o aluno.";
                        }

                        if ((entityTurmaCurriculoAvaliacaoDestino != null) && (entityTurmaCurriculoAvaliacaoDestino.tca_id > 0))
                        {
                            // Configura o currículo avaliação novo com mesma avaliação, caso necessário.
                            if (cadMov.entAluCurAvaNovo.tca_id <= 0)
                            {
                                cadMov.entAluCurAvaNovo.alu_id = cadMov.entAluCurNovo.alu_id;
                                cadMov.entAluCurAvaNovo.tur_id = entityTurmaCurriculoAvaliacaoDestino.tur_id;
                                cadMov.entAluCurAvaNovo.cur_id = entityTurmaCurriculoAvaliacaoDestino.cur_id;
                                cadMov.entAluCurAvaNovo.crr_id = entityTurmaCurriculoAvaliacaoDestino.crr_id;
                                cadMov.entAluCurAvaNovo.crp_id = entityTurmaCurriculoAvaliacaoDestino.crp_id;
                                cadMov.entAluCurAvaNovo.tca_id = entityTurmaCurriculoAvaliacaoDestino.tca_id;
                            }
                        }
                        else
                        {
                            throw new ValidationException(msg);
                        }
                    }
                    else
                    {
                        throw new ValidationException("Não é possível realizar essa movimentação, pois o aluno não possui " +
                                                        GestaoEscolarUtilBO.nomePadraoPeriodoAvaliacao(entityCurriculoPeriodo.crp_nomeAvaliacao) + " ativo(a) na turma.");
                    }
                }
            }
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de movimentação para o aluno
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem(long alu_id)
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            return dao.Select_MaiorOrdem(alu_id);
        }

        #endregion Validações

        #region Calcular frequência na movimentação

        /// <summary>
        /// Calcula o percentual de frequência acumulada do aluno
        /// </summary>
        /// <param name="cadMov">Referência a estrutura de cadastro de movimentação</param>
        /// <param name="tmo_tipoMovimento">Tipo de movimentação</param>        
        private static void CalcularFrequenciaAnterior(ref MTR_Movimentacao_Cadastro cadMov, int tmo_tipoMovimento)
        {
            if (MTR_TipoMovimentacaoBO.VerificarMovimentacaoInclusao(tmo_tipoMovimento))
            {
                if ((MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial)
                {
                    cadMov.entMovimentacao.mov_frequencia = 0;
                }
                else if ((MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular
                            || (MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal
                            || (MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios)
                {
                    if (cadMov.entMovimentacao.mov_frequencia <= 0)
                    {
                        cadMov.entMovimentacao.mov_frequencia = 100;
                    }
                }
                else if ((MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.Reconducao)
                {
                    cadMov.entMovimentacao.mov_frequencia = 0;
                }
            }
        }

        /// <summary>
        /// Retorna o id da cidade e da uf do endereço da entidade.
        /// [0]: cid_id
        /// [1]: unf_id
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public static Guid[] RetornaCid_UF_idEntidade(Guid ent_id)
        {
            DataTable dt =
                    SYS_EntidadeEnderecoBO.SelectEnderecosBy_Entidade(ent_id);

            Guid cid_id = Guid.Empty;
            Guid unf_id = Guid.Empty;

            if (dt.Rows.Count > 0)
            {
                cid_id = new Guid(dt.Rows[0]["cid_id"].ToString());
                unf_id = new Guid(dt.Rows[0]["unf_id"].ToString());
            }

            return new[] { cid_id, unf_id };
        }

        /// <summary>
        /// Retorna a quantidade de aulas e faltas do aluno por período do calendário
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula na turma do aluno</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="dataInicial">Data inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <param name="qtdeAulas">Retorna a quantidade total de aulas do período do calendário</param>
        /// <param name="qtdeFaltas">Retorna a quantidade total de faltas do período do calendário </param>
        /// <param name="banco">Transação com banco - opcional</param>
        public static void CalcularFrequenciaGlobal
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int tpc_id
            , byte tipoLancamento
            , DateTime dataInicial
            , DateTime dataFinal
            , out int qtdeAulas
            , out int qtdeFaltas
            , TalkDBTransaction banco
        )
        {
            MTR_MovimentacaoDAO dao = new MTR_MovimentacaoDAO();
            if (banco != null)
                dao._Banco = banco;

            DataTable dt = dao.CalcularFrequenciaGlobal(tur_id, alu_id, mtu_id, tpc_id, tipoLancamento, dataInicial, dataFinal);

            if (dt.Rows.Count > 0)
            {
                qtdeAulas = Convert.ToInt32(dt.Rows[0]["qtAulas"]);
                qtdeFaltas = Convert.ToInt32(dt.Rows[0]["qtFaltas"]);
            }
            else
            {
                qtdeAulas = 0;
                qtdeFaltas = 0;
            }
        }

        #endregion Calcular frequência na movimentação
    }
}