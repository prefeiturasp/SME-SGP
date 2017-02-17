using System;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TipoJustificativaFaltaBO : BusinessBase<ACA_TipoJustificativaFaltaDAO, ACA_TipoJustificativaFalta> 
    {
        /// <summary>
        /// Situações da tipos de justificativas de faltas 
        /// </summary>
        public enum ACA_TipoJustificativaFaltaSituacao : byte
        {
            Ativo = 1
            ,
            Excluido = 3
            ,
            Inativo = 4
        }

        /// <summary>
        /// Retorna todos os tipos de justificativas de faltas não excluídos logicamente
        /// Com paginação
        /// </summary>
        /// <param name="paginado"></param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="situacao"></param>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoJustificativaFalta
        (
             bool paginado
            , int currentPage
            , int pageSize
            , int situacao
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_TipoJustificativaFaltaDAO dao = new ACA_TipoJustificativaFaltaDAO();
            return dao.SelectBy_Pesquisa(paginado, currentPage / pageSize, pageSize, situacao, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de justificativas de faltas ativos.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaTiposJustificativaFalta()                               
        {
            return SelecionaTipoJustificativaFalta(false, 0, 0, 1);
        }

        /// <summary>
        /// Retorna um list da entidade ACA_TipoJustificativaFalta com todos os tipos de justificativas de faltas ativos.
        /// </summary>
        /// <returns></returns>
        public static List<ACA_TipoJustificativaFalta> TiposJustificativaFalta()
        {
            ACA_TipoJustificativaFaltaDAO dao = new ACA_TipoJustificativaFaltaDAO();

            List<ACA_TipoJustificativaFalta> lt = SelecionaTiposJustificativaFalta().Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new ACA_TipoJustificativaFalta())).ToList();

            return lt;
        }

        /// <summary>
        /// Verifica se já existe um tipo de justificativa de falta cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoNivelEnsino</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaNomeExistente
        (
            ACA_TipoJustificativaFalta entity
        )
        {
           ACA_TipoJustificativaFaltaDAO dao = new ACA_TipoJustificativaFaltaDAO();
           return dao.SelectBy_Nome(entity.tjf_id, entity.tjf_nome);
        }

        /// <summary>
        /// Inclui ou altera o tipo de justificativa de falta
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoJustificativaFalta</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_TipoJustificativaFalta entity
        )
        {
            if (entity.Validate())
            {
                if (VerificaNomeExistente(entity))
                    throw new DuplicateNameException("Já existe um tipo de justificativa de falta cadastrada com este nome.");

                ACA_TipoJustificativaFaltaDAO dao = new ACA_TipoJustificativaFaltaDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

         /// <summary>
        /// Deleta logicamente um tipo de justificativa de falta
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoJustificativaFalta</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_TipoJustificativaFalta entity
        )
        {
            ACA_TipoJustificativaFaltaDAO dao = new ACA_TipoJustificativaFaltaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o tipo de justificativa de falta
                if (GestaoEscolarUtilBO.VerificarIntegridade
                  (
                    "tjf_id"
                    , entity.tjf_id.ToString()
                    , "ACA_TipoJustificativaFalta"
                    , dao._Banco
                  ))
                {
                    throw new ValidationException("Não é possível excluir o tipo de justificativa de falta pois possui outros registros ligados a ele.");
                }

                //Deleta logicamente o tipo de justificativa de falta
                dao.Delete(entity);

                return true;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }
    }
}
