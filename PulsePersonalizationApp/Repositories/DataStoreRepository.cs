using EPiServer.Data.Dynamic;
using PulsePersonalizationApp.Interfaces;
using PulsePersonalizationApp.Models;
using System;
using System.Linq;

namespace PulsePersonalizationApp.Repositories
{
    public class DataStoreRepository
    {
        private static DataStoreRepository _instance;

        public static DataStoreRepository Instance
        {
            get { return _instance ?? (_instance = new DataStoreRepository()); }
        }

        private static DynamicDataStore Store<T>()
        {
            return typeof(T).GetStore(); 
        }

        public bool SaveData<T>(T settings) where T : IDataStoreModelInterface, new()
        {
            try
            {
                T currentSettings = LoadData<T>();
                Store<T>().Save(settings, currentSettings.Id);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public T LoadData<T>() where T : IDataStoreModelInterface, new()
        {
            T currentSettings = Store<T>().Items<T>().FirstOrDefault();
            if (currentSettings == null)
            {
                currentSettings = new T();
                Store<T>().Save(currentSettings);
            }
            return currentSettings;
        }
    }
}