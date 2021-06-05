// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings.Sections.Skins;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Screens.Menu;
using osu.Game.Screens.Play;
using osu.Game.Tests.Resources;
using osu.Game.Tests.Visual.Navigation;

namespace osu.Game.Tests.Visual.Settings
{
    public class TestSceneSettingsSkinPreviewButton : OsuGameTestScene
    {
        private LeasedBindable<WorkingBeatmap> workingBeatmap;

        private PreviewSkinButton previewSkinButton;

        public override void SetUpSteps()
        {
            base.SetUpSteps();
            AddStep("get preview skin button", () => previewSkinButton = Game.Settings.ChildrenOfType<PreviewSkinButton>().First());
        }

        [Test]
        public void TestPreviewSkinButtonNoBeatmaps()
        {
            AddStep("click preview button", () => previewSkinButton.Click());
            AddUntilStep("fail because no beatmaps to do the preview", () => Game.ScreenStack.CurrentScreen is MainMenu);
        }

        [Test]
        public void TestPreviewSkinButtonWorkingBeatmapLeased()
        {
            AddStep("open settings", () => Game.Settings.Show());
            AddUntilStep("preview skin button is enabled", () => previewSkinButton.Enabled.Value);
            AddStep("lease working beatmap bindable", () => workingBeatmap = Game.Beatmap.BeginLease(true));
            AddUntilStep("preview skin button is disabled", () => !previewSkinButton.Enabled.Value);
            AddStep("return lease on working beatmap bindable", () =>
            {
                workingBeatmap.Return();
                workingBeatmap = null;
            });
            AddUntilStep("preview skin button is enabled", () => previewSkinButton.Enabled.Value);
        }

        [Test]
        public void TestPreviewSkinButton()
        {
            AddStep("import test beatmap", () => Game.BeatmapManager.Import(TestResources.GetTestBeatmapForImport()).Wait());
            AddStep("click preview skin button", () => previewSkinButton.Click());
            AddUntilStep("wait for dialog appearance", () => Game.DialogOverlay.CurrentDialog != null);
            AddStep("click dialog", () => Game.DialogOverlay.CurrentDialog.Buttons.First().Click());
            AddUntilStep("screen switched to skin preview", () => Game.ScreenStack.CurrentScreen is PlayerLoader);
        }

        private class TestOsuRuleset : OsuRuleset
        {
            public override IEnumerable<Mod> GetModsFor(ModType type) => Enumerable.Empty<Mod>();
        }
    }
}
