/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_CalendarioPeriodo : Abstract_ACA_CalendarioPeriodo
	{
        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cal_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int cap_id { get; set; }
        [MSValidRange(100, "Descrição pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string cap_descricao { get; set; }
        [MSNotNullOrEmpty("Data inicial é obrigatório.")]
        public override DateTime cap_dataInicio { get; set; }
        [MSNotNullOrEmpty("Data final é obrigatório.")]
        public override DateTime cap_dataFim { get; set; }
        [MSDefaultValue(1)]
        public override byte cap_situacao { get; set; }
        public override DateTime cap_dataCriacao { get; set; }
        public override DateTime cap_dataAlteracao { get; set; }
        /// <summary>
        /// Retorna todas as chaves primárias separadas por ; na seguinte ordem:
        /// 0 - cal_id
        /// 1 - cap_id
        /// </summary>
        public string TodasChavesPrimarias
        {
            get
            {
                return String.Concat(this.cal_id, ";", this.cap_id);
            }
        }

        public int tpc_ordem { get; set; }
	}
}