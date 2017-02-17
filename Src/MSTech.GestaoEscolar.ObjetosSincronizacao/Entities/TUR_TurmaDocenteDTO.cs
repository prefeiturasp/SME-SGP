using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaDocenteDTO:TUR_TurmaDocente
    {

        public long tur_id { get; set; }

        public ReferenciaPesUsuario docente { get; set; }

        public class ReferenciaPesUsuario
        {
            public long doc_id { get; set; }
            public byte doc_situacao { get; set; }
            public DateTime doc_dataCriacao { get; set; }
            public DateTime doc_dataAlteracao { get; set; }
            public PES_PessoaDTO.PessoaDadosBasicosTipado pessoa { get; set; }
        }
    }
}
