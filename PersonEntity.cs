using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Query
{
	internal class PersonEntity : TableEntity
	{
		public string FirstName;
		public string LastName;
		public Dictionary<string, string> Attributes;

		public PersonEntity()
		{
		}

		public PersonEntity(string line)
		{
			Parse(line);
		}

		public void Parse(string line)
		{
			string[] data = line.Trim().Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
			FirstName = data[0];
			LastName = data[1];
			Attributes = new Dictionary<string, string>();
			for (int i = 2; i < data.Length; i++)
			{
				AddAttribute(data[i]);
			}

			// required properties
			PartitionKey = LastName.ToLowerInvariant();
			RowKey = FirstName.ToLowerInvariant();
			ETag = "*"; // always overwrite
		}

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            IDictionary<string, EntityProperty> dict = base.WriteEntity(operationContext);
            foreach (var kvp in Attributes)
            {
                dict.Add(kvp.Key, new EntityProperty(kvp.Value));
            }
            dict.Add("FirstName", new EntityProperty(this.FirstName));
            dict.Add("LastName", new EntityProperty(this.LastName));
            return dict;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            Attributes = new Dictionary<string, string>();
            foreach (var kvp in properties)
            {
                if (kvp.Key == "FirstName")
                {
                    this.FirstName = kvp.Value.StringValue;
                }
                else if (kvp.Key == "LastName")
                {
                    this.LastName = kvp.Value.StringValue;
                }
                else
                {
                    Attributes.Add(kvp.Key, kvp.Value.StringValue);
                }
            }

            base.ReadEntity(properties, operationContext);
        }

        private void AddAttribute(string rawAttribute)
		{
			string[] keyAndValue = rawAttribute.Split(new char[] { '=' });
			Attributes.Add(keyAndValue[0], keyAndValue[1]);
		}

        public override string ToString()
        {
            string value = FirstName + " " + LastName + " ";
            foreach (KeyValuePair<string, string> kvp in Attributes)
            {
                value += kvp.Key + "=" + kvp.Value + " ";
            }
            return value.Trim();
        }
    }
}