using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;
using System.Text;
using System.IO.IsolatedStorage;
using System;
namespace InlineScheduler.Advanced.State
{
    public class TempFolderStateProvider : IStateProvider
    {
        private readonly string _storagePath;
        private readonly DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(WorkState));

        // Some important assumptions
        // 1. It does not use Json.NET or something else to store state, just to reduce dependencies
        // 2. It uses Temp folder, just because it's much more easy to find where you disk space are gone
        // 3. It uses single folder, because in most cases work keys are unique even across instances of the scheduler
        // 4. Need to encode key to ensure file name is valid
        // 5. Need something to clean very old stuff

        public TempFolderStateProvider(string storagePath = null)
        {
            _storagePath = storagePath ?? Path.Combine(Path.GetTempPath(), "InlineScheduler");
        }

        public string StoragePath { get { return _storagePath; } }

        public void Store(string key, WorkState state)
        {
            // This code assumes that in most cases, directory exists
            // so no need to check this every time we want to store something
            // but in case folder not here, create one and try second time

            try
            {
                InternalStore(key, state);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(_storagePath);
                InternalStore(key, state); // Try second time
            }
        }

        private void InternalStore(string key, WorkState state)
        {
            using (var stream = File.Create(GetWorkItemPath(key)))
            {
                _serializer.WriteObject(stream, state);
            }
        }

        public WorkState Retrieve(string key)
        {
            // If anything fail, should not die            
            var itemStorePath = GetWorkItemPath(key);
            if (File.Exists(itemStorePath))
            {
                try
                {
                    using (var stream = File.OpenRead(itemStorePath))
                    {
                        return (WorkState)_serializer.ReadObject(stream);
                    }
                }
                catch (Exception) 
                {
                    // This is ok in most cases,
                    // so just ignore exception and return null                    
                }
            }

            return null;
        }

        public string GetWorkItemPath(string key) 
        {
            return Path.Combine(_storagePath, key);
        }

        public static TempFolderStateProvider CreateInTempFolder(string subPath) 
        {
            return new TempFolderStateProvider(Path.Combine(Path.GetTempPath(), subPath));
        }
    }
}
