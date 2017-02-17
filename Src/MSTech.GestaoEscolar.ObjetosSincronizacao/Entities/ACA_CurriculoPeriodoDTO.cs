namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ACA_CurriculoPeriodoDTO : ACA_CurriculoPeriodo
    {
        
        public new int? cur_id { get; set; }
        public new int? crr_id { get; set; }
        public new int? mep_id { get; set; }
        public new int? tci_id { get; set; }
        public new int? tcp_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ACA_TipoCicloDTO.Referencia tipoCiclo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ACA_TipoCurriculoPeriodoDTO.Referencia tipoCurriculoPeriodo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ACA_CurriculoDisciplinaDTO> listaCurriculoDisciplina { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ACA_CurriculoDisciplinaEletivaDTO> listaCurriculoDisciplinaEletiva { get; set; }
    }
}
