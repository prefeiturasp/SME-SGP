/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MTR_Matricula : AbstractMTR_Matricula
    {
        /// <summary>
        /// Id do processo de fechamento/início do ano letivo
        /// </summary>
        [MSNotNullOrEmpty("Processo fechamento/início ano letivo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int pfi_id { get; set; }

        /// <summary>
        /// Id do aluno
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// Id da renovação (pré-matrícula)
        /// </summary>        
        [DataObjectField(true, false, false)]
        public override int mtr_id { get; set; }

        /// <summary>
        /// Número de matrícula do aluno
        /// </summary>
        [MSValidRange(50, "Número de matrícula do aluno pode conter até 50 caracteres.")]
        public override string mtr_numeroMatricula { get; set; }

        /// <summary>
        /// Tipo de processo (1-Renovação,2-Importação do sistema Matrícula Digital,3-Matrícula de alunos oriundos de creches conveniadas,4-Matrícula de alunos oriundos de creches conveniadas - pelo sistema Inscrição Creche)
        /// </summary>        
        public override short mtr_tipoProcesso { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído, 4- Matriculado, 5-Inativo)
        /// </summary>
        [MSDefaultValue(1)]
        public override short mtr_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>        
        public override DateTime mtr_dataCriacao { get; set; }

        /// <summary>
        /// Data da última alteração do registro
        /// </summary>        
        public override DateTime mtr_dataAlteracao { get; set; }

        // Variáveis utilizadas na formação de turmas para o início do ano letivo
        public virtual Int64 numeroLinha { get; set; }
        public virtual string cur_nome { get; set; }
        public virtual string crp_descricao { get; set; }
        public virtual string pes_nome { get; set; }
        public virtual DateTime pes_dataNascimento { get; set; }
        public DateTime mov_dataRealizacao { get; set; }

        // Variável utilizada na renovação para indicar se a renovação será realizada pela progressão do PEJA
        public bool alunoPEJA { get; set; }

        //Variável auxiliar que informa a situação da turma
        public byte tur_situacao { get; set; }

        //Variável auxiliar que informa o codigo da turma
        public string tur_codigo { get; set; }
    }
}