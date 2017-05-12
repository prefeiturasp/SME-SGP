using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class AlunoAnotacaoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<AnotacaoDocenteDTO> anotacoesDocente { get; set; }

        public List<AnotacaoGestorDTO> anotacoesGestor { get; set; }
    }

    public class AnotacaoDocenteDTO
    {
        public string data { get; set; }
        public string anotacao { get; set; }
        public string nomeDocente { get; set; }
        public string nomeDisciplina { get; set; }
    }

    public class AnotacaoGestorDTO
    {
        public string data { get; set; }
        public string anotacao { get; set; }
    }
}
