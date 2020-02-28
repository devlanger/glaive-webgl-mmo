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

            Character = CharactersManager.CreateCharacter<Player>();

            int id = UsersManager.GetId();
            this.Id = id;
            Character.Pos = new Vector2UInt16(250, 205);
            UsersManager.AddUser(id, this);

            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.STR, 6);
            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.INT, 2);
            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.VIT, 4);
            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.DEX, 3);

            CharactersManager.Stats.SetProperty<int>(Character.id, ObjectStats.RESPAWN_TIME, 3);
            CharactersManager.Stats.SetProperty<int>(Character.id, ObjectStats.HP, 100);
            CharactersManager.Stats.SetProperty<int>(Character.id, ObjectStats.MAX_HP, 100);
            CharactersManager.Stats.SetProperty<uint>(Character.id, ObjectStats.MAX_EXPERIENCE, 300);
            CharactersManager.Stats.SetProperty<ushort>(Character.id, ObjectStats.LVL, 1);
            RegisterEventHandlers();

            PacketsSender.SpawnMonster(this, new PacketsSender.SpawnData(Character)
            {
                baseId = 0
            });

            PacketsSender.ControlCharacter(this, new PacketsSender.ControlData(Character));

            Item i = ItemsManager.Instance.CreateItem(3);
            CharactersManager.Items.records.Add(Character.id, new RecordsHandler<ushort, Item>());
            CharactersManager.Items.GetRecords(Character.id).SetRecord(0, i);
            PacketsSender.SendItemsList(this, RecordType.BACKPACK, CharactersManager.Items.GetRecords(Character.id).records);
        }

        private void Character_OnAttack(Character target)
        {
            foreach (var item in Character.GetObservedUsers())
            {
                PacketsSender.SendAttackAnimation(item, Character);
            }

            PacketsSender.SendAttackAnimation(this, Character);
        }

        private void RegisterEventHandlers()
        {
            Character.OnObserveCharacter += Character_OnObserveCharacter;
            Character.OnUnobserveCharacter += Character_OnUnobserveCharacter;
            Character.OnAttack += Character_OnAttack;
            Character.OnRespawn += Character_OnRespawn;

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.HP, (val) =>
            {
                int health = CharactersManager.Stats.GetProperty<int>(Character.id, ObjectStats.HP);

                PacketsSender.SendStat(this, Character.id, ObjectStats.HP, ObjectType.INT, health);
            });

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

            CharactersManager.Stats.RegisterChange(Character.id, ObjectStats.DEAD, (val) =>
            {
                byte value = CharactersManager.Stats.GetProperty<byte>(Character.id, ObjectStats.DEAD);
                PacketsSender.SendStat(this, Character.id, ObjectStats.DEAD, ObjectType.BYTE, value);
            });
        }

        private void Character_OnRespawn(Character obj)
        {
            PacketsSender.MoveData data = new PacketsSender.MoveData()
            {
                id = obj.id,
                posX = 235,
                posY = 243
            };

            foreach (var item in obj.GetObservedUsers())
            {
                PacketsSender.SetPosition(item, data);
            }

            PacketsSender.SetPosition(this, data);
        }

        private void Character_OnUnobserveCharacter(int id)
        {
            PacketsSender.DespawnMonster(this, id);
        }

        private void Character_OnObserveCharacter(int id)
        {
            if (CharactersManager.GetCharacter(id, out WorldObject c))
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
