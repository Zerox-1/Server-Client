using System.Net.Sockets;
using System.Net;
using System.Text;

int ID = 1;
byte[] b = new byte[512];
int count = 0;
IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(ipPoint);
socket.Listen();
Console.WriteLine("Сервер запущен. Ожидание подключений...");
// получаем входящее подключение
using Socket client = await socket.AcceptAsync();
await client.SendAsync(Encoding.Default.GetBytes((ID++).ToString()));
// получаем адрес клиента
Console.WriteLine($"Адрес подключенного клиента: {client.RemoteEndPoint}");
try
{
    while (true)
    {
        client.Receive(b);
        string msg = Encoding.Default.GetString(b);
        Console.WriteLine();
        double[] items = new double[3];
        string temp = "";
        int j = 0;
        string id = "";
        for (int i = 0; i < msg.Length; i++)
        {
            if (msg[i].ToString() == ";")
            {
                items[j] = Convert.ToDouble(temp);
                temp = "";
                j++;
            }
            else if (msg[i].ToString() == ":")
            {
                items[j] = Convert.ToDouble(temp);
                id = msg.Substring(i++);
                j = 0;
                break;
            }
            else
            {
                temp += msg[i];
            }
        }
        double D = items[1] * items[1] - 4 * items[0] * items[2];
        if (D < 0)
        {
            temp = "Квадратное уравнение не имеет действительных решений!";
            Console.WriteLine(temp);
            client.Send(Encoding.UTF8.GetBytes(temp));
        }
        else if (D == 0)
        {
            double res = (-items[1] - Math.Sqrt(D)) / (2 * items[0]);
            Console.WriteLine("Результат запроса клиента с ID" + id);
            Console.WriteLine(res);
            client.Send(Encoding.UTF8.GetBytes(res.ToString()));
            count++;
            Console.WriteLine("Кол-во решенных задач:" + count);
        }
        else
        {
            double res = (-items[1] - Math.Sqrt(D)) / (2 * items[0]);
            double res1 = (-items[1] + Math.Sqrt(D)) / (2 * items[0]);
            Console.WriteLine("Результат запроса клиента с ID" + id);
            Console.WriteLine(res + ";" + res1);
            client.Send(Encoding.UTF8.GetBytes(res.ToString() + ";" + res1.ToString()));
            count++;
            Console.WriteLine("Кол-во решенных задач:" + count);
        }

    }
}
catch(System.Net.Sockets.SocketException e)
{
    Console.WriteLine("Клиент отключился");
}