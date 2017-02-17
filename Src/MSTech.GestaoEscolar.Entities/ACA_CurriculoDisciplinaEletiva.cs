/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_CurriculoDisciplinaEletiva : Abstract_ACA_CurriculoDisciplinaEletiva
	{
        [MSValidRange(200, "Nome do grupo de [MSG_DISCIPLINAS] eletivas pode conter até 200 caracteres.")]
        public override string cde_nome { get; set; }
	}
}