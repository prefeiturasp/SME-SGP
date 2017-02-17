using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class StatusServicosDTO
    {
        public string ser_nomeProcedimento { get; set; }
        public string Situacao { get; set; }
        public string PREV_FIRE_TIME { get; set; }
        public string NEXT_FIRE_TIME { get; set; }
        public string ser_nome { get; set; }

        public StatusServicosDTO()
        {
            this.ser_nomeProcedimento = "";
            this.Situacao = "";
            this.PREV_FIRE_TIME = "";
            this.NEXT_FIRE_TIME = "";
            this.ser_nome = "";
        }
    }
}
