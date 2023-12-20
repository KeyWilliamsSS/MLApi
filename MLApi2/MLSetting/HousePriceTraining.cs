﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;
using MLApi.Models;

namespace MLApi.MLSetting
{
    public class HousePriceTraining
    {
        public static readonly string RetrainFilePath = Path.GetFullPath("MLSetting\\housing.csv");
        public const char RetrainSeparatorChar = ',';
        public const bool RetrainHasHeader = true;

        /// <summary>
        /// Train a new model with the provided dataset.
        /// </summary>
        /// <param name="outputModelPath">File path for saving the model. Should be similar to "C:\YourPath\ModelName.mlnet"</param>
        /// <param name="inputDataFilePath">Path to the data file for training.</param>
        /// <param name="separatorChar">Separator character for delimited training file.</param>
        /// <param name="hasHeader">Boolean if training file has a header.</param>
        public static void Train(string outputModelPath, char separatorChar = RetrainSeparatorChar, bool hasHeader = RetrainHasHeader)
        {
            string inputDataFilePath = RetrainFilePath;
            var mlContext = new MLContext();

            var data = LoadIDataViewFromFile(mlContext, inputDataFilePath, separatorChar, hasHeader);
            var model = RetrainModel(mlContext, data);
            SaveModel(mlContext, model, data, outputModelPath);
        }

        /// <summary>
        /// Load an IDataView from a file path.
        /// </summary>
        /// <param name="mlContext">The common context for all ML.NET operations.</param>
        /// <param name="inputDataFilePath">Path to the data file for training.</param>
        /// <param name="separatorChar">Separator character for delimited training file.</param>
        /// <param name="hasHeader">Boolean if training file has a header.</param>
        /// <returns>IDataView with loaded training data.</returns>
        public static IDataView LoadIDataViewFromFile(MLContext mlContext, string inputDataFilePath, char separatorChar, bool hasHeader)
        {
            return mlContext.Data.LoadFromTextFile<ModelInput>(inputDataFilePath, separatorChar, hasHeader);
        }

        /// <summary>
        /// Save a model at the specified path.
        /// </summary>
        /// <param name="mlContext">The common context for all ML.NET operations.</param>
        /// <param name="model">Model to save.</param>
        /// <param name="data">IDataView used to train the model.</param>
        /// <param name="modelSavePath">File path for saving the model. Should be similar to "C:\YourPath\ModelName.mlnet.</param>
        public static void SaveModel(MLContext mlContext, ITransformer model, IDataView data, string modelSavePath)
        {
            // Pull the data schema from the IDataView used for training the model
            DataViewSchema dataViewSchema = data.Schema;

            using (var fs = File.Create(modelSavePath))
            {
                mlContext.Model.Save(model, dataViewSchema, fs);
            }
        }

        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(@"ocean_proximity", @"ocean_proximity", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair(@"longitude", @"longitude"), new InputOutputColumnPair(@"latitude", @"latitude"), new InputOutputColumnPair(@"housing_median_age", @"housing_median_age"), new InputOutputColumnPair(@"total_rooms", @"total_rooms"), new InputOutputColumnPair(@"total_bedrooms", @"total_bedrooms"), new InputOutputColumnPair(@"population", @"population"), new InputOutputColumnPair(@"households", @"households"), new InputOutputColumnPair(@"median_income", @"median_income") }))
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"ocean_proximity", @"longitude", @"latitude", @"housing_median_age", @"total_rooms", @"total_bedrooms", @"population", @"households", @"median_income" }))
                                    .Append(mlContext.Regression.Trainers.FastForest(new FastForestRegressionTrainer.Options() { NumberOfTrees = 104, NumberOfLeaves = 4, FeatureFraction = 0.7650916F, LabelColumnName = @"median_house_value", FeatureColumnName = @"Features" }));

            return pipeline;
        }
    }
}