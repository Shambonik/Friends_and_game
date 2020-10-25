namespace AureoleCore.Responses
{
    public class CreateNewCharacter
    {
        public Error error { get; set; }

        public UnitCreateResponseData data { get; set; }

        public class UnitCreateResponseData
        {
            public long unit_id { get; set; }
        }
    }
}

