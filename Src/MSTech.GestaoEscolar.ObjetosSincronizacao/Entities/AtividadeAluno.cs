using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class AtividadeAluno
    {
        public Int64 Alu_id { get; set; }
        public string Tna_avaliacao { get; set; }
        public string Tna_relatorio { get; set; }

        public string Tna_dataAlteracao
        {
            get
            {
                return tna_dataAlteracao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }

        public DateTime tna_dataAlteracao_bd { private get; set; }

        public int Tna_participante { get; set; }
    }
}
