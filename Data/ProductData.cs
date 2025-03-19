using CsvHelper;
using Microsoft.ML.Data;
using System.Globalization;

namespace PriceComparisonApp.Data
{
    public static class ProductData
    {
        public static List<Product> LoadProductsFromCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return new List<Product>(csv.GetRecords<Product>());
        }
    }
}
