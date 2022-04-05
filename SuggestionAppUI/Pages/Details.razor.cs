using Microsoft.AspNetCore.Components;

namespace SuggestionAppUI.Pages;

public partial class Details
{
    [Parameter]
    public string Id { get; set; }

    private SuggestionModel _suggestion;
    private UserModel _loggedInUser;

    private List<StatusModel> _statuses;
    private string _settingStatus = "";
    private string _urlText = "";

    protected override async Task OnInitializedAsync()
    {
        _suggestion = await SuggestionData.GetSuggestion(Id);
        _loggedInUser = await AuthProvider.GetUserFromAuth(UserData);
        _statuses = await StatusData.GetAllStatuses();
    }

    private async Task CompleteSetStatus()
    {
        switch (_settingStatus)
        {
            case "Completed":
                if (string.IsNullOrWhiteSpace(_urlText)) return;

                _suggestion.SuggestionStatus = _statuses.First(s =>
                    string.Equals(s.StatusName, _settingStatus,
                        StringComparison.CurrentCultureIgnoreCase));
                _suggestion.OwnerNotes =
                    $"You are right, this is an important topic for developers. We created a resource about it here: <a href='{_urlText} target='_blank'>{_urlText}</a>";
                break;

            case "Watching":
                _suggestion.SuggestionStatus = _statuses.First(s =>
                    string.Equals(s.StatusName, _settingStatus,
                        StringComparison.CurrentCultureIgnoreCase));
                _suggestion.OwnerNotes =
                    "We noticed the interest this suggestion is getting! If more people are interested we may address this topic in an upcoming resource.";
                break;

            case "Upcoming":
                _suggestion.SuggestionStatus = _statuses.First(s =>
                    string.Equals(s.StatusName, _settingStatus,
                        StringComparison.CurrentCultureIgnoreCase));
                _suggestion.OwnerNotes =
                    "Great suggestion! We have a resource in the pipeline to address this topic.";
                break;

            case "Dismissed":
                _suggestion.SuggestionStatus = _statuses.First(s =>
                    string.Equals(s.StatusName, _settingStatus,
                        StringComparison.CurrentCultureIgnoreCase));
                _suggestion.OwnerNotes =
                    "Sometimes a good idea doesn't fit within our scope and vision. This is one of those ideas.";
                break;

            default:
                return;
        }

        _settingStatus = null;
        await SuggestionData.UpdateSuggestion(_suggestion);
    }

    private void ClosePage()
    {
        NavManager.NavigateTo("/");
    }

    private string GetUpvoteTopText()
    {
        if (_suggestion.UserVotes?.Count > 0)
            return _suggestion.UserVotes.Count.ToString("00");
        else if (_suggestion.Author.Id == _loggedInUser?.Id)
            return "Awaiting";
        else
            return "Click To";
    }

    private string GetUpvoteBottomText()
    {
        return _suggestion.UserVotes?.Count > 1 ? "Upvotes" : "Upvote";
    }

    private async Task VoteUp()
    {
        if (_loggedInUser is not null)
        {
            if (_suggestion.Author.Id == _loggedInUser.Id)
            {
                // Can't vote on your own suggestion
                return;
            }

            if (_suggestion.UserVotes.Add(_loggedInUser.Id) == false)
                _suggestion.UserVotes.Remove(_loggedInUser.Id);

            await SuggestionData.UpvoteSuggestion(_suggestion.Id, _loggedInUser.Id);
        }
        else
        {
            NavManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
        }
    }

    private string GetVoteClass()
    {
        if (_suggestion.UserVotes is null || _suggestion.UserVotes.Count == 0)
            return "suggestion-detail-no-votes";

        return _suggestion.UserVotes.Contains(_loggedInUser?.Id)
            ? "suggestion-detail-voted"
            : "suggestion-detail-not-voted";
    }

    private string GetStatusClass()
    {
        if (_suggestion?.SuggestionStatus is null)
        {
            return "suggestion-detail-status-none";
        }

        var output = _suggestion.SuggestionStatus.StatusName switch
        {
            "Completed" => "suggestion-detail-status-completed",
            "Watching" => "suggestion-detail-status-watching",
            "Upcoming" => "suggestion-detail-status-upcoming",
            "Dismissed" => "suggestion-detail-status-dismissed",
            _ => "suggestion-detail-status-none"
        };

        return output;
    }
}