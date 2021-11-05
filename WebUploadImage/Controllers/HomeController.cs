using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUploadImage.Service;

namespace WebUploadImage.Controllers
{
    public class HomeController : Controller
    {
        BlobService service = new BlobService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Upload()
        {
            CloudBlobContainer container = service.GetCloudBlobContainer();
            List<string> files = new List<string>();
            foreach (var item in container.ListBlobs())
            {
                files.Add(item.Uri.ToString());     // item.Uri.ToString() -> lấy về đường dẫn file, dạng URL
            }
            return View(files);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase image)
        {
            CloudBlobContainer container = service.GetCloudBlobContainer();
            // tạo CloudBlockBlob -> tương tự như file trên Windows -> để lưu trữ file được upload lên
            CloudBlockBlob block = container.GetBlockBlobReference(image.FileName);
            // upload file image từ client gửi lên vào block blob
            block.UploadFromStream(image.InputStream);
            return RedirectToAction("Upload");
        }

        [HttpPost]
        public string DeleteImg(string name)
        {
            Uri imgUri = new Uri(name);
            string filename = Path.GetFileName(imgUri.LocalPath);
            CloudBlobContainer container = service.GetCloudBlobContainer();
            CloudBlockBlob block = container.GetBlockBlobReference(filename);
            block.Delete(); // xóa blockblob => file bị xóa
            return "File Deleted.";
        }
    }
}