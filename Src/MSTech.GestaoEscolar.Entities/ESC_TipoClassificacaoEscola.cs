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
    [Serializable()]
	public class ESC_TipoClassificacaoEscola : Abstract_ESC_TipoClassificacaoEscola
	{
        [MSValidRange(100, "Tipo de classificação de escola pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de classificação de escola é obrigatório.")]
        public override string tce_nome { get; set; }

        public override DateTime tce_dataCriacao { get; set; }

        public override DateTime tce_dataAlteracao { get; set; }
        [MSDefaultValue(1)]
        public override byte tce_situacao { get; set; }

        public override bool tce_permiteQualquerCargoEscola { get; set; }
    }
}