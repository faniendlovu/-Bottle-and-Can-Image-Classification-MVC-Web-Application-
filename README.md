# Overview

The Bottle and Can Image classification application is an ASP.NET MVC Web application hosted on Azure. It integrates Azure Custom Vision to classify uploaded images as either a bottle or a Can. This document provides a comprehensive guide on how the application works and how to use it.

# Application Architecture
* ### ASP.NET MVC Web Application: 
The front-end user interface for uploading images.
* ### Azure Custom Vision: 
A machine learning model that classifies the uploaded images.
* ### Azure Web App: 
The hosting service for the web application.

# Prerequisites
* Visual Studio ( .NET development environment) 
* Azure subscription
* Azure Cognitive Services resources

# Setting Up

Before running the application, ensure the following keys are set correctly in your application's configuration:

* trainingEndpoint: The endpoint URL for training the Custom Vision model.
* training key: The key for accessing the training API.
* predictionEndpoint: The endpoint URL for making predictions.
* predictionKey: The key for accessing the prediction API.
* predictionResourceId: The resource ID for your Custom Vision prediction resource.

# Using the Application
* Navigate to the Index view.
* Click on the Predict button to go to the Predict view.
* Upload an image of a bottle or can.
* The application will display the classification results.
