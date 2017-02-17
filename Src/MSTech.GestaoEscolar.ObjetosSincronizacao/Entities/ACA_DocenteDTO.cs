using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ACA_DocenteDTO : ACA_Docente
    {
        public PES_PessoaDTO pessoa { get; set; }
        public List<ESC_EscolaDTO> listaEscola { get; set; }

        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public long doc_id { get; set; }
        }

        public struct DocentePessoaUsuario
        {
            public Int64 doc_id { get; set; }
            public byte doc_situacao { get; set; }
            public string doc_dataCriacao { get; set; }
            public string doc_dataAlteracao { get; set; }

            public PES_PessoaDTO.PessoaDadosBasicos Pessoa { get; set; }
            public SYS_UsuarioDTO.UsuarioDadosBasicos Usuario { get; set; }
        }
    }
}
