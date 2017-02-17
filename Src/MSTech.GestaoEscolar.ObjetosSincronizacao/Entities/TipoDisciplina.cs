using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TipoDisciplina
    {
        public int tds_id { get; set; }
        public string tds_nome { get; set; }
        public Int16 tds_base { get; set; }
        public int tds_ordem { get; set; }
        public Int16 tds_situacao { get; set; }
    }
}
