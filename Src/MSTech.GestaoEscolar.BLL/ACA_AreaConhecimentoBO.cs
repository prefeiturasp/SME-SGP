/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.ComponentModel;
    using MSTech.Validation.Exceptions;
    using MSTech.Data.Common;

    public enum TipoBaseGeral : byte
    {
        [StringValue("Resolução")]
        Resolucao = 1,
        [StringValue("Decreto")]
        Decreto = 2,
    }

    public enum TipoBase : byte
    {
        [StringValue("Nacional comum")]
        Nacional = 1,
        [StringValue("Parte diversificada")]
        Diversificada = 2,
    }

	/// <summary>
	/// Description: ACA_AreaConhecimento Business Object. 
	/// </summary>
	public class ACA_AreaConhecimentoBO : BusinessBase<ACA_AreaConhecimentoDAO, ACA_AreaConhecimento>
    {
        /// <summary>
        /// Gera os numeros das ordens para os tipos disciplinas se escolher o parametro academico CONTROLAR_ORDEM_DISCIPLINAS
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static void OrdenaTiposDisciplinas()
        {
            ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();
            dao.Ordena_AreaConhecimento_aco_Ordem();
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de disciplina
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de disciplina
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificarNomeExistente(ACA_AreaConhecimento entity)
        {
            ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();
            return dao.VerificaNomeExistente(entity);
        }

        /// <summary>
        /// Altera a ordem do tipo de disciplina
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo disciplina</param>
        /// <param name="entityDescer">Entidade do tipo disciplina</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_AreaConhecimento entityDescer
            , ACA_AreaConhecimento entitySubir
        )
        {
            ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();

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
        /// Inclui ou altera a area de conhecimento
        /// </summary>
        /// <param name="entity">Entidade ACA_AreaConhecimento</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save(ACA_AreaConhecimento entity, TalkDBTransaction banco = null)
        {
            if (!entity.Validate())
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            if (VerificarNomeExistente(entity))
                throw new ValidationException(string.Format("Área de conhecimento '{0}' já existente!", entity.aco_nome));

            if (entity.IsNew)
                entity.aco_ordem = SelecionaMaiorOrdem() + 1;

            ACA_AreaConhecimentoDAO dao = banco == null ? new ACA_AreaConhecimentoDAO() : new ACA_AreaConhecimentoDAO() { _Banco = banco};          
            
            return dao.Salvar(entity);
        }

        /// <summary>
        /// Retorna todas as áreas de conhecimento não excluídas logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivas()
        {
            ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();
            return dao.SelecionaAtivas();
        }
    }
}