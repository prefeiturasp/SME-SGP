using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaEventosSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ACA_Evento> ListaEventos { get; set; }

        public BuscaEventosSaidaDTO()
        {            
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.ListaEventos = new List<ACA_Evento>();
        }
    }
}