using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using PulsePersonalizationApp.SelectionFactories;

namespace PulsePersonalizationApp.Models
{
    public class PulseEthnographicSegmentModel : CriterionModelBase
    {
        #region Editable Properties
        [DojoWidget(
            WidgetType = "dijit.form.FilteringSelect",
            SelectionFactoryType = typeof(PulseEthnographicSegmentSelectionFactory)
        )]
        [Required]
        public string SegmentId { get; set; }

        #endregion

        public override ICriterionModel Copy()
        {
            return ShallowCopy();
        }

    }
}
