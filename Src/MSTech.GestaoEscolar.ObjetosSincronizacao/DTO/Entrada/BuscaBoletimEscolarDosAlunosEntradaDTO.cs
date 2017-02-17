using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaBoletimEscolarDosAlunosEntradaDTO
    {
        public string alu_ids { get; set; }
        public string mtu_ids { get; set; }
        public int tpc_id { get; set; }
    }
}
