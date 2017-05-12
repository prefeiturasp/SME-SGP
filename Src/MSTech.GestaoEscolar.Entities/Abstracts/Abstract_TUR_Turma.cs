/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    using System;
    using System.ComponentModel;
    using Data.Common.Abstracts;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public abstract class Abstract_TUR_Turma : Abstract_Entity
    {

        /// <summary>
        /// Propriedade tur_id.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public virtual long tur_id { get; set; }

        /// <summary>
        /// Propriedade esc_id.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int esc_id { get; set; }

        /// <summary>
        /// Propriedade uni_id.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int uni_id { get; set; }

        /// <summary>
        /// Propriedade tur_codigo.
        /// </summary>
        [MSValidRange(30)]
        public virtual string tur_codigo { get; set; }

        /// <summary>
        /// Propriedade tur_descricao.
        /// </summary>
        [MSValidRange(2000)]
        public virtual string tur_descricao { get; set; }

        /// <summary>
        /// Propriedade tur_vagas.
        /// </summary>
        public virtual int tur_vagas { get; set; }

        /// <summary>
        /// Propriedade tur_minimoMatriculados.
        /// </summary>
        public virtual int tur_minimoMatriculados { get; set; }

        /// <summary>
        /// Propriedade tur_duracao.
        /// </summary>
        public virtual byte tur_duracao { get; set; }

        /// <summary>
        /// Propriedade cal_id.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int cal_id { get; set; }

        /// <summary>
        /// Propriedade trn_id.
        /// </summary>
        public virtual int trn_id { get; set; }

        /// <summary>
        /// Propriedade fav_id.
        /// </summary>
        public virtual int fav_id { get; set; }

        /// <summary>
        /// Propriedade tur_docenteEspecialista.
        /// </summary>
        public virtual bool tur_docenteEspecialista { get; set; }

        /// <summary>
        /// Propriedade tur_situacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte tur_situacao { get; set; }

        /// <summary>
        /// Propriedade tur_dataCriacao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tur_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade tur_dataAlteracao.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime tur_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade tur_tipo.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual byte tur_tipo { get; set; }

        /// <summary>
        /// Propriedade tur_participaRodizio.
        /// </summary>
        public virtual bool tur_participaRodizio { get; set; }

        /// <summary>
        /// Propriedade tur_observacao.
        /// </summary>
        public virtual string tur_observacao { get; set; }

        /// <summary>
        /// Propriedade tur_codigoInep.
        /// </summary>
        [MSValidRange(10)]
        public virtual string tur_codigoInep { get; set; }

        /// <summary>
        /// Propriedade tur_dataEncerramento.
        /// </summary>
        public virtual DateTime tur_dataEncerramento { get; set; }

        /// <summary>
        /// Código da turma no EOL.
        /// </summary>
        public virtual int tur_codigoEOL { get; set; }
    }
}