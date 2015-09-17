using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kahia.Common.Extensions.ConversionExtensions;
using Kahia.Common.Extensions.StringExtensions;

namespace FormsAuthTest.Application.Helpers
{
    public static class AppSettingHelper
    {
        public static String Get(String key, String exceptionMessage, params object[] parameters)
        {
            var value = Get(key);
            if (value.IsNullOrEmptyString())
            {
                //Insert {0} as keyName
                parameters = InsertKeyNameParameter(parameters, key);
                throw new Exception(exceptionMessage.FormatString(parameters));
            }
            return value;
        }

        public static Boolean GetBool(String key, String exceptionMessage, params object[] parameters)
        {
            var value = Get(key).ToStringByDefaultValue().ToNullableBoolean();
            if (value == null)
            {
                //Insert {0} as keyName
                parameters = InsertKeyNameParameter(parameters, key);
                throw new Exception(exceptionMessage.FormatString(parameters));
            }
            return value.Value;
        }

        public static String Get(String key) { return ConfigurationManager.AppSettings[key]; }

        private static object[] InsertKeyNameParameter(object[] parameters, String key)
        {
            parameters = parameters ?? new object[0];
            var paramList = parameters.ToList();
            paramList.Insert(0, key);
            parameters = paramList.ToArray();
            return parameters;
        }

        /// <summary>
        /// Bu metod setting'i bulamadığında exception fırlatmaz, default değer döner!
        /// </summary>
        /// <param name="key"></param>
        /// <param name="extraMessage"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static String GetOrDefault(String key, String extraMessage, String defaultValue)
        {
            var result = ConfigurationManager.AppSettings[key];
            if (result.IsNullOrEmptyString())
                return defaultValue;
            return result;
        }

        public static bool? GetNullableBool(String key, String extraMessage)
        {
            return Get(key, extraMessage).ToNullableBoolean();
        }
    }
}
