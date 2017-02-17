using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class OrientacaoCurricular
    {
        public int Ocr_id {get; set; }
        public int Nvl_id {get; set; }
        public int Tds_id {get; set; } 
        public int Ocr_idSuperior {get; set; } 
        public string Ocr_descricao {get; set; }
        public string Ocr_codigo { get; set; }
        public DateTime Ocr_dataSincronizacao { get; set; }
        public List<OrientacaoCurricularNivelAprendizado> NiveisAprendizado { get; set; }
    }
}
