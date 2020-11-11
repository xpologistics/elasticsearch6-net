﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Elasticsearch6.Net;

namespace Nest6
{
	[DebuggerDisplay("{DebugDisplay,nq}")]
	public class Names : IEquatable<Names>, IUrlParameter
	{
		public Names(IEnumerable<string> names) : this(names?.Select(n => (Name)n).ToList()) { }

		public Names(IEnumerable<Name> names)
		{
			Value = names?.ToList();
			if (!Value.HasAny())
				throw new ArgumentException($"can not create {nameof(Names)} on an empty enumerable of ", nameof(names));
		}

		internal IList<Name> Value { get; }

		private string DebugDisplay => ((IUrlParameter)this).GetString(null);

		public bool Equals(Names other) => EqualsAllIds(Value, other.Value);

		string IUrlParameter.GetString(IConnectionConfigurationValues settings) =>
			string.Join(",", Value.Cast<IUrlParameter>().Select(n => n.GetString(settings)));

		public static Names Parse(string names) => names.IsNullOrEmptyCommaSeparatedList(out var list) ? null : new Names(list);

		public static implicit operator Names(Name name) => name == null ? null : new Names(new[] { name });

		public static implicit operator Names(string names) => Parse(names);

		public static implicit operator Names(string[] names) => names.IsEmpty() ? null : new Names(names);

		public static bool operator ==(Names left, Names right) => Equals(left, right);

		public static bool operator !=(Names left, Names right) => !Equals(left, right);

		private static bool EqualsAllIds(ICollection<Name> thisIds, ICollection<Name> otherIds)
		{
			if (thisIds == null && otherIds == null) return true;
			if (thisIds == null || otherIds == null) return false;
			if (thisIds.Count != otherIds.Count) return false;

			return thisIds.Count == otherIds.Count && !thisIds.Except(otherIds).Any();
		}

		public override bool Equals(object obj) => obj is string s ? Equals(Parse(s)) : obj is Names i && Equals(i);

		public override int GetHashCode() => Value.GetHashCode();
	}
}
