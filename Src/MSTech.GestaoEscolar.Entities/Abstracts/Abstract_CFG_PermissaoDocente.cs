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
    public abstract class Abstract_CFG_PermissaoDocente : Abstract_Entity
    {
		
		/// <summary>
		/// ID Permissão do docente.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int pdc_id { get; set; }

		/// <summary>
		/// ID iipo do docente.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual byte tdc_id { get; set; }

		/// <summary>
		/// ID tipo do docente permissao.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
        public virtual byte tdc_idPermissao { get; set; }

		/// <summary>
		/// id do modulo de permissao (EnumModuloPermissao).
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pdc_modulo { get; set; }

		/// <summary>
		/// Indicado permissao de consulta.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pdc_permissaoConsulta { get; set; }

		/// <summary>
		/// Indicado permissao de edicao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pdc_permissaoEdicao { get; set; }

		/// <summary>
		/// Situação do registro.
        /// 1-Ativo,
        /// 3-Excluido
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte pdc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime pdc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime pdc_dataAlteracao { get; set; }

    }
}