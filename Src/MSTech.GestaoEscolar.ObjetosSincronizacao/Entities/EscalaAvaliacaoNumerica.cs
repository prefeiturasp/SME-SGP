using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class EscalaAvaliacaoNumerica
    {
        public int esa_id { get; set; }
        public decimal ean_menorValor { get; set; }
        public decimal ean_maiorValor { get; set; }
        public decimal ean_variacao { get; set; }
        public byte ean_situacao { get; set; }
    }
}
