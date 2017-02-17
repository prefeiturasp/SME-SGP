using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.CustomResourceProviders.Caching;
using System;

namespace MSTech.GestaoEscolar.CustomResourceProviders
{
    /// <summary>
    /// Data access component for the StringResources table. 
    /// This type is thread safe.
    /// </summary>
    public class stringResources
    {

        /// <summary>
        /// Selecionas the por nome cultura codigo.
        /// </summary>
        /// <param name="rvr_NomeResource">The RVR_ nome resource.</param>
        /// <param name="rvr_cultura">The rvr_cultura.</param>
        /// <param name="rvr_codigo">The rvr_codigo.</param>
        /// <returns></returns>
        /// <author>juliano.real</author>
        /// <datetime>29/05/2014-16:45</datetime>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Dictionary<string, string> SelecionaPorNomeCulturaCodigo(
            string rcr_NomeResource
            , string rcr_cultura
            )
        {
            Dictionary<string, string> resCache = new Dictionary<string, string>();

            Func<Dictionary<string, string>> retorno = delegate ()
            {
                return resCache = new RES_ChaveResourceDAO().SelecionaPorNomeCulturaCodigo(rcr_NomeResource, rcr_cultura)
                    .Select().GroupBy(p => p["chave"].ToString()).ToDictionary(p => p.Key, p => p.First()["valor"].ToString());
            };

            string chave = string.Format("Cache_Resource_{0}_{1}", rcr_NomeResource, rcr_cultura);

            resCache = CacheManager.Factory.Get
                        (
                            chave,
                            retorno,
                            1440
                        );

            return resCache;
        }

    }

    public class resourceRecord
    {
        public string ResourceType { get; set; }

        public string CultureCode { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceValue { get; set; }
    }
}
