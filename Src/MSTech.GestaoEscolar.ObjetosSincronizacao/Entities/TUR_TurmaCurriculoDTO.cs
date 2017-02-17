using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaCurriculoDTO : TUR_TurmaCurriculo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cur_nome { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string crp_descricao { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }
    }
}
