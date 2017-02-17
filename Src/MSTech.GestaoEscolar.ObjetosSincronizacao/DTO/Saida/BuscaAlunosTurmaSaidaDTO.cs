using System;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaAlunosTurmaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<TurmaAlunos> TurmaAlunos { get; set; }        

        public BuscaAlunosTurmaSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.TurmaAlunos = new List<TurmaAlunos>();
        }
    }
}