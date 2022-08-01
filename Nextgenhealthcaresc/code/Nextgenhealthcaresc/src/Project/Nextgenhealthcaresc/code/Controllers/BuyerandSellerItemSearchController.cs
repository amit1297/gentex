using Nextgenhealthcare.Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Nextgenhealthcare.Project.Web.Controllers
{
    public class BuyerandSellerItemSearchController : ApiController
    {
        // GET: BuyerandSellerItemSearch
        [Route("api/getbuyerdetails")]
        [HttpPost]
        public IHttpActionResult Index(SearchParam searchParam)
        {

            var contextItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(new Sitecore.Data.ID("{BDC40A58-4138-4C07-97D1-2BA776420D56}"));


            var listofBuyer = contextItem.GetChildren()
            .Select(x => new BuyerSearch
            {
                BuyerName = char.ToUpper(x.Fields["BuyerName"].Value[0]) + x.Fields["BuyerName"].Value.Substring(1),
                PaymentMode = char.ToUpper(x.Fields["PaymentMode"].Value[0]) + x.Fields["PaymentMode"].Value.Substring(1),
                PaymentDuration = char.ToUpper(x.Fields["PaymentDuration"].Value[0]) + x.Fields["PaymentDuration"].Value.Substring(1)
            })
            .ToList();
            var filterData = listofBuyer.Where(x => x.BuyerName.ToLower() == searchParam.SearchKeyword.ToLower());
            return Json(filterData.ToList());

        }

        [Route("api/getitemdetailsbyname")]
        [HttpPost]
        public IHttpActionResult Itemdetailsbyname(SearchParam searchParam)
        {
            List<ItemSearch> itemSearchs = new List<ItemSearch>();

            var contextItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(new Sitecore.Data.ID("{BDC40A58-4138-4C07-97D1-2BA776420D56}"));

            var buyerName = contextItem.GetChildren().FirstOrDefault(x => x.Fields["BuyerName"].Value.ToLower() == searchParam.SearchKeyword.ToLower());

            var buyerItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(new Sitecore.Data.ID("{9D676C58-5C89-449E-BA06-BD61BDD6317B}"));
            
            var buyerItemDataas = buyerItem.GetChildren().FirstOrDefault(x => x.Fields["BuyerName"].Value.ToLower() == buyerName.Name.ToLower() && x.Fields["ItemName"].Value.ToLower() == searchParam.ItemName.ToLower());
            if (buyerItemDataas != null)
            {
                ItemSearch itemSearch = new ItemSearch();
                itemSearch.ItemName = char.ToUpper(buyerItemDataas.Fields["ItemName"].Value[0]) + buyerItemDataas.Fields["ItemName"].Value.Substring(1);
                itemSearch.ItemDiscount = char.ToUpper(buyerItemDataas.Fields["ItemDiscount"].Value[0]) + buyerItemDataas.Fields["ItemDiscount"].Value.Substring(1);


                var sellerItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(new Sitecore.Data.ID("{CD43952A-435D-42D3-B186-417CD210EBDD}"));

                var sellerItemDataas = sellerItem.GetChildren().Where(x => x.Fields["ItemName"].Value.ToLower() == buyerItemDataas.Fields["ItemName"].Value.ToLower()).ToList();
                List<SellerItem> sellerItems = new List<SellerItem>();
                foreach (var item in sellerItemDataas)
                {
                    SellerItem sellerItem1 = new SellerItem
                    {
                        SellerName = char.ToUpper(item.Fields["SellerName"].Value[0]) + item.Fields["SellerName"].Value.Substring(1),
                        ItemDiscount = char.ToUpper(item.Fields["ItemDiscount"].Value[0]) + item.Fields["ItemDiscount"].Value.Substring(1)
                    };
                    sellerItems.Add(sellerItem1);
                }
                itemSearch.SellerItems = sellerItems;
                itemSearchs.Add(itemSearch);
                return Json(itemSearchs);
            }
            return Json(itemSearchs);


        }
    }
}