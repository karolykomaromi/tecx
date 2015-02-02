namespace Infrastructure.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed class ObjectDataReader<T> : DbDataReader
    {
        private const string ShemaTableSchema = @"<?xml version=""1.0"" standalone=""yes""?>
<xs:schema id=""NewDataSet"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xs:element name=""NewDataSet"" msdata:IsDataSet=""true"" msdata:MainDataTable=""SchemaTable"" msdata:Locale="""">
    <xs:complexType>
        <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""SchemaTable"" msdata:Locale="""" msdata:MinimumCapacity=""1"">
            <xs:complexType>
            <xs:sequence>
                <xs:element name=""ColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""ColumnOrdinal"" msdata:ReadOnly=""true"" type=""xs:int"" default=""0"" minOccurs=""0"" />
                <xs:element name=""ColumnSize"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
                <xs:element name=""NumericPrecision"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
                <xs:element name=""NumericScale"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
                <xs:element name=""IsUnique"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsKey"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""BaseServerName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""BaseCatalogName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""BaseColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""BaseSchemaName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""BaseTableName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""DataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""AllowDBNull"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""ProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
                <xs:element name=""IsAliased"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsExpression"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsIdentity"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsAutoIncrement"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsRowVersion"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsHidden"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""IsLong"" msdata:ReadOnly=""true"" type=""xs:boolean"" default=""false"" minOccurs=""0"" />
                <xs:element name=""IsReadOnly"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
                <xs:element name=""ProviderSpecificDataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""DataTypeName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""XmlSchemaCollectionDatabase"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""XmlSchemaCollectionOwningSchema"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""XmlSchemaCollectionName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""UdtAssemblyQualifiedName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name=""NonVersionedProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
            </xs:sequence>
            </xs:complexType>
        </xs:element>
        </xs:choice>
    </xs:complexType>
    </xs:element>
</xs:schema>";

        private static readonly HashSet<Type> ScalarTypes = LoadScalarTypes();

        private static Type nullableOfT = typeof(int?).GetGenericTypeDefinition();

        private static List<Attribute> scalarAttributes;
        private static List<Attribute> scalarAttributesPlusRelatedObjectScalarAttributes;

        private readonly IEnumerator<T> enumerator;
        private readonly ObjectDataReaderOptions options;
        private readonly List<Attribute> attributes;

        private T current;
        private bool closed;

        public ObjectDataReader(IEnumerable<T> col)
            : this(col, ObjectDataReaderOptions.Default)
        {
        }

        public ObjectDataReader(IEnumerable<T> col, ObjectDataReaderOptions options)
        {
            this.enumerator = col.GetEnumerator();
            this.options = options;

            // done without a lock, so we risk running twice
            if (scalarAttributes == null)
            {
                scalarAttributes = DiscoverScalarAttributes(typeof(T));
            }

            if (options.FlattenRelatedObjects && scalarAttributesPlusRelatedObjectScalarAttributes == null)
            {
                var atts = DiscoverRelatedObjectScalarAttributes(typeof(T));
                scalarAttributesPlusRelatedObjectScalarAttributes = atts.Concat(scalarAttributes).ToList();
            }

            this.attributes = options.FlattenRelatedObjects ? scalarAttributesPlusRelatedObjectScalarAttributes : scalarAttributes;
        }

        public override int RecordsAffected
        {
            get { return -1; }
        }

        public override int Depth
        {
            get { return 1; }
        }

        public override bool IsClosed
        {
            get { return this.closed; }
        }

        public override int FieldCount
        {
            get
            {
                return this.attributes.Count;
            }
        }

        public override bool HasRows
        {
            get { throw new NotSupportedException(); }
        }

        public override object this[string name]
        {
            get { return this.GetValue(this.GetOrdinal(name)); }
        }

        public override object this[int i]
        {
            get { return this.GetValue(i); }
        }

        public override DataTable GetSchemaTable()
        {
            DataSet s = new DataSet { Locale = CultureInfo.CurrentCulture };

            s.ReadXmlSchema(new System.IO.StringReader(ShemaTableSchema));

            DataTable t = s.Tables[0];

            for (int i = 0; i < this.FieldCount; i++)
            {
                DataRow row = t.NewRow();
                row["ColumnName"] = this.GetName(i);
                row["ColumnOrdinal"] = i;

                Type type = this.GetFieldType(i);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(int?).GetGenericTypeDefinition())
                {
                    type = type.GetGenericArguments()[0];
                }

                row["DataType"] = this.GetFieldType(i);
                row["DataTypeName"] = this.GetDataTypeName(i);
                row["ColumnSize"] = -1;
                t.Rows.Add(row);
            }

            return t;
        }

        public override void Close()
        {
            this.closed = true;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            bool rv = this.enumerator.MoveNext();
            if (rv)
            {
                this.current = this.enumerator.Current;
            }

            return rv;
        }

        public override bool GetBoolean(int i)
        {
            return this.GetValue<bool>(i);
        }

        public override byte GetByte(int i)
        {
            return this.GetValue<byte>(i);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var buf = this.GetValue<byte[]>(i);
            int bytes = Math.Min(length, buf.Length - (int)fieldOffset);
            Buffer.BlockCopy(buf, (int)fieldOffset, buffer, bufferoffset, bytes);
            return bytes;
        }

        public override char GetChar(int i)
        {
            return this.GetValue<char>(i);
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            // throw new NotImplementedException();
            string s = this.GetValue<string>(i);
            int chars = Math.Min(length, s.Length - (int)fieldoffset);
            s.CopyTo((int)fieldoffset, buffer, bufferoffset, chars);

            return chars;
        }

        public override string GetDataTypeName(int i)
        {
            return this.attributes[i].Type.Name;
        }

        public override DateTime GetDateTime(int i)
        {
            return this.GetValue<DateTime>(i);
        }

        public override decimal GetDecimal(int i)
        {
            return this.GetValue<decimal>(i);
        }

        public override double GetDouble(int i)
        {
            return this.GetValue<double>(i);
        }

        public override Type GetFieldType(int i)
        {
            Type t = this.attributes[i].Type;

            if (!this.options.ExposeNullableTypes && IsNullable(t))
            {
                return StripNullableType(t);
            }

            return t;
        }

        public override float GetFloat(int i)
        {
            return this.GetValue<float>(i);
        }

        public override Guid GetGuid(int i)
        {
            return this.GetValue<Guid>(i);
        }

        public override short GetInt16(int i)
        {
            return this.GetValue<short>(i);
        }

        public override int GetInt32(int i)
        {
            return this.GetValue<int>(i);
        }

        public override long GetInt64(int i)
        {
            return this.GetValue<long>(i);
        }

        public override string GetName(int i)
        {
            Attribute a = this.attributes[i];

            if (a.IsRelatedAttribute && this.options.PrefixRelatedObjectColumns)
            {
                return a.FullName;
            }

            return a.Name;
        }

        public override int GetOrdinal(string name)
        {
            for (int i = 0; i < this.attributes.Count; i++)
            {
                var a = this.attributes[i];

                if (!a.IsRelatedAttribute && a.Name == name)
                {
                    return i;
                }

                if (this.options.PrefixRelatedObjectColumns && a.IsRelatedAttribute && a.FullName == name)
                {
                    return i;
                }

                if (!this.options.PrefixRelatedObjectColumns && a.IsRelatedAttribute && a.Name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string GetString(int i)
        {
            return this.GetValue<string>(i);
        }

        public override int GetValues(object[] values)
        {
            for (int i = 0; i < this.attributes.Count; i++)
            {
                values[i] = this.GetValue(i);
            }

            return this.attributes.Count;
        }

        public override object GetValue(int i)
        {
            object o = this.GetValue<object>(i);

            if (!this.options.ExposeNullableTypes && o == null)
            {
                return DBNull.Value;
            }

            return o;
        }

        public override bool IsDBNull(int i)
        {
            object o = this.GetValue<object>(i);

            return o == null;
        }

        public override IEnumerator GetEnumerator()
        {
            return this.enumerator;
        }

        protected override void Dispose(bool disposing)
        {
            this.Close();
            base.Dispose(disposing);
        }

        private static bool IsScalarType(Type t)
        {
            return ScalarTypes.Contains(t);
        }

        private static HashSet<Type> LoadScalarTypes()
        {
            HashSet<Type> set = new HashSet<Type>
                { 
                    // reference types
                    typeof(string),
                    typeof(byte[]),

                    // value types
                    typeof(byte),
                    typeof(short),
                    typeof(int),
                    typeof(long),
                    typeof(float),
                    typeof(double),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(Guid),
                    typeof(bool),
                    typeof(TimeSpan),

                    // nullable value types
                    typeof(byte?),
                    typeof(short?),
                    typeof(int?),
                    typeof(long?),
                    typeof(float?),
                    typeof(double?),
                    typeof(decimal?),
                    typeof(DateTime?),
                    typeof(Guid?),
                    typeof(bool?),
                    typeof(TimeSpan?)
                };

            return set;
        }

        private static List<Attribute> DiscoverScalarAttributes(Type thisType)
        {
            // Not a collection of entities, just an IEnumerable<String> or other scalar type.
            // So add just a single Attribute that returns the object itself
            if (IsScalarType(thisType))
            {
                return new List<Attribute> { new Attribute("Value", "Value", thisType, t => t, false) };
            }

            // find all the scalar properties
            var allProperties = (from p in thisType.GetProperties()
                                 where IsScalarType(p.PropertyType)
                                 select p).ToList();

            // Look for a constructor with arguments that match the properties on name and type
            // (name modulo case, which varies between constructor args and properties in coding convention)
            // If such an "ordering constructor" exists, return the properties ordered by the corresponding
            // constructor args ordinal position.  
            // An important instance of an ordering constructor, is that C# anonymous types all have one.  So
            // this enables a simple convention to specify the order of columns projected by the EntityDataReader
            // by simply building the EntityDataReader from an anonymous type projection.
            // If such a constructor is found, replace allProperties with a collection of properties sorted by constructor order.
            foreach (var completeConstructor in from ci in thisType.GetConstructors()
                                                where ci.GetParameters().Count() == allProperties.Count()
                                                select ci)
            {
                var q = (from cp in completeConstructor.GetParameters()
                         join p in allProperties
                             on new { n = cp.Name.ToLower(), t = cp.ParameterType } equals new { n = p.Name.ToLower(), t = p.PropertyType }
                         select new { cp, p }).ToList();

                // all constructor parameters matched by name and type to properties
                if (q.Count() == allProperties.Count())
                {
                    // sort all properties by constructor ordinal position
                    allProperties = (from o in q
                                     orderby o.cp.Position
                                     select o.p).ToList();

                    // stop looking for an ordering consturctor
                    break;
                }
            }

            return allProperties.Select(p => new Attribute(p)).ToList();
        }

        private static IEnumerable<Attribute> DiscoverRelatedObjectScalarAttributes(Type thisType)
        {
            var atts = new List<Attribute>();

            // get the related objects which aren't scalars, not EntityReference objects and not collections
            var relatedObjectProperties =
                (from p in thisType.GetProperties()
                 where !IsScalarType(p.PropertyType)
                       && !typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
                 select p).ToList();

            foreach (var rop in relatedObjectProperties)
            {
                var type = rop.PropertyType;

                // get the scalar properties for the related type
                var scalars = type.GetProperties().Where(p => IsScalarType(p.PropertyType)).ToList();

                foreach (var sp in scalars)
                {
                    string attName = rop.Name + "_" + sp.Name;

                    // create a value accessor which takes an instance of T, and returns the related object scalar
                    var valueAccessor = TypeHelper.MakeRelatedPropertyAccessor<T, object, T>(rop, sp);
                    Attribute att = new Attribute(rop.Name, attName, sp.PropertyType, valueAccessor, true);
                    atts.Add(att);
                }
            }

            return atts;
        }

        private static bool IsNullable(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == nullableOfT;
        }

        private static Type StripNullableType(Type t)
        {
            return t.GetGenericArguments()[0];
        }

        private TField GetValue<TField>(int i)
        {
            TField val = (TField)this.attributes[i].GetValue(this.current);
            return val;
        }

        private class Attribute
        {
            public readonly Type Type;
            public readonly string FullName;
            public readonly string Name;
            public readonly bool IsRelatedAttribute;

            private readonly Func<T, object> valueAccessor;

            public Attribute(PropertyInfo pi)
            {
                Contract.Requires(pi != null);
                Contract.Requires(pi.DeclaringType != null);

                this.FullName = pi.DeclaringType.Name + "_" + pi.Name;
                this.Name = pi.Name;
                this.Type = pi.PropertyType;
                this.IsRelatedAttribute = false;

                this.valueAccessor = TypeHelper.MakePropertyAccessor<T, object>(pi);
            }

            public Attribute(string fullName, string name, Type type, Func<T, object> getValue, bool isRelatedAttribute)
            {
                this.FullName = fullName;
                this.Name = name;
                this.Type = type;
                this.valueAccessor = getValue;
                this.IsRelatedAttribute = isRelatedAttribute;
            }

            public object GetValue(T target)
            {
                return this.valueAccessor(target);
            }
        }
    }
}
