using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// Dynamically removes objects from collection to assist garbage collection and free resources
    /// </summary>
    /// <typeparam name="T">Type of ObservableCollection</typeparam>
    public class EntityManager<T> where T : IKillable
    {
        private List<T> killList = new List<T>();

        /// <summary>
        /// Reference to original observable collection, as removing from a copy would be useless
        /// </summary>
        readonly Func<ObservableCollection<T>> entityListGetter;

        private ObservableCollection<T> EntityList
        {
            get
            {
                return entityListGetter();
            }
        }

        /// <summary>
        /// Initializes an EntityManager with the observable collection containing managed entities
        /// </summary>
        /// <param name="getter">Reference to observable collection through function (i.e. returns ObservableCollection)</param>
        public EntityManager(Func<ObservableCollection<T>> getter)
        {
            entityListGetter = getter;
            EntityList.AsEnumerable().ToList().ForEach(x => x.Kill += Kill);
            EntityList.CollectionChanged += AddKillableHandler;
        }

        /// <summary>
        /// Adds kill handler to newly added entities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddKillableHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Remove) && (e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Add) || e.Action.HasFlag(System.Collections.Specialized.NotifyCollectionChangedAction.Replace)))
            {
                List<T> newItems = e.NewItems as List<T>;
                List<T> addedItems = newItems.Where(x => !e.OldItems.Contains(x)).ToList();
                addedItems.ForEach(x => x.Kill += Kill);
            }
        }

        /// <summary>
        /// Queues up object for removal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Kill(object? sender, EventArgs e)
        {
            killList.Add((T)sender);
        }

        /// <summary>
        /// Kills all queued objects
        /// </summary>
        public void KillFlaggedObjects()
        {
            killList.ForEach(x => EntityList.Remove(x));
            killList.Clear();
        }
    }
}
