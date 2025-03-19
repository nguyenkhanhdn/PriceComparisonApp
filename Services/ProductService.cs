using CsvHelper;
using Microsoft.ML;
using PriceComparisonApp.Data;
using System.Globalization;

namespace PriceComparisonApp.Services
{
    public class ProductService
    {
        private static readonly string ModelPath = @"Data\product_recommender_model.zip";
        private static readonly string FilePath = "Data/products.csv";
        private MLContext mlContext;
        private ITransformer model;

        public ProductService()
        {
            mlContext = new MLContext();

            // Load hoặc Train mô hình
            if (File.Exists(ModelPath))
            {
                DataViewSchema modelSchema;
                model = mlContext.Model.Load(ModelPath, out modelSchema);
            }
            else
            {
                TrainModel();
            }
        }

        private void TrainModel()
        {
            var curDir = Environment.CurrentDirectory;            
            var modelFile = Path.Combine(curDir, @"Data\product_recommender_model.zip");

            var products = LoadProductsFromCsv();
            var productFeatures = products.Select(p => new ProductFeature
            {
                Id = p.Id,
                Features = $"{p.Price} {p.Rate} {p.Manufacturer}"
            }).ToList();

            IDataView dataView = mlContext.Data.LoadFromEnumerable(productFeatures);
            var pipeline = mlContext.Transforms.Text.FeaturizeText("FeatureVector", "Features");
            model = pipeline.Fit(dataView);

            // Lưu mô hình
            mlContext.Model.Save(model, dataView.Schema, modelFile);
        }

        public List<Product> GetRecommendedProducts(int productId)
        {
            var products = LoadProductsFromCsv();
            var productFeatures = products.Select(p => new ProductFeature
            {
                Id = p.Id,
                Features = $"{p.Price} {p.Rate} {p.Manufacturer}"
            }).ToList();

            IDataView dataView = mlContext.Data.LoadFromEnumerable(productFeatures);
            var transformedData = model.Transform(dataView);
            var featureVectors = mlContext.Data.CreateEnumerable<ProductVector>(transformedData, reuseRowObject: false).ToList();

            var selectedProduct = featureVectors.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct == null) return new List<Product>();

            var recommendations = featureVectors
                .Where(p => p.Id != productId)
                .Select(p => new
                {
                    ProductId = p.Id,
                    Similarity = CosineSimilarity(selectedProduct.FeatureVector, p.FeatureVector)
                })
                .OrderByDescending(p => p.Similarity)
                .Take(5)
                .Select(rec => products.First(p => p.Id == rec.ProductId))
                .ToList();

            return recommendations;
        }

        private List<Product> LoadProductsFromCsv()
        {
            var curDir = Environment.CurrentDirectory;            

            var dataFile = Path.Combine(curDir, @"data\Products.csv");

            using var reader = new StreamReader(dataFile);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Product>().ToList();
        }

        private float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            float dotProduct = 0, magnitudeA = 0, magnitudeB = 0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }
            magnitudeA = (float)Math.Sqrt(magnitudeA);
            magnitudeB = (float)Math.Sqrt(magnitudeB);
            return (magnitudeA * magnitudeB == 0) ? 0 : dotProduct / (magnitudeA * magnitudeB);
        }
    }
}
