using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Definitions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProfileDefinitionAttribute : Attribute
    {
        public ProfileDefinitionAttribute(string name, string scope, ProfileVersionStatus status)
        {
            this.m_MainName = name;
            this.Scope = scope;
            this.ProfileVersionStatus = status;
        }

        private readonly string m_MainName;
        public string Name 
        { 
            get { return string.Format("{0}{1}", m_MainName, ProfileVersionStatus.ReleaseCandidate == ProfileVersionStatus ? "(Release Candidate)" : ""); }
        }

        public string Scope { get; protected set; }

        public ProfileVersionStatus ProfileVersionStatus { get; protected set; }
    }

    public static class ProfileDefinitionAttributeUtils
    {
        private static ProfileDefinitionAttribute FirstAttribute(Type T)
        {
            var customAttributes = T.GetCustomAttributes(typeof (ProfileDefinitionAttribute), false).OfType<ProfileDefinitionAttribute>();

            return customAttributes.FirstOrDefault();
        }

        public static ProfileVersionStatus GetProfileVersionStatus(this Type T)
        {
            var attr = FirstAttribute(T);

            return (null == attr) ? ProfileVersionStatus.Undefined : attr.ProfileVersionStatus;
        }

        public static ProfileVersionStatus GetProfileVersionStatus(this IProfileDefinition T)
        {
            return GetProfileVersionStatus(T.GetType());
        }

        public static string GetProfileName(this Type T)
        {
            var attr = FirstAttribute(T);

            return (null == attr) ? null : attr.Name;
        }

        public static string GetProfileName(this IProfileDefinition T)
        {
            return GetProfileName(T.GetType());
        }

        public static string GetProfileScope(this Type T)
        {
            var attr = FirstAttribute(T);

            return (null == attr) ? null : attr.Scope;
        }

        public static string GetProfileScope(this IProfileDefinition T)
        {
            return GetProfileScope(T.GetType());
        }
    }
}
