using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;

namespace MSTech.GestaoEscolar.BLL
{
	/// <summary>
	/// ACA_TipoMacroCampoEletivaAluno Business Object 
	/// </summary>
	public class ACA_TipoMacroCampoEletivaAlunoBO : BusinessBase<ACA_TipoMacroCampoEletivaAlunoDAO,ACA_TipoMacroCampoEletivaAluno>
	{
        /// <summary>
        /// Retorna todos os tipos macro-campo disciplina eletiva.
        /// </summary> 
        /// <returns>DataTable contendo os tipos de macro-campo disciplina eletiva</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoMacroCampo()
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            return dao.SelectBy_Pesquisa(out totalRecords);
        }

        /// <summary>
        /// Inclui ou altera o tipo de macro-campo disciplina eletiva.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param>
        public new static bool Save(ACA_TipoMacroCampoEletivaAluno entity)
        {
            if (entity.Validate())
            {
                if (VerificaNomeExistente(entity))
                    throw new DuplicateNameException("Já existe um tipo de macro-campo cadastrado com este nome.");

                if (VerificaSiglaExistente(entity))
                    throw new DuplicateNameException("Já existe um tipo de macro-campo cadastrado com esta sigla.");

                ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Deleta logicamente o tipo de macro-campo disciplina eletiva .
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno </param>        
        /// <returns>True = deletado | False = não deletado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete(ACA_TipoMacroCampoEletivaAluno entity)
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (GestaoEscolarUtilBO.VerificarIntegridade("tea_id", entity.tea_id.ToString(), "ACA_TipoMacroCampoEletivaAluno", dao._Banco))
                {
                    throw new ValidationException("Não é possível excluir o tipo de macro-campo " + CustomResource.GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " eletivo(a) pois possui outros registros ligados a ele.");
                }
                return dao.Delete(entity);
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

        /// <summary>
        /// Verifica se já existe um tipo de macro-campo disciplina cadastrado com o mesmo nome.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoEletivaAluno</param> 
        public static bool VerificaNomeExistente(ACA_TipoMacroCampoEletivaAluno entity)
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            return dao.SelectBy_Nome(entity.tea_id, entity.tea_nome);
        }

        /// <summary>
        /// Verifica se já existe um tipo de macro-campo disciplina cadastrada com a mesma sigla.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoMacroCampoeletivaAluno</param>
        /// <returns></returns>
        public static bool VerificaSiglaExistente(ACA_TipoMacroCampoEletivaAluno entity)
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            return dao.SelectBy_Sigla(entity.tea_id, entity.tea_sigla);
        }

        /// <summary>
        /// Retorna os macro-campos não associados a disciplina eletiva.
        /// </summary>        
        /// <param name="dis_id">ID da disciplina</param>
        /// <returns>DataTable de macro-campos com os tipos de macro-campo disciplina</returns>
        public static DataTable SelecionaMacroCamposNaoAssociado(int dis_id)
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            return dao.SelectMacroCampoNaoAssociado(dis_id);
        }

        /// <summary>
        /// Retorna os macro-campos associados a disciplina eletiva.
        /// </summary>        
        /// <param name="dis_id">ID da disciplina</param>
        /// <returns>DataTable de macro-campos</returns>
        public static DataTable SelecionaMacroCamposAssociado(int dis_id)
        {
            ACA_TipoMacroCampoEletivaAlunoDAO dao = new ACA_TipoMacroCampoEletivaAlunoDAO();
            return dao.SelectMacroCampoAssociado(dis_id);
        }
	}
}