using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DropdownController : BaseController
    {
        public DropdownController(): base("GeoData") { }
        
        public async Task<ActionResult> GetCountries()
        {
            try
            {
                if(Utility.Countries is null)
                {
                    var result = await GetAsync<DropdownModel>("Countries");
                    if(result is ActionResult actionResult)
                    {
                        return actionResult;
                    }
                    else
                    {
                        var countries = result as List<DropdownModel>;
                        Utility.Countries = countries;
                        return Json(countries);
                    }
                }
                else
                {
                    return Json(Utility.Countries);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetStatesOfCountry(uint id)
        {
            try
            {
                var result = await GetAsync<List<DropdownModel>>((int)id, "StatesOfCountry");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetCitiesOfState(uint id)
        {
            try
            {
                var result = await GetAsync<List<DropdownModel>>((int)id, "CitiesOfState");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetStatesAndCitiesFromCity(uint id)
        {
            var stateId = await GetAsync<int>((int)id, "State");
            var countryId = await GetAsync<int>((int)stateId, "Country");

            var cities = await GetAsync<List<DropdownModel>>((int)stateId, "CitiesOfState");
            var states = await GetAsync<List<DropdownModel>>((int)countryId, "StatesOfCountry");

            return Json(new { cities, states, countryId, stateId });
        }

        public async Task<ActionResult> GetCorporateUser()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("CorporateUser", "Dropdown");
                if(result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var userCorporate = result as List<DropdownModel>;
                    return Json(userCorporate);
                }
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetFleetOwner()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("FleetOwner", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOnwer = result as List<DropdownModel>;
                    return Json(fleetOnwer);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        // Seperate Code Written By Kashif Shahzad
        public async Task<ActionResult> GetSupervisorForSiteAssigning()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Supervisor", "DropdownForSiteAssigning");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var Supervisors = result as List<DropdownModel>;
                    return Json(Supervisors);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetSupervisor()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Supervisor", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var Supervisors = result as List<DropdownModel>;
                    return Json(Supervisors);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetProject()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Project", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var project = result as List<DropdownModel>;
                    return Json(project);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetProjects(uint id)
        {
            try
            {
                var result = await GetAsync<List<DropdownModel>>((int)id,"Project", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var project = result as List<DropdownModel>;
                    return Json(project);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetUser()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var users = result as List<DropdownModel>;
                    return Json(users);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetDataEntryOperator()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/DataEntryOperator");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var approvalManagers = result as List<DropdownModel>;
                    return Json(approvalManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetApprovalManager()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/ApprovalManager");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var approvalManagers = result as List<DropdownModel>;
                    return Json(approvalManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetFinanceManager()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/FinanceManager");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var financeManagers = result as List<DropdownModel>;
                    return Json(financeManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetInstaller()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Installer", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var installers = result as List<DropdownModel>;
                    return Json(installers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetCrewForAssigningSite()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Crew", "DropdownForAssigningSite");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crews = result as List<DropdownModel>;
                    return Json(crews);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetCrew()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Crew", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crews = result as List<DropdownModel>;
                    return Json(crews);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetVehicleTypes()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Vehicle", "Dropdown");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vehicles = result as List<DropdownModel>;
                    return Json(vehicles);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetVehicleSpecification(uint id)
        {
            try
            {
                var result = await GetAsync<List<DropdownModel>>((int)id, "Vehicle", "DropDownVehicleTypeSpecification");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vehicleSpecification = result as List<DropdownModel>;
                    return Json(vehicleSpecification);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }



        // ForAddLogin
        public async Task<ActionResult> GetDataEntryOperatorForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/DataEntryOperatorForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var approvalManagers = result as List<DropdownModel>;
                    return Json(approvalManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetApprovalManagerForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/ApprovalManagerForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var approvalManagers = result as List<DropdownModel>;
                    return Json(approvalManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetFinanceManagerForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("User", "Dropdown/FinanceManagerForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var financeManagers = result as List<DropdownModel>;
                    return Json(financeManagers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetFleetOwnerForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("FleetOwner", "DropdownForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOnwer = result as List<DropdownModel>;
                    return Json(fleetOnwer);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetCorporateUserForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("CorporateUser", "DropdownForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var userCorporate = result as List<DropdownModel>;
                    return Json(userCorporate);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public async Task<ActionResult> GetInstallerForAddLogin()
        {
            try
            {
                var result = await GetAsync<DropdownModel>("Installer", "DropdownForAddLogin");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var installers = result as List<DropdownModel>;
                    return Json(installers);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}