namespace Application.ResponseMessage
{
    public static class MessageEvent
    {
        //Event Message response
        public const string StartEndTimeValidation = "Start date must after current time 12 hours and end date must after start date 30 mins!!";
        public const string StarTimeValidation = "Start date must after current time 12 hours and do not exceed 4 month from current time";
        public const string UpdateStartEndTimeValidation = "Start date must after event created time at least 6 hours and end date must after start date 30 mins!!";
        public const string EventIdNotExist = "EventId not exist!";
        public const string GetAllEvent = "Get all events!";
        public const string UserParticipatedEvent = "User participated events!";
        public const string PopularLocation = "Popular locations leaderboard";
        public const string PopularOrganizers = "Popular organizers(event creators)";
        public const string UserNotAllow = "User not allowed to create event";
        public const string LocationCoordInvalid = "Location coordinate must follow pattern: @\"^-?\\d+(?:\\.\\d+)?, *-?\\d+(?:\\.\\d+)?$\"";
        public const string TagLimitValidation = "Event's maximum tags is 5!";
        public const string UpdateEventWithStatus = "Can only update Event with status NotYet";
        public const string OnlyHostCanUpdateEvent = "Only host can update this event";
        public const string YouAreNotOwnerOfThisEvent = "You are not owner of this event";
        public const string OnlyHostCanCreateQuiz = "Only host can create and delete quiz";
        public const string QuizNotFound = "Quiz not found";
        public const string TheTimeOfAdvertisingIsStillRemain = "The time of advertising is still remain";
    }
}
