/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ACA_Turno : Abstract_ACA_Turno
    {
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
        [MSNotNullOrEmpty("Tipo de turno é obrigatório.")]
        public override int ttn_id { get; set; }
        [MSNotNullOrEmpty("Descrição do turno é obrigatório.")]
        public override string trn_descricao { get; set; }
        [MSDefaultValue(1)]
        public override byte trn_controleTempo { get; set; }
        [MSNotNullOrEmpty("Hora inicial é obrigatório.")]
        public override TimeSpan trn_horaInicio { get; set; }
        [MSNotNullOrEmpty("Hora final é obrigatório.")]
        public override TimeSpan trn_horaFim { get; set; }
        [MSDefaultValue(1)]
        public override byte trn_situacao { get; set; }
        public override DateTime trn_dataCriacao { get; set; }
        public override DateTime trn_dataAlteracao { get; set; }
    }
}