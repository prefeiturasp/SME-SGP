/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using GestaoEscolar.Entities;
    using GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using MSTech.Validation.Exceptions;
    using System;

    /// <summary>
    /// Description: TUR_TurmaAberturaAnosAnteriores Business Object. 
    /// </summary>
    public class TUR_TurmaAberturaAnosAnterioresBO : BusinessBase<TUR_TurmaAberturaAnosAnterioresDAO, TUR_TurmaAberturaAnosAnteriores>
	{
        #region Enumeradores

        /// <summary>
        /// Status da abertura dos anos anteriores
        /// </summary>
        public enum EnumTurmaAberturaAnosAnterioresStatus : byte
        {
            [Description("Aguardando execução")]
            AguardandoExecucao = 1,

            [Description("Em processamento de abertura")]
            EmProcessamentoAbertura = 2,

            [Description("Em processamento de fechamento")]
            EmProcessamentoFechamento = 3,

            [Description("Aberto")]
            Aberto = 4,

            [Description("Encerrado")]
            Encerrado = 5
        }

        /// <summary>
        /// Situações da abertura dos anos anteriores
        /// </summary>
        public enum EnumTurmaAberturaAnosAnterioresSituacao : byte
        {
            Ativo = 1
            ,
            Excluido = 3
            ,
            Inativo = 4
        }

        #endregion

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAberturasAnosLetivosBy_AnoDreEscStatus
        (
            int tab_ano,
            Guid uad_idSuperior,
            int uni_id,
            int esc_id,
            int tab_status,
            int currentPage,
            int pageSize
        )
        {
            TUR_TurmaAberturaAnosAnterioresDAO dao = new TUR_TurmaAberturaAnosAnterioresDAO();
            return dao.SelecionaAberturasAnosLetivosBy_AnoDreEscStatus(tab_ano, uad_idSuperior, uni_id, esc_id, tab_status, true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Inclui ou altera o tipo de justificativa para exclusão de aulas
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoJustificativaExclusaoAulas</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            TUR_TurmaAberturaAnosAnteriores entity
        )
        {
            if (entity.Validate())
            {
                if (VerificaRegistroExistente(entity))
                    throw new DuplicateNameException("Já existe um registro de abertura de turmas com essas informações.");

                TUR_TurmaAberturaAnosAnterioresDAO dao = new TUR_TurmaAberturaAnosAnterioresDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Verifica se já existe um tipo de justificativa para exclusão de aulas cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoNivelEnsino</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaRegistroExistente
        (
            TUR_TurmaAberturaAnosAnteriores entity
        )
        {
            TUR_TurmaAberturaAnosAnterioresDAO dao = new TUR_TurmaAberturaAnosAnterioresDAO();
            return dao.VerificaRegistroExistente(entity.tab_id, entity.tab_ano, entity.uad_idSuperior, entity.uni_id, entity.esc_id, entity.tab_dataInicio, entity.tab_dataFim);
        }
    }
}