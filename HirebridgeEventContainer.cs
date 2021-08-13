using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace HBReceiveEventExample
{
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
		public Dictionary<string, string> payload { get; set; }
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

		/*Sample of Incoming Json

  {
	 "companyid": "9909",
	 "eventtype": "Hire",
	 "functionid": null,
	 "loginfo": {
		"actioncode": "60",
		"actionname": "CandidateHired"
	 },
	 "timestamputc": "2018-11-20T16:48:00.7422261Z",
	 "payload": {		
		"regularpayday": "Thursday",
		"divisionid": "522",
		"firstname": "Paul",
		"lastname": "Chew",
		"phone1": "5615551212",
		"email": "pap@mailinator.com",
		"capplicantid": "14710265",
		"addr1": "1312",
		"addr2": "324234",
		"city": "Melrose",
		"state": "MA",
		"zipcode": "33434",
		"payfrequency": "1",
		"tbd-hiredate": "11/15/2018 12:00:00 AM",
		"tbd-startdate": "11/29/2018 12:00:00 AM",
		"workstate": "TN",
		"locationcode": "codenashville",
		"jobcode": "EquifaxJobCode1NeedValue",
		"templatecode": "4",
		"jobposition": "Sample Job",
		"tbd-dob": "11/29/2018",
		"tbd-empstatus": "True",
		"tbd-salary": "565656",
		"tbd-hourly": ""
	 }
  }       


		 */

	}
}