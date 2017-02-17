using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // representa a seção custom viewstate provider no web.config.
    // GoF Design Patterns: Factory.
    public class ViewStateProviderServiceSection : ConfigurationSection
    {
        // retorna uma coleção de viewstate providers do web.config.
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }


        // retorna ou informa o viewstate provider padrão.
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultProvider", DefaultValue = "ViewStateProviderCache")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("useProvider", DefaultValue = "ok")]
        public string UseProvider
        {
            get { return (string)base["useProvider"]; }
            set { base["useProvider"] = value; }
        }


    }
}
