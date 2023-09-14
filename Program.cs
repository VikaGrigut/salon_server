using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System;
using ser_ver;
using System.Collections.Generic;
using курсач;
using System.ComponentModel.DataAnnotations;

IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(ipPoint);
socket.Listen(100);
Console.WriteLine("Сервер запущен. Ожидание подключений...");
while (true)
{
    using Socket client = await socket.AcceptAsync();
    while (true)
    {
        
        var buffer = new byte[1_024];
        var received = client.Receive(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        Request request = JsonSerializer.Deserialize<Request>(response);
        switch (request.command)
        {
            case "registration":
                User userReg = JsonSerializer.Deserialize<User>(request.data);
                userReg.IdCount(Serialization.Input<User>());
                if(!Search.Repetitions(Serialization.Input<User>(), userReg.Login))
                {
                    bool correct = true;
                    List<ValidationResult> result;
                    (correct, result) = userReg.Validation();
                    if (!correct)
                    {
                        string problem = "";
                        foreach(var error in result)
                        {
                            problem = problem + error.ErrorMessage;
                        }
                        Response respon = new Response("no", problem);
                        var messBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respon));
                        client.Send(messBytes, SocketFlags.None);
                    }
                    else
                    {
                        List<User> profUser = new List<User>();
                        profUser.Add(userReg);
                        Serialization.UserOut(profUser);
                        List<User> usersReg = Serialization.Input<User>();
                        usersReg.Add(userReg);
                        Serialization.Output(usersReg);
                        Response respon = new Response("yes", "");
                        var messBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respon));
                        client.Send(messBytes, SocketFlags.None);
                    }
                }
                else
                {
                    Response respon = new Response("no", "Пользователь с таким логином уже существует");
                    var messBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respon));
                    client.Send(messBytes, SocketFlags.None);
                }
                break;
            case "entry":
                string message;
                bool exist;
                string login = JsonSerializer.Deserialize<List<string>>(request.data)[0];
                (message, exist) = Search.LoginIn(Serialization.Input<User>(), login, JsonSerializer.Deserialize<List<string>>(request.data)[1]);
                List<string> mess = new List<string>() { message, JsonSerializer.Serialize(exist) };    
                Response resp = new Response(JsonSerializer.Serialize(mess), "null");
                if (exist)
                {
                    List<User> user = new List<User>();
                    user.Add(Search.Profile(Serialization.Input<User>(), login));
                    Serialization.UserOut(user);
                }
                var messagBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(resp));
                client.Send(messagBytes, SocketFlags.None);
                break;
            case "profile":
                List<User> userProf = Serialization.UserIn<User>();
                string listUser = JsonSerializer.Serialize(userProf);
                var messageBytes = Encoding.UTF8.GetBytes(listUser);
                client.Send(messageBytes, SocketFlags.None);
                break;
            case "myService":
                List<User> userProfile = Serialization.UserIn<User>();
                if(userProfile[0].Services == null)
                {
                    message = "no";
                }
                else
                {
                    message = "yes";
                    userProfile[0].FromServToHistory();
                }
                Response res = new Response(message, JsonSerializer.Serialize(userProfile[0].Services));
                messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(res));
                client.Send(messageBytes, SocketFlags.None);
                break;
            case "balance":
                List<User> userProff = Serialization.UserIn<User>();
                List<User> users = Serialization.Input<User>();
                userProff[0].Balance = userProff[0].Balance + Int32.Parse(request.data);
                users[Search.Profile(users, userProff[0].Login).Id - 1].Balance = userProff[0].Balance;
                Response responss = new Response("ok", userProff[0].Balance.ToString());
                Serialization.UserOut(userProff);
                Serialization.Output(users);
                messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responss));
                client.Send(messageBytes, SocketFlags.None);
                break;
            case "history":
                userProfile = Serialization.UserIn<User>();
                string data;
                if (userProfile[0].Services == null)
                {
                    message = "no";
                    data = "no";
                }
                else
                {
                    userProfile[0].FromServToHistory();
                    if (userProfile[0].History.Count == 0) { message = "no"; data = "no"; } 
                    else { message = "yes"; data = JsonSerializer.Serialize(userProfile[0].History); }
                }
                Response respo = new Response(message, data);
                messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respo));
                client.Send(messageBytes, SocketFlags.None);
                break;
            case "makeService":
                List<string> infServ = JsonSerializer.Deserialize<List<string>>(request.data);
                string[] dataServ = User.DataShifr(infServ[0]);
                bool corect;
                string messag;
                List<Service> services = new List<Service>();
                userProf = Serialization.UserIn<User>();
                (corect, message) = Search.CorrectDate(dataServ);
                if (infServ[0] == "" | infServ[1] == "" | infServ[2] == "")
                {
                    messag = "no";
                    data = "Одно из полей не заполнено!";
                }
                else
                {
                    int re = 0;
                    bool result = Int32.TryParse(infServ[2], out re);
                    if(result)
                    {
                        if (corect)
                        {
                            Service service = new Service()
                            {
                                Name = infServ[1],
                                Price = Int32.Parse(infServ[2]),
                                Date = infServ[0],
                            };
                            if (userProf[0].Services != null)
                            {
                                services = userProf[0].Services;
                            }
                            if ((userProf[0].Balance - service.Price) < 0)
                            {
                                data = "Недостаточно средств. Пополните баланс!";
                                messag = "no";
                            }
                            else
                            {
                                services.Add(service);
                                data = $"Вы записались на услугу: {service.Name} на {service.Date}";
                                userProf[0].Balance -= service.Price;
                                userProf[0].Services = services;
                                messag = "yes";
                                users = Serialization.Input<User>();
                                users[Search.Profile(users, userProf[0].Login).Id - 1].Services = userProf[0].Services;
                                users[Search.Profile(users, userProf[0].Login).Id - 1].Balance = userProf[0].Balance;
                                Serialization.Output<User>(users);
                                Serialization.UserOut(userProf);
                            }
                        }
                        else
                        {
                            data = message;
                            messag = "no";
                        }
                    }
                    else { data = "Некорректные данные"; messag = "no"; }
                }
                Response reso = new Response(messag, data);
                messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(reso));
                client.Send(messageBytes, SocketFlags.None);
                break;
        }
        continue;

    }
}
