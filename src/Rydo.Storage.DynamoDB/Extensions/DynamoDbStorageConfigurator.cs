namespace Rydo.Storage.DynamoDB.Extensions
{
    using System;
    using Amazon;
    using Amazon.DynamoDBv2;
    using Amazon.Runtime;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using Rydo.Storage.Extensions;
    using Write;

    public static class DynamoDbStorageConfigurator
    {
        public static void UseDynamoDb(this StorageContainerConfiguratorBuilder configurator,
            Action<DynamoDbStorageConfiguratorBuilder> configure)
        {
            var builder = new DynamoDbStorageConfiguratorBuilder();
            configure(builder);

            var accessKey = string.Empty;
            var secretKey = string.Empty;

            configurator.Services.AddSingleton<IModelTypeContextContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ModelTypeContextContainer>>();

                var modelTypeContainer = new ModelTypeContextContainer(logger);

                foreach (var (modelTypeName, modelTypeDefinition) in builder.Entries)
                {
                    accessKey = modelTypeDefinition.AccessKey;
                    secretKey = modelTypeDefinition.SecretKey;

                    var dynamoDb = sp.GetRequiredService<IAmazonDynamoDB>();
                    var modelTypeContext =
                        new ModelTypeContext(modelTypeDefinition, new DynamoDbStorageService(dynamoDb));

                    modelTypeContainer.AddModelType(modelTypeContext);
                }

                return modelTypeContainer;
            });

            configurator.Services.AddSingleton<IStorageContentProvider, DynamoDbStorageContentProvider>();
            
            configurator.Services.AddSingleton<IAmazonDynamoDB>(_ =>
            {
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var config = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.SAEast1};
                return new AmazonDynamoDBClient(credentials, config);
            });

            configurator.Services.AddSingleton<IStorageConfiguratorBuilder>(builder);
            configurator.Services
                .AddSingleton<IStorageConfiguratorBuilder<DynamoDbModelTypeDefinitionBuilder>>(builder);
            configurator.Services
                .AddSingleton<IDynamoDbReadStorageContentProvider, DynamoDbReadStorageContentProvider>();
            configurator.Services
                .AddSingleton<IDynamoDbWriteStorageContentProvider, DynamoDbWriteStorageContentProvider>();
        }
    }
}