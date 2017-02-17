using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaCurriculoPeriodoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<CurriculoPeriodo> CurriculoPeriodo { get; set; }

        public BuscaCurriculoPeriodoSaidaDTO()
        {
            this.CurriculoPeriodo = new List<CurriculoPeriodo>();
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}
