/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
	using MSTech.Business.Common;
    using MSTech.Data.Common;    
    using MSTech.GestaoEscolar.CustomResourceProviders;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;	
    using MSTech.Validation.Exceptions;
    using ObjetosSincronizacao.Entities;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.ComponentModel;
	
	/// <summary>
	/// Description: TUR_TurmaHorario Business Object. 
	/// </summary>
	public class TUR_TurmaHorarioBO : BusinessBase<TUR_TurmaHorarioDAO, TUR_TurmaHorario>
	{
        #region Métodos de validação

        /// <summary>
        /// Verifica se existe um registro de turma horário cadastrado para o turno
        /// </summary>
        /// <param name="trn_id">ID do turno.</param>
        /// <returns></returns>
        public static bool VerificaExistePorTurno(int trn_id)
        {
            return new TUR_TurmaHorarioDAO().VerificaExistePorTurno(trn_id);
        }

        /// <summary>
        /// Valida a carga horária ao salvar a lista de turmaHorario.
        /// </summary>
        /// <param name="listaTurmaHorario"></param>
        /// <param name="banco"></param>
        private static void ValidarCarga(List<TUR_TurmaHorario> listaTurmaHorario, TalkDBTransaction banco)
        {
            string tud_ids = string.Join(";", listaTurmaHorario.Where(p => p.tud_id > 0).GroupBy(p => p.tud_id).Select(p => p.Key.ToString()).ToArray());
            if (!string.IsNullOrEmpty(tud_ids))
            {
                List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.SelecionaTurmaDisciplina(tud_ids, banco);

                var validacaoCargaSemanal = from TUR_TurmaHorario horario in listaTurmaHorario.Where(p => p.tud_id > 0)
                                            group horario by horario.tud_id into gHorario
                                            join TUR_TurmaDisciplina tud in listaDisciplinas
                                            on gHorario.Key equals tud.tud_id
                                            select new
                                            {
                                                tud_id = gHorario.Key
                                                ,
                                                tud_nome = gHorario.First().tud_nome
                                                ,
                                                tud_tipo = tud.tud_tipo
                                                ,
                                                quantidadeAulas = gHorario.Count()
                                                ,
                                                cargaHorarioSemanal = tud.tud_cargaHorariaSemanal
                                            };

                if (validacaoCargaSemanal.Any(p => p.quantidadeAulas > p.cargaHorarioSemanal && p.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaPrincipal && p.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia))
                {
                    string mensagem;

                    mensagem = string.Join("<br />",
                        validacaoCargaSemanal.Where(p => p.quantidadeAulas > p.cargaHorarioSemanal && p.tud_tipo != (byte)TurmaDisciplinaTipo.DisciplinaPrincipal)
                                             .Select(p => string.Format(CustomResource.GetGlobalResourceObject("BLL", "TUR_TurmaHorarioBO.SalvarTurmaHorario.ValidacaoTemposAula"), p.tud_nome, p.cargaHorarioSemanal))
                                             .ToArray());

                    if (!string.IsNullOrEmpty(mensagem))
                    {
                        throw new ValidationException(mensagem);
                    }
                }
            }
        }

        #endregion Métodos de validação

        #region Métodos de consulta

        /// <summary>
        /// Seleciona os tempos para as aulas, nas disciplinas que possuem aula no dia informado, por turma
        /// </summary>
        /// <returns></returns>
        public static List<CLS_TurmaAula> SelecionaAulas_Por_Turma_Data(long tur_id, int tpc_id, DateTime tau_data)
        {
            return GestaoEscolarUtilBO.MapToEnumerable<CLS_TurmaAula>
                (new TUR_TurmaHorarioDAO().SelecionaAulas_Por_Turma_Data(tur_id, tpc_id, tau_data)).ToList();
        }

        /// <summary>
        /// Seleciona turma horário por turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaHorario> SelecionaPorTurma(long tur_id)
        {
            TalkDBTransaction banco = new TUR_TurmaHorarioDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return SelecionaPorTurma(tur_id, banco);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }
	    public static List<TUR_TurmaHorarioDTO> Save(string Json)
        {
            //parse Json pra lista de DTO
            //salvar os itens da lista
            //retornar a lista
            List<TUR_TurmaHorarioDTO> lista = new List<TUR_TurmaHorarioDTO>();
            TalkDBTransaction banco = new TUR_TurmaHorarioDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                lista = (from item in listaDados.AsEnumerable()
                         select (TUR_TurmaHorarioDTO)JsonConvert.DeserializeObject<TUR_TurmaHorarioDTO>(item.ToString())).ToList();

                banco.Open();

                lista.ForEach(item => TUR_TurmaHorarioBO.Save(item, banco));
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

            return lista;
        }

        /// <summary>
        /// Seleciona turma horário por turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaHorario> SelecionaPorTurma(long tur_id, TalkDBTransaction banco)
        {
            return GestaoEscolarUtilBO.MapToEnumerable<TUR_TurmaHorario>(new TUR_TurmaHorarioDAO { _Banco = banco }.SelecionaPorTuma(tur_id)).ToList();
        }

        /// <summary>
        /// Seleciona os registros dos tuds informados que estão vigentes
        /// </summary>
        /// <param name="tud_ids"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        private static List<TUR_TurmaHorario> SelecionaPorTurmaDisciplinasVigentes(string tud_ids, TalkDBTransaction banco)
        {
            TUR_TurmaHorarioDAO dao = banco == null ? new TUR_TurmaHorarioDAO() : new TUR_TurmaHorarioDAO { _Banco = banco };
            return dao.SelecionaPorTurmaDisciplinasVigentes(tud_ids);
        }

        /// <summary>
        /// Seleciona os registros dos tuds informados que estão com a mesma vigência início do filtro
        /// </summary>
        /// <param name="tud_ids"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        private static List<TUR_TurmaHorario> SelecionaPorTurmaDisciplinasVigenciaInicio(string tud_ids, DateTime thr_vigenciaInicio, TalkDBTransaction banco)
        {
            TUR_TurmaHorarioDAO dao = banco == null ? new TUR_TurmaHorarioDAO() : new TUR_TurmaHorarioDAO { _Banco = banco };
            return dao.SelecionaPorTurmaDisciplinasVigenciaInicio(tud_ids, thr_vigenciaInicio);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>TUR_TurmaHorarioDTO</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TUR_TurmaHorarioDTO SelecionarTurmaHorarioPorId(int trn_id, int trh_id, int thr_id, long tud_id)
        {
            try
            {
                TUR_TurmaHorario turmaHorario = new TUR_TurmaHorarioDAO().SelecionarTurmaHorarioPorId(trn_id, trh_id, thr_id, tud_id);

                return turmaHorario != null ? (TUR_TurmaHorarioDTO)GestaoEscolarUtilBO.Clone(turmaHorario, new TUR_TurmaHorarioDTO()) : null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List&lt;TUR_TurmaHorarioDTO&gt;</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TUR_TurmaHorarioDTO> SelecionarTurmaHorario()
        {
            List<TUR_TurmaHorarioDTO> lista = new List<TUR_TurmaHorarioDTO>();

            try
            {
                TUR_TurmaHorarioDAO dao = new TUR_TurmaHorarioDAO();

                lista = (from dr in dao.Seleciona().AsEnumerable()
                         select (TUR_TurmaHorarioDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new TUR_TurmaHorarioDTO())).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }

        #endregion Métodos de consulta

        #region Métodos de inclusão/alteração

        /// <summary>
        /// Salva uma entidade de turma horário.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(TUR_TurmaHorario entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new TUR_TurmaHorarioDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva os registros recebidos pela API validando e encerrando os registros vigentes.
        /// </summary>
        /// <param name="listaTurmaHorario"></param>
        /// <param name="_banco"></param>
        /// <returns></returns>
        public static bool SalvarTurmaHorarioAPI(List<TUR_TurmaHorarioDTO> listaTurmaHorario, TalkDBTransaction _banco = null)
        {
            TalkDBTransaction banco = _banco == null ? new TUR_TurmaHorarioDAO()._Banco.CopyThisInstance() : _banco;
            if (_banco == null)
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (!listaTurmaHorario.Any())
                    return false;

                List<TUR_TurmaHorario> listaTurmaHorarioSalvar = new List<TUR_TurmaHorario>();
                foreach (TUR_TurmaHorarioDTO thr in listaTurmaHorario)
                {
                    TUR_TurmaHorario thrAdd = new TUR_TurmaHorario 
                    {
                        trn_id = thr.trn_id,
                        trh_id = thr.trh_id,
                        tud_id = thr.tud_id,
                        thr_vigenciaInicio = thr.thr_vigenciaInicio,
                        IsNew = true,
                        thr_dataAlteracao = DateTime.Now,
                        thr_dataCriacao = DateTime.Now,
                        thr_situacao = thr.thr_situacao,
                        thr_registroExterno = true
                    };

                    listaTurmaHorarioSalvar.Add(thrAdd);
                }

                ValidarCarga(listaTurmaHorarioSalvar, banco);

                string tud_ids = string.Join(";", listaTurmaHorarioSalvar.Where(p => p.tud_id > 0).GroupBy(p => p.tud_id).Select(p => p.Key.ToString()).ToArray());
                if (!string.IsNullOrEmpty(tud_ids))
                {
                    List<TUR_TurmaHorario> listaEncerrar = SelecionaPorTurmaDisciplinasVigentes(tud_ids, banco);

                    foreach (TUR_TurmaHorario thr in listaEncerrar)
                    {
                        thr.thr_vigenciaFim = listaTurmaHorarioSalvar.First().thr_vigenciaInicio.AddDays(-1);
                        thr.thr_situacao = 4;//Inativo

                        Save(thr, banco);
                    }
                }

                if (!listaTurmaHorarioSalvar.Any(t => t.thr_situacao == 3) && listaTurmaHorarioSalvar.Any(t => !Save(t, banco)))
                    throw new ValidationException("Erro ao salvar turma horário.");

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        /// <summary>
        /// Salva uma lista de turma horários da turma
        /// </summary>
        /// <param name="listaTurmaHorario"></param>
        /// <returns></returns>
        public static bool SalvarTurmaHorario(long tur_id, List<TUR_TurmaHorario> listaTurmaHorario, TalkDBTransaction _banco = null)
        {
            TalkDBTransaction banco = _banco == null ? new TUR_TurmaHorarioDAO()._Banco.CopyThisInstance() : _banco;
            if (_banco == null)
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                List<TUR_TurmaDisciplina> lstTud = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

                //Se há uma turma disciplina da turma que não está na lista de turma horário então não permite salvar
                if (lstTud.Where(t => t.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia).Any(t => !listaTurmaHorario.Any(th => th.tud_id == t.tud_id)))
                    throw new ValidationException(CustomResource.GetGlobalResourceObject("BLL", "TUR_TurmaHorarioBO.SalvarTurmaHorario.ValidarTudsTurmaHorario"));

                ValidarCarga(listaTurmaHorario, banco);

                listaTurmaHorario.Where(p => p.thr_id > 0 && p.tud_id <= 0)
                                 .Aggregate(true, (deletou, horario) => deletou & Delete(horario, banco));

                listaTurmaHorario.ForEach(p => p.IsNew = p.thr_id <= 0);

                listaTurmaHorario.Where(p => p.tud_id > 0)
                                 .Aggregate(true, (salvou, horario) => salvou & Save(horario, banco));

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        #endregion Métodos de inclusão/alteração				
        
        public static List<TUR_TurmaHorario> SelecionarVigentes(DateTime? dataVigente = null, TalkDBTransaction _banco = null)
        {
            var dao = new TUR_TurmaHorarioDAO();
            if (_banco != null) dao._Banco = _banco;

            return dao.SelecionaVigentes(dataVigente.GetValueOrDefault(DateTime.Today));
        }
	}
}