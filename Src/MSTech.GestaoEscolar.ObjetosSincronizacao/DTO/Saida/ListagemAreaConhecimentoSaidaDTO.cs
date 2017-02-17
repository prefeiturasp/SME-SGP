using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class ListagemAreaConhecimentoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ACA_AreaConhecimento> Data { get; set; }

        public ListagemAreaConhecimentoSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Data = new List<ACA_AreaConhecimento>();
        }

    }
}
