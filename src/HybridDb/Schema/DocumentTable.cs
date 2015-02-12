﻿using System;
using System.Data;

namespace HybridDb.Schema
{
    public class DocumentTable : Table
    {
        public DocumentTable(string name) : base(name)
        {
            DocumentColumn = new Column("Document", new SqlColumn(DbType.Binary, Int32.MaxValue, nullable: true));
            Register(DocumentColumn);

            DiscriminatorColumn = new Column("Discriminator", new SqlColumn(DbType.StringFixedLength, 255, nullable: true));
            Register(DiscriminatorColumn);

            VersionColumn = new Column("Version", new SqlColumn(DbType.Int32, nullable: true));
            Register(VersionColumn);
        }

        public Column DocumentColumn { get; private set; }
        public Column DiscriminatorColumn { get; private set; }
        public Column VersionColumn { get; private set; }
    }
}