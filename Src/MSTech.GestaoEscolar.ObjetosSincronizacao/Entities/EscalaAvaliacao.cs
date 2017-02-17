using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities;



namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class EscalaAvaliacao
    {
        public Int32 esa_id { get; set; }
        public Int16 esa_tipo { get; set; }
        public EscalaAvaliacaoNumerica escalaAvaliacaoNumerica { get; set; }
        public List<EscalaAvaliacaoParecer> listEscalaAvaliacaoParecer { get; set; }
    }
}
