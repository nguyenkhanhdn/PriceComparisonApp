using Microsoft.ML;

namespace PriceComparisonApp.Data
{
    public class ProductRecommender
    {
        public ProductRecommender() { }
        public void ProductRecomend()
        {
            MLContext mlContext = new MLContext();

            // Đọc dữ liệu từ file CSV
            string filePath = "products.csv"; // Đường dẫn tệp CSV
            var products = ProductData.LoadProductsFromCsv(filePath);

            // Chuyển đổi sản phẩm thành chuỗi đặc trưng
            var productFeatures = products.Select(p => new ProductFeature
            {
                Id = p.Id,
                Features = $"{p.Price} {p.Rate} {p.Manufacturer}"
            }).ToList();

            // Chuyển dữ liệu thành IDataView
            IDataView dataView = mlContext.Data.LoadFromEnumerable(productFeatures);

            // Áp dụng Text Featurizer để chuyển đổi đặc trưng thành vector số
            var pipeline = mlContext.Transforms.Text.FeaturizeText("FeatureVector", "Features");
            var model = pipeline.Fit(dataView);
            var transformedData = model.Transform(dataView);

            // Trích xuất vector đặc trưng
            var featureVectors = mlContext.Data.CreateEnumerable<ProductVector>(transformedData, reuseRowObject: false).ToList();

            // Chọn sản phẩm cần gợi ý
            int selectedProductId = 1; // Ví dụ: Chọn sản phẩm ID = 1
            var selectedProduct = featureVectors.FirstOrDefault(p => p.Id == selectedProductId);

            if (selectedProduct != null)
            {
                Console.WriteLine($"Gợi ý cho sản phẩm: {products.First(p => p.Id == selectedProductId).Name}");

                // Tính toán độ tương đồng Cosine
                var recommendations = featureVectors
                    .Where(p => p.Id != selectedProductId) // Loại bỏ sản phẩm đã chọn
                    .Select(p => new
                    {
                        ProductId = p.Id,
                        Similarity = CosineSimilarity(selectedProduct.FeatureVector, p.FeatureVector)
                    })
                    .OrderByDescending(p => p.Similarity) // Sắp xếp theo độ tương đồng giảm dần
                    .Take(3); // Lấy 3 sản phẩm gợi ý hàng đầu

                foreach (var rec in recommendations)
                {
                    var product = products.First(p => p.Id == rec.ProductId);
                    Console.WriteLine($"Sản phẩm ID: {product.Id}, Tên: {product.Name}, Độ tương đồng: {rec.Similarity:F4}");
                }
            }
        }
        public static float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

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
