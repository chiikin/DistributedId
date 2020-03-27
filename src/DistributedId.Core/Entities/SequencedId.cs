using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId.Core.Entities
{
    public class SequencedId
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 最大Id
        /// </summary>
        public long MaxId { get; set; }
        /// <summary>
        /// 步长
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
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
        /// 真实最大id，UseMachineRoomId或UseRandomTail为true时，MaxId不等于真实使用的Id，则此字段显示真实使用的最大MaxId
        /// </summary>
        public long RealMaxId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}
