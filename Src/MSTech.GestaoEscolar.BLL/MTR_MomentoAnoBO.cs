/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Data;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// MTR_MomentoAno Business Object 
	/// </summary>
	public class MTR_MomentoAnoBO : BusinessBase<MTR_MomentoAnoDAO,MTR_MomentoAno>
	{
        /// <summary>
        /// Retorna todas as configurações de momentos cadastrados no ano informado, dentro da entidade.
        /// </summary>
        /// <param name="ent_id">Entidade - obrigatório</param>
        /// <param name="mom_ano">Ano - opcional</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_Entidade_Ano(Guid ent_id, int mom_ano)
		{
		    MTR_MomentoAnoDAO dao = new MTR_MomentoAnoDAO();
		    return dao.SelectBy_Entidade_Ano(ent_id, mom_ano);
		}

        /// <summary>
        /// Retorna o prazo para movimentação de uma entidade e ano
        /// </summary>
        /// <param name="ent_id">Entidade - obrigatório</param>
        /// <param name="mom_ano">Ano - obrigatório</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados - Opcional (null para nova conexão)</param>
        /// <returns></returns>
        public static int SelecionaPrazoMovimentacaoPorEntidadeAno(Guid ent_id, int mom_ano, TalkDBTransaction bancoGestao)
        {
            MTR_MomentoAnoDAO dao = new MTR_MomentoAnoDAO();

            if (bancoGestao != null)
                dao._Banco = bancoGestao;

            DataTable dt = dao.SelectBy_Entidade_Ano(ent_id, mom_ano);

            string sPrazoMovimentacao = string.Empty;

            if (dt.Rows.Count > 0)
                sPrazoMovimentacao =  dt.Rows[0]["mom_prazoMovimentacao"].ToString();

            return string.IsNullOrEmpty(sPrazoMovimentacao) ? 0 : Convert.ToInt32(sPrazoMovimentacao);
        }

        /// <summary>
        /// Retorna o prazo para a aprovação da movimentação retroativa, poder ser por usuários com visão Gestão
        /// </summary>
        /// <param name="ent_id">Entidade - obrigatório</param>
        /// <param name="mom_ano">Ano - obrigatório</param>
        /// <param name="bancoGestao">Conexão aberta com o banco de dados - Opcional (null para nova conexão)</param>
        /// <returns></returns>
        public static int SelecionaPrazoAprovacaoRetroativaPorEntidadeAno(Guid ent_id, int mom_ano, TalkDBTransaction bancoGestao)
        {
            MTR_MomentoAnoDAO dao = new MTR_MomentoAnoDAO();

            if (bancoGestao != null)
                dao._Banco = bancoGestao;

            DataTable dt = dao.SelectBy_Entidade_Ano(ent_id, mom_ano);

            string sPrazoAprovacaoRetroativa = string.Empty;

            if (dt.Rows.Count > 0)
                sPrazoAprovacaoRetroativa = dt.Rows[0]["mom_prazoAprovacaoRetroativa"].ToString();

            return string.IsNullOrEmpty(sPrazoAprovacaoRetroativa) ? 0 : Convert.ToInt32(sPrazoAprovacaoRetroativa);
        }
        
        public static new bool Delete(MTR_MomentoAno entity)
        {
            TalkDBTransaction banco = new MTR_MomentoAnoDAO()._Banco;
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Verifica se o registro está sendo usado em outras tabelas.
                if (GestaoEscolarUtilBO.VerificaIntegridadaChaveDupla
                    (
                        "mom_ano"
                        , "mom_id"
                        , entity.mom_ano.ToString()
                        , entity.mom_id.ToString()
                        , "MTR_MomentoCalendarioPeriodo,MTR_MomentoCongelamentoEscola,MTR_TipoMomentoAno,MTR_MomentoAno,MTR_TipoMomentoAnoCurriculo"
                        , banco
                    ))
                {
                    throw new ValidationException("Não é possível excluir a configuração dos momentos de movimentação pois possui outros registros ligados a ela.");
                }

                return Delete(entity, banco);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }
	}
}