using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class MTR_MatriculaTurmaDisciplinaDTO : MTR_MatriculaTurmaDisciplina
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }
    }
}
