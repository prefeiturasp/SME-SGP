using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class EscolaEntidade
    {
        public string esc_nome { get; set; }
        public string esc_codigo { get; set; }
        public Guid ent_id { get; set; }
        public Guid uad_id { get; set; }
        public int esc_id { get; set; }
        public int tre_id { get; set; }
        public string tre_nome { get; set; }
    }
}
