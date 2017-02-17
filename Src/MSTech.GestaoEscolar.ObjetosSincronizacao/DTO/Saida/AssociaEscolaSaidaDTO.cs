using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class AssociaEscolaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }

        public AssociaEscolaSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
        }
    }
}