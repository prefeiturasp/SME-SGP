using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaParametrosAcademicosEntradaDTO
    {
        public Guid ent_id { get; set; }
        public string ChavesParametros { get; set; }

        public BuscaParametrosAcademicosEntradaDTO() {
            this.ent_id = new Guid();
            this.ChavesParametros = string.Empty;
        }
    }
}
