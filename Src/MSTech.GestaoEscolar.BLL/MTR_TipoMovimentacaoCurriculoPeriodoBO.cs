/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Estrutura para salvar os cursos períodos do parâmetro de movimentação
    /// </summary>
	public struct MTR_TipoMovimentacaoCurriculoPeriodo_Cadastro
	{
	    public MTR_TipoMovimentacaoCurriculoPeriodo entCurPer;
	    public List<MTR_ParametroTipoMovimentacaoCurriculoPeriodo> listParametros;
	}

	/// <summary>
	/// MTR_TipoMovimentacaoCurriculoPeriodo Business Object 
	/// </summary>
	public class MTR_TipoMovimentacaoCurriculoPeriodoBO : BusinessBase<MTR_TipoMovimentacaoCurriculoPeriodoDAO,MTR_TipoMovimentacaoCurriculoPeriodo>
	{
        /// <summary>
        /// Retorna todos os cursos períodos do parâmetro de movimentação não excluídos logicamente
        /// </summary>                
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>     
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCursosPeriodosPorParametroMovimentacao
        (
            int tmo_id
        )
        {
            MTR_TipoMovimentacaoCurriculoPeriodoDAO dao = new MTR_TipoMovimentacaoCurriculoPeriodoDAO();
            return dao.SelectBy_tmo_id(tmo_id);
        }

        /// <summary>
        /// Verifica se o curso/período está cadastrado para a movimentação
        /// </summary>                
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCursoExistentePorParametroMovimentacaoCurso
        (
            int tmo_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            MTR_TipoMovimentacaoCurriculoPeriodoDAO dao = new MTR_TipoMovimentacaoCurriculoPeriodoDAO();
            return dao.SelectBy_tmo_id_cur_id(tmo_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna uma lista com os curso períodos cadastrados para o parâmetro de movimentação.
        /// </summary>
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MTR_TipoMovimentacaoCurriculoPeriodo> SelecionaListCursosPeriodosPorParametroMovimentacao
        (
            int tmo_id
        )
        {
            List<MTR_TipoMovimentacaoCurriculoPeriodo> lista = new List<MTR_TipoMovimentacaoCurriculoPeriodo>();

            MTR_TipoMovimentacaoCurriculoPeriodoDAO dao = new MTR_TipoMovimentacaoCurriculoPeriodoDAO();

            DataTable dt = SelecionaCursosPeriodosPorParametroMovimentacao(tmo_id);

            foreach (DataRow dr in dt.Rows)
            {
                MTR_TipoMovimentacaoCurriculoPeriodo ent = new MTR_TipoMovimentacaoCurriculoPeriodo();
                ent = dao.DataRowToEntity(dr, ent);

                lista.Add(ent);
            }

            return lista;
        }

        /// <summary>
        /// Inclui ou altera o curso período do parâmetro de movimentação
        /// </summary>
        /// <param name="entity">Entidade MTR_TipoMovimentacaoCurriculoPeriodo</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>   
        /// <returns>True/False</returns>
        public new static bool Save
        (
            MTR_TipoMovimentacaoCurriculoPeriodo entity
            , Data.Common.TalkDBTransaction banco
            , Guid ent_id
        )
        {
            MTR_TipoMovimentacaoCurriculoPeriodoDAO dao = new MTR_TipoMovimentacaoCurriculoPeriodoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {                
                if (entity.Validate())
                {
                    // Verifica se o curso foi preenchido
                    if (entity.cur_id <=0)
                        throw new ArgumentException(GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + " é obrigatório.");

                    // Verifica se o curso foi preenchido
                    if (entity.crr_id <= 0)
                        throw new ArgumentException(GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + " é obrigatório.");

                    // Verifica se o curso foi preenchido
                    if (entity.crp_id <= 0)
                        throw new ArgumentException(GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + " é obrigatório.");

                    dao.Salvar(entity);
                }
                else
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
                }

                return true;
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }        
        /// <summary>
        /// Deleta o curso período do parâmetro de movimentação
        /// </summary>
        /// <param name="entity">Entidade MTR_TipoMovimentacaoCurriculoPeriodo</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            MTR_TipoMovimentacaoCurriculoPeriodo entity
            , Data.Common.TalkDBTransaction banco
        )
        {
            MTR_TipoMovimentacaoCurriculoPeriodoDAO dao = new MTR_TipoMovimentacaoCurriculoPeriodoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                /* Verificaçao comentada em relaçao ao QOS 11999:
                
                   Por definição não existe uma regra para validar se tanto a alteração 
                   como a exclusão podem ser efetuadas para um parametro de movimentação do tipo de curso e curriculo periodo, logo
                   nã há necessidade de verificar se existem registros relacionados a operação
               
                if (GestaoEscolarUtilBO.VerificaIntegridadaChaveDupla("tmo_id", "tmp_id", entity.tmo_id.ToString(), entity.tmp_id.ToString(), "MTR_TipoMovimentacaoCurriculoPeriodo,MTR_ParametroTipoMovimentacaoCurriculoPeriodo", dao._Banco))
                {
                    ACA_Curso cur = new ACA_Curso {cur_id = entity.cur_id};
                    ACA_CursoBO.GetEntity(cur, dao._Banco);

                    ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = entity.cur_id, crr_id = entity.crr_id, crp_id = entity.crp_id};
                    ACA_CurriculoPeriodoBO.GetEntity(crp, dao._Banco);

                    throw new ValidationException("Não é possível excluir o(a) " + GestaoEscolarUtilBO.nomePadraoCurso().ToLower() + " " + cur.cur_nome + " e " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " " + crp.crp_descricao + " pois possui outros registros ligados a ele(a).");
                }*/

                //Deleta logicamente o curso período do parâmetro de movimentação
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }
	}
}