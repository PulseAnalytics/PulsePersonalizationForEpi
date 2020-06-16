using EPiServer.Data;
using EPiServer.Data.Dynamic;
using PulsePersonalizationApp.Interfaces;
using System.Collections.Generic;

namespace PulsePersonalizationApp.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true, AutomaticallyCreateStore = true)]
    public class SegmentsListModel : IDataStoreModelInterface
    {
        public Identity Id { get; set; }
        public List<MarketSegmentModel> Segments { get; set; }
    }
}