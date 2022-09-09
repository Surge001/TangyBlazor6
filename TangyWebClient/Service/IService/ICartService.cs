using TangyWebClient.ViewModels;

namespace TangyWebClient.Service.IService
{
    public interface ICartService
    {
        public event Action OnChange;
        Task DecrementCart(ShoppingCart shoppingCart);

        Task IncrementCart(ShoppingCart shoppingCart);
    }
}
