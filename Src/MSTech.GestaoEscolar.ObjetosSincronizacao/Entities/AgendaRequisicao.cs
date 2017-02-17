using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class AgendaRequisicao
    {
        public int Req_id { get; set; }
        public int Age_periodicidade { get; set; }
        public List<AgendaHorarios> AgendaHorarios { get; set; }
        public List<Api> Apis { get; set; }
    }
}