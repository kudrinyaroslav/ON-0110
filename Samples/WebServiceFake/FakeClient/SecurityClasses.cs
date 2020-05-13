using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;

public class UsernameClientAssertion : SecurityPolicyAssertion
{
    public string username;
    public string password;

    public UsernameClientAssertion(string _username, string _password)
    {
        this.username = _username;
        this.password = _password;
    }

    public override SoapFilter
           CreateClientOutputFilter(FilterCreationContext context)
    {
        return new ClientOutputFilter(this, context);
    }

    public override SoapFilter
           CreateClientInputFilter(FilterCreationContext context)
    {
        // we don't provide ClientInputFilter
        return null;
    }

    public override SoapFilter
           CreateServiceInputFilter(FilterCreationContext context)
    {
        // we don't provide any processing for web service side
        return null;
    }

    public override SoapFilter
           CreateServiceOutputFilter(FilterCreationContext context)
    {
        // we don't provide any processing for web service side
        return null;
    }
}

class ClientOutputFilter : SendSecurityFilter
{
    UsernameClientAssertion parentAssertion;
    FilterCreationContext filterContext;

    public ClientOutputFilter(UsernameClientAssertion parentAssertion,
                              FilterCreationContext filterContext)
        : base(parentAssertion.ServiceActor, false, parentAssertion.ClientActor)
    {
        this.parentAssertion = parentAssertion;
        this.filterContext = filterContext;
    }

    public override void SecureMessage(SoapEnvelope envelope, Security security)
    {
        UsernameToken userToken = new UsernameToken(
            parentAssertion.username,
            parentAssertion.password,
            PasswordOption.SendHashed);
        // we don't send password over network
        // but we just use username/password to sign/encrypt message

        // Add the token to the SOAP header.
        security.Tokens.Add(userToken);

    }
}
