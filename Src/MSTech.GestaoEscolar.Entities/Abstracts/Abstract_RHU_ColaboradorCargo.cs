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
    public abstract class Abstract_RHU_ColaboradorCargo : Abstract_Entity
    {
		
		/// <summary>
		/// Propriedade col_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long col_id { get; set; }

		/// <summary>
		/// Propriedade crg_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int crg_id { get; set; }

		/// <summary>
		/// Propriedade coc_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int coc_id { get; set; }

		/// <summary>
		/// Propriedade coc_matricula.
		/// </summary>
		[MSValidRange(30)]
		public virtual string coc_matricula { get; set; }

		/// <summary>
		/// Propriedade coc_observacao.
		/// </summary>
		[MSValidRange(1000)]
		public virtual string coc_observacao { get; set; }

		/// <summary>
		/// Propriedade coc_vigenciaInicio.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime coc_vigenciaInicio { get; set; }

		/// <summary>
		/// Propriedade coc_vigenciaFim.
		/// </summary>
		public virtual DateTime coc_vigenciaFim { get; set; }

		/// <summary>
		/// Propriedade ent_id.
		/// </summary>
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Propriedade uad_id.
		/// </summary>
		public virtual Guid uad_id { get; set; }

		/// <summary>
		/// Propriedade chr_id.
		/// </summary>
		public virtual int chr_id { get; set; }

		/// <summary>
		/// Propriedade coc_vinculoSede.
		/// </summary>
		public virtual bool coc_vinculoSede { get; set; }

		/// <summary>
		/// Propriedade coc_vinculoExtra.
		/// </summary>
		public virtual bool coc_vinculoExtra { get; set; }

		/// <summary>
		/// Propriedade coc_situacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte coc_situacao { get; set; }

		/// <summary>
		/// Propriedade coc_dataCriacao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime coc_dataCriacao { get; set; }

		/// <summary>
		/// Propriedade coc_dataAlteracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime coc_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade coc_complementacaoCargaHoraria.
		/// </summary>
		public virtual bool coc_complementacaoCargaHoraria { get; set; }

		/// <summary>
		/// Propriedade coc_dataInicioMatricula.
		/// </summary>
		public virtual DateTime coc_dataInicioMatricula { get; set; }

		/// <summary>
		/// Propriedade coc_readaptado.
		/// </summary>
		public virtual bool coc_readaptado { get; set; }

		/// <summary>
		/// Propriedade coc_controladoIntegracao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool coc_controladoIntegracao { get; set; }

    }
}