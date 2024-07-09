using DataverseWebApiIntegration.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace DataverseWebApiIntegration.Controllers
{
    public class AccountsController : ApiController
    {
        //GET: api/accounts/getmodifiedaccounts? modifiedOn = { modifiedOn }
        [HttpGet]
        [Route("api/accounts/getmodifiedaccounts")]
        public IHttpActionResult GetModifiedAccounts(DateTime modifiedOn)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                var accounts = new List<Account>();

                using (var service = new CrmServiceClient(connectionString))
                {
                    if (service.IsReady)
                    {
                        var orgServiceContext = new OrganizationServiceContext(service);
                        var query = from account in orgServiceContext.CreateQuery("account")
                                    where (DateTime)account["modifiedon"] >= modifiedOn
                                    select new Account
                                    {
                                        Id = account.Id.ToString(),
                                        Name = account.GetAttributeValue<string>("name"),
                                        Telephone1 = account.GetAttributeValue<string>("telephone1"),
                                        Websiteurl = account.GetAttributeValue<string>("websiteurl"),
                                        Address1_line1 = account.GetAttributeValue<string>("address1_line1"),
                                        Address1_line2 = account.GetAttributeValue<string>("address1_line2"),
                                        Address1_city = account.GetAttributeValue<string>("address1_city"),
                                        Address1_postalcode = account.GetAttributeValue<string>("address1_postalcode"),
                                        Address1_stateorprovince = account.GetAttributeValue<string>("address1_stateorprovince")
                                    };
                        accounts = query.ToList();

                        return Ok(accounts);
                    }
                    else
                    {
                        return Content(HttpStatusCode.InternalServerError, "Failed to connect to Dataverse.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


        // GET: api/accounts/getaccountdetails?id={id}
        [HttpGet]
        [Route("api/accounts/getaccountdetails")]
        public IHttpActionResult GetAccountDetails(Guid id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (var service = new CrmServiceClient(connectionString))
                {
                    if (!service.IsReady)
                        return Content(HttpStatusCode.InternalServerError, "Failed to connect to Dataverse.");

                    var orgServiceContext = new OrganizationServiceContext(service);

                    var accountQuery = from account in orgServiceContext.CreateQuery("account")
                                where account.GetAttributeValue<Guid>("accountid") == id
                                select new Account
                                {
                                    Id = account.Id.ToString(),
                                    Name = account.GetAttributeValue<string>("name"),
                                    Telephone1 = account.GetAttributeValue<string>("telephone1"),
                                    Websiteurl = account.GetAttributeValue<string>("websiteurl"),
                                    Address1_line1 = account.GetAttributeValue<string>("address1_line1"),
                                    Address1_line2 = account.GetAttributeValue<string>("address1_line2"),
                                    Address1_city = account.GetAttributeValue<string>("address1_city"),
                                    Address1_postalcode = account.GetAttributeValue<string>("address1_postalcode"),
                                    Address1_stateorprovince = account.GetAttributeValue<string>("address1_stateorprovince"),
                                };
                    var contactsQuery = from contact in orgServiceContext.CreateQuery("contact")
                                where contact.GetAttributeValue<EntityReference>("parentcustomerid").Id == id
                                select new Contact
                                {
                                    Id = contact.Id.ToString(),
                                    FullName = contact.GetAttributeValue<string>("fullname"),
                                    Telephone1 = contact.GetAttributeValue<string>("telephone1"),
                                    Mobilephone = contact.GetAttributeValue<string>("mobilephone"),
                                    Emailaddress1 = contact.GetAttributeValue<string>("emailaddress1")
                                };

                    var accountResult = accountQuery.FirstOrDefault();
                    if(accountResult != null)
                    {
                        var contacts = contactsQuery.ToList();
                        accountResult.Contacts = contacts;
                    }                    

                    return Ok(accountResult);
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occurred: {ex}");
            }
        }
    }
}