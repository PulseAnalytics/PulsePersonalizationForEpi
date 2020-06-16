using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using PulsePersonalizationApp.Helpers;
using PulsePersonalizationApp.Models;
using PulsePersonalizationApp.Repositories;

namespace PulsePersonalizationApp.Services
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class InitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // Get all segments when this addon is initialized
            var segments = HttpHelper.ListSegments();

            if (segments != null && segments.Count > 0) { 
                SegmentsListModel model = DataStoreRepository.Instance.LoadData<SegmentsListModel>();
                model.Segments = segments;
                DataStoreRepository.Instance.SaveData(model);
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}