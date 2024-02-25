namespace EShopWebApp.Infrastructure.DataConstants
{
    public static class EntityValidationConstants
    {
        public static class Product
        {
            public const int NameMaxLength = 50;
            public const int DescriptionMaxLength = 1000;
            public const int QuantityMinValue = 0;
            public const int QuantityMaxValue = Int32.MaxValue;
            public const decimal PriceMaxValue = Decimal.MaxValue;
        }

        public static class Category
        {
            public const int NameMaxLength = 100;
        }
    }
}
