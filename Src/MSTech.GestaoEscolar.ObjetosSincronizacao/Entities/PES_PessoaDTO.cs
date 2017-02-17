using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class PES_PessoaDTO : PES_Pessoa
    {
        public new bool? IsNew { get { return null; } }

        public struct PessoaDadosBasicos
        {
            public Guid pes_id { get; set; }
            public string pes_nome { get; set; }
            public byte pes_situacao { get; set; }
            public string pes_dataCriacao { get; set; }
            public string pes_dataAlteracao { get; set; }
            public bool possuiAlteracaoFoto { get; set; }
        }

        public struct PessoaDadosBasicosTipado
        {
            public Guid pes_id { get; set; }
            public string pes_nome { get; set; }
            public byte pes_situacao { get; set; }
            public DateTime pes_dataCriacao { get; set; }
            public DateTime pes_dataAlteracao { get; set; }
            public bool possuiAlteracaoFoto { get; set; }
        }
    }
}
