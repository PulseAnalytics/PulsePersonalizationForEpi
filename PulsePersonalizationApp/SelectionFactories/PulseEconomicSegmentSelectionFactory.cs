
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Personalization.VisitorGroups;

namespace PulsePersonalizationApp.SelectionFactories
{
    public class PulseEconomicSegmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            return new[]
            {
                new SelectListItem { Value = "", Text = "Select a Segment..."},
                new SelectListItem { Value = "c_employment_rate", Text = "Nine-to-Five Crew"},
                new SelectListItem { Value = "c_mortgage_with_rate", Text = "Mortage Rate Trends"},
                new SelectListItem { Value = "c_property_tax", Text = "The Tax Man"},
                new SelectListItem { Value = "c_families_below_poverty_level", Text = "Below the Poverty Line"},
                new SelectListItem { Value = "c_gini_index", Text = "Gini Index"},
                new SelectListItem { Value = "c_family_below_poverty_level", Text = "Family Below Poverty Level"},
                new SelectListItem { Value = "c_per_capita_income", Text = "Making Bread"},
                new SelectListItem { Value = "c_housing_unit_median_gross_rent", Text = "For Rent"},
                new SelectListItem { Value = "c_housing_unit_median_value", Text = "Market Price"},
                new SelectListItem { Value = "c_median_household_income", Text = "Average Family Income"},
                new SelectListItem { Value = "c_occupancy_owner_rate", Text = "No Vacancy"},
                new SelectListItem { Value = "c_average_household_income", Text = "Median Moolah Made"}
            };
        }
    }
}