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
    public abstract class Abstract_ACA_ConfiguracaoServicoPendencia : Abstract_Entity
    {
		
		/// <summary>
		/// Id do tipo de nível de ensino..
		/// </summary>
		[MSNotNullOrEmpty("[tne_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tne_id { get; set; }

		/// <summary>
		/// Id do tipo de modalidade de ensino..
		/// </summary>
		[MSNotNullOrEmpty("[tme_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tme_id { get; set; }

		/// <summary>
		/// Id do tipo de turma..
		/// </summary>
		[MSNotNullOrEmpty("[tur_tipo] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual byte tur_tipo { get; set; }

		/// <summary>
		/// Id do registro..
		/// </summary>
		[MSNotNullOrEmpty("[csp_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int csp_id { get; set; }

		/// <summary>
		/// Sem nota..
		/// </summary>
		[MSNotNullOrEmpty("[csp_semNota] é obrigatório.")]
		public virtual bool csp_semNota { get; set; }

		/// <summary>
		/// Sem parecer..
		/// </summary>
		[MSNotNullOrEmpty("[csp_semParecer] é obrigatório.")]
		public virtual bool csp_semParecer { get; set; }

		/// <summary>
		/// Disciplina sem aula..
		/// </summary>
		[MSNotNullOrEmpty("[csp_disciplinaSemAula] é obrigatório.")]
		public virtual bool csp_disciplinaSemAula { get; set; }

		/// <summary>
		/// Resultado final..
		/// </summary>
		[MSNotNullOrEmpty("[csp_semResultadoFinal] é obrigatório.")]
		public virtual bool csp_semResultadoFinal { get; set; }

		/// <summary>
		/// Sem planejamento..
		/// </summary>
		[MSNotNullOrEmpty("[csp_semPlanejamento] é obrigatório.")]
		public virtual bool csp_semPlanejamento { get; set; }

		/// <summary>
		/// Sem síntese..
		/// </summary>
		[MSNotNullOrEmpty("[csp_semSintese] é obrigatório.")]
		public virtual bool csp_semSintese { get; set; }

		/// <summary>
		/// Situação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[csp_situacao] é obrigatório.")]
		public virtual int csp_situacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[csp_dataAlteracao] é obrigatório.")]
		public virtual DateTime csp_dataAlteracao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[csp_dataCriacao] é obrigatório.")]
		public virtual DateTime csp_dataCriacao { get; set; }

    }
}