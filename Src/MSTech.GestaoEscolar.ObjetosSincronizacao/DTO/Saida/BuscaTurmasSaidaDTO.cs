using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaTurmasSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Turma> Turmas { get; set; }
        public List<TipoDisciplinaDeficiencia> TiposDisciplinaDeficiencia { get; set; }

        public BuscaTurmasSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Turmas = new List<Turma>();
            this.TiposDisciplinaDeficiencia = new List<TipoDisciplinaDeficiencia>();
        }
    }
}