using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vnn88.Common.Infrastructure
{
    public static class EnumHelper
    {
        public static SelectList ToSelectList<T>(short? selected) where T : struct
        {
            return selected == null ? ToSelectList<T>() : ToSelectList<T>(selected.Value);
        }
        public static SelectList ToSelectList<T>() where T : struct
        {
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var values = Enum.GetValues(typeof(T)).Cast<Enum>().Select(e => new
                {
                    Id = Convert.ToInt32(e),
                    Name = e.GetDescription()
                });
                return new SelectList(values, nameof(EnumModel.Id), nameof(EnumModel.Name));
            }
            return null;
        }

        public static IEnumerable<SelectListItem> ToStringSelectList<T>() where T : struct
        {
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var values = Enum.GetValues(typeof(T)).Cast<Enum>().Select(e => new
                {
                    Id = e.GetName(),
                    Name = e.GetDescription()
                });
                return new SelectList(values, nameof(EnumModel.Id), nameof(EnumModel.Name));
            }
            return null;
        }
        public static SelectList ToSelectList<T>(short selected) where T : struct
        {
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var values = Enum.GetValues(t).Cast<Enum>().Select(e => new
                {
                    Id = Convert.ToInt32(e),
                    Name = e.GetDescription()
                });
                return new SelectList(values, nameof(EnumModel.Id), nameof(EnumModel.Name),selected);
            }
            return null;
        }

        public static List<EnumModel> ToEnumList<T>()
        {
            List<EnumModel> enumModels = new List<EnumModel>();
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var values = Enum.GetValues(t).Cast<Enum>().Select(e => new
                {
                    Id = Convert.ToInt32(e),
                    Name = e.GetDescription()
                });

                foreach (var item in values)
                {
                    enumModels.Add(new EnumModel { Id = item.Id, Name = item.Name });
                }
            }
            return enumModels;
        }

        public static List<dynamic> ToEnumListText<T>()
        {
            List<dynamic> enumModels = new List<dynamic>();
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var values = Enum.GetValues(t).Cast<Enum>().Select(e => new
                {
                    Id = e.GetDescription(),
                    Name = e.GetDescription()
                });

                foreach (var item in values)
                {
                    enumModels.Add(new {item.Id, item.Name });
                }
            }
            return enumModels;
        }

        public static string GetDescription<TEnum>(this TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }
            return value.ToString();
        }
        public static string GetName<TEnum>(this TEnum value)
        {
            return value.GetType().GetMember(value.ToString()).First().Name;
        }
        public static string GetDisplayName<TEnum>(this TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DisplayAttribute attribute = (DisplayAttribute)fi.GetCustomAttribute(typeof(DisplayAttribute), false);
                return attribute.Name ?? string.Empty;
            }
            return value.ToString();
        }
        public static bool IsEnum<T>(int selected) where T : struct
        {
            SelectList list = ToSelectList<T>();
            foreach (var item in list)
            {
                if (Convert.ToInt32(item.Value) == selected) { return true; }
            }
            return false;
        }
        public static bool IsEnum<T>(string multiSelected) where T : struct
        {
            if (string.IsNullOrWhiteSpace(multiSelected)) { return false; }
            if (multiSelected.IndexOf(" ", StringComparison.Ordinal) > -1) { return false; }

            char[] delimiterChars = { ',' };
            int[] selecteds = multiSelected.Split(delimiterChars).Select(x => Convert.ToInt32(x)).ToArray();
            int nSelecteds = selecteds.Distinct().Count();
            if (nSelecteds == selecteds.Length) { return false; }

            foreach (var item in selecteds)
            {
                if (!IsEnum<T>(item)) { return false; }
            }
            return true;
        }
    }
    public class EnumModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AutoComplete
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
}
