using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaUsuariosSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<SYS_UsuarioDTO.Usuario> Usuarios { get; set; }

        public BuscaUsuariosSaidaDTO()
        {
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.Usuarios = new List<SYS_UsuarioDTO.Usuario>();
        }
    }
}