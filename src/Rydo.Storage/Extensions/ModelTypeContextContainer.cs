namespace Rydo.Storage.Extensions
{
    using System;
    using System.Collections.Immutable;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;
    using Providers;

    public interface IModelTypeContextContainer
    {
        ImmutableDictionary<string, ModelTypeContext> Entries { get; }
        
        void AddModelType(ModelTypeContext modelTypeContext);

        bool TryGetModel(string modelName, out ModelTypeContext modelTypeContext);
    }

    public sealed class ModelTypeContextContainer : IModelTypeContextContainer
    {
        private readonly object _syncLock;
        private readonly ILogger<ModelTypeContextContainer> _logger;

        public ModelTypeContextContainer(ILogger<ModelTypeContextContainer> logger)
        {
            _logger = logger;
            _syncLock = new object();
            Entries = ImmutableDictionary<string, ModelTypeContext>.Empty;
        }

        public ImmutableDictionary<string, ModelTypeContext> Entries { get; private set; }

        public void AddModelType(ModelTypeContext modelTypeContext)
        {
            if (Entries.TryGetValue(modelTypeContext.Definition.ModeTypeName, out _))
            {
                throw new InvalidOperationException("topicDefinition.TopicName");
            }
            
            Entries = Entries.Add(modelTypeContext.Definition.ModeTypeName, modelTypeContext);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetModel(string modelName, out ModelTypeContext modelTypeContext)
        {
            modelTypeContext = default;

            lock (_syncLock)
            {
                if (Entries.TryGetValue(modelName, out modelTypeContext))
                    return true;
            }

            return false;
        }
    }
}