using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Docente
    {
        public Int64 doc_id { get; set; }
        public string doc_codigoInep { get; set; }
        public Int16 doc_situacao { get; set; }
        public Int64 col_id { get; set; }
        public Guid ent_id { get; set; }
        public Guid usu_id { get; set; }
        public string pes_nome { get; set; }
    }
}
