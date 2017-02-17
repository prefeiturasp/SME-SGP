/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
    public class MTR_MatriculaTurmaDisciplina : Abstract_MTR_MatriculaTurmaDisciplina
	{
        [DataObjectField(true, false, false)]
        public override int mtd_id { get; set; }
        [MSNotNullOrEmpty("Data de matrícula é obrigatório.")]
        public override DateTime mtd_dataMatricula { get; set; }
        
        [MSDefaultValue(1)]
        public override byte mtd_situacao { get; set; }

        public override DateTime mtd_dataCriacao { get; set; }
        public override DateTime mtd_dataAlteracao { get; set; }

        // Variáveis utilizadas na matricula nas turmas eletiva do aluno   
        public virtual int cur_id { get; set; }
        public virtual int crr_id { get; set; }
        public virtual int crp_id { get; set; }
        public virtual long tur_id { get; set; }
        public virtual string pes_nome { get; set; }        
        public virtual string tur_codigo { get; set; }

        // Variáveis utilizadas na matricula nas turmas multisseriadas
        public virtual string alc_matricula { get; set; }

        public bool apenasResultado { get; set; }

        /// <summary>
        /// Tipo de tabela utilizado para atualizar o resultado das matrículas turma disciplina.
        /// </summary>
        /// <returns></returns>
        public static DataTable TipoTabela_MatriculaTurmaDisciplinaResultado()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("mtu_id", typeof(Int32));
            dt.Columns.Add("mtd_id", typeof(Int32));
            dt.Columns.Add("mtd_avaliacao", typeof(String));
            dt.Columns.Add("mtd_relatorio", typeof(String));
            dt.Columns.Add("mtd_frequencia", typeof(Decimal));
            dt.Columns.Add("mtd_resultado", typeof(Byte));
            dt.Columns.Add("apenasResultado", typeof(Boolean));

            return dt;
        }

        /// <summary>
        /// Tipo de tabela utilizado para retornar uma lista de matriculas turma disciplina.
        /// </summary>
        /// <returns></returns>
        public static DataTable TipoTabela_AlunoMatriculaTurmaDisciplina()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("mtu_id", typeof(Int32));
            dt.Columns.Add("mtd_id", typeof(Int32));

            return dt;
        }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_MatriculaTurmaDisciplinaPeriodo.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_MatriculaTurmaDisciplinaPeriodo.</returns>
        public static DataTable TipoTabela_MatriculaTurmaDisciplinaPeriodo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tud_id", typeof(Int64));
            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("mtu_id", typeof(Int32));
            dt.Columns.Add("mtd_id", typeof(Int32));
            dt.Columns.Add("tpc_id", typeof(Int32));
            dt.Columns.Add("tur_id", typeof(Int64));
            dt.Columns.Add("cap_dataInicio", typeof(DateTime));
            dt.Columns.Add("cap_dataFim", typeof(DateTime));

            return dt;
        }
	}
}