using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class RHU_ColaboradorDTO : RHU_Colaborador
    {
        public PES_PessoaDTO pessoa { get; set; }
        public ACA_Docente docente { get; set; }
        public List<PES_PessoaDocumento> documentos { get; set; }
        public List<RHU_ColaboradorCargoDTO> colaboradorCargo { get; set; }
        public List<RHU_ColaboradorFuncao> colaboradorFuncao { get; set; }

        public new bool? IsNew { get { return null; } }
    }

    public class RHU_ColaboradorCargoDTO : RHU_ColaboradorCargo
    {
        public List<RHU_ColaboradorCargoDisciplina> colaboradorCargoDisciplina { get; set; }
    }
}
