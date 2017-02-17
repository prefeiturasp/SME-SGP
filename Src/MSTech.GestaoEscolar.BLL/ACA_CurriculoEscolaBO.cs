using System;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_CurriculoEscolaBO : BusinessBase<ACA_CurriculoEscolaDAO, ACA_CurriculoEscola>
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
           int cur_id
            , int esc_id
            , int uni_id
            , byte ttn_situacao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoEscolaDAO dao = new ACA_CurriculoEscolaDAO();
            return dao.SelectBy_cur_id_esc_id_uni_id(cur_id, esc_id, uni_id, ttn_situacao, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Verifica se o período está sendo utilizado na matricula ou na turma curriculo
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola</param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ExisteCurriculo_TurmaCurriculoMatricula
        (
            int esc_id
            , int uni_id
        )
        {
            ACA_CurriculoEscolaDAO dao = new ACA_CurriculoEscolaDAO();
            return dao.SelectBy_VerificaTurmaCurriculoMatricula(esc_id, uni_id);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregaCursosReferenteEscola
        (
            int esc_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            ACA_CurriculoEscolaDAO dao = new ACA_CurriculoEscolaDAO();
            return dao.SelectBy_esc_id(esc_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona a entidade pelos filtros
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">CrrId</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <returns>ACA_CurriculoEscola</returns>
        public static ACA_CurriculoEscola SelecionaPorCurIdCrrIdEscIdUniId
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
        )
        {
            ACA_CurriculoEscolaDAO dao = new ACA_CurriculoEscolaDAO();
            return dao.SelectBy_cur_id_crr_id_esc_id_uni_id(cur_id, crr_id, esc_id, uni_id);
        }

        /// <summary>
        /// Seleciona a entidade pelos filtros
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">CrrId</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <returns>ACA_CurriculoEscola</returns>
        public static ACA_CurriculoEscola SelecionaPorEscolaCurso(int esc_id, int uni_id, int cur_id, int crr_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_CurriculoEscolaDAO().SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id) :
                new ACA_CurriculoEscolaDAO { _Banco = banco }.SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id);
        }

        /// <summary>
        /// Consulta o currículo escola, referente a escola e ao
        /// curso passados por parâmetro e preenche a entidade currículo escola.
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoEscola.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>True = Se encontrou o currículo escola. / False = Não encontrou.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ConsultaCurriculoEscolaPorCurso
        (
            ACA_CurriculoEscola entity
            , Guid ent_id
        )
        {
            ACA_CurriculoEscolaDAO dao = new ACA_CurriculoEscolaDAO();
            return dao.SelectBy_cur_id_esc_id_uni_id_crr_id(entity, ent_id);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoEscola entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_CurriculoEscolaDAO curriculoEscolaDAO = new ACA_CurriculoEscolaDAO { _Banco = banco };
                return curriculoEscolaDAO.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}