using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;

namespace Server
{
    //класс для работы с почтовыми индексами по API  с http запросами
    internal class Index_worker
    {
        public string data; // наш индекс
        WebRequest request;

        //берем информацию с готового API
        public Index_worker(string code)
        {
            request = HttpWebRequest.Create($" http://basicdata.ru/api/json/zipcode/{code}/");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Unicode, true);
            string res = reader.ReadToEnd();
            byte[] bytes = Encoding.Unicode.GetBytes(res);
            byte[] b = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
            data = Encoding.Default.GetString(b);
            reader.Close();
            response.Close();

        }

    }
}
