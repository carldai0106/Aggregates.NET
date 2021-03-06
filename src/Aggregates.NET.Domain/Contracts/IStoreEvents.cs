﻿using NServiceBus.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates.Contracts
{
    public interface IStoreEvents
    {
        Task<IEventStream> GetStream<T>(String bucket, String streamId, Int32? start = null) where T : class, IEventSource;
        Task<IEnumerable<IWritableEvent>> GetEvents<T>(String bucket, String streamId, Int32? start = null, Int32? count = null) where T : class, IEventSource;
        Task<IEnumerable<IWritableEvent>> GetEventsBackwards<T>(String bucket, String streamId, Int32? start = null, Int32? count = null) where T : class, IEventSource;

        Task WriteEvents<T>(String bucket, String streamId, Int32 expectedVersion, IEnumerable<IWritableEvent> events, IDictionary<String, String> commitHeaders) where T : class, IEventSource;

        Task AppendEvents<T>(String bucket, String streamId, IEnumerable<IWritableEvent> events, IDictionary<String, String> commitHeaders) where T : class, IEventSource;

        Task WriteEventMetadata<T>(String bucket, String streamId, Int32? MaxCount = null, TimeSpan? MaxAge = null, TimeSpan? CacheControl = null) where T : class, IEventSource;

        IBuilder Builder { get; set; }
    }
}