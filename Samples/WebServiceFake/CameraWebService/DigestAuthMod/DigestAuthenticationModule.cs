// DigestAuthenticationModule.cs
//
// This HTTP Module is designed to support HTTP Digest authentication,
// without using the built-in IIS implementation.  The IIS implementation
// can only authenticate against the Active Directory store; but in
// many applications, one would rather authenticate against a separate
// database.
//
// The implementation was designed particularly for web services, but 
// should suffice for any web application.  For a non-service application,
// one obvious change would be to support a redirection on a failed login,
// to display a more friendly message to the user.
//
// The credential store in this version is a simple XML file (sample in
// users.xml).  In a real application, you would probably want to modify
// this to use a database or LDAP store.  An easy way to do this would be
// to derive from Rassoc.DigestAuthenticationModule and override the 
// AuthenticateUser function.
//
// Nonce selection in this implementation is relatively unsophisticated,
// but fast.  The nonce is based on the current system time, and will
// remain valid for 60 seconds.  Applications may want to override the 
// nonce selection and validation functionality to better guard against 
// classic attacks.
//
// Usage:
//
// (Assuming ASP.NET) 
// 1. Copy DigestAuthMod.dll to your ASP.NET application's bin directory.
// 2. Make the following changes to your web.config file (within <system.web>):
//     - change authentication line to: <authentication mode="None" /> 
//     - add an authorization section if you wish, such as
//         <authorization>
//           <deny users="?" />
//         </authorization>
//     - add the following lines:
//         <httpModules>
//           <add name="DigestAuthenticationModule" 
//                type="Rassoc.Samples.DigestAuthenticationModule,DigestAuthMod" />
//         </httpModules>   
// 3. Add the following to your web.config (within <configuration>):
//         <appSettings>
//           <add key="Rassoc.Samples.DigestAuthenticationModule_Realm" value="RassocDigestSample" />
//           <add key="Rassoc.Samples.DigestAuthenticationModule_UserFileVpath" value="~/users.xml" />
//         </appSettings>
//
//
// Greg Reinacker
// Reinacker & Associates, Inc.
// http://www.rassoc.com
// http://www.rassoc.com/gregr/weblog/
//

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Xml;

namespace Rassoc.Samples
{
	public class DigestAuthenticationModule : IHttpModule
	{
		public DigestAuthenticationModule()
		{
		}

		public void Dispose()
		{
		}

		public void Init(HttpApplication application)
		{
			application.AuthenticateRequest += new EventHandler(this.OnAuthenticateRequest);
			application.EndRequest += new EventHandler(this.OnEndRequest);
		}

		public void OnAuthenticateRequest(object source, EventArgs eventArgs)
		{
			HttpApplication app = (HttpApplication) source;

			string authStr = app.Request.Headers["Authorization"];

			if (authStr == null || authStr.Length == 0)
			{
				// No credentials; anonymous request
				return;
			}

			authStr = authStr.Trim();
			if (authStr.IndexOf("Digest",0) != 0)
			{
				// Don't understand this header...we'll pass it along and 
				// assume someone else will handle it
				return;
			}

			authStr = authStr.Substring(7);

			ListDictionary reqInfo = new ListDictionary();

			string[] elems = authStr.Split(new char[] {','});
			foreach (string elem in elems)
			{
				// form key="value"
				string[] parts = elem.Split(new char[] {'='}, 2);
				string key = parts[0].Trim(new char[] {' ','\"'});
				string val = parts[1].Trim(new char[] {' ','\"'});
				reqInfo.Add(key,val);
			}

			string username = (string)reqInfo["username"];
			string password = "";
			string[] roles;
			bool bOk = GetPasswordAndRoles(app, username, out password, out roles);
			if (!bOk)
			{
				// Invalid username; deny access
				DenyAccess(app);
				return;
			}

			string realm = ConfigurationSettings.AppSettings["Rassoc.Samples.DigestAuthenticationModule_Realm"];

			// calculate the Digest hashes

			// A1 = unq(username-value) ":" unq(realm-value) ":" passwd
			string A1 = String.Format("{0}:{1}:{2}",(string)reqInfo["username"],realm,password);

            System.Diagnostics.Debug.WriteLine(string.Format("A1: {0}", A1));

			// H(A1) = MD5(A1)
			string HA1 = GetMD5HashBinHex(A1);

            System.Diagnostics.Debug.WriteLine(string.Format("HA1: {0}", HA1));

			// A2 = Method ":" digest-uri-value
			string A2 = String.Format("{0}:{1}",app.Request.HttpMethod,(string)reqInfo["uri"]);

            System.Diagnostics.Debug.WriteLine(string.Format("A2: {0}", A2));

			// H(A2)
			string HA2 = GetMD5HashBinHex(A2);

            System.Diagnostics.Debug.WriteLine(string.Format("HA2: {0}", HA2));

			// KD(secret, data) = H(concat(secret, ":", data))
			// if qop == auth:
			// request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
			//                              ":" nc-value
			//                              ":" unq(cnonce-value)
			//                              ":" unq(qop-value)
			//                              ":" H(A2)
			//                            ) <">
			// if qop is missing,
			// request-digest  = <"> < KD ( H(A1), unq(nonce-value) ":" H(A2) ) > <">

			string unhashedDigest;
			if (reqInfo["qop"] != null)
			{
				unhashedDigest = String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
					HA1,
					(string)reqInfo["nonce"],
					(string)reqInfo["nc"],
					(string)reqInfo["cnonce"],
					(string)reqInfo["qop"],
					HA2);
			}
			else
			{
				unhashedDigest = String.Format("{0}:{1}:{2}",
					HA1,
					(string)reqInfo["nonce"],
					HA2);
			}

