using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Protocolo
    {
        public Int64 Pro_protocolo { get; set; }
        public int Pro_status { get; set; }
        public int Pro_tipo { get; set; }
        public string Pro_statusObservacao { get; set; }
    }
}