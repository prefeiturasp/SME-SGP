/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Drawing;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    /// <summary>
    /// Enumerador da situação do arquivo.
    /// </summary>
    public enum SYS_ArquivoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
            , Temporario = 4
    }

    #endregion Enumerador

    /// <summary>
    /// SYS_Arquivo Business Object
    /// </summary>
    public class SYS_ArquivoBO : BusinessBase<SYS_ArquivoDAO, SYS_Arquivo>
    {
        /// <summary>
        /// Valida se o arquivo é válido
        /// </summary>
        /// <param name="entity">entity SYS_Arquivo</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo permitido em KB</param>
        /// <param name="TiposArquivosPermitidos">Array com os arquivos permitidos</param>
        /// <returns>True se OK</returns>
        /// <exception cref="ValidationException">Throw quando arquivo for inválido</exception>
        private static bool ValidarArquivo(SYS_Arquivo entity, int tamanhoMaximoKB, string[] TiposArquivosPermitidos)
        {
            return ValidarTamanhoArquivo(entity, tamanhoMaximoKB) && ValidarTipoArquivo(entity, TiposArquivosPermitidos);
        }

        /// <summary>
        /// Valida se o arquivo é válido
        /// </summary>
        /// <param name="entity">entity SYS_Arquivo</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo permitido em KB</param>
        /// <returns>True se OK</returns>
        /// <exception cref="ValidationException">Throw quando o tamanho exceder o tamanho máximo.</exception>
        private static bool ValidarTamanhoArquivo(SYS_Arquivo entity, int tamanhoMaximoKB)
        {
            bool ret;
            int tamanhoBytes = 1024;
            int tamArquivo = entity.arq_data.Length / tamanhoBytes;

            if (tamArquivo <= tamanhoMaximoKB)
                ret = true;
            else
                throw new ValidationException(String.Format("O arquivo \"{0}\"  excede o limite de {1}MB para anexos.", entity.arq_nome, (tamanhoMaximoKB / 1024)));

            return ret;
        }

        /// <summary>
        /// Valida se o arquivo é válido
        /// </summary>
        /// <param name="entity">entity SYS_Arquivo</param>
        /// <param name="TiposArquivosPermitidos">Array com os arquivos permitidos</param>
        /// <returns>True se OK</returns>
        /// <exception cref="ValidationException">Throw quando a extensão não é permitida.</exception>
        private static bool ValidarTipoArquivo(SYS_Arquivo entity, string[] TiposArquivosPermitidos)
        {
            bool ret;
            string extensao = Path.GetExtension(entity.arq_nome);

            if (TiposArquivosPermitidos.Contains(extensao))
                ret = true;
            else
                throw new ValidationException(String.Format("Arquivos do tipo \"{0}\" não são permitidos.", extensao));

            return ret;
        }

        /// <summary>
        /// Salva o objeto SYS_Arquivo
        /// </summary>
        /// <param name="entity">entity SYS_Arquivo</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo permitido em KB</param>
        /// <param name="TiposArquivosPermitidos">Array com os arquivos permitidos</param>
        /// <param name="banco">Conexão com o Banco de dados</param>
        /// <returns>True se OK</returns>
        /// <exception cref="ValidationException">Throw quando arquivo for inválido</exception>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public static bool Save(SYS_Arquivo entity, int tamanhoMaximoKB, string[] TiposArquivosPermitidos, TalkDBTransaction banco)
        {
            bool ret = false;

            if (ValidarArquivo(entity, tamanhoMaximoKB, TiposArquivosPermitidos))
                ret = Save(entity, banco);

            return ret;
        }

        /// <summary>
        /// Salva o objeto SYS_Arquivo
        /// </summary>
        /// <param name="entity">entity SYS_Arquivo</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo permitido em KB</param>
        /// <param name="TiposArquivosPermitidos">Array com os arquivos permitidos</param>
        /// <returns>True se OK</returns>
        /// <exception cref="ValidationException">Throw quando arquivo for inválido</exception>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public static bool Save(SYS_Arquivo entity, int tamanhoMaximoKB, string[] TiposArquivosPermitidos)
        {
            SYS_ArquivoDAO dao = new SYS_ArquivoDAO();
            return Save(entity, tamanhoMaximoKB, TiposArquivosPermitidos, dao._Banco);
        }

        /// <summary>
        /// Monta uma entidade de arquivo de acordo com o documento passado.
        /// </summary>
        /// <param name="postedFile">Documento usado para upload.</param>
        /// <returns>Entidade de arquivo.</returns>
        public static SYS_Arquivo CriarAnexo(HttpPostedFile postedFile)
        {
            SYS_Arquivo entityArquivo;

            if (!string.IsNullOrEmpty(postedFile.FileName))
            {
                string nome = Path.GetFileName(postedFile.FileName);

                entityArquivo = new SYS_Arquivo
                {
                    arq_nome = nome
                    ,
                    arq_tamanhoKB = postedFile.ContentLength
                    ,
                    arq_typeMime = postedFile.ContentType
                    ,
                    arq_data = GetBytesFromHttpPostedFile(postedFile)
                    ,
                    arq_situacao = (byte)SYS_ArquivoSituacao.Ativo
                    ,
                    arq_dataCriacao = DateTime.Now
                    ,
                    arq_dataAlteracao = DateTime.Now
                };

                return entityArquivo;
            }

            entityArquivo = null;
            return entityArquivo;
        }

        /// <summary>
        /// Retorna o array de Bytes do arquivo do HttpPostedFile
        /// </summary>
        /// <param name="postedFile">HttpPostedFile</param>
        /// <returns>Array de Bytes</returns>
        public static byte[] GetBytesFromHttpPostedFile(HttpPostedFile postedFile)
        {
            byte[] file = null;

            if ((postedFile != null) && (postedFile.InputStream != null))
            {
                int Tamanho = Convert.ToInt32(postedFile.InputStream.Length);
                file = new byte[Tamanho];

                if (postedFile.InputStream.Length == 0)
                    throw new ValidationException("O arquivo tem 0 bytes, por isso ele não será anexado.");

                postedFile.InputStream.Read(file, 0, Tamanho);
            }
            return file;
        }

        /// <summary>
        /// Retorna Image do arquivo do arq_id(apenas arq_typeMime == "image/jpeg")
        /// </summary>
        /// <param name="arq_id"></param>
        /// <returns>Image</returns>
        public static Image SelecionaImagemPorArquivo(int arq_id, int appMinutosCacheCurto = 360)
        {

            SYS_Arquivo entiSysArquivo = new SYS_Arquivo();

            if (appMinutosCacheCurto > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = string.Format("Cache_SelecionaImagemPorArquivo{0}", arq_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        entiSysArquivo = new SYS_ArquivoDAO().NEW_SYS_Arquivo_SelecionaPorArquivo(arq_id);

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, entiSysArquivo, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        entiSysArquivo = (SYS_Arquivo)cache;
                    }
                }
            }

            if (entiSysArquivo == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                entiSysArquivo = new SYS_ArquivoDAO().NEW_SYS_Arquivo_SelecionaPorArquivo(arq_id);
            }
            

            if ((entiSysArquivo.arq_typeMime == "image/jpeg" || entiSysArquivo.arq_typeMime == "image/png") && entiSysArquivo.arq_tamanhoKB > 0)
            {
                MemoryStream ms = new MemoryStream(entiSysArquivo.arq_data);
                ms.Seek(0, SeekOrigin.Begin);
                return Image.FromStream(ms, true, true);
            }
            
            return null;
        }

        /// <summary>
        /// Método que faz a exclusão física.
        /// </summary>
        /// <param name="entity"> Entidade SYS_Arquivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public static bool ExcluiFisicamente(SYS_Arquivo entity)
        {
            SYS_ArquivoDAO dao = new SYS_ArquivoDAO();
            return dao.ExcluiFisicamente(entity);
        }
    }
}