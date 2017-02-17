/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.Validation;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
	public class ACA_HistoricoObservacaoPadrao : Abstract_ACA_HistoricoObservacaoPadrao
	{
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public override int hop_id { get; set; }
        public override byte hop_tipo { get; set; }
        public override string hop_nome { get; set; }
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string hop_descricao { get; set; }
        [MSDefaultValue(1)]
        public override byte hop_situacao { get; set; }
        public override DateTime hop_dataCriacao { get; set; }
        public override DateTime hop_dataAlteracao { get; set; }
	}
}