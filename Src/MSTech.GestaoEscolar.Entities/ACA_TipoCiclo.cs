/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// Ciclos de Aprendizagem
	/// </summary>
    [Serializable]
	public class ACA_TipoCiclo : Abstract_ACA_TipoCiclo
	{
        [DataObjectField(true, true, false)]
        public override int tci_id { get; set; }
       
        [MSValidRange(100, "Tipo de ciclo pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de ciclo é obrigatório.")]
        public override string tci_nome { get; set; }

        /// <summary>
        /// Campo que define se o tipo de ciclo pode possuir objeto de aprendizagem
        /// </summary>
        public override bool tci_objetoAprendizagem { get; set; }

        [MSDefaultValue(1)]
        public override byte tci_situacao { get; set; }
        
        public override DateTime tci_dataCriacao { get; set; }
        
        public override DateTime tci_dataAlteracao { get; set; }

        [MSDefaultValue(0)]
        public override bool tci_exibirBoletim { get; set; }

        [MSDefaultValue(1)]
        public override int tci_ordem { get; set; }
	}
}