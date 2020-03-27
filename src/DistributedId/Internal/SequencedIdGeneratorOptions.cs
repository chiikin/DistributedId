using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId
{

    internal delegate void NotifyLoadNextBufferDelegate(string businessId);

    internal class SequencedIdGeneratorOptions
    {
        public string BusinessId { get; set; }
        /// 是否使用机房Id，如果为true，则MaxId左移，并在尾部增加机房的id。机房号占用5bit
        /// </summary>
        public bool UseMachineRoomId { get; set; }
        /// <summary>
        /// 机房Id，最大值32，即5bit
        /// </summary>
        public short MachineRoomId { get; set; }
        /// <summary>
        /// 是否使用随机尾数，如果为true，则MaxId左移，并在尾部增加随机数。随机数占用5bit。
        /// </summary>
        public bool UseRandomTail { get; set; }

        /// <summary>
        /// 通知加载下一个Buffer
        /// </summary>
        public NotifyLoadNextBufferDelegate NotifyLoadNextBuffer { get; set; }
    }
}
