using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;

namespace MSTech.GestaoEscolar.CustomResourceProviders
{

   /// <summary>
   /// Implementation of IResourceReader required to retrieve a dictionary
   /// of resource values for implicit localization. 
   /// </summary>
   public class DBResourceReader : DisposableBaseType, IResourceReader, IEnumerable<KeyValuePair<string, object>> 
   {

       private ListDictionary resourceDictionary;
       public DBResourceReader(ListDictionary resourceDictionary)
       {
           this.resourceDictionary = resourceDictionary;
       }

       protected override void Cleanup()
       {
           try
           {
               this.resourceDictionary = null;
           }
           finally
           {
               base.Cleanup();
           }
       }

       #region IResourceReader Members

       public void Close()
       {
           this.Dispose();
       }

       public IDictionaryEnumerator GetEnumerator()
       {

           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.resourceDictionary.GetEnumerator();
       }

       #endregion

       #region IEnumerable Members

       IEnumerator IEnumerable.GetEnumerator()
       {
           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.resourceDictionary.GetEnumerator();
       }

       #endregion
       
       #region IEnumerable<KeyValuePair<string,object>> Members

       IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
       {
           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.resourceDictionary.GetEnumerator() as IEnumerator<KeyValuePair<string, object>>;
       }

       #endregion
   }

}