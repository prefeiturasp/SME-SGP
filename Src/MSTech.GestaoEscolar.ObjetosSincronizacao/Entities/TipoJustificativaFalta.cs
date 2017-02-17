using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TipoJustificativaFalta
    {
        public int tjf_id { get; set; }
        public string tjf_nome { get; set; }
        public bool tjf_abonaFalta { get; set; }
        public string tjf_codigo { get; set; }
        public Int16 tjf_situacao { get; set; }
    }
}
