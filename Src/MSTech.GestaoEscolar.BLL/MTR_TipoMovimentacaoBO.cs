/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public enum MTR_TipoMovimentacaoTipoMovimento : byte
    {
        MatriculaInicial = 1,
        TransferenciaDaRedeParticular = 2,
        TransferenciaDaRedePublicaOutrosMunicipios = 3,
        TransferenciaDaRedePublicaOutrosEstadosFederal = 4,
        Reconducao = 5,
        TransferenciaPropriaRede = 6,
        MudancaModalidaeEnsino = 7,
        Remanejamento = 8,
        RegularizacaoRecurso = 9,
        Adequacao = 10,
        Reclassificacao = 11,
        MudancaTurma = 12,
        MudancaBlocoPEJA = 13,
        TransferenciaParaRedeParticular = 14,
        TransferenciaParaRedePublicaOutrosMunicipios = 15,
        TransferenciaParaRedePublicaOutrosEstadosFederal = 16,
        Falecimento = 17,
        NecessidadeTrabalhar = 18,
        DoencaAnomaliaGrave = 19,
        Abandono = 20,
        DuplicidadeMatriculaExclusaoErroEscola = 21,
        TerminoPEJA = 22,
        RenovacaoMatricula = 23,
        ImportacaoSCA = 24,
        ExportacaoSCA = 25,
        CorrecaoDuplicidade = 26,
        ConclusaoNivelEnsino = 27,
        CargaInicialSistema = 28,
        RenovacaoInicial = 29,
        DesistenciaMatricula = 30,
        RenovacaoManual = 31
    }

    public enum MTR_TipoMovimentacaoSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
        Indisponivel = 5
    }

    /// <summary>
    /// MTR_TipoMovimentacao Business Object
    /// </summary>
    public class MTR_TipoMovimentacaoBO : BusinessBase<MTR_TipoMovimentacaoDAO, MTR_TipoMovimentacao>
    {
        #region Cache

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(MTR_TipoMovimentacao entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(MTR_TipoMovimentacao entity)
        {
            return string.Format("MTR_TipoMovimentacao_GetEntity_{0}", entity.tmo_id);
        }

        #endregion Cache

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static MTR_TipoMovimentacao GetEntity(MTR_TipoMovimentacao entity, TalkDBTransaction banco = null)
        {
            MTR_TipoMovimentacaoDAO dao = banco == null ? new MTR_TipoMovimentacaoDAO() : new MTR_TipoMovimentacaoDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            dao.Carregar(entity);

            return entity;
        }
        
        /// <summary>
        /// Retorna todos os parâmetros de movimentação não excluídos logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoMovimentacao
        (
            Guid ent_id
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            return dao.SelectBy_Pesquisa(string.Empty, 0, 0, 0, ent_id, false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna os tipos de movimentação não excluídos logicamente.
        /// </summary>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<MTR_TipoMovimentacao> SelecionaTipoMovimentacao_Todos
        (
            TalkDBTransaction banco
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO() { _Banco = banco };
            DataTable dt = dao.Select_Todos();

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new MTR_TipoMovimentacao())).ToList();
        }

        /// <summary>
        /// Retorna os parâmetros de movimentação não excluídos logicamente por categoria
        /// Sem paginação
        /// </summary>
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoMovimentacaoPorCategoria
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , bool renovacao
            , Guid ent_id
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            return dao.SelectBy_Categoria(inclusao, realocacao, exclusao, outros, renovacao, ent_id);
        }

        /// <summary>
        /// Seleciona o tipo de movimentação, filtrado pelo nome e tipo de movimento,
        /// e preenche a entidade.
        /// </summary>
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="entity">Entidade MTR_TipoMovimentacao com o tmo_nome preenchido.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool SelecionaTipoMovimentacaoPorCategoria_Nome
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , Guid ent_id
            , MTR_TipoMovimentacao entity
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            return dao.SelectBy_Categoria_Nome(inclusao, realocacao, exclusao, outros, ent_id, entity);
        }

        /// <summary>
        /// Retorna a chave de identificação do tipo de movimentação.
        /// </summary>
        /// <param name="tmo_tipoMovimento">Tipo de movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>tmo_id</returns>
        public static int Retorna_TipoMovimentacaoId
        (
            byte tmo_tipoMovimento
            , Guid ent_id
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            return dao.Retorna_TipoMovimentacaoId(tmo_tipoMovimento, ent_id);
        }
               
        /// <summary>
        /// Retorna nas 2 variáveis se a movimentação está no período válido, para o curso e ano letivo
        /// informados (através da matrícula do aluno).
        /// </summary>
        /// <param name="entMatrTurma">Matrícula atual do aluno na turma</param>
        /// <param name="entAluCur">Matrícula atual do aluno no curso</param>
        /// <param name="tmo_id">Tipo de movimentação a ser realizada</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        /// <param name="dataComparar">Data para comparação do momento</param>
        /// <param name="estaInicioMovimentacao">OUT - retorna se está no período de início de movimentação</param>
        /// <param name="estaFechamentoMovimentacao">OUT - retorna se está no período de fechamento de movimentação</param>
        /// <param name="listasFechamentoMatricula">Listas carregadas com dados do fechamento de matrícula</param>
        public static void VerificaPeriodoValidoMovimentacao
        (
            MTR_MatriculaTurma entMatrTurma
            , ACA_AlunoCurriculo entAluCur
            , int tmo_id
            , TalkDBTransaction bancoGestao
            , DateTime dataComparar
            , out bool estaInicioMovimentacao
            , out bool estaFechamentoMovimentacao
            , FormacaoTurmaBO.ListasFechamentoMatricula listasFechamentoMatricula = null
        )
        {
            // Chama método padrão para verificar o período valido da movimentação
            VerificaPeriodoValidoMovimentacao(entMatrTurma.tur_id, entAluCur.cur_id, entAluCur.crr_id, tmo_id, dataComparar, bancoGestao, out estaInicioMovimentacao, out estaFechamentoMovimentacao, listasFechamentoMatricula);
        }

        /// <summary>
        /// Retorna nas 2 variáveis se a movimentação está no período válido, para o curso e ano letivo
        /// informados (através da matrícula do aluno).
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="tmo_id">ID do tipo de movimentação</param>
        /// <param name="dataComparar">Data para comparação do momento</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        /// <param name="estaInicioMovimentacao">OUT - retorna se está no período de início de movimentação</param>
        /// <param name="estaFechamentoMovimentacao">OUT - retorna se está no período de fechamento de movimentação</param>
        /// <param name="listasFechamentoMatricula">Listas carregadas com dados do fechamento de matrícula</param>
        public static void VerificaPeriodoValidoMovimentacao
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int tmo_id
            , DateTime dataComparar
            , TalkDBTransaction bancoGestao
            , out bool estaInicioMovimentacao
            , out bool estaFechamentoMovimentacao
            , FormacaoTurmaBO.ListasFechamentoMatricula listasFechamentoMatricula = null
        )
        {
            TUR_Turma entTur = null;

            if (listasFechamentoMatricula != null && listasFechamentoMatricula.listTurma != null)
            {
                // Busca a entidade da lista de fechamento de matrícula.
                entTur = listasFechamentoMatricula.listTurma.Find(p => p.tur_id == tur_id);
            }

            if (entTur == null)
            {
                entTur = new TUR_Turma
                {
                    tur_id = tur_id
                };
                TUR_TurmaBO.GetEntity(entTur, bancoGestao);
            }

            ACA_CalendarioAnual entCalendario = null;
            if (listasFechamentoMatricula != null && listasFechamentoMatricula.listCalendarios != null)
            {
                // Busca a entidade da lista de fechamento de matrícula.
                entCalendario = listasFechamentoMatricula.listCalendarios.Find(p => p.cal_id == entTur.cal_id);
            }

            if (entCalendario == null)
            {
                entCalendario = new ACA_CalendarioAnual
                {
                    cal_id = entTur.cal_id
                };
                ACA_CalendarioAnualBO.GetEntity(entCalendario, bancoGestao);
            }

            int mom_ano = entCalendario.cal_ano;

            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelectBy_PeriodoValido_Curso(cur_id, crr_id, tmo_id, mom_ano, dataComparar);

            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["IsPeriodoInicioProcesso"].ToString()))
                {
                    estaInicioMovimentacao = Convert.ToBoolean(dt.Rows[0]["IsPeriodoInicioProcesso"]);
                }
                else
                {
                    estaInicioMovimentacao = false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[0]["IsPeriodoFimProcesso"].ToString()))
                {
                    estaFechamentoMovimentacao = Convert.ToBoolean(dt.Rows[0]["IsPeriodoFimProcesso"]);
                }
                else
                {
                    estaFechamentoMovimentacao = false;
                }
            }
            else
            {
                estaInicioMovimentacao = false;
                estaFechamentoMovimentacao = false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimento é por cursos e períodos
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarCursosPeriodos(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Reconducao:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                case MTR_TipoMovimentacaoTipoMovimento.TerminoPEJA:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimento tem cursos de destino
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarCursosPeriodosDestino(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimento é para todos os momentos
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarTodosMomentos(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Reconducao:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Falecimento:
                case MTR_TipoMovimentacaoTipoMovimento.NecessidadeTrabalhar:
                case MTR_TipoMovimentacaoTipoMovimento.DoencaAnomaliaGrave:
                case MTR_TipoMovimentacaoTipoMovimento.Abandono:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoMatricula:
                case MTR_TipoMovimentacaoTipoMovimento.ConclusaoNivelEnsino:
                case MTR_TipoMovimentacaoTipoMovimento.CargaInicialSistema:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação é de inclusão
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoInclusao(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Reconducao:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação é de realocação
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoRealocacao(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                case MTR_TipoMovimentacaoTipoMovimento.Remanejamento:
                case MTR_TipoMovimentacaoTipoMovimento.RegularizacaoRecurso:
                case MTR_TipoMovimentacaoTipoMovimento.Adequacao:
                case MTR_TipoMovimentacaoTipoMovimento.Reclassificacao:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaTurma:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação é de exclusão
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoExclusao(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedeParticular:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosMunicipios:
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosEstadosFederal:
                case MTR_TipoMovimentacaoTipoMovimento.Falecimento:
                case MTR_TipoMovimentacaoTipoMovimento.NecessidadeTrabalhar:
                case MTR_TipoMovimentacaoTipoMovimento.DoencaAnomaliaGrave:
                case MTR_TipoMovimentacaoTipoMovimento.Abandono:
                case MTR_TipoMovimentacaoTipoMovimento.DuplicidadeMatriculaExclusaoErroEscola:
                case MTR_TipoMovimentacaoTipoMovimento.TerminoPEJA:
                case MTR_TipoMovimentacaoTipoMovimento.DesistenciaMatricula:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação é de outros tipos
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoOutros(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoMatricula:
                case MTR_TipoMovimentacaoTipoMovimento.ConclusaoNivelEnsino:
                case MTR_TipoMovimentacaoTipoMovimento.CargaInicialSistema:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação é de renovação
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoRenovacao(int tipoMovimento)
        {
            switch ((MTR_TipoMovimentacaoTipoMovimento)tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifica se o tipo de movimentação gera transferencia de matricula do aluno.
        /// </summary>
        /// <param name="tipoMovimento">Tipo do movimento</param>
        /// <returns></returns>
        public static bool VerificarMovimentacaoTransferenciaMatriculaAluno(MTR_Movimentacao_Cadastro cadMov)
        {
            MTR_MovimentacaoDAO daoMov = new MTR_MovimentacaoDAO();
            TalkDBTransaction bancoGestao = daoMov._Banco;

            MTR_TipoMovimentacao tmo;
            if (cadMov.listasFechamentoMatricula.listTipoMovimentacao != null)
            {
                // Se os dados do fechamento de matrícula forem alimentados, pegar dados do fechamento.
                tmo = cadMov.listasFechamentoMatricula.listTipoMovimentacao.
                    Find(p => p.tmo_id == cadMov.entMovimentacao.tmo_id) ?? new MTR_TipoMovimentacao();
            }
            else
            {
                tmo = new MTR_TipoMovimentacao { tmo_id = cadMov.entMovimentacao.tmo_id };
                MTR_TipoMovimentacaoBO.GetEntity(tmo, daoMov._Banco);
            }

            switch ((MTR_TipoMovimentacaoTipoMovimento)tmo.tmo_tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MudancaTurma:
                case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoMatricula:
                    return true;

                case MTR_TipoMovimentacaoTipoMovimento.Adequacao:                   
                    bool isCurriculoPeriodoEJA = ACA_CurriculoPeriodoBO.VerificaCurriculoPeriodoEJA(new ACA_CurriculoPeriodo { cur_id = cadMov.entAluCurNovo.cur_id, crr_id = cadMov.entAluCurNovo.crr_id, crp_id = cadMov.entAluCurNovo.crp_id }, bancoGestao);
                    return !(cadMov.entAluCurAnterior.cur_id == cadMov.entAluCurNovo.cur_id
                                && cadMov.entAluCurAnterior.crr_id == cadMov.entAluCurNovo.crr_id
                                && cadMov.entAluCurAnterior.crp_id == cadMov.entAluCurNovo.crp_id
                                && isCurriculoPeriodoEJA) 
                            && cadMov.entAluCurAnterior.esc_id == cadMov.entAluCurNovo.esc_id 
                            && cadMov.entAluCurAnterior.uni_id == cadMov.entAluCurNovo.uni_id;

                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                    return cadMov.entAluCurAnterior.esc_id == cadMov.entAluCurNovo.esc_id
                            && cadMov.entAluCurAnterior.uni_id == cadMov.entAluCurNovo.uni_id;

                case MTR_TipoMovimentacaoTipoMovimento.Reclassificacao:
                    ACA_CurriculoPeriodo entCurPerAnterior = new ACA_CurriculoPeriodo
                    {
                        cur_id = cadMov.entAluCurAnterior.cur_id,
                        crr_id = cadMov.entAluCurAnterior.crr_id,
                        crp_id = cadMov.entAluCurAnterior.crp_id
                    };
                    ACA_CurriculoPeriodoBO.GetEntity(entCurPerAnterior, bancoGestao);

                    ACA_CurriculoPeriodo entCurPerNovo = new ACA_CurriculoPeriodo
                    {
                        cur_id = cadMov.entAluCurNovo.cur_id,
                        crr_id = cadMov.entAluCurNovo.crr_id,
                        crp_id = cadMov.entAluCurNovo.crp_id
                    };
                    ACA_CurriculoPeriodoBO.GetEntity(entCurPerNovo, bancoGestao);

                    // Alterado para pegar os cursos/períodos equivalentes com o curso de destino.
                    List<ACA_CurriculoPeriodo> listPeriodosEquivalentes = ACA_CurriculoPeriodoBO.Seleciona_PeriodosRelacionados_Equivalentes(cadMov.entAluCurNovo.cur_id, cadMov.entAluCurNovo.crr_id, cadMov.entAluCurNovo.crp_id);

                    return ((entCurPerAnterior.cur_id == entCurPerNovo.cur_id && entCurPerAnterior.crr_id == entCurPerNovo.crr_id)
                                || (listPeriodosEquivalentes.Count > 0 && listPeriodosEquivalentes.Exists(p => p.cur_id == entCurPerNovo.cur_id && p.crr_id == entCurPerNovo.crr_id)))
                            && !(entCurPerAnterior.crp_ordem == entCurPerNovo.crp_ordem
                                && ACA_CurriculoPeriodoBO.VerificaCurriculoPeriodoEJA(entCurPerNovo, bancoGestao)
                                && entCurPerNovo.crp_turmaAvaliacao)
                            && entCurPerAnterior.crp_ordem == entCurPerNovo.crp_ordem - 1
                            && cadMov.entAluCurAnterior.esc_id == cadMov.entAluCurNovo.esc_id
                            && cadMov.entAluCurAnterior.uni_id == cadMov.entAluCurNovo.uni_id;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Retorna o TipoMovimento da última movimentação do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <param name="qtdeDiasAposMov">Quantidade de dias após a última movimentação</param>
        /// <returns>TipoMovimento da última movimentação do aluno</returns>
        public static MTR_TipoMovimentacaoTipoMovimento TipoMovimentoUltimaMovimentacaoAluno(Int64 alu_id, DateTime dataMovimentacao, TalkDBTransaction bancoGestao, out int qtdeDiasAposMov)
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO { _Banco = bancoGestao };
            DataTable dtRetorno = dao.RetornaTipoMomentoUltimaMovimentacaoAluno(alu_id);

            byte tmo_tipoMovimento = 0;
            if (dtRetorno.Rows.Count > 0)
            {
                tmo_tipoMovimento = Convert.ToByte(dtRetorno.Rows[0]["tmo_tipoMovimento"].ToString());
                DateTime dataRealizacaoMov = Convert.ToDateTime(dtRetorno.Rows[0]["mov_dataRealizacao"].ToString());
                qtdeDiasAposMov = (dataMovimentacao - dataRealizacaoMov).Days;
            }
            else
            {
                qtdeDiasAposMov = -1;
            }

            return (MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento;
        }

        /// <summary>
        /// Retorna o tipo de movimento da última movimentação do aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <param name="nomeTipoMovimentacao">Nome do tipo de movimentação</param>
        /// <returns></returns>
        public static MTR_TipoMovimentacaoTipoMovimento TipoMovimentoUltimaMovimentacaoAluno(Int64 alu_id, TalkDBTransaction bancoGestao, out string nomeTipoMovimentacao)
        {
            nomeTipoMovimentacao = string.Empty;

            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO { _Banco = bancoGestao };
            DataTable dtRetorno = dao.RetornaTipoMomentoUltimaMovimentacaoAluno(alu_id);

            byte tmo_tipoMovimento = 0;
            foreach (DataRow dr in dtRetorno.Rows)
            {
                tmo_tipoMovimento = Convert.ToByte(dr["tmo_tipoMovimento"].ToString());
                nomeTipoMovimentacao = dr["tmo_nome"].ToString();
                break;
            }

            return (MTR_TipoMovimentacaoTipoMovimento)tmo_tipoMovimento;
        }

        /// <summary>
        /// Retorna o tipo de movimento da última movimentação do aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public static byte TipoMovimentoUltimaMovimentacaoAluno(Int64 alu_id)
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            DataTable dtRetorno = dao.RetornaTipoMomentoUltimaMovimentacaoAluno(alu_id);

            byte tmo_tipoMovimento = 0;
            foreach (DataRow dr in dtRetorno.Rows)
            {
                tmo_tipoMovimento = Convert.ToByte(dr["tmo_tipoMovimento"].ToString());
                break;
            }

            return tmo_tipoMovimento;
        }

        /// <summary>
        /// Retorna a data de realização da última movimentação
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados - opcional (null para uma nova conexão)</param>
        /// <returns></returns>
        public static DateTime SelecionaDataRealizacaoUltimaMovimentacao(long alu_id, TalkDBTransaction bancoGestao)
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();

            if (bancoGestao != null)
                dao._Banco = bancoGestao;

            DataTable dt = dao.RetornaTipoMomentoUltimaMovimentacaoAluno(alu_id);

            string sDataRealizacao = string.Empty;

            if (dt.Rows.Count > 0)
                sDataRealizacao = dt.Rows[0]["mov_dataRealizacao"].ToString();

            return string.IsNullOrEmpty(sDataRealizacao) ? new DateTime() : Convert.ToDateTime(sDataRealizacao);
        }

        /// <summary>
        /// Retorna todos os parâmetros de movimentação não excluídos logicamente por entidade
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoMovimentacaoPorEntidade
        (
            Guid ent_id
        )
        {
            MTR_TipoMovimentacaoDAO dao = new MTR_TipoMovimentacaoDAO();
            return dao.SelecionaTipoMovimentacaoPorEntidade(ent_id);
        }
    }
}