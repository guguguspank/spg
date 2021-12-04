using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Spg
{
    public static class Config
    {
        /// <summary>
        /// ∂¡»°YAML≈‰÷√
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadYaml<T>(string path)
        {
            var yaml = File.ReadAllText(path);
            var deserializer = new DeserializerBuilder().Build();
            var conf = deserializer.Deserialize<T>(yaml);
            return conf;
        }

        public static void SaveYaml<T>(T data, string path)
        {
            var serializer = new SerializerBuilder().Build();
            var s = serializer.Serialize(RuntimeData.Instance.Conf);
            File.WriteAllText(path, s, System.Text.Encoding.UTF8);
        }
    }
}