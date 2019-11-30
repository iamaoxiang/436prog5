using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Query
{
    public partial class _Default : Page
    {
        static string location1 = "https://s3-us-west-2.amazonaws.com/css490/input.txt";
		static string location2 = "https://css490.blob.core.windows.net/lab4/input.txt";


		private static AzureBlob blob = new AzureBlob();
		private static AzureTable table = new AzureTable();

		protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        private Stream GetStream(string primary, string backup)
        {
            WebRequest req = null;
            try
            {
                req = System.Net.WebRequest.Create(location1);
                return req.GetResponse().GetResponseStream();
            }
            catch (Exception)
            {
                // catch all errors
            }

            // in this case, we didnt return, but caught an error.  Try backup location
            req = System.Net.WebRequest.Create(location2);
            return req.GetResponse().GetResponseStream();
        }


        // load method
        protected void Button1_Click(object sender, EventArgs e)
        {
            // do a clear first
            Button2_Click(sender, e);

            // load the file as a stream
            using (Stream stream = GetStream(location1, location2))
            {
                // copy the data into my own blob
                blob.UploadToContainer(stream);
                StreamReader sr = new StreamReader(stream);
                while (!sr.EndOfStream)
                {
                    //insert data into table
                    table.Insert(new PersonEntity(sr.ReadLine()));
                }
            }
        }

		// clear method
		protected void Button2_Click(object sender, EventArgs e)
        {
			// delete our blob
			blob.DeleteContainerFile();

			// delete our table
			table.Delete();

            TextBox1.Text = "";
            TextBox2.Text = "";
            ListBox1.Items.Clear();
        }
        
        // query method
        protected void Button3_Click(object sender, EventArgs e)
        {
            ListBox1.Items.Clear();
            if (!table.Initialized)
            {
                ListBox1.Items.Add(new ListItem("Data is not loaded; no results to query."));
                return;
            }

			// query our table
			List<PersonEntity> results = table.Query(TextBox1.Text, TextBox2.Text);

            // write results into page
            ListBox1.DataSource = results;
            ListBox1.DataBind();
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
 