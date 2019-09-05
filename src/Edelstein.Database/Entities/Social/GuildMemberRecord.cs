using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Database.Entities.Social
{
    public class GuildMemberRecord : IDataEntity
    {
        public int ID { get; set; }
        
        [ForeignKey(typeof(GuildRecord))] public int GuildRecordID { get; set; }
        [ForeignKey(typeof(Character))] public int CharacterID { get; set; }
        
        public string Name { get; set; }
        public short Job { get; set; }
        public byte Level { get; set; }
        public byte Grade { get; set; }
        public bool Online { get; set; }
        public int Commitment { get; set; }
        
        // TODO: AllianceGrade
    }
}