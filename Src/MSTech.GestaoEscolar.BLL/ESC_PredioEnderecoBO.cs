using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_PredioEnderecoBO : BusinessBase<ESC_PredioEnderecoDAO, ESC_PredioEndereco>
    {
        /// <summary>
        /// Seleciona os endereços da unidade
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade</param>
        /// <returns></returns>
        public static DataTable SelecionaEndereco(Guid ent_id, Guid uad_id)
        {
            ESC_PredioEnderecoDAO dao = new ESC_PredioEnderecoDAO();
            return dao.SelecionaEndereco(ent_id, uad_id);
        }
    }
}