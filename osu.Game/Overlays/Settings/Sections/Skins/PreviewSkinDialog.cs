// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Overlays.Settings.Sections.Skins
{
    public class PreviewSkinDialog : PopupDialog
    {
        public PreviewSkinDialog(Action action)
        {
            HeaderText = @"Preview selected skin in gameplay?";
            Icon = FontAwesome.Regular.Gem;

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes! I want to see how good this new skin looks!",
                    Action = action
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!"
                }
            };
        }
    }
}
