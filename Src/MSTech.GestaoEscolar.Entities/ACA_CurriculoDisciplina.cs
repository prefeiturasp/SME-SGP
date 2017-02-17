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
	public class ACA_CurriculoDisciplina : Abstract_ACA_CurriculoDisciplina
	{
        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override byte crd_tipo { get; set; }
        [MSDefaultValue(1)]
        public override byte crd_situacao { get; set; }        
        public override DateTime crd_dataCriacao { get; set; }
        public override DateTime crd_dataAlteracao { get; set; }   
   
        //Utilizado no cadastro de disciplina eletiva do aluno
        public virtual string crp_descricao { get; set; }

        //Utilizado no cadastro de disciplina multisseriada
        public virtual string cur_nome { get; set; }
        public virtual string dis_nome { get; set; }
        public virtual int tds_id { get; set; }
	}
}