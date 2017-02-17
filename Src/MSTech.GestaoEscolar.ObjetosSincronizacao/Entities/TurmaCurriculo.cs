using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TurmaCurriculo
    {
        public Int64 tur_id { get; set; }
        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }
        public int tcr_prioridade { get; set; }
        public byte tcr_situacao { get; set; }
    }
}
