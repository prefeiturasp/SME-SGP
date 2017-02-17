using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class SYS_UsuarioDTO : SYS_Usuario 
    {
        public PES_PessoaDTO pessoa { get; set; }

        public new Guid? pes_id { get; set; }

        public new bool? IsNew { get { return null; } }

        public ACA_AlunoDTO.Referencia aluno { get; set; }

        public ACA_DocenteDTO.Referencia docente { get; set; }

        public struct Usuario
        {
            public Guid Usu_id { get; set; }
            public string Usu_login { get; set; }
            public string Usu_senha { get; set; }
            public byte Usu_criptografia { get; set; }
            public byte Usu_situacao { get; set; }

            public List<UsuarioGrupo> Grupos { get; set; }

            public Professor Professor { get; set; }
            public Administrador Administrador { get; set; }
        }

        public struct UsuarioDadosBasicos
        {
            public Guid usu_id { get; set; }
            public string usu_login { get; set; }
            public string usu_senha { get; set; }
            public byte usu_criptografia { get; set; }
            public byte usu_situacao { get; set; }
            public string usu_dataCriacao { get; set; }
            public string usu_dataAlteracao { get; set; }
        }
    }
}
