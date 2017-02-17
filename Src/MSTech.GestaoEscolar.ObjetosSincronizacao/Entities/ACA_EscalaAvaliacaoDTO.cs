using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ACA_EscalaAvaliacaoDTO : ACA_EscalaAvaliacao
    {

        public ACA_EscalaAvaliacaoNumericaDTO escalaAvaliacaoNumerica { get; set; }
        public List<ACA_EscalaAvaliacaoParecerDTO> listaEscalaAvaliacaoParecer { get; set; }

        public bool? IsNew
        {
            get
            {
                return null;
            }
        }
    }
}
