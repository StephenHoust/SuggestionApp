using SuggestionAppUI.Models;

namespace SuggestionAppUI.Pages;

public partial class Create
{
    private CreateSuggestionModel _suggestion = new();
    private List<CategoryModel> _categories;
    private UserModel _loggedInUser;

    protected override async Task OnInitializedAsync()
    {
        _categories = await CategoryData.GetAllCategories();
        _loggedInUser = await AuthProvider.GetUserFromAuth(UserData);
    }

    private void ClosePage()
    {
        NavManager.NavigateTo("/");
    }

    private async Task CreateSuggestion()
    {
        SuggestionModel s = new()
        {
            Suggestion = _suggestion.Suggestion,
            Description = _suggestion.Description,
            Author = new BasicUserModel(_loggedInUser),
            Category = _categories.FirstOrDefault(c => c.Id == _suggestion.CategoryId)
        };

        if (s.Category is null)
        {
            _suggestion.CategoryId = "";
            return;
        }

        await SuggestionData.CreateSuggestion(s);
        _suggestion = new();
        ClosePage();
    }
}