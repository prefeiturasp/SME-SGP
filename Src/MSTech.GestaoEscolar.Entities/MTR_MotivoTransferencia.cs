/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
    
  [Serializable()]
	public class MTR_MotivoTransferencia : Abstract_MTR_MotivoTransferencia
	{
      [MSValidRange(100, "Motivo de transferência pode conter até 100 caracteres.")]
      [MSNotNullOrEmpty("Motivo de transferência é obrigatório.")]
        public override string mot_nome { get; set; }

        public override DateTime mot_dataCriacao { get; set; }

        public override DateTime mot_dataAlteracao { get; set; }
        [MSDefaultValue(1)]
        public override byte mot_situacao { get; set; }
	}
}