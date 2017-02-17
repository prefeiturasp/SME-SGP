/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.Validation;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.Data;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_Turma : Abstract_TUR_Turma
	{		
		[DataObjectField(true, true, false)]
        public override Int64 tur_id { get; set; }
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int esc_id { get; set; }
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int uni_id { get; set; }
        [MSValidRange(30,"Código pode conter até 30 caracteres.")]
        public override string tur_codigo { get; set; }
        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        public override int cal_id { get; set; }
        [MSNotNullOrEmpty("Formato de avaliação é obrigatório.")]
        public override int fav_id { get; set; }
        public override int trn_id { get; set; }
        [MSDefaultValue(1)]
        public override byte tur_situacao { get; set; }
        public override DateTime tur_dataCriacao { get; set; }
        public override DateTime tur_dataAlteracao { get; set; }

        /// <summary>
        /// Indica se o fechamento da turma é automático.
        /// </summary>
        public bool fav_fechamentoAutomatico { get; set; }

        /// <summary>
        /// Tipo tabela TipoTabela_Turma
        /// </summary>
        /// <returns></returns>
        public static DataTable TipoTabela_Turma()
        {
            DataTable dtTurma = new DataTable();
            dtTurma.Columns.Add("tur_id", typeof(Int64));
            return dtTurma;
        }
    }
}