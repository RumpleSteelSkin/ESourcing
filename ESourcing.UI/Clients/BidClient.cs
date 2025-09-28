using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Models;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients;

public class BidClient
{
    private HttpClient Client { get; }

    public BidClient(HttpClient client)
    {
        Client = client;
        Client.BaseAddress = new Uri(CommonInfo.BaseAddress);
    }

    public async Task<Result<List<BidViewModel>>> GelAllBidsByAuctionId(string id)
    {
        var response = await Client.GetAsync("/Bid/GetBidsByAuctionId/" + id);
        if (!response.IsSuccessStatusCode) return new Result<List<BidViewModel>>(false, ResultConstant.RecordNotFound);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<BidViewModel>>(responseData);
        return result != null && result.Count != 0
            ? new Result<List<BidViewModel>>(true, ResultConstant.RecordFound, result.ToList())
            : new Result<List<BidViewModel>>(false, ResultConstant.RecordNotFound);
    }

    public async Task<Result<string>> SendBid(BidViewModel model)
    {
        var dataAsString = JsonConvert.SerializeObject(model);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        var response = await Client.PostAsync("/Bid/SendBid", content);
        if (!response.IsSuccessStatusCode) return new Result<string>(false, ResultConstant.RecordCreateNotSuccessfully);
        var responseData = await response.Content.ReadAsStringAsync();
        return new Result<string>(true, ResultConstant.RecordCreateSuccessfully, responseData);
    }
}