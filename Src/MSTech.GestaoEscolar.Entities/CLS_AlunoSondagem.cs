/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.Data;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_AlunoSondagem : Abstract_CLS_AlunoSondagem
    {
        /// <summary>
        /// ID da tabela ACA_Sondagem.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_Sondagem é obrigatório.")]
        public override int snd_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_SondagemAgendamento.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_SondagemAgendamento é obrigatório.")]
        public override int sda_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_Aluno.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_Aluno é obrigatório.")]
        public override long alu_id { get; set; }
        
        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluido).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte asn_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime asn_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime asn_dataAlteracao { get; set; }

        /// <summary>
        /// Variável auxiliar para a situação do agendamento
        /// </summary>
        public byte sda_situacao { get; set; }

        public static DataTable TipoTabela_AlunoSondagem()
        {
            DataTable dtAlunoSondagem = new DataTable();
            dtAlunoSondagem.Columns.Add("alu_id", typeof(long));
            dtAlunoSondagem.Columns.Add("sdq_id", typeof(int));
            dtAlunoSondagem.Columns.Add("sdq_idSub", typeof(int));
            dtAlunoSondagem.Columns.Add("sdr_id", typeof(int));
            dtAlunoSondagem.Columns.Add("respAluno", typeof(bool));
            return dtAlunoSondagem;
        }
    }
}