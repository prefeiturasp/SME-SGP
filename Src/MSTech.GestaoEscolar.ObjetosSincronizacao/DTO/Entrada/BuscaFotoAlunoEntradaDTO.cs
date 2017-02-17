using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaFotoAlunoEntradaDTO
    {
        public String Alu_id { get; set; }
        public DateTime Fot_dataSincronizacao { get; set; }
        public int Fot_largura { get; set; }
        public int Fot_altura { get; set; }
    }
}
