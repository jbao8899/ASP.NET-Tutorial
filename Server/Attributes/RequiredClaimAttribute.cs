namespace Server.Attributes
{
    // Manual authorization, probably don't copy this

    [AttributeUsage (AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredClaimAttribute : Attribute
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }

        public RequiredClaimAttribute(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
