namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.CoreSSO.Entities;

    public class END_EnderecoDTO : END_Endereco
    {
        public new bool? IsNew { get { return null; } }
        
        public class Referencia
        {
            public long? end_id { get; set; }
        }
    }
}
