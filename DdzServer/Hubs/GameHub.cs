using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DdzServer.Models;
using DdzServer.Services;
using System.Collections.Generic;

namespace DdzServer.Hubs
{
    public class GameHub : Hub
    {
        private readonly RoomService _roomService;
        private readonly Carder _carder;

        public GameHub(RoomService roomService, Carder carder)
        {
            _roomService = roomService;
            _carder = carder;
        }

        // 加入房间
        public async Task JoinRoom(string roomId, string playerId, string nickName)
        {
            var player = new Player { AccountId = playerId, NickName = nickName };
            _roomService.JoinRoom(roomId, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("PlayerJoined", playerId, nickName);
        }

        // 离开房间
        public async Task LeaveRoom(string roomId, string playerId)
        {
            var player = new Player { AccountId = playerId };
            _roomService.LeaveRoom(roomId, player);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("PlayerLeft", playerId);
        }

        // 开始游戏
        public async Task StartGame(string roomId)
        {
            _roomService.StartGame(roomId, _carder);
            await Clients.Group(roomId).SendAsync("GameStarted");
        }

        // 抢地主
        public async Task RobMaster(string roomId, string playerId)
        {
            // 业务逻辑略，假设直接设置为地主
            var player = new Player { AccountId = playerId };
            _roomService.SetMaster(roomId, player);
            await Clients.Group(roomId).SendAsync("MasterSet", playerId);
        }

        // 出牌
        public async Task PlayCards(string roomId, string playerId, List<Card> cards)
        {
            var player = new Player { AccountId = playerId };
            _roomService.PlayCards(roomId, player, cards);
            await Clients.Group(roomId).SendAsync("CardsPlayed", playerId, cards);
        }

        // 结算
        public async Task Settle(string roomId)
        {
            _roomService.Settle(roomId);
            await Clients.Group(roomId).SendAsync("GameSettled");
        }

        // 更多事件可根据原 socket.io 逻辑继续补充...
    }
} 