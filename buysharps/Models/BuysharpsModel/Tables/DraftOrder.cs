using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Tables
{
    public partial class DraftOrder
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        public DateTimeOffset created_at { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total_price")]
        public string TotalPrice { get; set; }


        [JsonProperty("customer")]
        public Customer? Customer { get; set; }
    }

    public partial class RootOrdersList
    {
        public List<DraftOrder> draft_orders { get; set; }
    }
}