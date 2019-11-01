
namespace buysharps.Shopify.Oauth
{
    /// <summary>
    /// The State, access token 
    /// </summary>
    public class ShopifyAuthorizationState
    {
        /// <summary>
        /// 
        /// </summary>
        public ShopifyAuthorizationState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; }
    }
}