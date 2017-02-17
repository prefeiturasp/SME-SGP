using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class MatriculaTurma
    {
        public int mtu_id { get; set; }
        public PES_PessoaDTO.PessoaDadosBasicos Pessoa { get; set; }
        public string mtr_numeroMatricula { get; set; }
    }
}
