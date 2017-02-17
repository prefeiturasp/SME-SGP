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
    [Serializable()]
	public class TUR_TurmaDisciplinaMultisseriada : Abstract_TUR_TurmaDisciplinaMultisseriada
	{
        /// <summary>
        /// ID da disciplina relacionado ao docente..
        /// </summary>
        [MSNotNullOrEmpty("DIsciplina do docente é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tud_idDocente { get; set; }

        /// <summary>
        /// Propriedade alu_id.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// Propriedade mtu_id.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula turma do aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }

        /// <summary>
        /// Propriedade mtd_id.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula turma disciplina do aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtd_id { get; set; }

        // Variáveis utilizadas na matricula nas turmas multisseriadas do docente
        public virtual int cur_id { get; set; }
        public virtual int crr_id { get; set; }
        public virtual int crp_id { get; set; }
        public virtual long tur_id { get; set; }
        public virtual string pes_nome { get; set; }
        public virtual string tur_codigo { get; set; }
        public virtual string alc_matricula { get; set; }
        public virtual int mtd_numeroChamada { get; set; }
        public virtual DateTime mtd_dataMatricula { get; set; }
        public virtual byte mtd_situacao { get; set; }
        public virtual DateTime mtd_dataSaida { get; set; }
        public virtual long tud_id { get; set; }
	}
}