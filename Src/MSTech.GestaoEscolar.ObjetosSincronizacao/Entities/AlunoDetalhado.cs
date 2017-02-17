using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class AlunoDetalhado
    {
        public Guid uad_id { get; set; }
        public Guid ent_id { get; set; }
        public Int32 esc_id { get; set; }
        public string alc_matricula { get; set; }
        public Int64 alu_id { get; set; }
        public Guid pes_id { get; set; }
        public string pes_nome { get; set; }
        public Int32 alc_id { get; set; }
        public Int32 mtu_id { get; set; }
        public Int64 tur_id { get; set; }
        public string tur_codigo { get; set; }
        public string tur_descricao { get; set; }
    }
}
