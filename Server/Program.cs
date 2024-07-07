

using System.Collections.Concurrent;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;

var server = new WebSocketServer("wss://0.0.0.0:80");

// the key is the id of host socket. and value is instance of host socket
var connections =new ConcurrentDictionary<string, IWebSocketConnection>(); 

server.Start(ws=>{
    ws.OnOpen = () =>
            {
                string wsId = ws.ConnectionInfo.Id.ToString();
                Console.WriteLine($"FFFFFFFFFFFFF WebSocket Opened: {wsId}");
                connections[wsId] = ws; // add socket to list of active sockets.
                var response = new MessageData
                {
                    Type = "lobby_created",
                    Id = wsId
                };
                string jsonResponse = JsonConvert.SerializeObject(response);
                ws.Send(jsonResponse);

            };
    ws.OnMessage = message=>{
        
        dynamic messageObj = Newtonsoft.Json.JsonConvert.DeserializeObject(message);

        IWebSocketConnection otherPlayer = connections[messageObj.Id.ToString()]; // Get the socket of the other player

        // otherPlayer.Send("You got contacted");
        if (messageObj.Type == "connect"){ // other player joined the lobby
            var response = new MessageData
                {
                    Type = "other_joined",
                    Id =  ws.ConnectionInfo.Id.ToString()
                };
                string jsonResponse = JsonConvert.SerializeObject(response);
                otherPlayer.Send(jsonResponse);
        }

        else if(messageObj.Type == "move"){ // if a player moved alert the other player
            Console.WriteLine("YAAY SOMETHING MOVD");
            var response = new MoveData
                {
                    Type = "move",
                    nodeId =  messageObj.nodeId
                };
            string jsonResponse = JsonConvert.SerializeObject(response);
            otherPlayer.Send(jsonResponse);

        }
        
    };
  
});



WebApplication.CreateBuilder(args).Build().Run();


