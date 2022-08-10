using UniverseLib.UI.Models;

namespace HalfbyteMedia.OnARoll.MakioClient.UI.Widgets.AutoComplete
{
    public interface ISuggestionProvider
    {
        InputFieldRef InputField { get; }
        bool AnchorToCaretPosition { get; }

        bool AllowNavigation { get; }

        void OnSuggestionClicked(Suggestion suggestion);
    }
}
