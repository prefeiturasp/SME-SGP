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
    public abstract class Abstract_TUR_TurmaDisciplinaTerritorio : Abstract_Entity
    {
		
		/// <summary>
		/// ID da relação entre experiência e território.
		/// </summary>
		[MSNotNullOrEmpty("[tte_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual long tte_id { get; set; }

		/// <summary>
		/// ID da turma disciplina de experiência.
		/// </summary>
		[MSNotNullOrEmpty("[tud_idExperiencia] é obrigatório.")]
		public virtual long tud_idExperiencia { get; set; }

		/// <summary>
		/// ID da turma disciplina do território do saber.
		/// </summary>
		[MSNotNullOrEmpty("[tud_idTerritorio] é obrigatório.")]
		public virtual long tud_idTerritorio { get; set; }

		/// <summary>
		/// Data de vigência inicial da relação entre experiência e território.
		/// </summary>
		[MSNotNullOrEmpty("[tte_vigenciaInicio] é obrigatório.")]
		public virtual DateTime tte_vigenciaInicio { get; set; }

		/// <summary>
		/// Data de vigência final da relação entre experiência e território.
		/// </summary>
		public virtual DateTime tte_vigenciaFim { get; set; }

		/// <summary>
		/// Situacao do registro (1 - Ativo, 3 excluído).
		/// </summary>
		[MSNotNullOrEmpty("[tte_situacao] é obrigatório.")]
		public virtual byte tte_situacao { get; set; }

		/// <summary>
		/// Propriedade tte_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty("[tte_dataCriacao] é obrigatório.")]
		public virtual DateTime tte_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade tte_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty("[tte_dataAlteracao] é obrigatório.")]
		public virtual DateTime tte_dataAlteracao { get; set; }

    }
}