﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.ResourceManager.Core;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Resources
{
    /// <summary> The predefined tag client. </summary>
    public class PredefinedTagOperations : ResourceOperationsBase
    {
        /// <summary>
        /// The resource type for predefined tag.
        /// </summary>
        public static readonly ResourceType ResourceType = "Microsoft.Resources/tagNames";

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedTagOperations"/> class for mocking.
        /// </summary>
        protected PredefinedTagOperations()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedTagOperations"/> class.
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="id"> The id of the subscription. </param>
        internal PredefinedTagOperations(ClientContext clientContext, ResourceIdentifier id)
            : base(clientContext, id)
        {
        }

        /// <summary>
        /// Gets the valid resource type for this operation class.
        /// </summary>
        protected override ResourceType ValidResourceType => ResourceType;

        private TagRestOperations RestClient => new TagRestOperations(Diagnostics, Pipeline, Id.SubscriptionId, BaseUri);

        /// <summary> This operation allows deleting a value from the list of predefined values for an existing predefined tag name. The value being deleted must not be in use as a tag value for the given tag name for any resource. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to delete. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteValueAsync(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.DeleteValue");
            scope.Start();
            try
            {
                return await RestClient.DeleteValueAsync(tagName, tagValue, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows deleting a value from the list of predefined values for an existing predefined tag name. The value being deleted must not be in use as a tag value for the given tag name for any resource. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to delete. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteValue(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.DeleteValue");
            scope.Start();
            try
            {
                return RestClient.DeleteValue(tagName, tagValue, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows adding a value to the list of predefined values for an existing predefined tag name. A tag value can have a maximum of 256 characters. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<PredefinedTagValue>> CreateOrUpdateValueAsync(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.CreateOrUpdateValue");
            scope.Start();
            try
            {
                return await RestClient.CreateOrUpdateValueAsync(tagName, tagValue, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows adding a value to the list of predefined values for an existing predefined tag name. A tag value can have a maximum of 256 characters. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<PredefinedTagValue> CreateOrUpdateValue(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.CreateOrUpdateValue");
            scope.Start();
            try
            {
                return RestClient.CreateOrUpdateValue(tagName, tagValue, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteAsync(string tagName, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.Delete");
            scope.Start();
            try
            {
                var operation = await StartDeleteAsync(tagName, cancellationToken).ConfigureAwait(false);
                return await operation.WaitForCompletionResponseAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Delete(string tagName, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.Delete");
            scope.Start();
            try
            {
                var operation = StartDelete(tagName, cancellationToken);
                return operation.WaitForCompletion(cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<PredefinedTagDeleteOperation> StartDeleteAsync(string tagName, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.StartDelete");
            scope.Start();
            try
            {
                var response = await RestClient.DeleteAsync(tagName, cancellationToken).ConfigureAwait(false);
                return new PredefinedTagDeleteOperation(Diagnostics, Pipeline, RestClient.CreateDeleteRequest(Id.Name).Request, response);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual PredefinedTagDeleteOperation StartDelete(string tagName, CancellationToken cancellationToken = default)
        {
            using var scope = Diagnostics.CreateScope("PredefinedTagOperations.StartDelete");
            scope.Start();
            try
            {
                var response = RestClient.Delete(tagName, cancellationToken);
                return new PredefinedTagDeleteOperation(Diagnostics, Pipeline, RestClient.CreateDeleteRequest(Id.Name).Request, response);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <inheritdoc/>
        protected override void ValidateResourceType(ResourceIdentifier identifier)
        {
            if (identifier is null)
                throw new ArgumentException("Invalid resource type for TagsOperation", nameof(identifier));
        }
    }
}
