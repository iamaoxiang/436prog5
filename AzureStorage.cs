using Microsoft.WindowsAzure.Storage;

namespace Query
{
	internal class AzureStorage
	{
		protected const string connectionString = "DefaultEndpointsProtocol=https;AccountName=graceazurestorage;AccountKey=0RxvXwmgtDRRyFRJ0oyaAziPjtkQXsrOCOMUzxCTv1/d3JVOSpaaeX/lQIMRniyfM7fwAs9BH//Vx/ndNmbOaA==;EndpointSuffix=core.windows.net";

		private static CloudStorageAccount _account = null;
		protected CloudStorageAccount Account
		{
			get
			{
				if (_account == null)
				{
					_account = CloudStorageAccount.Parse(connectionString);
				}
				return _account;
			}
		}
	}
}