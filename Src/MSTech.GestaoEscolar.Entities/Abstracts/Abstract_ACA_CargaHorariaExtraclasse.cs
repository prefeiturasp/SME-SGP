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
    public abstract class Abstract_ACA_CargaHorariaExtraclasse : Abstract_Entity
    {
		
		/// <summary>
		/// ID da disciplina.
		/// </summary>
		[MSNotNullOrEmpty("[dis_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int dis_id { get; set; }

		/// <summary>
		/// ID do calendário anual.
		/// </summary>
		[MSNotNullOrEmpty("[cal_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// ID do tipo de período do calendário.
		/// </summary>
		[MSNotNullOrEmpty("[tpc_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// ID da carga horaria extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[che_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int che_id { get; set; }

		/// <summary>
		/// Carga horária de atividades extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[che_cargaHoraria] é obrigatório.")]
		public virtual decimal che_cargaHoraria { get; set; }

		/// <summary>
		/// Situação do registro ( 1 - Ativo, 3 - Excluído).
		/// </summary>
		[MSNotNullOrEmpty("[che_situacao] é obrigatório.")]
		public virtual byte che_situacao { get; set; }

		/// <summary>
		/// Data de criação do regsitro.
		/// </summary>
		[MSNotNullOrEmpty("[che_dataCriacao] é obrigatório.")]
		public virtual DateTime che_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[che_dataAlteracao] é obrigatório.")]
		public virtual DateTime che_dataAlteracao { get; set; }

    }
}