using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId.Core.Entities
{
    public class SequencedIdBuffer:SequencedId
    {
        /// <summary>
        /// 当前可用Id
        /// </summary>
        public long CurrentId { get; set; }
    }
}
