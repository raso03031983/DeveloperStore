using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DeveloperStore.Sales.Domain.Events.Status
{
    public enum EventStatus
    {
        [Display(Name = "Sale Create")]
        Sale_Create = 1,

        [Display(Name = "Sale Update")]
        Sale_Update = 2,

        [Display(Name = "Sale Cancel")]
        Sale_Cancel = 3,

        [Display(Name = "Sale Item Update")]
        Sale_Item_Update = 4
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? enumValue.ToString();
        }
    }
}
