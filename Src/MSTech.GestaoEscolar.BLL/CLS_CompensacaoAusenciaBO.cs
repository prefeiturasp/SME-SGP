/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.CoreSSO.BLL;
    using MSTech.CoreSSO.Entities;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Validation.Exceptions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Description: CLS_CompensacaoAusencia Business Object.
    /// </summary>
    public class CLS_CompensacaoAusenciaBO : BusinessBase<CLS_CompensacaoAusenciaDAO, CLS_CompensacaoAusencia>
    {
        #region Metodos

        #region Sincronizacao diario de classe

        /// <summary>
        /// Processa os protocolos informados.
        /// </summary>
        /// <param name="ltProtocolo">Lista de protocolos em processamento.</param>
        /// <param name="tentativasProtocolo">Quantidade máxima de tentativas para processar protocolos.</param>
        /// <returns></returns>
        public static bool ProcessarProtocoloCompensacao(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
        {
            DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa > tentativasProtocolo))
            {
                protocolo.pro_statusObservacao = String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                , tentativasProtocolo, protocolo.pro_statusObservacao);
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                protocolo.tur_id = -1;
                protocolo.tud_id = -1;
                protocolo.tau_id = -1;
                protocolo.pro_qtdeAlunos = -1;
                dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
            }

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa <= tentativasProtocolo))
            {
                // Abre uma transação para cada protocolo dentro do laço.
                // Assim é possível processar o próximo protocolo, mesmo que o atual esteja com erro.
                TalkDBTransaction bancoSincronizacao = new CLS_TurmaAulaDAO()._Banco.CopyThisInstance();
                bancoSincronizacao.Open(IsolationLevel.ReadCommitted);
                bool processou = false;

                try
                {
                    if (protocolo.pro_tentativa <= tentativasProtocolo)
                    {
                        JObject json = JObject.Parse(protocolo.pro_pacote);

                        JArray compensacoes = ((JArray)json.SelectToken("CompensacaoFalta") ?? new JArray());

                        foreach (JObject compensacao in compensacoes)
                        {
                            long tud_id = Convert.ToInt64(compensacao.SelectToken("tud_id"));

                            ACA_FormatoAvaliacao formatoAvaliacao = ACA_FormatoAvaliacaoBO.CarregarPorTud(tud_id);
                            ACA_CalendarioAnual cal = ACA_CalendarioAnualBO.SelecionaPorTurmaDisciplina(tud_id, bancoSincronizacao);

                            // apenas protocolos de turmas ativas e do ano letivo corrente podem ser processados
                            if (!DCL_ProtocoloBO.PodeProcessarProtocolo(0, tud_id))
                                throw new ValidationException("O protocolo pertence a uma turma que não esta ativa ou de um ano letivo diferente do corrente, não pode ser processado!");

                            int cpa_id = Convert.ToInt32(compensacao.SelectToken("cpa_id"));
                            int quantidadeAulasCompensadas = Convert.ToInt32(compensacao.SelectToken("cpa_quantidadeAulasCompensadas"));

                            DateTime cpa_dataAlteracao = Convert.ToDateTime(compensacao.SelectToken("cpa_dataAlteracao"));

                            CLS_CompensacaoAusencia compensacaoAusencia;

                            long pro_protocoloCriacao = (long)(compensacao.GetValue("pro_protocolo", StringComparison.OrdinalIgnoreCase) ?? "0");
                            if (cpa_id > 0 || pro_protocoloCriacao <= 0)
                            {
                                compensacaoAusencia = new CLS_CompensacaoAusencia
                                {
                                    tud_id = tud_id,
                                    cpa_id = cpa_id
                                };
                                GetEntity(compensacaoAusencia, bancoSincronizacao);
                            }
                            else
                            {
                                List<DCL_Protocolo> protocoloCriacaoCompensacao = DCL_ProtocoloBO.SelectBy_Protocolos(pro_protocoloCriacao.ToString());
                                Guid pro_idCriacao = protocoloCriacaoCompensacao.Count() > 0 ? protocoloCriacaoCompensacao[0].pro_id : Guid.Empty;
                                compensacaoAusencia = SelectByDisciplinaProtocolo(tud_id, pro_idCriacao);
                                if (!compensacaoAusencia.IsNew)
                                {
                                    compensacaoAusencia.pro_id = protocolo.pro_id;
                                }
                            }

                            if (compensacaoAusencia.IsNew)
                            {
                                compensacaoAusencia.cpa_id = -1;
                                compensacaoAusencia.cpa_dataCriacao = DateTime.Now;
                                compensacaoAusencia.pro_id = protocolo.pro_id;
                            }
                            else if (!compensacaoAusencia.IsNew && (cpa_dataAlteracao < compensacaoAusencia.cpa_dataAlteracao))
                            {
                                throw new ValidationException("Compensação existe e foi alterada mais recentemente.");
                            }

                            compensacaoAusencia.tpc_id = Convert.ToInt32(compensacao.SelectToken("tpc_id"));
                            compensacaoAusencia.cpa_quantidadeAulasCompensadas = quantidadeAulasCompensadas;
                            compensacaoAusencia.cpa_atividadesDesenvolvidas = Convert.ToString(compensacao.SelectToken("cpa_atividadesDesenvolvidas"));
                            compensacaoAusencia.cpa_situacao = Convert.ToInt16(compensacao.SelectToken("cpa_situacao"));
                            compensacaoAusencia.cpa_dataAlteracao = DateTime.Now;                            

                            if (compensacaoAusencia.cpa_situacao != 3)
                            {
                                JArray arrayAlunos = ((JArray)compensacao.SelectToken("AlunosCompensados") ?? new JArray());
                                List<CLS_CompensacaoAusenciaAluno> listaAlunos = new List<CLS_CompensacaoAusenciaAluno>();
                                foreach (JObject jsonAluno in arrayAlunos)
                                {
                                    int alu_id = Convert.ToInt32(jsonAluno.SelectToken("alu_id"));
                                    MTR_MatriculaTurmaDisciplina matricula = MTR_MatriculaTurmaDisciplinaBO.BuscarPorAlunoTurmaDisciplina(alu_id, tud_id, bancoSincronizacao);

                                    CLS_CompensacaoAusenciaAluno compensacaoAluno = new CLS_CompensacaoAusenciaAluno
                                    {
                                        tud_id = tud_id,
                                        cpa_id = compensacaoAusencia.cpa_id,
                                        alu_id = alu_id,
                                        mtu_id = matricula.mtu_id,
                                        mtd_id = matricula.mtd_id
                                    };

                                    CLS_CompensacaoAusenciaAlunoBO.GetEntity(compensacaoAluno);

                                    if (compensacaoAluno.IsNew)
                                    {
                                        compensacaoAluno.caa_dataCriacao = DateTime.Now;
                                    }

                                    compensacaoAluno.caa_situacao = 1;
                                    compensacaoAluno.caa_dataAlteracao = DateTime.Now;


                                    listaAlunos.Add(compensacaoAluno);
                                }

                                processou = Save(compensacaoAusencia, listaAlunos, formatoAvaliacao.fav_fechamentoAutomatico, cal.cal_id, bancoSincronizacao);

                            }
                            else
                            {
                                // persistindo os dados.
                                processou = true;
                                // Caso o fechamento seja automático, grava na fila de processamento.
                                if (formatoAvaliacao.fav_fechamentoAutomatico && compensacaoAusencia.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, new Guid()))
                                {
                                    processou &= CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(compensacaoAusencia.tud_id, compensacaoAusencia.tpc_id, bancoSincronizacao);
                                }
                                processou &= Delete(compensacaoAusencia, bancoSincronizacao);
                            }
                        }
                    }


                    // Processou com sucesso.
                    protocolo.pro_statusObservacao = String.Format("Protocolo processado com sucesso ({0}).",
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                }
                catch (ArgumentException ex)
                {
                    // Se ocorrer uma excessão de validação, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoSincronizacao.Close(ex);
                }
                catch (ValidationException ex)
                {
                    // Se ocorrer uma excessão de validação, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoSincronizacao.Close(ex);
                }
                catch (Exception ex)
                {
                    // Se ocorrer uma excessão de erro, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoSincronizacao.Close(ex);
                }
                finally
                {
                    if (bancoSincronizacao.ConnectionIsOpen)
                        bancoSincronizacao.Close();
                }
            }

            DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);

            return true;
        }

        #endregion Sincronizacao diario de classe

        /// <summary>
        /// Seleciona os avisos.
        /// </summary>
        /// <param name="uad_idSuperior">The uad_id superior.</param>
        /// <param name="esc_id">The esc_id.</param>
        /// <param name="uni_id">The uni_id.</param>
        /// <param name="cur_id">The cur_id.</param>
        /// <param name="crr_id">The crr_id.</param>
        /// <param name="cap_id">The cap_id.</param>
        /// <param name="tud_id">The tud_id.</param>
        /// <param name="gru_id">The gru_id.</param>
        /// <param name="usu_id">The usu_id.</param>
        /// <param name="adm">if set to <c>true</c> [adm].</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectByPesquisa(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, int cap_id, long tud_id, Guid gru_id, Guid usu_id, bool adm, long tur_id, Guid ent_id)
        {
            // Tipo de evento de efetivação de notas, configurado nos parâmetros.
            int tev_idEfetivacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            CLS_CompensacaoAusenciaDAO dao = new CLS_CompensacaoAusenciaDAO();
            return dao.SelectByPesquisa(uad_idSuperior, esc_id, uni_id, cur_id, crr_id, cap_id, tud_id, gru_id, usu_id, adm, tur_id, tev_idEfetivacao, out totalRecords);
        }

        /// <summary>
        /// Pesquisa da tela de compensação de ausência.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior da escola.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="cap_id">ID do período do calendário.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="gru_id">ID do grupo do usuário logado.</param>
        /// <param name="usu_id">ID do usuário logado.</param>
        /// <param name="adm">Flag que indica se o usuário é administrador do sistema.</param>
        /// <param name="tev_idEfetivacao">ID do tipo de evento da efetivação.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tipoDocente">Tipo de docente.</param>
        /// <param name="totalRecords"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectByPesquisaFiltroDeficiencia(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, int cap_id, long tud_id, Guid gru_id, Guid usu_id, bool adm, long tur_id, EnumTipoDocente tipoDocente, Guid ent_id)
        {
            // Tipo de evento de efetivação de notas, configurado nos parâmetros.
            int tev_idEfetivacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            return new CLS_CompensacaoAusenciaDAO().SelectByPesquisaFiltroDeficiencia(uad_idSuperior, esc_id, uni_id, cur_id, crr_id, cap_id, tud_id, gru_id, usu_id, adm, tur_id, tev_idEfetivacao, (byte)tipoDocente, out totalRecords);
        }

        /// <summary>
        /// Seleciona compensações de ausência para o aluno, segundo parâmetros.
        /// </summary>
        /// <param name="tud_id">ID disciplina da turma</param>
        /// <param name="tpc_id">ID tipo de período do calendário</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula</param>
        /// <param name="mtd_id">ID matrícula-disciplina</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorAluno(long tud_id, int tpc_id, long alu_id, int mtu_id, int mtd_id)
        {
            CLS_CompensacaoAusenciaDAO dao = new CLS_CompensacaoAusenciaDAO();
            return dao.SelecionaPorAluno(tud_id, tpc_id, alu_id, mtu_id, mtd_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os ids necessários para carregar os combos da tela de cadastro.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="cpa_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable RetornaIdsCadastro(long tud_id, int cpa_id)
        {
            CLS_CompensacaoAusenciaDAO dao = new CLS_CompensacaoAusenciaDAO();
            return dao.RetornaIdsCadastro(tud_id, cpa_id);
        }

        /// <summary>
        /// Override do Save passando o banco - salva tambem a tabela CLS_CompensacaoAusenciaAluno
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="lista">Lista de CLS_CompensacaoAusenciaAluno a ser salva</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns>Se salvou com sucesso</returns>
        public static bool Save(CLS_CompensacaoAusencia entity, List<CLS_CompensacaoAusenciaAluno> lista, bool fechamentoAutomatico, int cal_id, TalkDBTransaction banco)
        {
            // permite cadatrar compensacao apenas para o ultimo periodo aberto,
            // o metodo ja retorna os periodos abertos ordenados do mais recente para o mais antigo            
            List<sComboPeriodoCalendario> dtPeriodos = ACA_TipoPeriodoCalendarioBO.SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(-1, entity.tud_id, -1, Guid.Empty, true);
            if (dtPeriodos.Count > 0 && dtPeriodos.Any(p => p.tpc_id == entity.tpc_id))
            {
                //validacoes
                DataTable dt = MTR_MatriculaTurmaDisciplinaBO.SelecionaAtivosCompensacaoAusencia(
                    entity.tud_id,
                    entity.tpc_id,
                    0,
                    entity.cpa_id
                );
                dt.PrimaryKey = new DataColumn[1] { dt.Columns["alu_id"] };

                TUR_TurmaDisciplina turmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = entity.tud_id });
                // se for disciplina especial
                if (turmaDisciplina.tud_disciplinaEspecial
                    // e o docente do tipo especial tem permissao de editar a compensacao
                    && CFG_PermissaoDocenteBO.SelecionaPermissaoModulo((byte)EnumTipoDocente.Especial, (byte)EnumModuloPermissao.Compensacoes).Any(p => p.pdc_permissaoEdicao))
                {
                    // adiciono na lista de validacao os aluno especiais
                    DataTable dtEspecial = MTR_MatriculaTurmaDisciplinaBO.SelecionaAtivosCompensacaoAusenciaFiltroDeficiencia(
                                                -1,
                                                entity.tud_id,
                                                entity.tpc_id,
                                                0,
                                                EnumTipoDocente.Especial,
                                                entity.cpa_id);
                    dt.Merge(dtEspecial, false, MissingSchemaAction.AddWithKey);
                }

                bool valida = true;
                string msgValidacao = "";

                if (turmaDisciplina.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                {
                    if (!TUR_TurmaDisciplinaTerritorioBO.VerificaOferecimentoExperienciaBimestre(turmaDisciplina.tud_id, cal_id, entity.tpc_id))
                    {
                        valida = false;
                        msgValidacao += "A experiência não possui territórios vigentes no bimestre.";
                    }
                }

                // caso exista alguma compensação já criada pro aluno
                // valida se pode existir alguma outra.
                foreach (CLS_CompensacaoAusenciaAluno cpaa in lista)
                {
                    var results = from row in dt.AsEnumerable()
                                  where row.Field<Int64>("alu_id") == cpaa.alu_id
                                  select row;

                    if (results.Count() > 0)
                    {
                        DataTable aux = results.CopyToDataTable();
                        int totalFaltasCompensar = Convert.ToInt32(aux.Rows[0]["TotalFaltasCompensar"]);
                        if (entity.cpa_quantidadeAulasCompensadas > totalFaltasCompensar)
                        {
                            valida = false;
                            msgValidacao += String.Format("Aluno(a) {0} não pode ter essa quantidade de ausências compensadas. </br>", aux.Rows[0]["pes_nome"].ToString());
                        }
                    }
                    else
                    {
                        valida = false;
                        ACA_Aluno aluno = ACA_AlunoBO.GetEntity(new ACA_Aluno { alu_id = cpaa.alu_id });
                        PES_Pessoa pes = PES_PessoaBO.GetEntity(new PES_Pessoa { pes_id = aluno.pes_id });
                        msgValidacao += String.Format("Aluno(a) {0} não pode ter ausências compensadas nos bimestres em que não estava na turma. </br>", pes.pes_nome);
                    }
                }
                if (!valida)
                    throw new ValidationException(msgValidacao);

                // Salva CLS_CompensacaoAusencia
                if (!Save(entity, banco))
                    throw new Exception("Erro ao tentar salvar compensação de ausência.");

                // Caso o fechamento seja automático, grava na fila de processamento.
                if (fechamentoAutomatico && entity.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, new Guid()))
                {
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(entity.tud_id, entity.tpc_id, banco);
                }

                List<CLS_CompensacaoAusenciaAluno> listaBanco = CLS_CompensacaoAusenciaAlunoBO.SelectByCpa_id(entity.cpa_id, entity.tud_id);

                foreach (CLS_CompensacaoAusenciaAluno item in lista)
                {
                    CLS_CompensacaoAusenciaAluno ent = listaBanco.Find(p => p.tud_id == item.tud_id &&
                                                                        p.cpa_id == entity.cpa_id &&
                                                                        p.alu_id == item.alu_id &&
                                                                        p.mtd_id == item.mtd_id &&
                                                                        p.mtu_id == item.mtu_id);

                    if (ent != null)
                    {
                        // Achou na lista que vem no banco, altera.
                        ent.IsNew = false;
                        CLS_CompensacaoAusenciaAlunoBO.Save(ent, banco);

                        // Remove o registro da lista do banco para restar somente os que serao excluidos.
                        listaBanco.Remove(ent);
                    }
                    else
                    {
                        // Não achou na lista do banco, inclui.
                        item.cpa_id = entity.cpa_id;
                        CLS_CompensacaoAusenciaAlunoBO.GetEntity(item, banco);
                        if (item.caa_situacao == 3)
                        {
                            item.caa_situacao = 1;
                            item.IsNew = false;
                        }
                        else
                            item.IsNew = true;
                        CLS_CompensacaoAusenciaAlunoBO.Save(item, banco);
                    }
                }

                if (listaBanco.Count > 0)
                {
                    foreach (CLS_CompensacaoAusenciaAluno item in listaBanco)
                    {
                        CLS_CompensacaoAusenciaAlunoBO.Delete(item, banco);
                    }
                }
            }
            else
            {
                throw new ValidationException("Não existe evento de fechamento do bimestre aberto para realizar o lançamento.");
            }
            return true;
        }

        /// <summary>
        /// Override do Save passando o banco - salva tambem a tabela CLS_CompensacaoAusenciaAluno
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="lista">Lista de CLS_CompensacaoAusenciaAluno a ser salva</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <returns>Se salvou com sucesso</returns>
        public static bool Save(CLS_CompensacaoAusencia entity, List<CLS_CompensacaoAusenciaAluno> lista, bool fechamentoAutomatico, int cal_id)
        {
            TalkDBTransaction banco = new CLS_TurmaAulaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return Save(entity, lista, fechamentoAutomatico, cal_id, banco);
            }
            catch (Exception err)
            {
                banco.Close(err);

                throw err;
            }
            finally
            {
                //Fechamento da transação
                banco.Close();
            }
        }

        public new static bool Delete(CLS_CompensacaoAusencia entity)
        {
            CLS_CompensacaoAusenciaDAO dao = new CLS_CompensacaoAusenciaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                ACA_FormatoAvaliacao formatoAvaliacao = ACA_FormatoAvaliacaoBO.CarregarPorTud(entity.tud_id);
                // Caso o fechamento seja automático, grava na fila de processamento.
                if (formatoAvaliacao.fav_fechamentoAutomatico && entity.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, new Guid()))
                {
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(entity.tud_id, entity.tpc_id, dao._Banco);
                }

                CLS_CompensacaoAusenciaAlunoDAO daoAluno = new CLS_CompensacaoAusenciaAlunoDAO { _Banco = dao._Banco };

                List<CLS_CompensacaoAusenciaAluno> listaBanco = daoAluno.SelectByCpa_id(entity.cpa_id, entity.tud_id);

                foreach (CLS_CompensacaoAusenciaAluno item in listaBanco)
                {
                    if (!daoAluno.Delete(item))
                        throw new Exception("Erro ao tentar excluir compensação de ausência do aluno.");
                }

                // Exclui CLS_CompensacaoAusencia
                if (!dao.Delete(entity))
                    throw new Exception("Erro ao tentar excluir compensação de ausência.");

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);

                throw err;
            }
            finally
            {
                //Fechamento da transação
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Retorna compensação de ausência filtrado por disciplina e protocolo de criação.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="pro_id"></param>
        /// <returns></returns>
        public static CLS_CompensacaoAusencia SelectByDisciplinaProtocolo(long tud_id, Guid pro_id)
        {
            return new CLS_CompensacaoAusenciaDAO().SelectByDisciplinaProtocolo(tud_id, pro_id);
        }

        #endregion Metodos
    }
}