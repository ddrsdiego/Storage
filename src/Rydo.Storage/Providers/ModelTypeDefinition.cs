namespace Rydo.Storage.Providers
{
    using System;
    using Read;
    using Write;
    using Write.v2;

    public interface IStorageWriteContext
    {
        IStorageWrite Writer { get; }
        IStorageWriterConsumer Consumer { get; }
    }

    internal sealed class StorageWriteContext : IStorageWriteContext
    {
        public StorageWriteContext(IStorageWrite storageWrite, IStorageWriterConsumer writerConsumer)
        {
            // Writer = new Write.v2.InternalStorageWrite();
            Consumer = writerConsumer;
        }

        public IStorageWrite Writer { get; }
        public IStorageWriterConsumer Consumer { get; }
    }

    public interface IStorageReadContext
    {
        IStorageRead Reader { get; }
        IStorageReaderConsumer Consumer { get; }
    }

    internal sealed class StorageReadContext : IStorageReadContext
    {
        public StorageReadContext(IStorageRead storageRead, IStorageReaderConsumer readerConsumer)
        {
            Reader = storageRead;
            Consumer = readerConsumer;
        }

        public IStorageRead Reader { get; }
        public IStorageReaderConsumer Consumer { get; }
    }

    public readonly struct ModelTypeContext
    {
        public ModelTypeContext(IModelTypeDefinition definition, IStorageService storageService)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public ModelTypeContext(IModelTypeDefinition definition,
            IStorageService storageService,
            IStorageWriteContext writeContext,
            IStorageReadContext readContext)
        {
            WriteContext = writeContext;
            ReadContext = readContext;
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public readonly IStorageReadContext ReadContext;
        public readonly IStorageWriteContext WriteContext;
        public readonly IStorageService StorageService;
        public readonly IModelTypeDefinition Definition;
    }

    public sealed class ModelTypeDefinition : IModelTypeDefinition
    {
        internal ModelTypeDefinition(Type modelType)
        {
            ModelType = modelType;
            ModeTypeName = SanitizeModeTypeName(modelType);
        }

        private static string SanitizeModeTypeName(Type modelType) =>
            ModelTypeDefinitionHelper.SanitizeModeTypeName(modelType);

        public Type ModelType { get; }
        public string TableName { get; internal set; }
        public string ModeTypeName { get; }
        public TimeSpan TimeToLive { get; internal set; }
        public string WriteEndpoint { get; internal set; }
        public string ReadEndpoint { get; internal set; }
        public bool UseMemoryCache { get; internal set; }
        public string DbInstance { get; internal set; }
        public int WriteBufferSize { get; internal set; }
        public int ReadBufferSize { get; internal set; }
        public string AccessKey { get; internal set; }
        public string SecretKey { get; internal set; }
    }
}