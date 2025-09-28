using System.Net.Http.Headers;
using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Models;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients;

public class AuctionClient
{
    private HttpClient Client { get; }

    public AuctionClient(HttpClient client,IConfiguration configuration)
    {
        Client = client;
        Client.BaseAddress = new Uri(configuration["BaseAddress"]!);
    }

    public async Task<Result<List<AuctionViewModel>>> GetAuctions()
    {
        var response = await Client.GetAsync("/Auction/GetAll");
        if (!response.IsSuccessStatusCode)
            return new Result<List<AuctionViewModel>>(false, ResultConstant.RecordNotFound);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<AuctionViewModel>>(responseData);
        return result != null && result.Count != 0
            ? new Result<List<AuctionViewModel>>(true, ResultConstant.RecordFound, result.ToList())
            : new Result<List<AuctionViewModel>>(false, ResultConstant.RecordNotFound);
    }

    public async Task<Result<AuctionViewModel>> CreateAuction(AuctionViewModel model)
    {
        var dataAsString = JsonConvert.SerializeObject(model);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await Client.PostAsync($"/Auction/Create", content);
        if (!response.IsSuccessStatusCode)
            return new Result<AuctionViewModel>(false, ResultConstant.RecordCreateNotSuccessfully);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
        return result != null
            ? new Result<AuctionViewModel>(true, ResultConstant.RecordCreateSuccessfully, result)
            : new Result<AuctionViewModel>(false, ResultConstant.RecordCreateNotSuccessfully);
    }

    public async Task<Result<AuctionViewModel>> GetAuctionById(string id)
    {
        var response = await Client.GetAsync("/Auction/GetById/" + id);
        if (!response.IsSuccessStatusCode) return new Result<AuctionViewModel>(false, ResultConstant.RecordNotFound);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
        return result != null
            ? new Result<AuctionViewModel>(true, ResultConstant.RecordFound, result)
            : new Result<AuctionViewModel>(false, ResultConstant.RecordNotFound);
    }

    public async Task<Result<string>> CompleteBid(string id)
    {
        var response = await Client.PostAsync("/Auction/CompleteAuction/" + id, null);
        if (!response.IsSuccessStatusCode) return new Result<string>(false, ResultConstant.RecordCreateNotSuccessfully);
        var responseData = await response.Content.ReadAsStringAsync();
        return new Result<string>(true, ResultConstant.RecordCreateSuccessfully, responseData);
    }
}