using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaDisciplinaDTO : TUR_TurmaDisciplina 
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tds_nome { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new long? tur_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDocente> listaTurmaDocente { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TUR_TurmaDisciplinaRelDisciplina turmaDiscRelDisciplina { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDisciplinaCalendario> listaTurmaCalendario { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDisciplinaNaoAvaliado> listaTurmaDisciplinaNaoAvaliado { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TUR_TurmaDTO.Referencia turma { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ACA_DocenteDTO.Referencia docente { get; set; }
    }
}
