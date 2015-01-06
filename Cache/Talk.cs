using System;

namespace Cache
{
    public class Talk : ITalk
    {
        [Caching(CachingMethod.Get)]
        public System.Collections.Generic.List<string> GetData()
        {
            Data.UpData();
            return Data.GetData();
        }
    }
}
