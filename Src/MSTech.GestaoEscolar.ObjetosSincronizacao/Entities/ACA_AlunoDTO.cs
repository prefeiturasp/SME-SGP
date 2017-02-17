using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ACA_AlunoDTO : ACA_Aluno
    {
        public PES_PessoaDTO pessoa { get; set; }

        public List<MTR_MatriculaTurmaDTO> listaMatriculaTurma { get; set; }

        public List<ACA_AlunoCurriculoDTO> listaAlunoCurriculo { get; set; }

        public TUR_TurmaDTO turma { get; set; }

        public new Guid? pes_id { get; set; }

        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public long alu_id { get; set; }
        }

        public class ReferenciaPesUsuario
        {
            public long alu_id { get; set; }
            public byte alu_situacao { get; set; }
            public DateTime alu_dataCriacao { get; set; }
            public DateTime alu_dataAlteracao { get; set; }
            public PES_PessoaDTO.PessoaDadosBasicosTipado pessoa { get; set; }
        }
    }

    public struct Aluno
    {
        public Int64 Alu_id { get; set; }
        public byte Alu_situacao { get; set; }
        public string Pes_nome { get; set; }
        public int? Mtu_numeroChamada { get; set; }
        public string Pes_dataNascimento { get; set; }
        public byte Pes_sexo { get; set; }
        public string Taa_frequenciaBitMap { get; set; }
        public int Taa_frequencia { get; set; }
        public string Taa_dataAlteracao { get; set; }
        public string Taa_anotacao { get; set; }
        public string Mtu_dataMatricula { get; set; }
        public string Mtu_dataSaida { get; set; }
        public int temFoto { get; set; }
        public string arq_dataAlteracao { get; set; }
        public List<Deficiencia> Deficiencias { get; set; }
    }

    public struct AlunoTurma
    {
        public Int64 alu_id { get; set; }
        public string pes_nome { get; set; }
        public int mtu_numeroChamada { get; set; }
        public string mtu_dataMatricula { get; set; }
        public string mtu_dataSaida { get; set; }
        public int temFoto { get; set; }
    }

    /// <summary>
    /// estrutura de dados do aluno com os dados de pessoa e usuario (se existir)
    /// </summary>
    public struct AlunoPessoaUsuario
    {
        public Int64 alu_id { get; set; }
        public byte alu_situacao { get; set; }
        public string alu_dataCriacao { get; set; }
        public string alu_dataAlteracao { get; set; }
        public PES_PessoaDTO.PessoaDadosBasicos Pessoa { get; set; }
        public SYS_UsuarioDTO.UsuarioDadosBasicos Usuario { get; set; }
    }
}
