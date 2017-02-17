using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class CLS_TurmaNotaDTO : CLS_TurmaNota
    {

        public DateTime dataAlteracaoAtividade { get; set; }
        //public Guid usu_idAlteracaoAtividade { get; set; }
        public DateTime dataLancamentoNota { get; set; }
        //public Guid usu_idLancamentoNota { get; set; }
        public DateTime dataExclusaoAtividade { get; set; }
        //public Guid usu_idExclusaoAtividade { get; set; }
        public DateTime maior_dataAlteracao { get; set; }
        public List<CLS_TurmaNotaAlunoDTO> alunos { get; set; }

    }
}
