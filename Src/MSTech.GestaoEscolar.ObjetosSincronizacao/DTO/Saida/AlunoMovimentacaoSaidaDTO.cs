using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class AlunoMovimentacaoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<MovimentacaoDTO> movimentacoes { get; set; }
    }

    public class MovimentacaoDTO
    {
        public string dataRealizacao { get; set; }
        public string tipo { get; set; }
        public string escolaAnterior { get; set; }
        public string escolaAtual { get; set; }
        public string turmaAnterior { get; set; }
        public string turmaAtual { get; set; }
    }
}
