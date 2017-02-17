using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class CalendarioAnualEntradaDTO
    {        
        public Int64 esc_id { get; set; }
        public String SyncDate { get; set; }
        public int cal_ano { get; set; }
    }
}
