/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    #region ENUMERADORES

    public enum LOG_TurmaNota_Alteracao_Tipo : byte
    {
        AlteracaoAtividade = 1,
        LancamentoNotas = 2,
        ExclusaoAtividade = 3
    }

    public enum LOG_TurmaNota_Alteracao_Origem : byte
    {
        WebDiarioClasse = 1,
        WebListao = 2,
        Sincronizacao = 3
    }

    #endregion

	/// <summary>
	/// Description: LOG_TurmaNota_Alteracao Business Object. 
	/// </summary>
	public class LOG_TurmaNota_AlteracaoBO : BusinessBase<LOG_TurmaNota_AlteracaoDAO, LOG_TurmaNota_Alteracao>
	{
        /// <summary>
        /// Salva a lista de log de nota em lote
        /// </summary>
        /// <param name="listLogNota">Lista de log de notas</param>
        /// <param name="banco">Transação do banco</param>
        public static bool SalvarEmLote(List<LOG_TurmaNota_Alteracao> listLogNota, TalkDBTransaction banco)
        {
            DataTable dtLOGTurmaNotaAlteracao = LOG_TurmaNota_Alteracao.TipoTabela_LOG_TurmaNota_Alteracao();
            if (listLogNota != null && listLogNota.Any())
            {
                object lockObject = new object();

                Parallel.ForEach
                (
                    listLogNota,
                    logNota =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtLOGTurmaNotaAlteracao.NewRow();
                            dtLOGTurmaNotaAlteracao.Rows.Add(LOGTurmaNotaAlteracaoToDataRow(logNota, dr));
                        }
                    }
                );

                bool retorno = banco == null ?
                       new LOG_TurmaNota_AlteracaoDAO().SalvarEmLote(dtLOGTurmaNotaAlteracao) :
                       new LOG_TurmaNota_AlteracaoDAO { _Banco = banco }.SalvarEmLote(dtLOGTurmaNotaAlteracao);

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
        public static DataRow LOGTurmaNotaAlteracaoToDataRow(LOG_TurmaNota_Alteracao logNota, DataRow dr)
        {
            dr["idAtividade"] = logNota.idAtividade;
            dr["tud_id"] = logNota.tud_id;
            dr["tnt_id"] = logNota.tnt_id;
            dr["ltn_tipo"] = logNota.ltn_tipo;
            dr["ltn_origem"] = logNota.ltn_origem;
            dr["usu_id"] = logNota.usu_id;
            dr["ltn_data"] = logNota.ltn_data;

            return dr;
        }
    }
}