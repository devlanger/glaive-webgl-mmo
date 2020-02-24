using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GlaiveServer
{
    public class User : WebSocketBehavior
    {
        public MemoryStream memoryStream;
        public BinaryReader reader;

        public Character Character { get; private set; }

        public int Id { get; set; }

        protected override void OnOpen()
        {
            base.OnOpen();

            memoryStream = new MemoryStream();
            reader = new BinaryReader(memoryStream);

            Character = CharactersManager.CreateCharacter();

            int id = UsersManager.GetId();
            this.Id = id;
            Character.Pos = new Vector2UInt16(250, 205);
            Character.baseId = 1;
            UsersManager.AddUser(id, this);

            Character.OnObserveCharacter += Character_OnObserveCharacter;
            Character.OnUnobserveCharacter += Character_OnUnobserveCharacter;

            PacketsSender.SpawnMonster(this, new PacketsSender.SpawnData(Character)
            {
                baseId = 1
            });
            PacketsSender.ControlCharacter(this, Character.id);
        }

        private void Character_OnUnobserveCharacter(int id)
        {
            PacketsSender.DespawnMonster(this, id);
        }

        private void Character_OnObserveCharacter(int id)
        {
            if (CharactersManager.GetCharacter(id, out Character c))
            {
                PacketsSender.SpawnMonster(this, new PacketsSender.SpawnData(c));
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            UsersManager.RemoveUser(Id, Character);

            Character.OnObserveCharacter -= Character_OnObserveCharacter;
            Character.OnUnobserveCharacter -= Character_OnUnobserveCharacter;

            Character = null;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var data = e.RawData;


            PacketsReceivedManager.ReceiveData(this, data);
        }

        public void SendData(byte[] data)
        {
            Send(data);
        }
    }
}
