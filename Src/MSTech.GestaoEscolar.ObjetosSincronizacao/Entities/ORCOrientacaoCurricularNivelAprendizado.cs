using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ORCOrientacaoCurricularNivelAprendizado
    {
        public long Ocr_id { get; set; }
        public int Ocn_id { get; set; }
        public int Nap_id { get; set; }
        public int Ocn_situacao { get; set; }
    }
}
