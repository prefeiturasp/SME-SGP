using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Data.Common;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// DCL_Requisicao Business Object 
    /// </summary>
    public class DCL_RequisicaoBO : BusinessBase<DCL_RequisicaoDAO, DCL_Requisicao>
    {
        public enum eDCL_Requisicao
        {
            BuscaEscola = 1,
            AssociaçãoEscola = 2,
            DadosIniciais = 3,
            BuscaTurmas = 4,
            BuscaAlunosTurma = 5,
            BuscaUsuários = 6,
            BuscaStatusProtocolos = 7,
            SincronizaçãoDiárioClasse = 8
        }


        [Obsolete("Classe apenas para consulta e Enum", true)]
        public new static bool Save(DCL_Requisicao entity)
        {
            return false;
        }

        [Obsolete("Classe apenas para consulta e Enum", true)]
        public new static bool Save(DCL_Requisicao entity, TalkDBTransaction banco)
        {
            return false;
        }

        [Obsolete("Classe apenas para consulta e Enum", true)]
        public new static bool Delete(DCL_Requisicao entity)
        {
            return false;
        }

        [Obsolete("Classe apenas para consulta e Enum", true)]
        public new static bool Delete(DCL_Requisicao entity, TalkDBTransaction banco)
        {
            return false;
        }

        /// <summary>
        /// Busca requisições que podem ter agendamentos
        /// </summary>
        /// <param name="currentPage">Pagina atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>DataTable contendo requisições</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscarBy_permiteAgenda(int currentPage, int pageSize)
        {
            DCL_RequisicaoDAO dao = new DCL_RequisicaoDAO();
            return dao.SelectBy_permiteAgenda(currentPage, pageSize, out totalRecords);
        }
    }
}
