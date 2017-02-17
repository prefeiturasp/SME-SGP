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
	public class ESC_EscolaOrgaoSupervisao : Abstract_ESC_EscolaOrgaoSupervisao
	{
        [DataObjectField(true, false, false)]
        public override int eos_id { get; set; }
        [MSValidRange(200, "Nome deve conter até 200 caracteres.")]
        public override string eos_nome { get; set; }
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
        [MSDefaultValue(1)]
        public override byte eos_situacao { get; set; }        
        public override DateTime eos_dataCriacao { get; set; }
        public override DateTime eos_dataAlteracao { get; set; }

        /// <summary>
        /// Nome da Unidade Administrativa
        /// </summary>
        public string uad_nome { get; set; }
	}
}