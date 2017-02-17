using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaAulaEntradaDTO
    {
        public Int64 Tud_id { get; set; }
        public Int64 Tur_id { get; set; }
        public Int32 paraTras { get; set; }
        public Int32 paraFrente { get; set; }
        public Boolean primeiraSincronizacao { get; set; }
    }
}
