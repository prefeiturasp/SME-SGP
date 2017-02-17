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
    public abstract class Abstract_ACA_Aluno : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid pes_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }
		public virtual string alu_observacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool alu_dadosIncompletos { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool alu_historicoEscolaIncompleto { get; set; }
		public virtual int rlg_id { get; set; }
		public virtual byte alu_meioTransporte { get; set; }
		public virtual byte alu_tempoDeslocamento { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool alu_regressaSozinho { get; set; }
        public virtual DateTime alu_dataCadastroFisico { get; set; }
        [MSValidRange(100)]
        public virtual string alu_responsavelInfo { get; set; }
        [MSValidRange(20)]
        public virtual string alu_responsavelInfoDoc { get; set; }
        [MSValidRange(20)]
        public virtual string alu_responsavelInfoOrgaoEmissao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte alu_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime alu_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
        public virtual DateTime alu_dataAlteracao { get; set; }
        public virtual bool alu_aulaReligiao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool alu_gemeo { get; set; }
        public virtual bool alu_possuiEmail { get; set; }
        public virtual Guid end_id { get; set; }
        public virtual string alu_observacaoSituacao { get; set; }
        public virtual string alu_codigoExterno { get; set; }
        [MSValidRange(20)]
        public virtual string alu_protocoloExcedente { get; set; }
        public virtual bool alu_possuiInformacaoSigilosa { get; set; }
        public virtual bool alu_bloqueioBoletimOnline { get; set; }
    }
}