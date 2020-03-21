using DistributedId.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DistributedId
{
    /// <summary>
    /// Id序列，使用双buffer。
    /// </summary>
    internal class SequencedIdGenerator : IDistributedIdGenerator
    {
        private object _changeBufferLocker = new object();
        private IdBuffer _currentBuffer,_nextBuffer=null;
        private long _loadNextBufferThresholdLv1;
        private bool _loadingNextBuffer = false;
        private readonly SequencedIdGeneratorOptions _options;
        private readonly Random random=new Random();

        public SequencedIdGenerator(SequencedIdGeneratorOptions options,IdBuffer idBuffer)
        {
            _options = options;
            options.MachineRoomId &= 0x1f;

            _currentBuffer = idBuffer?? throw new ArgumentNullException(nameof(idBuffer));
            _loadNextBufferThresholdLv1 = idBuffer.Current + (long)((idBuffer.Max - idBuffer.Current) * 0.1);
        }
        #region IDistributedIdGenerator
        public long Next()
        {
            if (NeedPrepareNextBuffer())
            {
                SendPrepareNextBufferMessage();
            }
            IdBuffer buffer = _currentBuffer;
            long newId;
            if(buffer.TryNext(out newId))
            {
                return Process(newId);
            }
            else
            {
                UseNextBuffer(buffer);
                buffer = _currentBuffer;
                if(buffer.TryNext(out newId))
                {
                    return Process(newId);
                }
                throw new Exception("缓冲没有可用Id");
            }
        }

        public long[] Next(int count)
        {
            long[] ids = new long[count];
            for(int i = 0; i < count; i++)
            {
                ids[i] = Next();
            }
            return ids;
        }
        #endregion

        /// <summary>
        /// 对Id进行附加的加工处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private long Process(long id)
        {
            if (_options.UseMachineRoomId)
            {
                id = (id << 5 )| (long)(_options.MachineRoomId&0x1f);
            }
            if (_options.UseRandomTail)
            {
                id = (id << 5) | (long)(random.Next() & 0x1f);
            }
            return id;
        }
        /// <summary>
        /// 设置下一个buffer
        /// </summary>
        /// <param name="idBuffer"></param>
        public void SetNextBuffer(IdBuffer idBuffer)
        {
            //更新阈值
            _loadNextBufferThresholdLv1 = idBuffer.Current + (long)((idBuffer.Max - idBuffer.Current) * 0.1);
            _nextBuffer = idBuffer;
            _loadingNextBuffer = false;
        }

        /// <summary>
        /// 判断是否需要准备下一个buffer
        /// </summary>
        /// <returns></returns>
        private bool NeedPrepareNextBuffer()
        {
            return _nextBuffer==null && !_loadingNextBuffer && _currentBuffer.Current > _loadNextBufferThresholdLv1;
        }
        /// <summary>
        /// 发送准备下一个buffer的消息
        /// </summary>
        private void SendPrepareNextBufferMessage()
        {
            _loadingNextBuffer = true;
            //发送加载下一个缓冲Id的消息
            _options.NotifyLoadNextBuffer?.Invoke(_options.BusinessId);
        }

        /// <summary>
        /// 使用下一个Buffer
        /// </summary>
        /// <param name="oldBuffer"></param>
        private void UseNextBuffer(IdBuffer oldBuffer)
        {
            if (oldBuffer == _currentBuffer)
            {
                lock (_changeBufferLocker)
                {
                    if (oldBuffer == _currentBuffer)
                    {
                        if(_nextBuffer!=null && _nextBuffer.Current > _currentBuffer.Max)
                        {
                            _currentBuffer = _nextBuffer;
                            _nextBuffer = null;
                        }
                        else
                        {
                            throw new Exception("下一个Id缓冲未就绪！");
                        }
                    }
                }
            }
        }
    }
}
