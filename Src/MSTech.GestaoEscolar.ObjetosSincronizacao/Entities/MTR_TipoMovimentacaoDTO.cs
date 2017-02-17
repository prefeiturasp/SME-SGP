using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class MTR_TipoMovimentacaoDTO : MTR_TipoMovimentacao
    {
        public bool? IsNew { get { return null; } }
        public int? tmv_idSaida { get; set; }
        public int? tmv_idEntrada { get; set; }

        public ACA_TipoMovimentacaoDTO.Referencia tipoMovimentacaoSaida { get; set; }
        public ACA_TipoMovimentacaoDTO.Referencia tipoMovimentacaoEntrada { get; set; }

        public class Referencia
        {
            public int? tmo_id { get; set; }
        }  
    }
}
