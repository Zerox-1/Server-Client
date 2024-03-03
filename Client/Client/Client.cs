using System.Net.Sockets;
using System.Text;

using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
int ID;
try
{
    await socket.ConnectAsync("127.0.0.1", 8888);
    var responseBytes = new byte[512];
    var bytes = await socket.ReceiveAsync(responseBytes);
    string items = "";
    string temp;
    string response;
    Console.WriteLine($"Подключение к {socket.RemoteEndPoint} установлено");
    ID = Convert.ToInt16(Encoding.UTF8.GetString(responseBytes, 0, bytes));
    while (true)
    {
        Console.WriteLine("Введите коэфиценты квадратного уравнения:");
        Console.Write("a:");
        temp = Convert.ToString(Console.ReadLine());
        while (!double.TryParse(temp, out var parsedNumber))
        {
            Console.WriteLine("Коэфицент должен быть числом!");
            temp = Convert.ToString(Console.ReadLine());
        }
        items += temp + ";";
        Console.Write("b:");
        temp = Convert.ToString(Console.ReadLine());
        while (!double.TryParse(temp, out var parsedNumber))
        {
            Console.WriteLine("Коэфицент должен быть числом!");
            temp = Convert.ToString(Console.ReadLine());
        }
        items += temp + ";";
        Console.Write("c:");
        temp = Convert.ToString(Console.ReadLine());
        while (!double.TryParse(temp, out var parsedNumber))
        {
            Console.WriteLine("Коэфицент должен быть числом!");
            temp = Convert.ToString(Console.ReadLine());
        }
        items += temp + ":" + ID + ":";
        var messageBytes = Encoding.UTF8.GetBytes(items);
        socket.Send(messageBytes);
        bytes = await socket.ReceiveAsync(responseBytes);
        response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        Console.WriteLine("Ответ на ваш запрос:" + response);
        items = "";
        Console.WriteLine("Для отключения впишите Exit,для продолжения что-угодно");
        temp = Convert.ToString(Console.ReadLine());
        if (temp == "Exit" || temp=="exit")
        {
            socket.Close();
            break;
        }
    }
}
catch (SocketException)
{
    Console.WriteLine($"Не удалось установить подключение с {socket.RemoteEndPoint}");
}