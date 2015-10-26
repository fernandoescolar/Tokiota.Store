namespace Tokiota.Store.Helpers
{
    using Resources;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Resources;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class EnumHelper
    {
        private const string TypeExceptionMessage = "The 'enumType' should be an enumeration.";
        public static MvcHtmlString DropDownEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(TypeExceptionMessage, "enumType");
            }

            var resourceManager = new ResourceManager(typeof (LanguageResources));
            var values = Enum.GetValues(enumType);
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var items = values.Cast<object>().Select(value => new SelectListItem
                                                              {
                                                                  Selected = metadata.SimpleDisplayText == value.ToString(),
                                                                  Value = value.ToString(), 
                                                                  Text = GetResource(resourceManager, value.ToString())
                                                              });
         
            return html.DropDownListFor(expression, items);
        }

        public static MvcHtmlString LabelEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(TypeExceptionMessage, "enumType");
            }

            var resourceManager = new ResourceManager(typeof(LanguageResources));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return html.Label(GetResource(resourceManager, metadata.SimpleDisplayText));
        }

        private static string GetResource(ResourceManager resourceManager, string value)
        {
            string result;
            try
            {
                result = resourceManager.GetString(value) ?? value;
            }
            catch
            {
                Debug.WriteLine("Reource not found: " + value);
                result = value;
            }

            return result;
        }
    }
}