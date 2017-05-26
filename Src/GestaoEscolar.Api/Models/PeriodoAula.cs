using System;
using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class PeriodoAula
    {
        public string periodoCalendario { get; set; }
        public string periodo { get; set; }
        public DateTime periodoDataFim { get; set; }
        public int tipoPeriodoCalendarioId { get; set; }
        public int? aulasPrevistas { get; set; }
        public int aulasDadas { get; set; }
        public int aulasRepostas { get; set; }
        public int aulasSugestao { get; set; }
        public bool permitirEditar { get; set; }
    }
}