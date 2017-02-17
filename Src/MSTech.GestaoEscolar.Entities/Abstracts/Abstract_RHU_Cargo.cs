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
    public abstract class Abstract_RHU_Cargo : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela RHU_Cargo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int crg_id { get; set; }

		/// <summary>
		/// Codigo do cargo..
		/// </summary>
		[MSValidRange(20)]
		public virtual string crg_codigo { get; set; }

		/// <summary>
		/// Nome do cargo.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string crg_nome { get; set; }

		/// <summary>
		/// Descrição do cargo.
		/// </summary>
		public virtual string crg_descricao { get; set; }

		/// <summary>
		/// Campo Id da tabela RHU_TipoVinculo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tvi_id { get; set; }

		/// <summary>
		/// Cargo de docente..
		/// </summary>
		public virtual bool crg_cargoDocente { get; set; }

		/// <summary>
		/// Maximo de aulas por semana..
		/// </summary>
		public virtual byte crg_maxAulaSemana { get; set; }

		/// <summary>
		/// Maximo de aulas por dia..
		/// </summary>
		public virtual byte crg_maxAulaDia { get; set; }

		/// <summary>
		/// Código de integração do cargo..
		/// </summary>
		[MSValidRange(20)]
		public virtual string crg_codIntegracao { get; set; }

		/// <summary>
		/// Cargo de especialista..
		/// </summary>
		public virtual bool crg_especialista { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte crg_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crg_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime crg_dataAlteracao { get; set; }

		/// <summary>
		/// Campo Id da tabela SYS_Entidde do CoreSSO.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Chave.
		/// </summary>
		[MSValidRange(100)]
		public virtual string pgs_chave { get; set; }

        /// <summary>
        /// Tipo do cargo: 1-Comum, 2-Cargo base, 3-Atribuição esporádica, 4-Indireto.
        /// </summary>
        [MSNotNullOrEmpty]
		public virtual byte crg_tipo { get; set; }

        /// <summary>
		/// Indica se o cargo é controlado pela integração
		/// </summary>
        public virtual bool crg_controleIntegracao { get; set; }
    }
}