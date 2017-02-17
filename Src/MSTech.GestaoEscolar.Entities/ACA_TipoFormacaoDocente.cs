/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_TipoFormacaoDocente : Abstract_ACA_TipoFormacaoDocente
	{
        [MSValidRange(100, "Tipo de formação do docente pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de formação do docente é obrigatório")]
        public override string tfd_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte tfd_situacao { get; set; }
        public override DateTime tfd_dataCriacao { get; set; }
        public override DateTime tfd_dataAlteracao { get; set; }
	}
}