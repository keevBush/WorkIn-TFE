using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkInApi.DAL;
using WorkInApi.Models;
using WorkInApi.PredictedModels;
using static Microsoft.ML.DataOperationsCatalog;

namespace WorkInApi.MachineLearning
{
    public static class CommentaireML
    {
        public static PredictedCommentaire AttribScoreAndTypeOfComment(this PredictedCommentaire commentaire)
        {
            MLContext mlContext = new MLContext();
            TrainTestData splitDataView = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, splitDataView.TrainSet);
            Evaluate(mlContext, model, splitDataView.TestSet);
            PredictionEngine<PredictedCommentaire, PredictedModels.PredictedCommentaire> predictionFunction = mlContext.Model.CreatePredictionEngine<PredictedCommentaire, PredictedModels.PredictedCommentaire>(model);
            var resultPrediction = predictionFunction.Predict(commentaire);
            return resultPrediction;
        }

        private static void Evaluate(MLContext mlContext, ITransformer model, IDataView testSet)
        {
            IDataView predictions = model.Transform(testSet);
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
        }

        private static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainSet)
        {
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(PredictedCommentaire.Value))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            var model = estimator.Fit(trainSet);
            return model;
        }

        public static TrainTestData LoadData(MLContext mlContext)
        {
            IEnumerable<PredictedCommentaire> commentaires = new CommentaireCollection().GetAllItem();
            IDataView dataView = mlContext.Data.LoadFromEnumerable<PredictedCommentaire>(commentaires);
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            return splitDataView;
        }
    }
}
