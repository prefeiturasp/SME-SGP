using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    public enum eTipoCargo : byte
    {
        Comum = 1, CargoBase = 2, AtribuicaoEsporadica = 3
    }

    #endregion

    public class RHU_CargoBO : BusinessBase<RHU_CargoDAO, RHU_Cargo>
    {
        /// <summary>
        /// Retorna todos os cargos do tipo informado.
        /// </summary>
        public static List<RHU_Cargo> SelecionaPorTipo(Guid ent_id, eTipoCargo crg_tipo)
        {
            DataTable dt = new RHU_CargoDAO().SelectPorTipo(ent_id, (byte)crg_tipo);

            return (from DataRow dr in dt.Rows
                    select (RHU_Cargo)GestaoEscolarUtilBO.DataRowToEntity(dr, new RHU_Cargo())).ToList();
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        public static DataTable SelecionaNaoExcluidos(Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectNaoExcluidos(ent_id);
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_cargoDocente.
        /// </summary>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">ID da entidade</param>
        public static DataTable SelecionaNaoExcluidosPorCargoDocente(bool crg_cargoDocente, Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectNaoExcluidosByCargoDocente(crg_cargoDocente, ent_id);
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_controleIntegracao.
        /// </summary>
        /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
        /// <param name="ent_id">ID da entidade</param>
        public static DataTable SelecionaVerificandoControleIntegracao(bool crg_controleIntegracao, Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelecionaVerificandoControleIntegracao(crg_controleIntegracao, ent_id);
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_controleIntegracao e crg_cargoDocente.
        /// </summary>
        /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">ID da entidade</param>
        public static DataTable SelecionaVerificandoControleIntegracaoPorCargoDocente(bool crg_controleIntegracao, bool crg_cargoDocente, Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelecionaVerificandoControleIntegracaoPorCargoDocente(crg_controleIntegracao, crg_cargoDocente, ent_id);
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por tvi_id e ent_id.
        /// </summary>
        /// <param name="tvi_id">Tipo de vínculo</param>
        /// <param name="ent_id">ID da entidade</param>
        public static DataTable SelecionaCargoByTipoVinculo(int tvi_id, Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelecionaCargoByTipoVinculo(tvi_id, ent_id);
        }

        /// <summary>
        /// Retorna todos os cargos não excluídos logicamente.
        /// </summary>
        /// <param name="tvi_id">ID do tipo de vínculo</param>
        /// <param name="crg_nome">Nome do cargo</param>
        /// <param name="crg_codigo">Código do cargo</param>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCargoPaginado
        (
            int tvi_id
            , string crg_nome
            , string crg_codigo
            , byte crg_cargoDocente
            , Guid ent_id
        )
        {
            totalRecords = 0;

            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectBy_Pesquisa(tvi_id, crg_nome, crg_codigo, crg_cargoDocente, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os cargos não excluídos logicamente
        /// Sem paginação.
        /// </summary>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCargo
        (
            byte crg_cargoDocente
            , Guid ent_id
        )
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectBy_Pesquisa(0, string.Empty, string.Empty, crg_cargoDocente, ent_id, out totalRecords);
        }

        /// <summary>
        /// Verifica se já existe um cargo cadastrado com o mesmo nome na mesma entidade.
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaNomeExistente(RHU_Cargo entity, TalkDBTransaction banco = null)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_Nome(entity.crg_id, entity.crg_nome, entity.ent_id);
        }

        /// <summary>
        /// Verifica se já existe um cargo cadastrado com o mesmo código na mesma entidade.
        /// </summary>
        /// <param name="crg_codigo">Código do cargo</param>
        /// <param name="ent_id">Id da entidade</param>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_Codigos(string crg_codigos, Guid ent_id)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectBy_Codigos(crg_codigos, ent_id);
        }

        /// <summary>
        /// Seleciona todos os cargos cadastrados com o mesmo código e entidade
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCodigoExistente(RHU_Cargo entity, TalkDBTransaction banco = null)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_Codigo(entity.crg_id, entity.crg_codigo, entity.ent_id);
        }

		/// <summary>
		/// Seleciona cargos de acordo com os filtros passados
		/// </summary>
		/// <param name="crg_situacao">Situação do cargo</param>
		/// <param name="crg_cargoDocente">Flag se é cargo de docente</param>
		public static DataTable SelectBy_CargoDocente_Situacao(int crg_situacao, bool crg_cargoDocente, Guid ent_id)
		{
			RHU_CargoDAO dao = new RHU_CargoDAO();
			return dao.SelectBy_CargoDocente_Situacao(crg_situacao, crg_cargoDocente, ent_id);
		}

        /// <summary>
        /// Consulta se já existe o código de determinado cargo,
        /// passando a entidade cargo.
        /// </summary>
        /// <param name="entity">Entidade Cargo (contendo o código do cargo)</param>
        /// <returns>True = já existe o código / False = não existe o código</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ConsultarCodigoExistente(RHU_Cargo entity)
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            return dao.SelectBy_Codigo(entity);
        }

        /// <summary>
        /// Inclui ou altera o cargo.
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save(RHU_Cargo entity, List<RHU_CargoDisciplina> list)
        {
            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            TalkDBTransaction banco = new RHU_CargoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {  
                if (!entity.Validate())
                    throw new ValidationException(entity.PropertiesErrorList[0].Message);

                if (VerificaNomeExistente(entity, banco))
                    throw new DuplicateNameException("Já existe um cargo cadastrado com este nome.");

                if (!string.IsNullOrEmpty(entity.crg_codigo) && VerificaCodigoExistente(entity, banco))
                    throw new DuplicateNameException("Já existe um cargo cadastrado com este código.");

                if (entity.crg_cargoDocente)
                {
                    if (entity.crg_maxAulaDia <= 0)
                        throw new ArgumentException("Máximo de aulas por dia deve ser maior do que 0.");
                    if (entity.crg_maxAulaDia > 24)
                        throw new ArgumentException("Máximo de aulas por dia não pode ser maior do que 24.");
                    if (entity.crg_maxAulaSemana <= 0)
                        throw new ArgumentException("Máximo de aulas por semana deve ser maior do que 0.");
                    if (entity.crg_maxAulaSemana > 168)
                        throw new ArgumentException("Máximo de aulas por semana não pode ser maior do que 168.");
                }

                // Salva o cargo.
                Save(entity, banco);

                if (entity.IsNew)
                {
                    // Incrementa um na integridade da entidade.
                    entDao.Update_IncrementaIntegridade(entity.ent_id);
                }

                // Exclui as disciplinas do cargo.
                RHU_CargoDisciplina entityDelete = new RHU_CargoDisciplina { crg_id = entity.crg_id };
                RHU_CargoDisciplinaBO.DeleteByCargo(entityDelete, banco);

                // Salva as disciplinas do cargo.
                foreach (RHU_CargoDisciplina entityDis in list)
                {
                    entityDis.crg_id = entity.crg_id;
                    RHU_CargoDisciplinaBO.Save(entityDis, banco);
                }

                return true;
            }
            catch (Exception ex)
            {
                entDao._Banco.Close(ex);
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (entDao._Banco.ConnectionIsOpen)
                    entDao._Banco.Close();

                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// Deleta logicamente um cargo.
        /// </summary>
        /// <param name="entity">Entidade RHU_Cargo</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            RHU_Cargo entity
        )
        {
            RHU_CargoDAO dao = new RHU_CargoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Verifica se o cargo pode ser deletado.
                if (GestaoEscolarUtilBO.VerificarIntegridade("crg_id", entity.crg_id.ToString(), "RHU_Cargo, RHU_CargoDisciplina", dao._Banco))
                    throw new ValidationException("Não é possível excluir o cargo pois possui outros registros ligados a ele.");

                // Decrementa um na integridade da entidade.
                entDao.Update_DecrementaIntegridade(entity.ent_id);

                // Deleta logicamente o cargo.
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                entDao._Banco.Close(err);

                throw;
            }
            finally
            {
                dao._Banco.Close();
                entDao._Banco.Close();
            }
        }
    }
}