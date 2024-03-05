namespace Shared.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SkipTokenValidationAttribute : Attribute
    {
    }
}
