using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;


namespace piliplugin
{
    using Newtonsoft.Json.Linq;

    public class Mailssage
    {
        private readonly string account = null;
        private readonly string password = null;

        private readonly string host = null;
        private readonly int port = 0;

        private readonly List<string> to = null;

        /// <summary>
        /// 工厂构造
        /// </summary>
        /// <param name="path">Mailssage.json的绝对路径</param>
        public Mailssage(string path)
        {
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(path, System.Text.Encoding.UTF8);
            }
            catch//找不到配置文件则创建
            {
                FileStream fileStream = new(path, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("{}");
                streamWriter.Close();
                fileStream.Close();

                jsonString = File.ReadAllText(path, System.Text.Encoding.UTF8);
            }
        }

        public void SendMessage(string msg)
        {
            var client = new SmtpClient
            {
                Host = host,
                Port = port,
                UseDefaultCredentials = false,//不使用凭据
                Credentials = new System.Net.NetworkCredential(account, password)
            };

            foreach (var dest in to)
            {
                client.Send("Mailssage Plugin", dest, "新的噼里啪啦通知", "HelloWorld!");
            }
        }
    }
}
