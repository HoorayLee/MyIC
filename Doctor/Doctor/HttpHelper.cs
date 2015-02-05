using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace Doctor
{
    class HttpHelper
    {
        private static readonly string ServerBaseUri = Properties.Settings.Default.ServerBaseUrl;

        /// <summary>
        /// 发送Http请求到指定url并返回结果
        /// </summary>
        /// <param name="fullUrl">指定url地址：一个ashx文件</param>
        /// <param name="content">发送给服务器的内容</param>
        /// <returns>null：连接失败 其他：连接成功，返回服务器相应</returns>
        public static string ConnectionForResult(string url, string content)
        {
            url = ServerBaseUri + url;
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(content);
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.ContentEncoding == "gzip")
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (GZipStream zipStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            byte[] buf = new byte[1024];
                            int readCount = zipStream.Read(buf, 0, buf.Length);
                            while (readCount > 0)
                            {
                                memoryStream.Write(buf, 0, readCount);
                                readCount = zipStream.Read(buf, 0, buf.Length);
                            }
                        }
                        result = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 上传文件：可用于上传图片
        /// </summary>
        /// <param name="url">指定url地址：一个ashx文件</param>
        /// <param name="fileName">要上传文件的本地路径</param>
        /// <returns>是否上传成功</returns>
        public static bool UploadFile(string url, string fileName)
        {
            url = ServerBaseUri + url;
            if (!File.Exists(fileName))
            {
                throw new Exception("File not Exist");
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    client.Headers.Add("FileName", newFileName);
                    client.UploadFile(url, "POST", fileName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
