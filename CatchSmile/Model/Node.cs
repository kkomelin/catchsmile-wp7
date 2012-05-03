using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace CatchSmile.Model
{
    /// <summary>
    /// This is SQL CE model for Node table.
    /// I used tips from the amazing article http://msdn.microsoft.com/en-us/library/hh286405%28v=vs.92%29.aspx
    /// </summary>
    [Table]
    public class Node : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _nodeId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int NodeId
        {
            get { return _nodeId; }
            set
            {
                if (_nodeId != value)
                {
                    NotifyPropertyChanging("NodeId");
                    _nodeId = value;
                    NotifyPropertyChanged("NodeId");
                }
            }
        }

        private int _nid;

        [Column]
        public int Nid
        {
            get { return _nid; }
            set
            {
                if (_nid != value)
                {
                    NotifyPropertyChanging("Nid");
                    _nid = value;
                    NotifyPropertyChanged("Nid");
                }
            }
        }

        private string _title;

        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _type;

        [Column]
        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    NotifyPropertyChanging("Type");
                    _type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        private string _uri;

        [Column]
        public string Uri
        {
            get { return _uri; }
            set
            {
                if (_uri != value)
                {
                    NotifyPropertyChanging("Uri");
                    _uri = value;
                    NotifyPropertyChanged("Uri");
                }
            }
        }

        /// <summary>
        /// Internal column for the associated File ID value.
        /// </summary>
        [Column]
        internal int _fileId;

        /// <summary>
        /// Entity reference, to identify the File "storage" table.
        /// </summary>
        private EntityRef<File> _file;

        /// <summary>
        /// Association, to describe the relationship between this key and that "storage" table.
        /// </summary>
        [Association(Storage = "_file", ThisKey = "_fileId", OtherKey = "FileId")]
        public File File
        {
            get { return _file.Entity; }
            set
            {
                NotifyPropertyChanging("File");
                _file.Entity = value;

                if (value != null)
                {
                    _fileId = value.FileId;
                }

                NotifyPropertyChanging("File");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to notify that a property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///  Used to notify that a property is about to change.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
