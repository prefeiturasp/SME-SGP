using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Arquivo
    {
        public long Arq_id { get; set; }
        public string Arq_nome { get; set; }
        public string Arq_typeMime { get; set; }
        public string Arq_dataAlteracao { get; set; }
    }
}
