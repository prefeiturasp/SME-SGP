/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Validation.Exceptions;
    using System.Text;

    #region Estrutura

    /// <summary>
    /// Tipo da mensagem de confirmação da orientação curricular
    /// </summary>
    public enum ORC_OrientacaoCurriculaTipoMensagem : byte
    {
        DadosReplicarOrientacao = 1
    }
    
    #endregion Estrutura

    /// <summary>
    /// Description: ORC_OrientacaoCurricular Business Object.
    /// </summary>
    public class ORC_OrientacaoCurricularBO : BusinessBase<ORC_OrientacaoCurricularDAO, ORC_OrientacaoCurricular>
    {
        #region Estrutura

        [Serializable]
        public struct OrientacaoCurricular
        {
            public int nvl_ordem { get; set; }

            public string nvl_nome { get; set; }

            public ORC_OrientacaoCurricular entOrientacao { get; set; }

            public List<OrientacaoCurricular> ltOrientacaoCurricularFilho { get; set; }

            public List<ORC_OrientacaoCurricularNivelAprendizado> ltNivelAprendizadoOrientacaoFilho { get; set; }
        }

        #endregion Estrutura

        #region Métodos de consulta

        /// <summary>
        /// Seleciona as orientações curriculares por calendario, período e por disciplina.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns></returns>
        public static List<OrientacaoCurricular> SelecionaPorCalendarioPeriodoDisciplina(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id, long mat_id)
        {
            ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();
            DataTable dt = dao.SelecionaPorCalendarioPeriodoDisciplina(cal_id, cur_id, crr_id, crp_id, tds_id, mat_id);
            List<OrientacaoCurricular> lista = new List<OrientacaoCurricular>();

            if (dt.Rows.Count > 0)
            {
                lista.AddRange(
                    (from DataRow dr in dt.Rows
                     where string.IsNullOrEmpty(dr["ocr_idSuperior"].ToString())
                     select new OrientacaoCurricular
                     {
                         nvl_ordem = Convert.ToInt32(dr["nvl_ordem"]),
                         nvl_nome = dr["nvl_nome"].ToString(),
                         entOrientacao = dao.DataRowToEntity(dr, new ORC_OrientacaoCurricular()),
                         ltOrientacaoCurricularFilho = RetornaListaFilhos(dr, dt),
                         ltNivelAprendizadoOrientacaoFilho = new List<ORC_OrientacaoCurricularNivelAprendizado>()
                     }).ToList()
                );
            }

            return lista;
        }

        /// <summary>
        /// O método é usado para montar a estrutura de orientações curriculares para ser usado na tela de cadastro
        /// de orientação curriculares.
        /// </summary>
        /// <param name="drSuperior">Orientação superior</param>
        /// <param name="dtOrientacaoCurricular"></param>
        /// <returns></returns>
        private static List<OrientacaoCurricular> RetornaListaFilhos(DataRow drSuperior, DataTable dtOrientacaoCurricular)
        {
            List<OrientacaoCurricular> lista = new List<OrientacaoCurricular>();

            long ocr_idSuperior = Convert.ToInt64(drSuperior["ocr_id"]);

            if (dtOrientacaoCurricular.Rows.Cast<DataRow>().Where(row => !string.IsNullOrEmpty(row["ocr_idSuperior"].ToString()))
                                                           .Any(row => Convert.ToInt64(row["ocr_idSuperior"]) == ocr_idSuperior))
            {
                lista.AddRange(
                    (from DataRow dr in dtOrientacaoCurricular.Rows
                     where !string.IsNullOrEmpty(dr["ocr_idSuperior"].ToString()) &&
                           Convert.ToInt64(dr["ocr_idSuperior"]) == ocr_idSuperior
                     select new OrientacaoCurricular
                     {
                         nvl_ordem = Convert.ToInt32(dr["nvl_ordem"]),
                         nvl_nome = dr["nvl_nome"].ToString(),
                         entOrientacao = new ORC_OrientacaoCurricularDAO().DataRowToEntity(dr, new ORC_OrientacaoCurricular()),
                         ltOrientacaoCurricularFilho = RetornaListaFilhos(dr, dtOrientacaoCurricular)
                     }).ToList()
                );
            }
            else
            {
                lista.AddRange(
                    (from DataRow dr in dtOrientacaoCurricular.Rows
                     where !string.IsNullOrEmpty(dr["ocr_idSuperior"].ToString()) &&
                           Convert.ToInt64(dr["ocr_idSuperior"]) == ocr_idSuperior
                     select new OrientacaoCurricular
                     {
                         nvl_ordem = Convert.ToInt32(dr["nvl_ordem"]),
                         nvl_nome = dr["nvl_nome"].ToString(),
                         entOrientacao = new ORC_OrientacaoCurricularDAO().DataRowToEntity(dr, new ORC_OrientacaoCurricular()),
                         ltOrientacaoCurricularFilho = new List<OrientacaoCurricular>()
                     }).ToList()
                );
            }

            return lista;
        }

        /// <summary>
        /// Seleciona as orientações curriculares por nivel.
        /// <param name="cal_id">ID do nível.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorNivel(int nvl_id)
        {
            return new ORC_OrientacaoCurricularDAO().SelecionaPorNivel(nvl_id);
        }

        /// <summary>
        /// Retorna registros ativos ou com data de alteração posterior a ultima sincronizacao.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        /// <param name="crp_id">Curriculo periodo</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tds_id">Tipo de disciplina</param>
        /// <returns></returns>
        public static DataTable SelecionaPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();
            return dao.SelectPorDataSincronizacao(syncDate, tur_id, cur_id, crr_id, crp_id, cal_id, tds_id);
        }

        /// <summary>
        /// Seleciona lista de IDs das orientações curriculares que derivam da orientação passada por parâmetro
        /// e que possuem nível final.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular em que a busca é iniciada.</param>
        /// <returns>Lista de IDs de orientações curriculares.</returns>
        public static List<long> SelecionaUltimoNivel(long ocr_id)
        {
            return new ORC_OrientacaoCurricularDAO().SelecionaUltimoNivel(ocr_id);
        }

        /// <summary>
        /// Seleciona as orientações curriculares por calendario, período e por disciplina para construção do treeview.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns>DataTabel com dados</returns>
        public static DataTable SelecionaPorCalendarioPeriodoDisciplinaTreeview(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id)
        {
            return new ORC_OrientacaoCurricularDAO().SelecionaPorCalendarioPeriodoDisciplinaTreeview(cal_id, cur_id, crr_id, crp_id, tds_id);
        }

        /// <summary>
        /// Seleciona as orientações curriculares por calendario, período e por disciplina para construção do treeview de relacionamento entre matrizes.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="mat_id">ID da matriz relacionada ou que se deseja relacionar.</param>
        /// <param name="ocr_id">ID da habilidade da matriz padrão para relacionamento.</param>
        /// <returns>DataTabel com dados</returns>
        public static DataTable SelecionaPorCalendarioPeriodoDisciplinaTreeview_ByMatriz(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id, Int64 mat_id, Int64 ocr_id)
        {
            return new ORC_OrientacaoCurricularDAO().SelecionaPorCalendarioPeriodoDisciplinaTreeview_ByMatriz(cal_id, cur_id, crr_id, crp_id, tds_id, mat_id, ocr_id);
        }

        /// <summary>
        /// Retorna se a orientação possui habilidade relacionada.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <returns></returns>
        public static bool SelecionaPossuiRelacionamento(Int64 ocr_id)
        {
            return new ORC_OrientacaoCurricularDAO().SelecionaPossuiRelacionamento(ocr_id);
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela turmadisciplina e database
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <returns>se tpc_id != de null retorna resultado da procedure que filtra orientações curriculares planejados vinculadas ao tipo do calendario periodo passado</returns>
        public static List<ORC_OrientacaoCurricular> SelecionaPorTurmaDisciplinaDataBase(long tud_id, DateTime dataBase, long ocr_idSuperior, Nullable<long> tpc_id)
        {
            ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();

            //se tpc_id != de null retorna resultado da procedure que filtra arientações curriculares planejados vinculadas ao tipo do calendario periodo passado
            if (tpc_id != null)
                return dao.SelecionaPorTurmaDisciplinaDataBaseTpc_id(tud_id, dataBase, ocr_idSuperior, tpc_id);

            return dao.SelecionaPorTurmaDisciplinaDataBase(tud_id, dataBase, ocr_idSuperior); 
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela database, entidade e escola
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static DataTable SelecionaOrientacoesPorEntidadeEscolaDataBase(Guid ent_id, int esc_id, DateTime dataBase)
        {
            ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();

            return dao.SelecionaOrientacoesPorEntidadeEscolaDataBase(ent_id, esc_id, dataBase);
        }

        #endregion Métodos de consulta

        #region Métodos de verificação

        /// <summary>
        /// Método verifica se para o mesmo nível e disciplina existe uma orientação curricular com o código passado.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="nvl_id">ID do nível.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="ocr_codigo">Código da orientação curricular.</param>
        /// <returns>True se já existe.</returns>
        public static bool VerificaCodigoExistente(ORC_OrientacaoCurricular entity, TalkDBTransaction banco = null)
        {
            ORC_OrientacaoCurricularDAO dao = banco == null ?
                                              new ORC_OrientacaoCurricularDAO() :
                                              new ORC_OrientacaoCurricularDAO { _Banco = banco };

            return dao.VerificaCodigoExistente(entity.ocr_id, entity.ocr_idSuperior, entity.nvl_id, entity.tds_id, entity.ocr_codigo);
        }

        /// <summary>
        /// Método verifica se para o mesmo nível e disciplina existe uma orientação curricular com a descrição passada.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="nvl_id">ID do nível.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="ocr_descricao">Descrição da orientação curricular.</param>
        /// <returns>True se já existe.</returns>
        public static bool VerificaDescricaoExistente(ORC_OrientacaoCurricular entity, TalkDBTransaction banco = null)
        {
            ORC_OrientacaoCurricularDAO dao = banco == null ?
                                              new ORC_OrientacaoCurricularDAO() :
                                              new ORC_OrientacaoCurricularDAO { _Banco = banco };

            return dao.VerificaDescricaoExistente(entity.ocr_id, entity.ocr_idSuperior, entity.nvl_id, entity.tds_id, entity.ocr_descricao);
        }

        /// <summary>
        /// Método verifica se existe orientação curricular com o código passado.
        /// </summary>
        /// <param name="entity">Entity da orientação curricular</param>
        /// <param name="banco">Banco da transacao.</param>
        /// <returns>True se já existe.</returns>
        public static bool VerificaOrientacaoCurricularExistenteCodigo(ORC_OrientacaoCurricular entity, TalkDBTransaction banco = null)
        {
            ORC_OrientacaoCurricularDAO dao = banco == null ?
                                              new ORC_OrientacaoCurricularDAO() :
                                              new ORC_OrientacaoCurricularDAO { _Banco = banco };

            return dao.SelectBy_CodigoOrientacaoCurricular(entity);
        }

        /// <summary>
        /// Verifica se a lista de orientações curriculares está sendo usado em outras telas.
        /// </summary>
        /// <param name="ltOrientacao">Lista de ids das orientações curriculares.</param>
        /// <returns></returns>
        public static bool VerificaIntegridade(List<long> ltOrientacao, TalkDBTransaction banco)
        {
            try
            {
                return ltOrientacao.Aggregate(false, (usado, ocr_id) => usado |= GestaoEscolarUtilBO.VerificarIntegridade("ocr_id", ocr_id.ToString(), "ORC_OrientacaoCurricular,ORC_OrientacaoCurricularNivelAprendizado", banco));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Métodos de verificação

        #region Métodos para inserir / alterar

        /// <summary>
        /// O método realiza as validações necessárias e salva uma orientação curricular.
        /// </summary>
        /// <param name="entity">Entidade que representa uma orientação curricular.</param>
        /// <returns></returns>
        public static new bool Save(ORC_OrientacaoCurricular entity)
        {
            if (entity.Validate())
            {
                if (VerificaCodigoExistente(entity))
                    throw new DuplicateNameException("Já existe uma orientação curricular cadastrada com esse código.");

                if (VerificaDescricaoExistente(entity))
                    throw new DuplicateNameException("Já existe uma orientação curricular cadastrada com essa descrição.");

                GestaoEscolarUtilBO.LimpaCache(string.Format(ORC_OrientacaoCurricularNivelAprendizadoBO.Cache_SelecionaPorOrientacaoNivelAprendizado));

                return new ORC_OrientacaoCurricularDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método realiza as validações necessárias e salva uma orientação curricular.
        /// </summary>
        /// <param name="entity">Entidade que representa uma orientação curricular.</param>
        /// <returns></returns>
        public static new bool Save(ORC_OrientacaoCurricular entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                if (VerificaCodigoExistente(entity))
                    throw new DuplicateNameException("Já existe uma orientação curricular cadastrada com esse código.");

                if (VerificaDescricaoExistente(entity))
                    throw new DuplicateNameException("Já existe uma orientação curricular cadastrada com essa descrição.");

                GestaoEscolarUtilBO.LimpaCache(string.Format(ORC_OrientacaoCurricularNivelAprendizadoBO.Cache_SelecionaPorOrientacaoNivelAprendizado));

                ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Função recursiva para replicar a orientação curricular e as filhas
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="dtOrientacoes"></param>
        /// <param name="dtNiveisReplica"></param>
        /// <param name="tds_id"></param>
        /// <param name="ocr_idPai"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvaFilhos
        (
            DataRow[] rows,
            DataTable dtOrientacoes,
            List<sNivelOrientacaoCurricular> dtNiveisReplica,
            DataTable dtOrientacoesNova,
            int tds_id,
            long ocr_idPai,
            long mat_id,
            TalkDBTransaction banco
        )
        {
            ORC_OrientacaoCurricular orientacao;
            ORC_OrientacaoCurricularNivelAprendizado orientacaoNivelApr;

            try
            {
                string ocr_ids = string.Join(";", rows.GroupBy(p => p["ocr_id"].ToString()).Select(p => p.Key).ToArray());

                List<sOrientacaoNivelAprendizado> dtNiveisAprendizado = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

                var orientacaoNivelAprendizado = from row in dtNiveisAprendizado
                                                 group row by row.ocr_id
                                                     into grupo
                                                     select new
                                                     {
                                                         ocr_id = grupo.Key
                                                         ,
                                                         ltNiveisAprendizado = grupo.GroupBy(p => p.nap_id).Select(p => p.Key).ToList()
                                                     };

                foreach (DataRow rowFilho in rows)
                {
                    List<int> ltNiveisAprendizado = orientacaoNivelAprendizado.Any(p => p.ocr_id == Convert.ToInt64(rowFilho["ocr_id"])) ?
                        orientacaoNivelAprendizado.ToList().Find(p => p.ocr_id == Convert.ToInt64(rowFilho["ocr_id"])).ltNiveisAprendizado :
                        new List<int>();

                    string nvl_id = dtNiveisReplica.Where(p => p.nvl_ordem == Convert.ToInt32(rowFilho["nvl_ordem"]))
                                                   .Select(p => p.nvl_id.ToString()).FirstOrDefault();

                    if (!string.IsNullOrEmpty(nvl_id))
                    {
                        bool existe = dtOrientacoesNova.Rows.Cast<DataRow>().Any(p => p["ocr_codigo"].ToString().Equals(rowFilho["ocr_codigo"].ToString())
                                                                                && p["ocr_descricao"].ToString().Equals(rowFilho["ocr_descricao"].ToString())
                                                                                && p["nvl_id"].ToString().Equals(nvl_id));

                        /* Salva as orientações filhas */

                        orientacao = new ORC_OrientacaoCurricular
                        {
                            ocr_id = -1,
                            nvl_id = Convert.ToInt32(nvl_id),
                            tds_id = tds_id,
                            mat_id = mat_id,
                            ocr_idSuperior = ocr_idPai,
                            ocr_codigo = rowFilho["ocr_codigo"].ToString(),
                            ocr_descricao = rowFilho["ocr_descricao"].ToString(),
                            ocr_situacao = 1,
                            ocr_dataAlteracao = DateTime.Now,
                            IsNew = true
                        };

                        if (existe)
                        {
                            string ocr_id_existente = dtOrientacoesNova.Rows.Cast<DataRow>()
                                                           .Where(p => p["ocr_codigo"].ToString().Equals(rowFilho["ocr_codigo"].ToString())
                                                               && p["ocr_descricao"].ToString().Equals(rowFilho["ocr_descricao"].ToString())
                                                               && p["nvl_id"].ToString().Equals(nvl_id))
                                                           .Select(p => p["ocr_id"].ToString()).FirstOrDefault();

                            orientacao.IsNew = false;
                            if (!string.IsNullOrEmpty(ocr_id_existente))
                            {
                                orientacao.ocr_id = Convert.ToInt64(ocr_id_existente);
                            }
                        }

                        Save(orientacao, banco);

                        // Verifica se os filhos possuem mais filhos, se sim, chama a função recursivamente
                        DataRow[] rowsNetos = dtOrientacoes.Select("ocr_idSuperior = " + rowFilho["ocr_id"].ToString());

                        if (rowsNetos.Length > 0)
                        {
                            SalvaFilhos(rowsNetos, dtOrientacoes, dtNiveisReplica, dtOrientacoesNova, tds_id, orientacao.ocr_id, mat_id, banco);
                        }
                        else
                        {
                            //Remove os níveis de aprendizado já existentes
                            if (existe)
                            {
                                DataTable dtOcn = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectNivelAprendizadoByOcrId(orientacao.ocr_id, 0, banco);
                                foreach (DataRow row in dtOcn.Rows)
                                {
                                    ORC_OrientacaoCurricularNivelAprendizado ocn = new ORC_OrientacaoCurricularNivelAprendizado
                                                                                    {
                                                                                        ocr_id = orientacao.ocr_id,
                                                                                        nap_id = Convert.ToInt32(row["nap_id"]),
                                                                                        ocn_id = Convert.ToInt32(row["ocn_id"])
                                                                                    };
                                    ORC_OrientacaoCurricularNivelAprendizadoBO.GetEntity(ocn, banco);

                                    if (!ocn.IsNew)
                                    {
                                        ocn.ocn_situacao = 3; //Excluido
                                        ocn.ocn_dataAlteracao = DateTime.Now;

                                        ORC_OrientacaoCurricularNivelAprendizadoBO.Save(ocn, banco);
                                    }
                                }
                            }

                            // Replicar os níveis de aprendizado
                            foreach (int nap_id in ltNiveisAprendizado)
                            {
                                orientacaoNivelApr = new ORC_OrientacaoCurricularNivelAprendizado
                                {
                                    ocr_id = orientacao.ocr_id,
                                    nap_id = ORC_NivelAprendizadoBO.SelectCursoPeriodoBy_nap_id(nap_id, Convert.ToInt32(nvl_id), banco),
                                    ocn_id = -1,
                                    ocn_situacao = 1,
                                    ocn_dataCriacao = DateTime.Now,
                                    ocn_dataAlteracao = DateTime.Now,
                                    IsNew = true
                                };
                                ORC_OrientacaoCurricularNivelAprendizadoBO.Save(orientacaoNivelApr, banco);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //banco.Close(ex);
                throw ex;
            }
            //finally
            //{
            //    if (banco.ConnectionIsOpen)
            //        banco.Close();
            //}
        }

        public static bool ReplicarOrientacaoCurricular
        (
            int cur_id,
            int crr_id,
            int crp_idReplicar,
            int crp_id,
            int cal_id,
            int tds_id,
            long ocr_id_replicar,
            int cur_id_replicar,
            long mat_id
        )
        {
            TalkDBTransaction bancoGestao = new ORC_OrientacaoCurricularDAO()._Banco.CopyThisInstance();
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            List<ORC_OrientacaoCurricular> ltOrientacao = new List<ORC_OrientacaoCurricular>();
            ORC_OrientacaoCurricular orientacao;
            ORC_OrientacaoCurricularDAO dao = new ORC_OrientacaoCurricularDAO();

            try
            {
                DataTable dt = dao.SelecionaPorCalendarioPeriodoDisciplina(cal_id, cur_id, crr_id, crp_id, tds_id, mat_id);

                List<sNivelOrientacaoCurricular> dtNiveisReplica = ORC_NivelBO.SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(cur_id_replicar, crr_id, crp_idReplicar, cal_id, tds_id, mat_id, GestaoEscolarUtilBO.MinutosCacheLongo);

                // DataTable com as orientações caso já tenha cadastrado
                DataTable dtOrientacoes = dao.SelecionaPorCalendarioPeriodoDisciplina(cal_id, cur_id_replicar, crr_id, crp_idReplicar, tds_id, mat_id);

                foreach (DataRow row in dt.Rows)
                {
                    string nvl_id = dtNiveisReplica.Where(p => p.nvl_ordem == Convert.ToInt32(row["nvl_ordem"]))
                                                   .Select(p => p.nvl_id.ToString()).FirstOrDefault();

                    if (string.IsNullOrEmpty(row["ocr_idSuperior"].ToString()) &&
                        row["ocr_id"].ToString().Equals(ocr_id_replicar.ToString()) &&
                        !string.IsNullOrEmpty(nvl_id))
                    {
                        /* Salva as orientações curriculares pais */

                        orientacao = new ORC_OrientacaoCurricular
                        {
                            ocr_id = -1,
                            nvl_id = Convert.ToInt32(nvl_id),
                            tds_id = tds_id,
                            mat_id = mat_id,
                            ocr_codigo = row["ocr_codigo"].ToString(),
                            ocr_descricao = row["ocr_descricao"].ToString(),
                            ocr_situacao = 1,
                            ocr_dataAlteracao = DateTime.Now,
                            IsNew = true
                        };

                        bool existe = dtOrientacoes.Rows.Cast<DataRow>().Any(p => p["ocr_codigo"].ToString().Equals(row["ocr_codigo"].ToString())
                                                                                && p["ocr_descricao"].ToString().Equals(row["ocr_descricao"].ToString())
                                                                                && p["nvl_id"].ToString().Equals(nvl_id));

                        if (existe)
                        {
                            string ocr_id_existente = dtOrientacoes.Rows.Cast<DataRow>()
                                                           .Where(p => p["ocr_codigo"].ToString().Equals(row["ocr_codigo"].ToString())
                                                               && p["ocr_descricao"].ToString().Equals(row["ocr_descricao"].ToString())
                                                               && p["nvl_id"].ToString().Equals(nvl_id))
                                                           .Select(p => p["ocr_id"].ToString()).FirstOrDefault();

                            orientacao.IsNew = false;
                            if (!string.IsNullOrEmpty(ocr_id_existente))
                            {
                                orientacao.ocr_id = Convert.ToInt64(ocr_id_existente);
                            }
                        }

                        Save(orientacao, bancoGestao);

                        DataRow[] rows = dt.Select("ocr_idSuperior = " + row["ocr_id"].ToString());

                        if (rows.Length > 0)
                        {
                            SalvaFilhos(rows, dt, dtNiveisReplica, dtOrientacoes, tds_id, orientacao.ocr_id, mat_id, bancoGestao);
                        }
                    }
                }
            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch (DuplicateNameException de)
            {
                throw de;
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                throw ex;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                    bancoGestao.Close();
            }

            return true;
        }

        #endregion Métodos para inserir / alterar

        #region Métodos de exclusão

        /// <summary>
        /// O método deleta uma orientação curricular e todas as outras ligadas a esse.
        /// </summary>
        /// <param name="ORC_OrientacaoCurricular">Entidade da orientação curricular inicial.</param>
        /// <returns></returns>
        public static bool DeletarHierarquia(ORC_OrientacaoCurricular entity)
        {
            TalkDBTransaction banco = new ORC_OrientacaoCurricularDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                List<ORC_OrientacaoCurricularNivelAprendizado> ltOrientacaoNivel = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectTodosNivelAprendizadoByOcrId(entity.ocr_id);
                List<ORC_OrientacaoCurricularNivelAprendizado> ltOrientacaoNivelAtivos = ltOrientacaoNivel.Where(p => p.ocn_situacao != 3).ToList();

                // Apaga os níveis de aprendizado da orientação curricular
                foreach (ORC_OrientacaoCurricularNivelAprendizado orientacaoNivel in ltOrientacaoNivelAtivos)
                {
                    orientacaoNivel.ocn_situacao = 3;
                    ORC_OrientacaoCurricularNivelAprendizadoBO.Save(orientacaoNivel, banco);
                }

                if (VerificaIntegridade(SelecionaUltimoNivel(entity.ocr_id), banco))
                    throw new ValidationException("Não é possível excluir a orientação curricular pois possui outros registros ligados a ele.");

                return new ORC_OrientacaoCurricularDAO().DeletarHierarquia(entity.ocr_id);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw ex;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        #endregion Métodos de exclusão       
    }
}