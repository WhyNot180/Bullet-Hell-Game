using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private ObservableCollection<T> entityList;

        /// <summary>
        /// Initializes an EntityManager with the observable collection containing managed entities
        /// </summary>
        /// <param name="getter">Reference to observable collection through function (i.e. returns ObservableCollection)</param>
        public EntityManager(ObservableCollection<T> entityList)
        {
            this.entityList = entityList;
            this.entityList.AsEnumerable().ToList().ForEach(x => x.Kill += Kill);
            this.entityList.CollectionChanged += AddKillableHandler;
        }

        /// <summary>
        /// Adds kill handler to newly added entities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddKillableHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!e.Action.HasFlag(NotifyCollectionChangedAction.Remove) && 
                (e.Action.HasFlag(NotifyCollectionChangedAction.Add) || 
                e.Action.HasFlag(NotifyCollectionChangedAction.Replace)))
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
            killList.ForEach(x => entityList.Remove(x));
            killList.Clear();
        }
    }
}
