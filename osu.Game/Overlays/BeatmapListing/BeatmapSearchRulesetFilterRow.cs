// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Resources.Localisation.Web;
using osu.Game.Rulesets;

namespace osu.Game.Overlays.BeatmapListing
{
    public class BeatmapSearchRulesetFilterRow : BeatmapSearchFilterRow<RulesetInfo>
    {
        private const string ruleset_filter_item_any = @"Any";

        public BeatmapSearchRulesetFilterRow()
            : base(BeatmapsStrings.ListingSearchFiltersMode)
        {
        }

        protected override Drawable CreateFilter() => new RulesetFilter();

        private class RulesetFilter : BeatmapSearchFilter
        {
            protected override TabItem<RulesetInfo> CreateTabItem(RulesetInfo value) => new RulesetFilterTabItem(value);

            [BackgroundDependencyLoader]
            private void load(RulesetStore rulesets)
            {
                AddItem(new RulesetInfo
                {
                    Name = ruleset_filter_item_any
                });

                foreach (var r in rulesets.AvailableRulesets)
                    AddItem(r);
            }
        }

        private class RulesetFilterTabItem : FilterTabItem<RulesetInfo>
        {
            public RulesetFilterTabItem(RulesetInfo value)
                : base(value)
            {
            }

            protected override LocalisableString LabelFor(RulesetInfo ruleset) => ruleset.Name == ruleset_filter_item_any ? BeatmapsStrings.ModeAny : ruleset.Name;
        }
    }
}
