using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class RHU_FuncaoBO: BusinessBase<RHU_FuncaoDAO, RHU_Funcao>
    {
        /// <summary>
        /// Retorna todas as funções não excluídas logicamente
        /// </summary>        
        /// <param name="fun_nome">Nome da função</param>
        /// <param name="fun_codigo">Código da função</param>
        /// <param name="ent_id">Entidade do usuário logado</param>       
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFuncao
        (
            string fun_nome
            , string fun_codigo
            , Guid ent_id            
        )
        {
            totalRecords = 0;

            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            return dao.SelectBy_Pesquisa(fun_nome, fun_codigo, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todas as funções não excluídas logicamente
        /// Sem paginação
        /// </summary>          
        /// <param name="ent_id">Entidade do usuário logado</param>    
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFuncao
        (            
            Guid ent_id
        )
        {
            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            return dao.SelectBy_Pesquisa(string.Empty, string.Empty, ent_id, out totalRecords);
        }

        /// <summary>
        /// Verifica se já existe uma função cadastrada com o mesmo nome na mesma entidade
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaNomeExistente
        (
            RHU_Funcao entity
        )
        {
            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            return dao.SelectBy_Nome(entity.fun_id, entity.fun_nome, entity.ent_id);
        }

        /// <summary>
        /// Verifica se já existe uma função cadastrada com o mesmo código na mesma entidade
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCodigoExistente
        (
            RHU_Funcao entity
        )
        {
            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            return dao.SelectBy_Codigo(entity.fun_id, entity.fun_codigo, entity.ent_id);
        }

        /// <summary>
        /// Consulta se existe código da função
        /// cadastrada no banco com situação ativo ou bloqueado.
        /// </summary>
        /// <param name="entity">Entidade da função(contendo o código da função)</param>
        /// <returns>True - caso encontre algum registro /False - caso não encontre</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ConsultarCodigoExistente(RHU_Funcao entity)
        {
            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            return dao.SelectBy_Codigo(entity);
        }

        /// <summary>
        /// Inclui ou altera a função
        /// </summary>
        /// <param name="entity">Entidade RHU_Funcao</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            RHU_Funcao entity
        )
        {
            RHU_FuncaoDAO funDao = new RHU_FuncaoDAO();
            funDao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (entity.Validate())
                {
                    if (VerificaNomeExistente(entity))                    
                        throw new DuplicateNameException("Já existe uma função cadastrada com este nome.");                    

                    if (VerificaCodigoExistente(entity))                    
                        throw new DuplicateNameException("Já existe uma função cadastrada com este código.");                    
                    
                    //Salva o cargo
                    funDao.Salvar(entity);

                    if (entity.IsNew)
                    {
                        //Incrementa um na integridade da entidade
                        entDao.Update_IncrementaIntegridade(entity.ent_id);
                    }
                }
                else
                {
                    throw new ValidationException(entity.PropertiesErrorList[0].Message);
                }

                return true;
            }
            catch (Exception err)
            {
                funDao._Banco.Close(err);
                entDao._Banco.Close(err);

                throw;
            }
            finally
            {                
                funDao._Banco.Close();
                entDao._Banco.Close();
            }
        }

        /// <summary>
        /// Deleta logicamente uma função
        /// </summary>
        /// <param name="entity">Entidade RHU_Funcao</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            RHU_Funcao entity
        )
        {
            RHU_FuncaoDAO dao = new RHU_FuncaoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se a função pode ser deletada
                if (GestaoEscolarUtilBO.VerificarIntegridade("fun_id", entity.fun_id.ToString(), "RHU_Funcao", dao._Banco))                
                    throw new ValidationException("Não é possível excluir a função pois possui outros registros ligados a ela.");                

                //Decrementa um na integridade da entidade
                entDao.Update_DecrementaIntegridade(entity.ent_id);

                //Deleta logicamente a função
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                entDao._Banco.Close(err);

                throw;
            }
            finally
            {
                dao._Banco.Close();
                entDao._Banco.Close();
            }
        }
    }
}
