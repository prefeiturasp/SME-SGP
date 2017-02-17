using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // representa uma coleção de viewstate providers.
    public class ViewStateProviderCollection : ProviderCollection
    {
        // retorna um viewState provider de uma lista, passando um nome.
        public new ViewStateProviderBase this[string name]
        {
            get { return base[name] as ViewStateProviderBase; }
        }
        
        // adiciona um viewstate provider para uma coleção de providers.
        public override void Add(ProviderBase provider)
        {
            if (provider != null && provider is ViewStateProviderBase)
            {
                base.Add(provider);
            }
        }
    }
}
