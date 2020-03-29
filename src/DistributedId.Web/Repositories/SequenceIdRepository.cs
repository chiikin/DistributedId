using DistributedId.Core;
using DistributedId.Core.Entities;
using DistributedId.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DistributedId.Web.Models;

namespace DistributedId.Web.Repositories
{
    public class SequenceIdRepository : ISequencedIdStore
    {
        private readonly IdStoreContext _idStoreContext;
        public SequenceIdRepository(IdStoreContext idStoreContext)
        {
            _idStoreContext = idStoreContext;
        }

        public async Task<SequencedIdBuffer> GetBufferAsync(string businessId)
        {
            SequencedIdEntity sequencedId = null;
            using (var trans = await _idStoreContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                sequencedId = _idStoreContext.SequencedIds.FirstOrDefault(x => x.BusinessId == businessId);
                if (sequencedId == null)
                {
                    //创建默认记录
                    sequencedId = GetNewEntity(businessId);
                    _idStoreContext.SequencedIds.Add(sequencedId);
                    _idStoreContext.SaveChanges();
                    sequencedId = _idStoreContext.SequencedIds.FirstOrDefault(x => x.BusinessId == businessId);
                }
                long newMaxId = sequencedId.MaxId + sequencedId.Steps;
                long newRealMaxId = GetRealMaxId(newMaxId, sequencedId);

                sequencedId.MaxId = newMaxId;
                sequencedId.RealMaxId = newRealMaxId;
                sequencedId.UpdateTime = DateTime.Now;

                _idStoreContext.SequencedIds.Update(sequencedId);

                _idStoreContext.SaveChanges();
                trans.Commit();
            }
            return new SequencedIdBuffer()
            {
                BusinessId = businessId,
                MaxId = sequencedId.MaxId,
                UseMachineRoomId = sequencedId.UseMachineRoomId == 1,
                MachineRoomId = sequencedId.MachineRoomId,
                UseRandomTail = sequencedId.UseRandomTail == 1,
                Description = sequencedId.Description,
                RealMaxId = sequencedId.RealMaxId,
                Steps = sequencedId.Steps,
                CurrentId = sequencedId.MaxId - sequencedId.Steps + 1
            };
        }

        private long GetRealMaxId(long newMaxId, SequencedIdEntity entity)
        {
            if (entity.UseMachineRoomId == 1)
            {
                newMaxId = (newMaxId << 5) | 0x1f;
            }
            if (entity.UseRandomTail == 1)
            {
                newMaxId = (newMaxId << 5) | 0x1f;
            }
            return newMaxId;
        }

        private SequencedIdEntity GetNewEntity(string businessId)
        {
            var dt = DateTime.Now;
            return new SequencedIdEntity()
            {
                BusinessId = businessId,
                MaxId = 0,
                UseMachineRoomId = 0,
                MachineRoomId = 0,
                UseRandomTail = 1,
                Description = string.Empty,
                RealMaxId =0,
                Steps = 1000,
                CreateTime = dt,
                UpdateTime = dt
            };
        }
    }
}
