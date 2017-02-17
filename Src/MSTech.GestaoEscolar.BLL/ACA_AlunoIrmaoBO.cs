using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_AlunoIrmaoBO : BusinessBase<ACA_AlunoIrmaoDAO, ACA_AlunoIrmao>    
    {
        /// <summary>
        /// Retorna um datatable contendo todos os irmãos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecord">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Irmãos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable GetSelectBy_alu_id
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            try
            {
                ACA_AlunoIrmaoDAO dao = new ACA_AlunoIrmaoDAO();
                return dao.SelectBy_alu_id(alu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona o id do último irmão cadastrado para o aluno + 1
        /// se não houver irmão cadastrado para o aluno retorna 1
        /// filtrados por alu_id
        /// </summary>
        /// <param name="ali_id">Campo ali_id da tabela ACA_AlunoIrmao do bd</param>                
        /// <returns>ali_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimoIrmaoCadastrado
        (
            long alu_id            
        )
        {
            ACA_AlunoIrmaoDAO dao = new ACA_AlunoIrmaoDAO();
            try
            {
                return dao.SelectBy_alu_id_top_one(alu_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona o id do irmão pelo codigo do aluno irmão        
        /// filtrados por alu_id, alu_idIrmao
        /// </summary>
        /// <param name="alu_id">Campo alu_id da tabela ACA_AlunoIrmao do bd</param>                
        /// <param name="alu_idIrmao">Campo alu_idIrmao da tabela ACA_AlunoIrmao do bd</param>                
        /// <returns>pes_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaCodigoIrmao
        (
            long alu_id
            , long alu_idIrmao
        )
        {
            ACA_AlunoIrmaoDAO dao = new ACA_AlunoIrmaoDAO();
            try
            {
                return dao.Select_ali_id_By_alu_id(alu_id, alu_idIrmao);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inclui um novo irmão para o aluno
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoIrmao</param>        
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            MSTech.GestaoEscolar.Entities.ACA_AlunoIrmao entityAlunoIrmao
            , MSTech.Data.Common.TalkDBTransaction banco
        )
        {

            if (entityAlunoIrmao.Validate())
            {
                ACA_AlunoIrmaoDAO alurespDAO = new ACA_AlunoIrmaoDAO();
                alurespDAO._Banco = banco;

                return alurespDAO.Salvar(entityAlunoIrmao);
            }
            else
            {
                throw new MSTech.Validation.Exceptions.ValidationException(entityAlunoIrmao.PropertiesErrorList[0].Message);
            }
        }
    }    
}