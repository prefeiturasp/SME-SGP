using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{

    public struct AlunoCF
    {
        public long alu_id;
    }

    public class CompensacaoFalta
    {
        public Int64 tud_id { get; set; }
        public int cpa_id { get; set; }
        public int tpc_id { get; set; }
        public int cpa_quantidadeAulasCompensadas { get; set; }
        public string cpa_atividadesDesenvolvidas { get; set; }
        public int cpa_situacao { get; set; }
        public string cpa_dataAlteracao { get; set; }
        public long pro_protocolo { get; set; }

        public List<AlunoCF> Alunos { get; set; }

        public CompensacaoFalta()
        {
            this.Alunos = new List<AlunoCF>();
        }
    }
}
