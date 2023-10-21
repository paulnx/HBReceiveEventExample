using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

      CaseInSensitiveDictionary<string> m_payload;

      public HirebridgeEventContainer()
      {
         payloadordered = new OrderedDictionary();


      }

      [JsonIgnore]
      public CaseInSensitiveDictionary<string> payload
      {
         get
         {
            if (m_payload == null)
            {
               m_payload = new CaseInSensitiveDictionary<string>();
               foreach (string key in payloadordered.Keys)
               {
                  m_payload[key] = Convert.ToString(payloadordered[key]);
               }
            }
            return m_payload;
         }

         set { m_payload = value; }
      }


      [JsonProperty("payload")]
      public OrderedDictionary payloadordered
      {
         get;
         set;

      }
      public DataTable payloadtable { get; set; }

      public string this[string keyname]
      {
         get => GetPayload(keyname);
         set => payloadordered[keyname] = value;
      }

      public string GetPayload(string key)
      {
         string keyValue = string.Empty;
         if (payload.ContainsKey(key))
            keyValue = payload[key];
         return keyValue;
      }
      public static HirebridgeEventContainer Create(string jsonData)
      {
         JsonSerializerSettings settings = new JsonSerializerSettings();
         settings.DateParseHandling = DateParseHandling.None;
         return JsonConvert.DeserializeObject<HirebridgeEventContainer>(jsonData, settings);
      }


      public string ToJson()
      {
         return JsonConvert.SerializeObject(this);
      }

   }
}