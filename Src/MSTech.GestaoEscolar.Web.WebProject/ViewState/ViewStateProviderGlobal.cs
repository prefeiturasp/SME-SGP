using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // Viewstate provider que é implementado usando um global singleton hashtable.
    
    // Gof Design Pattern: Strategy.
    public class ViewStateProviderGlobal : ViewStateProviderBase
    {
        // salva o view state para a página em uma variável global
        public override void SavePageState(string name, object viewState)
        {
            GlobalViewStateSingleton.Instance.ViewStates.Add(name,viewState);
        }

        // retorna o viewstate para a página da variável global
        public override object LoadPageState(string name)
        {
            return GlobalViewStateSingleton.Instance.ViewStates[name];
        }
    }
}
