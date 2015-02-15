using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// PicUploadHandler 的摘要说明
    /// </summary>
    public class PicUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count != 1)
            {
                throw new Exception("一次只能上传一个文件");
            }

            HttpPostedFile file = context.Request.Files[0];
            string fileName = file.FileName;
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            file.SaveAs(Path.Combine(context.Server.MapPath("~/UploadFiles/"), newFileName));

            byte[] bytes = Encoding.UTF8.GetBytes(newFileName);
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
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