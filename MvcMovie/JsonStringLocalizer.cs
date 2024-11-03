using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace MvcMovie
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public JsonStringLocalizer(IDistributedCache cache){ 
            _cache = cache;
        }
        public LocalizedString this[string name]
        {
            // it's loclaze string value Which will be read from json file by way method GetString 
            get
            { 
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }
        // it's same above but array of paramze , if you the use add specific value it's return with value  
        public LocalizedString this[string name, params object[] arguments]
        {
            get{
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value , arguments))
                    : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

            // read json file 
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(stream);
            using JsonTextReader reader = new JsonTextReader(streamReader);

            // return all value from json file 
            while (reader.Read()){
                if(reader.TokenType != JsonToken.PropertyName)
                    continue;
            
                var key = reader.Value as string;
                reader.Read();
                var value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value);       
            
            }
        
        }
                // create method adjectives sent key to method return default languages 
        private string GetString(string key) {
            // Resources/ar-EG.json  , Resources/en-US.json
            // how to konw defult culture => thread.Current
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

            // To make sure this culture language is correct so that you can send it to method GetValueFromJson
            var fullFilePath = Path.GetFullPath(filePath);

            // To check whether this file exists or not
            if(File.Exists(fullFilePath)){
                // locale_en-Us-welcome    || locale_ar-EG-welcome
                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                var cacheValue = _cache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(cacheValue))
                    return cacheValue;

                var result = GetValueFromJson(key, fullFilePath);

                if (!string.IsNullOrEmpty(result))
                    _cache.SetString(cacheKey, result);

                return result;

            }

            return string.Empty;
        }
        private string GetValueFromJson(string propertyName , string fileName)
        { 
            if(string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(fileName))
                return string.Empty;

            using FileStream stream = new FileStream(fileName , FileMode.Open , FileAccess.Read , FileShare.Read);
            using StreamReader streamReader = new StreamReader(stream);
            using JsonTextReader reader = new JsonTextReader(streamReader);

            while (reader.Read()) 
            { 
                if(reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                { 
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);
                }
			}

			return string.Empty;
        }
    }
}
