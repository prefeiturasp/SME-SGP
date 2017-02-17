/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_CFG_ParametroMensagem : Abstract_Entity
    {

		/// <summary>
		/// ID do parâmetro de mensagem.
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int pms_id { get; set; }

		/// <summary>
		/// Tela que será utilizada a mensagem. 
		/// 1-Planejamento anual, 2-Cadastro de aulas
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pms_tela { get; set; }

		/// <summary>
		/// Chave do parâmetro de mensagem.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string pms_chave { get; set; }

		/// <summary>
		/// Mensagem.
		/// </summary>
		[MSValidRange(2000)]
		[MSNotNullOrEmpty()]
		public virtual string pms_valor { get; set; }

		/// <summary>
		/// Descrição da utilização da mensagem.
		/// </summary>
		[MSValidRange(200)]
		public virtual string pms_descricao { get; set; }

		/// <summary>
		/// Situação do registro. 1-Ativo,3-Excluido
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pms_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pms_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pms_dataAlteracao { get; set; }

    }
}