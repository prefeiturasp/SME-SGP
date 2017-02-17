using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class FilaAlunoFechamentoPendenciaDTO
    {
        public string Situacao { get; set; }
        public int QtDisciplinas { get; set; }

        public FilaAlunoFechamentoPendenciaDTO()
        {
            this.Situacao = "";
            this.QtDisciplinas = 0;
        }
    }
}
