﻿using Aggregates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates.Internal
{
    public class DefaultOOBHandler : IOOBHandler
    {
        private readonly IStoreEvents _store;

        public DefaultOOBHandler(IStoreEvents store)
        {
            _store = store;
        }


        public async Task Publish<T>(String Bucket, String StreamId, IEnumerable<IWritableEvent> Events, IDictionary<String, String> commitHeaders) where T : class, IEventSource
        {
            await _store.AppendEvents<T>(Bucket + ".OOB", StreamId, Events, commitHeaders).ConfigureAwait(false);

            // if the stream is new, also write stream metadata to limit the number of events stored
            // OOB events are by definition not mission critical so we can save a bit of space by scavaging old data
            if (Events.Any(x => x.Descriptor.Version == 0))
                await _store.WriteEventMetadata<T>(Bucket + ".OOB", StreamId, MaxCount: 200000).ConfigureAwait(false);
        }
        public Task<IEnumerable<IWritableEvent>> Retrieve<T>(String Bucket, String StreamId, Int32? Skip = null, Int32? Take = null, Boolean Ascending = true) where T : class, IEventSource
        {
            if(!Ascending)
                return _store.GetEventsBackwards<T>(Bucket + ".OOB", StreamId);
            return _store.GetEvents<T>(Bucket + ".OOB", StreamId);
        }
    }
}
