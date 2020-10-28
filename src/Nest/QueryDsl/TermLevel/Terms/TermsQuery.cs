﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest6
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(TermsQueryJsonConverter))]
	public interface ITermsQuery : IFieldNameQuery
	{
		IEnumerable<object> Terms { get; set; }
		IFieldLookup TermsLookup { get; set; }
	}

	public class TermsQuery : FieldNameQueryBase, ITermsQuery
	{
		public IEnumerable<object> Terms { get; set; }
		public IFieldLookup TermsLookup { get; set; }
		protected override bool Conditionless => IsConditionless(this);

		internal override void InternalWrapInContainer(IQueryContainer c) => c.Terms = this;

		internal static bool IsConditionless(ITermsQuery q) => q.Field.IsConditionless()
			|| (q.Terms == null
				|| !q.Terms.HasAny()
				|| q.Terms.All(t => t == null
					|| ((t as string)?.IsNullOrEmpty()).GetValueOrDefault(false))
			)
			&&
			(q.TermsLookup == null
				|| q.TermsLookup.Id == null
				|| q.TermsLookup.Path.IsConditionless()
				|| q.TermsLookup.Index == null
				|| q.TermsLookup.Type == null
			);
	}

	/// <summary>
	/// A query that match on any (configurable) of the provided terms.
	/// This is a simpler syntax query for using a bool query with several term queries in the should clauses.
	/// </summary>
	/// <typeparam name="T">The type that represents the expected hit type</typeparam>
	public class TermsQueryDescriptor<T>
		: FieldNameQueryDescriptorBase<TermsQueryDescriptor<T>, ITermsQuery, T>
			, ITermsQuery where T : class
	{
		protected override bool Conditionless => TermsQuery.IsConditionless(this);
		IEnumerable<object> ITermsQuery.Terms { get; set; }
		IFieldLookup ITermsQuery.TermsLookup { get; set; }

		public TermsQueryDescriptor<T> TermsLookup<TOther>(Func<FieldLookupDescriptor<TOther>, IFieldLookup> selector)
			where TOther : class => Assign(selector(new FieldLookupDescriptor<TOther>()), (a, v) => a.TermsLookup = v);

		public TermsQueryDescriptor<T> Terms<TValue>(IEnumerable<TValue> terms) => Assign(terms?.Cast<object>(), (a, v) => a.Terms = v);

		public TermsQueryDescriptor<T> Terms<TValue>(params TValue[] terms) => Assign(terms, (a, v) =>
		{
			if (v?.Length == 1 && typeof(IEnumerable).IsAssignableFrom(typeof(TValue)) && typeof(TValue) != typeof(string))
				a.Terms = (v.First() as IEnumerable)?.Cast<object>();
			else a.Terms = v?.Cast<object>();
		});
	}
}
