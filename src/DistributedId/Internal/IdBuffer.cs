using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId
{
    /// <summary>
    /// id的buff结构，表示id可以选择的值的范围：Current&lt;id&lt;=Max
    /// </summary>
    internal class IdBuffer
    {
        public IdBuffer(long current,long max)
        {
            if (current < 0)
                throw new ArgumentOutOfRangeException("当前值小于0");
            if(max< current)
                throw new ArgumentOutOfRangeException("最大值小于当前值");
            _current = current;
            Max = max;
        }
        private long _current;

        public long Max { get; }
        public long Current { get => _current; }

        public bool TryNext(out long value)
        {
            value = 0; 
            long tmpValue;
            if (_current <= Max)
            {
                tmpValue = System.Threading.Interlocked.Increment(ref _current);
                if (tmpValue <= Max)
                {
                    value= tmpValue;
                    return true;
                }
            }
            return false;
        }
    }
}
