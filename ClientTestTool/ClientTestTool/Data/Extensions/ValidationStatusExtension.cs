using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Extensions
{
  public static class ValidationStatusExtension
  {
    public static bool Passed(this ValidationStatus status)
    {
      return ValidationStatus.Passed     == status ||
             ValidationStatus.HttpDigest == status ||
             ValidationStatus.RtspDigest == status;
    }

    public static bool Failed(this ValidationStatus status)
    {
      return !Passed(status);
    }
  }
}
