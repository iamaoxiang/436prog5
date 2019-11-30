namespace Query
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using Microsoft.Azure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Blob;

	internal class AzureBlob : AzureStorage
	{
        private const string containerName = "hw436p4"; // always use the same container name for this class
		private const string fileName = "data.txt"; // always use the same file name for this class

        public AzureBlob()
		{
		}

		private static CloudBlobClient _client = null;
		internal CloudBlobClient Client
		{
			get
			{
				if (_client == null)
				{
					_client = Account.CreateCloudBlobClient();
				}
				return _client;
			}
		}
        public bool Initialized
        {
            get { return _container != null; }
        }
                
        private static CloudBlobContainer _container = null;
		internal CloudBlobContainer Container
		{
			get
			{
				if (_container == null)
				{
					_container = Client.GetContainerReference(containerName);
				}
				return _container;
			}
		}

		public void UploadToContainer(Stream stream)
		{
			AsyncUtil.RunSync(() => UploadToContainerAsync(stream));
		}

		public async Task UploadToContainerAsync(Stream stream)
		{
			if (await Container.CreateIfNotExistsAsync())
			{
				await Container.SetPermissionsAsync(new BlobContainerPermissions
				{
					PublicAccess = BlobContainerPublicAccessType.Blob
				});
			}

			CloudBlockBlob cloudBlockBlob = Container.GetBlockBlobReference(fileName);
			cloudBlockBlob.Properties.ContentType = "text/plain";
			await cloudBlockBlob.UploadFromStreamAsync(stream);
		}


		public void DeleteContainerFile()
		{
			if (_container == null)
				return;
			CloudBlockBlob cloudBlockBlob = Container.GetBlockBlobReference(fileName);
			cloudBlockBlob.Delete();
            _container = null;
        }

	}
}