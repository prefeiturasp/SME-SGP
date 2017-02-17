using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class JustificativaFaltaAluno
    {
        public long alu_id { get; set; }
        public int tjf_id { get; set; }
        public DateTime afj_dataInicio { get; set; }
        public DateTime afj_dataFim { get; set; }
        public Int16 afj_situacao { get; set; }			
        public int afj_id { get; set; }
        public DateTime afj_dataAlteracao { get; set; }		
    }
}
