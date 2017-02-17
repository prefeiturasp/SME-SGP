using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaDadosCursosEntidadeSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<Curriculo> Curriculos { get; set; }
        public List<CurriculoPeriodo> CurriculosPeriodo { get; set; }
        public List<TipoNivelEnsino> TiposNivelEnsino { get; set; }                

        public BuscaDadosCursosEntidadeSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Cursos = new List<Curso>();
            this.Curriculos = new List<Curriculo>();
            this.CurriculosPeriodo = new List<CurriculoPeriodo>();
            this.TiposNivelEnsino = new List<TipoNivelEnsino>();
        }
    }
}
