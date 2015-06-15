using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MyVisionHelper
    {
        public static AnalysisResult AnalyzeImage(string url, string subscriptionKey)
        {
            HttpClient client = new HttpClient();
            StringContent postData = new StringContent("{'url':'" + url + "'}",Encoding.UTF8,"application/json");
            var postResponse = client.PostAsync("https://api.projectoxford.ai/vision/v1/analyses?visualFeatures=All&subscription-key=" + subscriptionKey, postData).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<AnalysisResult>(postResponse);
        }

        public static OcrResults RecognizeText(string url, string subscriptionKey)
        {
            HttpClient client = new HttpClient();
            StringContent postData = new StringContent("{'url':'" + url + "'}", Encoding.UTF8, "application/json");
            var postResponse = client.PostAsync("https://api.projectoxford.ai/vision/v1/ocr?language=en&detectOrientation=false&subscription-key=" + subscriptionKey, postData).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<OcrResults>(postResponse);
        }

        public static string GetTextToSpeak(string ocrResultsJson)
        {
            var ocrResults = Newtonsoft.Json.JsonConvert.DeserializeObject<Microsoft.ProjectOxford.Vision.Contract.OcrResults>(ocrResultsJson ?? "");
            return GetTextToSpeak(ocrResults);
        }

        public static string GetTextToSpeak(OcrResults ocrResults)
        {
            StringBuilder sb = new StringBuilder();
            if (ocrResults != null)
            {
                foreach (var o in ocrResults.Regions)
                {
                    foreach (var l in o.Lines)
                    {
                        foreach (var w in l.Words)
                        {
                            sb.Append(w.Text + " ");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        public static string GetFacesToSpeak(AnalysisResult analysisResult)
        {
            StringBuilder sb = new StringBuilder();
            if (analysisResult != null)
            {
                int male = analysisResult.Faces.Count(f => f.Gender == "Male");
                int female = analysisResult.Faces.Count(f => f.Gender == "Female");

                if (male > 0)
                    sb.Append(male + " males. ");

                if (female > 0)
                    sb.Append(female + " females. ");
            }
            return sb.ToString();
        }
    }
}
