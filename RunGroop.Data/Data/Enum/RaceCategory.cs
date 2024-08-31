using System.Runtime.Serialization;

namespace RunGroopWebApp.Data.Enum
{
    public enum RaceCategory
    {
        [EnumMember(Value ="Maration")]
        Marathon,
        [EnumMember(Value = "Ultra")]
        Ultra,
        [EnumMember(Value = "FiveK")]
        FiveK,
        [EnumMember(Value = "TenK")]
        TenK,
        [EnumMember(Value = "HalfMaration")]
        HalfMarathon
    }
}