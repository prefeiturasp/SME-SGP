/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class SYS_Servicos : Abstract_SYS_Servicos
	{
        // utilizado no cadastro de serviços
        public virtual string ser_ativoDescricao { get; set; }
	}
}