using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class ListagemTipoDisciplinaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ACA_TipoDisciplinaDTO> Data { get; set; }

        public ListagemTipoDisciplinaSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Data = new List<ACA_TipoDisciplinaDTO>();
        }
    }
}
