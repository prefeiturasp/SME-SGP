using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaDTO : TUR_Turma
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaCurriculoDTO> listaTurmaCurriculo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDisciplinaDTO> listaTurmaDisciplina { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaCurriculoAvaliacao> listaTurmaCurriculoAvaliacao { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MTR_MatriculaTurmaDTO> listaMatriculaTurma { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDocenteDTO> listaTurmaDocente { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ESC_EscolaDTO.Referencia escola { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public long tur_id { get; set; }
        }
    }
}
