﻿namespace SuggestionAppLibrary.DataAccess;

public interface ISuggestionData
{
    Task<List<SuggestionModel>> GetAllSuggestions();
    Task<List<SuggestionModel>> GetUserSuggestions(string userId);
    Task<List<SuggestionModel>> GetAllApprovedSuggestions();
    Task<SuggestionModel> GetSuggestion(string id);
    Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApproval();
    Task UpdateSuggestion(SuggestionModel suggestion);
    Task UpvoteSuggestion(string suggestionId, string userId);
    Task CreateSuggestion(SuggestionModel suggestion);
}