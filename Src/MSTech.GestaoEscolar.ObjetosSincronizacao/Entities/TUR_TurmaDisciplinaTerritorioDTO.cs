using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaDisciplinaTerritorioDTO
    {
        public long tte_id { get; set; }
        public long Tud_idExperiencia { get; set; }
        public DateTime tte_vigenciaInicio { get; set; }
        public DateTime tte_vigenciaFim { get; set; }
        public byte tte_situacao { get; set; }
    }
}
