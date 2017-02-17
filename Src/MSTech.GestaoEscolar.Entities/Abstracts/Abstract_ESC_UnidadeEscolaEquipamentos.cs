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
    public abstract class AbstractESC_UnidadeEscolaEquipamentos : Abstract_Entity
    {
		
		/// <summary>
		/// Id da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id da tabela ESC_UnidadeEscolaEquipamentos.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ueq_id { get; set; }

		/// <summary>
		/// Aparelho de Televisão.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_aparelhoTelevisao { get; set; }

		/// <summary>
		/// Videocassete.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_videocassete { get; set; }

		/// <summary>
		/// DVD.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_dvd { get; set; }

		/// <summary>
		/// Antena parabólica.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_antenaParabolica { get; set; }

		/// <summary>
		/// Copiadora.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_copiadora { get; set; }

		/// <summary>
		/// Retroprojetor.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_retroprojetor { get; set; }

		/// <summary>
		/// Impressora.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_impressora { get; set; }

		/// <summary>
		/// Aparelho de som.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_aparelhoSom { get; set; }

		/// <summary>
		/// Projetor Multimídia (Data show).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_projetorMultimidia { get; set; }

		/// <summary>
		/// Fax.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_fax { get; set; }

		/// <summary>
		/// Máquina Fotográfica/Filmadora.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_maquinaFotografica { get; set; }

		/// <summary>
		/// Computadores.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ueq_computadores { get; set; }

		/// <summary>
		/// Situação do registro: 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ueq_situacao { get; set; }

		/// <summary>
		/// Data de Criação de registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ueq_dataCriacao { get; set; }

		/// <summary>
		/// Data de Alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ueq_dataAlteracao { get; set; }

    }
}