using Com.AMap.Api.Services.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.Web.Http;

namespace dentists.helper
{
    class Reqdoc
    {
        private static HttpClient httpclient;
        private static CancellationTokenSource cts;
        public async static Task<string> postreq(string uri, String req)
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "POST";
            using (Stream stream = await request.GetRequestStreamAsync()) 
            {
                byte[] buf = Encoding.UTF8.GetBytes(req); 
                stream.Write(buf, 0, buf.Length);
                stream.Flush();
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                Stream resStream = response.GetResponseStream();
                byte[] bytes = new byte[2048];
                int readCount =await resStream.ReadAsync(bytes, 0, bytes.Length);
                return Encoding.UTF8.GetString(bytes, 0, readCount);
            }
            catch (Exception ex)
            {
                return "error";
            }
            //httpclient = new HttpClient();
            // //使用CancellationTokenSource对象来控制异步任务的取消操作
            //HttpStringContent httpStringContent = new HttpStringContent(req);
            //try
            //{
            //    HttpResponseMessage responses = await httpclient.PostAsync(new Uri(uri), httpStringContent);
            //    responseBody = await responses.Content.ReadAsStringAsync();
            //}
            //catch (Exception ex)
            //{
                
            //}
        }
    }
}
