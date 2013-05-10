﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Two10.AzureIndexStore
{
    public class IndexClient
    {
        private readonly CloudTableClient tableClient;

        public IndexClient(CloudStorageAccount account)
        {
            this.tableClient = account.CreateCloudTableClient();
        }

        public IndexStore<T> GetIndexReference<T>(string name)
        {
            var table = tableClient.GetTableReference(string.Format("wazindex{0}", name));
            return new IndexStore<T>(table, name);
        }

        public IndexStore<Dictionary<string,string>> GetIndexReference(string name)
        {
            var table = tableClient.GetTableReference(string.Format("wazindex{0}", name));
            return new IndexStore<Dictionary<string, string>>(table, name);
        }

        public CachedIndexStore<T> GetCachedIndexReference<T>(string name, CacheItemPolicy policy)
        {
            var table = tableClient.GetTableReference(string.Format("wazindex{0}", name));
            return new CachedIndexStore<T>(table, name, policy);
        }

        public CachedIndexStore<Dictionary<string, string>> GetCachedIndexReference(string name, CacheItemPolicy policy)
        {
            var table = tableClient.GetTableReference(string.Format("wazindex{0}", name));
            return new CachedIndexStore<Dictionary<string, string>>(table, name, policy);
        }

        public IEnumerable<string> ListIndexs()
        {
            return this.tableClient.ListTables().Where(x => x.Name.StartsWith("wazindex")).Select(x =>
            {
                var name = x.Name.Replace("wazindex", "");
                return name;
            });
        }

    }
}
