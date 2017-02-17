/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class AbstractTUR_TurmaDisciplina : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Propriedade tud_codigo.
		/// </summary>
		[MSValidRange(30)]
		[MSNotNullOrEmpty]
		public virtual string tud_codigo { get; set; }

		/// <summary>
		/// Propriedade tud_nome.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string tud_nome { get; set; }

		/// <summary>
		/// Propriedade tud_multiseriado.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool tud_multiseriado { get; set; }

		/// <summary>
		/// Propriedade tud_vagas.
		/// </summary>
		public virtual int tud_vagas { get; set; }

		/// <summary>
		/// Propriedade tud_minimoMatriculados.
		/// </summary>
		public virtual int tud_minimoMatriculados { get; set; }

		/// <summary>
		/// 1 - Anual, 2 - Semestral.
		/// </summary>
		public virtual byte tud_duracao { get; set; }

		/// <summary>
		/// 1 - Normal, 2 - Regime Especial, 3 - Excepcional.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tud_modo { get; set; }

		/// <summary>
		/// 1–Obrigatória, 
        /// 3–Optativa, 
        /// 4–Eletiva, 
        /// 5–Disciplina principal, 
        /// 6–Docente da turma e docente específico – obrigatória, 
        /// 7–Docente da turma e docente específico – eletiva, 
        /// 8–Depende da disponibilidade de professor – obrigatória, 
        /// 9–Depende da disponibilidade de professor – eletiva,
        /// 10–DisciplinaEletivaAluno,
        /// 11–Regência,
        /// 12–Componente regência,
        /// 13-Docente específico – complementação da regência.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tud_tipo { get; set; }

		/// <summary>
		/// Propriedade tud_dataInicio.
		/// </summary>
		public virtual DateTime tud_dataInicio { get; set; }

		/// <summary>
		/// Propriedade tud_dataFim.
		/// </summary>
		public virtual DateTime tud_dataFim { get; set; }

		/// <summary>
		/// Propriedade tud_cargaHorariaSemanal.
		/// </summary>
		public virtual int tud_cargaHorariaSemanal { get; set; }

		/// <summary>
		/// Propriedade tud_aulaForaPeriodoNormal.
		/// </summary>
		public virtual bool tud_aulaForaPeriodoNormal { get; set; }

		/// <summary>
		/// Indica se a TurmaDisciplina é global.
		/// </summary>
		public virtual bool tud_global { get; set; }

		/// <summary>
		/// 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tud_situacao { get; set; }

		/// <summary>
		/// Propriedade tud_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tud_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tud_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tud_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade tud_disciplinaEspecial.
		/// </summary>
		public virtual bool tud_disciplinaEspecial { get; set; }

        /// <summary>
        /// Propriedade tud_naoLancarNota.
        /// </summary>
        public virtual bool tud_naoLancarNota { get; set; }

        /// <summary>
        /// Propriedade tud_naoLancarFrequencia.
        /// </summary>
        public virtual bool tud_naoLancarFrequencia { get; set; }

        /// <summary>
        /// Propriedade tud_naoExibirNota.
        /// </summary>
        public virtual bool tud_naoExibirNota { get; set; }

        /// <summary>
        /// Propriedade tud_naoExibirFrequencia.
        /// </summary>
        public virtual bool tud_naoExibirFrequencia { get; set; }

        /// <summary>
        /// Propriedade tud_semProfessor.
        /// </summary>
        public virtual bool tud_semProfessor { get; set; }

        /// <summary>
        /// Propriedade tud_naoExibirBoletim.
        /// </summary>
        public virtual bool tud_naoExibirBoletim { get; set; }

        /// <summary>
        /// Propriedade tud_naoLancarPlanejamento.
        /// </summary>
        public virtual bool tud_naoLancarPlanejamento { get; set; }

        /// <summary>
        /// ID da experiência
        /// </summary>
        public virtual int ter_id { get; set; }

        /// <summary>
        /// Propriedade tud_permitirLancarAbonoFalta.
        /// </summary>
        public virtual bool tud_permitirLancarAbonoFalta { get; set; }

        /// <summary>
		/// Propriedade tud_duplaRegencia.
		/// </summary>
		[MSNotNullOrEmpty]
        [MSDefaultValue(false)]
        public virtual bool tud_duplaRegencia { get; set; }
    }
}