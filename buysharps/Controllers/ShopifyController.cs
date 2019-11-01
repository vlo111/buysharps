using buysharps.Models;
using buysharps.Shopify.Oauth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using buysharps.Shopify;
using System.Web.Script.Serialization;
using buysharps.Models.BuysharpsModel.Tables;
using buysharps.Models.BuysharpsModel.UnitOfWork;
using System.Runtime.Caching;
using buysharps.Helpers;
using System.Collections;
using MvcPaging;

namespace buysharps.Controllers
{
    public class ShopifyController : Controller
    {
        private static ShopifyAuthorizationState authState;
        private GetRepositories repositories = new GetRepositories();
        private const int defaultPageSize = 5;

        /// <summary>
        /// Autentification from shopify
        /// </summary>
        /// <param name="shopname"></param>
        /// <returns></returns>
        public ActionResult Authorization(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // strip the .myshopify.com in case they added it
                string shopName = model.ShopName.Replace(".myshopify.com", String.Empty);

                // prepare the URL that will be executed after authorization is requested
                Uri requestUrl = this.Url.RequestContext.HttpContext.Request.Url;

                Uri returnURL = new Uri(string.Format("{0}://{1}{2}",
                                                        requestUrl.Scheme,
                                                        requestUrl.Authority,
                                                        this.Url.Action("ShopifyAuthCallback", "Shopify")));
                // Get Api keys from Web.config file
                var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

                // Get scope for full url when we get ...
                //
                //

                var authUrl = authorizer.GetAuthorizationURL(new string[] { ConfigurationManager.AppSettings["Shopify.Scope"] }, returnURL.ToString());

                return Redirect(authUrl);
            }
            return View();
        }
        /// <summary>
        /// This action is called by shopify after the authorization has finished
        /// Need to add this method to Shopify partner settings (Whitelisted redirection URL(s))
        /// </summary>
        /// <param name="code"></param>
        /// <param name="shop"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ShopifyAuthCallback(string code, string shop, string error)
        {
            if (!String.IsNullOrEmpty(error))
            {
                this.TempData["Error"] = error;
                return RedirectToAction("Authorization");
            }
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(shop))
                return RedirectToAction("Index", "Shopify");

            var shopName = shop.Replace(".myshopify.com", String.Empty);
            var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

            if (authState != null && authState.AccessToken != null)
            {
                ShopifyAuthorize.SetAuthorization(this.HttpContext, authState);
            }

            authState = authorizer.AuthorizeClient(code);

