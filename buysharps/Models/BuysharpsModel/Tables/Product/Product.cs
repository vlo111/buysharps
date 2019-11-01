using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace buysharps.Models.BuysharpsModel.Tables
{
    public class Product
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        public string body_html { get; set; }

        [DisplayName("Created at")]
        public DateTimeOffset created_at { get; set; }



        [JsonProperty("variants")]
        public List<Variant> Variants { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public class RootProductsList
    {
        public List<Product> products { get; set; }
    }

}