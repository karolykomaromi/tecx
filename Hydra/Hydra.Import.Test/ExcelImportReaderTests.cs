using System.Globalization;
using System.Linq.Expressions;
using DocumentFormat.OpenXml.EMMA;
using Hydra.Infrastructure;

namespace Hydra.Import.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Infrastructure.Logging;
    using Hydra.Nh.Infrastructure.I18n;
    using NHibernate;
    using Xunit;

    public class ExcelImportReaderTests
    {
        [Fact]
        public void Should_Read_ResourceItems_From_Excel_Sheet()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.GetSharedStringTable();

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Resources", s.Name, StringComparison.OrdinalIgnoreCase));

                    WorksheetPart part = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Worksheet worksheet = part.Worksheet;

                    PropertyWriterCollection writers = new PropertyWriterCollectionBuilder<ResourceItem>().ForAll();

                    IImportReader<ResourceItem> items = new ExcelImportReader<ResourceItem>(worksheet, sharedStringTable, writers);

                    ResourceItem[] actual = items.ToArray();

                    Assert.Equal("FOO", actual[0].Name);
                    Assert.Equal("DE", actual[0].TwoLetterISOLanguageName);
                    Assert.Equal("FOO_DE", actual[0].Value);

                    Assert.Equal("BAR.BAZ", actual[3].Name);
                    Assert.Equal("EN", actual[3].TwoLetterISOLanguageName);
                    Assert.Equal("BAR.BAZ_EN", actual[3].Value);
                }
            }
        }
    }

    public class PropertyWriterCollectionBuilder<T> : Builder<PropertyWriterCollection>
    {
        private readonly List<Func<IPropertyWriter>> writerFactories;
        
        public PropertyWriterCollectionBuilder()
        {
            this.writerFactories = new List<Func<IPropertyWriter>>();
        }

        public static Func<IPropertyWriter> GetWriterFactory(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return () => new StringPropertyWriter(property);
            }

            return () => PropertyWriter.Null;
        }

        public PropertyWriterCollectionBuilder<T> For<TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            MemberExpression expression = (MemberExpression)propertySelector.Body;

            PropertyInfo property = (PropertyInfo)expression.Member;

            Func<IPropertyWriter> writerFactory = GetWriterFactory(property);

            this.writerFactories.Add(writerFactory);

            return this;
        }

        public PropertyWriterCollectionBuilder<T> ForAll()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                PropertyInfo p = property;

                Func<IPropertyWriter> writerFactory = GetWriterFactory(property);

                this.writerFactories.Add(writerFactory);
            }

            return this;
        }

        public override PropertyWriterCollection Build()
        {
            return new PropertyWriterCollection(this.writerFactories.Select(f => f()).ToArray());
        }
    }

    public interface IImportReader<out T> : IEnumerable<T>
    {
    }

    public class NullWriter<T> : IImportWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            return new ImportFailed();
        }
    }

    public class NullReader<T> : IImportReader<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public interface IImportWriter<in T>
    {
        ImportResult Write(IEnumerable<T> items);
    }

    public class NhBulkImportWriter<T> : IImportWriter<T>
    {
        private readonly IStatelessSession session;

        public NhBulkImportWriter(IStatelessSession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public ImportResult Write(IEnumerable<T> items)
        {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<ImportResult>() != null);

            try
            {
                foreach (T item in items)
                {
                    this.session.Insert(item);
                }

                return new ImportSucceeded();
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new ImportFailed();
            }
        }
    }

    public class ExcelImportReader<T> : IImportReader<T>
        where T : new()
    {
        private readonly Worksheet worksheet;
        private readonly SharedStringTable sharedStringTable;
        private readonly PropertyWriterCollection writers;

        public ExcelImportReader(Worksheet worksheet, SharedStringTable sharedStringTable, PropertyWriterCollection writers)
        {
            Contract.Requires(worksheet != null);
            Contract.Requires(sharedStringTable != null);
            Contract.Requires(writers != null);

            this.worksheet = worksheet;
            this.sharedStringTable = sharedStringTable;
            this.writers = writers;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Row rowWithPropertyNames = this.worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex == 1);

            if (rowWithPropertyNames == null)
            {
                yield break;
            }

            var properties = rowWithPropertyNames.Descendants<Cell>()
                .Select(cell => new
                {
                    PropertyName = ExcelHelper.GetCellValue(cell, this.sharedStringTable),
                    ColumnName = ExcelHelper.GetColumnName(cell)
                })
                .ToDictionary(x => x.ColumnName, x => x.PropertyName, StringComparer.OrdinalIgnoreCase);

            foreach (Row row in this.worksheet.Descendants<Row>().SkipWhile(r => r.RowIndex <= 1))
            {
                T item = new T();

                foreach (Cell cell in row.Descendants<Cell>())
                {
                    string columnName = ExcelHelper.GetColumnName(cell);

                    string propertyName;
                    if (properties.TryGetValue(columnName, out propertyName))
                    {
                        IPropertyWriter writer = this.writers[propertyName];

                        string value = ExcelHelper.GetCellValue(cell, this.sharedStringTable);

                        writer.Write(item, value, CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);
                    }
                }

                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class PropertyWriterCollection
    {
        private readonly IDictionary<string, IPropertyWriter> writers;

        public PropertyWriterCollection(params IPropertyWriter[] writers)
        {
            this.writers = (writers ?? new IPropertyWriter[0])
                .ToDictionary(
                    w => w.PropertyName,
                    w => w,
                    StringComparer.OrdinalIgnoreCase);
        }

        public IPropertyWriter this[string propertyName]
        {
            get
            {
                Contract.Requires(propertyName != null);
                Contract.Ensures(Contract.Result<IPropertyWriter>() != null);

                IPropertyWriter writer;
                if (this.writers.TryGetValue(propertyName, out writer))
                {
                    return writer;
                }

                return PropertyWriter.Null;
            }
        }

        public void Add(IPropertyWriter writer)
        {
            Contract.Requires(writer != null);

            this.writers[writer.PropertyName] = writer;
        }
    }

    public class NullPropertyWriter : IPropertyWriter
    {
        public string PropertyName
        {
            get { return string.Empty; }
        }

        public void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
        }
    }

    public abstract class PropertyWriter : IPropertyWriter
    {
        public static readonly IPropertyWriter Null = new NullPropertyWriter();

        public abstract string PropertyName { get; }

        public abstract void Write(object instance, string value, CultureInfo source, CultureInfo target);
    }

    public interface IPropertyWriter
    {
        string PropertyName { get; }

        void Write(object instance, string value, CultureInfo source, CultureInfo target);
    }

    public class StringPropertyWriter : PropertyWriter
    {
        private readonly PropertyInfo property;

        public StringPropertyWriter(PropertyInfo property)
        {
            Contract.Requires(property != null);

            this.property = property;
        }

        public override string PropertyName
        {
            get { return this.property.Name; }
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            Contract.Requires(instance != null);

            this.property.SetValue(instance, value);
        }
    }
}
