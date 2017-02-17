using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class CLS_TurmaAulaAlunoDTO : CLS_TurmaAulaAluno
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CLS_TurmaAulaAlunoTipoAnotacao> listaTurmaAulaAlunoTipoAnotacao { get; set; }
    
    }
}
