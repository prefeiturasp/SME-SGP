/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using MSTech.GestaoEscolar.BLL.Caching;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Description: CLS_AlunoAvaliacaoTurmaObservacao Business Object. 
    /// </summary>
    public class CLS_AlunoAvaliacaoTurmaObservacaoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaObservacaoDAO, CLS_AlunoAvaliacaoTurmaObservacao>
    {
        #region Estruturas

        /// <summary>
        /// Dados de observaçao do aluno.
        /// </summary>
        [Serializable]
        public struct DadosAlunoObservacao
        {
            public long tur_id { get; set; }
            public int esc_id { get; set; }
            public long alu_id { get; set; }
            public int mtu_id { get; set; }
            public bool inativoBimestre { get; set; }
            public bool foraRede { get; set; }
            public string pes_nome { get; set; }
            public long arq_idFoto { get; set; }
            public int mtu_numeroChamada { get; set; }
            public string alc_matricula { get; set; }
            public DateTime mtu_dataMatricula { get; set; }
            public DateTime mtu_dataSaida { get; set; }
            public int fav_id { get; set; }
            public decimal fav_variacao { get; set; }
            public int cal_id { get; set; }
            public int cal_ano { get; set; }
            public int ava_id { get; set; }
            public int tpc_id { get; set; }
            public int tpc_ordem { get; set; }
            public string cap_descricao { get; set; }
            public bool calendarioFinalizado { get; set; }
            public bool bimestreAtual { get; set; }
            public bool periodoPassado { get; set; }
            public bool eventoAberto { get; set; }
            public bool ultimoPeriodo { get; set; }
            public string ato_qualidade { get; set; }
            public string ato_desempenhoAprendizado { get; set; }
            public string ato_recomendacaoAluno { get; set; }
            public string ato_recomendacaoResponsavel { get; set; }
            public int aat_id { get; set; }
            public string aat_justificativaPosConselho { get; set; }
            public bool bimestreAtivo { get; set; }
            public DateTime ato_dataAlteracao { get; set; }
            public string usuarioAlteracao { get; set; }
            public bool naoVisualizarDados { get; set; }
            public DateTime cap_dataInicio { get; set; }
            public DateTime cap_dataFim { get; set; }
            public string tur_codigoEOL { get; set; }
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva as observações para conselho pedagógico.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="ltObservacao">Lista de observações.</param>
        /// <param name="banco">Banco</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static bool SalvarObservacao
        (
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id,
            CLS_AlunoAvaliacaoTur_Observacao observacao,
            Guid usu_idLogado,
            byte resultado,
            string justificativaResultado,
            DateTime dataUltimaAlteracaoObservacao,
            DateTime dataUltimaAlteracaoNotaFinal,
            ACA_FormatoAvaliacao entFormatoAvaliacao,
            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina,
            List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina,
            int tamanhoMaximoKB,
            string[] TiposArquivosPermitidos,
            ref List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao,
            Guid ent_id,
            int tpc_id
        )
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaObservacaoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            List<AlunoFechamentoPendencia> FilaProcessamento = new List<AlunoFechamentoPendencia>();

            try
            {
                if (observacao.entityObservacao != null && observacao.entityObservacao != new CLS_AlunoAvaliacaoTurmaObservacao())
                    Save(observacao.entityObservacao, banco);
                int tpc_idUtilizado = -1;                 

                List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplinasUltimoPeriodo = new List<CLS_AvaliacaoTurDisc_Cadastro>();
                listaDisciplinasUltimoPeriodo = listaDisciplina.Where(d => d.entity.ava_id != ava_id).ToList();
                if (listaDisciplinasUltimoPeriodo.Count > 0)
                {
                    tpc_idUtilizado = ACA_AvaliacaoBO.SelecionaMaiorBimestre_ByFormatoAvaliacao(entFormatoAvaliacao.fav_id, banco);
                }
              
                CLS_AlunoAvaliacaoTurmaDisciplinaBO.SaveAvaliacaoFinal(
                        tur_id
                        , entFormatoAvaliacao
                        , listaDisciplina
                        , tamanhoMaximoKB
                        , TiposArquivosPermitidos
                        , dataUltimaAlteracaoNotaFinal
                        , listaMatriculaTurmaDisciplina
                        , ref listaAtualizacaoEfetivacao
                        , banco);

                tpc_idUtilizado = tpc_idUtilizado > 0 ? tpc_idUtilizado : tpc_id;
                if (entFormatoAvaliacao.fav_fechamentoAutomatico && listaMatriculaTurmaDisciplina.Any())
                    FilaProcessamento.AddRange(listaMatriculaTurmaDisciplina
                        .Select(p => new AlunoFechamentoPendencia {
                            tud_id = p.tud_id,
                            tpc_id = tpc_idUtilizado,
                            afp_frequencia = true,
                            afp_nota = true,
                            afp_processado = (Byte)(p.mtd_id <= 0 ? 0 : 2)
                        }).ToList());

                // Se for passado o resultado, salva na MTR_MatriculaTurma.
                if (!usu_idLogado.Equals(Guid.Empty))
                {
                    MTR_MatriculaTurma entMatr = new MTR_MatriculaTurma
                    {
                        alu_id = alu_id
                        ,
                        mtu_id = mtu_id
                    };
                    MTR_MatriculaTurmaBO.GetEntity(entMatr, banco);

                    // Se o registro foi alterado depois da data da alteração mais recente no momento em que os dados foram carregados,
                    // interrompe o salvamento e alerta o usuário de que é necessário atualizar os dados 
                    if (entMatr != null && !entMatr.IsNew && Convert.ToDateTime(entMatr.mtu_dataAlteracao.ToString()) > dataUltimaAlteracaoObservacao)
                    {
                        throw new ValidationException("Existe registro alterado mais recentemente, é necessário atualizar os dados.");
                    }
                    else
                    {
                        entMatr.usu_idResultado = usu_idLogado;
                        entMatr.mtu_resultado = resultado;
                        entMatr.mtu_relatorio = justificativaResultado;
                        MTR_MatriculaTurmaBO.Save(entMatr, banco);

                        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.GERAR_HISTORICO_SALVAR_PARECER_CONCLUSIVO, ent_id))
                        {
                            ACA_AlunoHistoricoBO.GeracaoHistoricoPedagogicoPorAluno(entMatr.alu_id, entMatr.mtu_id, banco);
                        }

                        if (entFormatoAvaliacao.fav_fechamentoAutomatico)
                            FilaProcessamento.Add(new AlunoFechamentoPendencia {
                                tud_id = listaMatriculaTurmaDisciplina.FirstOrDefault().tud_id,
                                tpc_id = tpc_id,
                                afp_frequencia = true,
                                afp_nota = true,
                                afp_processado = (Byte)(entMatr.IsNew == true ? 0 : 2)
                            });
                    }
                }

                // Limpa cache do fechamento, para atualizar o check.
                GestaoEscolarUtilBO.LimpaCache(MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(tur_id, fav_id, ava_id));
                List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                listaDisciplinas.ForEach(p =>
                {
                    GestaoEscolarUtilBO.LimpaCache(MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(p.tud_id, fav_id, ava_id, string.Empty));
                    GestaoEscolarUtilBO.LimpaCache(MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(p.tud_id, fav_id, ava_id, string.Empty));

                    // Chaves do fechamento automatico
                    string chave = string.Empty;
                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, p.tud_id, tpc_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, p.tud_id, tpc_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, p.tud_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, p.tud_id);
                    CacheManager.Factory.RemoveByPattern(chave); 

                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpc_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpc_id);
                    CacheManager.Factory.RemoveByPattern(chave);
                    //
                });

                if (entFormatoAvaliacao.fav_fechamentoAutomatico && FilaProcessamento.Any(a => a.tpc_id > 0))
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                        FilaProcessamento
                        .GroupBy(g => new { g.tud_id, g.tpc_id })
                        .Select(p => new AlunoFechamentoPendencia {
                            tud_id = p.FirstOrDefault().tud_id,
                            tpc_id = p.FirstOrDefault().tpc_id,
                            afp_frequencia = p.FirstOrDefault().afp_frequencia,
                            afp_nota = p.FirstOrDefault().afp_nota,
                            afp_processado = FilaProcessamento
                                .Where(w => w.tud_id == p.FirstOrDefault().tud_id && w.tpc_id == p.FirstOrDefault().tpc_id)
                                .Min(m => m.afp_processado)
                        })
                        .Where(w => w.tpc_id > 0 && w.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                        .ToList()
                    , banco);

                return true;
            }
            catch (ValidationException ex)
            {
                banco.Close(ex);
                throw;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método salva as observações para conselho pedagógico.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="ltObservacao">Lista de observações.</param>
        /// <param name="banco">Banco</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static bool SalvarObservacao
        (
            long tur_id,
            long alu_id,
            int mtu_id,
            long[] tud_ids,
            List<CLS_AlunoAvaliacaoTurmaObservacao> listaObservacao,
            List<CLS_AlunoAvaliacaoTurma> listaAlunoAvaliacaoTurma,
            List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAlunoAvaliacaoTurmaDisciplina,
            List<ACA_AlunoAnotacao> listaAlunoAnotacao,
            bool permiteEditarResultadoFinal,
            Guid usu_idLogado,
            byte resultado,
            DateTime dataUltimaAlteracaoObservacao,
            DateTime dataUltimaAlteracaoNotaFinal,
            ACA_FormatoAvaliacao entFormatoAvaliacao,
            List<CLS_AvaliacaoTurDisc_Cadastro> listaDisciplina,
            List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina,
            int tamanhoMaximoKB,
            string[] TiposArquivosPermitidos,
            ref List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao,
            Guid ent_id,
            IDictionary<int, int> dicAvaTpc,
            List<CLS_AlunoAvaliacaoTurmaObservacaoBO.DadosAlunoObservacao> listaDadosPeriodo
        )
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaObservacaoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            List<AlunoFechamentoPendencia> FilaProcessamento = new List<AlunoFechamentoPendencia>();
            int tpc_idUltimoPeriodo = (dicAvaTpc.Count() > 0 ? dicAvaTpc.Max(p => p.Value) : 0);

            try
            {
                foreach (CLS_AlunoAvaliacaoTurmaObservacao observacao in listaObservacao)
                {
                    observacao.usu_idAlteracao = usu_idLogado;
                    Save(observacao, banco);
                }

                foreach (CLS_AlunoAvaliacaoTurma alunoAvaliacaoTurma in listaAlunoAvaliacaoTurma)
                {
                    CLS_AlunoAvaliacaoTurmaBO.Save(alunoAvaliacaoTurma, banco);
                }


                object lockObject = new object();

                DataTable dtAlunoAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplina.TipoTabela_AlunoAvaliacaoTurmaDisciplina();
                if (listaAlunoAvaliacaoTurmaDisciplina.Any())
                {
                    Parallel.ForEach
                    (
                        listaAlunoAvaliacaoTurmaDisciplina,
                        alunoAvaliacaoTurmaDisciplina =>
                        {
                            lock (lockObject)
                            {
                                DataRow drAlunoAvaliacaoTurmaDisciplina = dtAlunoAvaliacaoTurmaDisciplina.NewRow();
                                dtAlunoAvaliacaoTurmaDisciplina.Rows.Add(
                                    CLS_AlunoAvaliacaoTurmaDisciplinaBO.EntityToDataRow(alunoAvaliacaoTurmaDisciplina, drAlunoAvaliacaoTurmaDisciplina));
                            }
                        }
                    );
                }

                if (dtAlunoAvaliacaoTurmaDisciplina.Rows.Count > 0)
                {
                    CLS_AlunoAvaliacaoTurmaDisciplinaBO.SalvarEmLotePosConselho(dtAlunoAvaliacaoTurmaDisciplina, banco);

                    if (entFormatoAvaliacao.fav_fechamentoAutomatico && listaAlunoAvaliacaoTurmaDisciplina.Any())
                        FilaProcessamento.AddRange(listaAlunoAvaliacaoTurmaDisciplina
                            .Select(p => new AlunoFechamentoPendencia {
                                tud_id = p.tud_id,
                                tpc_id = (p.tpc_id > 0 ? p.tpc_id : tpc_idUltimoPeriodo),
                                afp_frequencia = true,
                                afp_nota = true,
                                afp_processado = (Byte)(p.atd_id <= 0 ? 0 : 2)
                            }).ToList());
                }
                

                foreach (ACA_AlunoAnotacao alunoAnotacao in listaAlunoAnotacao)
                {
                    ACA_AlunoAnotacaoBO.Save(alunoAnotacao, banco);
                }

                CLS_AlunoAvaliacaoTurmaDisciplinaBO.SaveAvaliacaoFinal(
                        tur_id
                        , entFormatoAvaliacao
                        , listaDisciplina
                        , tamanhoMaximoKB
                        , TiposArquivosPermitidos
                        , dataUltimaAlteracaoNotaFinal
                        , listaMatriculaTurmaDisciplina
                        , ref listaAtualizacaoEfetivacao
                        , banco);

                if (entFormatoAvaliacao.fav_fechamentoAutomatico && listaMatriculaTurmaDisciplina.Any() && (tpc_idUltimoPeriodo > 0))
                    FilaProcessamento.AddRange(listaMatriculaTurmaDisciplina
                        .Select(p => new AlunoFechamentoPendencia {
                            tud_id = p.tud_id,
                            tpc_id = tpc_idUltimoPeriodo,
                            afp_frequencia = true,
                            afp_nota = true,
                            afp_processado = (Byte)(p.mtd_id <= 0 ? 0 : 2)
                        }).ToList());

                if (permiteEditarResultadoFinal)
                {
                    // Se for passado o resultado, salva na MTR_MatriculaTurma.
                    MTR_MatriculaTurma entMatr = new MTR_MatriculaTurma
                    {
                        alu_id = alu_id
                        ,
                        mtu_id = mtu_id
                    };
                    MTR_MatriculaTurmaBO.GetEntity(entMatr, banco);

                    // So registra a alteracao do parecer conclusivo (resultado)
                    // se for um registro novo e tem valor selecionado, ou se houve alteracao do valor.
                    if ((entMatr.IsNew && entMatr.mtu_resultado > 0) || entMatr.mtu_resultado != resultado)
                    {
                        // Se o registro foi alterado depois da data da alteração mais recente no momento em que os dados foram carregados,
                        // interrompe o salvamento e alerta o usuário de que é necessário atualizar os dados 
                        if (entMatr != null && !entMatr.IsNew && Convert.ToDateTime(entMatr.mtu_dataAlteracao.ToString()) > dataUltimaAlteracaoObservacao)
                        {
                            throw new ValidationException("Existe registro alterado mais recentemente, é necessário atualizar os dados.");
                        }
                        else
                        {
                            entMatr.usu_idResultado = usu_idLogado;
                            entMatr.mtu_resultado = resultado;
                            MTR_MatriculaTurmaBO.Save(entMatr, banco);

                            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.GERAR_HISTORICO_SALVAR_PARECER_CONCLUSIVO, ent_id))
                            {
                                ACA_AlunoHistoricoBO.GeracaoHistoricoPedagogicoPorAluno(entMatr.alu_id, entMatr.mtu_id, banco);
                            }
                                                     
                            if (entFormatoAvaliacao.fav_fechamentoAutomatico)
                                FilaProcessamento.Add(new AlunoFechamentoPendencia {
                                    tud_id = tud_ids.FirstOrDefault(),
                                    tpc_id = tpc_idUltimoPeriodo,
                                    afp_frequencia = true,
                                    afp_nota = true,
                                    afp_processado = (Byte)(entMatr.IsNew == true ? 0 : 2)
                                });
                        }
                    }
                }

                // Limpa cache do fechamento, para atualizar o check, as notas pos-conselho, o parecer e a sintese final.
                string chave = string.Empty;
                int tpcId = -1;
                listaDadosPeriodo.ForEach(q =>
                {
                    // Fechamento de bimestre
                    chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(tur_id, entFormatoAvaliacao.fav_id, q.ava_id);
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Remove(chave);
                    }
                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato(tur_id, entFormatoAvaliacao.fav_id, q.ava_id);
                    CacheManager.Factory.Remove(chave);

                    // Fechamento final
                    chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Final(tur_id, entFormatoAvaliacao.fav_id, q.ava_id);
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Remove(chave);
                    }
                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final(tur_id, entFormatoAvaliacao.fav_id, q.ava_id);
                    CacheManager.Factory.Remove(chave);

                    // Fechamento automatico de bimestre
                    if (dicAvaTpc.TryGetValue(q.ava_id, out tpcId))
                    {
                        chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpcId);
                        CacheManager.Factory.Remove(chave);
                    }

                    // Fechamento automatico final
                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_id);
                    CacheManager.Factory.Remove(chave);
                });
                List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                listaDisciplinas.ForEach(p =>
                {
                    // Fechamento de bimestre
                    chave = String.Format("{0}_{1}_{2}", ModelCache.FECHAMENTO_BIMESTRE_PATTERN_KEY, p.tud_id, entFormatoAvaliacao.fav_id);
                    CacheManager.Factory.RemoveByPattern(chave);
                    chave = String.Format("{0}_{1}_{2}", ModelCache.FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY, p.tud_id, entFormatoAvaliacao.fav_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    // Fechamento final
                    chave = String.Format("{0}_{1}_{2}", ModelCache.FECHAMENTO_FINAL_PATTERN_KEY, p.tud_id, entFormatoAvaliacao.fav_id);
                    CacheManager.Factory.RemoveByPattern(chave);
                    chave = String.Format("{0}_{1}_{2}", ModelCache.FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY, p.tud_id, entFormatoAvaliacao.fav_id);
                    CacheManager.Factory.RemoveByPattern(chave);

                    // Fechamento automatico final
                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, p.tud_id);
                    CacheManager.Factory.Remove(chave);
                    chave = String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, p.tud_id);
                    CacheManager.Factory.Remove(chave);

                    listaDadosPeriodo.ForEach(q =>
                    {
                        // Fechamento automatico de bimestre
                        if (dicAvaTpc.TryGetValue(q.ava_id, out tpcId))
                        {
                            chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, p.tud_id, tpcId);
                            CacheManager.Factory.Remove(chave);
                            chave = String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, p.tud_id, tpcId);
                            CacheManager.Factory.Remove(chave);
                        }
                    });
                });
                //

                //Adiciona tuds para processamento de pendência de notas (quando salva o parecer final
                foreach (long tud in tud_ids.Where(t => !FilaProcessamento.Any(f => f.tud_id == t)))
                    FilaProcessamento.Add(new AlunoFechamentoPendencia
                    {
                        tud_id = tud,
                        tpc_id = tpc_idUltimoPeriodo,
                        afp_frequencia = false,
                        afp_frequenciaExterna = false,
                        afp_nota = true,
                        afp_processado = (byte)2
                    });

                if (entFormatoAvaliacao.fav_fechamentoAutomatico && FilaProcessamento.Any(a => a.tpc_id > 0))
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                        FilaProcessamento
                        .GroupBy(g => new { g.tud_id, g.tpc_id })
                        .Select(p => new AlunoFechamentoPendencia {
                            tud_id = p.FirstOrDefault().tud_id,
                            tpc_id = p.FirstOrDefault().tpc_id,
                            afp_frequencia = p.FirstOrDefault().afp_frequencia,
                            afp_nota = p.FirstOrDefault().afp_nota,
                            afp_processado = FilaProcessamento
                                .Where(w => w.tud_id == p.FirstOrDefault().tud_id && w.tpc_id == p.FirstOrDefault().tpc_id)
                                .Min(m => m.afp_processado)
                        })
                        .Where(w => w.tpc_id > 0 && w.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                        .ToList()
                    , banco);

                return true;
            }
            catch (ValidationException ex)
            {
                banco.Close(ex);
                throw;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método salva um registro CLS_AlunoAvaliacaoTurmaObservacao
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaObservacao</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaObservacao entity)
        {
            if (entity.Validate())
            {
                CLS_AlunoAvaliacaoTurmaObservacao entAux = new CLS_AlunoAvaliacaoTurmaObservacao
                {
                    tur_id = entity.tur_id
                    ,
                    alu_id = entity.alu_id
                    ,
                    mtu_id = entity.mtu_id
                    ,
                    fav_id = entity.fav_id
                    ,
                    ava_id = entity.ava_id
                };
                GetEntity(entAux);

                entity.IsNew = entAux.IsNew;

                if (string.IsNullOrEmpty(entity.ato_qualidade) &&
                    string.IsNullOrEmpty(entity.ato_desempenhoAprendizado) &&
                    string.IsNullOrEmpty(entity.ato_recomendacaoAluno) &&
                    string.IsNullOrEmpty(entity.ato_recomendacaoResponsavel))
                {
                    return entity.IsNew ? true : Delete(entity);
                }
                else
                {
                    return new CLS_AlunoAvaliacaoTurmaObservacaoDAO().Salvar(entity);
                }
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro CLS_AlunoAvaliacaoTurmaObservacao
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaObservacao</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaObservacao entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                CLS_AlunoAvaliacaoTurmaObservacao entAux = new CLS_AlunoAvaliacaoTurmaObservacao
                {
                    tur_id = entity.tur_id
                    ,
                    alu_id = entity.alu_id
                    ,
                    mtu_id = entity.mtu_id
                    ,
                    fav_id = entity.fav_id
                    ,
                    ava_id = entity.ava_id
                };
                GetEntity(entAux, banco);

                entity.IsNew = entAux.IsNew;
                return new CLS_AlunoAvaliacaoTurmaObservacaoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion	

        #region Selects

        /// <summary>
        /// Seleciona os dados de observaçao por aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tev_idFechamento">Id do tipo de evento do fechamento do bimestre.</param>
        /// <returns>Dados do aluno.</returns>
        public static List<DadosAlunoObservacao> SelecionarPorAluno(long alu_id, int mtu_id, long tur_id, int tev_idFechamento, bool documentoOficial)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO();
            DataTable dt = dao.SelecionarPorAluno(alu_id, mtu_id, tur_id, tev_idFechamento, documentoOficial);
            List<DadosAlunoObservacao> lista = dt.Rows.Cast<DataRow>().Select(dr =>
                        new DadosAlunoObservacao
                        {
                            tur_id = Convert.ToInt64(dr["tur_id"]),
                            esc_id = Convert.ToInt32(dr["esc_id"]),
                            alu_id = Convert.ToInt64(dr["alu_id"]),
                            mtu_id = Convert.ToInt32(dr["mtu_id"]),
                            inativoBimestre = Convert.ToBoolean(dr["inativoBimestre"]),
                            foraRede = Convert.ToBoolean(dr["foraRede"]),
                            pes_nome = Convert.ToString(dr["pes_nome"]),
                            arq_idFoto = Convert.ToInt64(string.IsNullOrEmpty(dr["arq_idFoto"].ToString()) ? "0" : dr["arq_idFoto"].ToString()),
                            mtu_numeroChamada = Convert.ToInt32(dr["mtu_numeroChamada"]),
                            alc_matricula = Convert.ToString(dr["alc_matricula"]),
                            mtu_dataMatricula = Convert.ToDateTime(dr["mtu_dataMatricula"]),
                            mtu_dataSaida = Convert.ToDateTime((!string.IsNullOrEmpty(dr["mtu_dataSaida"].ToString())) ? dr["mtu_dataSaida"] : new DateTime().ToString()),
                            fav_id = Convert.ToInt32(dr["fav_id"]),
                            fav_variacao = Convert.ToDecimal(dr["fav_variacao"]),
                            cal_id = Convert.ToInt32(dr["cal_id"]),
                            cal_ano = Convert.ToInt32(dr["cal_ano"]),
                            ava_id = Convert.ToInt32(dr["ava_id"]),
                            tpc_id = Convert.ToInt32(dr["tpc_id"]),
                            tpc_ordem = Convert.ToInt32(dr["tpc_ordem"]),
                            cap_descricao = Convert.ToString(dr["cap_descricao"]),
                            calendarioFinalizado = Convert.ToBoolean(dr["calendarioFinalizado"]),
                            bimestreAtual = Convert.ToBoolean(dr["bimestreAtual"]),
                            periodoPassado = Convert.ToBoolean(dr["periodoPassado"]),
                            eventoAberto = Convert.ToBoolean(dr["eventoAberto"]),
                            ultimoPeriodo = Convert.ToBoolean(dr["ultimoPeriodo"]),
                            ato_qualidade = Convert.ToString(dr["ato_qualidade"]),
                            ato_desempenhoAprendizado = Convert.ToString(dr["ato_desempenhoAprendizado"]),
                            ato_recomendacaoAluno = Convert.ToString(dr["ato_recomendacaoAluno"]),
                            ato_recomendacaoResponsavel = Convert.ToString(dr["ato_recomendacaoResponsavel"]),
                            aat_id = Convert.ToInt32(dr["aat_id"]),
                            aat_justificativaPosConselho = Convert.ToString(dr["aat_justificativaPosConselho"]),
                            bimestreAtivo = Convert.ToBoolean(dr["bimestreAtivo"]),
                            ato_dataAlteracao = !string.IsNullOrEmpty(dr["ato_dataAlteracao"].ToString()) ? Convert.ToDateTime(dr["ato_dataAlteracao"]) : new DateTime(),
                            usuarioAlteracao = dr["usuarioAlteracao"].ToString(),
                            naoVisualizarDados = Convert.ToBoolean(dr["naoVisualizarDados"]),
                            cap_dataInicio = Convert.ToDateTime(dr["cap_dataInicio"]),
                            cap_dataFim = Convert.ToDateTime(dr["cap_dataFim"]),
                            tur_codigoEOL = dr["tur_codigoEOL"].ToString()
                        }).ToList();

            return lista;
        }

        /// <summary>
        /// Seleciona os dados de observaçao por aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tev_idFechamento">Id do tipo de evento do fechamento do bimestre.</param>
        /// <returns>CLS_AlunoAvaliacaoTurmaObservacao.</returns>
        public static List<CLS_AlunoAvaliacaoTurmaObservacao> SelecionaListaPorAluno(long alu_id, int mtu_id, long tur_id, int tev_idFechamento, bool documentoOficial)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO();
            DataTable dtRet = dao.SelecionarPorAluno(alu_id, mtu_id, tur_id, tev_idFechamento, documentoOficial);

            List<CLS_AlunoAvaliacaoTurmaObservacao> result = new List<CLS_AlunoAvaliacaoTurmaObservacao>();
            foreach (DataRow dr in dtRet.Rows)
            {
                result.Add((CLS_AlunoAvaliacaoTurmaObservacao)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaObservacao()));
            }

            return result;
        }

        #endregion
    }
}