/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ACA_AlunoResponsavel : Abstract_Entity
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Int64 alu_id { get; set; }
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int alr_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual int tra_id { get; set; }
        public virtual Guid pes_id { get; set; }
        [MSValidRange(200)]
        public virtual string alr_profissao { get; set; }
        [MSValidRange(200)]
        public virtual string alr_empresa { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool alr_principal { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool alr_constaCertidaoNascimento { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte alr_situacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime alr_dataCriacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual DateTime alr_dataAlteracao { get; set; }
        public virtual bool alr_apenasFiliacao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool alr_moraComAluno { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool alr_omitidoFormaLei { get; set; }

        /// <summary>
        /// Tipo de responsável (1-Responsável financeiro, 2-Responsável pedagógico, 3-Responsável financeiro e pedagógico).
        /// </summary>
        public virtual byte alr_tipoResponsavel { get; set; }
    }
}