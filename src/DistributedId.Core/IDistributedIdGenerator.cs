using System;

namespace DistributedId.Core
{
    /// <summary>
    /// id生成器
    /// </summary>
    public interface IDistributedIdGenerator
    {
        long Next();
        long[] Next(int count);
    }
}
