using System;
using System.Collections.Generic;

namespace SessionExplorer.Entities
{
    using System.Collections;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Utilities;

    public class Session
    {
        private string sessionId;
        private SessionItemCollection items;
        private BinaryReader binaryReader;
        private readonly int size;
        private readonly DateTime created;
        private readonly DateTime expires;

        #region properties

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId
        {
            get { return sessionId.ToUpper(); }
            set { sessionId = value; }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public SessionItemCollection Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Gets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created
        {
            get { return created; }
        }

        /// <summary>
        /// Gets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires
        {
            get { return expires; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        public Session() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="buffer">The buffer.</param>
        public Session(string sessionId, byte[] buffer)
        {
            Items = new SessionItemCollection();
            SessionId = sessionId;
            if (buffer != null)
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    Decode(new MemoryStream(buffer));
                    ms.Close();
                }
                size = buffer.Length;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="created">The created.</param>
        /// <param name="expires">The expires.</param>
        public Session(string sessionId, byte[] buffer, DateTime created, DateTime expires)
            : this(sessionId, buffer)
        {
            this.created = created;
            this.expires = expires;
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void Decode(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.ReadInt32(); // session timeout
                bool state = reader.ReadBoolean(); // we'll be using this
                reader.ReadBoolean(); // don't need this
                if (state)
                    DecodeStateItems(reader);
            }
        }

        /// <summary>
        /// Decodes the state items.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void DecodeStateItems(BinaryReader reader)
        {
            int sessionItems = reader.ReadInt32();
            if (sessionItems > 0)
            {
                int checkFlag = reader.ReadInt32();
                int currentItem = 0;
                while (currentItem < sessionItems)
                {
                    if (currentItem != checkFlag)
                        Items.Add(new SessionItem(currentItem, 0, reader.ReadString(), null));
                    currentItem++;
                }

                for (currentItem = 0; currentItem < sessionItems; currentItem++)
                    if (currentItem != 0)
                        Items.FindBy(currentItem).SeekPosition = reader.ReadInt32();

                int lastOffset = reader.ReadInt32();
                byte[] buffer = new byte[lastOffset];
                reader.BaseStream.Read(buffer, 0, lastOffset);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    binaryReader = new BinaryReader(memoryStream);
                    SetValues();
                }
            }
        }

        /// <summary>
        /// Fields
        /// </summary>
        enum DataType : byte
        {
            Boolean = 3,
            Byte = 6,
            Char = 7,
            DateTime = 4,
            Decimal = 5,
            Double = 9,
            Guid = 0x11,
            Int16 = 11,
            Int32 = 2,
            Int64 = 12,
            IntPtr = 0x12,
            Null = 0x15,
            Object = 20,
            SByte = 10,
            Single = 8,
            String = 1,
            TimeSpan = 0x10,
            UInt16 = 13,
            UInt32 = 14,
            UInt64 = 15,
            UIntPtr = 0x13
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        private void SetValues()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                binaryReader.BaseStream.Seek(Items[i].SeekPosition, SeekOrigin.Begin);
                Items[i].Value = ReadValueFromStream();
            }
            binaryReader.Close();
        }

        /// <summary>
        /// Reads the value from stream.
        /// </summary>
        /// <returns></returns>
        object ReadValueFromStream()
        {
            int[] array;
            int n;
            switch (((DataType)binaryReader.ReadByte()))
            {
                case DataType.String:
                    {
                        return binaryReader.ReadString();
                    }
                case DataType.Int32:
                    {
                        return binaryReader.ReadInt32();
                    }
                case DataType.Boolean:
                    {
                        return binaryReader.ReadBoolean();
                    }
                case DataType.DateTime:
                    {
                        return new DateTime(binaryReader.ReadInt64());
                    }
                case DataType.Decimal:
                    {
                        array = new int[4];
                        n = 0;
                        goto Label_00CA;
                    }
                case DataType.Byte:
                    {
                        return binaryReader.ReadByte();
                    }
                case DataType.Char:
                    {
                        return binaryReader.ReadChar();
                    }
                case DataType.Single:
                    {
                        return binaryReader.ReadSingle();
                    }
                case DataType.Double:
                    {
                        return binaryReader.ReadDouble();
                    }
                case DataType.SByte:
                    {
                        return binaryReader.ReadSByte();
                    }
                case DataType.Int16:
                    {
                        return binaryReader.ReadInt16();
                    }
                case DataType.Int64:
                    {
                        return binaryReader.ReadInt64();
                    }
                case DataType.UInt16:
                    {
                        return binaryReader.ReadUInt16();
                    }
                case DataType.UInt32:
                    {
                        return binaryReader.ReadUInt32();
                    }
                case DataType.UInt64:
                    {
                        return binaryReader.ReadUInt64();
                    }
                case DataType.TimeSpan:
                    {
                        return new TimeSpan(binaryReader.ReadInt64());
                    }
                case DataType.Guid:
                    {
                        byte[] buffer1 = binaryReader.ReadBytes(0x10);
                        return new Guid(buffer1);
                    }
                case DataType.IntPtr:
                    {
                        if (IntPtr.Size == 4)
                        {
                            return new IntPtr(binaryReader.ReadInt32());
                        }
                        return new IntPtr(binaryReader.ReadInt64());
                    }
                case DataType.UIntPtr:
                    {
                        if (UIntPtr.Size == 4)
                        {
                            return new UIntPtr(binaryReader.ReadUInt32());
                        }
                        return new UIntPtr(binaryReader.ReadUInt64());
                    }
                case DataType.Object:
                    {
                        try
                        {
                            return new BinaryFormatter().Deserialize(binaryReader.BaseStream);
                        }
                        catch
                        {
                            return "Deserialisation failure: Unknown object type.";
                        }
                    }
                case DataType.Null:
                    {
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }

        Label_00CA:
            if (n >= 4)
            {
                return new decimal(array);
            }
            array[n] = binaryReader.ReadInt32();
            n++;
            goto Label_00CA;
        }
    }

    public class SessionItem
    {
        private int seekPosition;
        private readonly int index;
        private readonly string key;
        private object value;

        #region properties

        /// <summary>
        /// Gets or sets the seek position.
        /// </summary>
        /// <value>The seek position.</value>
        public int SeekPosition
        {
            get { return seekPosition; }
            set { seekPosition = value; }
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        /// <value>The full name of the type.</value>
        public string Description
        {
            get { return string.Format("{0}: {1}", key, GetValueAsString()); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionItem"/> class.
        /// </summary>
        public SessionItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionItem"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="seekPosition">The seek position.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public SessionItem(int index, int seekPosition, string key, object value)
        {
            this.index = index;
            this.seekPosition = seekPosition;
            this.key = key;
            this.value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value as string.
        /// </summary>
        /// <returns></returns>
        private string GetValueAsString()
        {
            return value != null
                ? (value.GetType().FullName.Equals("System.String")
                    || value.GetType().FullName.Equals("System.Boolean")
                    || value.GetType().FullName.Equals("System.Int32"))
                    ? string.Format("{0}", value)
                    : (value.GetType().FullName.StartsWith("System.Collections.Generic.List") && value.GetType().FullName.Contains("[[System.String,"))
                        ? string.Format("[{0}]", string.Join(", ", ((List<string>)value).ToArray()))
                        : (value.GetType().FullName.StartsWith("System.Collections.Generic.Dictionary") && value.GetType().FullName.Contains("[[System.String,") && value.GetType().FullName.Contains("],[System.String,"))
                            ? GetDictionaryAsString((Dictionary<string, string>)value)
                            : GetObjectAsString(value)
                : string.Empty;
        }

        /// <summary>
        /// Gets the dictionary as string.
        /// </summary>
        /// <param name="stringDictionary">The string dictionary.</param>
        /// <returns></returns>
        private static string GetDictionaryAsString(IEnumerable<KeyValuePair<string, string>> stringDictionary)
        {
            List<string> kvpString = new List<string>();
            foreach (KeyValuePair<string, string> pair in stringDictionary)
                kvpString.Add(string.Format("{0}: {1}", pair.Key, pair.Value));
            return string.Format("[{0}]", string.Join(", ", kvpString.ToArray()));
        }

        /// <summary>
        /// Gets the dictionary as string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        private static string GetDictionaryAsString(IEnumerable<KeyValuePair<string, object>> dictionary)
        {
            List<string> kvpString = new List<string>();
            foreach (KeyValuePair<string, object> pair in dictionary)
                kvpString.Add(string.Format("{0}: {1}", pair.Key, pair.Value));
            return string.Format("[{0}]", string.Join(", ", kvpString.ToArray()));
        }

        /// <summary>
        /// Gets the object as string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private static string GetObjectAsString(object obj)
        {
            return GetDictionaryAsString(new ObjectProperties(obj));
        }

        #endregion
    }

    public class SessionItemCollection : CollectionBase
    {
        /// <summary>
        /// Gets or sets the <see cref="Entities.SessionItem"/> at the specified index.
        /// </summary>
        /// <value></value>
        public SessionItem this[int index]
        {
            get { return (SessionItem)List[index]; }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(SessionItem value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Entities.SessionItem"/> with the specified key.
        /// </summary>
        /// <value></value>
        public SessionItem this[string key]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    if (this[i].Key.Equals(key))
                        return this[i];
                return null;
            }
            set
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Key.Equals(key))
                    {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public SessionItem FindBy(int index)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Index == index)
                {
                    return this[i];
                }
            }
            return null;
        }
    }
}
