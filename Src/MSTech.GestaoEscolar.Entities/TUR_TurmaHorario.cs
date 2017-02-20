/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;    
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class TUR_TurmaHorario : Abstract_TUR_TurmaHorario
	{
        [MSNotNullOrEmpty("Disciplina da turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }
        
        public override int thr_id { get; set; }

        [MSDefaultValue(1)]
        public override byte thr_situacao { get; set; }

        public override DateTime thr_dataCriacao { get; set; }

        public override DateTime thr_dataAlteracao { get; set; }

        [MSDefaultValue(0)]
        public override bool thr_registroExterno { get; set; }

        public byte trh_diaSemana { get; set; }

        public TimeSpan trh_horaInicio { get; set; }

        public TimeSpan trh_horaFim { get; set; }

        public byte trh_tipo { get; set; }

        public string tud_nome { get; set; }

        public int esc_id { get; set; }

        public int uni_id { get; set; }

        public long tur_id { get; set; }

        public int cal_id { get; set; }

        public int ttn_id { get; set; }
    }
}