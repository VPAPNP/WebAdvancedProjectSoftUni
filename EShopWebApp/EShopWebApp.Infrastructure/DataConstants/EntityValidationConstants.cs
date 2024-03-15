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
        

        

        public static class User
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        

        public static class Brand
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

        }

        public static class PaymentInfo
        {
            public  const int CardNumberMinLength = 16;
            public  const int CardNumberMaxLength = 16;


        }

        public static class ShippingInfo
        {
           
            public const int AddressMaxLength = 50;
            
            public const int CityMaxLength = 50;
           
            public const int RecipientNameMaxLength = 50;
        }

        public static class ApplicationUser
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int MiddleNameMaxLength = 50;
            public const int AddressMaxLength = 500;
            
        }

    }
}
