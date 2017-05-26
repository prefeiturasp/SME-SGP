using System;
using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class TurmaDisciplinaAulaPrevista
    {
        public long turmaDisciplinaId { get; set; }
        public int escolaId { get; set; }
        public int turmaId { get; set; }
        public int calendarioId { get; set; }
        public byte turmaDocentePosicao { get; set; }
        public byte turmaDisciplinaTipo { get; set; }
        public bool fechamentoAutomatico { get; set; }
        public DateTime periodoDataFim { get; set; }
        public IEnumerable<AulaPrevista> aulasPrevistas { get; set; }

    }
}