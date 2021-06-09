using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Focus.ForDesktop
{
    internal class DesktopRegistry
    {
        private const string KeyName = @"Software\Microsoft\Windows\Shell\Bags\1\Desktop";

        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        public IDictionary<string, string> GetRegistryValues()
        {
            using (var registry = Registry.CurrentUser.OpenSubKey(KeyName))
            {
                return registry.GetValueNames().ToDictionary(n => n, n => GetValue(registry, n));
            }
        }

        private string GetValue(RegistryKey registry, string valueName)
        {
            var value = registry.GetValue(valueName);
            if (value == null)
            { return string.Empty; }

            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, value);

                var bytes = stream.ToArray();

                return Convert.ToBase64String(bytes);
            }
        }

        public void SetRegistryValues(IDictionary<string, string> values)
        {
            using (var registry = Registry.CurrentUser.OpenSubKey(KeyName, true))
            {
                foreach (var item in values)
                {
                    registry.SetValue(item.Key, GetValue(item.Value));
                }
            }
        }

        private object GetValue(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            { return null; }

            var bytes = Convert.FromBase64String(stringValue);

            using (var stream = new MemoryStream(bytes))
            {
                return _formatter.Deserialize(stream);
            }
        }
    }

}
