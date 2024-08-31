using System.Runtime.Serialization;

namespace RunGroopWebApp.Data.Enum
{
    public enum ClubCategory
    {
        [EnumMember(Value ="RoadRunner")]
        RoadRunner,
        [EnumMember(Value = "Womens")]
        Womens,
        [EnumMember(Value = "City")]
        City,
        [EnumMember(Value = "Trail")]
        Trail,
        [EnumMember(Value = "Endurance")]
        Endurance
    }
}
