using DistributedId.Core;
using DistributedId.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistributedId
{
    public sealed class DistributedIdFactory : IDistributedIdFactory
    {
        private readonly ISequencedIdStore _idSequenceStore;
        public DistributedIdFactory(ISequencedIdStore idSequenceStore)
        {
            _idSequenceStore = idSequenceStore;
        }
        public IDistributedIdGenerator GetSequencedIdGenerator(string bussinessId)
        {
            if (string.IsNullOrWhiteSpace(bussinessId))
            {
                throw new ArgumentNullException(nameof(bussinessId), "业务代码不能为空");
            }
            return _idSequences.GetOrAdd(bussinessId, CreateIdSequence);
        }

        #region 私有方法
        private readonly ConcurrentDictionary<string, SequencedIdGenerator> _idSequences = new ConcurrentDictionary<string, SequencedIdGenerator>();

        private SequencedIdGenerator CreateIdSequence(string businessId)
        {
            var idSequenceBuffer = _idSequenceStore.GetBufferAsync(businessId).ConfigureAwait(true).GetAwaiter().GetResult();
            var options = new SequencedIdGeneratorOptions()
            {
                UseMachineRoomId = idSequenceBuffer.UseMachineRoomId,
                MachineRoomId = idSequenceBuffer.MachineRoomId,
                UseRandomTail = idSequenceBuffer.UseRandomTail,
                NotifyLoadNextBuffer= NotifyLoadNextBuffer
            };
            IdBuffer idBuffer = new IdBuffer(idSequenceBuffer.CurrentId,idSequenceBuffer.MaxId);
            SequencedIdGenerator idSequence = new SequencedIdGenerator(options, idBuffer);
            return idSequence;
        }

        private void NotifyLoadNextBuffer(string businessId)
        {
            LoadNextBufferAsync(businessId);
        }

        private async Task LoadNextBufferAsync(string businessId)
        {
            var idSequenceBuffer =await _idSequenceStore.GetBufferAsync(businessId);

            IdBuffer idBuffer = new IdBuffer(idSequenceBuffer.CurrentId, idSequenceBuffer.MaxId);

            if(_idSequences.TryGetValue(businessId,out SequencedIdGenerator idSequence))
            {
                idSequence.SetNextBuffer(idBuffer);
            }
        }

        #endregion
    }
}
