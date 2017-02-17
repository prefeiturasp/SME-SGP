namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_CalendarioAnualDTO : ACA_CalendarioAnual
    {
        public bool? IsNew { get { return null; } }
        public List<ACA_CalendarioCursoDTO.Referencia> listaCalendarioCurso { get; set; }
        public List<ACA_CalendarioPeriodoDTO> listaCalendarioPeriodo { get; set; }

        public class Referencia
        {
            public int? cal_id { get; set; }
        }

        public ACA_CalendarioAnualDTO()
        {
            this.listaCalendarioCurso = new List<ACA_CalendarioCursoDTO.Referencia>();
            this.listaCalendarioPeriodo = new List<ACA_CalendarioPeriodoDTO>();
        }
    }
}
