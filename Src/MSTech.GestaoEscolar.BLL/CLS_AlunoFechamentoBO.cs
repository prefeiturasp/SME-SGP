/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using System;
    using System.Linq;

	/// <summary>
	/// Description: CLS_AlunoFechamento Business Object. 
	/// </summary>
	public class CLS_AlunoFechamentoBO : BusinessBase<CLS_AlunoFechamentoDAO, CLS_AlunoFechamento>
	{
	    public static bool SalvarFechamentoAutomaticoEmLote(List<CLS_AlunoAvaliacaoTurmaDisciplina> ltAlunoAvaliacaoTurmaDisciplina,
                                                            List<CLS_AlunoFechamento> ltAlunoFechamento,
                                                            List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplinaResultado,
                                                            TalkDBTransaction banco = null)
        {
            CLS_AlunoFechamentoDAO dao = new CLS_AlunoFechamentoDAO();

            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }
            else
            {
                dao._Banco = banco;
            }

            try
            {
                using (DataTable dtAlunoAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplina.TipoTabela_AlunoAvaliacaoTurmaDisciplina(),
                                 dtAlunoFechamento = CLS_AlunoFechamento.TipoTabela_AlunoFechamento(),
                                 dtMatriculaTurmaDisciplinaResultado = MTR_MatriculaTurmaDisciplina.TipoTabela_MatriculaTurmaDisciplinaResultado())
                {
                    if (ltAlunoAvaliacaoTurmaDisciplina.Any())
                    {
                        ltAlunoAvaliacaoTurmaDisciplina.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtAlunoAvaliacaoTurmaDisciplina.NewRow();
                                dtAlunoAvaliacaoTurmaDisciplina.Rows.Add(CLS_AlunoAvaliacaoTurmaDisciplinaBO.EntityToDataRow(p, dr));
                            }
                        );
                    }

                    if (ltAlunoFechamento.Any())
                    {
                        ltAlunoFechamento.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtAlunoFechamento.NewRow();
                                dtAlunoFechamento.Rows.Add(EntityToDataRow(p, dr));
                            }
                        );
                    }

                    if (ltMatriculaTurmaDisciplinaResultado.Any())
                    {
                        ltMatriculaTurmaDisciplinaResultado.ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtMatriculaTurmaDisciplinaResultado.NewRow();
                                dr["alu_id"] = p.alu_id;
                                dr["mtu_id"] = p.mtu_id;
                                dr["mtd_id"] = p.mtd_id;
                                dr["mtd_avaliacao"] = DBNull.Value;
                                dr["mtd_relatorio"] = DBNull.Value;
                                dr["mtd_frequencia"] = DBNull.Value;
                                dr["mtd_resultado"] = p.mtd_resultado;
                                dr["apenasResultado"] = p.apenasResultado;
                                dtMatriculaTurmaDisciplinaResultado.Rows.Add(dr);
                            }
                        );
                    }

                    return dao.SalvarFechamentoAutomaticoEmLote(dtAlunoAvaliacaoTurmaDisciplina, dtAlunoFechamento, dtMatriculaTurmaDisciplinaResultado);
                }
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }

        public static DataRow EntityToDataRow(CLS_AlunoFechamento entity, DataRow dr)
        {
            dr["tud_id"] = entity.tud_id;
            dr["tpc_id"] = entity.tpc_id;
            dr["alu_id"] = entity.alu_id;
            dr["mtu_id"] = entity.mtu_id;
            dr["mtd_id"] = entity.mtd_id;

            if (entity.caf_qtFaltas >= 0)
            {
                dr["caf_qtFaltas"] = entity.caf_qtFaltas;
            }
            else
            {
                dr["caf_qtFaltas"] = DBNull.Value;
 
            }

            if (entity.caf_qtAulas > 0)
            {
                dr["caf_qtAulas"] = entity.caf_qtAulas;
            }
            else
            {
                dr["caf_qtAulas"] = DBNull.Value;
            }

            if (entity.caf_qtFaltasReposicao >= 0)
            {
                dr["caf_qtFaltasReposicao"] = entity.caf_qtFaltasReposicao;
            }
            else
            {
                dr["caf_qtFaltasReposicao"] = DBNull.Value;
            }

            if (entity.caf_qtAulasReposicao > 0)
            {
                dr["caf_qtAulasReposicao"] = entity.caf_qtAulas;
            }
            else
            {
                dr["caf_qtAulasReposicao"] = DBNull.Value;
            }

            if (entity.caf_qtAusenciasCompensadas >= 0)
            {
                dr["caf_qtAusenciasCompensadas"] = entity.caf_qtAusenciasCompensadas;
            }
            else
            {
                dr["caf_qtAusenciasCompensadas"] = DBNull.Value;
            }

            if (entity.caf_frequencia >= 0)
            {
                dr["caf_frequencia"] = entity.caf_frequencia;
            }
            else
            {
                dr["caf_frequencia"] = DBNull.Value;
            }

            if (entity.caf_frequenciaFinalAjustada >= 0)
            {
                dr["caf_frequenciaFinalAjustada"] = entity.caf_frequenciaFinalAjustada;
            }
            else
            {
                dr["caf_frequenciaFinalAjustada"] = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(entity.caf_avaliacao))
            {
                dr["caf_avaliacao"] = entity.caf_avaliacao;
            }
            else
            {
                dr["caf_avaliacao"] = DBNull.Value;
            }

            dr["caf_efetivado"] = entity.caf_efetivado;
            dr["caf_dataAlteracao"] = DateTime.Now;

            if (entity.caf_qtFaltasExterna >= 0)
            {
                dr["caf_qtFaltasExterna"] = entity.caf_qtFaltasExterna;
            }
            else
            {
                dr["caf_qtFaltasExterna"] = DBNull.Value;

            }

            if (entity.caf_qtAulasExterna > 0)
            {
                dr["caf_qtAulasExterna"] = entity.caf_qtAulasExterna;
            }
            else
            {
                dr["caf_qtAulasExterna"] = DBNull.Value;
            }

            return dr;
        }
	}
}