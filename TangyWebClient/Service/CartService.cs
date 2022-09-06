using Blazored.LocalStorage;
using Tangy.Common;
using TangyWebClient.Service.IService;
using TangyWebClient.ViewModels;

namespace TangyWebClient.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorage;

        public CartService(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cart = await this.localStorage
                .GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            

            for(int i=0; i<cart.Count; i++)
            {
                if (cart[i].ProductId == cartToDecrement.ProductId
                    && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cart[i].Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= cartToDecrement.Count;
                    }
                }

            }
            await localStorage.SetItemAsync(SD.ShoppingCart, cart);
        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            var cart = await this.localStorage
                .GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemCart = false;
            if (cart == null)
            {
                cart = new List<ShoppingCart>();
            }
            foreach (var item in cart)
            {
                if (item.ProductId == cartToAdd.ProductId
                    && item.ProductPriceId == cartToAdd.ProductPriceId)
                {
                    itemCart = true;
                    item.Count += cartToAdd.Count;
                }
            }
            if (!itemCart)
            {
                cart.Add(new ShoppingCart(){
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }
            await localStorage.SetItemAsync(SD.ShoppingCart, cart);
        }
    }
}
