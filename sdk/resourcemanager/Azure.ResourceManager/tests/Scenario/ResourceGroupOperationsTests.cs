﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.Tests
{
    public class ResourceGroupOperationsTests : ResourceManagerTestBase
    {
        public ResourceGroupOperationsTests(bool isAsync)
            : base(isAsync)//, RecordedTestMode.Record)
        {
        }

        [TestCase]
        [RecordedTest]
        public async Task DeleteRg()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            await rg.DeleteAsync();
        }

        [TestCase]
        [RecordedTest]
        public async Task StartDeleteRg()
        {
            var rgOp = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg = await rgOp.WaitForCompletionAsync();
            var deleteOp = await rg.StartDeleteAsync();
            var response = deleteOp.GetRawResponse();
            Assert.AreEqual(202, response.Status);
            await deleteOp.UpdateStatusAsync();
            await deleteOp.WaitForCompletionResponseAsync();
            await deleteOp.WaitForCompletionResponseAsync(TimeSpan.FromSeconds(2));

            var rgOp2 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg2 = await rgOp.WaitForCompletionAsync();
            rg2.Id.Name = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg2.StartDeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public void StartDeleteNonExistantRg()
        {
            var rgOp = InstrumentClientExtension(Client.GetResourceGroupOperations($"/subscriptions/{TestEnvironment.SubscriptionId}/resourceGroups/fake"));
            var deleteOpTask = rgOp.StartDeleteAsync();
            RequestFailedException exception = Assert.ThrowsAsync<RequestFailedException>(async () => await deleteOpTask);
            Assert.AreEqual(404, exception.Status);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg2 = await rg1.GetAsync();
            Assert.AreEqual(rg1.Data.Name, rg2.Data.Name);
            Assert.AreEqual(rg1.Data.Id, rg2.Data.Id);
            Assert.AreEqual(rg1.Data.Type, rg2.Data.Type);
            Assert.AreEqual(rg1.Data.Properties.ProvisioningState, rg2.Data.Properties.ProvisioningState);
            Assert.AreEqual(rg1.Data.Location, rg2.Data.Location);
            Assert.AreEqual(rg1.Data.ManagedBy, rg2.Data.ManagedBy);
            Assert.AreEqual(rg1.Data.Tags, rg2.Data.Tags);

            rg1.Id.Name = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.GetAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Update()
        {
            var rgName = Recording.GenerateAssetName("testrg");
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(rgName);
            var parameters = new ResourceGroupPatchable
            {
                Name = rgName
            };
            ResourceGroup rg2 = await rg1.UpdateAsync(parameters);
            Assert.AreEqual(rg1.Data.Name, rg2.Data.Name);
            Assert.AreEqual(rg1.Data.Id, rg2.Data.Id);
            Assert.AreEqual(rg1.Data.Type, rg2.Data.Type);
            Assert.AreEqual(rg1.Data.Properties.ProvisioningState, rg2.Data.Properties.ProvisioningState);
            Assert.AreEqual(rg1.Data.Location, rg2.Data.Location);
            Assert.AreEqual(rg1.Data.ManagedBy, rg2.Data.ManagedBy);
            Assert.AreEqual(rg1.Data.Tags, rg2.Data.Tags);

            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.UpdateAsync(null));

            rg1.Id.Name = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.UpdateAsync(parameters));
        }

        [TestCase]
        [RecordedTest]
        public async Task StartExportTemplate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var parameters = new ExportTemplateRequest();
            parameters.Resources.Add("*");
            var expOp = await rg.StartExportTemplateAsync(parameters);
            await expOp.WaitForCompletionAsync();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var expOp = await rg.StartExportTemplateAsync(null);
                _ = await expOp.WaitForCompletionAsync();
            });

            rg.Id.Name = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg.StartExportTemplateAsync(parameters));
        }

        [TestCase]
        [RecordedTest]
        public async Task AddTag()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            Assert.AreEqual(0, rg1.Data.Tags.Count);
            ResourceGroup rg2 = await rg1.AddTagAsync("key", "value");
            Assert.AreEqual(1, rg2.Data.Tags.Count);
            Assert.IsTrue(rg2.Data.Tags.Contains(new KeyValuePair<string, string>("key", "value")));
            Assert.AreEqual(rg1.Data.Name, rg2.Data.Name);
            Assert.AreEqual(rg1.Data.Id, rg2.Data.Id);
            Assert.AreEqual(rg1.Data.Type, rg2.Data.Type);
            Assert.AreEqual(rg1.Data.Properties.ProvisioningState, rg2.Data.Properties.ProvisioningState);
            Assert.AreEqual(rg1.Data.Location, rg2.Data.Location);
            Assert.AreEqual(rg1.Data.ManagedBy, rg2.Data.ManagedBy);

            Assert.ThrowsAsync<ArgumentException>(async () => _ = await rg1.AddTagAsync(null, "value"));
            Assert.ThrowsAsync<ArgumentException>(async () => _ = await rg1.AddTagAsync(" ", "value"));
        }

        [TestCase]
        [RecordedTest]
        public async Task SetTags()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            Assert.AreEqual(0, rg1.Data.Tags.Count);
            var tags = new Dictionary<string, string>()
            {
                { "key", "value"}
            };
            ResourceGroup rg2 = await rg1.SetTagsAsync(tags);
            Assert.AreEqual(tags, rg2.Data.Tags);
            Assert.AreEqual(rg1.Data.Name, rg2.Data.Name);
            Assert.AreEqual(rg1.Data.Id, rg2.Data.Id);
            Assert.AreEqual(rg1.Data.Type, rg2.Data.Type);
            Assert.AreEqual(rg1.Data.Properties.ProvisioningState, rg2.Data.Properties.ProvisioningState);
            Assert.AreEqual(rg1.Data.Location, rg2.Data.Location);
            Assert.AreEqual(rg1.Data.ManagedBy, rg2.Data.ManagedBy);

            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.SetTagsAsync(null));
        }

        [TestCase]
        [RecordedTest]
        public async Task RemoveTag()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var tags = new Dictionary<string, string>()
            {
                { "k1", "v1"},
                { "k2", "v2"}
            };
            rg1 = await rg1.SetTagsAsync(tags);
            ResourceGroup rg2 = await rg1.RemoveTagAsync("k1");
            var tags2 = new Dictionary<string, string>()
            {
                { "k2", "v2"}
            };
            Assert.AreEqual(tags2, rg2.Data.Tags);
            Assert.AreEqual(rg1.Data.Name, rg2.Data.Name);
            Assert.AreEqual(rg1.Data.Id, rg2.Data.Id);
            Assert.AreEqual(rg1.Data.Type, rg2.Data.Type);
            Assert.AreEqual(rg1.Data.Properties.ProvisioningState, rg2.Data.Properties.ProvisioningState);
            Assert.AreEqual(rg1.Data.Location, rg2.Data.Location);
            Assert.AreEqual(rg1.Data.ManagedBy, rg2.Data.ManagedBy);

            Assert.ThrowsAsync<ArgumentException>(async () => _ = await rg1.RemoveTagAsync(null));
            Assert.ThrowsAsync<ArgumentException>(async () => _ = await rg1.RemoveTagAsync(" "));
        }

        [TestCase]
        [RecordedTest]
        public async Task ListAvailableLocations()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var locations = await rg.GetAvailableLocationsAsync();
            int count = 0;
            foreach (var location in locations)
            {
                count++;
            }
            Assert.GreaterOrEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task MoveResources()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg2 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var genericResources = Client.DefaultSubscription.GetGenericResources();
            var aset = await CreateGenericAvailabilitySetAsync(rg1.Id);

            int countRg1 = await GetResourceCountAsync(genericResources, rg1);
            int countRg2 = await GetResourceCountAsync(genericResources, rg2);
            Assert.AreEqual(1, countRg1);
            Assert.AreEqual(0, countRg2);

            var moveInfo = new ResourcesMoveInfo();
            moveInfo.TargetResourceGroup = rg2.Id;
            moveInfo.Resources.Add(aset.Id);
            _ = await rg1.MoveResourcesAsync(moveInfo);

            countRg1 = await GetResourceCountAsync(genericResources, rg1);
            countRg2 = await GetResourceCountAsync(genericResources, rg2);
            Assert.AreEqual(0, countRg1);
            Assert.AreEqual(1, countRg2);

            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.MoveResourcesAsync(null));
        }

        [TestCase]
        [RecordedTest]
        public async Task StartMoveResources()
        {
            var rg1Op = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var rg2Op = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg1 = await rg1Op.WaitForCompletionAsync();
            ResourceGroup rg2 = await rg2Op.WaitForCompletionAsync();
            var genericResources = Client.DefaultSubscription.GetGenericResources();
            var asetOp = await StartCreateGenericAvailabilitySetAsync(rg1.Id);
            GenericResource aset = await asetOp.WaitForCompletionAsync();

            int countRg1 = await GetResourceCountAsync(genericResources, rg1);
            int countRg2 = await GetResourceCountAsync(genericResources, rg2);
            Assert.AreEqual(1, countRg1);
            Assert.AreEqual(0, countRg2);

            var moveInfo = new ResourcesMoveInfo();
            moveInfo.TargetResourceGroup = rg2.Id;
            moveInfo.Resources.Add(aset.Id);
            var moveOp = await rg1.StartMoveResourcesAsync(moveInfo);
            _ = await moveOp.WaitForCompletionResponseAsync();

            countRg1 = await GetResourceCountAsync(genericResources, rg1);
            countRg2 = await GetResourceCountAsync(genericResources, rg2);
            Assert.AreEqual(0, countRg1);
            Assert.AreEqual(1, countRg2);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var moveOp = await rg1.StartMoveResourcesAsync(null);
                _ = await moveOp.WaitForCompletionResponseAsync();
            });
        }

        [TestCase]
        [RecordedTest]
        public async Task ValidateMoveResources()
        {
            ResourceGroup rg1 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg2 = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).CreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var aset = await CreateGenericAvailabilitySetAsync(rg1.Id);

            var moveInfo = new ResourcesMoveInfo();
            moveInfo.TargetResourceGroup = rg2.Id;
            moveInfo.Resources.Add(aset.Id);
            Response response = await rg1.ValidateMoveResourcesAsync(moveInfo);

            Assert.AreEqual(204, response.Status);

            Assert.ThrowsAsync<ArgumentNullException>(async () => _ = await rg1.ValidateMoveResourcesAsync(null));
        }

        [TestCase]
        [RecordedTest]
        public async Task StartValidateMoveResources()
        {
            var rg1Op = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            var rg2Op = await Client.DefaultSubscription.GetResourceGroups().Construct(Location.WestUS2).StartCreateOrUpdateAsync(Recording.GenerateAssetName("testrg"));
            ResourceGroup rg1 = await rg1Op.WaitForCompletionAsync();
            ResourceGroup rg2 = await rg2Op.WaitForCompletionAsync();
            var asetOp = await StartCreateGenericAvailabilitySetAsync(rg1.Id);
            GenericResource aset = await asetOp.WaitForCompletionAsync();

            var moveInfo = new ResourcesMoveInfo();
            moveInfo.TargetResourceGroup = rg2.Id;
            moveInfo.Resources.Add(aset.Id);
            var validateOp = await rg1.StartValidateMoveResourcesAsync(moveInfo);
            Response response = await validateOp.WaitForCompletionResponseAsync();

            Assert.AreEqual(204, response.Status);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var moveOp = await rg1.StartValidateMoveResourcesAsync(null);
                _ = await moveOp.WaitForCompletionResponseAsync();
            });
        }

        [TestCase]
        [Ignore("4622 needs complete with a Mocked example to fill in this test")]
        public void CreateResourceFromModel()
        {
            //public ArmResponse<TOperations> CreateResource<TContainer, TOperations, TResource>(string name, TResource model, azure_proto_core.Location location = default)
        }

        [TestCase]
        [Ignore("4622 needs complete with a Mocked example to fill in this test")]
        public void CreateResourceFromModelAsync()
        {
            //public ArmResponse<TOperations> CreateResource<TContainer, TOperations, TResource>(string name, TResource model, azure_proto_core.Location location = default)
        }
    }
}
