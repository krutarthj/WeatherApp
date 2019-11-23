using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace WeatherApp.Core.Utilities
{
    public static class MvxObservableCollectionExtensions
    {
        public static void Refresh<T>(this MvxObservableCollection<T> collection, IReadOnlyList<T> items)
        {
            var dispatcher = Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
            if (dispatcher.IsOnMainThread)
            {
                ApplyRefresh(collection, items);
            }
            else
            {
                var taskCompletionSource = new TaskCompletionSource<bool>();
                dispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    ApplyRefresh(collection, items);
                    taskCompletionSource.TrySetResult(true);
                });
                taskCompletionSource.Task.Wait();
            }
        }

        private static void ApplyRefresh<T>(this MvxObservableCollection<T> collection, IReadOnlyList<T> items)
        {
            //If collection is currently empty, add the entire list
            if (collection.Count == 0)
            {
                collection.AddRange(items);
                return;
            }

            //Initialize Properties for type T
            PropertyInfo[] properties = typeof(T).GetProperties();

            //Remove any "old" items from the Collection, and update existing ones
            for (var x = 0; x < collection.Count; x++)
            {
                T item = collection[x];

                //Check to See if item exists in the updated list
                T newItem = FindExistingItem(items, item);
                if (newItem != null)
                {
                    //Update the existing item
                    item.CopyPropertiesFrom(newItem, properties);

                    continue;
                }

                //If the item no longer exists in the new list, remove it from the collection
                collection.RemoveAt(x);
                x--;
            }

            //Add any "new" items to the collection
            int updatedItemsCount = items.Count;
            for (var x = 0; x < updatedItemsCount; x++)
            {
                T updatedItem = items[x];

                if (collection.Count > x && !collection[x].Equals(updatedItem))
                {
                    collection.Insert(x, updatedItem);
                }
                else if (collection.Count <= x)
                {
                    collection.Add(updatedItem);
                }
            }
        }

        private static T FindExistingItem<T>(IReadOnlyList<T> newItems, T existingItem)
        {
            int count = newItems.Count;
            for (var x = 0; x < count; x++)
            {
                T newItem = newItems[x];

                if (newItem != null && newItem.Equals(existingItem))
                {
                    return newItem;
                }
            }

            return default;
        }
    }
}