using ProjectAspEShop2024.Dto;

namespace ProjectAspEShop2024.BusinesLogic
{
    public class Cart
    {
        public string ContinueShoppingUrl { get; set; }

        public int ProductCount => Positions
            .Sum(p => p.Quantity);

        public int TotalCost => Positions
            .Sum(p => p.Product.Price * p.Quantity);

        public List<CartRecord> Positions { get; set; }

        public Cart()
        {
            Positions = new List<CartRecord>();
        }

        public void Clear()
        {
            Positions.Clear();
        }

        public void AddProduct(ProductDto productDto)
        {
            // ищем данный продукт среди позиций в корзине
            var position = Positions
                .FirstOrDefault(p => 
                    p.Product.ProductId == productDto.ProductId);

            if (position == null)
            { // не нашли - делаем новую запись
                position = new CartRecord
                {
                    Product = productDto,
                    Quantity = 1
                };
                Positions.Add(position);
            }
            else
            { // нашли - увеличиваем кол-во
                position.Quantity++;
            }
        }

        public void RemoveProduct(ProductDto productDto, int count)
        {
            // ищем данный продукт среди позиций в корзине
            var position = Positions
                .FirstOrDefault(p =>
                    p.Product.ProductId == productDto.ProductId);

            if (position == null) return;

             // нашли - уменьшаем кол-во
            position.Quantity -= count;
            if (position.Quantity <= 0)
            { // удаляем позицию
                Positions.Remove(position);
            } 
        }
    }

    public class CartRecord
    {
        public int Cost => Product.Price * Quantity;

        public ProductDto Product { get; set; }

        public int Quantity { get; set; }
    }
}
