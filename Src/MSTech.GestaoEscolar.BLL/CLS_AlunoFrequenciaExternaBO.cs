/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Common;
    using Validation.Exceptions;
    #region Estrutura

    [Serializable]
    public struct DadosLancamentoFreqExterna
    {
        public long alu_id;
        public int mtu_id;
        public int cur_id;
        public int crr_id;
        public int crp_id;
        public string tur_codigo;
        public long tur_id;
        public int tpc_id;
        public int tpc_ordem;
        public int mtd_id;
        public long tud_id;
        public int mtd_idRegencia;
        public long tud_idRegencia;
        public string Disciplina;
        public string tpc_nome;
        public int numeroFaltas;
        public int ID;
        public byte tud_tipo;
        public int tds_id;
        public int tds_ordem;
        public bool EnriquecimentoCurricular;
        public int numeroAulas;
        public int numeroAulasPrevistas;
        public bool possuiLancamentoAulasPrevistas;
    }
	
    #endregion Estrutura

	/// <summary>
	/// Description: CLS_AlunoFrequenciaExterna Business Object. 
	/// </summary>
	public class CLS_AlunoFrequenciaExternaBO : BusinessBase<CLS_AlunoFrequenciaExternaDAO, CLS_AlunoFrequenciaExterna>
	{
        #region Métodos de consulta
        /// <summary>
        /// Retorna os lançamentos de frequência externa para os alunos da tabela de mts, no tpc informado
        /// </summary>
        /// <param name="listaMtds"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public static List<CLS_AlunoFrequenciaExterna> SelecionaPor_MatriculasDisciplinaPeriodo(List<MTR_MatriculaTurmaDisciplina> listaMtds, int tpc_id)
        {
            //CLS_AlunoFrequenciaExternaDAO dao = new CLS_AlunoFrequenciaExternaDAO();
            DataTable dt = MTR_MatriculaTurmaDisciplina.TipoTabela_AlunoMatriculaTurmaDisciplina();
            foreach (MTR_MatriculaTurmaDisciplina item in listaMtds)
            {
                DataRow dr = GestaoEscolarUtilBO.EntityToDataRow(dt, item);
                dt.Rows.Add(dr);
            }

            DataTable dtRet = new CLS_AlunoFrequenciaExternaDAO().SelecionaPor_MatriculasDisciplinaPeriodo(dt, tpc_id);
				
            return GestaoEscolarUtilBO.DataTableToListEntity<CLS_AlunoFrequenciaExterna>(dtRet);
        }
        /// <summary>
        /// Seleciona os dados para lançamento de frequencia externa do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno</param>
        /// <returns></returns>
        public static List<DadosLancamentoFreqExterna> SelecionaDadosAlunoLancamentoFrequenciaExterna(long alu_id, int mtu_id)
        {
            return new CLS_AlunoFrequenciaExternaDAO().SelecionaDadosAlunoLancamentoFrequenciaExterna(alu_id, mtu_id)
                                                      .Select()
                                                      .Select(p => (DadosLancamentoFreqExterna)GestaoEscolarUtilBO.DataRowToEntity(p, new DadosLancamentoFreqExterna()))
                                                      .ToList();
        }

        #endregion Métodos de consulta

        #region Métodos de salvar

        public static bool Salvar(List<CLS_AlunoFrequenciaExterna> lstAlunoFrequenciaExterna, int fav_id, Guid ent_id)
        {
            TalkDBTransaction banco = new CLS_AlunoFrequenciaExternaDAO()._Banco.CopyThisInstance();
            List<AlunoFechamentoPendencia> FilaProcessamento = new List<AlunoFechamentoPendencia>();

            try
            {
                if (lstAlunoFrequenciaExterna.Aggregate(true, (salvou, freq) => salvou & Save(freq, banco)))
                {
                    ACA_FormatoAvaliacao entFormatoAvaliacao = new ACA_FormatoAvaliacao { fav_id = fav_id };
                    ACA_FormatoAvaliacaoBO.GetEntity(entFormatoAvaliacao, banco);

                    if (entFormatoAvaliacao.fav_fechamentoAutomatico)
                    {
                        FilaProcessamento.AddRange
                        (lstAlunoFrequenciaExterna.GroupBy(p => new { p.tud_id, p.tpc_id })
                                                  .Select(p => new AlunoFechamentoPendencia
                                                  {
                                                      tud_id = p.Key.tud_id
                                                      ,
                                                      tpc_id = p.Key.tpc_id
                                                      ,
                                                      afp_frequencia = false
                                                      ,
                                                      afp_nota = false
                                                      ,
                                                      afp_frequenciaExterna = true
                                                      ,
                                                      afp_processado = 0
                                                  })
                        );

                        if (FilaProcessamento.Any())
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(
                                FilaProcessamento
                                .GroupBy(g => new { g.tud_id, g.tpc_id })
                                .Select(p => new AlunoFechamentoPendencia
                                {
                                    tud_id = p.FirstOrDefault().tud_id,
                                    tpc_id = p.FirstOrDefault().tpc_id,
                                    afp_frequencia = p.FirstOrDefault().afp_frequencia,
                                    afp_nota = p.FirstOrDefault().afp_nota,
                                    afp_frequenciaExterna = p.FirstOrDefault().afp_frequenciaExterna,
                                    afp_processado = FilaProcessamento
                                        .Where(w => w.tud_id == p.FirstOrDefault().tud_id && w.tpc_id == p.FirstOrDefault().tpc_id)
                                        .Min(m => m.afp_processado)
                                })
                                .Where(w => w.tpc_id > 0 && w.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                                .ToList()
                            , banco);
                        }
                    }
                    
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        public static new bool Save(CLS_AlunoFrequenciaExterna entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoFrequenciaExternaDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion Métodos de salvar
	}
}