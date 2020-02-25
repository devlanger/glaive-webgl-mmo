using GameCoreEngine;
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
            CharactersManager.Stats.SetProperty<uint>(Character.id, ObjectStats.MAX_EXPERIENCE, 300);
            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.LVL, 1);
            RegisterEventHandlers();

            PacketsSender.SpawnMonster(this, new PacketsSender.SpawnData(Character)
            {
                baseId = 1
            });
            PacketsSender.ControlCharacter(this, Character.id);
        }

        private void RegisterEventHandlers()
        {
            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.EXPERIENCE, (val) =>
            {
                uint maxExperience = CharactersManager.Stats.GetProperty<uint>(Character.id, ObjectStats.MAX_EXPERIENCE);

                PacketsSender.SendStat(this, Character.id, ObjectStats.MAX_EXPERIENCE, ObjectType.UINT, maxExperience);
                PacketsSender.SendStat(this, Character.id, ObjectStats.EXPERIENCE, ObjectType.UINT, val);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.LVL, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.LVL);
                PacketsSender.SendStat(this, Character.id, ObjectStats.LVL, ObjectType.USHORT, value);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.STR, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.STR);
                PacketsSender.SendStat(this, Character.id, ObjectStats.STR, ObjectType.USHORT, value);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.DEX, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.DEX);
                PacketsSender.SendStat(this, Character.id, ObjectStats.DEX, ObjectType.USHORT, value);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.VIT, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.VIT);
                PacketsSender.SendStat(this, Character.id, ObjectStats.VIT, ObjectType.USHORT, value);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.INT, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.INT);
                PacketsSender.SendStat(this, Character.id, ObjectStats.INT, ObjectType.USHORT, value);
            });

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.STATPOINTS, (val) =>
            {
                ushort value = CharactersManager.Stats.GetProperty<ushort>(Character.id, ObjectStats.STATPOINTS);
                PacketsSender.SendStat(this, Character.id, ObjectStats.STATPOINTS, ObjectType.USHORT, value);
            });
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
