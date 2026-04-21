using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PeakLogix.DAL.Interfaces;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Middlewares;

public class EmptyToNullModelBinder(IClaimData claims) : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value) && bindingContext.ModelType == typeof(string))
        {
            if (valueProviderResult != ValueProviderResult.None)
            {
                value = string.Empty;
            }
            else
            {
                value = null;
            }
        }
        if (bindingContext.ModelName.ToLower() == "username")
        {
            value = claims.UserName;
        }
        else if (bindingContext.ModelName.ToLower() == "wsid")
        {
            value = claims.WSID;
        }
        bindingContext.Result = ModelBindingResult.Success(value);
        return Task.CompletedTask;
    }
}
public class EmptyToNullModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(string))
        {
            return new BinderTypeModelBinder(typeof(EmptyToNullModelBinder));
        }

        return null;
    }
}