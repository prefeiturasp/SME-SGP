using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class CalendarioAnualSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }

        public List<CalendarioPeriodo> CalendarioPeriodo { get; set; }

        public CalendarioAnualSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.CalendarioPeriodo = new List<CalendarioPeriodo>();
        }
    }
}
