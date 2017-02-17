namespace MSTech.GestaoEscolar.CustomResourceProviders.Caching
{
        using MStech.Infrastructure.Caching;
        using MStech.Infrastructure.Caching.Memory;

        public static class CacheManager
        {
            private static ICacheManager cacheManager;

            /// <summary>
            /// Gerenciador de cache utilizado no sistema.
            /// </summary>
            public static ICacheManager Factory
            {
                get
                {
                    return cacheManager ?? new MemoryCacheManager();
                }
            }
        }
}
