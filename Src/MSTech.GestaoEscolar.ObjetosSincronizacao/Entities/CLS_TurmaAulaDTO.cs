using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class CLS_TurmaAulaDTO : CLS_TurmaAula
    {
        public long doc_id { get; set; }
        public DateTime dataAlteracaoAula { get; set; }
        //public Guid usu_idAlteracaoAula { get; set; }
        public DateTime dataExclusaoAula { get; set; }
        //public Guid usu_idExclusaoAula { get; set; }
        public DateTime dataAlteracaoPlanoAula { get; set; }
        //public Guid usu_idAlteracaoPlanoAula { get; set; }
        public DateTime dataAlteracaoFrequencia { get; set; }
        //public Guid usu_idAlteracaoFrequencia { get; set; }
        public DateTime dataAlteracaoAnotacao { get; set; }
        //public Guid usu_idAlteracaoAnotacao { get; set; }
        public DateTime maior_dataAlteracao { get; set; }
        public List<CLS_TurmaAulaAlunoDTO> alunos { get; set; }
        public List<CLS_TurmaNotaDTO> atividades { get; set; }
        public List<CLS_TurmaAulaRecursoDTO> recursos { get; set; }
        public List<CLS_TurmaAulaOrientacaoCurricularDTO> habilidadesPlanoAula { get; set; }

    }
}
