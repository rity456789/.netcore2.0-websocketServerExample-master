using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketDemo.Model;

namespace WebSocketDemo.WebSockets
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        //mark socketID and user
        private List<Identification> _userIdentify = new List<Identification>(); //store id of socket and username

        public void SetUserIdentification(string sId, int uId)
        {
            Identification user = new Identification();
            user.sId = sId;
            user.uId = uId;
            _userIdentify.Add(user);
        }

        public string GetUserIdentification(int uId)
        {
            var sId = "";
            foreach (var identify in _userIdentify)
            {
                if (uId == identify.uId)
                {
                    sId = identify.sId;
                }
            }
            return sId;
        }


        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public void SetSocketById(string oldId, string newId)
        {
            var socket = _sockets.FirstOrDefault(p => p.Key == oldId).Value;
            
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            string sId = CreateConnectionId();
            while (!_sockets.TryAdd(sId, socket))
            {
                sId = CreateConnectionId();
            }
            


        }

        public async Task RemoveSocket(string id)
        {
            try
            {
                WebSocket socket;
                
                _sockets.TryRemove(id, out socket);

                //remove identify
                var itemToRemove = _userIdentify.Single(r => r.sId == id);
                _userIdentify.Remove(itemToRemove);

                await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);


            }
            catch (Exception)
            {

            }

        }
        
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
