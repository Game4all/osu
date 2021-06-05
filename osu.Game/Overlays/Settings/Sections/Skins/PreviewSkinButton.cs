// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Notifications;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play;

namespace osu.Game.Overlays.Settings.Sections.Skins
{
    public class PreviewSkinButton : SettingsButton
    {
        [Resolved]
        private BeatmapManager beatmapManager { get; set; }

        [Resolved]
        private Bindable<RulesetInfo> ruleset { get; set; }

        [Resolved]
        private Bindable<WorkingBeatmap> workingBeatmap { get; set; }

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private OsuGame game { get; set; }

        [Resolved]
        private DialogOverlay dialogOverlay { get; set; }

        public PreviewSkinButton()
        {
            Text = "Preview selected skin";
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Action = presentDialog;
        }

        protected override void LoadComplete()
        {
            workingBeatmap.BindDisabledChanged(disabled => Enabled.Value = !disabled, true);
            base.LoadComplete();
        }

        private void presentDialog()
        {
            var autoModInstance = ruleset.Value.CreateInstance().GetAutoplayMod();

            if (!beatmapManager.GetAllUsableBeatmapSetsEnumerable(IncludedDetails.Minimal).Any())
            {
                notifications.Post(new SimpleNotification
                {
                    Text = "No usable beatmaps were found for preview."
                });
                return;
            }

            if (autoModInstance == null)
            {
                notifications.Post(new SimpleNotification
                {
                    Text = "The current ruleset doesn't have an autoplay mod for preview."
                });
                return;
            }

            dialogOverlay.Push(new PreviewSkinDialog(() => previewSkin(autoModInstance)));
        }

        private void previewSkin(ModAutoplay autoplayMod)
        {
            var availableBeatmaps = beatmapManager.QueryBeatmaps(beatmap => beatmap.Ruleset == ruleset.Value && !beatmap.BeatmapSet.Protected).ToList();

            workingBeatmap.Value = beatmapManager.GetWorkingBeatmap(availableBeatmaps.ElementAt(RNG.Next(availableBeatmaps.Count())));

            game.PerformFromScreen(screen =>
            {
                screen.Push(new PlayerLoader(() =>
                {
                    return new ReplayPlayer((beatmap, mods) => autoplayMod.CreateReplayScore(beatmap, mods));
                }));
            });
        }
    }
}
