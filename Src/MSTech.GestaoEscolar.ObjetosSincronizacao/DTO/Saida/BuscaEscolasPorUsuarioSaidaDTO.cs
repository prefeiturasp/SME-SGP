using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaEscolasPorUsuarioSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<ESC_EscolaDTO.EscolaEndereco> Escolas { get; set; }

        public BuscaEscolasPorUsuarioSaidaDTO()
        {
            this.Escolas = new List<ESC_EscolaDTO.EscolaEndereco>();
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}
