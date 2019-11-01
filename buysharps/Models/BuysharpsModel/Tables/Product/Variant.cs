using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Tables
{
    public class Variant
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }


        [JsonProperty("product_id")]
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}