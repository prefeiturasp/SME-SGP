namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ACA_AlunoCurriculoDTO : ACA_AlunoCurriculo
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? alc_id { get; set; }
        }
    }
}