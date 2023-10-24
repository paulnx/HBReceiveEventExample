using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace HBReceiveEventExample
{

   public class RemoteService
   {
      public RemoteService() { }

      public async Task<HttpResponseMessage> SendData( string strData)
      {
         StringContent content = new StringContent(strData, Encoding.UTF8);
         HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, DestUrl);
         request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         request.Content = content;
         HttpResponseMessage resp = await HttpClientSingle.SendAsync(request);
         return resp;
      }

      public string ConvertToCSV(Dictionary<string, string> payloadData)
      {
         string csvReturn = string.Empty;
         csvReturn = AppendItemsToCSVString(payloadData.Keys) + "\n" + AppendItemsToCSVString(payloadData.Values) + "\n";
         return csvReturn;
      }

      #region hide private methods
      private string AppendItemsToCSVString(ICollection items)
      {
         const string delim = ",";
         string csvLine = string.Empty;
         foreach (string item in items)
         {
            if (csvLine.Length > 0)
               csvLine += delim;
            csvLine = AppendCsvItem(csvLine, item);
         }
         return csvLine;
      }

      private string AppendCsvItem(string csvLineBuffer, string strItem)
      {
         string SpecialChars = "\"\x0A\x0D" + ",";
         if (strItem == null)
            strItem = string.Empty;
         if (strItem.IndexOfAny(SpecialChars.ToCharArray()) > -1)
            csvLineBuffer = csvLineBuffer + "\"" + strItem.Replace("\"", "\"\"") + "\"";
         else
            csvLineBuffer = csvLineBuffer + strItem;
         return csvLineBuffer;
      }
      #endregion

      static HttpClient m_HttpClientSingle;
      public HttpClient HttpClientSingle
      {
         get
         {
            if (m_HttpClientSingle == null)
            {
               m_HttpClientSingle = new HttpClient();
            }
            return m_HttpClientSingle;

         }

      }


      public string DestUrl {
         get {
            string localdestUrl = System.Environment.GetEnvironmentVariable("desturl");
            if (string.IsNullOrEmpty(localdestUrl))
               localdestUrl = "http://ptsv3.com/t/random411/post";
            return localdestUrl;


         }
      
      }







   }
}
