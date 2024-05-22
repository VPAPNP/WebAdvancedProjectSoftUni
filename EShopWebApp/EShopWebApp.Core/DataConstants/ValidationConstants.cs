namespace EShopWebApp.Core.DataConstants
{
    public static class ValidationConstants
    {
        public static class Product
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 3;
            public const int DescriptionMinLength = 3;
            public const int DescriptionMaxLength = 1000;
            public const int QuantityMinValue = 0;
            public const int QuantityMaxValue = Int32.MaxValue;
            public const int PriceMinValue = 0;
            public const int PriceMaxValue = Int32.MaxValue;
            public const int LongDescriptionMaxLength = 2000;
            
        }

        public static class Category
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
