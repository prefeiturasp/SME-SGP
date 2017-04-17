/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.ComponentModel;
    using MSTech.Data.Common;
    using System.Web;
    using System.Web.Caching;
    using MSTech.GestaoEscolar.BLL.Caching;

    #region Enumeradores

    /// <summary>
    /// Enumerador da situação da permissão do docente.
    /// </summary>
    public enum EnumPermissaoDocenteSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
    }

    /// <summary>
    /// Enumerador das permissões dos docentes para os módulos.
    /// </summary>
    public enum EnumModuloPermissao : byte
    {
        [Description("Aula")]
        Aula = 1
        ,
        [Description("Plano de aula")]
        PlanoAula = 2
        ,
        [Description("Anotações")]
        Anotacoes = 3
        ,
        [Description("Planejamento anual")]
        PlanejamentoAnual = 4
        ,
        [Description("Frequência")]
        Frequencia = 5
        ,
        [Description("Avaliações")]
        Avaliacoes = 6
        ,
        [Description("Compensações")]
        Compensacoes = 7
        ,
        [Description("Efetivação")]
        Efetivacao = 8
        ,
        [Description("Boletim")]
        Boletim = 9
        ,
        [Description("Indicadores")]
        Indicadores = 10
        ,
        [Description("Objetos de conhecimento")]
        ObjetosAprendizagem = 11
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Estrutura para controle de permissão do docente.
    /// </summary>
    [Serializable]
    public struct sPermissaoDocente
    {
        public byte tdc_idPermissao { get; set; }
        public byte tdt_posicaoPermissao { get; set; }
        public bool pdc_permissaoConsulta { get; set; }
        public bool pdc_permissaoEdicao { get; set; }
    }

    #endregion

    /// <summary>
	/// Description: CFG_PermissaoDocente Business Object. 
	/// </summary>
	public class CFG_PermissaoDocenteBO : BusinessBase<CFG_PermissaoDocenteDAO, CFG_PermissaoDocente>
	{
        #region Métodos de consulta
				
        /// <summary>
        /// O método retorna as permissões para o docente no módulo passado por parâmetro.
        /// </summary>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <param name="pdc_modulo">Módulo do sistema.</param>
        /// <returns></returns>
        public static List<sPermissaoDocente> SelecionaPermissaoModulo(byte tdt_posicao, byte pdc_modulo)
        {
            // Se for nova turma, verificar se está em cache, não buscar do banco.
            string chave = String.Format(ModelCache.PERMISSAODOCENTE_PERMISSAOMODULO_MODEL_KEY, tdt_posicao, pdc_modulo);

            List<sPermissaoDocente> dados;

            dados = CacheManager.Factory.Get(
                        chave,
                        () =>
                        {
                            return (from DataRow dr in new CFG_PermissaoDocenteDAO().SelecionaPermissaoModulo(tdt_posicao, pdc_modulo).Rows
                                    select new sPermissaoDocente
                                    {
                                        tdc_idPermissao = Convert.ToByte(dr["tdc_idPermissao"])
                                        ,
                                        tdt_posicaoPermissao = Convert.ToByte(dr["tdt_posicaoPermissao"])
                                        ,
                                        pdc_permissaoConsulta = Convert.ToBoolean(dr["pdc_permissaoConsulta"])
                                        ,
                                        pdc_permissaoEdicao = Convert.ToBoolean(dr["pdc_permissaoEdicao"])
                                    }).ToList();
                        },
                        GestaoEscolarUtilBO.MinutosCacheLongo
                    );

            return dados;
        }

        /// <summary>
        /// Retorna as permissões de cada tipo de docente de cada módulo.
        /// </summary>
        /// <returns>Lista de permissões por módulo</returns>
        public static List<CFG_PermissaoDocente> SelecionarPermissaoModulo()
        {
            // Se for nova turma, verificar se está em cache, não buscar do banco.
            string chave = ModelCache.PERMISSAODOCENTE_PERMISSAOMODULO_PATTERN_KEY;

            List<CFG_PermissaoDocente> lst;

            lst = CacheManager.Factory.Get(
                        chave,
                        () =>
                        {
                            return new CFG_PermissaoDocenteDAO().SelecionarPermissaoModulo();
                        },
                        GestaoEscolarUtilBO.MinutosCacheLongo
                    );

            return lst;
        }

        /// <summary>
        /// Salva as permissões no banco
        /// </summary>
        /// <param name="lstPermDoc">Lista com as permissões a serem gravadas no banco</param>
        /// <returns>Lista de permissões</returns>
        public static bool Salvar(List<CFG_PermissaoDocente> lstPermDoc)
        {
            TalkDBTransaction bancoGestao = new CFG_PermissaoDocenteDAO()._Banco.CopyThisInstance();

            try
            {
                bancoGestao.Open();

                CacheManager.Factory.RemoveByPattern(ModelCache.PERMISSAODOCENTE_PERMISSAOMODULO_PATTERN_KEY);

                foreach (CFG_PermissaoDocente item in lstPermDoc)
                {
                    item.IsNew = item.pdc_id == 0;
                    CFG_PermissaoDocenteBO.Save(item, bancoGestao);
                }
            }
            catch (Exception e)
            {
                bancoGestao.Close(e);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                {
                    bancoGestao.Close();
                }
            }

            return true;
        }

        #endregion
	}
}