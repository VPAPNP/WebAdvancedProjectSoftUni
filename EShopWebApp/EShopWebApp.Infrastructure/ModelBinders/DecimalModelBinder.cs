using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace EShopWebApp.Infrastructure.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext? bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
            {
                decimal parsedValue = 0m;
                bool susccess =  false;
                try
                {
                    string formDecimalValue = valueProviderResult.FirstValue
                        .Replace(",",CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    formDecimalValue = formDecimalValue
                        .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    parsedValue= Convert.ToDecimal(formDecimalValue);
                    susccess = true;
                }
                catch (FormatException fe)
                {
                   bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe,bindingContext.ModelMetadata);
                }

                if (susccess)
                {
                    bindingContext.Result = ModelBindingResult.Success(parsedValue);
                }
            }
            return Task.CompletedTask;
        }
    }
}
