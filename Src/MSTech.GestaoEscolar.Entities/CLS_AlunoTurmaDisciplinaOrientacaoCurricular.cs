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
	/// Description: .
	/// </summary>
    [Serializable]
	public class CLS_AlunoTurmaDisciplinaOrientacaoCurricular : Abstract_CLS_AlunoTurmaDisciplinaOrientacaoCurricular
	{
        /// <summary>
        /// ID da Turma.
        /// </summary>
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] da turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }

        /// <summary>
        /// ID do Aluno.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// ID da MatriculaTurma.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }

        /// <summary>
        /// ID da MatriculaTurmaDisciplina.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula do(a) [MSG_DISCIPLINA] da turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtd_id { get; set; }

        /// <summary>
        /// Propriedade ocr_id.
        /// </summary>
        [MSNotNullOrEmpty("Orientação curricular é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long ocr_id { get; set; }

        /// <summary>
        /// ID da CLS_AlunoHabilidade.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int aha_id { get; set; }

        /// <summary>
        /// ID do tipo de período do calendário.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de período do calendário é obrigatório.")]
        public override int tpc_id { get; set; }

        /// <summary>
        /// Propriedade aha_alcancada.
        /// </summary>
        [MSDefaultValue(0)]
        public override bool aha_alcancada { get; set; }

        /// <summary>
        /// Propriedade aha_efetivada.
        /// </summary>
        [MSDefaultValue(0)]
        public override bool aha_efetivada { get; set; }

        /// <summary>
        /// Propriedade aha_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte aha_situacao { get; set; }

        /// <summary>
        /// Propriedade aha_dataCriacao.
        /// </summary>
        public override DateTime aha_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade aha_dataAlteracao.
        /// </summary>
        public override DateTime aha_dataAlteracao { get; set; }

        /// <summary>
        /// Variável auxiliar que armazena a ordem do período do calendário.
        /// </summary>
        public int tpc_ordem { get; set; }
	}
}