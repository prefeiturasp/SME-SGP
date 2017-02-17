using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaTiposAtividadeAvaliativaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<TipoAtividadeAvaliativa> TiposAtividadeAvaliativa { get; set; }

        public BuscaTiposAtividadeAvaliativaSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.TiposAtividadeAvaliativa = new List<TipoAtividadeAvaliativa>();
        }
    }
}
