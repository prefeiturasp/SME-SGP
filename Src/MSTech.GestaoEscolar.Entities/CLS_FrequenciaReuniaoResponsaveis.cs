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
    ///
    /// </summary>
    [Serializable]
    public class CLS_FrequenciaReuniaoResponsaveis : Abstract_CLS_FrequenciaReuniaoResponsaveis
    {
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }

        [MSNotNullOrEmpty("Calendario é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cal_id { get; set; }

        [MSNotNullOrEmpty("Período é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cap_id { get; set; }

        [MSNotNullOrEmpty("Reunião é obrigatória.")]
        [DataObjectField(true, false, false)]
        public override int frp_id { get; set; }

        [MSNotNullOrEmpty("Frequência é obrigatória.")]
        public override bool frp_frequencia { get; set; }

        public override DateTime frp_dataCriacao { get; set; }

        public override DateTime frp_dataAlteracao { get; set; }
    }

    [Serializable]
    public class CLS_FrequenciaReuniaoResponsaveis_SalvarEmLote
    {
        public Int64 alu_id { get; set; }

        public int cal_id { get; set; }

        public int cap_id { get; set; }

        public int frp_id { get; set; }

        public bool frp_frequencia { get; set; }
    }
}