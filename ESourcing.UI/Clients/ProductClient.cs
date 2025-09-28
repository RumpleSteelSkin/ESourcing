using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Models;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients;

public class ProductClient
{
    private HttpClient Client { get; }

    public ProductClient(HttpClient client, IConfiguration configuration)
    {
        Client = client;
        Client.BaseAddress = new Uri(configuration["BaseAddress"]!);
    }

    public async Task<Result<List<ProductViewModel>>> GetProducts()
    {
        var response = await Client.GetAsync("/Product/GetAll");
        if (!response.IsSuccessStatusCode)
            return new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ProductViewModel>>(responseData);
        return result != null && result.Count != 0
            ? new Result<List<ProductViewModel>>(true, ResultConstant.RecordFound, result.ToList())
            : new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
    }
}