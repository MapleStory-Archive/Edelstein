using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldObj
    {
        int ID { get; set; }
        FieldObjType Type { get; }
        
        IField Field { get; set; }

        Point Position { get; set; }

        IPacket GetEnterFieldPacket();
        IPacket GetLeaveFieldPacket();
    }
}