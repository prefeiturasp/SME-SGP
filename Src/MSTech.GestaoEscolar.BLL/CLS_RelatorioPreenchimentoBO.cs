/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using Validation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using CoreSSO.Entities;
    using CoreSSO.BLL;
    using CoreSSO.DAL;
    [Serializable]
    public class RelatorioPreenchimentoAluno
    {
        public CLS_RelatorioPreenchimento entityRelatorioPreenchimento { get; set; }

        public CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entityPreenchimentoAlunoTurmaDisciplina { get; set; }


        public List<CLS_QuestionarioConteudoPreenchimento> lstQuestionarioConteudoPreenchimento { get; set; }

        public List<CLS_QuestionarioRespostaPreenchimento> lstQuestionarioRespostaPreenchimento { get; set; }

        public bool processarPendencia { get; set; }

        public RelatorioPreenchimentoAluno()
        {
            entityRelatorioPreenchimento = new CLS_RelatorioPreenchimento();
            entityPreenchimentoAlunoTurmaDisciplina = new CLS_RelatorioPreenchimentoAlunoTurmaDisciplina();
            lstQuestionarioConteudoPreenchimento = new List<CLS_QuestionarioConteudoPreenchimento>();
            lstQuestionarioRespostaPreenchimento = new List<CLS_QuestionarioRespostaPreenchimento>();
            processarPendencia = false;
        }
    }

    /// <summary>
    /// Description: CLS_RelatorioPreenchimento Business Object. 
    /// </summary>
    public class CLS_RelatorioPreenchimentoBO : BusinessBase<CLS_RelatorioPreenchimentoDAO, CLS_RelatorioPreenchimento>
	{
        /// <summary>
        /// Seleciona dados de preenchimento de relaório do aluno
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public static RelatorioPreenchimentoAluno SelecionaPorRelatorioAlunoTurmaDisciplina(int rea_id, long alu_id, long tur_id, long tud_id, int tpc_id, long reap_id)
        {
            RelatorioPreenchimentoAluno rel = new RelatorioPreenchimentoAluno();

            using (DataSet ds = new CLS_RelatorioPreenchimentoDAO().SelecionaPorRelatorioAlunoTurmaDisciplina(rea_id, alu_id, tur_id, tud_id, tpc_id, reap_id))
            {
                using (DataTable dtRelatorioPreenchimento = ds.Tables[0],
                                 dtRelatorioPreenchimentoAlunoTurmaDisciplina = ds.Tables[1],
                                 dtQuestionarioConteudoPreenchimento = ds.Tables[2],
                                 dtQuestionarioRespostaPreenchimento = ds.Tables[3])
                {
                    if (dtRelatorioPreenchimento.Rows.Count > 0)
                    {
                        rel.entityRelatorioPreenchimento = dtRelatorioPreenchimento.Rows[0].ToEntity<CLS_RelatorioPreenchimento>();
                    }

                    if (dtRelatorioPreenchimentoAlunoTurmaDisciplina.Rows.Count > 0)
                    {
                        rel.entityPreenchimentoAlunoTurmaDisciplina = dtRelatorioPreenchimentoAlunoTurmaDisciplina.Rows[0].ToEntity<CLS_RelatorioPreenchimentoAlunoTurmaDisciplina>();
                    }

                    rel.lstQuestionarioConteudoPreenchimento = dtQuestionarioConteudoPreenchimento.ToEntityList<CLS_QuestionarioConteudoPreenchimento>();

                    rel.lstQuestionarioRespostaPreenchimento = dtQuestionarioRespostaPreenchimento.ToEntityList<CLS_QuestionarioRespostaPreenchimento>();
                }
            }

            return rel;
        }

        public static new bool Save(CLS_RelatorioPreenchimento entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_RelatorioPreenchimentoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity)); 
        }

        public static bool Salvar(RelatorioPreenchimentoAluno relatorio, List<CLS_AlunoDeficienciaDetalhe> lstDeficienciaDetalhe, bool permiteAlterarRacaCor, byte racaCor, List<CLS_RelatorioPreenchimentoAcoesRealizadas> lstAcoesRealizadas)
        {
            CLS_RelatorioPreenchimentoDAO dao = new CLS_RelatorioPreenchimentoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            PES_PessoaDAO daoCore = new PES_PessoaDAO();
            daoCore._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                bool retorno = true;

                if (permiteAlterarRacaCor)
                {
                    ACA_Aluno alu = new ACA_Aluno { alu_id = relatorio.entityPreenchimentoAlunoTurmaDisciplina.alu_id };
                    ACA_AlunoBO.GetEntity(alu);

                    PES_Pessoa pes = new PES_Pessoa { pes_id = alu.pes_id };
                    PES_PessoaBO.GetEntity(pes);

                    pes.pes_racaCor = racaCor;
                    PES_PessoaBO.Save(pes, daoCore._Banco);
                }

                List<CLS_AlunoDeficienciaDetalhe> lstDeficienciaDetalheBanco =
                    (from sAlunoDeficiencia alunoDeficiencia in CLS_AlunoDeficienciaDetalheBO.SelecionaPorAluno(relatorio.entityPreenchimentoAlunoTurmaDisciplina.alu_id)
                     from sAlunoDeficienciaDetalhe alunoDeficienciaDetalhe in alunoDeficiencia.lstDeficienciaDetalhe
                     select new CLS_AlunoDeficienciaDetalhe
                     {
                         alu_id = relatorio.entityPreenchimentoAlunoTurmaDisciplina.alu_id
                         ,
                         tde_id = alunoDeficiencia.tde_id
                         ,
                         dfd_id = alunoDeficienciaDetalhe.dfd_id
                     }).ToList();

                if (lstDeficienciaDetalheBanco.Any())
                {
                    lstDeficienciaDetalheBanco.ForEach(p => CLS_AlunoDeficienciaDetalheBO.Delete(p, dao._Banco));
                }


                if (relatorio.entityRelatorioPreenchimento.reap_id > 0)
                {
                    CLS_QuestionarioConteudoPreenchimentoBO.ExcluiPorReapId(relatorio.entityRelatorioPreenchimento.reap_id, dao._Banco);
                    CLS_QuestionarioRespostaPreenchimentoBO.ExcluiPorReapId(relatorio.entityRelatorioPreenchimento.reap_id, dao._Banco);
                }

                retorno &= Save(relatorio.entityRelatorioPreenchimento, dao._Banco);

                relatorio.entityPreenchimentoAlunoTurmaDisciplina.reap_id = relatorio.entityRelatorioPreenchimento.reap_id;
                retorno &= CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.Save(relatorio.entityPreenchimentoAlunoTurmaDisciplina, dao._Banco);
                
                relatorio.lstQuestionarioConteudoPreenchimento.ForEach
                (
                    p =>
                    {
                        p.reap_id = relatorio.entityRelatorioPreenchimento.reap_id;
                        retorno &= CLS_QuestionarioConteudoPreenchimentoBO.Save(p, dao._Banco);
                    }
                );

                relatorio.lstQuestionarioRespostaPreenchimento.ForEach
                (
                    p =>
                    {
                        p.reap_id = relatorio.entityRelatorioPreenchimento.reap_id;
                        retorno &= CLS_QuestionarioRespostaPreenchimentoBO.Save(p, dao._Banco);
                    }
                );

                lstDeficienciaDetalhe.ForEach
                (
                    p =>
                    {
                        retorno &= CLS_AlunoDeficienciaDetalheBO.Save(p, dao._Banco);
                    }
                );

                lstAcoesRealizadas.ForEach
                (
                    p =>
                    {
                        if (p.rpa_situacao == (byte)CLS_RelatorioPreenchimentoAcoesRealizadasSituacao.Excluido)
                        {
                            retorno &= CLS_RelatorioPreenchimentoAcoesRealizadasBO.Delete(p, dao._Banco);
                        }
                        else
                        {
                            p.reap_id = relatorio.entityRelatorioPreenchimento.reap_id;
                            retorno &= CLS_RelatorioPreenchimentoAcoesRealizadasBO.Save(p, dao._Banco);
                        }
                    }
                );

                CLS_RelatorioAtendimento relatorioAtendimento = CLS_RelatorioAtendimentoBO.GetEntity(new CLS_RelatorioAtendimento { rea_id = relatorio.entityRelatorioPreenchimento.rea_id });
                if (relatorioAtendimento.rea_tipo == (byte)CLS_RelatorioAtendimentoTipo.RP)
                {
                    ACA_CalendarioAnual calendario = ACA_CalendarioAnualBO.SelecionaPorTurma(relatorio.entityPreenchimentoAlunoTurmaDisciplina.tur_id);
                    List<MTR_MatriculaTurma> matriculasAno = MTR_MatriculaTurmaBO.GetSelectMatriculasAlunoAno(relatorio.entityPreenchimentoAlunoTurmaDisciplina.alu_id, calendario.cal_ano);
                    matriculasAno.ForEach(p => CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.LimpaCache_AlunoPreenchimentoPorPeriodoDisciplina(relatorio.entityPreenchimentoAlunoTurmaDisciplina.tpc_id, p.tur_id));

                    if (relatorio.processarPendencia)
                    {
                        if (relatorio.entityPreenchimentoAlunoTurmaDisciplina.tpc_id > 0)
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(relatorio.entityPreenchimentoAlunoTurmaDisciplina.tud_id, relatorio.entityPreenchimentoAlunoTurmaDisciplina.tpc_id, dao._Banco);
                        }
                        else
                        {
                            ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(calendario.cal_id, GestaoEscolarUtilBO.MinutosCacheLongo);
                            List<AlunoFechamentoPendencia> FilaProcessamento = new List<AlunoFechamentoPendencia>();
                            FilaProcessamento.AddRange(ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(calendario.cal_id, GestaoEscolarUtilBO.MinutosCacheLongo)
                                .Select(p => new AlunoFechamentoPendencia
                                {
                                    tud_id = relatorio.entityPreenchimentoAlunoTurmaDisciplina.tud_id,
                                    tpc_id = p.tpc_id,
                                    afp_frequencia = true,
                                    afp_nota = false,
                                    afp_processado = 2
                                }).ToList());
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(FilaProcessamento, dao._Banco);
                        }
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                daoCore._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }

                if (daoCore._Banco.ConnectionIsOpen)
                {
                    daoCore._Banco.Close();
                }
            }
        }
    }
}