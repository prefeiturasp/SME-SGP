/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using MSTech.GestaoEscolar.Entities.Abstracts;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class CLS_FrequenciaReuniao : AbstractCLS_FrequenciaReuniao
    {
        /// <summary>
        /// Data de criação.
        /// </summary>
        public override DateTime frr_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração.
        /// </summary>
        public override DateTime frr_dataAlteracao { get; set; }
    }

    [Serializable]
    public class CLS_FrequenciaReuniao_SalvarEmLote
    {
        public long tur_id { get; set; }

        public int cal_id { get; set; }

        public int cap_id { get; set; }

        public int frp_id { get; set; }

        public bool frr_efetivado { get; set; }
    }
}