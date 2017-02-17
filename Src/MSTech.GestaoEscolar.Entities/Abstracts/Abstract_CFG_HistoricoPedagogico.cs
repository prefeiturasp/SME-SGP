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
    public abstract class Abstract_CFG_HistoricoPedagogico : Abstract_Entity
    {
		
		/// <summary>
		/// Ano letivo de geração do histórico.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int chp_anoLetivo { get; set; }

		/// <summary>
		/// Curso padrão do Ensino Fundamental utilizado para carregar as disciplinas e períodos.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cur_idFundamentalPadrao { get; set; }

        /// <summary>
        /// Quantidade máxima de projetos/atividades complementares que podem ser cadastrados por aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int chp_qtdMaxProjeto { get; set; }

    }
}