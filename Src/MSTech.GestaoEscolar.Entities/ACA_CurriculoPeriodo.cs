/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;
using System.Data;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
    /// </summary>
    [Serializable()]
    public class ACA_CurriculoPeriodo : Abstract_ACA_CurriculoPeriodo
    {        
        [DataObjectField(true, false, false)]
        public override int crp_id { get; set; }        
        
        public override int crp_ordem { get; set; }
        
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string crp_descricao { get; set; }
        
        [MSNotNullOrEmpty("Controle de horas/aulas é obrigatório.")]
        public override byte crp_controleTempo { get; set; }
        
        [MSValidRange(1, 7, "Quantidade de dias da semana que possui aula deve ser um número inteiro maior que 0 (zero) e menor ou igual a 7 (sete).")]
        [MSNotNullOrEmpty("Quantidade de dias da semana que possui aula é obrigatório.")]        
        public override byte crp_qtdeDiasSemana { get; set; }

        [MSValidRange(100, "Ciclo pode conter até 100 caracteres.")]
        public override string crp_ciclo { get; set; }
        
        [MSNotNullOrEmpty("Turmas por avaliação é obrigatório.")]
        public override bool crp_turmaAvaliacao { get; set; }
        
        [MSValidRange(100, "Nome da avaliação pode conter até 100 caracteres.")]
        public override string crp_nomeAvaliacao { get; set; }

        [MSDefaultValue(0)]
        public override bool crp_concluiNivelEnsino { get; set; }
        
        [MSDefaultValue(1)]
        public override byte crp_situacao { get; set; }
        public override DateTime crp_dataCriacao { get; set; }
        public override DateTime crp_dataAlteracao { get; set; }
        public override int tci_id { get; set; }
        public override int tcp_id { get; set; }

        public static DataTable TipoTabela_CurriculoPeriodo()
        {
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("cur_id", typeof(Int32));
                dt.Columns.Add("crr_id", typeof(Int32));
                dt.Columns.Add("crp_id", typeof(Int32));
                return dt;
            }
        }

        public DataRow EntityToDataRow_TipoTabela_CurriculoPeriodo(DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["cur_id"] = this.cur_id;
            dr["crr_id"] = this.crr_id;
            dr["crp_id"] = this.crp_id;

            return dr;
        }
    }
}