using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            if (!customAttributes.Any())
                throw new ArgumentException("T: type has no ProfileDefinition attribute specified");

            return customAttributes.First();
        }

        public static ProfileVersionStatus GetProfileVersionStatus(this Type T)
        {
            return FirstAttribute(T).ProfileVersionStatus;
        }

        public static ProfileVersionStatus GetProfileVersionStatus(this IProfileDefinition T)
        {
            return GetProfileVersionStatus(T.GetType());
        }

        public static string GetProfileName(this Type T)
        {
            return FirstAttribute(T).Name;
        }

        public static string GetProfileName(this IProfileDefinition T)
        {
            return GetProfileName(T.GetType());
        }

        public static string GetProfileScope(this Type T)
        {
            return FirstAttribute(T).Scope;
        }

        public static string GetProfileScope(this IProfileDefinition T)
        {
            return GetProfileScope(T.GetType());
        }

        public static bool ScopesMatched(this Type T, IEnumerable<string> scopes)
        {
            var method = T.GetMethod("ScopesMatched", BindingFlags.Public | BindingFlags.Static);
            if (null != method)
                return (bool)method.Invoke(T, new [] {scopes});

            return scopes.Contains(T.GetProfileScope());
        }

        public static bool ScopesMatched(this IProfileDefinition T, IEnumerable<string> scopes)
        {
            return ScopesMatched(T.GetType(), scopes);
        }
    }
}
