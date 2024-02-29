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
            public const int ImageMaxLength = 2048;
        }

        public static class Category
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Image
        {
            public const int ImageMaxLength = 2048;
        }
        public static class ProductCategories
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Order
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class User
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Address
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Brand
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

        }


    }
}
