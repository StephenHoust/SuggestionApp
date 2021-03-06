namespace SuggestionAppUI.Pages;

public partial class SampleData
{
    private bool _categoriesCreated = false;
    private bool _statusesCreated = false;

    private async Task GenerateSampleData()
    {
        UserModel user = new()
        {
            FirstName = "Stephen",
            LastName = "Houst",
            EmailAddress = "Stephen@test.com",
            DisplayName = "Sample Stephen Houst",
            ObjectIdentifier = "abc-123"
        };
        await UserData.CreateUser(user);

        var foundUser = await UserData.GetUserFromAuthentication("abc-123");
        var categories = await CategoryData.GetAllCategories();
        var statuses = await StatusData.GetAllStatuses();

        HashSet<string> votes = new();
        votes.Add("1");
        votes.Add("2");
        votes.Add("3");

        SuggestionModel suggestion = new()
        {
            Author = new BasicUserModel(foundUser),
            Category = categories[0],
            Suggestion = "Our First Suggestion",
            Description = "This is a suggestion created by the sample data generation method."
        };
        await SuggestionData.CreateSuggestion(suggestion);

        suggestion = new SuggestionModel
        {
            Author = new BasicUserModel(foundUser),
            Category = categories[1],
            Suggestion = "Our Second Suggestion",
            Description =
                "This is another suggestion created by the sample data generation method.",
            SuggestionStatus = statuses[0],
            OwnerNotes = "This is the note for the status."
        };
        await SuggestionData.CreateSuggestion(suggestion);

        suggestion = new SuggestionModel
        {
            Author = new BasicUserModel(foundUser),
            Category = categories[2],
            Suggestion = "Our Third Suggestion",
            Description =
                "This is another suggestion created by the sample data generation method.",
            SuggestionStatus = statuses[1],
            OwnerNotes = "This is the note for the status."
        };
        await SuggestionData.CreateSuggestion(suggestion);

        suggestion = new SuggestionModel
        {
            Author = new BasicUserModel(foundUser),
            Category = categories[3],
            Suggestion = "Our Fourth Suggestion",
            Description =
                "This is another suggestion created by the sample data generation method.",
            SuggestionStatus = statuses[2],
            UserVotes = votes,
            OwnerNotes = "This is the note for the status."
        };
        await SuggestionData.CreateSuggestion(suggestion);

        votes.Add("4");

        suggestion = new SuggestionModel
        {
            Author = new BasicUserModel(foundUser),
            Category = categories[4],
            Suggestion = "Our Fifth Suggestion",
            Description =
                "This is another suggestion created by the sample data generation method.",
            SuggestionStatus = statuses[3],
            UserVotes = votes,
            OwnerNotes = "This is the note for the status."
        };
        await SuggestionData.CreateSuggestion(suggestion);
    }

    private async Task CreateCategories()
    {
        var categories = await CategoryData.GetAllCategories();

        if (categories?.Count > 0)
        {
            return;
        }

        CategoryModel cat = new()
        {
            CategoryName = "Courses",
            CategoryDescription = "Full paid courses."
        };
        await CategoryData.CreateCategory(cat);


        cat = new CategoryModel
        {
            CategoryName = "Dev Questions",
            CategoryDescription = "Advice on being a developer."
        };
        await CategoryData.CreateCategory(cat);

        cat = new CategoryModel
        {
            CategoryName = "In-Depth Tutorial",
            CategoryDescription = "A deep-dive video on how to use a topic."
        };
        await CategoryData.CreateCategory(cat);

        cat = new CategoryModel
        {
            CategoryName = "10-Minute Training",
            CategoryDescription = "A quick \"How do I use this?\"."
        };
        await CategoryData.CreateCategory(cat);

        cat = new CategoryModel
        {
            CategoryName = "Other",
            CategoryDescription = "Not sure what category this fits in"
        };
        await CategoryData.CreateCategory(cat);

        _categoriesCreated = true;
    }

    private async Task CreateStatuses()
    {
        var statuses = await StatusData.GetAllStatuses();

        if (statuses?.Count > 0)
        {
            return;
        }

        StatusModel status = new()
        {
            StatusName = "Completed",
            StatusDescription =
                "The suggestion was accepted and the corresponding item was created."
        };
        await StatusData.CreateStatus(status);

        status = new StatusModel
        {
            StatusName = "Watching",
            StatusDescription =
                "The suggestion is interesting. We are watching to se how much interest there is in it."
        };
        await StatusData.CreateStatus(status);

        status = new StatusModel
        {
            StatusName = "Upcoming",
            StatusDescription = "The suggestion was accepted and it will be released soon."
        };
        await StatusData.CreateStatus(status);

        status = new StatusModel
        {
            StatusName = "Dismissed",
            StatusDescription = "The suggestion was not something that we are going to undertake."
        };
        await StatusData.CreateStatus(status);

        _statusesCreated = true;
    }
}