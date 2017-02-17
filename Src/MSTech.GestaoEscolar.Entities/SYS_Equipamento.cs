using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SYS_Equipamento : Abstract_SYS_Equipamento
    {  
        /// <summary>
        /// Descrição do equipamento
        /// </summary>
        [MSValidRange(200)]
        [MSNotNullOrEmpty("Campo descrição é obrigatório.")]
        public override string equ_descricao { get; set; }        
    }
}
