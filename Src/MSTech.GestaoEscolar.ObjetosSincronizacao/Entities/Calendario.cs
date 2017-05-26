using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Calendario
    {
        public int Cal_id { get; set; }
        public DateTime Tpc_dataSincronizacao { get; set; }
        public List<ACA_CalendarioPeriodoDTO> Periodos { get; set; }
        public List<Nivel> Niveis { get; set; }
    }

    public class CalendarioAlunoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<CalendarioAlunoDTO> calendarios { get; set; }
    }

    public class CalendarioAlunoDTO
    {
        public int cal_id { get; set; }
        public int cal_ano { get; set; }
        public int mtu_id { get; set; }
        public int tpc_id { get; set; }
    }
}
