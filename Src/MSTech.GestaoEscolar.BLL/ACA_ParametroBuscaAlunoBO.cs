using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Tipos de parâmetros de busca de aluno
    /// </summary>
    public enum ACA_ParametroBuscaAlunoTipo : byte
    {
        TipoDocumentacao = 1
        ,
        Nome = 2
        ,
        NomeMae = 3
        ,
        NomePai = 4
        ,
        DataNascimento = 5
        ,
        NumeroMatricula = 6
        ,
        CertidaoNascimento = 7
    }

    public class ACA_ParametroBuscaAlunoBO : BusinessBase<ACA_ParametroBuscaAlunoDAO, ACA_ParametroBuscaAluno>
    {
        /// <summary>
        /// Retorna um datatable contendo todos os parametros de busca de aluno cadastrados no BD
        /// não excluidos logicamente.
        /// </summary>
        /// <returns>Datatable com parametros de busca de aluno</returns>
        public static DataTable GetSelect
        (
           int pba_id
           , int pba_tipo
           , Guid tdo_id           
           , bool pba_integridade
           , byte pba_situacao
           , bool paginado
           , int currentPage
           , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_ParametroBuscaAlunoDAO dao = new ACA_ParametroBuscaAlunoDAO();

            try
            {
                return dao.SelectBy_All(pba_id, pba_tipo, tdo_id, pba_integridade, pba_situacao, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe busca aluno 
        /// cadastrado no banco com situação Ativo.
        /// </summary>
        /// <param name="entity">Entidade do Parametro Busca Aluno</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaBuscaAlunoExistente(ACA_ParametroBuscaAluno entity)
        {            
            try
            {
                if (entity.pba_tipo == 1)                
                    return GetSelect(0, entity.pba_tipo, entity.tdo_id, false, Convert.ToByte(0), false, 1 , 1).Rows.Count > 0;
                
                return GetSelect(0, entity.pba_tipo, Guid.Empty, false, Convert.ToByte(0), false, 1, 1).Rows.Count > 0;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe mais de dois documentos
        /// cadastrado no banco com situação Ativo.
        /// </summary>
        /// <param name="entity">Entidade do Parametro Busca Aluno</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaBuscaAlunoQuantidade(ACA_ParametroBuscaAluno entity)
        {            
            try
            {
                if (entity.pba_tipo == 1)
                    return GetSelect(0, entity.pba_tipo, Guid.Empty, false, Convert.ToByte(0), false, 1, 1).Rows.Count >= 2;
                
                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inclui um novo parâmetro de busca de aluno    
        /// </summary>
        /// <param name="entityParametroBuscaAluno">Entidade ACA_ParametroBuscaAluno</param>                
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_ParametroBuscaAluno entityParametroBuscaAluno            
        )
        {
            ACA_ParametroBuscaAlunoDAO dao = new ACA_ParametroBuscaAlunoDAO();
            dao._Banco.Open();

            try
            {
                //Salva dados na tabela ACA_ParametroBuscaAluno
                if (entityParametroBuscaAluno.Validate())
                {
                    if (VerificaBuscaAlunoExistente(entityParametroBuscaAluno))                    
                        throw new DuplicateNameException("Já existe um parâmetro de busca de aluno cadastrado com esse tipo.");
                    
                    if (VerificaBuscaAlunoQuantidade(entityParametroBuscaAluno))                    
                        throw new ArgumentException("Só é permitido cadastrar dois parâmetros de busca de aluno com esse tipo.");
                                        
                    dao.Salvar(entityParametroBuscaAluno);
                }
                else
                {
                    throw new Validation.Exceptions.ValidationException(entityParametroBuscaAluno.PropertiesErrorList[0].Message);
                }

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }
    }
}
