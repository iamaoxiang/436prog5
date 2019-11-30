using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Query
{
	internal class AzureTable : AzureStorage
	{
		private const string tableName = "hw436p4table";

		private static CloudTableClient _client = null;
		internal CloudTableClient Client
		{
			get
			{
				if (_client == null)
				{
					_client = Account.CreateCloudTableClient();
				}
				return _client;
			}
		}


		private static CloudTable _table = null;
		internal CloudTable Table
		{
			get
			{
				if (_table == null)
				{
					_table = Client.GetTableReference(tableName);
					_table.DeleteIfExists();
					RetryCreateIfNotExist(_table);
                }
                return _table;
			}
		}

        public bool Initialized
        {
            get { return _table != null; }
        }


        public void Insert(PersonEntity item)
		{	
			// Create the TableOperation that inserts the entity.
			var insertOperation = TableOperation.InsertOrReplace(item);

			// Execute the insert operation.
			var result = Table.Execute(insertOperation);

			// check inserted PersonEntity
			PersonEntity inserted = result.Result as PersonEntity;
		}

		public void Delete()
		{
			if (_table != null)
				_table.DeleteIfExists();
            _table = null;
        }

		// if you just deleted, CreateIfNotExists throws.  So retry until previous delete finished
		private bool RetryCreateIfNotExist(CloudTable table)
		{
			do
			{
				try
				{
					return table.CreateIfNotExists();
				}
				catch (StorageException e)
				{
					if (e.RequestInformation.HttpStatusCode == 409)
						Thread.Sleep(1000);
					else
						throw;
				}
			} while (true);
		}


		public List<PersonEntity> Query(string firstName, string lastName)
		{
			TableQuery<PersonEntity> query = null;
			if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
			{
				query = new TableQuery<PersonEntity>();
			}
			else if (string.IsNullOrWhiteSpace(lastName))
			{
				string filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, firstName.ToLowerInvariant());
				query = new TableQuery<PersonEntity>().Where(filter);
			}
			else if (string.IsNullOrWhiteSpace(firstName))
			{
				string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName.ToLowerInvariant());
				query = new TableQuery<PersonEntity>().Where(filter);
			}
			else
			{
				string filter = TableQuery.CombineFilters(
						TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, firstName.ToLowerInvariant()),
						TableOperators.And,
						TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName.ToLowerInvariant()));
				query = new TableQuery<PersonEntity>().Where(filter);
			}
			var result = Table.ExecuteQuery(query);

			return result.ToList();
		}

	}
}