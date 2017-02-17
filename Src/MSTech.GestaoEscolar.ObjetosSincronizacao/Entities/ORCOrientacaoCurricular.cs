using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ORCOrientacaoCurricular
    {
        public long Ocr_id { get; set; }
        public int Nvl_id { get; set; }
        public int Tds_id { get; set; }
        public long Ocr_idSuperior { get; set; }
        public string Ocr_descricao { get; set; }
        public string Ocr_codigo { get; set; }
        public int Ocr_situacao { get; set; }
    }
}
