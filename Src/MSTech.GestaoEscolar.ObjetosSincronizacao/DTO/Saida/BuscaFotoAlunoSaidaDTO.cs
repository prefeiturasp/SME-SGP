using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaFotoAlunoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<Foto> Fotos { get; set; }

        public BuscaFotoAlunoSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Fotos = new List<Foto>();
        }

    }
}
