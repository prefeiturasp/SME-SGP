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
	public class ACA_TurnoEscola : Abstract_ACA_TurnoEscola
	{
        [MSNotNullOrEmpty("Turno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tes_id { get; set; }
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int esc_id { get; set; }
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int uni_id { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime tes_vigenciaInicio { get; set; }
        [MSNotNullOrEmpty("Situação é obrigatório.")]
        public override byte tes_situacao { get; set; }
        [MSNotNullOrEmpty("Data de criação é obrigatório.")]
        public override DateTime tes_dataCriacao { get; set; }
        [MSNotNullOrEmpty("Data de alteração é obrigatório.")]
        public override DateTime tes_dataAlteracao { get; set; }
	}
}