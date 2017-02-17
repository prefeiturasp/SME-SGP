/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações da observação do histórico escolar
    /// </summary>
    public enum ACA_HistoricoObservacaoPadraoSituacao : byte
    {
        Ativo = 1
        ,
        Bloqueado = 2
        ,
        Excluido = 3
    }

    /// <summary>
    /// Tipos de observação do histórico escolar
    /// </summary>
    public enum ACA_HistoricoObservacaoPadraoTipo : byte
    {
        Observacao = 1
        ,
        CriterioAvaliacao = 2
        ,
        ObservacaoComplementar = 3
        ,
        CertificadoConclusaoCurso = 4
    }

    /// <summary>
    /// ACA_HistoricoObservacaoPadrao Business Object 
    /// </summary>
    public class ACA_HistoricoObservacaoPadraoBO : BusinessBase<ACA_HistoricoObservacaoPadraoDAO, ACA_HistoricoObservacaoPadrao>
    {
        /// <summary>
        /// Retorna um datatable contendo as observações não excluídas logicamente.
        /// Com parâmetros para o uso em Grid.
        /// </summary>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns>Datatable contendo as observações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosGrid
        (
            bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_HistoricoObservacaoPadraoDAO dao = new ACA_HistoricoObservacaoPadraoDAO();
            try
            {
                return dao.SelectBy_All(paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo as observações não excluídas logicamente.
        /// </summary>
        /// <returns>Datatable contendo as observações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodos()
        {
            try
            {
                ACA_HistoricoObservacaoPadraoDAO dao = new ACA_HistoricoObservacaoPadraoDAO();
                try
                {
                    return dao.SelecionaTodos();
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Retorna um datatable contendo as observações não excluídas logicamente.
        /// Filtrando por tipo de observação.
        /// </summary>
        /// <param name="hop_tipo">Tipo de observação</param>
        /// <returns>Datatable contendo as observações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTipo
        (
            int hop_tipo
        )
        {
            try
            {
                ACA_HistoricoObservacaoPadraoDAO dao = new ACA_HistoricoObservacaoPadraoDAO();
                try
                {
                    return dao.SelecionaPorTipo(hop_tipo);
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Salva os dados da observação.
        /// </summary>
        /// <param name="entityHistoricoObservacaoPadrao">Entidade ACA_HistoricoObservacaoPadrao</param>
        /// <returns>Retorna true se salvou com sucesso.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Salvar
        (
            ACA_HistoricoObservacaoPadrao entityHistoricoObservacaoPadrao
        )
        {
            try
            {
                if (VerificaSeJaExisteNome(entityHistoricoObservacaoPadrao.hop_id, entityHistoricoObservacaoPadrao.hop_nome))
                {
                    throw new DuplicateNameException("Já existe uma observação cadastrada com esse nome.");
                }

                Save(entityHistoricoObservacaoPadrao);

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Verifica se existe outra observação com o mesmo nome. Certifica-se de que situação
        /// seja diferente de inativo e não seja o próprio registro.
        /// </summary>
        /// <param name="hop_id">Id da observação</param>
        /// <param name="hop_nome">Nome da observação</param>
        /// <returns>Retorna true se já existe uma observação com o mesmo nome</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaSeJaExisteNome
        (
            int hop_id
            , string hop_nome
        )
        {
            ACA_HistoricoObservacaoPadraoDAO dao = new ACA_HistoricoObservacaoPadraoDAO();
            try
            {
                return dao.VerificaSeJaExisteNome(hop_id, hop_nome);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deleta logicamente uma observação do histórico escolar
        /// </summary>
        /// <param name="entity">Entidade ACA_HistoricoObservacaoPadrao</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_HistoricoObservacaoPadrao entity 
        )
        {
            ACA_HistoricoObservacaoPadraoDAO dao = new ACA_HistoricoObservacaoPadraoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);
            
            try
            {
                //Verifica se o tipo de movimentação pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("hop_id", entity.hop_id.ToString(), "ACA_HistoricoObservacaoPadrao", dao._Banco))
                    throw new ValidationException("Não é possível excluir a observação do histórico escolar pois possui outros registros ligados a ela.");

                if (MTR_ParametroTipoMovimentacaoBO.VerificaValorParametroTipoMovimentacao(entity.hop_id.ToString(), ChaveParametroTipoMovimentacao.ObservacaoPadraoHistorico))
                    throw new ValidationException("Não é possível excluir a observação do histórico escolar pois possui outros registros ligados a ela.");

                //Deleta logicamente a observação do histórico escolar
                dao.Delete(entity);
               
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