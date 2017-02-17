using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaDadosDisciplinasSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<ACA_TipoDisciplinaDTO> TiposDisciplina { get; set; }

        public BuscaDadosDisciplinasSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Disciplinas = new List<Disciplina>();
            this.TiposDisciplina = new List<ACA_TipoDisciplinaDTO>();
        }
    }
}
