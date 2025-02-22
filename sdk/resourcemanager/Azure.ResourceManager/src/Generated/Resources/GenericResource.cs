﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;
using Azure.ResourceManager.Core;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Resources
{
    /// <summary>
    /// A class representing a generic azure resource along with the instance operations that can be performed on it.
    /// </summary>
    public class GenericResource : GenericResourceOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResource"/> class for mocking.
        /// </summary>
        protected GenericResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResource"/> class.
        /// </summary>
        /// <param name="operations"> The operations object to copy the client parameters from. </param>
        /// <param name="resource"> The data model representing the generic azure resource. </param>
        /// <exception cref="ArgumentNullException"> If <see cref="ArmClientOptions"/> or <see cref="TokenCredential"/> is null. </exception>
        internal GenericResource(OperationsBase operations, GenericResourceData resource)
            : base(operations, resource.Id)
        {
            Data = resource;
        }

        /// <summary>
        /// Gets the data representing this generic azure resource.
        /// </summary>
        public virtual GenericResourceData Data { get; }
    }
}
