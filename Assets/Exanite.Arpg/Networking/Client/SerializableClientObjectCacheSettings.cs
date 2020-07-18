using System;
using System.Net.Sockets;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using UnityEngine;

namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// Unity serializable version of <see cref="ClientObjectCacheSettings"/>
    /// </summary>
    [Serializable]
    public sealed class SerializableClientObjectCacheSettings
    {
        [SerializeField] private int maxWriters = 2;
        [SerializeField] private int maxReaders = 2;
        [SerializeField] private int maxMessages = 4;
        [SerializeField] private int maxMessageBuffers = 4;
        [SerializeField] private int maxSocketAsyncEventArgs = 32;
        [SerializeField] private int maxActionDispatcherTasks = 16;
        [SerializeField] private int maxAutoRecyclingArrays = 4;

        [SerializeField] private int extraSmallMemoryBlockSize = 16;
        [SerializeField] private int maxExtraSmallMemoryBlocks = 2;
        [SerializeField] private int smallMemoryBlockSize = 64;
        [SerializeField] private int maxSmallMemoryBlocks = 2;
        [SerializeField] private int mediumMemoryBlockSize = 256;
        [SerializeField] private int maxMediumMemoryBlocks = 2;
        [SerializeField] private int largeMemoryBlockSize = 1024;
        [SerializeField] private int maxLargeMemoryBlocks = 2;
        [SerializeField] private int extraLargeMemoryBlockSize = 4096;
        [SerializeField] private int maxExtraLargeMemoryBlocks = 2;

        [SerializeField] private int maxMessageReceivedEventArgs = 4;

        /// <summary>
        /// The maximum number of <see cref="DarkRiftWriter"/>s to cache per thread
        /// </summary>
        public int MaxWriters
        {
            get
            {
                return maxWriters;
            }

            set
            {
                maxWriters = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="DarkRiftReader"/>s to cache per thread
        /// </summary>
        public int MaxReaders
        {
            get
            {
                return maxReaders;
            }

            set
            {
                maxReaders = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="Message"/>s to cache per thread
        /// </summary>
        public int MaxMessages
        {
            get
            {
                return maxMessages;
            }

            set
            {
                maxMessages = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="MessageBuffer"/>s to cache per thread
        /// </summary>
        public int MaxMessageBuffers
        {
            get
            {
                return maxMessageBuffers;
            }

            set
            {
                maxMessageBuffers = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="SocketAsyncEventArgs"/> to cache per thread
        /// </summary>
        public int MaxSocketAsyncEventArgs
        {
            get
            {
                return maxSocketAsyncEventArgs;
            }

            set
            {
                maxSocketAsyncEventArgs = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="ActionDispatcherTask"/>s to cache per thread
        /// </summary>
        public int MaxActionDispatcherTasks
        {
            get
            {
                return maxActionDispatcherTasks;
            }

            set
            {
                maxActionDispatcherTasks = value;
            }
        }

        /// <summary>
        /// The maximum number of <see cref="AutoRecyclingArray"/> instances stored per thread
        /// </summary>
        public int MaxAutoRecyclingArrays
        {
            get
            {
                return maxAutoRecyclingArrays;
            }

            set
            {
                maxAutoRecyclingArrays = value;
            }
        }

        /// <summary>
        /// The number of bytes in the extra small memory bocks cached
        /// </summary>
        public int ExtraSmallMemoryBlockSize
        {
            get
            {
                return extraSmallMemoryBlockSize;
            }

            set
            {
                extraSmallMemoryBlockSize = value;
            }
        }

        /// <summary>
        /// The maximum number of extra small memory blocks stored per thread
        /// </summary>
        public int MaxExtraSmallMemoryBlocks
        {
            get
            {
                return maxExtraSmallMemoryBlocks;
            }

            set
            {
                maxExtraSmallMemoryBlocks = value;
            }
        }

        /// <summary>
        /// The number of bytes in the small memory bocks cached
        /// </summary>
        public int SmallMemoryBlockSize
        {
            get
            {
                return smallMemoryBlockSize;
            }

            set
            {
                smallMemoryBlockSize = value;
            }
        }

        /// <summary>
        /// The maximum number of small memory blocks stored per thread
        /// </summary>
        public int MaxSmallMemoryBlocks
        {
            get
            {
                return maxSmallMemoryBlocks;
            }

            set
            {
                maxSmallMemoryBlocks = value;
            }
        }

        /// <summary>
        /// The number of bytes in the medium memory bocks cached
        /// </summary>
        public int MediumMemoryBlockSize
        {
            get
            {
                return mediumMemoryBlockSize;
            }

            set
            {
                mediumMemoryBlockSize = value;
            }
        }

        /// <summary>
        /// The maximum number of medium memory blocks stored per thread
        /// </summary>
        public int MaxMediumMemoryBlocks
        {
            get
            {
                return maxMediumMemoryBlocks;
            }

            set
            {
                maxMediumMemoryBlocks = value;
            }
        }

        /// <summary>
        /// The number of bytes in the large memory bocks cached
        /// </summary>
        public int LargeMemoryBlockSize
        {
            get
            {
                return largeMemoryBlockSize;
            }

            set
            {
                largeMemoryBlockSize = value;
            }
        }

        /// <summary>
        /// The maximum number of large memory blocks stored per thread
        /// </summary>
        public int MaxLargeMemoryBlocks
        {
            get
            {
                return maxLargeMemoryBlocks;
            }

            set
            {
                maxLargeMemoryBlocks = value;
            }
        }

        /// <summary>
        /// The number of bytes in the extra large memory bocks cached
        /// </summary>
        public int ExtraLargeMemoryBlockSize
        {
            get
            {
                return extraLargeMemoryBlockSize;
            }

            set
            {
                extraLargeMemoryBlockSize = value;
            }
        }

        /// <summary>
        /// The maximum number of extra large memory blocks stored per thread
        /// </summary>
        public int MaxExtraLargeMemoryBlocks
        {
            get
            {
                return maxExtraLargeMemoryBlocks;
            }

            set
            {
                maxExtraLargeMemoryBlocks = value;
            }
        }

        /// <summary>
        /// The maximum number of MessageReceivedEventArgs to cache per thread
        /// </summary>
        public int MaxMessageReceivedEventArgs
        {
            get
            {
                return maxMessageReceivedEventArgs;
            }

            set
            {
                maxMessageReceivedEventArgs = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="ClientObjectCacheSettings"/> based on this <see cref="SerializableClientObjectCacheSettings"/>
        /// </summary>
        /// <returns></returns>
        public ClientObjectCacheSettings ToClientObjectCacheSettings()
        {
            return new ClientObjectCacheSettings
            {
                MaxWriters = MaxWriters,
                MaxReaders = MaxReaders,
                MaxMessages = MaxMessages,
                MaxMessageBuffers = MaxMessageBuffers,
                MaxSocketAsyncEventArgs = MaxSocketAsyncEventArgs,
                MaxActionDispatcherTasks = MaxActionDispatcherTasks,
                MaxAutoRecyclingArrays = MaxAutoRecyclingArrays,

                ExtraSmallMemoryBlockSize = ExtraSmallMemoryBlockSize,
                MaxExtraSmallMemoryBlocks = MaxExtraSmallMemoryBlocks,
                SmallMemoryBlockSize = SmallMemoryBlockSize,
                MaxSmallMemoryBlocks = MaxSmallMemoryBlocks,
                MediumMemoryBlockSize = MediumMemoryBlockSize,
                MaxMediumMemoryBlocks = MaxMediumMemoryBlocks,
                LargeMemoryBlockSize = LargeMemoryBlockSize,
                MaxLargeMemoryBlocks = MaxLargeMemoryBlocks,
                ExtraLargeMemoryBlockSize = ExtraLargeMemoryBlockSize,
                MaxExtraLargeMemoryBlocks = MaxExtraLargeMemoryBlocks,

                MaxMessageReceivedEventArgs = MaxMessageReceivedEventArgs
            };
        }
    }
}
