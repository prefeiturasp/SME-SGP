using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Turno
    {
        public int trn_id { get; set; }
        public Guid ent_id { get; set; }
        public string trn_descricao { get; set; }
        public bool trn_padrao { get; set; }
        public Int16 trn_situacao { get; set; }
        public Int16 trn_controleTempo { get; set; }
        public TimeSpan trn_horaInicio { get; set; }
        public TimeSpan trn_horaFim { get; set; }
    }
}
