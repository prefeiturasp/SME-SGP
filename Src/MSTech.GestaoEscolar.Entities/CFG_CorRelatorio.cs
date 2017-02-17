/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CFG_CorRelatorio : Abstract_CFG_CorRelatorio
	{
        /// <summary>
        /// Id do ralatório.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int rlt_id { get; set; }

        /// <summary>
        /// Id da cor do ralatório.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int cor_id { get; set; }

        /// <summary>
        /// Cor da paleta do ralatório.
        /// </summary>
        [MSValidRange(10, "Descrição pode conter até 10 caracteres.")]
        public override string cor_corPaleta { get; set; }

        /// <summary>
        /// ordem da cor do ralatório.
        /// </summary>
       [MSDefaultValue(1)]
        public override short cor_ordem { get; set; }

        /// <summary>
        /// Situação da cor do ralatório.
        /// </summary>
        [MSDefaultValue(1)]
       public override short cor_situacao { get; set; }

        /// <summary>
         ///Data de criação da cor do relatório.
         ///</summary>
        public override DateTime cor_dataCriacao { get; set; }

         /// <summary>         
         ///Data de alteração da cor do relatório.
        ///<summary>
        public override DateTime cor_dataAlteracao { get; set; }        

	}
}