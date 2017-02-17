using System;
using System.Collections.Generic;
using System.Text;
//using System.Web.Caching;
//using System.Web;
//using System.Web.SessionState;
using System.Linq;
using System.Web.UI;
//using System.Web.Configuration;

using System.IO;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject.ViewState;


namespace MSTech.GestaoEscolar.Web.WebProject.ViewState
{
    // Viewstate provider que é implementado usando o .Net normal.
    
    // Gof Design Pattern: Strategy.
    public class ViewStateProviderDefault : ViewStateProviderBase
    {
        // salva a informação do view state para uma página no cache
        public override void SavePageState(string name, object viewState)
        {
            //var cache = HttpContext.Current.Cache;
            ////var session = HttpContext.Current.Session;
            //int timeExpiration = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("ViewStateCacheExpiration"));

            ////cache.Add(name, viewState, null, DateTime.Now.AddMinutes(session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            //cache.Add(name, viewState, null, DateTime.Now.AddMinutes(timeExpiration), TimeSpan.Zero, CacheItemPriority.Default, null);

            //Pair pair1;
            //System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
            //Object ViewState;
            //if (pViewState is Pair)
            //{
            //    pair1 = ((Pair)pViewState);
            //    pageStatePersister1.ControlState = pair1.First;
            //    ViewState = pair1.Second;
            //}
            //else
            //{
            //    ViewState = pViewState;
            //}
            //LosFormatter mFormat = new LosFormatter();
            //StringWriter mWriter = new StringWriter();
            //mFormat.Serialize(mWriter, ViewState);
            //String mViewStateStr = mWriter.ToString();
            //byte[] pBytes = System.Convert.FromBase64String(mViewStateStr);
            //pBytes = Compressor.Compress(pBytes);
            //String vStateStr = System.Convert.ToBase64String(pBytes);
            //pageStatePersister1.ViewState = vStateStr;
            //pageStatePersister1.Save();


            throw new NotImplementedException();



        }

        
        // retorna informação do viewstate para a página do cache.
        public override object LoadPageState(string name)
        {
            throw new NotImplementedException();
            //return HttpContext.Current.Cache[name];
        }
    }
}
