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
        public string tur_codigo;
        public long tur_id;
        public int tpc_id;
        public int tpc_ordem;
        public int mtd_id;
        public long tud_id;
        public string Disciplina;
        public string tpc_nome;
        public int numeroFaltas;
        public int ID;
        public byte tud_tipo;
        public int tds_id;
        public int tds_ordem;
        public bool EnriquecimentoCurricular;
        public int numeroAulas;
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

        public static bool Salvar(List<CLS_AlunoFrequenciaExterna> lstAlunoFrequenciaExterna)
        {
            TalkDBTransaction banco = new CLS_AlunoFrequenciaExternaDAO()._Banco.CopyThisInstance();

            try
            {
                return lstAlunoFrequenciaExterna.Aggregate(true, (salvou, freq) => salvou & Save(freq, banco));
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