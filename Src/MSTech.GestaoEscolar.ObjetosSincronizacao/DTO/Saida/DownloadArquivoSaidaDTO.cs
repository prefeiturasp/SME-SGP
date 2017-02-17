using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class DownloadArquivoSaidaDTO
    {

        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public String Arq_data { get; set; }

        public DownloadArquivoSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Arq_data = "";
        }
    }
}
