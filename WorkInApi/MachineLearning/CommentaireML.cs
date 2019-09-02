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

        public static TrainTestData LoadData(MLContext mlContext,PredictedCommentaire commentaire)
        {
            IEnumerable<PredictedCommentaire> commentaires = new CommentaireCollection().GetItems(c=>c.Value!=commentaire.Value);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<PredictedCommentaire>(commentaires);
            //Ce code utilise la méthode TrainTestSplit() pour diviser l'ensemble de données chargé en ensembles de données train et test et les retourner dans la classe TrainTestData. 
            //La valeur par défaut est 10%, dans ce cas nous utilisons 20% pour évaluer plus de données.
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            return splitDataView;
        }

        private static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainSet)
        {
            //La méthode FeaturizeText() convertit la colonne de texte (SentimentText) en une colonne de type touche numérique caractéristiques de la colonne utilisée par l'algorithme machine learning 
            //et l'ajoute comme une nouvelle colonne d'ensemble de données
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(PredictedCommentaire.Value))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            var model = estimator.Fit(trainSet);
            return model;
        }

        private static void Evaluate(MLContext mlContext, ITransformer model, IDataView testSet)
        {
            IDataView predictions = model.Transform(testSet);
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
            //permet d'obtenir la précision d'un modèle, c'est-à-dire la proportion de prédictions correctes dans l'ensemble de test
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            // indique dans quelle mesure le modèle classe correctement les classes positive et négative
            Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            //est la mesure d'équilibre entre la précision et le rappel. Il faut que le F1Score soit le plus proche possible de 1
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
        }

        public static PredictedCommentaire AttribScoreAndTypeOfComment(this PredictedCommentaire commentaire)
        {
            MLContext mlContext = new MLContext();
            //chargement des commentaires de la base de donnée
            TrainTestData splitDataView = LoadData(mlContext,commentaire);
            //Transformation des commentaire pour être exploité par l'intelligence artificuelle
            ITransformer model = BuildAndTrainModel(mlContext, splitDataView.TrainSet);
            //Evaluation du model de prédiction ainsi on a le taux de précision de notre model
            Evaluate(mlContext, model, splitDataView.TestSet);
            PredictionEngine<PredictedCommentaire, PredictedModels.PredictedCommentaire> predictionFunction =
                mlContext.Model.CreatePredictionEngine<PredictedCommentaire, PredictedModels.PredictedCommentaire>(model);
            //Prédction grâce au model crée avec la classification binaire
            var resultPrediction = predictionFunction.Predict(commentaire);
            var collection = new CommentaireCollection();
            if (collection.GetItems(c => c.Value == commentaire.Value).Count() == 0)
                collection.NewItems(new PredictedCommentaire
                {
                    Id = resultPrediction.Id,
                    Value= resultPrediction.Value,
                    Date=resultPrediction.Date,
                    SentimentType = resultPrediction.SentimentType
                });
            return resultPrediction;
        }

        

       

        
    }
}
