namespace Application.ResponseMessage
{
    namespace Application.ResponseMessage
    {
        public static class MessageEvent
        {
            // Start date must be at least 12 hours after the current time, and the end date must be at least 30 minutes after the start date
            public const string StartEndTimeValidation = "START_END_TIME_VALIDATION";

            // Start date must be at least 12 hours after the current time, and should not exceed 4 months from the current time
            public const string StarTimeValidation = "START_TIME_VALIDATION";

            // Start date must be at least 6 hours after the event creation time, and the end date must be at least 30 minutes after the start date
            public const string UpdateStartEndTimeValidation = "UPDATE_START_END_TIME_VALIDATION";

            // The event ID does not exist in the system
            public const string EventIdNotExist = "EVENT_ID_NOT_EXIST";

            // A successful message when all events are fetched
            public const string GetAllEvent = "GET_ALL_EVENTS";

            // A successful message indicating that the user participated in certain events
            public const string UserParticipatedEvent = "USER_PARTICIPATED_EVENTS";

            // Popular locations leaderboard message response
            public const string PopularLocation = "POPULAR_LOCATIONS_LEADERBOARD";

            // Popular organizers (event creators) leaderboard message response
            public const string PopularOrganizers = "POPULAR_ORGANIZERS";

            // User is not allowed to create an event
            public const string UserNotAllow = "USER_NOT_ALLOWED_TO_CREATE_EVENT";

            // Location coordinate must follow a specific pattern for validation
            public const string LocationCoordInvalid = "INVALID_LOCATION_COORDINATE_PATTERN";

            // Maximum number of tags allowed for an event is 5
            public const string TagLimitValidation = "TAG_LIMIT_VALIDATION";

            // Can only update the event if its status is 'NotYet'
            public const string UpdateEventWithStatus = "CAN_ONLY_UPDATE_EVENT_WITH_STATUS_NOT_YET";

            // Only the host of the event can update the event
            public const string OnlyHostCanUpdateEvent = "ONLY_HOST_CAN_UPDATE_EVENT";

            // You are not the owner of this event
            public const string YouAreNotOwnerOfThisEvent = "NOT_OWNER_OF_EVENT";

            // Only the host of the event can create a quiz for the event
            public const string OnlyHostCanCreateQuiz = "ONLY_HOST_CAN_CREATE_QUIZ";

            // The specified quiz could not be found
            public const string QuizNotFound = "QUIZ_NOT_FOUND";

            // There is still remaining time for the current advertising period
            public const string TheTimeOfAdvertisingIsStillRemain = "ADVERTISING_TIME_REMAINING";
        }
    }

}
