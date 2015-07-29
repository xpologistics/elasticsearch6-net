using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nest
{
	internal static class Extensions
	{
		internal static INestSerializer Serializer = new NestSerializer(new ConnectionSettings());

		internal static string GetStringValue(this Enum enumValue)
		{
			var type = enumValue.GetType();
			var info = type.GetField(enumValue.ToString());
			var da = (EnumMemberAttribute[])(info.GetCustomAttributes(typeof(EnumMemberAttribute), false));

			if (da.Length > 0)
				return da[0].Value;
			else
				return string.Empty;
		}
		
		internal static readonly JsonConverter dateConverter = new IsoDateTimeConverter { Culture = CultureInfo.InvariantCulture };
		internal static readonly JsonSerializer serializer = new JsonSerializer();
		internal static string ToJsonNetString(this DateTime date)
		{
			using (var writer = new JTokenWriter())
			{
				dateConverter.WriteJson(writer, date, new JsonSerializer());
				return writer.Token.ToString();
			}
		}





		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
		{
			return items.GroupBy(property).Select(x => x.First());
		}

		public static T? ToEnum<T>(this string str) where T : struct
		{
			var enumType = typeof(T);
			foreach (var name in Enum.GetNames(enumType))
			{
				var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
				if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
			}
			//throw exception or whatever handling you want or
			return null;
		}
		internal static string Utf8String(this byte[] bytes)
		{
			return bytes == null ? null : Encoding.UTF8.GetString(bytes);
		}

		internal static byte[] Utf8Bytes(this string s)
		{
			return s.IsNullOrEmpty() ? null : Encoding.UTF8.GetBytes(s);
		}
		internal static string GetStringValue(this IEnumerable<Enum> enumValues)
		{
			return string.Join(",", enumValues.Select(e => e.GetStringValue()));
		}
		internal static bool JsonEquals(this string json, string otherjson)
		{
			var nJson = JObject.Parse(json).ToString();
			var nOtherJson = JObject.Parse(otherjson).ToString();
			return nJson == nOtherJson;
		}

		internal static bool IsNullOrEmpty(this TypeName value)
		{
			return value == null || value.GetHashCode() == 0;
		}


		internal static void ThrowIfNullOrEmpty(this string @object, string parameterName)
		{
			@object.ThrowIfNull(parameterName);
			if (string.IsNullOrWhiteSpace(@object))
				throw new ArgumentException("Argument can't be null or empty", parameterName);
		}
		internal static void ThrowIfEmpty<T>(this IEnumerable<T> @object, string parameterName)
		{
			@object.ThrowIfNull(parameterName);
			if (!@object.Any())
				throw new ArgumentException("Argument can not be an empty collection", parameterName);
		}

		internal static IList<T> EagerConcat<T>(this IEnumerable<T> list, IEnumerable<T> other)
		{
			list = list.HasAny() ? list : Enumerable.Empty<T>();
			var l = new List<T>(list);
			if (other.HasAny()) l.AddRange(other);
			return l;
		}
		internal static bool HasAny<T>(this IEnumerable<T> list, Func<T, bool> predicate)
		{
			return list != null && list.Any(predicate);
		}
		internal static bool HasAny<T>(this IEnumerable<T> list)
		{
			return list != null && list.Any();
		}

		internal static void ThrowIfNull<T>(this T value, string message = null)
		{
			if (value == null && message.IsNullOrEmpty()) throw new ArgumentNullException(nameof(value));
			else if (value == null) throw new ArgumentNullException(nameof(value), message);
		}

		internal static string F(this string format, params object[] args)
		{
			var c = CultureInfo.InvariantCulture;
			format.ThrowIfNull("format");
			return string.Format(c, format, args);
		}
		internal static string EscapedFormat(this string format, params object[] args)
		{
			format.ThrowIfNull("format");
			var arguments = new List<object>();
			foreach (var a in args)
			{
				var s = a as string;
				arguments.Add(s != null ? Uri.EscapeDataString(s) : a);
			}
			return string.Format(format, arguments.ToArray());
		}
		internal static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}


		internal static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
		{
			int idx = 0;
			foreach (T item in enumerable)
				handler(item, idx++);
		}
		
		//TODO also filter nulls

		internal static List<T> ToListOrNullIfEmpty<T>(this IEnumerable<T> xs, Func<T, bool> predicate) =>
			!xs.HasAny(predicate) ? null : xs.ToList();

		internal static List<T> ToListOrNullIfEmpty<T>(this IEnumerable<T> enumerable) =>
			enumerable.HasAny() ? enumerable.ToList() : null;

		internal static Dictionary<TKey, TValue> NullIfNoKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			var i = dictionary?.Count;
			return i.GetValueOrDefault(0) > 0 ? dictionary : null;
		}

		internal static IDictionary<TKey, TValue> NullIfNoKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			var i = dictionary?.Count;
			return i.GetValueOrDefault(0) > 0 ? dictionary : null;
		}


		internal static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> xs) => xs ?? new T[0];

	}



}
