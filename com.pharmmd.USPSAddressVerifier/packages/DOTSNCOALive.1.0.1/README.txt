******************************************************************

						SERVICE OBJECTS, INC.

******************************************************************
Service: Nuget Package for DOTS NCOA Live

Operation: RunNCOALive

Description:  This package installs the DOTSNCOALive.dll and
its dependancies.  This library wraps up the API calls to our
DOTS NCOA Live web service into best practices
enforcing the highest uptime solution by implementing failover.

IMPLEMENTATION DETAILS: See the following sections - Standard
Implementation

Note: The code being suggested in this file provides one possible
solution using this particular service. There are many other
possible solutions to using this service which may fit a
particular problem . Please contact support@serviceobjects.com for
more information.

******************************************************************

Date Created:    7/26/2016
Last Modified:   7/26/2016

Modified by: D. Van Lant

******************************************************************

WEBSITE
http://www.serviceobjects.com

https://www.serviceobjects.com/products/address-geocoding/ncoa-live

DEVELOPERS GUIDE
https://docs.serviceobjects.com/display/devguide/DOTS+NCOA+Live

FREE TRIAL
http://www.serviceobjects.com/dots-key?wsid=64

SUPPORT EMAIL
support@serviceobjects.com 

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTIBILITY AND/OR
FITNESS FOR A PARTICULAR PURPOSE.

******************************************************************

STANDARD IMPLEMENTATION

Step 1.  Get the license key

When the package is installed all the endpoints to the service are
added to your Web.config file.  There is also an appSetting added
in that file as well:

			<add key="NCOALicenseKey" value="wsXX-XXXX-XXXX"/>
			
You will need to replace the value attribute with a real trial or live key
from Service Objects.  A trial key can be obtained from:

			http://www.serviceobjects.com/dots-key?wsid=8
			
In your code behind page you will want to pull the key from your
Web.config file with a call like this:
				
			string LicenseKey = ConfigurationManager.AppSettings["NCOALicenseKey"];
			
Step 2.  Make the API call

Gather your inout vaiables and use them as parameters in the line
below to make thew call to the API operation:

			DOTSNCOALive.NCOAService.NCOAAddressResponse response = DOTSNCOALive.DOTSNCOA.RunNCOALive([Addresses], [JobID], [LicenseKey], [IsLive]);

Replace the parameter values in the square brackets above with your
respective input values.

Step 3.  Process the Response

Details about what you get back in the response can be found in the developers guide.  The link for the guide is given above.
			
Find the section on the operation "RunNCOALive".  This is the
operation this call makes and is the recommended operation for this
service. See the developers guide for more information on the types of values that the services return.