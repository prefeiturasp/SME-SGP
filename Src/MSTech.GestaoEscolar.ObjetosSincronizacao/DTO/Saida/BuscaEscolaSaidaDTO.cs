using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaEscolaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public ESC_EscolaDTO.EscolaEndereco Escola { get; set; }

        public BuscaEscolaSaidaDTO()
        {            
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}