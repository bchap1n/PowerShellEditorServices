// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;
using OmniSharp.Extensions.DebugAdapter.Protocol.Events;
using OmniSharp.Extensions.DebugAdapter.Protocol.Server;
namespace Microsoft.PowerShell.EditorServices.Services.PowerShell.Host
{
    internal class NullPSHostUI : PSHostUserInterface
    {
        /// <summary>
        /// Gets or sets the debug adapter server facade for forwarding output events.
        /// When set, Write and WriteLine calls are forwarded as DAP output events.
        /// </summary>
        internal IDebugAdapterServerFacade DebugServer { get; set; }

        public NullPSHostUI() => RawUI = new NullPSHostRawUI();

        public override bool SupportsVirtualTerminal => false;

        public override PSHostRawUserInterface RawUI { get; }

        public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions) => new();

        public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice) => 0;

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options) => new(userName: string.Empty, password: new SecureString());

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
            => PromptForCredential(caption, message, userName, targetName, PSCredentialTypes.Default, PSCredentialUIOptions.Default);

        public override string ReadLine() => string.Empty;

        public override SecureString ReadLineAsSecureString() => new();

        public override void Write(string value) => DebugServer?.SendNotification(EventNames.Output, new OutputEvent
        {
            Output = value,
            Category = OutputEventCategory.Console
        });

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value) => Write(value);

        public override void WriteLine(string value) => DebugServer?.SendNotification(EventNames.Output, new OutputEvent
        {
            Output = value + Environment.NewLine,
            Category = OutputEventCategory.Console
        });

        public override void WriteLine() => WriteLine(string.Empty);

        public override void WriteErrorLine(string value) => DebugServer?.SendNotification(EventNames.Output, new OutputEvent
        {
            Output = value + Environment.NewLine,
            Category = OutputEventCategory.Console
        });

        public override void WriteDebugLine(string message) { }

        public override void WriteInformation(InformationRecord record) { }

        public override void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value) => WriteLine(value);
        public override void WriteProgress(long sourceId, ProgressRecord record) { }

        public override void WriteVerboseLine(string message) { }

        public override void WriteWarningLine(string message) { }
    }
}
