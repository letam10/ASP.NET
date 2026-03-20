 using System.Text.Json;
using TechShop.Models;

namespace TechShop.Services
{
    public interface ICartService
    {
        List<CartItem> GetCart(ISession session);
        void AddToCart(ISession session, CartItem item);
        void UpdateQuantity(ISession session, int productId, int quantity);
        void RemoveFromCart(ISession session, int productId);
        void ClearCart(ISession session);
        decimal GetTotal(ISession session);
    }

    public class CartService : ICartService
    {
        private const string CartKey = "Cart";

        public List<CartItem> GetCart(ISession session)
        {
            var json = session.GetString(CartKey);
            return json == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        public void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            var existing = cart.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null) existing.Quantity += item.Quantity;
            else cart.Add(item);
            SaveCart(session, cart);
        }

        public void UpdateQuantity(ISession session, int productId, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = quantity;
            }
            SaveCart(session, cart);
        }

        public void RemoveFromCart(ISession session, int productId)
        {
            var cart = GetCart(session);
            cart.RemoveAll(c => c.ProductId == productId);
            SaveCart(session, cart);
        }

        public void ClearCart(ISession session) => session.Remove(CartKey);

        public decimal GetTotal(ISession session) => GetCart(session).Sum(c => c.Subtotal);

        private void SaveCart(ISession session, List<CartItem> cart)
            => session.SetString(CartKey, JsonSerializer.Serialize(cart));
    }
}
