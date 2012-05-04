using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace CatchSmile.Model
{
    /// <summary>
    /// The model for a file.
    /// 
    /// Drupal 7 file structure:
    /// 'filesize' => filesize($filename),
    /// 'filename' => basename($filename),
    /// 'file' => base64_encode(file_get_contents($filename)),
    /// 'uid' => $logged_user->user->uid,
    /// </summary>
    [Table]
    public class File : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _fileId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int FileId
        {
            get { return _fileId; }
            set
            {
                if (_fileId != value)
                {
                    NotifyPropertyChanging("FileId");
                    _fileId = value;
                    NotifyPropertyChanged("FileId");
                }
            }
        }

        private int _fid;

        [Column]
        public int Fid
        {
            get { return _fid; }
            set
            {
                if (_fid != value)
                {
                    NotifyPropertyChanging("Fid");
                    _fid = value;
                    NotifyPropertyChanged("Fid");
                }
            }
        }

        private long _filesize;

        [Column]
        public long FileSize
        {
            get { return _filesize; }
            set
            {
                if (_filesize != value)
                {
                    NotifyPropertyChanging("FileSize");
                    _filesize = value;
                    NotifyPropertyChanged("FileSize");
                }
            }
        }

        private string _filename;

        [Column]
        public string FileName
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    NotifyPropertyChanging("FileName");
                    _filename = value;
                    NotifyPropertyChanged("FileName");
                }
            }
        }

        private string _fileContent;

        //[Column]
        public string FileContent
        {
            get { return _fileContent; }
            set
            {
                if (_fileContent != value)
                {
                    NotifyPropertyChanging("FileContent");
                    _fileContent = value;
                    NotifyPropertyChanged("FileContent");
                }
            }
        }

        private int _uid;

        [Column]
        public int Uid
        {
            get { return _uid; }
            set
            {
                if (_uid != value)
                {
                    NotifyPropertyChanging("Uid");
                    _uid = value;
                    NotifyPropertyChanged("Uid");
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
        /// Used to notify that a property is about to change.
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
