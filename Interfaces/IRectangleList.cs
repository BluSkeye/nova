using ssi;
using System.ComponentModel;

namespace ssi
{
    public class IRectangleList : IObservableList<RectangleListItem>, INotifyPropertyChanged
    {
        public bool HasChanged { get; set; }

        public IRectangleList()
            : base(new RectangleListItem.RectangleListItemComparer())
        {
            foreach (RectangleListItem item in Items)
            {
                item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
            }

            HasChanged = false;
        }
        
        ~IRectangleList()
        {
            foreach (RectangleListItem item in Items)
            {
                item.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
            }
        }

        private void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnChildPropertyChanged(e.PropertyName);
            HasChanged = true;
        }

        override protected void ItemRemoved(RectangleListItem removedItem)
        {
            removedItem.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
            HasChanged = true;
        }

        override protected void ItemAdded(RectangleListItem addedItem)
        {
            addedItem.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
            HasChanged = true;
        }

        override protected void ItemReplaced(RectangleListItem replacedItem)
        {
            //overwrite and do anything
        }

        public event PropertyChangedEventHandler ChildPropertyChanged;

        protected void OnChildPropertyChanged(string propertyName)
        {
            if (this.ChildPropertyChanged != null)
                ChildPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}