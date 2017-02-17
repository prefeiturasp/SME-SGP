using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class ListagemPlanejamentoTurmaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Planejamento> Planejamento { get; set; }
        public List<AlunoTurmaDisciplinaOrientacaoCurricular> AlunoTurmaDisciplinaOrientacaoCurricular { get; set; }

        public ListagemPlanejamentoTurmaSaidaDTO()
        {
            this.Planejamento = new List<Planejamento>();
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Planejamento = new List<Planejamento>();
            this.AlunoTurmaDisciplinaOrientacaoCurricular = new List<AlunoTurmaDisciplinaOrientacaoCurricular>();
        }
    }
}
