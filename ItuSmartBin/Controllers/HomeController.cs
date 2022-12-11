using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItuSmartBin.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Globalization;

namespace ItuSmartBin.Controllers
{
    public class HomeController : Controller
    {

        private static string trainingEndpoint = "https://itusmartbin.cognitiveservices.azure.com/";
        private static string trainingKey = "490b907cee58498a9609dbf42e02a98c";
        // You can obtain these values from the Keys and Endpoint page for your Custom Vision Prediction resource in the Azure Portal.
        private static string predictionEndpoint = "https://itusmartbin-prediction.cognitiveservices.azure.com/";
        private static string predictionKey = "9244f9cabaa64daca1541c774d2af877";

        //You can obtain this value from the Properties page for your Custom Vision Prediction resource in the Azure Portal.See the "Resource ID" field.This typically has a value such as:
        // /subscriptions/<your subscription ID>/resourceGroups/<your resource group>/providers/Microsoft.CognitiveServices/accounts/<your Custom Vision prediction resource name>
        private static string predictionResourceId = "/subscriptions/f20f7988-4432-4e11-b635-e7041286715a/resourceGroups/Intelligence/providers/Microsoft.CognitiveServices/accounts/itusmartbin-Prediction";

        private static List<string> hemlockImages;
        private static List<string> japaneseCherryImages;
        private static Tag hemlockTag;
        private static Tag japaneseCherryTag;
        private static Iteration iteration;
        private static string publishedModelName = "Iteration3";
        private static MemoryStream testImage;

        // <snippet_creds>
        //// You can obtain these values from the Keys and Endpoint page for your Custom Vision resource in the Azure Portal.
        //private static string trainingEndpoint = "https://imageclassification5.cognitiveservices.azure.com/";
        //private static string trainingKey = "201f527e97524a0b8dcfb20bcd0b560e";
        //// You can obtain these values from the Keys and Endpoint page for your Custom Vision Prediction resource in the Azure Portal.
        //private static string predictionEndpoint = "https://imageclassification5-prediction.cognitiveservices.azure.com/";
        //private static string predictionKey = "e7ec5b2f4b2d4090bcbea6f3325e955f";

        //// You can obtain this value from the Properties page for your Custom Vision Prediction resource in the Azure Portal. See the "Resource ID" field. This typically has a value such as:
        //// /subscriptions/<your subscription ID>/resourceGroups/<your resource group>/providers/Microsoft.CognitiveServices/accounts/<your Custom Vision prediction resource name>
        //private static string predictionResourceId = "/subscriptions/f20f7988-4432-4e11-b635-e7041286715a/resourceGroups/Intelligence/providers/Microsoft.CognitiveServices/accounts/ImageClassification5-Prediction";

        //private static List<string> hemlockImages;
        //private static List<string> japaneseCherryImages;
        //private static Tag hemlockTag;
        //private static Tag japaneseCherryTag;
        //private static Iteration iteration;
        //private static string publishedModelName = "Iteration1";
        //private static MemoryStream testImage;
        //// </snippet_creds>

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Predict([Bind(Exclude = "Image")] binimageclassification model, HttpPostedFileBase imagefile)
        {
            Businesslogic logic = new Businesslogic();
            var file=  logic.ConvertToBytes(imagefile);

            testImage = new MemoryStream(file);

            CustomVisionPredictionClient predictionApi = AuthenticatePrediction(predictionEndpoint, predictionKey);
            CustomVisionTrainingClient trainingApi = AuthenticateTraining(trainingEndpoint, trainingKey);

            // Make a prediction against the new project

            Project findproject = trainingApi.GetProject(Guid.Parse("ba05cce5-2313-4a35-88da-d07333f5bae2"));
            //Project findproject = trainingApi.GetProject(Guid.Parse("d3253602-0c92-4fe3-a429-f888d273a885"));
            var result = predictionApi.ClassifyImage(findproject.Id, publishedModelName, testImage);

            List<binimageclassification> binimageclassificationList = new List<binimageclassification>();    
            // Loop over each prediction and write out the results
            foreach (var c in result.Predictions)
            {
                binimageclassification binimageclassificationitem = new binimageclassification();
                binimageclassificationitem.Tagname = c.TagName;
                binimageclassificationitem.percentage = c.Probability.ToString("P", CultureInfo.InvariantCulture);

                //if (c.Probability.ToString("P", CultureInfo.InvariantCulture) == "0")
                //{
                //    binimageclassificationitem.results = "No";
                //}
                //if (c.Probability.ToString("P", CultureInfo.InvariantCulture) == "70")
                //{
                //    binimageclassificationitem.results = "Yes";
                //}
                binimageclassificationList.Add(binimageclassificationitem);

             
            }
            var list = binimageclassificationList;
            ViewBag.binList = list;
            return View();
        }

        private static CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey)
        {
            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = endpoint
            };
            return trainingApi;
        }
        private static CustomVisionPredictionClient AuthenticatePrediction(string endpoint, string predictionKey)
        {
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(predictionKey))
            {
                Endpoint = endpoint
            };
            return predictionApi;
        }
    }
}