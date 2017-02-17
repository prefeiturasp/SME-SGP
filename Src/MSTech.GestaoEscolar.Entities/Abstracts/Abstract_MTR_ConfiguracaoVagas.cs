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
    public abstract class Abstract_MTR_ConfiguracaoVagas : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cfg_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crr_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int crp_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cvg_qtdeMaxAluno { get; set; }
		public virtual int cvg_qtdeMaxAlunoDeficiente { get; set; }
		public virtual int cvg_permiteAlunoDeficiente { get; set; }
		public virtual int cvg_qtdeVagaOcupaDeficiente { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cvg_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cvg_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cvg_dataAlteracao { get; set; }

    }
}