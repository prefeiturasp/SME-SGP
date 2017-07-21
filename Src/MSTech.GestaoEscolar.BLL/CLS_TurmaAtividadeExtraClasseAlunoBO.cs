/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using System.Reflection;
    using ObjetosSincronizacao.Util;
    using System.Data;
    using System.Linq;
    using Data.Common;

    public class TipoTabela_TurmaAtividadeExtraClasseALuno : TipoTabela
    {
        private string dataVazia = new DateTime().ToString();

        [Order]
        public long tud_id { get; set; }
        [Order]
        public int tae_id { get; set; }
        [Order]
        public long alu_id { get; set; }
        [Order]
        public int mtu_id { get; set; }
        [Order]
        public int mtd_id { get; set; }
        [Order]
        [DBNullValue(typeof(string))]
        public string aea_avaliacao { get; set; }
        [Order]
        [DBNullValue(typeof(string))]
        public string aea_relatorio { get; set; }
        [Order]
        public bool aea_entregue { get; set; }
        [Order]
        public byte aea_situacao { get; set; }
        [Order]
        [DBNullValue(typeof(DateTime))]
        public DateTime aea_dataAlteracao { get; set; }

        public TipoTabela_TurmaAtividadeExtraClasseALuno(CLS_TurmaAtividadeExtraClasseAluno entity) : base(entity)
        {

        }
    }

    /// <summary>
    /// Description: CLS_TurmaAtividadeExtraClasseAluno Business Object. 
    /// </summary>
    public class CLS_TurmaAtividadeExtraClasseAlunoBO : BusinessBase<CLS_TurmaAtividadeExtraClasseAlunoDAO, CLS_TurmaAtividadeExtraClasseAluno>
	{
        #region Métodos de inclusão/alteração

        /// <summary>
        /// Salva as notas das atividades extraclasse
        /// </summary>
        /// <param name="lstTurmaAtividadeExtraClasseAluno"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<CLS_TurmaAtividadeExtraClasseAluno> lstTurmaAtividadeExtraClasseAluno, long tud_id, int tpc_id, byte tud_tipo, bool fechamentoAutomatico, Guid ent_id, List<CLS_TurmaAtividadeExtraClasse> lstRelacionamento, long tur_id)
        {
            TalkDBTransaction banco = new CLS_TurmaAtividadeExtraClasseAlunoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                List<long> lstTudId = new List<long>();
                foreach (CLS_TurmaAtividadeExtraClasse atividade in lstRelacionamento)
                {
                    List<CLS_TurmaAtividadeExtraClasse> lstRelacionadas = CLS_TurmaAtividadeExtraClasseBO.SelecionaAtividadeExtraclasseRelacionada(atividade.taer_id);
                    List<CLS_TurmaAtividadeExtraClasseAluno> lstLancamento = lstTurmaAtividadeExtraClasseAluno.FindAll(p => p.tud_id == atividade.tud_id && p.tae_id == atividade.tae_id);
                    List<MTR_MatriculaTurmaDisciplina> lstMatriculaDisciplina = new List<MTR_MatriculaTurmaDisciplina>();

                    lstRelacionadas.FindAll(p => p.tud_id != atividade.tud_id || p.tae_id != atividade.tae_id).ForEach(p =>
                    {
                        if (!lstMatriculaDisciplina.Any(m => m.tud_id == p.tud_id))
                        {
                            lstMatriculaDisciplina.AddRange(MTR_MatriculaTurmaDisciplinaBO.SelecionaMatriculasPorTurmaDisciplina(p.tud_id.ToString(), banco));
                        }
                        lstLancamento.ForEach(a =>
                        {
                            CLS_TurmaAtividadeExtraClasseAluno ent = new CLS_TurmaAtividadeExtraClasseAluno
                            {
                                tud_id = p.tud_id,
                                tae_id = p.tae_id,
                                alu_id = a.alu_id,
                                mtu_id = a.mtu_id,
                                mtd_id = lstMatriculaDisciplina.Find(m => m.alu_id == a.alu_id && m.mtu_id == a.mtu_id && m.tud_id == p.tud_id).mtd_id,
                                aea_avaliacao = a.aea_avaliacao,
                                aea_relatorio = a.aea_relatorio,
                                aea_entregue = a.aea_entregue,
                                aea_dataAlteracao = a.aea_dataAlteracao,
                                aea_situacao = 1
                            };
                            lstTurmaAtividadeExtraClasseAluno.Add(ent);
                        });
                        if (!lstTudId.Any(t => t == p.tud_id))
                        {
                            lstTudId.Add(p.tud_id);
                        }
                    });
                }

                using (DataTable dt = lstTurmaAtividadeExtraClasseAluno.Select(p => new TipoTabela_TurmaAtividadeExtraClasseALuno(p).ToDataRow()).CopyToDataTable())
                {
                    CLS_TurmaAtividadeExtraClasseAlunoDAO dao = new CLS_TurmaAtividadeExtraClasseAlunoDAO();
                    dao._Banco = banco;
                    if (dao.SalvarEmLote(dt))
                    {
                        // Caso o fechamento seja automático, grava na fila de processamento.
                        if (fechamentoAutomatico && tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(tud_id, tpc_id, banco);
                        }

                        if (lstTudId.Any())
                        {
                            List<TUR_TurmaDisciplina> turmaDisciplina = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                            lstTudId.ForEach(p =>
                                {
                                    if (fechamentoAutomatico && turmaDisciplina.Find(t => t.tud_id == p).tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                                    {
                                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(p, tpc_id, banco);
                                    }
                                }
                            );
                        }
                        return true;
                    }

                    return false;
                }
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

        #endregion Métodos de inclusão/alteração
    }
}