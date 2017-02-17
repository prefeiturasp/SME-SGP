using System.Web;
using System.Web.SessionState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // Viewstate provider que é implementado usando a Session.

    // Gof Design Pattern: Strategy.
    public class ViewStateProviderSession : ViewStateProviderBase
    {
        // salva a informação do view state para a página no objeto Session
        public override void SavePageState(string name, object viewState)
        {
            var session = HttpContext.Current.Session;
            session[name] = viewState;
        }
        
        // retorna informação do viewstate para a página da Session
        public override object LoadPageState(string name)
        {
            var session = HttpContext.Current.Session;
            return session[name];
        }
    }
}
