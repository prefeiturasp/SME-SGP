using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_CalendarioEscolaBO : BusinessBase<ACA_CalendarioEscolaDAO, ACA_CalendarioEscola>
    {
        /// <summary>
        /// Retorna um datatable contendo todas as escolas do calendario
        /// que não foram excluídas logicamente, filtrados por 
        /// id do calendario
        /// </summary>
        /// <param name="cal_id">Id da tabela ACA_Calendario do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com as escolas do calendario</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cal_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_CalendarioEscolaDAO dao = new ACA_CalendarioEscolaDAO();
            try
            {
                return dao.SelectBy_cal_id(cal_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica se já existe a escola e unidade escola cadastrado para o calendário
        /// e excluido logicamente
        /// filtrados por cal_id, esc_id, uni_id
        /// </summary>
        /// <param name="cal_id">Campo cal_id da tabela ACA_CalendarioEscola do bd</param>        
        /// <param name="esc_id">Campo esc_id da tabela ACA_CalendarioEscola do bd</param>        
        /// <param name="uni_id">Campo uni_id da tabela ACA_CalendarioEscola do bd</param>        
        /// <returns>true ou false</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaEscolaExistente
        (
            int cal_id
            , int esc_id
            , int uni_id
        )
        {
            ACA_CalendarioEscolaDAO dao = new ACA_CalendarioEscolaDAO();
            try
            {
                return dao.SelectBy_cal_id_esc_id_uni_id_excluido(cal_id, esc_id, uni_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inclui uma nova escola para o calendário
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioEscola</param>        
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            MSTech.GestaoEscolar.Entities.ACA_CalendarioEscola entity
            , MSTech.Data.Common.TalkDBTransaction banco
        )
        {
            try
            {
                if (entity.Validate())
                {
                    ACA_CalendarioEscolaDAO dao = new ACA_CalendarioEscolaDAO();
                    dao._Banco = banco;

                    dao.Salvar(entity);
                }
                else
                {
                    throw new MSTech.Validation.Exceptions.ValidationException(entity.PropertiesErrorList[0].Message);
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona o calendário pelo id do aluno, id da matrícula turma e id da escola.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="mtu_id"></param>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelectCalendarioByAluIdMtuIdEscId(long alu_id, int mtu_id, int esc_id)
        {
            ACA_CalendarioEscolaDAO dao = new ACA_CalendarioEscolaDAO();
            return dao.SelectCalendarioByAluIdMtuIdEscId(alu_id, mtu_id, esc_id);
        }

        
    }
}
