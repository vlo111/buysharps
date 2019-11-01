using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Tables
{
    public class Image
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("alt")]
        public string Alt { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }


        [JsonProperty("product_id")]
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}