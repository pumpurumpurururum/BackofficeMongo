using System;
using System.Collections.Specialized;
using System.IO;

namespace BaseCms.Helpers
{
    public class UploadUsage
    {
        private String address
        {
            get
            {
                return "http://media.360.ru/getimage.php";
            }
        }

        public String UploadFileWithFormFields(String oldpath, String newpath)
        {
            string filePath = oldpath;

            // this represents fields from a form
            var postData = new NameValueCollection { { "filefullpath", newpath } };

            string responseText;
            using (var response = Upload.PostFile(new Uri(address), postData, filePath, "filecont", null, null, null))
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            return responseText;
        }


        public String UploadFileWithFormFields(Stream file, String newpath)
        {


            // this represents fields from a form
            var postData = new NameValueCollection { { "filefullpath", newpath } };

            string responseText;
            using (var response = Upload.PostFile(new Uri(address), postData, file, "filename", "filecont", null, null, null))
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            return responseText;
        }

    }
}