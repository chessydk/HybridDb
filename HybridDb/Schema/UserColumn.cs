﻿using System;

namespace HybridDb.Schema
{
    public class UserColumn : Column
    {
        public UserColumn(string columnName, SqlColumn sqlColumn)
        {
            Name = columnName;
            SqlColumn = sqlColumn;
        }

        public UserColumn(string name, Type type)
        {
            Name = name;
            SqlColumn = new SqlColumn(type);
        }

        public UserColumn(string name)
        {
            Name = name;
            SqlColumn = new SqlColumn();
        }
    }

    public class CollectionColumn : Column
    {
        public CollectionColumn(Table table, string columnName)
        {
            Name = columnName;
            SqlColumn = new SqlColumn(typeof(int));
            Table = new Table(table.Name + "_" + columnName);
        }

        public Table Table { get; set; }
    }
}