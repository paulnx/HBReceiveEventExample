using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
namespace HBReceiveEventExample
{

   public class CaseInSensitiveDictionary<TValue> : Dictionary<string, TValue>
	{
		public CaseInSensitiveDictionary() : base(StringComparer.InvariantCultureIgnoreCase) { }
	}
	public class HirebridgeEventContainer
	{
		public class Loginfo
		{
			public string actioncode { get; set; }
			public string actionname { get; set; }
		}
		public string companyid { get; set; }
		public string eventtype { get; set; }
		public string functionid { get; set; }
		public Loginfo loginfo { get; set; }
		public DateTime timestamputc { get; set; }
		public CaseInSensitiveDictionary <string> payload { get; set; }
		public DataTable payloadtable { get; set; }

		public string GetPayload(string key)
		{
			string keyValue = string.Empty;
			if (payload.ContainsKey(key))
				keyValue = payload[key];
			return keyValue;
		}
		public static HirebridgeEventContainer Create(string jsonData)
		{ 
			return JsonConvert.DeserializeObject<HirebridgeEventContainer>(jsonData);
		}
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}


	}
}