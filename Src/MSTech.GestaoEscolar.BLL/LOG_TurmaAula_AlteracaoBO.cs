/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using MSTech.Data.Common;

    #region ENUMERADORES

    public enum LOG_TurmaAula_Alteracao_Tipo : byte
    {
        AlteracaoAula = 1,
        AlteracaoPlanoAula = 2,
        AlteracaoFreq = 3,
        AnotacaoAluno = 4,
        ExclusaoAula = 5
    }

    public enum LOG_TurmaAula_Alteracao_Origem : byte
    {
        WebDiarioClasse = 1,
        WebListao = 2,
        Sincronizacao = 3,
        WebAgenda = 4
    }

    #endregion

	/// <summary>
	/// Description: LOG_TurmaAula_Alteracao Business Object. 
	/// </summary>
	public class LOG_TurmaAula_AlteracaoBO : BusinessBase<LOG_TurmaAula_AlteracaoDAO, LOG_TurmaAula_Alteracao>
    {/// <summary>
        /// Salva a lista de log de aula em lote
        /// </summary>
        /// <param name="listLogAula">Lista de log de aulas</param>
        /// <param name="banco">Transação do banco</param>
        public static bool SalvarEmLote(List<LOG_TurmaAula_Alteracao> listLogAula, TalkDBTransaction banco)
        {
            DataTable dtLOGTurmaAulaAlteracao = LOG_TurmaAula_Alteracao.TipoTabela_LOG_TurmaAula_Alteracao();
            if (listLogAula != null && listLogAula.Any())
            {
                object lockObject = new object();

                Parallel.ForEach
                (
                    listLogAula,
                    logAula =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtLOGTurmaAulaAlteracao.NewRow();
                            dtLOGTurmaAulaAlteracao.Rows.Add(LOGTurmaAulaAlteracaoToDataRow(logAula, dr));
                        }
                    }
                );

                bool retorno = banco == null ?
                       new LOG_TurmaAula_AlteracaoDAO().SalvarEmLote(dtLOGTurmaAulaAlteracao) :
                       new LOG_TurmaAula_AlteracaoDAO { _Banco = banco }.SalvarEmLote(dtLOGTurmaAulaAlteracao);

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
        public static DataRow LOGTurmaAulaAlteracaoToDataRow(LOG_TurmaAula_Alteracao logAula, DataRow dr)
        {
            dr["idAula"] = logAula.idAula;
            dr["tud_id"] = logAula.tud_id;
            dr["tau_id"] = logAula.tau_id;
            dr["lta_tipo"] = logAula.lta_tipo;
            dr["lta_origem"] = logAula.lta_origem;
            dr["usu_id"] = logAula.usu_id;
            dr["lta_data"] = logAula.lta_data;

            return dr;
        }	
	}
}