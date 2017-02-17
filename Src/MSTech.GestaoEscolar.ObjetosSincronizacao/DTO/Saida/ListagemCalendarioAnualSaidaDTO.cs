using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class ListagemCalendarioAnualSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ACA_CalendarioAnualDTO> Data { get; set; }

        public ListagemCalendarioAnualSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Data = new List<ACA_CalendarioAnualDTO>();
        }
    }
}
