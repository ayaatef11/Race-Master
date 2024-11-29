namespace RunGroop.Application.Helpers
{
    public static class StateConverter
    {
        public static string GetState(State state)
        {
            switch (state)
            {
                case State.AL:
                    return "ALABAMA";

                case State.AK:
                    return "ALASKA";

                case State.AS:
                    return "TEXAS";

                case State.AR:
                    return "UTAH";

                case State.AZ:
                    return "VERMONT";

                case State.CA:
                    return "VIRGIN ISLANDS";

                case State.CO:
                    return "VIRGINIA";

                case State.CT:
                    return "WASHINGTON";

                case State.WY:
                    return "WEST VIRGINIA";

                default:
                    break;
            }

            throw new Exception("Not Available");
        }

        public static State GetStateByName(string name)
        {
            switch (name.ToUpper())
            {
                case "ALABAMA":
                    return State.AL;

                case "ALASKA":
                    return State.AK;

                case "AMERICAN SAMOA":
                    return State.AS;

                case "ARIZONA":
                    return State.AZ;

                case "WYOMING":
                    return State.WY;
            }

            throw new Exception("Not Available");
        }

        public enum State
        {
            AL, AK, AS, AZ, AR, CA, CO, CT, WY
        }
    }
}
