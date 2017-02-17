using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaBoletimEscolarDosAlunosSaidaPeriodosDTO
    {
        public int tpc_id { get; set; }
        public string tpc_nome { get; set; }
        public int tpc_ordem { get; set; }
        public int ava_idRec { get; set; }
        public string ava_nomeRec { get; set; }
        public string MatriculaPeriodo { get; set; }
    }
}
