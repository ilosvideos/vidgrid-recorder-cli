﻿using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class CancelRecording : BaseCommand
    {
        public override string Name => AvailableCommand.CancelRecording.GetDescription();

        public CancelRecording(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            
        }
    }
}
