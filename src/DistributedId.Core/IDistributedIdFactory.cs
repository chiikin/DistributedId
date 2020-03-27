using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId.Core
{
    public interface IDistributedIdFactory
    {
        /// <summary>
        /// 获取有序的Id生成器
        /// </summary>
        /// <param name="bussinessId"></param>
        /// <returns></returns>
        IDistributedIdGenerator GetSequencedIdGenerator(string bussinessId);
    }
}
