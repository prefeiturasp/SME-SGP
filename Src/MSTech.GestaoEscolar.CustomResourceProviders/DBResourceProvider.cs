using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web.Compilation;

namespace MSTech.GestaoEscolar.CustomResourceProviders
{
    /// <summary>
    /// Resource provider accessing resources from the database.
    /// This type is thread safe.
    /// </summary>
    public class DBResourceProvider : DisposableBaseType, IResourceProvider
    {
        private string classKey;
        private stringResources dalc;

        // resource cache
        private ConcurrentDictionary<string, Dictionary<string, string>> resourceCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Constructs this instance of the provider
        /// supplying a resource type for the instance.
        /// </summary>
        /// <param name="classKey">The class key.</param>
        public DBResourceProvider(string classKey)
        {
            this.classKey = classKey;
        }

        #region IResourceProvider Members

        /// <summary>
        /// Retrieves a resource entry based on the specified culture and
        /// resource key. The resource type is based on this instance of the
        /// DBResourceProvider as passed to the constructor.
        /// To optimize performance, this function caches values in a dictionary
        /// per culture.
        /// </summary>
        /// <param name="resourceKey">The resource key to find.</param>
        /// <param name="culture">The culture to search with.</param>
        /// <returns>
        /// If found, the resource string is returned.
        /// Otherwise an empty string is returned.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">DBResourceProvider object is already disposed.</exception>
        /// <exception cref="System.ArgumentNullException">resourceKey</exception>
        public object GetObject(string resourceKey, CultureInfo culture)
        {

            if (Disposed)
            {
                throw new ObjectDisposedException("DBResourceProvider object is already disposed.");
            }

            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }

            if (culture == null || string.IsNullOrEmpty(culture.Name))
            {
                culture = CultureInfo.GetCultureInfo("pt-BR");
            }

            string resourceValue = null;
            Dictionary<string, string> resCacheByCulture = null;

            if (!resourceCache.ContainsKey(culture.Name))
            {
                resCacheByCulture = new stringResources().SelecionaPorNomeCulturaCodigo(classKey, culture.Name);
                resourceCache.GetOrAdd(culture.Name, resCacheByCulture);
            }
            else
            {
                resCacheByCulture = resourceCache[culture.Name];
            }

            if ((resCacheByCulture != null) && (resCacheByCulture.ContainsKey(resourceKey)))
            {
                resourceValue = resCacheByCulture[resourceKey];
            }

            else
            {
                resourceValue = resourceKey;
            }

            return resourceValue;
        }

        public IResourceReader ResourceReader { get; private set; }

        #endregion

        protected override void Cleanup()
        {
            try
            {
                this.resourceCache.Clear();
            }
            finally
            {
                base.Cleanup();
            }
        }

    }
}