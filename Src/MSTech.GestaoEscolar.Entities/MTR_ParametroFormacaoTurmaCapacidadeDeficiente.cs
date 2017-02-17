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
	public class MTR_ParametroFormacaoTurmaCapacidadeDeficiente : Abstract_MTR_ParametroFormacaoTurmaCapacidadeDeficiente
    {
        public override int pfc_id { get; set; }

        [MSNotNullOrEmpty("Quantidade de deficientes é obrigatório.")]
        public override int pfc_qtdDeficiente { get; set; }

        [MSNotNullOrEmpty("Capacidade com aluno deficiente é obrigatório.")]
        public override int pfc_capacidadeComDeficiente { get; set; }
        
        [MSDefaultValue(1)]
        public override byte pfc_situacao { get; set; }

        public override DateTime pfc_dataCriacao { get; set; }
        public override DateTime pfc_dataAlteracao { get; set; }
	}
}