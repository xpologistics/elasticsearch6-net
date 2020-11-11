﻿using System;
using System.Diagnostics;
using Elasticsearch6.Net;

namespace Nest6
{
	[ContractJsonConverter(typeof(RelationNameJsonConverter))]
	[DebuggerDisplay("{DebugDisplay,nq}")]
	public class RelationName : IEquatable<RelationName>, IUrlParameter
	{
		private RelationName(string type) => Name = type;

		private RelationName(Type type) => Type = type;

		public string Name { get; }
		public Type Type { get; }

		internal string DebugDisplay => Type == null ? Name : $"{nameof(RelationName)} for typeof: {Type?.Name}";

		private static int TypeHashCode { get; } = typeof(RelationName).GetHashCode();

		public bool Equals(RelationName other) => EqualsMarker(other);

		string IUrlParameter.GetString(IConnectionConfigurationValues settings)
		{
			if (!(settings is IConnectionSettingsValues nestSettings))
				throw new ArgumentNullException(nameof(settings),
					$"Can not resolve {nameof(RelationName)} if no {nameof(IConnectionSettingsValues)} is provided");

			return nestSettings.Inferrer.RelationName(this);
		}

		public static RelationName From<T>() => typeof(T);

		public static RelationName Create(Type type) => GetRelationNameForType(type);

		public static RelationName Create<T>() where T : class => GetRelationNameForType(typeof(T));

		private static RelationName GetRelationNameForType(Type type) => new RelationName(type);

		public static implicit operator RelationName(string typeName) => typeName.IsNullOrEmpty() ? null : new RelationName(typeName);

		public static implicit operator RelationName(Type type) => type == null ? null : new RelationName(type);

		public override int GetHashCode()
		{
			unchecked
			{
				var result = TypeHashCode;
				result = (result * 397) ^ (Name?.GetHashCode() ?? Type?.GetHashCode() ?? 0);
				return result;
			}
		}

		public static bool operator ==(RelationName left, RelationName right) => Equals(left, right);

		public static bool operator !=(RelationName left, RelationName right) => !Equals(left, right);

		public override bool Equals(object obj) =>
			obj is string s ? EqualsString(s) : obj is RelationName r && EqualsMarker(r);

		public bool EqualsMarker(RelationName other)
		{
			if (!Name.IsNullOrEmpty() && other != null && !other.Name.IsNullOrEmpty())
				return EqualsString(other.Name);
			if (Type != null && other?.Type != null)
				return Type == other.Type;

			return false;
		}

		//already private in master, breaking change to change in 6.x
		public bool EqualsString(string other) => !other.IsNullOrEmpty() && other == Name;

		public override string ToString()
		{
			if (!Name.IsNullOrEmpty()) return Name;

			return Type != null ? Type.Name : string.Empty;
		}
	}
}
