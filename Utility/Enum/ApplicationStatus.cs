namespace Utility.Enum
{
    public enum ApplicationStatus
    {
        /// <summary>
        /// InComplete means its under applicant's draft mode
        /// </summary>
        Incomplete = 1,
        /// <summary>
        /// submitted by applicant
        /// </summary>
        Submitted = 2, 
        Reviewed = 3,
        Supervised = 4,
        Approved = 5,
        Rejected = 6,
        Discarded = 7,
        Expired = 8
    }
}
