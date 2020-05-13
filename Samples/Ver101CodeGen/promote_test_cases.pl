#!/usr/bin/perl

 # chapter names for attributes
 $chapters{"         Discovery Tests\n"} = "Device Discovery";
 $chapters{"         Device Tests\n"} = "Device Management";
 $chapters{"         Media Tests\n"} = "Media";
 $chapters{"         RTSP tests\n"} = "Real Time Streaming";


 # function returning types and vals
 $retvals{"GetCapabilities"} = "Capabilities capabilities = ";
 $retvals{"GetHostname"} = "HostnameInformation hostnameInformation = ";
 $retvals{"GetDNS"} = "DNSInformation dnsInformation = ";
 $retvals{"GetNTP"} = "NTPInformation ntpInformation = ";
 $retvals{"GetScopes"} = "ScopesType Scope = ";
 $retvals{"Probe"} = "ProbeMatchesType match = ";
 $retvals{"GetSystemDateAndTime"} = "SystemDateTime SystemDateAndTime = ";
# $retvals{""} = " = ";
# $retvals{""} = " = ";
# $retvals{""} = " = ";
# $retvals{""} = " = ";

 open (ifile, "../../References/ONVIF_Tester/ONVIF_TestCases/OnvifTests.cs");
 @filecont = <ifile>;
 close ifile;
 $len = @filecont;

 open (ofile, ">GenTestCases.cs");
 open (oofile, ">GenProxies.cs");

 $chapternumber = 2;

 for ($line = 0; $line < $len; $line++)
 {
  $tmp = $filecont[$line];
  if ($tmp=~s/#region//) 
  { 
   &proceedregion();                # one region - one chapter, one test type
  };
 }
 close oofile;
 close ofile;


sub proceedregion()
{
 $chapternumber++;
 $testnumber = 0;
 $region = $chapters{$tmp};
 #print "start at $line $region\n"; 
 for (; $line < $len; $line++)
 {
  $tmp = $filecont[$line];
  if ($tmp=~s/#endregion//) 
  { 
   #print "end at $line\n"; 
   return;
  };
  if ($tmp=~s/public string//)      # function signature
  { 
   &proceedfunction()
  };
 }
}

sub proceedfunction()
{
 $testnumber++;
 (@tempfunc) = split(/, /, $filecont[$line-4]);
 $funcname = $tempfunc[1];
 $funcname =~s/ test\n//;
 $funcname =~s/ Inquery//;
 #print "using $funcname\n";
 &printattributes()                  # printing attributes
 &printfuncgen()                     # printing function body
}

sub printattributes()
{
 print ofile "\n\n        [Test (Name =\"$chapternumber.$testnumber NVT \U$funcname\",\n";
 print ofile     "            Path=\"$region\",\n";
 print ofile     "            Version=1.01,\n";
 print ofile     "            RequirementLevel = RequirementLevel.Must)]\n";
}

sub printfuncgen()
{
 $funcnamebis = $funcname." Test";     # decorating functon name
 $funcnamebis =~ s/\b(\w)/\U$1/g;
 $funcnamebis =~ s/ //g;

 print ofile     "        public void $funcnamebis()\n";
 print ofile     "        {\n";
 print ofile     "            try{\n";
 print ofile     "            {\n";
 print ofile     "                BeginTest()\n";
 print ofile   "\n";
 &printbody();                         # printing body
 print ofile   "\n                EndTest(Tests.Common.Trace.TestStatus.Passed);\n";
 print ofile     "            }\n";
 print ofile     "            catch (StopEventException)\n            {\n\n            }\n";
 print ofile     "            catch (Exception ex)\n            {\n                StepFailed(ex);\n                TestFailed(ex);\n            }\n";
 print ofile   "\n        }\n";
}

sub printbody()
{
 for ($lline = $line+1; $lline < $len; $lline++)
 {
  $tmp = $filecont[$lline];


  if ($tmp=~s/#endregion//) 
  { 
   return;                      # end of region is the end of functon
  };
  if ($tmp=~s/public string//) 
  { 
   return;                      # new function is the end of old function
  };

  $tmp=~s/DEFAULT_DEVICE_TYPE/\"dn:NetworkVideoTransmitter\"/;
  
  # try-catch for negative tests

  if ($tmp=~/wrongTYPE/)
  {
   print ofile     "                try { \n";
   $nextlinecatch = 1;
  }
  if ($tmp=~/BADTYPE/)
  {
   print ofile $tmp;
   print ofile     "                try { \n";
   $nextlinecatch = 2;
  }
  if ($tmp=~/"Test#\$%"/)
  {
   print ofile     "                try { \n";
   $nextlinecatch = 2;
  }
  if ($tmp=~/"10.1.1.255"/)
  {
   print ofile     "                try { \n";
   $nextlinecatch = 6;
  }
  if ($tmp=~/"test#\$%"/)
  {
   print ofile     "                try { \n";
   $nextlinecatch = 6;
  }

  # catching parameter
  if ($tmp=~ s/Parameters\.Temporary_String = (.+);/$1/g)
  {
   $TemporaryParam = $1;
  };

  # are we calling IO function?
  (@tempfunc) = split(/Build_/, $tmp);
  if (@tempfunc > 1)
  {
   $call = $tempfunc[1];
   $call =~s/Request//;
   ($method, $rest) = split(/\(/, $call);
   $retval = $retvals{$method};
   &createmethod();

   $call =~ s/Parameters.Temporary_String/$TemporaryParam/g;
   print ofile     "                $retval$call";
  }
  else
  {
   #some staff for parameters
   if ($tmp=~/DNS_Server/)
   {
    $tmp =~ s/Parameters.Temporary_String/$TemporaryParam/g;
    print ofile $tmp;
   }
   if ($tmp=~/NTP =/)
   {
    $tmp =~ s/Parameters.Temporary_String/$TemporaryParam/g;
    print ofile $tmp;
   }
   if ($tmp=~/NTP\[0\]/)
   {
    $tmp =~ s/Parameters.Temporary_String/$TemporaryParam/g;
    print ofile $tmp;
   }
  }

  if ($tmp=~/GetHelloResponse/)
  {
   print ofile     "                HelloType Hello = GetHello();\n";
  }

  if ($tmp=~/multicast BYE/)
  {
   print ofile     "                ByeType Bye = GetBye();\n";
  }

  if ($tmp=~/Compare_RemoteDiscovery_ScopesType/)
  {
   print ofile     "                Assert(Hello.Types == \"dn:NetworkVideoTransmitter\", \"Wrong device\", \"Hello Message validation\");\n";
   print ofile     "                Assert(Hello.Scopes != null, \"No Scopes\", \"Hello Message validation\");\n";
   print ofile     "                string[] Scopes = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware,\n".
                   "                                                RemoteDiscovery.Constants.ScopeTypePrefix_Location,\n".
                   "                                                RemoteDiscovery.Constants.ScopeTypePrefix_Name,\n".
                   "                                                RemoteDiscovery.Constants.ScopeTypePrefix_Type};\n";
   print ofile     "                Assert(Hello.Scopes.Text == Scopes, \"Wrong Scopes\", \"Hello Message validation\");\n";
  }

  if ($tmp=~/if \(Parameters.Scopes == null\)/)
  {
   print ofile     "                Assert(Scopes != null, \"No Scopes\", \"GetScopes validation\");\n";
  }


  if ($tmp=~/Scope.Text = new string\[\] { scope.ScopeItem };/)
  {
  }
  else
  {
   if ($tmp=~/Scope.Text = /)    # scopes parameters passing by
   {
    print ofile    "                ScopesType Scope = new ScopesType;\n";
    print ofile $tmp;
   }
  }

  # magic with capabilities asserting
  if ($tmp=~/if \(GCR.Capabilities/)
  {
   $tmp =~ s/Capabilities(.+)/$1/g;
   $abody = "!(capabilities".$1;
   $domain = $1;
   $back = $1;
   $domain =~ s/.(.+) ==/$1/g;
   if ($back NE $1)
   {
    $domain = $1;
   }
   else
   {
    $domain = "";
   }
   $domain = "\"Check ".$domain." capabilities\"";
   $wrongassert = $filecont[$lline+2];
   $wrongassert =~ s/\"(.+)\"/$1/g;
   $wrongassert = "\"$1\"";
   print ofile     "                Assert($abody, $wrongassert, $domain);\n";
  }

  if ($tmp=~/if \(GHR.HostnameInformation.Name/)
  {
   print ofile     "                Assert(HostnameInformation.Name.Equals($TemporaryParam), \"Name not set\", \"Host name validation\");\n";
  }


  # magic with positive DNS asserting
  if ($tmp=~/if \(GDR.DNSInformation.(.+)== null/g)
  {
   $tmp =~ s/DNSInformation(.+)/$1/g;
   $abody = "!(dnsInformation".$1;
   $domain = $1;
   $back = $1;
   $domain =~ s/.(.+) ==/$1/g;
   if ($back NE $1)
   {
    $domain = $1;
   }
   else
   {
    $domain = "";
   }
   $domain = "\"Check ".$domain." settings\"";
   $wrongassert = $filecont[$lline+2];
   $wrongassert =~ s/\"(.+)\"/$1/g;
   $wrongassert = "\"$1\"";
   print ofile     "                Assert($abody, $wrongassert, $domain);\n";

   if ($tmp=~/DNSManual/)
   {
    print ofile    "                bool foundEntry = false;\n";
    print ofile    "                for (int x = 0; x < DNSInformation.DNSManual.Length; x++)\n";
    print ofile    "                {\n";
    print ofile    "                    if ((dnsInformation.DNSManual[x].IPv4Address.Equals($TemporaryParam)) &&\n";
    print ofile    "                        (dnsInformation.DNSManual[x].Type == DeviceManagement.IPType.IPv4))\n";
    print ofile    "                         foundEntry = true;\n";
    print ofile    "                }\n";
    print ofile    "                Assert(foundEntry, \"IP Address\", $domain);\n";
   }

  }



  # magic with negative DNS asserting
  if ($tmp=~/if \(\(GDR.DNSInformation.DNSManual != null\)\)/g)
  {
   $tmp =~ s/DNSInformation(.+)/$1/g;
   $abody = "!(dnsInformation".$1;
   $domain = $1;
   $back = $1;
   $domain =~ s/.(.+) !=/$1/g;
   if ($back NE $1)
   {
    $domain = $1;
   }
   else
   {
    $domain = "";
   }
   $domain = "\"Check ".$domain." settings\"";
   $wrongassert = $filecont[$lline+2];
   $wrongassert =~ s/\"(.+)\"/$1/g;
   $wrongassert = "\"$1\"";
   print ofile     "                Assert($abody, $wrongassert, $domain);\n";

   if ($tmp=~/DNSManual/)
   {
    print ofile    "                bool foundEntry = false;\n";
    print ofile    "                for (int x = 0; x < DNSInformation.DNSManual.Length; x++)\n";
    print ofile    "                {\n";
    print ofile    "                    if ((dnsInformation.DNSManual[x].IPv4Address.Equals($TemporaryParam)) &&\n";
    print ofile    "                        (dnsInformation.DNSManual[x].Type == DeviceManagement.IPType.IPv4))\n";
    print ofile    "                         foundEntry = true;\n";
    print ofile    "                }\n";
    print ofile    "                Assert(!foundEntry, \"Wrong IP Address set\", $domain);\n";
   }

  }


  # magic with NTP asserting
  if ($tmp=~/if \(GNTPR.NTPInformation/)
  {
   $tmp =~ s/NTPInformation(.+)/$1/g;
   $abody = "(ntpInformation".$1;
   $domain = $1;
   $back = $1;
   $domain =~ s/.(.+) ==/$1/g;
   if ($back NE $1)
   {
    $domain = $1;
   }
   else
   {
    $domain = "";
   }
   $domain = "\"Check ".$domain." settings\"";
   $wrongassert = $filecont[$lline+2];
   $wrongassert =~ s/\"(.+)\"/$1/g;
   $wrongassert = "\"$1\"";
   print ofile     "                Assert($abody, $wrongassert, $domain);\n";
  }


  # magic with DateTime asserting
  if ($tmp=~/if \(GSDTR.SystemDateAndTime/)
  {
   $tmp =~ s/SystemDateAndTime(.+)/$1/g;
   $abody = "(SystemDateAndTime".$1;
   $domain = $1;
   $back = $1;
   $domain =~ s/.(.+) ==/$1/g;
   if ($back NE $1)
   {
    $domain = $1;
   }
   else
   {
    $domain = "";
   }
   $domain = "\"Check ".$domain." settings\"";
   $wrongassert = $filecont[$lline+2];
   $wrongassert =~ s/\"(.+)\"/$1/g;
   $wrongassert = "\"$1\"";
   print ofile     "                Assert($abody, $wrongassert, $domain);\n";
  }



  #catch epilogue
  $nextlinecatch--;
  if ($nextlinecatch == 0)
  {
   print ofile     "                }\n";
   print ofile     "                catch (FaultException exc) {\n";
   print ofile     "                    StepPassed(); };\n";
   print ofile     "                catch (TimeoutException exc) {\n";
   print ofile     "                    StepPassed(); };\n";
  }


 }
}

sub createmethod()
{
 $methods{$method}++;
 if ($methods{$method} > 1)  { return; };
 $temp = $method;
 if ($temp=~s/Get//)
 {
  print oofile     "    public $method()\n";
 }
 else
 {
  print oofile     "    public void $method(xxx)\n";
 }
 
}