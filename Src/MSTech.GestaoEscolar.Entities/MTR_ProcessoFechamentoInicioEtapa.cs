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
	[Serializable]
	public class MTR_ProcessoFechamentoInicioEtapa : Abstract_MTR_ProcessoFechamentoInicioEtapa
    {
        [MSNotNullOrEmpty("Processo de fechamento/início ano letivo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int pfi_id { get; set; }

        public override int pfe_id { get; set; }

        [MSNotNullOrEmpty("Data de fim da etapa é obrigatório.")]
        public override DateTime pfe_dataFim { get; set; }

        [MSNotNullOrEmpty("Data de início da etapa é obrigatório.")]
        public override DateTime pfe_dataInicio { get; set; }

        [MSNotNullOrEmpty("Tipo da etapa é obrigatório.")]
        public override byte pfe_tipoEtapa { get; set; }

        [MSNotNullOrEmpty("Alcance é obrigatório.")]
        public override byte pfe_alcance { get; set; }

        public override int cal_idFechamento { get; set; }
        public override int cal_idInicio { get; set; }
        public override int cur_id { get; set; }
        public override int crr_id { get; set; }
        public override int crp_id { get; set; }
        public override int esc_id { get; set; }
        public override int uni_id { get; set; }

        [MSDefaultValue(1)]
        public override byte pfe_situacao { get; set; }

        public override DateTime pfe_dataCriacao { get; set; }
        public override DateTime pfe_dataAlteracao { get; set; }
	}
}