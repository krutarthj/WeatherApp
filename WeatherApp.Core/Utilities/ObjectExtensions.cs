using System;
using System.Reflection;
using MvvmCross.ViewModels;

namespace WeatherApp.Core.Utilities
{
    public static class ObjectExtensions
    {
        public static void CopyPropertiesFrom<T>(this T original, T updated, PropertyInfo[] properties = null)
        {
            if (properties == null)
            {
                //Find all the properties from original object and new object
                properties = typeof(T).GetProperties();
            }

            //Check Each property of this object type
            int propertyCount = properties.Length;
            for (var x = 0; x < propertyCount; x++)
            {
                PropertyInfo property = properties[x];

                //If possible, set the original object's property to the value of the new object
                if (property.CanWrite)
                {
                    //Check if the property is an MvxObservableCollection or not
                    if (property.PropertyType.IsGenericType && typeof(MvxObservableCollection<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()))
                    {
                        //Find the "Refresh" method from MvxObservableCollectionExtensions
                        MethodInfo refreshMethod = typeof(MvxObservableCollectionExtensions).GetMethod("Refresh");
                        if (refreshMethod != null)
                        {
                            //Get the values of old collection and the updated collection
                            object oldCollection = property.GetValue(original);
                            object updatedCollection = property.GetValue(updated);
                           
                            //get property for the propertyType of the property
                            PropertyInfo getPropertyCollection = property.PropertyType.GetProperty("Item");

                            if (getPropertyCollection != null)
                            {
                                Type getPropertyTypeOfCollection = getPropertyCollection.PropertyType;

                                //Make generic Refresh method using the collection's propertyType
                                MethodInfo makeGenericRefreshMethod = refreshMethod.MakeGenericMethod(getPropertyTypeOfCollection);

                                //Refresh method parameters
                                object[] args = { oldCollection, updatedCollection };

                                //Invoke the "Refresh" method using the parameters
                                makeGenericRefreshMethod.Invoke(null, args);
                            }
                        }
                    }
                    else
                    {
                        property.SetValue(original, property.GetValue(updated));
                    }
                }
            }
        }
    }
}