﻿using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace E_commerceTechnologyWebsite.KhoLuuTru
{
	public static class SessionExtensions
	{
		public static void SetJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T GetJson<T>(this ISession session, string key)
		{
			var sessionData = session.GetString(key);
			return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
		}
	}
}