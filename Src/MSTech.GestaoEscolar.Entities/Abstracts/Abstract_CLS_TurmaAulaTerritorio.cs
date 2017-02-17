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
    public abstract class Abstract_CLS_TurmaAulaTerritorio : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade tud_idExperiencia.
		/// </summary>
		[MSNotNullOrEmpty("[tud_idExperiencia] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_idExperiencia { get; set; }

		/// <summary>
		/// Propriedade tau_idExperiencia.
		/// </summary>
		[MSNotNullOrEmpty("[tau_idExperiencia] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tau_idExperiencia { get; set; }

		/// <summary>
		/// Propriedade tud_idTerritorio.
		/// </summary>
		[MSNotNullOrEmpty("[tud_idTerritorio] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_idTerritorio { get; set; }

		/// <summary>
		/// Propriedade tau_idTerritorio.
		/// </summary>
		[MSNotNullOrEmpty("[tau_idTerritorio] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tau_idTerritorio { get; set; }

    }
}