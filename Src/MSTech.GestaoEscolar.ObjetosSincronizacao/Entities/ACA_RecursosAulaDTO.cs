using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ACA_RecursosAulaDTO
    {
        public int rsa_id { get; set; }
        public string rsa_nome { get; set; }
        public byte rsa_situacao { get; set; }
        public string rsa_dataCriacao { get; set; }
        public string rsa_dataAlteracao { get; set; }
    }
}
