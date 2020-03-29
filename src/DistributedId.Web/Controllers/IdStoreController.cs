using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedId.Core;
using DistributedId.Core.Entities;
using DistributedId.Web.Data;
using DistributedId.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DistributedId.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdStoreController : ControllerBase
    {
        private readonly ILogger<IdStoreController> _logger;
        private readonly IdStoreContext _idStoreContext;
        private readonly ISequencedIdStore _sequencedIdStore;

        public IdStoreController(ILogger<IdStoreController> logger, IdStoreContext idStoreContext, ISequencedIdStore _sequencedIdStore)
        {
            _logger = logger;
            _idStoreContext = idStoreContext;
            this._sequencedIdStore = _sequencedIdStore;
        }

        [HttpGet("{businessId}")]
        public SequencedIdBuffer Get(string businessId)
        {
            //var a= _idStoreContext.SequencedIds.FirstOrDefault(x => x.BusinessId == businessId);
            return _sequencedIdStore.GetBufferAsync(businessId).ConfigureAwait(true).GetAwaiter().GetResult();
            //return new SequencedIdEntity() {  BusinessId= businessId };
        }
    }
}
