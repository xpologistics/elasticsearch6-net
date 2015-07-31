﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Net.Connection
{
	//TODO make sure we attach as much information from this pipeline to unrecoverable exceptions
	public class ElasticsearchException : Exception
	{
		public PipelineFailure Cause { get; }
		public IElasticsearchResponse Response { get; }
		public bool Recoverable => Cause == PipelineFailure.BadResponse || Cause == PipelineFailure.Unexpected || Cause == PipelineFailure.BadPing;

		//TODO exception messages
		public ElasticsearchException(PipelineFailure cause, Exception innerException) : base("", innerException)
		{
			this.Cause = cause;
		}

		public ElasticsearchException(PipelineFailure cause, IElasticsearchResponse response)
		{
			this.Cause = cause;
			this.Response = response;
		}
	}

}
