using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MsVisionDog.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MsVisionDog.Web.Controllers
{
    [Authorize]
    public class MyImageController : Controller
    {
        MyDbContext _db;

        public MyImageController()
        {
            _db = new MyDbContext();
        }

        public ActionResult Index()
        {
            var myImages = _db.MyImages.Where(e => e.UserID == User.Identity.Name).OrderByDescending(e => e.DateCreated).ToList();
            ViewBag.MyImages = myImages;

            return View();
        }

        public ActionResult Upload()
        {
            ViewBag.MyEntities = _db.MyEntities.Where(e => e.UserID == User.Identity.Name).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase imageFile, string[] myEntities)
        {
            if (imageFile == null)
                return RedirectToAction("Index");

            string myImageID = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(imageFile.FileName);
            string blobName = myImageID + extension;

            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(User.Identity.Name.Replace("@", "-").Replace(".", "-").ToLower());
            container.CreateIfNotExists();

            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromStream(imageFile.InputStream);

            var computerVisionApiResponse = MyVisionHelper.AnalyzeImage(blockBlob.Uri.ToString(), ConfigurationManager.AppSettings["ComputerVisionApiSubscriptionKey"]);
            var recognizeTextResponse = MyVisionHelper.RecognizeText(blockBlob.Uri.ToString(), ConfigurationManager.AppSettings["ComputerVisionApiSubscriptionKey"]);

            var myImage = new MyImage();
            myImage.MyImageID = myImageID;
            myImage.UserID = User.Identity.Name;
            myImage.Url = blockBlob.Uri.ToString();
            myImage.DateCreated = DateTime.UtcNow;
            myImage.ComputerVisionApiResponse = JsonConvert.SerializeObject(computerVisionApiResponse);
            myImage.DetailsJson = JsonConvert.SerializeObject(recognizeTextResponse);

            _db.MyImages.Add(myImage);

            if (myEntities != null)
            {
                foreach (var myEntity in myEntities)
                {
                    var myEntityImage = new MyEntityImage();
                    myEntityImage.MyEntityID = myEntity;
                    myEntityImage.MyImageID = myImage.MyImageID;
                    myEntityImage.DateCreated = DateTime.UtcNow;
                    myEntityImage.UserID = User.Identity.Name;
                    _db.MyEntityImages.Add(myEntityImage);
                }
            }

            _db.SaveChanges();

            // Get OCR Text
            var text = MyVisionHelper.GetTextToSpeak(myImage.DetailsJson);
            if (text != "")
            {
                SaveTextAsAudio("ocr-" + myImage.MyImageID, "Image contains these words: " + text);
            }

            // Category Text
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var c in computerVisionApiResponse.Categories.Where(c => !c.Name.StartsWith("text_")).OrderByDescending(c => c.Score))
            {
                sb.Append(c.Name.Replace("_", " ") + " ");
                count++;
            }
            text = sb.ToString();
            if (text != "")
            {
                SaveTextAsAudio("cat-" + myImage.MyImageID, "Image depicts " + count + " categories: " + text);
            }

            return RedirectToAction("Index");
        }

        private void SaveTextAsAudio(string fileName, string text)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(User.Identity.Name.Replace("@", "-").Replace(".", "-").ToLower());

            var sh = new MySpeechHelper();
            sh.Speak(ConfigurationManager.AppSettings["SpeechApiSubscriptionKey"], text);

            string blobName = fileName + ".wav";
            var audioStream = sh.GetAudioStream();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromStream(audioStream);
        }
    }
}