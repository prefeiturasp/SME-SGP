namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class MTR_MovimentacaoDTO : MTR_Movimentacao
    {
        public bool? IsNew { get { return null; } }
        public long? alu_id { get; set; }
        public int? mtu_idAnterior { get; set; }
        public int? mtu_idAtual { get; set; }
        public int? tmv_idEntrada { get; set; }
        public int? tmv_idSaida { get; set; }
        public int? alc_idAnterior { get; set; }
        public int? alc_idAtual { get; set; }
        public int? tmo_id { get; set; }

        public ACA_AlunoDTO.Referencia aluno { get; set; }
        public MTR_MatriculaTurmaDTO.Referencia matriculaTurmaAnterior { get; set; }
        public MTR_MatriculaTurmaDTO.Referencia matriculaTurmaAtual { get; set; }
        public ACA_TipoMovimentacaoDTO.Referencia tipoMovimentacaoEntrada { get; set; }
        public ACA_TipoMovimentacaoDTO.Referencia tipoMovimentacaoSaida { get; set; }
        public ACA_AlunoCurriculoDTO.Referencia alunoCurriculoAnterior { get; set; }
        public ACA_AlunoCurriculoDTO.Referencia alunoCurriculoAtual { get; set; }
        public MTR_TipoMovimentacaoDTO.Referencia tipoMovimentacao { get; set; }

        public class Referencia
        {
            public int? mov_id { get; set; }            
        }

        public MTR_MovimentacaoDTO() {
            this.aluno = new ACA_AlunoDTO.Referencia();
            this.matriculaTurmaAnterior = new MTR_MatriculaTurmaDTO.Referencia();
            this.matriculaTurmaAtual = new MTR_MatriculaTurmaDTO.Referencia();
            this.tipoMovimentacaoEntrada = new ACA_TipoMovimentacaoDTO.Referencia();
            this.tipoMovimentacaoSaida = new ACA_TipoMovimentacaoDTO.Referencia();
            this.alunoCurriculoAnterior = new ACA_AlunoCurriculoDTO.Referencia();
            this.alunoCurriculoAtual = new ACA_AlunoCurriculoDTO.Referencia();
            this.tipoMovimentacao = new MTR_TipoMovimentacaoDTO.Referencia();
        }

        public delegate TResult FuncMovimentacao<TResult>(TResult arg1);
    }

    public struct Movimentacao
    {
        public long alu_id { get; set; }
        public int tmo_id { get; set; }
        public int? escolaSaida { get; set; }
        public long? turmaSaida { get; set; }
        public int? escolaEntrada { get; set; }
        public long? turmaEntrada { get; set; }
    }
    
}