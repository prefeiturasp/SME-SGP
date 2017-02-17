using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class PlanejamentoAnualEntradaDTO
    {
        public Int64 esc_id { get; set; }
        public String SyncDate { get; set; }

        public bool sincronizarPorEscola;

        public Int64 tur_id { get; set; }
        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }
        public int cal_id { get; set; }
        public int tds_id { get; set; }
    }
}
