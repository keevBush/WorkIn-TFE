using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WorkInTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pubs = new List<Publication>
            {
                new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date= new DateTime(2018,12,12),
                    Libele=new []{"HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId1"
                    ,TypePublication=TypePublication.Link
                    
                },
                new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,2,12),
                    Libele=new []{"HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId2"
                    ,TypePublication=TypePublication.Link
                },
                new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,1,1),
                    Libele=new []{ "HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId1"
                    ,TypePublication=TypePublication.Link
                },
                new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,5,12),
                    Libele=new []{"HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId2"
                    ,TypePublication=TypePublication.Link
                },
                 new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,6,12),
                    Libele=new []{"HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId2"
                     ,TypePublication=TypePublication.Link
                },
                 new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,5,12),
                    Libele=new []{"HTML 5","CSS 3" ,"XML"},
                    Link="link",
                    UserIdentity="UId3"
                     ,TypePublication=TypePublication.Link
                },
                  new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,5,12),
                    Libele=new []{"HTML 5","XML"},
                    Link="link",
                    UserIdentity="UId4"
                     ,TypePublication=TypePublication.Link
                },
                   new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,5,12),
                    Libele=new []{"CSS 3" ,"XML"},
                    Link="link",
                    UserIdentity="UId5"
                     ,TypePublication=TypePublication.Link
                },
                  new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=new DateTime(2019,5,15),
                    Libele=new []{"HTML 5","CSS 3","XML" },
                    Link="link",
                    UserIdentity="UId2"
                     ,TypePublication=TypePublication.Link
                },
                   new Publication
                {
                    Id=Guid.NewGuid().ToString(),
                    Date=DateTime.Now,
                    Libele=new []{"HTML 5","CSS 3" },
                    Link="link",
                    UserIdentity="UId1"
                     ,TypePublication=TypePublication.Link
                },

            };
            var _mlContext = new MLContext(seed: 0);
            var _trainingDataView = _mlContext.Data.LoadFromEnumerable(pubs);
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "UserIdentity", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Libele", outputColumnName: "LibeleFeaturized"))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Date", outputColumnName: "DateFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "LibeleFeaturized", "DateFeaturized"));
                //.AppendCacheCheckpoint(_mlContext);

            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features"))
                   .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            var _trainedModel = trainingPipeline.Fit(_trainingDataView);

            var _predEngine = _mlContext.Model.CreatePredictionEngine<Publication, UserPredict>(_trainedModel);
            var post = new Publication
            {
                Date = DateTime.Now,
                Libele = new [] { "XAML" },
            };
            Console.WriteLine(JsonConvert.SerializeObject( _predEngine.Predict(post)));
            //Console.WriteLine(JsonConvert.SerializeObject(ConsumeModel()));
            Console.ReadKey();
        }

        

        //public static ModelOutput ConsumeModel()
        //{
        //    // Load the model
        //    MLContext mlContext = new MLContext();
        //    ITransformer mlModel = mlContext.Model.Load("MLModel.zip", out var modelInputSchema);
        //    var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

        //    // Use the code below to add input data
        //    var input = new ModelInput() {
        //        Date=DateTime.Now.ToString(),
        //        IdPub=Guid.NewGuid().ToString(),
        //        Libele="XML"
        //    };
        //    // input.

        //    // Try model on sample data
        //    ModelOutput result = predEngine.Predict(input);
        //    return result;
        //}
    }
    public class Publication
    {
        [NoColumn]
        public string Id { get; set; }
        [VectorType()]
        public string[] Libele { get; set; }
        public string UserIdentity { get; set; }// = new UserIdentity();
        [VectorType()]
        public DateTime Date { get; set; }
        public string Link { get; set; }
        [NoColumn]
        public TypePublication TypePublication { get; set; }
    }
    public enum TypePublication
    {
        Texte,Link,Image,Video
    }

    public class UserIdentity
    {
        [ColumnName("userId")]
        public string Id { get; set; }
        [ColumnName("userNom")]
        public string Nom { get; set; }
        
    }
    public class UserPredict
    {
        [ColumnName("PredictedLabel")]
        public string UserIdentity { get; set; }
    }
}
