// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.Storage.Files.DataLake.Models
{
    /// <summary> An enumeration of blobs. </summary>
    internal partial class ListBlobsHierarchySegmentResponse
    {
        /// <summary> Initializes a new instance of ListBlobsHierarchySegmentResponse. </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="containerName"></param>
        /// <param name="segment"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="serviceEndpoint"/>, <paramref name="containerName"/>, or <paramref name="segment"/> is null. </exception>
        internal ListBlobsHierarchySegmentResponse(string serviceEndpoint, string containerName, BlobHierarchyListSegment segment)
        {
            if (serviceEndpoint == null)
            {
                throw new ArgumentNullException(nameof(serviceEndpoint));
            }
            if (containerName == null)
            {
                throw new ArgumentNullException(nameof(containerName));
            }
            if (segment == null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            ServiceEndpoint = serviceEndpoint;
            ContainerName = containerName;
            Segment = segment;
        }

        /// <summary> Initializes a new instance of ListBlobsHierarchySegmentResponse. </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="containerName"></param>
        /// <param name="prefix"></param>
        /// <param name="marker"></param>
        /// <param name="maxResults"></param>
        /// <param name="delimiter"></param>
        /// <param name="segment"></param>
        /// <param name="nextMarker"></param>
        internal ListBlobsHierarchySegmentResponse(string serviceEndpoint, string containerName, string prefix, string marker, int? maxResults, string delimiter, BlobHierarchyListSegment segment, string nextMarker)
        {
            ServiceEndpoint = serviceEndpoint;
            ContainerName = containerName;
            Prefix = prefix;
            Marker = marker;
            MaxResults = maxResults;
            Delimiter = delimiter;
            Segment = segment;
            NextMarker = nextMarker;
        }

        public string ServiceEndpoint { get; }
        public string ContainerName { get; }
        public string Prefix { get; }
        public string Marker { get; }
        public int? MaxResults { get; }
        public string Delimiter { get; }
        public BlobHierarchyListSegment Segment { get; }
        public string NextMarker { get; }
    }
}
