﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	[ContractJsonConverter(typeof(SimulatedActionsConverter))]
	public class SimulatedActions
	{
		private SimulatedActions() { }

		public IEnumerable<string> Actions { get; private set; }

		public static SimulatedActions All => new SimulatedActions { UseAll = true };
		public bool UseAll { get; private set; }

		public static SimulatedActions Some(params string[] actions) => new SimulatedActions { Actions = actions };

		public static SimulatedActions Some(IEnumerable<string> actions) => new SimulatedActions { Actions = actions };
	}

	internal class SimulatedActionsConverter : JsonConverter
	{
		public override bool CanRead => true;

		public override bool CanWrite => true;

		public override bool CanConvert(Type objectType) => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String) return SimulatedActions.All;

			return SimulatedActions.Some(serializer.Deserialize<List<string>>(reader));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var s = value as SimulatedActions;
			if (s == null) return;

			if (s.UseAll) writer.WriteValue("_all");
			else serializer.Serialize(writer, s.Actions);
		}
	}
}
