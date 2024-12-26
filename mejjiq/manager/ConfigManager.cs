using System;
using System.Collections.Generic;
using System.IO;

namespace dusk.mejjiq.manager
{
    public class ConfigManager
    {
        private Dictionary<string, Dictionary<string, string>> _configData;

        public ConfigManager(string filePath)
        {
            _configData = new Dictionary<string, Dictionary<string, string>>();
            LoadConfig(filePath);
        }

        // Load the config file and parse it
        private void LoadConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // If the config file doesn't exist, create a new one with default values
                CreateDefaultConfig(filePath);
                Console.WriteLine($"Config file not found. A new one has been created at {filePath}");
            }

            string currentSection = null;
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue; // Ignore empty lines or comments

                if (trimmedLine.StartsWith('[') && trimmedLine.EndsWith(']'))
                {
                    // Section header (e.g., [Graphics], [Misc])
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    _configData[currentSection] = new Dictionary<string, string>();
                }
                else if (currentSection != null && trimmedLine.Contains('='))
                {
                    // Key-value pair (e.g., ResolutionWidth=1920)
                    var parts = trimmedLine.Split(['='], 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        _configData[currentSection][key] = value;
                    }
                }
            }
        }

        // Method to create a default config file
        private void CreateDefaultConfig(string filePath)
        {
            var defaultConfig = new Dictionary<string, Dictionary<string, string>>
            {
                { "Graphics", new Dictionary<string, string>
                    {
                        { "ResolutionWidth", "1920" },
                        { "ResolutionHeight", "1080" },
                        { "Fullscreen", "false" }
                    }
                },
                { "Misc", new Dictionary<string, string>
                    {
                        //TODO: empty for now
                    }
                }
            };

            // Write the default config to the file
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var section in defaultConfig)
                {
                    writer.WriteLine($"[{section.Key}]");
                    foreach (var kvp in section.Value)
                    {
                        writer.WriteLine($"{kvp.Key}={kvp.Value}");
                    }
                    writer.WriteLine();
                }
            }
        }

        // Get value for a given section and key
        public string GetValue(string section, string key)
        {
            if (_configData.ContainsKey(section) && _configData[section].ContainsKey(key))
            {
                return _configData[section][key];
            }
            throw new KeyNotFoundException($"Key '{key}' not found in section '{section}'");
        }

        // Optionally, you can add a method to get integer or boolean values directly
        public int GetIntValue(string section, string key)
        {
            return int.Parse(GetValue(section, key));
        }

        public bool GetBoolValue(string section, string key)
        {
            return bool.Parse(GetValue(section, key));
        }
    }
}