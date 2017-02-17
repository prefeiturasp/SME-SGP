using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject.ViewState;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class MotherPageLogadoCompressedViewState : MotherPageLogado
    {
        #region ViewState Provider Service Access

        protected override void SavePageStateToPersistenceMedium(object pViewState)
        {
            //############
            //Uma outra opção é se usar um Provider, envia o ViewState direto, sem compactação.
            //Bastaria apenas fazer uma condição ( if (ViewStateProviderService.UseProvider) ) e chamar a linha abaixo.
            //base.SavePageStateToPersistenceMedium(pViewState);

            if (ViewStateProviderService.UseProvider)
            {
                base.SavePageStateToPersistenceMedium(pViewState);
            }
            else
            {
                Pair pair1;
                System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
                Object ViewState;
                if (pViewState is Pair)
                {
                    pair1 = ((Pair)pViewState);
                    pageStatePersister1.ControlState = pair1.First;
                    ViewState = pair1.Second;
                }
                else
                {
                    ViewState = pViewState;
                }

                LosFormatter mFormat = new LosFormatter();
                StringWriter mWriter = new StringWriter();
                mFormat.Serialize(mWriter, ViewState);
                String mViewStateStr = mWriter.ToString();
                byte[] pBytes = System.Convert.FromBase64String(mViewStateStr);
                pBytes = Compressor.Compress(pBytes);
                String vStateStr = System.Convert.ToBase64String(pBytes);
                //pageStatePersister1.ViewState = vStateStr;
                pageStatePersister1.Save();

                //##### - Precisa averiguar se há uma forma compactar o pair1.First que é uma HybridDictionary.
                //##### Do jeito que está, está compactando apenas o ViewState Pair e não o ViewState do Controle.

                //Este irá salvar no hidden viewstate padrão ou no custom hidden viewstate_key.
                //Ao salvar no hidden viewstate padrão o campo "pageStatePersister1.ViewState" acima, será preenchido. 
                base.SavePageStateToPersistenceMedium(vStateStr);
            }
        }


        protected override object LoadPageStateFromPersistenceMedium()
        {
            //#########
            //Esta é a opção usando um Provider que não está usando a compactação, 
            //porém grava no provider o ViewState de tudo inclusive do ControlState.
            if (ViewStateProviderService.UseProvider)
            {
                return base.LoadPageStateFromPersistenceMedium();
            }
            else
            {
                System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
                pageStatePersister1.Load();
                String vState = String.Empty;
                //#########- 
                //Esta é a opção usando um Provider, que está usando a compactação, 
                //porém, apenas do ViewState e não do ControlState(permanecendo no fonte da página).
                //if (ViewStateProviderService.UseProvider)
                //{
                //    string name = Request.Form["__VIEWSTATE_KEY"];
                //    string customViewStateHidden_Key = (string)ViewStateProviderService.LoadPageState(name);
                //    vState = customViewStateHidden_Key;
                //}
                //else
                //{
                    vState = pageStatePersister1.ViewState.ToString();
                //}

                byte[] pBytes = System.Convert.FromBase64String(vState);
                pBytes = Compressor.Decompress(pBytes);
                LosFormatter mFormat = new LosFormatter();
                Object ViewState = mFormat.Deserialize(System.Convert.ToBase64String(pBytes));

                return new Pair(pageStatePersister1.ControlState, ViewState);
            }
        }

        #endregion

    }
}
