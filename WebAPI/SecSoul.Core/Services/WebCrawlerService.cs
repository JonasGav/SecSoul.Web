using System;
using System.IO;
using System.Net;
using System.Text;
using SecSoul.Model.Entity;

namespace SecSoul.Core.Services
{
    public class WebCrawlerService
    {
        public string GetHtml(string PageUri)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PageUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return data;
            }

            return "";
        }
    }
}