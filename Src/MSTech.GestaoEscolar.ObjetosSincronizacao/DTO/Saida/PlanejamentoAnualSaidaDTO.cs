using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class PlanejamentoAnualSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }

        public List<ORCNivel> Niveis { get; set; }
        public List<ORCOrientacaoCurricular> OrientacoesCurriculares { get; set; }
        public List<ORCOrientacaoCurricularNivelAprendizado> NiveisAprendizadoOrientacao { get; set; }
        public List<ORCNivelAprendizado> NiveisAprendizado { get; set; }

        public PlanejamentoAnualSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Niveis = new List<ORCNivel>();
            this.NiveisAprendizado = new List<ORCNivelAprendizado>();
            this.NiveisAprendizadoOrientacao = new List<ORCOrientacaoCurricularNivelAprendizado>();
            this.OrientacoesCurriculares = new List<ORCOrientacaoCurricular>();
        }
    }
}
