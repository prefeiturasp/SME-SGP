/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_ACA_CurriculoCapitulo : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_CurriculoCapitulo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int crc_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoNivelEnsino.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tne_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoDisciplina.
		/// </summary>
		public virtual int tds_id { get; set; }

		/// <summary>
		/// Ano do calendário.
		/// </summary>
		public virtual int cal_ano { get; set; }

		/// <summary>
		/// Título do capítulo.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string crc_titulo { get; set; }

		/// <summary>
		/// Descrição do capítulo.
		/// </summary>
		[MSValidRange(4000)]
		public virtual string crc_descricao { get; set; }

		/// <summary>
		/// Ordem do capítulo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crc_ordem { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte crc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crc_dataAlteracao { get; set; }

    }
}