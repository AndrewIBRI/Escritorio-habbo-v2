namespace Butterfly.Util
{
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class QueryChunk
    {
        private EndingType endingType;
        private Dictionary<string, object> parameters;
        private StringBuilder queries;
        private int queryCount;

        public QueryChunk()
        {
            this.parameters = new Dictionary<string, object>();
            this.queries = new StringBuilder();
            this.queryCount = 0;
            this.endingType = EndingType.Sequential;
        }

        public QueryChunk(string startQuery)
        {
            this.parameters = new Dictionary<string, object>();
            this.queries = new StringBuilder(startQuery);
            this.endingType = EndingType.Continuous;
            this.queryCount = 0;
        }

        internal void AddParameter(string parameterName, object value)
        {
            this.parameters.Add(parameterName, value);
        }

        internal void AddQuery(string query)
        {
            this.queryCount++;
            this.queries.Append(query);
            switch (this.endingType)
            {
                case EndingType.Sequential:
                    this.queries.Append(";");
                    break;

                case EndingType.Continuous:
                    this.queries.Append(",");
                    break;
            }
        }

        internal void Dispose()
        {
            this.parameters.Clear();
            this.queries.Clear();
            this.parameters = null;
            this.queries = null;
        }

        internal void Execute(IQueryAdapter dbClient)
        {
            if (this.queryCount != 0)
            {
                this.queries = this.queries.Remove(this.queries.Length - 1, 1);
                dbClient.setQuery(this.queries.ToString());
                foreach (KeyValuePair<string, object> pair in this.parameters)
                {
                    dbClient.addParameter(pair.Key, pair.Value);
                }
                dbClient.runQuery();
            }
        }
    }
}

