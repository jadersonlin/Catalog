using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CatalogoAPI.Tests
{
    public class ProdutosControllerTests
    {
        private HttpClient httpClient;

        public ProdutosControllerTests(IProdutoRepository _repository)
        {
            httpClient = new HttpClient();
        }

        [Fact]
        public async Task POST_insert_products_from_spreadsheet()
        {
            var response = await httpClient.PostAsync("/weatherforecast", );

        }
    }
}
