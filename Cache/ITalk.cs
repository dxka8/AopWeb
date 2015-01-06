using System.Collections.Generic;

namespace Cache
{
     public interface ITalk
     {
        [Caching(CachingMethod.Get)]
         List<string> GetData();
     }
}
