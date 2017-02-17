using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // Define os métodos que o viewstate providers devem implementar.
    
    // Design Pattern: Microsoft's Provider Design Pattern.
    // GoF Design Patterns: Strategy, Factory, Singleton.
    // 
    // Strategy Pattern porque o cliente tem a opção de escolher a estratégia de provedor viewstate que irá utilizar.
    // Singleton Pattern porque GlobalViewStateSingleton é um Singleton que mantém uma lista de todos os viewstate providers disponíveis.
    // Factory Pattern porque os viewstate providers são criados(instanciados) de acordo com as configurações do web.config.

    public abstract class ViewStateProviderBase : ProviderBase
    {
        // salva qualquer informação da view ou control state para a página.
        public abstract void SavePageState(string name, object viewState);

        
        // retorna/lê qualquer informação da view ou control state da página.
        public abstract object LoadPageState(string name);
    }
}
