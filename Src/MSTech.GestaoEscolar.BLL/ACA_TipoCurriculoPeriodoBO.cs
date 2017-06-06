/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Validation.Exceptions;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Description: ACA_TipoCurriculoPeriodo Business Object. 
    /// </summary>
    public class ACA_TipoCurriculoPeriodoBO : BusinessBase<ACA_TipoCurriculoPeriodoDAO, ACA_TipoCurriculoPeriodo>
    {
        /// <summary>
        /// Inclui ou altera o tipo de curriculo periodo 
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoCurriculoPeriodo</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SalvarCurriculoPeriodo
        (
        ACA_TipoCurriculoPeriodo entity
        )
        {
            ACA_TipoCurriculoPeriodoDAO dao = new ACA_TipoCurriculoPeriodoDAO();

            if (entity.Validate())
            {
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }


        /// <summary>
        /// Retorna todos os tipos de currículo período pelo ano letivo e tipo nivel ensino
        /// </summary>        
        /// <param name="chp_anoLetivo">Ano letivo</param>
        /// <param name="tne_id">ID do tipo nível de ensino</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorAnoLetivoNivelEnsino(int chp_anoLetivo, int tne_id)
        {
            ACA_TipoCurriculoPeriodoDAO dao = new ACA_TipoCurriculoPeriodoDAO();
            return dao.SelecionaPorAnoLetivoNivelEnsino(chp_anoLetivo, tne_id);
        }

        /// <summary>
        /// Retorna os tipos de ciclo de aprendizagem ativos
        /// </summary>
        /// <param name="tne_id">Tipo nivel de ensino</param>
        /// <param name="tme_id">Tipo modalidade de ensino</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectByPesquisa
        (
            int tne_id
            , int tme_id
         )
        {
            ACA_TipoCurriculoPeriodoDAO dao = new ACA_TipoCurriculoPeriodoDAO();
            return dao.SelectByPesquisa(tne_id, tme_id, out totalRecords);
        }

        /// <summary>
        /// Retorna os tipos de currículo período ativos por nível e modalidade de ensino
        /// de acordo com as atribuições do docente.
        /// </summary>
        /// <param name="tne_id">Tipo nivel de ensino</param>
        /// <param name="tme_id">Tipo modalidade de ensino</param>
        /// <param name="doc_id">ID do docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoCurriculoPeriodoDocente
        (
            int tne_id
            , int tme_id
            , long doc_id
         )
        {
            return new ACA_TipoCurriculoPeriodoDAO().SelecionaTipoCurriculoPeriodoDocente(tne_id, tme_id, doc_id);
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de curriculo período
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_TipoCurriculoPeriodoDAO dao = new ACA_TipoCurriculoPeriodoDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Altera a ordem do tipo curriculo periodo tci_ordem
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo de curriculo periodo</param>
        /// <param name="entityDescer">Entidade do tipo de curriculo periodo</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_TipoCurriculoPeriodo entityDescer
            , ACA_TipoCurriculoPeriodo entitySubir
        )
        {
            ACA_TipoCurriculoPeriodoDAO dao = new ACA_TipoCurriculoPeriodoDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }

    }
}