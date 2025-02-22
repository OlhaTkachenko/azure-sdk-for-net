// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Core
{
    /// <summary>
    /// A class representing the operations that can be performed over a specific resource.
    /// </summary>
    public abstract class ResourceOperationsBase : OperationsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class for mocking.
        /// </summary>
        protected ResourceOperationsBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class.
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="id"></param>
        internal ResourceOperationsBase(ClientContext clientContext, ResourceIdentifier id)
            : base(clientContext, id)
        {
        }
    }

    /// <summary>
    /// Base class for all operations over a resource.
    /// </summary>
    /// <typeparam name="TOperations"> The type implementing operations over the resource. </typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Types differ by type argument only")]
    public abstract class ResourceOperationsBase<TOperations> : ResourceOperationsBase
        where TOperations : ResourceOperationsBase<TOperations>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase{TOperations}"/> class for mocking.
        /// </summary>
        protected ResourceOperationsBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase{TOperations}"/> class.
        /// </summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class.
        /// <param name="parentOperations"> The resource representing the parent resource. </param>
        /// <param name="id"> The identifier of the resource that is the target of operations. </param>
        protected ResourceOperationsBase(OperationsBase parentOperations, ResourceIdentifier id)
            : base(new ClientContext(parentOperations.ClientOptions, parentOperations.Credential, parentOperations.BaseUri, parentOperations.Pipeline), id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class.
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="id"></param>
        internal ResourceOperationsBase(ClientContext clientContext, ResourceIdentifier id)
            : base(clientContext, id)
        {
        }

        /// <summary>
        /// Gets details for this resource from the service.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. The default value is <see cref="CancellationToken.None" />. </param>
        /// <returns> A response with the <see cref="Response{TOperations}"/> operation for this resource. </returns>
        public abstract Response<TOperations> Get(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details for this resource from the service.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. The default value is <see cref="CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that on completion returns a response with the <see cref="Response{TOperations}"/> operation for this resource. </returns>
        public abstract Task<Response<TOperations>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all available geo-locations.
        /// </summary>
        /// <param name="resourceType"> The <see cref="ResourceType"/> instance to use for the list. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. The default value is <see cref="CancellationToken.None" />. </param>
        /// <returns> A collection of location that may take multiple service requests to iterate over. </returns>
        protected IEnumerable<Location> ListAvailableLocations(ResourceType resourceType, CancellationToken cancellationToken = default)
        {
            ProviderInfo resourcePageableProvider = Tenant.GetProvider(resourceType.Namespace, null, cancellationToken);
            if (resourcePageableProvider is null)
                throw new InvalidOperationException($"{resourceType.Type} not found for {resourceType.Namespace}");
            var theResource = resourcePageableProvider.ResourceTypes.FirstOrDefault(r => resourceType.Type.Equals(r.ResourceType));
            if (theResource is null)
                throw new InvalidOperationException($"{resourceType.Type} not found for {resourceType.Type}");
            return theResource.Locations.Select(l => (Location)l);
        }

        /// <summary>
        /// Lists all available geo-locations.
        /// </summary>
        /// <param name="resourceType"> The <see cref="ResourceType"/> instance to use for the list. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. The default value is <see cref="CancellationToken.None" />. </param>
        /// <returns> A collection of location that may take multiple service requests to iterate over. </returns>
        protected async Task<IEnumerable<Location>> ListAvailableLocationsAsync(ResourceType resourceType, CancellationToken cancellationToken = default)
        {
            ProviderInfo resourcePageableProvider = await Tenant.GetProviderAsync(resourceType.Namespace, null, cancellationToken).ConfigureAwait(false);
            if (resourcePageableProvider is null)
                throw new InvalidOperationException($"{resourceType.Type} not found for {resourceType.Namespace}");
            var theResource = resourcePageableProvider.ResourceTypes.FirstOrDefault(r => resourceType.Type.Equals(r.ResourceType));
            if (theResource is null)
                throw new InvalidOperationException($"{resourceType.Type} not found for {resourceType.Type}");
            return theResource.Locations.Select(l => (Location)l);
        }
    }
}
