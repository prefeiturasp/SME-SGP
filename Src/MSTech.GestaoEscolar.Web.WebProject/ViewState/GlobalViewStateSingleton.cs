using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // GlobalViewStateSingleton mantém uma lista de viewstates em uma hashtable globalmente acessível. 
    // Este é uma classe de helper Singleton para a classe ViewStateProviderGlobal.
    
    // Gof Design Pattern: Singleton.
    public class GlobalViewStateSingleton
    {
        #region The Singleton definition

        // esta é a única instance desta classe
        private static readonly GlobalViewStateSingleton _instance = new GlobalViewStateSingleton();

        // private constructor for GlobalViewStateSingleton.
        // previne outros de criar instâncias adicionais.
        
        private GlobalViewStateSingleton()
        {
            ViewStates = new Dictionary<string, object>();
        }

        
        // retorna a única instência da classe GlobalViewStateSingleton
        public static GlobalViewStateSingleton Instance
        {
            get { return _instance; }
        }

        #endregion

        
        // retorna uma lista de ViewStates.
        public Dictionary<string, object> ViewStates { get; private set; }
    }
}
