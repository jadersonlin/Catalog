using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Extraction
{
    public class ExcelExtractionService : IExtractionService
    {
        private readonly IStorageService storageService;

        private const string CategoryIdCell = "B1";
        private const int LmColumn = 1;
        private const int NameColumn = 2;
        private const int FreeShippingColumn = 3;
        private const int DescriptionColumn = 4;
        private const int PriceColumn = 5;
        private const int FirstCollectionRow = 4;

        public ExcelExtractionService(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public async Task<Tuple<IList<ProductData>, IList<KeyValuePair<string, string>>>> ExtractDataFromFile(string fileId)
        {
            var fileStream = storageService.GetFile(fileId);

            var validator = new ExcelExtractionValidator();
            var validationErrors = new List<KeyValuePair<string, string>>();
            IList<ProductData> products = new List<ProductData>();

            await ExtractDataFromFile(fileStream, validator, products);

            if (!validator.IsExtractionValid())
            {
                products = new List<ProductData>();
                validationErrors = validator.GetValidationErrors();
            }

            return new Tuple<IList<ProductData>, IList<KeyValuePair<string, string>>>(products, validationErrors);
        }

        private async Task ExtractDataFromFile(Task<Stream> fileStream, ExcelExtractionValidator validator, IList<ProductData> products)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(await fileStream);

            var worksheet = package.Workbook.Worksheets[0];

            GetProductsFromSpreadsheet(worksheet, validator, products);
        }

        private void GetProductsFromSpreadsheet(ExcelWorksheet worksheet, ExcelExtractionValidator validator, IList<ProductData> products)
        {
            var categoryIdCellObject = worksheet.Cells[CategoryIdCell].Value;

            var categoryId = validator.TryGetInt("CategoryId", categoryIdCellObject, CategoryIdCell);

            for (var row = FirstCollectionRow; row < worksheet.Cells.Rows; row++)
            {
                var lmCellObject = worksheet.Cells[row, LmColumn].Value;

                if (lmCellObject == null || string.IsNullOrWhiteSpace(lmCellObject.ToString()))
                {
                    if (row == FirstCollectionRow)
                        validator.NotifyEmptySpreadsheet();
                    
                    break;
                }

                var lm = validator.TryGetInt("Lm", lmCellObject, row, LmColumn);

                var nameCellObject = worksheet.Cells[row, NameColumn].Value;
                var name = validator.TryGetString("Name", nameCellObject, row, NameColumn);

                var freeShippingCellObject = worksheet.Cells[row, FreeShippingColumn].Value;
                var freeShipping = validator.TryGetBool("FreeShipping", freeShippingCellObject, row, FreeShippingColumn);

                var descriptionCellObject = worksheet.Cells[row, DescriptionColumn].Value;
                var description = validator.TryGetString("Description", descriptionCellObject, row, DescriptionColumn);

                var priceCellObject = worksheet.Cells[row, PriceColumn].Value;
                var price = validator.TryGetDecimal("Price", priceCellObject, row, PriceColumn);

                if (validator.IsExtractionValid())
                    products.Add(new ProductData
                    {
                        CategoryId = categoryId.Value,
                        Lm = lm.Value,
                        Description = description,
                        Name = name,
                        Price = price.Value,
                        FreeShipping = freeShipping.Value
                    });
            }
        }
    }
}