            return RedirectToAction("DataList", "Shopify");
        }

        /// <summary>
        /// This Action provides data from the Shopify store.
        /// Save the data to database
        /// Return Product List
        /// </summary>
        /// <returns></returns>
        public ActionResult DataList()
        {
            ShopifyAPIClient api = new ShopifyAPIClient(authState);

            JavaScriptSerializer js = new JavaScriptSerializer();

            // Get and Deserialize json products, customers and orders
            #region Deserialize Data form AliClient

            var root_products = js.Deserialize<RootProductsList>((string)api.Get("/admin/products.json"));
            var root_customers = js.Deserialize<RootCustomersList>((string)api.Get("/admin/customers.json"));
            var root_orders = js.Deserialize<RootOrdersList>((string)api.Get("/admin/draft_orders.json"));

            #endregion

            // Save to database if the db not exist
            if (!System.Data.Entity.Database.Exists("BuySharpsContext"))
            {
                repositories.ProductRepository.AddRange(root_products.products);
                repositories.CustomerRepository.AddRange(root_customers.Customers);
                repositories.OrderRepository.AddRange(root_orders.draft_orders);
            }

            // Data Caching
            #region Cache Manager

            if (!MemoryCache.Default.Contains("CacheKey"))
            {
                CacheManager.List = new ArrayList {
                    root_products.products,
                    root_orders.draft_orders,
                    root_customers.Customers };
            }

            #endregion


            return Redirect("Product");
        }

        #region Product Get, Show, Delete

        /// <summary>
        /// Return all products to view from cache list
        /// Paging Product List
        /// </summary>
        /// <returns></returns>
        public ActionResult Product(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value : 1;

            // Get products from cache
            var products = ((List<Product>)(CacheManager.List as ArrayList)[0]);
            ViewData["product_name"] = "";

            return View(products.ToPagedList(currentPageIndex, defaultPageSize));
        }
        /// <summary>
        /// This is return popup view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductDelete(long? id)
        {
            var products = ((List<Product>)(CacheManager.List as ArrayList)[0]);
            var product = products.Where(i => i.Id == id).FirstOrDefault();
            // replace html tag for display
            product.body_html = System.Text.RegularExpressions.Regex.Replace(product.body_html, "<[^>]*>", "");

            return PartialView("~/views/Shopify/_partial_views/_productDel.cshtml", product);
        }
        /// <summary>
        /// Delete from db, cache and shopify store
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProductDelete(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShopifyAPIClient api = new ShopifyAPIClient(authState);

                    // del from shopify store
                    api.Delete("/admin/products/" + product.Id + ".json");

                    // del from db
                    repositories.ProductRepository.RemoveById(product.Id);

                    // find by id product for delete 
                    var AllProducts = ((List<Product>)(CacheManager.List as ArrayList)[0]);
                    var prodcache = AllProducts.Where(p => p.Id == product.Id).FirstOrDefault();

                    // remove from cache
                    ((List<Product>)(CacheManager.List as ArrayList)[0]).Remove(prodcache);

                    return Json(new { success = "removed" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { error = "Model is not valid" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Customer Get, Show, Delete

        /// <summary>
        /// Return all customers to view from cache list
        /// </summary>
        /// <returns></returns>
        public ActionResult Customer(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value : 1;

            // Get customers from cache
            var customers = ((List<Customer>)(CacheManager.List as ArrayList)[2]);
            ViewData["customer_name"] = "";

            return View(customers.ToPagedList(currentPageIndex, defaultPageSize));
        }
        /// <summary>
        /// This is return popup view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CustomerDelete(long? id)
        {
            var customers = ((List<Customer>)(CacheManager.List as ArrayList)[2]);
            var customer = customers.Where(i => i.Id == id).FirstOrDefault();
            return PartialView("~/views/Shopify/_partial_views/_customerDel.cshtml", customer);
        }
        /// <summary>
        /// Delete from db, cache and shopify store
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CustomerDelete(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShopifyAPIClient api = new ShopifyAPIClient(authState);

                    // Del from shopify store
                    api.Delete("/admin/customers/" + customer.Id + ".json");

                    // Del from db
                    repositories.CustomerRepository.RemoveById(customer.Id);

                    // Find by id product for delete 
                    var AllCustomers = ((List<Customer>)(CacheManager.List as ArrayList)[2]);
                    var customercache = AllCustomers.Where(p => p.Id == customer.Id).FirstOrDefault();

                    //Remove from cache
                    ((List<Customer>)(CacheManager.List as ArrayList)[2]).Remove(customercache);

                    return Json(new { success = "removed" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { error = "Model is not valid" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Order Get, Show, Delete
        /// <summary>
        /// Return all orders to view from cache list
        /// </summary>
        /// <returns></returns>
        public ActionResult Order(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            ViewData["custome_order_name"] = "";

            // Get orders from cache
            var orders = ((List<DraftOrder>)(CacheManager.List as ArrayList)[1]);

            return View(orders.ToPagedList(currentPageIndex, defaultPageSize));
        }
        /// <summary>
        /// This is return popup view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDelete(long? id)
        {
            var orders = ((List<DraftOrder>)(CacheManager.List as ArrayList)[1]);
            var order = orders.Where(i => i.Id == id).FirstOrDefault();
            return PartialView("~/views/Shopify/_partial_views/_orderDel.cshtml", order);
        }
        /// <summary>
        /// Delete from db, cache and shopify store
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OrderDelete(DraftOrder order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShopifyAPIClient api = new ShopifyAPIClient(authState);

                    // Del from shopify store
                    api.Delete("/admin/orders/" + order.Id + ".json");

                    // Del from db
                    repositories.OrderRepository.RemoveById(order.Id);

                    // Find by id product for delete 
                    var AllOrders = ((List<DraftOrder>)(CacheManager.List as ArrayList)[1]);
                    var ordcache = AllOrders.Where(p => p.Id == order.Id).FirstOrDefault();

                    //Remove from cache
                    ((List<DraftOrder>)(CacheManager.List as ArrayList)[1]).Remove(ordcache);

                    return Json(new { success = "removed" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { error = "Model is not valid" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion





        /// <summary>
        /// This is Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}