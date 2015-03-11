﻿using System;
using System.Data;

namespace HybridDb.Config
{
    public class DocumentTable : Table
    {
        public DocumentTable(string name) : base(name)
        {
            IdColumn = new SystemColumn("Id", typeof(Guid),  isPrimaryKey: true);
            Register(IdColumn);

            EtagColumn = new SystemColumn("Etag", typeof(Guid));
            Register(EtagColumn);

            CreatedAtColumn = new SystemColumn("CreatedAt", typeof(DateTimeOffset));
            Register(CreatedAtColumn);

            ModifiedAtColumn = new SystemColumn("ModifiedAt", typeof(DateTimeOffset));
            Register(ModifiedAtColumn);

            DocumentColumn = new Column("Document", typeof(byte[]), Int32.MaxValue);
            Register(DocumentColumn);

            DiscriminatorColumn = new Column("Discriminator", typeof(string), length: 255);
            Register(DiscriminatorColumn);

            StateColumn = new Column("State", typeof(string), length: 255);
            Register(StateColumn);

            VersionColumn = new Column("Version", typeof(int));
            Register(VersionColumn);
        }

        public SystemColumn IdColumn { get; private set; }
        public SystemColumn EtagColumn { get; private set; }
        public SystemColumn CreatedAtColumn { get; private set; }
        public SystemColumn ModifiedAtColumn { get; private set; }
        public Column DocumentColumn { get; private set; }
        public Column DiscriminatorColumn { get; private set; }
        public Column StateColumn { get; private set; }
        public Column VersionColumn { get; private set; }
    }
}