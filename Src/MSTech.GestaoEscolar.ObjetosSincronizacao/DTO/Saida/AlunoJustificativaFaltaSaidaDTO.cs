using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class AlunoJustificativaFaltaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<JustificativaDTO> justificativas { get; set; }
    }

    public class JustificativaDTO
    {
        public string tipo { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public string observacao { get; set; }
    }
}
