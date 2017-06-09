using System;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TipoNivelEnsinoBO : BusinessBase<ACA_TipoNivelEnsinoDAO, ACA_TipoNivelEnsino>
    {
        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectByPesquisaTipoNivelEnsinoEscola
        (
           int esc_id,
           int uni_id
        )
        {
            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();
            return dao.SelectBy_Pesquisa_TipoNivelEnsino_Escola(esc_id, uni_id);
        }

        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente
        /// Com paginação
        /// </summary>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoNivelEnsinoPaginado
        (
            int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();
            return dao.SelectBy_Pesquisa(true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoNivelEnsino()
        {
            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente,
        /// desconsiderando o nível infantil
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoNivelEnsinoSemInfantil(Guid ent_id)
        {
            int tne_idInfantil = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, ent_id);
            DataTable dt = SelecionaTipoNivelEnsino();
            if (dt.Select(string.Format("tne_id <> {0}", tne_idInfantil)).Length > 0)
            {
                return dt.Select(string.Format("tne_id <> {0}", tne_idInfantil)).CopyToDataTable();
            }
            return new DataTable();
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de nivel de ensino
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Altera a ordem do tipo de nivel de ensino
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo de periodo do calendário</param>
        /// <param name="entityDescer">Entidade do tipo de periodo do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_TipoNivelEnsino entityDescer
            , ACA_TipoNivelEnsino entitySubir
        )
        {
            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();

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

        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAreaConhecimento()
        {
            ACA_TipoNivelEnsinoDAO dao = new ACA_TipoNivelEnsinoDAO();
            return dao.SelectBy_Pesquisa_Area_Conhecimento(false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de nível de ensino não excluídos logicamente
        /// de acordo com as atribuições do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoNivelEnsinoDocenteEventoSemInfantil(long doc_id, string eventosAbertos, Guid ent_id)
        {
            int tne_idInfantil = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, ent_id);
            DataTable dt = new ACA_TipoNivelEnsinoDAO().SelecionaTipoNivelEnsinoDocenteEvento(doc_id, eventosAbertos);
            if (dt.Select(string.Format("tne_id <> {0}", tne_idInfantil)).Length > 0)
            {
                return dt.Select(string.Format("tne_id <> {0}", tne_idInfantil)).CopyToDataTable();
            }
            return new DataTable();
        }
    }    
}