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
	public class ACA_EscalaAvaliacaoParecer : Abstract_ACA_EscalaAvaliacaoParecer
	{
        [MSNotNullOrEmpty("Escala de avaliação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int esa_id { get; set; }

        [DataObjectField(true, false, false)]
        public override int eap_id { get; set; }

        [MSValidRange(10, "Valor pode conter até 10 caracteres.")]
        [MSNotNullOrEmpty("Valor é obrigatório.")]
        public override string eap_valor { get; set; }
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Descrição é obrigatório.")]
        public override string eap_descricao { get; set; }
        [MSValidRange(20, "Abreviatura pode conter até 20 caracteres.")]
        public override string eap_abreviatura { get; set; }
        [MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override int eap_ordem { get; set; }
        
        [MSDefaultValue(1)]
        public override byte eap_situacao { get; set; }
        public override DateTime eap_dataCriacao { get; set; }
        public override DateTime eap_dataAlteracao { get; set; }

        public string descricao { get; set; }
    }
}