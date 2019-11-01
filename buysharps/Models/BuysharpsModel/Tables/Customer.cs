using buysharps.Models.BuysharpsModel.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Tables
{
    public partial class Customer
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        public string email { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public long orders_count { get; set; }
        public string state { get; set; }
        public string note { get; set; }
        public string phone { get; set; }

        public ICollection<DraftOrder>? Orders { set; get; }
    }

    public partial class RootCustomersList
    {
        [JsonProperty("customers")]
        public List<Customer> Customers { get; set; }
    }
}