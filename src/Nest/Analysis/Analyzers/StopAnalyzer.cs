﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	/// <summary>
	/// An analyzer of type stop that is built using a Lower Case Tokenizer, with Stop Token Filter.
	/// </summary>
	public interface IStopAnalyzer : IAnalyzer
	{
		/// <summary>
		/// A list of stopword to initialize the stop filter with. Defaults to the english stop words.
		/// </summary>
		[JsonProperty("stopwords")]
		[JsonConverter(typeof(StopWordsJsonConverter))]
		StopWords StopWords { get; set; }

		/// <summary>
		/// A path (either relative to config location, or absolute) to a stopwords file configuration.
		/// </summary>
		[JsonProperty("stopwords_path")]
		string StopwordsPath { get; set; }
	}

	/// <inheritdoc />
	public class StopAnalyzer : AnalyzerBase, IStopAnalyzer
	{
		public StopAnalyzer() : base("stop") { }

		/// <inheritdoc />
		public StopWords StopWords { get; set; }

		/// <inheritdoc />
		public string StopwordsPath { get; set; }
	}

	/// <inheritdoc />
	public class StopAnalyzerDescriptor : AnalyzerDescriptorBase<StopAnalyzerDescriptor, IStopAnalyzer>, IStopAnalyzer
	{
		protected override string Type => "stop";

		StopWords IStopAnalyzer.StopWords { get; set; }
		string IStopAnalyzer.StopwordsPath { get; set; }

		public StopAnalyzerDescriptor StopWords(params string[] stopWords) => Assign(stopWords, (a, v) => a.StopWords = v);

		public StopAnalyzerDescriptor StopWords(IEnumerable<string> stopWords) =>
			Assign(stopWords.ToListOrNullIfEmpty(), (a, v) => a.StopWords = v);

		public StopAnalyzerDescriptor StopWords(StopWords stopWords) => Assign(stopWords, (a, v) => a.StopWords = v);

		public StopAnalyzerDescriptor StopwordsPath(string path) => Assign(path, (a, v) => a.StopwordsPath = v);
	}
}
