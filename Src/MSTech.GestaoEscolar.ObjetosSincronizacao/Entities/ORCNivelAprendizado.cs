using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ORCNivelAprendizado
    {
        public int Nap_id { get; set; }
        public string Nap_descricao { get; set; }
        public string Nap_sigla { get; set; }
        public int Nap_situacao { get; set; }
        public Int32 Cur_id { get; set; }
        public Int32 Crr_id { get; set; }
        public Int32 Crp_id { get; set; }
    }
}
