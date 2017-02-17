using System;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class SincronizacaoDiarioClasseSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public Protocolo Protocolo { get; set; }

        public SincronizacaoDiarioClasseSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Protocolo = new Protocolo();
        }
    }
}