namespace TecX.Search.Data.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    public class CustomSprocAccessor<TResult> : SprocAccessor<TResult>
    {
        private readonly string procedureName;

        private int totalRowCount;

        private bool totalRowCountSet;

        public CustomSprocAccessor(Database database, string procedureName, IRowMapper<TResult> rowMapper)
            : base(database, procedureName, rowMapper)
        {
            this.procedureName = procedureName;
        }

        public IParameterMapper ParameterMapper
        {
            get
            {
                FieldInfo fieldInfo = typeof(SprocAccessor<TResult>).GetField(
                    FieldNames.ParameterMapper, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fieldInfo != null)
                {
                    return (IParameterMapper)fieldInfo.GetValue(this);
                }

                throw new MissingFieldException(typeof(SprocAccessor<TResult>).Name, FieldNames.ParameterMapper);
            }
        }

        public IResultSetMapper<TResult> ResultSetMapper
        {
            get
            {
                FieldInfo fieldInfo = typeof(CommandAccessor<TResult>).GetField(
                    FieldNames.ResultSetMapper, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fieldInfo != null)
                {
                    return (IResultSetMapper<TResult>)fieldInfo.GetValue(this);
                }

                throw new MissingFieldException(typeof(SprocAccessor<TResult>).Name, FieldNames.ResultSetMapper);
            }
        }

        public int TotalRowCount
        {
            get
            {
                if (!this.totalRowCountSet)
                {
                    throw new InvalidOperationException(
                        "TotalRowCount is only available after the DbCommand of this Accessor has finished (after iterating over all results).");
                }

                return this.totalRowCount;
            }

            private set
            {
                this.totalRowCount = value;

                this.totalRowCountSet = true;
            }
        }

        public string ProcedureName
        {
            get
            {
                return this.procedureName;
            }
        }

        public override IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            using (DbCommand command = this.Database.GetStoredProcCommand(this.ProcedureName))
            {
                this.ParameterMapper.AssignParameters(command, parameterValues);

                using (IDataReader reader = this.Database.ExecuteReader(command))
                {
                    List<TResult> result = new List<TResult>();

                    foreach (TResult r in this.ResultSetMapper.MapSet(reader))
                    {
                        result.Add(r);
                    }

                    DbParameter parameter = command.Parameters
                                                .OfType<DbParameter>()
                                                .FirstOrDefault(p => p.ParameterName.EndsWith(FieldNames.TotalRowsCount, StringComparison.InvariantCultureIgnoreCase));

                    if (parameter != null)
                    {
                        this.TotalRowCount = (int)parameter.Value;
                    }
                    else
                    {
                        this.TotalRowCount = -1;
                    }

                    return result;
                }
            }
        }

        private static class FieldNames
        {
            /// <summary>
            /// parameterMapper
            /// </summary>
            public const string ParameterMapper = "parameterMapper";

            /// <summary>
            /// resultSetMapper
            /// </summary>
            public const string ResultSetMapper = "resultSetMapper";

            /// <summary>
            /// totalRowsCount
            /// </summary>
            public const string TotalRowsCount = "totalRowsCount";
        }
    }
}