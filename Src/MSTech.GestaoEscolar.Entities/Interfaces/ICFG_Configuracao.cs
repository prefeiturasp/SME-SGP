using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Validation;
using MSTech.Data.Common.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{
    public interface ICFG_Configuracao
    {
        [DataObjectField(true, false, false)]
        Guid cfg_id { get; set; }

        [MSValidRange(100, "Chave pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Chave é obrigatório.")]
        string cfg_chave { get; set; }

        [MSValidRange(300, "Valor pode conter até 300 caracteres.")]
        [MSNotNullOrEmpty("Valor é obrigatório.")]
        string cfg_valor { get; set; }

        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        string cfg_descricao { get; set; }

        [MSDefaultValue(1)]
        byte cfg_situacao { get; set; }

        DateTime cfg_dataCriacao { get; set; }
        DateTime cfg_dataAlteracao { get; set; }
    }
}
