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
    public abstract class Abstract_ACA_CurriculoObjetivo : Abstract_Entity
    {
		
		/// <summary>
		/// ID da tabela ACA_CurriculoObjetivo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int cro_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoNivelEnsino.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tne_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// ID da tabela ACA_TipoCurriculoPeriodo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tcp_id { get; set; }

		/// <summary>
		/// Ano do calendário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cal_ano { get; set; }

		/// <summary>
		/// Descrição do objetivo.
		/// </summary>
		[MSValidRange(500)]
		[MSNotNullOrEmpty]
		public virtual string cro_descricao { get; set; }

		/// <summary>
		/// Ordem do objetivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cro_ordem { get; set; }

		/// <summary>
		/// Tipo do objetivo (1-Eixo, 2-Tópico, 3-Objetivos de aprendizagem).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte cro_tipo { get; set; }

		/// <summary>
		/// ID do registro pai.
		/// </summary>
		public virtual int cro_idPai { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte cro_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cro_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cro_dataAlteracao { get; set; }

    }
}