/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
			
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ORC_MatrizHabilidades : Abstract_ORC_MatrizHabilidades
	{
        [DataObjectField(true, true, false)]
        public override Int64 mat_id { get; set; }
        
        /// <summary>
        /// Data de criação da matriz de habilidades.
        /// </summary>       
        public virtual DateTime mat_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração da matriz de habilidades.
        /// </summary>       
        public virtual DateTime mat_dataAlteracao { get; set; }
               
	}
}