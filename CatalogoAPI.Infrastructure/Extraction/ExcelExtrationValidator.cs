using System.Collections.Generic;
using System.Linq;

namespace Catalog.Infrastructure.Extraction
{
    public class ExcelExtrationValidator
    {
        private readonly IList<KeyValuePair<string, string>> validationErrors;

        public ExcelExtrationValidator()
        {
            validationErrors = new List<KeyValuePair<string, string>>();
        }

        public bool IsExtractionValid()
        {
            return validationErrors.Count == 0;
        }

        public List<KeyValuePair<string, string>> GetValidationErrors()
        {
            return validationErrors.ToList();
        }

        public int? TryGetString(string propertyName, object value, string cellAdress)
        {
            var intValue = TryGetInt(value);

            if (intValue == null)
                validationErrors.Add(new KeyValuePair<string, string>($"Invalid {propertyName} in cell {cellAdress}", value.ToString()));

            return intValue;
        }

        public int? TryGetInt(object value)
        {
            if (int.TryParse(value.ToString(), out var result))
                return result;

            return null;
        }

        public int? TryGetInt(string propertyName, object value, int row, int column)
        {
            var intValue = TryGetInt(value);

            if (intValue == null)
                AddValidationError(propertyName, value, row, column);

            return intValue;
        }

        public string TryGetString(string propertyName, object value, int row, int column)
        {
            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
                AddValidationError(propertyName, value, row, column);

            return stringValue;
        }

        public bool? TryGetBool(string propertyName, object value, int row, int column)
        {
            var valueString = value.ToString();
            if (valueString == "1" || valueString == "0")
                return valueString == "1";

            AddValidationError(propertyName, value, row, column);
            return null;
        }

        public decimal? TryGetDecimal(string propertyName, object value, int row, int column)
        {
            if (decimal.TryParse(value.ToString(), out var result))
                return result;

            AddValidationError(propertyName, value, row, column);
            return null;
        }
        
        private void AddValidationError(string propertyName, object value, int row, int column)
        {
            validationErrors.Add(
                new KeyValuePair<string, string>($"Invalid {propertyName} in row {row + 1} and column {column + 1} ",
                    value.ToString()));
        }

        public void NotifyEmptySpreadsheet()
        {
            validationErrors.Add(new KeyValuePair<string, string>("Invalid spreadsheet","Empty file."));
        }
    }
}
