namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GraficoAtendimentoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public string TituloGrafico { get; set; }

        public byte TipoGrafico { get; set; }

        public string NomeEixoAgrupamento { get; set; }

        public List<GraficoAtendimentoDadoDTO> Dados { get; set; }
    }

    public class GraficoAtendimentoDadoDTO
    {
        public string Label { get; set; }

        public string Serie { get; set; }

        public int Valor { get; set; }
    }
}
