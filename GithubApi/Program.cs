using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using Newtonsoft;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

namespace Git
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = "X-rus";
            string repo = "xnet";

            //Десериализация ответа сервера
            List<Tag> tags = JsonConvert.DeserializeObject<List<Tag>>(GetTags(user, repo));

            //Сериализация в XML
            XmlSerializer XML_serializer = new XmlSerializer(typeof(List<Tag>));
            using (StreamWriter fs = new StreamWriter("tags.xml", false))
            {
                XML_serializer.Serialize(fs, tags);
            }

            //Сериализация в JSON
            using (StreamWriter str = new StreamWriter("tags.json", false))
            {
                str.Write(JsonConvert.SerializeObject(tags));
            }

            Console.WriteLine("Completed..");
            Console.ReadKey();
        }

        //Отправка запроса
        public static string GetTags(string owner, string repo)
        {
            string url = string.Format("https://api.github.com/repos/{0}/{1}/tags", owner, repo);
            using (HttpRequest req = new HttpRequest())
            {
                req.AddHeader("Accept", "application/vnd.github.v3+json");
                req.UserAgent = "Testing github API";
                var res = req.Get(url);
                return res.ToString();
            }
        }
    }

    [Serializable]
    public class Commit
    {
        public Commit() { }
        public string sha { get; set; }
        public string url { get; set; }
    }

    [Serializable]
    public class Tag
    {
        public Tag() { }
        public string name { get; set; }
        public string zipball_url { get; set; }
        public string tarball_url { get; set; }
        public Commit commit { get; set; }
    }

}
