///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.Tests.Engine.Base
{
    /// <summary>
    /// Template of a method used to backup device information before the test. 
    /// </summary>
    /// <typeparam name="T">Type of the class/structure to be retrieved and then passed to 
    /// method for restoring.</typeparam>
    /// <returns>Data to be used to restore device state.</returns>
    public delegate T Backup<T>();
    
}
