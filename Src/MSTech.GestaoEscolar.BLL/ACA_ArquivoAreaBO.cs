/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Data;
    using System.ComponentModel;
    using System.Collections.Generic;
	using MSTech.Business.Common;
    using MSTech.Data.Common;
    using MSTech.CoreSSO.DAL;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Validation.Exceptions;
    using System.Linq;

	/// <summary>
	/// Description: ACA_ArquivoArea Business Object. 
	/// </summary>
	public class ACA_ArquivoAreaBO : BusinessBase<ACA_ArquivoAreaDAO, ACA_ArquivoArea>
    {
        #region Enumeradores

        /// <summary>
        /// Situação do registro.
        /// </summary>
        public enum eTipoSituacao
        {
            Ativo = 1
            ,
            Excluido = 3
        }

        /// <summary>
        /// Tipo de documento.
        /// </summary>
        public enum eTipoDocumento
        {
            Arquivo = 1
            ,
            Link = 2
        }

        #endregion Enumeradores

        #region Métodos de inclusão/alteração

        /// <summary>
        /// Inclui ou altera o Documento
        /// </summary>
        /// <param name="listDocumentos">Lista de documentos da area</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Salvar
        (
            int tad_id,
            Guid uad_idSuperior,
            int esc_id,
            List<ACA_ArquivoArea> listDocumentos
        )
        {
            //Inicio do processo de Registro no BD.
            ACA_ArquivoAreaDAO dao = new ACA_ArquivoAreaDAO();

            //Abertura de BEGIN TRAN para Salvar Documentos.
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                List<ACA_ArquivoArea> listaBanco;
                using (DataTable dtBanco =  GetSelectBy_Id_Dre_Escola(tad_id, esc_id, uad_idSuperior, -1, true, false, dao._Banco))
                {
                    listaBanco = dtBanco.Rows.Count > 0 ?
                        dtBanco.Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new ACA_ArquivoArea())).ToList() :
                        new List<ACA_ArquivoArea>();
                }

                foreach (ACA_ArquivoArea entityArquivoArea in listDocumentos)
                {
                    ACA_ArquivoAreaBO.Save(entityArquivoArea, dao._Banco);

                    if (entityArquivoArea.arq_id > 0)
                    {
                        SYS_Arquivo arq = new SYS_Arquivo { arq_id = entityArquivoArea.arq_id };
                        SYS_ArquivoBO.GetEntity(arq, dao._Banco);
                        arq.arq_situacao = (byte)SYS_ArquivoSituacao.Ativo;
                        SYS_ArquivoBO.Save(arq, dao._Banco);
                    }
                }

                bool teste = listaBanco.Where(p => !listDocumentos.Exists(q => q.aar_id == p.aar_id && q.tad_id == p.tad_id))
                          .ToList()
                          .Aggregate(true, (deletou, doc) => deletou & Delete(doc, dao._Banco));

                return true;
            }
            catch (Exception err)
            {
                //Roolback da transação
                dao._Banco.Close(err);
                entDao._Banco.Close(err);
                throw;
            }
            finally
            {
                //Fechamento da transação
                if (dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }

                if (entDao._Banco.ConnectionIsOpen)
                {
                    entDao._Banco.Close();
                }
            }
        }

        /// <summary>
        /// Inclui um novo documento
        /// </summary>
        /// <param name="entity">Entidade ACA_ArquivoArea</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_ArquivoArea entity
            , TalkDBTransaction banco
        )
        {
            try
            {
                //if (entity.Validate())
                //{
                    ACA_ArquivoAreaDAO dao = new ACA_ArquivoAreaDAO { _Banco = banco };
                    return dao.Salvar(entity);
                //}

                throw new ValidationException(entity.PropertiesErrorList[0].Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Métodos de inclusão/alteração

        #region Métodos de consulta

        /// <summary>
        /// Retorna um datatable contendo todos os documentos cadastrados para uma area
        /// </summary>
        /// <param name="tad_id">ID da area</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_ArquivoArea> GetSelectBy_tad_id
        (
            int tad_id
        )
        {
            try
            {
                ACA_ArquivoAreaDAO dao = new ACA_ArquivoAreaDAO();
                return dao.Select_By_tad_id(tad_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo os documentos cadastrados para uma area filtrando DRE e Escola
        /// </summary>
        /// <param name="tad_id">ID da area</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uad_id">ID da DRE</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Id_Dre_Escola
        (
            int tad_id,
            int esc_id,
            Guid uad_idSuperior,
            int tne_id,
            bool incluirPpp,
            bool apenasPpp,
            TalkDBTransaction banco = null
        )
        {
            ACA_ArquivoAreaDAO dao = banco == null ?
                    new ACA_ArquivoAreaDAO() : new ACA_ArquivoAreaDAO { _Banco = banco };
            return dao.SelectBy_Id_Dre_Escola(tad_id, esc_id, tne_id, incluirPpp, apenasPpp, uad_idSuperior);
        }

        /// <summary>
        /// Retorna ultimo aar_id do documentos da area
        /// </summary>
        /// <param name="tad_id">ID da area</param>
        /// <returns>ID do Documento da area</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int GetSelectUltimo_aar_idBy_tad_id
        (
            int tad_id
        )
        {
            try
            {
                ACA_ArquivoAreaDAO dao = new ACA_ArquivoAreaDAO();
                return dao.GetSelectUltimo_aar_idBy_tad_id(tad_id);
            }
            catch
            {
                throw;
            }
        }

        #endregion Métodos de consulta
    }
}