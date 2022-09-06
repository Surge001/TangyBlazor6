using TangyWebClient.ViewModels;

namespace TangyWebClient.Service.IService
{
    public interface ICartService
    {
        Task DecrementCart(ShoppingCart shoppingCart);

        Task IncrementCart(ShoppingCart shoppingCart);
    }
}
