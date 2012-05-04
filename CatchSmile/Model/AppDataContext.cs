using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace CatchSmile.Model
{
    public class AppDataContext: DataContext
    {
        /// <summary>
        /// Pass the connection string to the base class.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public AppDataContext(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Node table.
        /// </summary>
        public Table<Node> Nodes;

        /// <summary>
        /// File table.
        /// </summary>
        public Table<File> Files;
    }
}
