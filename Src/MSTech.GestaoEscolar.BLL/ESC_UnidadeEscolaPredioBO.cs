using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_UnidadeEscolaPredioBO : BusinessBase<ESC_UnidadeEscolaPredioDAO, ESC_UnidadeEscolaPredio>        
    {
        /// <summary>
        /// Verifica o código do predio principal cadastrado e vigente para a unidade escola
        /// filtradas por escola, unidade escola.
        /// </summary>        
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade escolar</param>
        /// <returns>prd_id</returns>
        public static int VerificaPredioPrincipalAtivo(int esc_id, int uni_id)
        {
            ESC_UnidadeEscolaPredioDAO dao = new ESC_UnidadeEscolaPredioDAO();
            return dao.SelectBy_esc_id_uni_id(esc_id, uni_id);
        }

        /// <summary>
        /// Consulta código do predio principal cadastrado e vigente para a unidade escola,
        /// e carrega a entidade unidade escola prédio.
        /// </summary>        
        /// <param name="entity">Entidade ESC_UnidadeEscolaPredio</param>
        /// <returns>True|False</returns>
        public static bool ConsultaPredioPrincipalAtivo(ESC_UnidadeEscolaPredio entity)
        {
            ESC_UnidadeEscolaPredioDAO dao = new ESC_UnidadeEscolaPredioDAO();
            return dao.SelectBy_esc_id_uni_id(entity);
        }
    }
}


