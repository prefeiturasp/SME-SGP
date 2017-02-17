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
    public abstract class Abstract_CFG_PermissaoModuloOperacao : Abstract_Entity
    {
		
		/// <summary>
		/// ID do grupo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid gru_id { get; set; }

		/// <summary>
		/// ID do sistema.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int sis_id { get; set; }

		/// <summary>
		/// ID do módulo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mod_id { get; set; }

		/// <summary>
		/// Operação dentro do módulo
        //1-Dados do aluno, 
        //2-Histórico escolar do EF, 
        //3-Histórico escolar do EJA, 
        //4-Histórico escolar - transferência, 
        //5-istórico escolar - informações complementares.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int pmo_operacao { get; set; }

		/// <summary>
		/// Informa se o grupo de usuários tem permissão de consulta na operação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pmo_permissaoConsulta { get; set; }

		/// <summary>
		/// Informa se o grupo de usuários tem permissão de edição na operação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pmo_permissaoEdicao { get; set; }

		/// <summary>
		/// Propriedade pmo_permissaoInclusao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pmo_permissaoInclusao { get; set; }

		/// <summary>
		/// Propriedade pmo_permissaoExclusao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool pmo_permissaoExclusao { get; set; }

    }
}