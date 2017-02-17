using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaTurmaDisciplinaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ACA_TipoDisciplinaDTO> TipoDisciplinas { get; set; }

        public BuscaTurmaDisciplinaSaidaDTO()
        {
            this.TipoDisciplinas = new List<ACA_TipoDisciplinaDTO>();
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}
