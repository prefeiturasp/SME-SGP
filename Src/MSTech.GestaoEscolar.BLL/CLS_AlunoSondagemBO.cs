/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using System.Collections.Generic;
    using System.Data;/// <summary>
                      /// Description: CLS_AlunoSondagem Business Object. 
                      /// </summary>
    public class CLS_AlunoSondagemBO : BusinessBase<CLS_AlunoSondagemDAO, CLS_AlunoSondagem>
    {
        /// <summary>
        /// Seleciona os alunos ligados à sondagem/agendamento.
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="sda_id">ID do agendamento</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public static List<CLS_AlunoSondagem> SelectAgendamentosBy_Sondagem(int snd_id, int sda_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoSondagemDAO dao = new CLS_AlunoSondagemDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectAgendamentosBy_Sondagem(snd_id, sda_id);
        }

        /// <summary>
        /// Salva em lote o lançamento de sondagem para os alunos de uma turma.
        /// </summary>
        /// <param name="lstLancamentoTurma"></param>
        /// <param name="snd_id"></param>
        /// <param name="sda_id"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<ACA_Sondagem_Lancamento> lstLancamentoTurma, int snd_id, int sda_id)
        {
            DataTable dtAlunoSondagem = CLS_AlunoSondagem.TipoTabela_AlunoSondagem();
            lstLancamentoTurma.ForEach(p =>
            {
                dtAlunoSondagem.Rows.Add(AlunoSondagemToDataRow(p, dtAlunoSondagem.NewRow()));
            });

            CLS_AlunoSondagemDAO dao = new CLS_AlunoSondagemDAO();
            dao.SalvarEmLote(dtAlunoSondagem, snd_id, sda_id);
            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade ACA_Sondagem_Lancamento.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow AlunoSondagemToDataRow(ACA_Sondagem_Lancamento entity, DataRow dr)
        {
            dr["alu_id"] = entity.alu_id;
            dr["sdq_id"] = entity.sdq_id;
            dr["sdq_idSub"] = entity.sdq_idSub;
            dr["sdr_id"] = entity.sdr_id;
            dr["respAluno"] = entity.respAluno;
            return dr;
        }
    }
}