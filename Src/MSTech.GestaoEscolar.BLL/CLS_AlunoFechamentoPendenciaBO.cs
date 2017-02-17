/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using System;
    using MSTech.GestaoEscolar.BLL.Caching;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Linq;

    #region Estruturas


    /// <summary>
    /// Estrutura utilizada listar os tuds e tpcs que devem ser incluidos na fila de fechamento automatico.
    /// </summary>
    [Serializable]
    public struct AlunoFechamentoPendencia
    {
        public long tud_id { get; set; }

        public int tpc_id { get; set; }

        public bool afp_frequencia { get; set; }

        public bool afp_nota { get; set; }

        public Byte afp_processado { get; set; }

    }

    [Serializable]
    public struct sParametroAtualizacaoFechamentoDisciplina
    {
        public long tur_id { get; set; }
        public int cal_id { get; set; }
        public int esc_id { get; set; }
        public int uni_id { get; set; }
        public long tud_id { get; set; }
        public byte tud_tipo { get; set; }
        public bool tud_naoLancarNota { get; set; }
        public int fav_id { get; set; }
        public byte fav_tipoLancamentoFrequencia { get; set; }
        public byte fav_calculoQtdeAulasDadas { get; set; }
        public decimal fav_variacao { get; set; }
        public byte fav_criterioAprovacaoResultadoFinal { get; set; }
        public decimal fav_percentualMinimoFrequenciaFinalAjustadaDisciplina { get; set; }
    }

    [Serializable]
    public struct sAtualizacaoFechamento
    {
        public sParametroAtualizacaoFechamentoDisciplina TurmaDisciplina { get; set; }
        public List<ACA_CalendarioPeriodo> listaCalendarioPeriodo { get; set; }
        public List<MTR_MatriculaTurmaDisciplina> listaAluno { get; set; }
        public List<TUR_TurmaDisciplinaAulaPrevista> listaAulaPrevista { get; set; }
        public int qtdeTitulares { get; set; }
        public List<CLS_TurmaAula> listaAula { get; set; }
        public List<CLS_TurmaAulaAluno> listaFrequencia { get; set; }
        public List<CLS_CompensacaoAusencia> listaCompensacao { get; set; }
        public List<CLS_CompensacaoAusenciaAluno> listaCompensacaoAluno { get; set; }
        public List<CLS_AlunoAvaliacaoTurmaDisciplina> listaFechamento { get; set; }
        public List<CLS_AlunoFechamento> listaAlunoFechamento { get; set; }
        public List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listaAlunoDisciplinaMedia { get; set; }
        public List<ACA_Evento> listaEvento { get; set; }
        public List<ACA_Avaliacao> listaAvaliacao { get; set; }
    }

    [Serializable]
    public struct sQuantidadeAulaFalta
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int qtAulas { get; set; }
        public int qtdeFaltas { get; set; }
        public int qtFaltasReposicao { get; set; }
        public int qtAulasNormais { get; set; }
        public int qtAulasReposicao { get; set; }
        public int qtFaltasReposicaoNaoAcumuladas { get; set; }
    }

    [Serializable]
    public struct sQuantidadeAulaFaltaAdicional
    {
        public long tud_idOrigem { get; set; }
        public long alu_idOrigem { get; set; }
        public int mtu_idOrigem { get; set; }
        public int mtd_idOrigem { get; set; }
        public int tpc_id { get; set; }
        public int tpc_ordem { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }

        public int tau_id { get; set; }
        public DateTime tau_data { get; set; }
        public int tau_numeroAulas { get; set; }
        public bool tau_reposicao { get; set; }
        public int taa_frequencia { get; set; }
    }

    [Serializable]
    public struct sCompensacaoFechamento
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int cpa_quantidadeAulasCompensadas { get; set; }
        public int cpa_quantidadeAulasCompensadasAcumuladas { get; set; }
    }

    [Serializable]
    public struct sFechamentoAnteriores
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int quantidadeAulasAcumuladas { get; set; }
        public int quantidadeFaltasAcumuladas { get; set; }
    }

    [Serializable]
    public struct sSomatorioAulasFaltasFechamento
    {
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public int faltas { get; set; }
        public int aulas { get; set; }
        public int compensadas { get; set; }
        public int compensadasAcumuladas { get; set; }
    }

    [Serializable]
    public struct sAlunosDisciplinasPeriodos
    {
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public int tpc_id { get; set; }
        public long tur_id { get; set; }
        public DateTime cap_dataInicio { get; set; }
        public DateTime cap_dataFim { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: CLS_AlunoFechamentoPendencia Business Object. 
    /// </summary>
    public class CLS_AlunoFechamentoPendenciaBO : BusinessBase<CLS_AlunoFechamentoPendenciaDAO, CLS_AlunoFechamentoPendencia>
    {
        /// <summary>
        ///	Retorna os logs da execução de fechamento de acordo com os filtros.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaExecucoesFila(int top, bool somenteCompleta)
        {
            return new CLS_AlunoFechamentoPendenciaDAO().SelecionaExecucoesFila(top, somenteCompleta);
        }

        /// <summary>
        /// Retorna as quantidades da fila de acordo com a situação.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaFila_PorSituacao()
        {
            return new CLS_AlunoFechamentoPendenciaDAO().SelecionaFila_PorSituacao();
        }

        public delegate bool ProcessarFilaPendente(long tud_id, byte ava_tipo, DataTable pendencias);

        /// <summary>
        /// Salva item na fila de processamento com a flag frequência marcada.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarFilaFrequencia(long tud_id, int tpc_id, TalkDBTransaction banco)
        {
            RemoveCacheFechamentoAutomatico(tud_id);
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { _Banco = banco };
            return dao.SalvarFilaFrequencia(tud_id, tpc_id);
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag frequência marcada.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarFilaFrequencia(long tud_id, int tpc_id)
        {
            RemoveCacheFechamentoAutomatico(tud_id);
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { };
            return dao.SalvarFilaFrequencia(tud_id, tpc_id);
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag nota marcada.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarFilaNota(long tud_id, int tpc_id, TalkDBTransaction banco)
        {
            RemoveCacheFechamentoAutomatico(tud_id);
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { _Banco = banco };
            return dao.SalvarFilaNota(tud_id, tpc_id);
        }

        /// <summary>
        /// Salva item na fila de processamento com a flag nota marcada.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarFilaNota(long tud_id, int tpc_id)
        {
            RemoveCacheFechamentoAutomatico(tud_id);
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { };
            return dao.SalvarFilaNota(tud_id, tpc_id);
        }

        /// <summary>
        /// Salva item na fila de processamento com a processo = 2, para verificar pendencia de fechamento.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarFilaPendencias(long tud_id, int tpc_id, TalkDBTransaction banco)
        {
            RemoveCacheFechamentoAutomatico(tud_id);
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { _Banco = banco };
            return dao.SalvarFilaPendencias(tud_id, tpc_id);
        }

        /// <summary>
        /// Salva item na fila de processamento com a processo = 2, para verificar pendencia de fechamento.
        /// </summary>
        /// <param name="listFila">Lista com tpc_ids e tud_ids.</param>
        /// <returns></returns>
        public static void SalvarFilaPendencias(List<AlunoFechamentoPendencia> listFila, TalkDBTransaction banco)
        {
            object lockObject = new object();

            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO { _Banco = banco };
            DataTable dtAlunoFechamentoPendencia = CLS_AlunoFechamentoPendencia.TipoTabela_AlunoFechamentoPendencia();
            if (listFila.Any())
            {
                Parallel.ForEach
                (
                    listFila,
                    alunoFechamentoPendencia =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtAlunoFechamentoPendencia.NewRow();
                            dtAlunoFechamentoPendencia.Rows.Add(AlunoFechamentoPendenciaToDataRow(alunoFechamentoPendencia, dr));
                        }
                    }
                );
            }
            dao.SalvarFilaPendencias(dtAlunoFechamentoPendencia);
        }

        /// <summary>
        /// Retorna se existe registro nao processado na fila de fechamento para a turma disciplina e periodo.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        /// <param name="tud_tipo">Tipo da disciplina na turma.</param>
        /// <param name="tpc_id">Id do tipo período calendário.</param>
        /// <returns></returns>
        public static DataTable SelecionarAguardandoProcessamento(long tur_id, long tud_id, byte tud_tipo, int tpc_id = -1)
        {
            CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO();
            return dao.SelecionarAguardandoProcessamento(tur_id, tud_id, tud_tipo, tpc_id);
        }

        public static bool Processar(long tud_id, byte ava_tipo, DataTable pendencias)
        {
            if (pendencias == null || pendencias.Rows.Count == 0)
                return true;
            else
            {
                CLS_AlunoFechamentoPendenciaDAO dao = new CLS_AlunoFechamentoPendenciaDAO();
                                
                bool retorno = dao.Processar(tud_id);
                        
                if (ava_tipo == (byte)AvaliacaoTipo.Final)
                {
                    dao.ProcessarPendenciasAnteriores(tud_id);
                }

                return retorno;
            }
        }
                
        /// <summary>
        /// Remove os caches do fechamento automatico.
        /// </summary>
        /// <param name="tud_id">Id da disciplina na turma.</param>
        private static void RemoveCacheFechamentoAutomatico(long tud_id)
        {
            string chave = ModelCache.FECHAMENTO_AUTO_BIMESTRE_PATTERN_KEY + "_" + tud_id;
            CacheManager.Factory.RemoveByPattern(chave);

            chave = ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY + "_" + tud_id;
            CacheManager.Factory.RemoveByPattern(chave);

            chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_id);
            CacheManager.Factory.RemoveByPattern(chave);

            chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id);
            CacheManager.Factory.RemoveByPattern(chave);

            TUR_TurmaDisciplina turmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_id });
            if (turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
            {
                TUR_Turma turma = TUR_TurmaRelTurmaDisciplinaBO.SelecionarTurmaPorTurmaDisciplina(tud_id);

                chave = ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY + "_" + turma.tur_id;
                CacheManager.Factory.RemoveByPattern(chave);

                chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, turma.tur_id);
                CacheManager.Factory.RemoveByPattern(chave);
            }
        }

        /// <summary>
        /// O método converte um registro da CLS_AlunoFechamentoPendencia em um DataRow.
        /// </summary>
        /// <param name="alunoFechamentoPendencia">Registro da CLS_AlunoFechamentoPendencia.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        public static DataRow AlunoFechamentoPendenciaToDataRow(AlunoFechamentoPendencia alunoFechamentoPendencia, DataRow dr, DateTime tna_dataAlteracao = new DateTime())
        {
            dr["tud_id"] = alunoFechamentoPendencia.tud_id;
            dr["tpc_id"] = alunoFechamentoPendencia.tpc_id;
            dr["afp_frequencia"] = alunoFechamentoPendencia.afp_frequencia;
            dr["afp_nota"] = alunoFechamentoPendencia.afp_nota;
            dr["afp_processado"] = alunoFechamentoPendencia.afp_processado;

            return dr;
        }
    }
}