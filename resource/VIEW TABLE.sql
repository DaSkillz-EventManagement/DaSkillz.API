CREATE VIEW EventStatistics AS
SELECT
    e.EventID,
    COUNT(DISTINCT CASE WHEN p.RoleEventID = 4 THEN p.UserID END) AS totalVisitor,
    COUNT(DISTINCT CASE WHEN p.RoleEventID = 3 THEN p.UserID END) AS totalCheckinStaff,
    COUNT(DISTINCT CASE WHEN s.Status = 'Confirmed' and s.IsSponsored = 1 THEN s.UserID END) AS totalSponsor,
    COUNT(DISTINCT CASE WHEN p.CheckedIn IS NOT NULL THEN p.UserID END) AS totalCheckedIn,
    COUNT(DISTINCT CASE WHEN p.IsCheckedMail = 1 THEN p.UserID END)  AS totalCheckedMail,
    COUNT(DISTINCT f.UserID) AS totalFeedback,
    CAST(ROUND(AVG(CAST(CASE WHEN f.Rating IS NOT NULL THEN f.Rating ELSE 0 END AS FLOAT)), 1) AS DECIMAL(4, 1)) AS averageStar,
    COUNT(DISTINCT CASE WHEN f.Rating = 1 THEN f.UserID END) AS totalFbOneStar,
    COUNT(DISTINCT CASE WHEN f.Rating = 2 THEN f.UserID END) AS totalFbTwoStar,
    COUNT(DISTINCT CASE WHEN f.Rating = 3 THEN f.UserID END) AS totalFbThreeStar,
    COUNT(DISTINCT CASE WHEN f.Rating = 4 THEN f.UserID END) AS totalFbFourStar,
    COUNT(DISTINCT CASE WHEN f.Rating = 5 THEN f.UserID END) AS totalFbFiveStar,
    SUM(DISTINCT CASE WHEN s.IsSponsored = 1 AND TRY_CAST(s.Amount AS DECIMAL(18, 2)) IS NOT NULL THEN TRY_CAST(s.Amount AS DECIMAL(18, 2)) ELSE 0 END) AS totalSponsored,
    SUM(DISTINCT CASE WHEN s.IsSponsored = 1 AND TRY_CAST(s.Amount AS DECIMAL(18, 2)) IS NOT NULL THEN TRY_CAST(s.Amount AS DECIMAL(18, 2)) ELSE 0 END) + 
    ISNULL(
        (SELECT SUM(TRY_CAST(t.Amount AS DECIMAL(18, 2)))
         FROM Transactions t
         WHERE t.EventID = e.EventID
           AND NOT EXISTS (
               SELECT 1
               FROM SponsorEvent se
               WHERE se.EventID = e.EventID
                 AND t.SubscriptionType = 'Advertise'
                 AND se.UserID = t.UserId
                 AND se.Status = 1
           )
        ), 0
    ) AS revenue
FROM
    Events e
    LEFT JOIN Participant p ON e.EventID = p.EventID
    LEFT JOIN Feedback f ON e.EventID = f.EventID
    LEFT JOIN SponsorEvent s ON e.EventID = s.EventID
GROUP BY
    e.EventID;
