using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class CLS_TipoAtividadeAvaliativaDTO
    {
        public int tav_id { get; set; }
        public string tav_nome { get; set; }
        public byte tav_situacao { get; set; }
        public string tav_dataAlteracao { get; set; }
        public string tav_dataCriacao { get; set; }
    }
}
