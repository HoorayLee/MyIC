using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// SelfCheckPhotoDownloadHandler 的摘要说明
    /// </summary>
    public class SelfCheckPhotoDownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream);
            string fileName = reader.ReadToEnd();

            context.Response.WriteFile(Path.Combine("C:/Received/", fileName));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}