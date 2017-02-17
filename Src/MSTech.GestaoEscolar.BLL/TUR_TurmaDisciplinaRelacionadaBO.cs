/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using MSTech.Validation.Exceptions;
using System.ComponentModel;
    using MSTech.Data.Common;
    using System.Web;
    using MSTech.GestaoEscolar.BLL.Caching;

    #region Enumeradores

    /// <summary>
    /// Situação do relacionamento entre turmas disciplinas.
    /// </summary>
    public enum TUR_TurmaDisciplinaRelacionadaSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
        Inativo = 4
    }

    #endregion Enumeradores
    
    /// <summary>
	/// Description: TUR_TurmaDisciplinaRelacionada Business Object. 
	/// </summary>
	public class TUR_TurmaDisciplinaRelacionadaBO : BusinessBase<TUR_TurmaDisciplinaRelacionadaDAO, TUR_TurmaDisciplinaRelacionada>
    {
        #region Saves

        /// <summary>
        /// Salva os registros de TurmaDocente, efetua todas as validações necessárias, se precisar excluir um registro
        ///     do TurmaDocente é necessário mandar esse registro na linha com situação de excluido.
        /// </summary>
        /// <param name="listTurmaDocente">Lista de entidade TUR_TurmaDocente</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        public static void SalvarTurmaDisciplinaRelacionada(List<TUR_TurmaDisciplinaRelacionada> listTurmaDisciplinaRelacionada, Guid ent_id, string tur_id, string doc_id = "")
        {
            TUR_TurmaDisciplinaRelacionadaDAO dao = new TUR_TurmaDisciplinaRelacionadaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Salva a lista de TurmaDisciplinaRelacionada enviada
                foreach (TUR_TurmaDisciplinaRelacionada turmaDisciplinaRelacionada in listTurmaDisciplinaRelacionada)
                {
                    if (!turmaDisciplinaRelacionada.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(turmaDisciplinaRelacionada));

                    //Verifica se as datas das vigências estão válidas
                    if (turmaDisciplinaRelacionada.tdr_vigenciaFim != new DateTime() && turmaDisciplinaRelacionada.tdr_vigenciaInicio > turmaDisciplinaRelacionada.tdr_vigenciaFim)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a vigência final.");

                    if (!dao.Salvar(turmaDisciplinaRelacionada))
                        throw new ArgumentException("Erro ao salvar a atribuição de docente.");
                }

                foreach (TUR_TurmaDisciplinaRelacionada turmaDisciplinaRelacionada in listTurmaDisciplinaRelacionada)
                    TUR_TurmaDocenteBO.LimpaCache(new TUR_TurmaDocente
                {
                        tud_id = turmaDisciplinaRelacionada.tud_id,
                        doc_id = Convert.ToInt64(doc_id)
                    }, ent_id, Convert.ToInt64(tur_id));
            }
            catch (SqlException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ValidationException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        #endregion Saves
    
        /// <summary>
        /// Seleciona o historico de docencia compartilhada da disciplina.
        /// </summary>
        /// <param name="tud_id">ID disciplina compartilhada</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarHistoricoPorDisciplinaCompartilhada(long tud_id)
        {
            TUR_TurmaDisciplinaRelacionadaDAO dao = new TUR_TurmaDisciplinaRelacionadaDAO();
            return dao.SelecionarHistoricoPorDisciplinaCompartilhada(tud_id, out totalRecords);
        }

        /// <summary>
        /// Valida se ja existe aula criada na data de encerramento da vigencia ou em data posterior
        /// </summary>
        /// <param name="tud_id">ID disciplina compartilhada</param>
        /// <param name="data">Data para validar</param>
        /// <returns>true -> existe aula; false -> nao existe aula</returns>
        public static bool ValidarAulaDocenciaCompartilhada(long tud_id, DateTime data)
        {
            TUR_TurmaDisciplinaRelacionadaDAO dao = new TUR_TurmaDisciplinaRelacionadaDAO();
            return dao.ValidarAulaDocenciaCompartilhada(tud_id, data);
        }
    }
}