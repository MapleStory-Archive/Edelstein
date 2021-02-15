﻿using System.Collections.Generic;
using Edelstein.Protocol.Database;

namespace Edelstein.Protocol.Parsing.Entities.Characters
{
    public interface ICharacter : IDataEntity
    {
        int AccountWorldID { get; set; }

        string Name { get; set; }
        byte Gender { get; set; }
        byte Skin { get; set; }
        int Face { get; set; }
        int Hair { get; set; }

        long[] Pets { get; set; }

        byte Level { get; set; }
        short Job { get; set; }
        short STR { get; set; }
        short DEX { get; set; }
        short INT { get; set; }
        short LUK { get; set; }

        int HP { get; set; }
        int MaxHP { get; set; }
        int MP { get; set; }
        int MaxMP { get; set; }

        short AP { get; set; }
        short SP { get; set; }
        IDictionary<byte, byte> ExtendSP { get; set; }

        int EXP { get; set; }
        short POP { get; set; }

        int Money { get; set; }
        int TempEXP { get; set; }

        int FieldID { get; set; }
        byte FieldPortal { get; set; }

        int PlayTime { get; set; }

        short SubJob { get; set; }
    }
}