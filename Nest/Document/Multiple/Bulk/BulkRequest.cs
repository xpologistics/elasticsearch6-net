﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	[JsonConverter(typeof(BulkRequestJsonConverter))]
	public partial interface IBulkRequest
	{
		[JsonIgnore]
		IList<IBulkOperation> Operations { get; set; }
	}

	public partial class BulkRequest
	{
		public IList<IBulkOperation> Operations { get; set; }
	}

	public partial class BulkDescriptor
	{
		IList<IBulkOperation> IBulkRequest.Operations { get; set; } = new SynchronizedCollection<IBulkOperation>();

		public BulkDescriptor AddOperation(IBulkOperation operation) => Assign(operation, (a, v) => a.Operations.AddIfNotNull(v));

		public BulkDescriptor Create<T>(Func<BulkCreateDescriptor<T>, IBulkCreateOperation<T>> bulkCreateSelector) where T : class =>
			AddOperation(bulkCreateSelector?.Invoke(new BulkCreateDescriptor<T>()));

		/// <summary>
		/// CreateMany, convenience method to create many documents at once.
		/// </summary>
		/// <param name="objects">the objects to create</param>
		/// <param name="bulkCreateSelector">A func called on each object to describe the individual create operation</param>
		public BulkDescriptor CreateMany<T>(IEnumerable<T> @objects,
			Func<BulkCreateDescriptor<T>, T, IBulkCreateOperation<T>> bulkCreateSelector = null
		) where T : class
		{
			foreach (var item in @objects)
				AddOperation(bulkCreateSelector.InvokeOrDefault(new BulkCreateDescriptor<T>().Document(item), item));

			return this;
		}

		public BulkDescriptor Index<T>(Func<BulkIndexDescriptor<T>, IBulkIndexOperation<T>> bulkIndexSelector) where T : class =>
			AddOperation(bulkIndexSelector?.Invoke(new BulkIndexDescriptor<T>()));

		/// <summary>
		/// IndexMany, convenience method to pass many objects at once.
		/// </summary>
		/// <param name="objects">the objects to index</param>
		/// <param name="bulkIndexSelector">A func called on each object to describe the individual index operation</param>
		public BulkDescriptor IndexMany<T>(IEnumerable<T> @objects, Func<BulkIndexDescriptor<T>, T, IBulkIndexOperation<T>> bulkIndexSelector = null)
			where T : class
		{
			foreach (var item in @objects)
				AddOperation(bulkIndexSelector.InvokeOrDefault(new BulkIndexDescriptor<T>().Document(item), item));

			return this;
		}

		public BulkDescriptor Delete<T>(T obj, Func<BulkDeleteDescriptor<T>, IBulkDeleteOperation<T>> bulkDeleteSelector = null) where T : class =>
			AddOperation(bulkDeleteSelector.InvokeOrDefault(new BulkDeleteDescriptor<T>().Document(obj)));

		public BulkDescriptor Delete<T>(Func<BulkDeleteDescriptor<T>, IBulkDeleteOperation<T>> bulkDeleteSelector) where T : class =>
			AddOperation(bulkDeleteSelector?.Invoke(new BulkDeleteDescriptor<T>()));

		/// <summary>
		/// DeleteMany, convenience method to delete many objects at once.
		/// </summary>
		/// <param name="objects">the objects to delete</param>
		/// <param name="bulkDeleteSelector">A func called on each object to describe the individual delete operation</param>
		public BulkDescriptor DeleteMany<T>(IEnumerable<T> @objects,
			Func<BulkDeleteDescriptor<T>, T, IBulkDeleteOperation<T>> bulkDeleteSelector = null
		) where T : class
		{
			foreach (var item in @objects)
				AddOperation(bulkDeleteSelector.InvokeOrDefault(new BulkDeleteDescriptor<T>().Document(item), item));

			return this;
		}

		/// <summary>
		/// DeleteMany, convenience method to delete many objects at once.
		/// </summary>
		/// <param name="ids">Enumerable of string ids to delete</param>
		/// <param name="bulkDeleteSelector">A func called on each ids to describe the individual delete operation</param>
		public BulkDescriptor DeleteMany<T>(IEnumerable<string> ids,
			Func<BulkDeleteDescriptor<T>, string, IBulkDeleteOperation<T>> bulkDeleteSelector = null
		) where T : class
		{
			foreach (var item in ids)
				AddOperation(bulkDeleteSelector.InvokeOrDefault(new BulkDeleteDescriptor<T>().Id(item), item));

			return this;
		}

		/// <summary>
		/// DeleteMany, convenience method to delete many objects at once.
		/// </summary>
		/// <param name="ids">Enumerable of int ids to delete</param>
		/// <param name="bulkDeleteSelector">A func called on each ids to describe the individual delete operation</param>
		public BulkDescriptor DeleteMany<T>(IEnumerable<long> ids,
			Func<BulkDeleteDescriptor<T>, long, IBulkDeleteOperation<T>> bulkDeleteSelector = null
		) where T : class
		{
			foreach (var item in ids)
				AddOperation(bulkDeleteSelector.InvokeOrDefault(new BulkDeleteDescriptor<T>().Id(item), item));

			return this;
		}

		/// <summary>
		/// Updatemany, convenience method to pass many objects at once to do multiple updates.
		/// </summary>
		/// <param name="objects">the objects to update</param>
		/// <param name="bulkUpdateSelector">An func called on each object to describe the individual update operation</param>
		public BulkDescriptor UpdateMany<T>(IEnumerable<T> @objects,
			Func<BulkUpdateDescriptor<T, T>, T, IBulkUpdateOperation<T, T>> bulkUpdateSelector
		) where T : class
		{
			foreach (var item in @objects)
				AddOperation(bulkUpdateSelector.InvokeOrDefault(new BulkUpdateDescriptor<T, T>().IdFrom(item), item));

			return this;
		}

		/// <summary>
		/// Update many, convenience method to pass many objects at once to do multiple updates.
		/// </summary>
		/// <param name="objects">the objects to update</param>
		/// <param name="bulkUpdateSelector">An func called on each object to describe the individual update operation</param>
		public BulkDescriptor UpdateMany<T, TPartialDocument>(IEnumerable<T> @objects,
			Func<BulkUpdateDescriptor<T, TPartialDocument>, T, IBulkUpdateOperation<T, TPartialDocument>> bulkUpdateSelector
		) where T : class
		  where TPartialDocument : class
		{
			foreach (var item in @objects)
				AddOperation(bulkUpdateSelector.InvokeOrDefault(new BulkUpdateDescriptor<T, TPartialDocument>().IdFrom(item), item));

			return this;
		}

		public BulkDescriptor Update<T>(Func<BulkUpdateDescriptor<T, T>, IBulkUpdateOperation<T, T>> bulkUpdateSelector) where T : class =>
			Update<T, T>(bulkUpdateSelector);

		public BulkDescriptor Update<T, TPartialDocument>(
			Func<BulkUpdateDescriptor<T, TPartialDocument>, IBulkUpdateOperation<T, TPartialDocument>> bulkUpdateSelector
		) where T : class
		  where TPartialDocument : class => AddOperation(bulkUpdateSelector?.Invoke(new BulkUpdateDescriptor<T, TPartialDocument>()));
	}
}