namespace WebApplication1.Models.Repositories
{
    public static class ShirtRepository
    {
        private static List<Shirt> _shirts = new List<Shirt>()
        {
            new Shirt() { Id = 1, BrandName = "Nike", Color = "Blue", IsForMen = true, Price = 30.0, Size = 10 },
            new Shirt() { Id = 2, BrandName = "Nike", Color = "Black", IsForMen = true, Price = 35.0, Size = 12 },
            new Shirt() { Id = 3, BrandName = "Adidas", Color = "Pink", IsForMen = false, Price = 28.0, Size = 8 },
            new Shirt() { Id = 4, BrandName = "Adidas", Color = "Yellow", IsForMen = false, Price = 30.0, Size = 9 }
        };

        // Why needed??? Just check if GetShirtById(id) is null or not
        public static bool ShirtExists(int id)
        {
            return _shirts.Any(shirt => shirt.Id == id);
        }

        public static List<Shirt> GetShirts()
        {
            return _shirts;
        }

        public static void AddShirt(Shirt toAdd)
        {
            int maxId = _shirts.Max(shirt => shirt.Id);
            toAdd.Id = maxId + 1;
            _shirts.Add(toAdd);
        }

        public static Shirt? GetShirtByProperties(Shirt query)
        {
            return (
                from shirt in _shirts
                where shirt.BrandName == query.BrandName
                where shirt.Color == query.Color
                where shirt.Size == query.Size
                where shirt.IsForMen == query.IsForMen
                where shirt.Price == query.Price
                select shirt
            ).FirstOrDefault();
        }

        public static Shirt? GetShirtById(int id)
        {
            return _shirts.FirstOrDefault(shirt => shirt.Id == id);
        }

        public static void UpdateShirt(Shirt shirt)
        {
            // Will be validated before here
            Shirt toUpdate = _shirts.First(s => s.Id == shirt.Id);
            toUpdate.BrandName = shirt.BrandName;
            toUpdate.Color = shirt.Color;
            toUpdate.Size = shirt.Size;
            toUpdate.IsForMen = shirt.IsForMen;
            toUpdate.Price = shirt.Price;
        }

        public static void DeleteShirt(int id)
        {
            Shirt? toDelete = GetShirtById(id);

            if (toDelete != null)
            {
                _shirts.Remove(toDelete);
            }
        }
    }
}
