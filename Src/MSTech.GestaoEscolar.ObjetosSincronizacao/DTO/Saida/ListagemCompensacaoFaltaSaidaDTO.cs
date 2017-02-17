using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class ListagemCompensacaoFaltaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<CompensacaoFalta> CompensacaoFalta { get; set; }

        public ListagemCompensacaoFaltaSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.CompensacaoFalta = new List<CompensacaoFalta>();
        }
    }
}
