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
        /// <returns>上传成功则返回GUID字符串（用于唯一标识一个图片）</returns>
        public static string UploadFile(string url, string fileName)
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
                    byte[] bytes = client.UploadFile(url, "POST", fileName);
                    string guidStr = Encoding.UTF8.GetString(bytes);
                    return guidStr;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 下载文件：用于下载个人信息的用户照片和执照
        /// </summary>
        /// <param name="url">指定url地址：一个ashx文件</param>
        /// <param name="fileName">DoctorModel类中的文件路径</param>
        public static void DownloadFile(string url, string fileName)
        {
            //判断本地是否有图片缓存，如是则不用下载图片

            WebClient client = new WebClient();
        }
    }
}
