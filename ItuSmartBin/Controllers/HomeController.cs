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

        private static string trainingEndpoint = "[URL]";
        private static string trainingKey = "[API-KEY]";
        // You can obtain these values from the Keys and Endpoint page for your Custom Vision Prediction resource in the Azure Portal.
        private static string predictionEndpoint = "[URL]";
        private static string predictionKey = "[API-KEY]";

        //You can obtain this value from the Properties page for your Custom Vision Prediction resource in the Azure Portal.See the "Resource ID" field.This typically has a value such as:
        // /subscriptions/<your subscription ID>/resourceGroups/<your resource group>/providers/Microsoft.CognitiveServices/accounts/<your Custom Vision prediction resource name>
        private static string predictionResourceId = "";

        private static List<string> hemlockImages;
        private static List<string> japaneseCherryImages;
        private static Tag hemlockTag;
        private static Tag japaneseCherryTag;
        private static Iteration iteration;
        private static string publishedModelName = "Iteration";
        private static MemoryStream testImage;

  
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