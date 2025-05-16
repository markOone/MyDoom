using System;
using System.Collections.Generic;

public static class Memoizer
{
	public class CacheEntity<TResult>
	{
		public TResult Value{get;}

		public DateTime LastUsed{get; set;}//LRU
		public int UseCount{get;set;}//LFU
		public DateTime CreatedAt{get; set;}//TIME-BASED

		public CacheEntity(TResult value)
		{
			Value = value;

			LastUsed = DateTime.Now;
			UseCount = 1; 
			CreatedAt = DateTime.Now;
		}
	}

	public class MemoizationOptions
	{
		public int Size {get; set;} = -1;//Unlimited
		public EvictionPolicy Policy {get; set;} = EvictionPolicy.Unlimited;
	}

	public enum EvictionPolicy{
		LRU,
		LFU,
		TimeBased,
		Unlimited
	}

	private static Dictionary<(float, float), CacheEntity<float>> MyCache = new();

	public static Func<float, float, float> Memoize(this Func<float, float, float> func, MemoizationOptions options = null)
		{
			var cache = MyCache;

			if(options == null) options = new MemoizationOptions();

			return new Func<float, float, float>((arg1, arg2) =>
			{
				var cacheKey = (arg1, arg2);

				if(cache.TryGetValue(cacheKey, out CacheEntity<float> value))
				{
					value.LastUsed = DateTime.Now;
                    value.UseCount++;
                    return value.Value;
				}

				float result = func(arg1, arg2);

				//Apply Eviction Policy
				if((cache.Count >= options.Size) && options.Size > 0)
				{
					ApplyEvictionPolicy(cache, options);
				}

				CacheEntity<float> newEntity = new CacheEntity<float>(result);
				MyCache[cacheKey] = newEntity;

				return result;
			});
		}

		public static void ApplyEvictionPolicy(Dictionary<(float, float), CacheEntity<float>> cache, MemoizationOptions options)
		{
			if(options.Policy == EvictionPolicy.Unlimited) return;
			if(options.Policy == EvictionPolicy.LRU)
			{
				KeyValuePair<(float, float), CacheEntity<float>> lastestUsed = default;
				DateTime oldestUsedTime = DateTime.MaxValue;

				foreach(var cacheObj in cache)
				{
					if(cacheObj.Value.LastUsed < oldestUsedTime)
					{
						lastestUsed = cacheObj;
						oldestUsedTime = cacheObj.Value.LastUsed;
					}
				}

				cache.Remove(lastestUsed.Key);
			}
			if(options.Policy == EvictionPolicy.LFU)
			{
				KeyValuePair<(float, float), CacheEntity<float>> leastUsed = default;
				int leastUsedAmount = int.MaxValue;

				foreach(var cacheObj in cache)
				{
					if(cacheObj.Value.UseCount < leastUsedAmount)
					{
						leastUsed = cacheObj;
						leastUsedAmount = cacheObj.Value.UseCount;
					}
				}

				cache.Remove(leastUsed.Key);
			}
			if(options.Policy == EvictionPolicy.TimeBased)
			{
				KeyValuePair<(float, float), CacheEntity<float>> oldestObj = default;
				DateTime oldestTime = DateTime.MaxValue;

				foreach(var cacheObj in cache)
				{
					if(cacheObj.Value.CreatedAt < oldestTime)
					{
						oldestObj = cacheObj;
						oldestTime = cacheObj.Value.CreatedAt;
					}
				}

				cache.Remove(oldestObj.Key);
			}
		}
}