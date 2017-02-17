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
    public abstract class Abstract_RES_ChaveResource : Abstract_Entity
    {
		
		/// <summary>
		/// Chave do parâmetro de mensagem..
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual string rcr_chave { get; set; }

		/// <summary>
		/// Descrição da utilização da mensagem..
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual string rcr_NomeResource { get; set; }

		/// <summary>
		/// Propriedade rcr_cultura.
		/// </summary>
		[MSValidRange(10)]
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual string rcr_cultura { get; set; }

		/// <summary>
		/// Propriedade rcr_codigo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rcr_codigo { get; set; }

		/// <summary>
		/// Mensagem..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string rcr_valor { get; set; }

		/// <summary>
		/// Situação do registro.
        //1-Ativo,
        //3-Excluido
        //.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rcr_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rcr_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rcr_dataAlteracao { get; set; }

    }
}