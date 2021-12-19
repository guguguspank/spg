using System.IO;
using YamlDotNet.Serialization;

namespace Spg
{
    public static class Config
    {
        /// <summary>
        /// 从文件简单读取YAML配置
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

        /// <summary>
        /// 从字符流读取YAML配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T LoadYamlStream<T>(string content)
        {
            var deserializer = new DeserializerBuilder().Build();
            var conf = deserializer.Deserialize<T>(content);
            return conf;
        }

        /// <summary>
        /// 简单存储对象到yaml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SaveYaml<T>(T data, string path)
        {
            var serializer = new SerializerBuilder().Build();
            var s = serializer.Serialize(data);
            File.WriteAllText(path, s, System.Text.Encoding.UTF8);
        }
    }
}