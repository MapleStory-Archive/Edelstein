﻿using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Stats.Modify;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class StatCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The type of stat")]
        public ModifyStatType Type { get; set; }

        [ArgPosition(1), ArgRequired]
        [ArgDescription("The value of stat")]
        public int Value { get; set; }
    }

    public class StatCommand : AbstractCommand<StatCommandArgs>
    {
        public override string Name => "Stat";
        public override string Description => "Sets the character stat to the desired value";

        public StatCommand()
        {
            Aliases.Add("Set");
        }

        public override async Task Execute(IFieldObjUser user, StatCommandArgs args)
        {
            switch (args.Type)
            {
                case ModifyStatType.Skin:
                case ModifyStatType.Face:
                case ModifyStatType.Hair:
                    await user.Prompt(target =>
                        target.AskAvatar("Is this style okay?", new[] { args.Value })
                    );
                    break;
                case ModifyStatType.SP:
                    if (GameConstants.IsExtendSPJob(user.Character.Job))
                    {
                        var jobLevel = await user.Prompt(s => s.AskNumber("Which job level would you like to set?", min: 0, max: 10));

                        await user.ModifyStats(s => s.SetExtendSP((byte)jobLevel, (byte)args.Value));
                        await user.Message($"Successfully set extend SP (job level: {jobLevel}) to {args.Value}");
                        return;
                    }
                    break;
            }
            // TODO: pet

            await user.ModifyStats(s =>
            {
                switch (args.Type)
                {
                    case ModifyStatType.Skin:
                        s.Skin = (byte)args.Value;
                        break;
                    case ModifyStatType.Face:
                        s.Face = args.Value;
                        break;
                    case ModifyStatType.Hair:
                        s.Hair = args.Value;
                        break;
                    case ModifyStatType.Pet:
                    case ModifyStatType.Pet2:
                    case ModifyStatType.Pet3:
                        break;
                    case ModifyStatType.Level:
                        s.Level = (byte)args.Value;
                        break;
                    case ModifyStatType.Job:
                        s.Job = (short)args.Value;
                        break;
                    case ModifyStatType.STR:
                        s.STR = (short)args.Value;
                        break;
                    case ModifyStatType.DEX:
                        s.DEX = (short)args.Value;
                        break;
                    case ModifyStatType.INT:
                        s.INT = (short)args.Value;
                        break;
                    case ModifyStatType.LUK:
                        s.LUK = (short)args.Value;
                        break;
                    case ModifyStatType.HP:
                        s.HP = args.Value;
                        break;
                    case ModifyStatType.MaxHP:
                        s.MaxHP = args.Value;
                        break;
                    case ModifyStatType.MP:
                        s.MP = args.Value;
                        break;
                    case ModifyStatType.MaxMP:
                        s.MaxMP = args.Value;
                        break;
                    case ModifyStatType.AP:
                        s.AP = (short)args.Value;
                        break;
                    case ModifyStatType.SP:
                        s.SP = (short)args.Value;
                        break;
                    case ModifyStatType.EXP:
                        s.EXP = args.Value;
                        break;
                    case ModifyStatType.POP:
                        s.POP = (short)args.Value;
                        break;
                    case ModifyStatType.Money:
                        s.Money = args.Value;
                        break;
                    case ModifyStatType.TempEXP:
                        s.TempEXP = args.Value;
                        break;
                }
            });
            await user.Message($"Successfully set {args.Type} to {args.Value}");
        }
    }
}