            System.Diagnostics.Debug.WriteLine(string.Format("unhashedDigest: {0}", unhashedDigest));

			string hashedDigest = GetMD5HashBinHex(unhashedDigest);

            System.Diagnostics.Debug.WriteLine(string.Format("hashedDigest: {0}", hashedDigest));

			bool isNonceStale = !IsValidNonce((string)reqInfo["nonce"]);
			app.Context.Items["staleNonce"] = isNonceStale;

			if (((string)reqInfo["response"] == hashedDigest) && (!isNonceStale))
			{
				app.Context.User = new GenericPrincipal(new GenericIdentity(username, "Rassoc.Samples.Digest"),roles);
			}
			else
			{
				// Invalid credentials or stale nonce; deny access
				DenyAccess(app);
				return;
			}
		}

		public void OnEndRequest(object source, EventArgs eventArgs)
		{
			// We add the WWW-Authenticate header here, so if an authorization 
			// fails elsewhere than in this module, we can still request authentication 
			// from the client.

			HttpApplication app = (HttpApplication) source;
			if (app.Response.StatusCode == 401)
			{
				string realm = ConfigurationSettings.AppSettings["Rassoc.Samples.DigestAuthenticationModule_Realm"];
				string nonce = GetCurrentNonce();
				bool isNonceStale = false;
				object staleObj = app.Context.Items["staleNonce"];
				if (staleObj != null)
					isNonceStale = (bool)staleObj;

				StringBuilder challenge = new StringBuilder("Digest");
				challenge.Append(" realm=\"");
				challenge.Append(realm);
				challenge.Append("\"");
				challenge.Append(", nonce=\"");
				challenge.Append(nonce);
				challenge.Append("\"");
				challenge.Append(", opaque=\"0000000000000000\"");
				challenge.Append(", stale=");
				challenge.Append(isNonceStale ? "true" : "false");
				challenge.Append(", algorithm=MD5");
				challenge.Append(", qop=\"auth\"");

				app.Response.AppendHeader("WWW-Authenticate", challenge.ToString());
				app.Response.StatusCode = 401;
			}
		}

		private void DenyAccess(HttpApplication app)
		{
			app.Response.StatusCode = 401;
			app.Response.StatusDescription = "Access Denied";

			// Write to response stream as well, to give user visual 
			// indication of error during development
			app.Response.Write("401 Access Denied");

			app.CompleteRequest();
		}

		private string GetMD5HashBinHex(string val)
		{
			Encoding enc = new ASCIIEncoding();
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] bHA1 = md5.ComputeHash(enc.GetBytes(val));
			string HA1 = "";
			for (int i = 0 ; i < 16 ; i++)
				HA1 += String.Format("{0:x02}",bHA1[i]);
			return HA1;
		}

		protected virtual string GetCurrentNonce()
		{
			// This implementation will create a nonce which is the text 
			// representation of the current time, plus one minute.  The
			// nonce will be valid for this one minute.
			DateTime nonceTime = DateTime.Now + TimeSpan.FromMinutes(1);
			string expireStr = nonceTime.ToString("G");

			Encoding enc = new ASCIIEncoding();
			byte[] expireBytes = enc.GetBytes(expireStr);
			string nonce = Convert.ToBase64String(expireBytes);

			// nonce can't end in '=', so trim them from the end
			nonce = nonce.TrimEnd(new Char[] {'='});
			return nonce;
		}

		protected virtual bool IsValidNonce(string nonce)
		{
			DateTime expireTime;

			// pad nonce on the right with '=' until length is a multiple of 4
			int numPadChars = nonce.Length % 4;
			if (numPadChars > 0)
				numPadChars = 4 - numPadChars;
			string newNonce = nonce.PadRight(nonce.Length + numPadChars, '=');

			try
			{
				byte[] decodedBytes = Convert.FromBase64String(newNonce);
				string expireStr = new ASCIIEncoding().GetString(decodedBytes);
				expireTime = DateTime.Parse(expireStr);
			}
			catch (FormatException)
			{
				return false;
			}

			return (DateTime.Now <= expireTime);
		}

		protected virtual bool GetPasswordAndRoles(HttpApplication app, string username, out string password, out string[] roles)
		{
			// Note - this implementation reads users from an XML file, whose location
			// is specified in the web.config.  This is here purely to provide a quick
			// example.  In a real application, you would probably want to override 
			// this function to validate credentials against a database or LDAP store.

			roles = null;
			password = null;

			string userFileVpath = ConfigurationSettings.AppSettings["Rassoc.Samples.DigestAuthenticationModule_UserFileVpath"];
			string userFileName = app.Request.MapPath(userFileVpath);

			XmlDocument userDoc = new XmlDocument();
			userDoc.Load(userFileName);

			string xPath = String.Format("/users/user[@name='{0}']",username);
			XmlNode theUser = userDoc.SelectSingleNode(xPath);
			
			if (theUser != null)
			{
				password = theUser.Attributes["password"].Value;

				XmlNodeList roleNodes = theUser.SelectNodes("role");
				int numRoles = roleNodes.Count;
				roles = new string[roleNodes.Count];
				if (numRoles > 0)
				{
					for (int i = 0 ; i < numRoles ; i++)
					{
						XmlNode roleNode = roleNodes[i];
						roles[i] = roleNode.Attributes["name"].Value;
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
