namespace TecX.Agile.Serialization
{
    public static class Constants
    {
        public static class MessageTypeIds
        {
            /// <summary>1</summary>
            public const int CaretMoved = 1;

            /// <summary>2</summary>
            public const int FieldHighlighted = 2;

            /// <summary>21</summary>
            public const int StoryCardMoved = 21;

            /// <summary>32</summary>
            public const int PropertyUpdated = 32;

        }

        public static class DataTypes
        {
            /// <summary>0</summary>
            public const byte Null = 0;

            /// <summary>1</summary>
            public const byte String = 1;

            /// <summary>2</summary>
            public const byte Guid = 2;

            /// <summary>3</summary>
            public const byte Double = 3;

            /// <summary>4</summary>
            public const byte Decimal = 4;

            /// <summary>5</summary>
            public const byte Color = 5;

            /// <summary>6</summary>
            public const byte Int32 = 6;
        }
    }
}
