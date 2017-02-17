/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Collections.Generic;
    using MSTech.Validation.Exceptions;
    using System.Data;
    using MSTech.Data.Common;
    using System;
    using System.Linq;
    using System.Web;
    using System.Reflection;
    using MSTech.GestaoEscolar.BLL.Caching;

    #region Enumeradores

    /// <summary>
    /// Enumerador do tipo de docente.
    /// </summary>
    public enum EnumTipoDocente : byte
    {
        [Description("Titular")]
        Titular = 1
        ,
        [Description("Compartilhado")]
        Compartilhado = 2
        ,
        [Description("Projeto")]
        Projeto = 3
        ,
        [Description("Substituto")]
        Substituto = 4
        ,
        [Description("Especial")]
        Especial = 5
        ,
        [Description("Segundo Titular")]
        SegundoTitular = 6
    }

    /// <summary>
    /// Enumerador da situação do tipo de docente.
    /// </summary>
    public enum EnumTipoDocenteSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
    }

    #endregion

    /// <summary>
    /// Description: ACA_TipoDocente Business Object. 
    /// </summary>
    public class ACA_TipoDocenteBO : BusinessBase<ACA_TipoDocenteDAO, ACA_TipoDocente>
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna uma lista com os valores de um determinado enumerador.
        /// </summary>
        /// <param name="enumerador">Enumerador que terá os valores listados.</param>
        /// <returns>Lista com descrição e valores dos elementos do enumerador.</returns>
        public static Dictionary<byte, string> ListaTipoDocentes()
        {
            Dictionary<byte, string> dic = new Dictionary<byte, string>();

            dic = (from Enum valor in Enum.GetValues(typeof(EnumTipoDocente))
                   select new
                   {
                       chave = (byte)((EnumTipoDocente)valor)
                       ,
                       valor = DescricaoEnum(valor)
                   }).ToDictionary(p => p.chave, p => p.valor);
            return dic;
        }

        /// <summary>
        /// Retorna a descrição de um determinado elemento de um Enumerador.
        /// </summary>
        /// <param name="elemento">Elemento do enumerador de onde a descrição será retornada.</param>
        /// <returns>String com a descrição do elemento do Enumerador.</returns>
        public static string DescricaoEnum(Enum elemento)
        {
            FieldInfo infoElemento = elemento.GetType().GetField(elemento.ToString());
            DescriptionAttribute[] atributos = (DescriptionAttribute[])infoElemento.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (atributos.Length > 0)
            {
                if (atributos[0].Description != null)
                {
                    return atributos[0].Description;
                }

                return "Titular";
            }

            return elemento.ToString();
        }
        
        /// <summary>
        /// Retorna a posição do tipo de docente.
        /// </summary>
        /// <param name="tipoDocente">Tipo de docente (enum).</param>
        /// <param name="appMinutosCacheLongo">Minutos de cache configurados para o cache longo.</param>
        /// <returns>Posição do docente.</returns>
        public static byte SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente tipoDocente, int appMinutosCacheLongo)
        {
            byte posicao;

            Func<byte> retorno = delegate()
            {
                return new ACA_TipoDocenteDAO().SelecionaPosicaoPorTipoDocente((byte)tipoDocente);
            };

            if (appMinutosCacheLongo > 0)
            {
                posicao = CacheManager.Factory.Get(
                    String.Format(ModelCache.TIPO_DOCENTE_POSICAO_POR_TIPO_DOCENTE_MODEL_KEY, (byte)tipoDocente),
                    retorno,
                    appMinutosCacheLongo
                );
            }
            else
            {
                posicao = retorno();
            }

            return posicao;
        }

        /// <summary>
        /// Seleciona todos os tipos de docentes ativos.
        /// </summary>
        /// <returns></returns>
        public static List<ACA_TipoDocente> SelecionaAtivos(int appMinutosCacheLongo = 0)
        {
            List<ACA_TipoDocente> dados;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                // Busca do cache os dados.
                string chave = RetornaChaveCache_TipoDocente();
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    // Retorna todos os tipos de docentes no cache.
                    dados = SelecionaAtivos();

                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<ACA_TipoDocente>)cache;
                }
            }
            else
            {
                dados = new ACA_TipoDocenteDAO().SelecionaAtivos();
            }

            return dados;
        }

        /// <summary>
        /// Retorna o tipo de docente pela posição.
        /// </summary>
        /// <param name="tdc_posicao">Posição do docente.</param>
        /// <param name="appMinutosCacheLongo">Minutos de cache configurados para o cache longo.</param>
        /// <returns>Tipo do docente.</returns>
        public static EnumTipoDocente SelecionaTipoDocentePorPosicao
        (
            byte tdc_posicao
            , int appMinutosCacheLongo = 0
        )
        {
            byte tipoDocente;
            if (tdc_posicao > 0)
            {
                if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
                {
                    // Busca do cache os dados.
                    string chave = RetornaChaveCache_TipoDocente();
                    object cache = HttpContext.Current.Cache[chave];
                    List<ACA_TipoDocente> dados;

                    if (cache == null)
                    {
                        // Retorna todos os tipos de docentes no cache.
                        dados = SelecionaAtivos();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo)
                            , System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<ACA_TipoDocente>)cache;
                    }

                    tipoDocente = (from ACA_TipoDocente item in dados
                                   where item.tdc_posicao == tdc_posicao
                                   select item.tdc_id).FirstOrDefault();
                }
                else
                {
                    tipoDocente = new ACA_TipoDocenteDAO().SelecionaTipoDocentePorPosicao(tdc_posicao);
                }
            }
            else
            {
                tipoDocente = (byte)EnumTipoDocente.Titular;
            }

            Array tiposDocente = Enum.GetValues(typeof(EnumTipoDocente));

            if (tiposDocente.Cast<byte>().Any(p => p == tipoDocente))
            {
                return (EnumTipoDocente)tipoDocente;
            }

            throw new ValidationException("Posição do docente inválida.");
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_TipoDocente()
        {
            return "Cache_SelecionaTipoDocente";
        }

        /// <summary>
        /// Verifica se já existe um tipo de docente cadastrado com o mesmo tipo
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoDocente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTipoDocenteExistente
        (
            ACA_TipoDocente entity
        )
        {
            ACA_TipoDocenteDAO dao = new ACA_TipoDocenteDAO();
            return dao.SelectBy_TipoDocente(entity.tdc_id); //, entity.tdc_descricao);
        }

        /// <summary>
        /// Verifica se já existe o tipo de docente cadastrado com situação 3
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoDocente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTipoDocenteSeExcluido
        (
            ACA_TipoDocente entity
        )
        {
            ACA_TipoDocenteDAO dao = new ACA_TipoDocenteDAO();
            return dao.SelectBy_TipoDocente_Situacao(entity.tdc_id);
        }

        /// <summary>
        /// Retornar se existe outro tipo de docente cadastrado na mesma posição.
        /// </summary>
        /// <param name="tdc_id">id do tipo do docente</param>
        /// <param name="tdc_posicao">posicao cadastrada para o tipo de docente</param>
        public static bool VerificaDuplicidadePorPosicao(ACA_TipoDocente entity)
        {
            ACA_TipoDocenteDAO dao = new ACA_TipoDocenteDAO();
            return dao.VerificaDuplicidadePorPosicao(entity.tdc_id, entity.tdc_posicao);
        }

        #endregion

        #region Métodos de save

        /// <summary>
        /// Inclui ou altera o tipo do docente
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoDocente</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_TipoDocente entity
        )
        {
            if (entity.Validate())
            {
                if (VerificaTipoDocenteSeExcluido(entity))
                {
                    // setou falso para ser chamado a SP responsável por fazer um update, isso será executado somente se
                    // estiver incluindo um registro que já existe na tabela mas com situação = 3 (excluido)
                    entity.IsNew = false;
                }
                else if (VerificaTipoDocenteExistente(entity))
                {
                    if (entity.IsNew)
                    {   // executa qdo é tentado cadastrar um tipo que já está cadastrado.
                        throw new DuplicateNameException("Já existe um tipo de docente cadastrado com esse tipo.");
                    }
                }
                
                //Limpa o cache
                CacheManager.Factory.Remove(string.Format(ModelCache.TIPO_DOCENTE_POSICAO_POR_TIPO_DOCENTE_MODEL_KEY, entity.tdc_id));
                HttpContext.Current.Cache.Remove(RetornaChaveCache_TipoDocente());
                
                ACA_TipoDocenteDAO dao = new ACA_TipoDocenteDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        #endregion

    }
}