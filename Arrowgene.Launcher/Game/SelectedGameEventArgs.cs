using System;

namespace Arrowgene.Launcher.Game
{
    public class SelectedGameEventArgs : EventArgs
    {

        public SelectedGameEventArgs(GameBase game)
        {
            Game = game;
        }

        public GameBase Game { get; }

    }
}
