using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DistributedId.Core.Entities;

namespace DistributedId.Core
{
    public interface ISequencedIdStore
    {
        Task<SequencedIdBuffer> GetBufferAsync(string businessId);
    }
}
