namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class MTR_MatriculaTurmaDTO : MTR_MatriculaTurma
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new long? alu_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new long? tur_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ACA_AlunoDTO.ReferenciaPesUsuario aluno { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TUR_TurmaDTO.Referencia turma { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MTR_MatriculaTurmaDisciplinaDTO> listaMatriculaTurmaDisciplina { get; set; }

        public class Referencia
        {
            public int? mtu_id { get; set; }
        }
    }
}