using System;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.ComponentModel;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações do aluno
    /// </summary>
    public enum ACA_AlunoEscolaOrigemTipoCadastro : byte
    {
        MovimentacaoOrigem = 1
        ,
        MovimentacaoDestino = 2
    }

    /// <summary>
    /// Estrutura para salvar a escola de origem
    /// </summary>
    [Serializable]
    public struct ACA_AlunoEscolaOrigem_Cadastro
    {
        public ACA_AlunoEscolaOrigem entEscolaOrigem;
        public END_Endereco entEndereco;
    }

    public class ACA_AlunoEscolaOrigemBO : BusinessBase<ACA_AlunoEscolaOrigemDAO, ACA_AlunoEscolaOrigem>
    {
        /// <summary>
        /// Retorna todas as escolas de origem ativas  
        /// </summary>
        /// <param name="tre_id">ID do tipo de rede de ensino</param>
        /// <param name="eco_nome">Nome da escola de origem</param>        
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable</returns>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Select_EscolasPor_RedeEnsino_Nome
        (
            int tre_id
            , string eco_nome
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoEscolaOrigemDAO dao = new ACA_AlunoEscolaOrigemDAO();
            return dao.Select_EscolasPor_RedeEnsino_Nome(tre_id, eco_nome, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as escolas de origem
        /// que não foram excluídas logicamente, filtradas por 
        ///	eco_id, eco_nome, eco_situacao             
        /// </summary>
        /// <param name="eco_id">ID da escola de origem</param>
        /// <param name="tre_id">ID do tipo de rede de ensino</param>
        /// <param name="eco_nome">Nome da escola de origem</param>        
        /// <param name="eco_situacao">Situacao da escola de origem</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as escola de origem</returns>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            long eco_id
            , int tre_id
            , string eco_nome
            , byte eco_situacao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoEscolaOrigemDAO dao = new ACA_AlunoEscolaOrigemDAO();
            return dao.SelectBy_All(eco_id, tre_id, eco_nome, eco_situacao, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Método padrão para salvar as escolas de origem pelo UserControl de movimentações
        /// </summary>
        /// <param name="cadEscolaOrigem">Estrutura ACA_AlunoEscolaOrigem_Cadastro</param>        
        /// <param name="bancoCore">Conexão aberta com o banco de dados do CoreSSO</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados do GestaoEscolar</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Salvar
        (
            ACA_AlunoEscolaOrigem_Cadastro cadEscolaOrigem
            , TalkDBTransaction bancoCore
            , TalkDBTransaction bancoGestao
        )
        {
            ACA_AlunoEscolaOrigemDAO aluDao = new ACA_AlunoEscolaOrigemDAO();
            END_EnderecoDAO endDao = new END_EnderecoDAO();

            if (bancoGestao == null)
            {
                aluDao._Banco.Open(IsolationLevel.ReadCommitted);
                endDao._Banco.Open(IsolationLevel.ReadCommitted);
            }
            else
            {
                aluDao._Banco = bancoGestao;
                endDao._Banco = bancoCore;
            }

            try
            {
                if (!string.IsNullOrEmpty(cadEscolaOrigem.entEscolaOrigem.eco_nome))
                {
                    // Salva o endereço da escola se não existir no banco e se foi informado pelo usuário
                    if (cadEscolaOrigem.entEndereco.IsNew && !string.IsNullOrEmpty(cadEscolaOrigem.entEndereco.end_logradouro))
                        END_EnderecoBO.Save(cadEscolaOrigem.entEndereco, Guid.Empty, bancoCore);

                    // Atualiza o id do endereço na tabela de escola de origem se informado pelo usuário
                    if (!string.IsNullOrEmpty(cadEscolaOrigem.entEndereco.end_logradouro))
                        cadEscolaOrigem.entEscolaOrigem.end_id = cadEscolaOrigem.entEndereco.end_id;

                    // Salva a escola de origem
                    Save(cadEscolaOrigem.entEscolaOrigem, bancoGestao);
                }

                return true;
            }
            catch (Exception err)
            {
                if (bancoGestao == null)
                {
                    aluDao._Banco.Close(err);
                    endDao._Banco.Close(err);
                }

                throw;
            }
            finally
            {
                if (bancoGestao == null)
                {
                    aluDao._Banco.Close();
                    endDao._Banco.Close();
                }
            }
        }

        /// <summary>
        /// Incluir uma nova escola fora da rede
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoEscolaOrigem</param>        
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_AlunoEscolaOrigem entity
            , TalkDBTransaction banco
        )
        {
            // Verifica se os dados da pessoa serão sempre salvos em maiúsculo.
            string sSalvarMaiusculo = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
            bool Salvar_Sempre_Maiusculo = !string.IsNullOrEmpty(sSalvarMaiusculo) && Convert.ToBoolean(sSalvarMaiusculo);

            if (Salvar_Sempre_Maiusculo)
            {
                if (!string.IsNullOrEmpty(entity.eco_nome))
                {
                    entity.eco_nome = entity.eco_nome.ToUpper();
                }
            }

            if (entity.Validate())
            {
                ACA_AlunoEscolaOrigemDAO dao = new ACA_AlunoEscolaOrigemDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}
