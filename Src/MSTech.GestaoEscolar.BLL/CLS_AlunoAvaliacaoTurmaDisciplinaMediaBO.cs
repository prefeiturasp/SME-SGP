/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Data.Common;
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using MSTech.GestaoEscolar.BLL.Caching;

    /// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaDisciplinaMedia Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDisciplinaMediaBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO, CLS_AlunoAvaliacaoTurmaDisciplinaMedia>
	{
        /// <summary>
        /// Busca as médias da turma salvas pra disicplina e período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do período</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> BuscaNotasFinaisTurma(long tud_id, int tpc_id, TalkDBTransaction banco)
		{
            return (new CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO { _Banco = banco }
                    .BuscaNotasFinaisTurma(tud_id, tpc_id)).Rows.Cast<DataRow>()
                    .Select(p => (CLS_AlunoAvaliacaoTurmaDisciplinaMedia)GestaoEscolarUtilBO.DataRowToEntity(p, new CLS_AlunoAvaliacaoTurmaDisciplinaMedia())).ToList();
		}

        /// <summary>
        /// Busca as médias da turma salvas pra disicplina e período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do período</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> BuscaNotasFinaisTud(long tud_id, int tpc_id, TalkDBTransaction banco)
        {
            return (new CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO { _Banco = banco }
                    .BuscaNotasFinaisTud(tud_id, tpc_id)).Rows.Cast<DataRow>()
                    .Select(p => (CLS_AlunoAvaliacaoTurmaDisciplinaMedia)GestaoEscolarUtilBO.DataRowToEntity(p, new CLS_AlunoAvaliacaoTurmaDisciplinaMedia())).ToList();
        }

        /// <summary>
        /// Salva os dados da média dos alunos.
        /// </summary>
        /// <param name="ltAlunoAvaliacaoTurmaDisciplinaMedia">Lista de médias dos alunos.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(long tur_id, long tud_id, int tpc_id, int fav_id, List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> ltAlunoAvaliacaoTurmaDisciplinaMedia, TalkDBTransaction banco)
        {
            DataTable dtAlunoAvaliacaoTurmaDisciplinaMedia = CLS_AlunoAvaliacaoTurmaDisciplinaMedia.TipoTabela_AlunoAvaliacaoTurmaDisciplinaMedia();
            if (ltAlunoAvaliacaoTurmaDisciplinaMedia != null && ltAlunoAvaliacaoTurmaDisciplinaMedia.Any())
            {
                object lockObject = new object();

                Parallel.ForEach
                (
                    ltAlunoAvaliacaoTurmaDisciplinaMedia,
                    alunoAvaliacaoTurmaDisciplinaMedia =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtAlunoAvaliacaoTurmaDisciplinaMedia.NewRow();
                            dtAlunoAvaliacaoTurmaDisciplinaMedia.Rows.Add(AlunoAvaliacaoTurmaDisciplinaMediaToDataRow(alunoAvaliacaoTurmaDisciplinaMedia, dr));
                        }
                    }
                );

                bool retorno = banco == null ?
                       new CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO().SalvarEmLote(dtAlunoAvaliacaoTurmaDisciplinaMedia) :
                       new CLS_AlunoAvaliacaoTurmaDisciplinaMediaDAO { _Banco = banco }.SalvarEmLote(dtAlunoAvaliacaoTurmaDisciplinaMedia);

                if (retorno && HttpContext.Current != null)
                {
                    // Limpa o cache do fechamento
                    try
                    {
                        string chave = string.Empty;
                        List<ACA_Avaliacao> avaliacao = ACA_AvaliacaoBO.GetSelectBy_FormatoAvaliacaoPeriodo(fav_id, tpc_id);

                        if (avaliacao.Any())
                        {
                            int ava_id = avaliacao.First().ava_id;
                            if (tud_id > 0)
                            {
                                chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, fav_id, ava_id, string.Empty);
                                CacheManager.Factory.RemoveByPattern(chave);

                                chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, fav_id, ava_id, string.Empty);
                                CacheManager.Factory.RemoveByPattern(chave);

                                chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato(tur_id, fav_id, ava_id);
                                CacheManager.Factory.Remove(chave);
                            }
                            else
                            {
                                chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(tur_id, fav_id, ava_id);
                                HttpContext.Current.Cache.Remove(chave);
                            }

                            // Recupero o id da avaliacao final para limpar o cache
                            DataTable avaFinal = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(AvaliacaoTipo.Final, fav_id);
                            ava_id = avaFinal.Rows.Count == 0 ? -1 : Convert.ToInt32(avaFinal.Rows[0]["ava_id"]);

                            // Limpa o cache da efetivacao final
                            if (ava_id > 0)
                            {
                                if (tud_id > 0)
                                {
                                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinal(tud_id, fav_id, ava_id, string.Empty);
                                    CacheManager.Factory.RemoveByPattern(chave);

                                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaFinalFiltroDeficiencia(tud_id, fav_id, ava_id, string.Empty);
                                    CacheManager.Factory.RemoveByPattern(chave);

                                    chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelect_ComponentesRegencia_By_TurmaFormato_Final(tur_id, fav_id, ava_id);
                                    CacheManager.Factory.Remove(chave);
                                }
                                else
                                {
                                    chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Final(tur_id, fav_id, ava_id);
                                    HttpContext.Current.Cache.Remove(chave);
                                }
                            }
                        }
                    }
                    catch
                    { }
                }

                return retorno;
            }

            return true;
        }

        /// <summary>
        /// O método que converte o registro da CLS_AlunoAvaliacaoTurmaDisciplinaMedia em um DataRow.
        /// </summary>
        /// <param name="alunoAvaliacaoTurmaDisciplinaMedia">Registro da CLS_AlunoAvaliacaoTurmaDisciplinaMedia.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        private static DataRow AlunoAvaliacaoTurmaDisciplinaMediaToDataRow(CLS_AlunoAvaliacaoTurmaDisciplinaMedia alunoAvaliacaoTurmaDisciplinaMedia, DataRow dr)
        {
            dr["tud_id"] = alunoAvaliacaoTurmaDisciplinaMedia.tud_id;
            dr["alu_id"] = alunoAvaliacaoTurmaDisciplinaMedia.alu_id;
            dr["mtu_id"] = alunoAvaliacaoTurmaDisciplinaMedia.mtu_id;
            dr["mtd_id"] = alunoAvaliacaoTurmaDisciplinaMedia.mtd_id;
            dr["tpc_id"] = alunoAvaliacaoTurmaDisciplinaMedia.tpc_id;

            if (!string.IsNullOrEmpty(alunoAvaliacaoTurmaDisciplinaMedia.atm_media))
                dr["atm_media"] = alunoAvaliacaoTurmaDisciplinaMedia.atm_media;
            else
                dr["atm_media"] = DBNull.Value;

            dr["atm_situacao"] = alunoAvaliacaoTurmaDisciplinaMedia.atm_situacao;

            return dr;
        }

	}
}