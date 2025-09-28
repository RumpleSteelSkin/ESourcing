using ESourcing.Core.Repositories;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Clients;
using ESourcing.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers;

public class AuctionController(
    IUserRepository userRepository,
    ProductClient productClient,
    AuctionClient auctionClient,
    BidClient bidClient)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var auctionList = await auctionClient.GetAuctions();
        return auctionList.IsSuccess ? View(auctionList.Data) : View();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var productList = await productClient.GetProducts();
        if (productList.IsSuccess)
            ViewBag.ProductList = productList.Data;

        var userList = await userRepository.GetAllAsync();
        ViewBag.UserList = userList;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AuctionViewModel model)
    {
        model.Status = 0;
        model.CreatedAt = DateTime.Now;
        model.IncludedSellers.Add(model.SellerId);
        var createAuction = await auctionClient.CreateAuction(model);
        if (createAuction.IsSuccess)
            return RedirectToAction("Index");
        return View(model);
    }

    public async Task<IActionResult> Detail(string id)
    {
        var model = new AuctionBidsViewModel();

        var auctionResponse = await auctionClient.GetAuctionById(id);
        var bidsResponse = await bidClient.GelAllBidsByAuctionId(id);

        model.SellerUserName = HttpContext.User?.Identity.Name;
        model.AuctionId = auctionResponse.Data.Id;
        model.ProductId = auctionResponse.Data.ProductId;
        model.Bids = bidsResponse.Data;
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        model.IsAdmin = Convert.ToBoolean(isAdmin);

        return View(model);
    }

    [HttpPost]
    public async Task<Result<string>> SendBid(BidViewModel model)
    {
        model.CreateAt = DateTime.Now;
        var sendBidResponse = await bidClient.SendBid(model);
        return sendBidResponse;
    }

    [HttpPost]
    public async Task<IActionResult> CompleteBid(string id)
    {
        await auctionClient.CompleteBid(id);
        return RedirectToAction("Index");
    }
}