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
		public virtual int gra_id { get; set; }

		/// <summary>
		/// Enumerador, 1- Periodo de preenchimento, 2-Detalhamento de deficiencia, 3- Raca/Cor, 4- Faixa de idade, 5- Sexo.
		/// </summary>
		[MSNotNullOrEmpty("[gff_tipoFiltro] é obrigatório.")]
		public virtual byte gff_tipoFiltro { get; set; }

		/// <summary>
		/// Id do filtro fixo..
		/// </summary>
		[MSNotNullOrEmpty("[gff_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int gff_id { get; set; }

		/// <summary>
		/// Valor do filtro, separado por virgula caso exista mais de um valor.
		/// </summary>
		[MSNotNullOrEmpty("[gff_valorFiltro] é obrigatório.")]
		public virtual string gff_valorFiltro { get; set; }

		/// <summary>
		/// Situação do registro (1 - Ativo; 3 - Excluído)..
		/// </summary>
		[MSNotNullOrEmpty("[gff_situacao] é obrigatório.")]
		public virtual int gff_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty("[gff_dataCriacao] é obrigatório.")]
		public virtual DateTime gff_dataCriacao { get; set; }

		/// <summary>
		/// Data da última alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty("[gff_dataAlteracao] é obrigatório.")]
		public virtual DateTime gff_dataAlteracao { get; set; }

    }
}