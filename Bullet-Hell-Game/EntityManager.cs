using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class EntityManager<T> where T : IKillable
    {
        readonly Func<ObservableCollection<T>> entityListGetter;

        private ObservableCollection<T> EntityList
        {
            get
            {
                return entityListGetter();
            }
        }

        public EntityManager(Func<ObservableCollection<T>> getter)
        {
            entityListGetter = getter;
            EntityList.AsEnumerable().ToList().ForEach(x => x.Kill += Kill);
            EntityList.CollectionChanged += AddKillableHandler;
        }

        private void AddKillableHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Remove) && (e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Add) || e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Replace)))
            {
                List<T> newItems = e.NewItems as List<T>;
                List<T> addedItems = newItems.Where(x => !e.OldItems.Contains(x)).ToList();
                addedItems.ForEach(x => x.Kill += Kill);
            }
        }

        void Kill(object? sender, EventArgs e)
        {
            EntityList.Remove((T)sender);
            Debug.WriteLine("Hellos");
        }
    }
}
