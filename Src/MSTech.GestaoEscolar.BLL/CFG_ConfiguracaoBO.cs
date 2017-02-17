using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Enumerador de sistema
    /// para configurações
    /// </summary>
    public enum eConfig
    {
        Academico,
        Biblioteca,
        InscricaoCreche,
        Merenda,
    }
    
    public class CFG_ConfiguracaoBO
    {
        /// <summary>
        /// Retorna as configurações ativas do sistema passado por parâmetro.
        /// </summary>
        /// <param name="config">Enumerador que indica de qual sistema será carregado as configurações</param>
        /// <param name="dictionary">Dicionário que será carregado com as configurações</param>
        public static void Consultar(eConfig config, out IDictionary<string, ICFG_Configuracao> dictionary)
        {
            dictionary = new Dictionary<string, ICFG_Configuracao>();
            switch (config)
            {
                case eConfig.Academico:
                    {
                        foreach (ICFG_Configuracao p in CFG_ConfiguracaoAcademicoBO.Consultar())
                            dictionary.Add(p.cfg_chave, p);
                        break;
                    }
                case eConfig.InscricaoCreche:
                    {
                        foreach (ICFG_Configuracao p in CFG_ConfiguracaoInscricaoCrecheBO.Consultar())
                            dictionary.Add(p.cfg_chave, p);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }
}
