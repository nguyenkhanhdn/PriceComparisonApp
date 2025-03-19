using Microsoft.ML.Data;

namespace PriceComparisonApp.Data
{
    public class ProductVector
    {
        public float Id { get; set; }
        [VectorType(100)] // Giả định vector có tối đa 100 chiều
        public float[] FeatureVector { get; set; }
    }
}
