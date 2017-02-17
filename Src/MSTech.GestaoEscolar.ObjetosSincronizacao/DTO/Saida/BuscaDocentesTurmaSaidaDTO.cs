using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaDocentesTurmaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Docente> Docentes { get; set; }

        public BuscaDocentesTurmaSaidaDTO()
        {
            this.Docentes = new List<Docente>();
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}
