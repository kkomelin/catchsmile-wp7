using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using CatchSmile.Model;
using System.Data.Linq;

namespace CatchSmile.ViewModel
{
    /// <summary>
    /// The ViewModel for the application.
    /// Contains the methods for database interactions.
    /// <seealso cref="http://en.wikipedia.org/wiki/Model_View_ViewModel"/>
    /// <seealso cref="http://msdn.microsoft.com/en-us/Video/gg241309"/>
    /// </summary>
    public class AppViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// LINQ to SQL data context for the local database.
        /// </summary>
        private AppDataContext appDB;

        /// <summary>
        /// Class constructor, create the data context object.
        /// </summary>
        /// <param name="appDBConnectionString">Database connection string.</param>
        public AppViewModel(string appDBConnectionString)
        {
            appDB = new AppDataContext(appDBConnectionString);
        }

        private ObservableCollection<Node> _nodes;
        /// <summary>
        /// Collection of nodes.
        /// </summary>
        public ObservableCollection<Node> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                NotifyPropertyChanged("Nodes");
            }
        }

        private ObservableCollection<File> _files;
        /// <summary>
        /// Collection of files.
        /// </summary>
        public ObservableCollection<File> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                NotifyPropertyChanged("File");
            }
        }

        /// <summary>
        /// Load node collection from the database.
        /// </summary>
        public void LoadData()
        {
            // Select all nodes from the database using LINQ to SQL.
            var nodesInDB = from Node node in appDB.Nodes
                                select node;
            
            // Load all node objects.
            Nodes = new ObservableCollection<Node>(nodesInDB);
        }

        /// <summary>
        /// Add node object to the database and Nodes collection.
        /// </summary>
        /// <param name="newToDoItem">Node object</param>
        public void AddNode(Node node)
        {
            // Add the node object to the data context.
            appDB.Nodes.InsertOnSubmit(node);

            // Save changes to the database.
            appDB.SubmitChanges();

            // Add the node object to the Nodes observable collection.
            Nodes.Add(node);
        }

        /// <summary>
        /// Remove a node object from the database and Node collection.
        /// </summary>
        /// <param name="toDoForDelete"></param>
        public void DeleteNode(Node node)
        {
            // Remove the node object from the Nodes observable collection.
            Nodes.Remove(node);

            // Remove the node object from the data context.
            appDB.Nodes.DeleteOnSubmit(node);

            // Save changes to the database.
            appDB.SubmitChanges();
        }

        /// <summary>
        /// Add file object to the database and Files collection.
        /// </summary>
        /// <param name="newToDoItem">File object</param>
        public void AddFile(File file)
        {
            // Add the file object to the data context.
            appDB.Files.InsertOnSubmit(file);

            // Save changes to the database.
            appDB.SubmitChanges();
        }

        /// <summary>
        /// Write changes in the data context to the database.
        /// </summary>
        public void SaveChangesToDB()
        {
            appDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify layout that a property has changed.
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
    }
}
