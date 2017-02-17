using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class EscalaAvaliacaoParecer
    {
        public int esa_id { get; set; }
        public int eap_id { get; set; }
        public string eap_valor { get; set; }
        public string eap_descricao { get; set; }
        public string eap_abreviatura { get; set; }
        public int eap_ordem { get; set; }
        public decimal eap_equivalenteInicio { get; set; }
        public decimal eap_equivalenteFim { get; set; }
        public byte eap_situacao { get; set; }
    }
}
