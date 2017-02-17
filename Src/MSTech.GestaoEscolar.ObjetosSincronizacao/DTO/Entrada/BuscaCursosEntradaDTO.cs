using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaCursosEntradaDTO
    {
        public Guid Ent_id { get; set; }
        public int cal_id { get; set; }
        public int esc_id { get; set; }
    }
}
