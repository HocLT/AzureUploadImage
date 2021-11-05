using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUploadImage.Service
{
    public class BlobService
    {
        public CloudBlobContainer GetCloudBlobContainer() {
            // tạo đối tượng CloudStorageAccount với thông tin từ tập tin cấu hình
            CloudStorageAccount account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("UploadCon"));
            // tạo đối tượng client
            CloudBlobClient client = account.CreateCloudBlobClient();
            // get tham chiếu đến images container từ client
            CloudBlobContainer container = client.GetContainerReference("images");
            // nếu chưa có container : images => tạo và cấp quyền
            if (container.CreateIfNotExists())
            {
                // cấp quyền
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return container;
        }
    }
}