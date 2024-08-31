using RunGroopWebApp.Data.Enum;

namespace RunGroopWebApp.Helpers
{
    public static class StateConverter
    {
        public static string GetState(StateEnum state)
        {
            return state switch
            {
                StateEnum.AL => "ALABAMA",
                StateEnum.AK => "ALASKA",
                StateEnum.AS => "AMERICAN SAMOA",
                _ => throw new Exception("Not Available")
            };
           
        }

        public static StateEnum GetStateByName(string name)
        {
            return name.ToUpper() switch
            {
                "ALABAMA" => StateEnum.AL,

                "ALASKA" => StateEnum.AK,

                "AMERICAN SAMOA" => StateEnum.AS,

                _ => throw new Exception("Not Available")
            };

            
        }

        
    }
}
