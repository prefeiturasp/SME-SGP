namespace MSTech.GestaoEscolar.Entities
{
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;

    [Serializable]
    public class DCL_ProtocoloReprocesso : Abstract_DCL_ProtocoloReprocesso
    {
        [DataObjectField(true, false, false)]
        public override int prp_seq { get; set; }
    }
}
