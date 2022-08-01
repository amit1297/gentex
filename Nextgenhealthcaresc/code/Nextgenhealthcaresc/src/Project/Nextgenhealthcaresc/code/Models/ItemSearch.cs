using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nextgenhealthcare.Project.Web.Models
{
    public class ItemSearch
    {
        public string ItemName { get; set; }
        public string ItemDiscount { get; set; }
        public List<SellerItem> SellerItems { get; set; }
    }
}