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
    public abstract class Abstract_REL_GraficoAtendimento_FiltrosFixos : Abstract_Entity
    {
		
		/// <summary>
		/// ID do grafico.
		/// </summary>
		[MSNotNullOrEmpty("[gra_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int gra_id { get; set; }

		/// <summary>
		/// Enumerador, 1- Periodo de preenchimento, 2-Detalhamento de deficiencia, 3- Raca/Cor, 4- Faixa de idade, 5- Sexo.
		/// </summary>
		[MSNotNullOrEmpty("[gff_tipoFiltro] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual byte gff_tipoFiltro { get; set; }

		/// <summary>
		/// Valor do filtro, separado por virgula caso exista mais de um valor.
		/// </summary>
		[MSNotNullOrEmpty("[gff_valorFiltro] é obrigatório.")]
		public virtual string gff_valorFiltro { get; set; }

    }
}