
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Personalization.VisitorGroups;

namespace PulsePersonalizationApp.SelectionFactories
{
    public class PulseClimateSegmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            return new[]
            {
                new SelectListItem { Value = "", Text = "Select a Segment..."}
            };
        }
    }
}