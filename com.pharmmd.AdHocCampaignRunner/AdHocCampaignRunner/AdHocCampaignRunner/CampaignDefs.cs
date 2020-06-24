using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHocCampaignRunner
{
    public class CampaignDefs
    {
        private static int year = DateTime.Now.Year;
        //private static string excelbasefiletemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-";
        private static string excelbasefiletemplate = System.Configuration.ConfigurationManager.AppSettings["ExcelOutputPathTemplate"].ToString();

        private static Dictionary<string, CampaignInfo> _campaigns = new Dictionary<string, CampaignInfo>();

        public static Dictionary<string, CampaignInfo> Campaigns
        {
            get
            {
                if (_campaigns.Count == 0)
                {
                    DataTable dt = CheckRunAdHocCampaigns.CheckRunAdHocCampaignsHelper.GetCampaignNamesList();

                    foreach (DataRow dr in dt.Rows)
                    {
                        _campaigns.Add(dr["campaignname"].ToString(), new CampaignInfo(dr["campaignname"].ToString(), dr["dialertable"].ToString(), dr["dialertablehx"].ToString(), excelbasefiletemplate + dr["campaignname"].ToString() + "_(0).xlsx"));
                    }



                    /*
                    _campaigns.Add(CampaignDefs.SCAN_PES.Name, CampaignDefs.SCAN_PES);
                    _campaigns.Add(CampaignDefs.SCAN_PHARMACIST.Name, CampaignDefs.SCAN_PHARMACIST);
                    _campaigns.Add(CampaignDefs.SCAN_PES_SPANISH.Name, CampaignDefs.SCAN_PES_SPANISH);
                    _campaigns.Add(CampaignDefs.SCAN_PHARMACIST_SPANISH.Name, CampaignDefs.SCAN_PHARMACIST_SPANISH);
                    _campaigns.Add(CampaignDefs.SUMMACARE_PES.Name, CampaignDefs.SUMMACARE_PES);
                    _campaigns.Add(CampaignDefs.SUMMACARE_PHARMACIST.Name, CampaignDefs.SUMMACARE_PHARMACIST);
                    _campaigns.Add(CampaignDefs.MERIDIAN_PES.Name, CampaignDefs.MERIDIAN_PES);
                    _campaigns.Add(CampaignDefs.MERIDIAN_PHARMACIST.Name, CampaignDefs.MERIDIAN_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_PDP_PES.Name, CampaignDefs.HEALTHSPRING_PDP_PES);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_PDP_PHARMACIST.Name, CampaignDefs.HEALTHSPRING_PDP_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_SURGE.Name, CampaignDefs.HEALTHSPRING_SURGE);
                    _campaigns.Add(CampaignDefs.HIGHMARKPDP_PES.Name, CampaignDefs.HIGHMARKPDP_PES);
                    _campaigns.Add(CampaignDefs.HIGHMARKPDP_PHARMACIST.Name, CampaignDefs.HIGHMARKPDP_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HIGHMARKMAPD_PES.Name, CampaignDefs.HIGHMARKMAPD_PES);
                    _campaigns.Add(CampaignDefs.HIGHMARKMAPD_PHARMACIST.Name, CampaignDefs.HIGHMARKMAPD_PHARMACIST);
                    _campaigns.Add(CampaignDefs.CHP_PES.Name, CampaignDefs.CHP_PES);
                    _campaigns.Add(CampaignDefs.CHP_PES_NONENG.Name, CampaignDefs.CHP_PES_NONENG);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Cantonese.Name, CampaignDefs.CHP_PHARMACIST_Cantonese);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Korean.Name, CampaignDefs.CHP_PHARMACIST_Korean);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Mandarin.Name, CampaignDefs.CHP_PHARMACIST_Mandarin);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Vietnamese.Name, CampaignDefs.CHP_PHARMACIST_Vietnamese);


                    _campaigns.Add(CampaignDefs.HIGHMARKMAPD_WV.Name, CampaignDefs.HIGHMARKMAPD_WV);
                    _campaigns.Add(CampaignDefs.HIGHMARKPDP_WV.Name, CampaignDefs.HIGHMARKPDP_WV);
                    */
                }
                return _campaigns;
            }
        }

        /*
        public CampaignInfo MasterCampaign = new CampaignInfo(string _name, string _dialertable, string _dialertablehx, string _excelpathtemplate)
        {
            
        };
        */


        /*
       public static CampaignInfo SCAN_PES = new CampaignInfo
       (
           "SCAN_PES", "MstrCamp_MTMPes_ScanAdhoc", "MstrCamp_MTMPes_ScanAdhoc_Hx", @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\{0}\SCAN_PES_{0}.xlsx"
       );

       public static CampaignInfo SCAN_PHARMACIST = new CampaignInfo
       (
           "SCAN_PHARMACIST",
           "MstrCamp_MTMPharmacist_ScanAdhoc",
           "MstrCamp_MTMPharmacist_ScanAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\SCAN_PHARMACIST_(0).xlsx"
       );
       public static CampaignInfo SCAN_PES_SPANISH = new CampaignInfo
       (
           "SCAN_PES_SPANISH",
           "MstrCamp_MTMPes_ScanAdhoc_Spanish",
           "MstrCamp_MTMPes_ScanAdhoc_Spanish_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\SCAN_PES_SPANISH_(0).xlsx"
       );
       public static CampaignInfo SCAN_PHARMACIST_SPANISH = new CampaignInfo
       (
           "SCAN_PHARMACIST_SPANISH",
           "MstrCamp_MTMPharmacist_ScanAdhoc_Spanish",
           "MstrCamp_MTMPharmacist_ScanAdhoc_Spanish_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\SCAN_PHARMACIST_SPANISH_(0).xlsx"
       );
       public static CampaignInfo SUMMACARE_PES = new CampaignInfo
       (
           "SUMMACARE_PES",
           "MstrCamp_MTMPes_SummaCareAdhoc",
           "MstrCamp_MTMPes_SummaCareAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\SUMMACARE_PES_(0).xlsx"
       );
       public static CampaignInfo SUMMACARE_PHARMACIST = new CampaignInfo
       (
           "SUMMACARE_PHARMACIST",
           "MstrCamp_MTMPharmacist_SummaCareAdhoc",
           "MstrCamp_MTMPharmacist_SummaCareAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\SUMMACARE_PHARMACIST_(0).xlsx"
       );
       public static CampaignInfo MERIDIAN_PES = new CampaignInfo
       (
           "MERIDIAN_PES",
           "MstrCamp_MTMPes_MeridianAdhoc",
           "MstrCamp_MTMPes_MeridianAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\MERIDIAN_PES_(0).xlsx"
       );
       public static CampaignInfo MERIDIAN_PHARMACIST = new CampaignInfo
       (
           "MERIDIAN_PHARMACIST",
           "MstrCamp_MTMPharmacist_MeridianAdhoc",
           "MstrCamp_MTMPharmacist_MeridianAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\MERIDIAN_PHARMACIST_(0).xlsx"
       );

       public static CampaignInfo HEALTHSPRING_SURGE = new CampaignInfo
       (
           "HEALTHSPRING_SURGE",
           "MstrCamp_MTMPes_HealthSpringSurgeAdhoc",
           "MstrCamp_MTMPes_HealthSpringSurgeAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HEALTHSPRING_SURGE_(0).xlsx"
       );



       public static CampaignInfo HEALTHSPRING_PDP_PES = new CampaignInfo
       (
           "HEALTHSPRING_PDP_PES",
           "MstrCamp_MTMPes_HealthSpringPDPAdhoc",
           "MstrCamp_MTMPes_HealthSpringPDPAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HEALTHSPRING_PDP_PES_(0).xlsx"
       );

       public static CampaignInfo HEALTHSPRING_PDP_PHARMACIST = new CampaignInfo
       (
           "HEALTHSPRING_PDP_PHARMACIST",
           "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc",
           "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HEALTHSPRING_PDP_PHARMACIST_(0).xlsx"
       );

       public static CampaignInfo HIGHMARKPDP_WV = new CampaignInfo
       (
           "HIGHMARKPDP_WV",
           "MstrCamp_MTMPes_Highmark",
           "MstrCamp_MTMPes_Highmark_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKPDP_WV(0).xlsx"
       );
       public static CampaignInfo HIGHMARKMAPD_WV = new CampaignInfo
       (
           "HIGHMARKMAPD_WV",
           "MstrCamp_MTMPes_Highmark",
           "MstrCamp_MTMPes_Highmark_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKMAPD_WV(0).xlsx"
       );

       public static CampaignInfo HIGHMARKPDP_PES = new CampaignInfo
       (
           "HIGHMARKPDP_PES",
           "MstrCamp_MTMPes_HighmarkPDPAdhoc",
           "MstrCamp_MTMPes_HighmarkPDPAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKPDP_PES_(0).xlsx"
       );
       public static CampaignInfo HIGHMARKPDP_PHARMACIST = new CampaignInfo
       (
           "HIGHMARKPDP_PHARMACIST",
           "MstrCamp_MTMPharmacist_HighmarkPDPAdhoc",
           "MstrCamp_MTMPharmacist_HighmarkPDPAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKPDP_PHARMACIST_(0).xlsx"
       );
       public static CampaignInfo HIGHMARKMAPD_PES = new CampaignInfo
       (
           "HIGHMARKMAPD_PES",
           "MstrCamp_MTMPes_HighmarkMAPDAdhoc",
           "MstrCamp_MTMPes_HighmarkMAPDAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKMAPD_PES_(0).xlsx"
       );
       public static CampaignInfo HIGHMARKMAPD_PHARMACIST = new CampaignInfo
       (
           "HIGHMARKMAPD_PHARMACIST",
           "MstrCamp_MTMPharmacist_HighmarkMAPDAdhoc",
           "MstrCamp_MTMPharmacist_HighmarkMAPDAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\HIGHMARKMAPD_PHARMACIST_(0).xlsx"
       );
       public static CampaignInfo CHP_PES = new CampaignInfo
       (
           "CHP_PES",
           "MstrCamp_MTMPes_CHPAdhoc",
           "MstrCamp_MTMPes_CHPAdhoc_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PES_(0).xlsx"
       );
       public static CampaignInfo CHP_PES_NONENG = new CampaignInfo
       (
           "CHP_PES_NONENG",
           "MstrCamp_MTMPes_CHPAdhoc_NONENG",
           "MstrCamp_MTMPes_CHPAdhoc_NONENG_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PES_NONENG_(0).xlsx"
       );
       public static CampaignInfo CHP_PHARMACIST_Cantonese = new CampaignInfo
       (
           "CHP_PHARMACIST_Cantonese",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Cantonese",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Cantonese_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PHARMACIST_Cantonese_(0).xlsx"
       );
       public static CampaignInfo CHP_PHARMACIST_Korean = new CampaignInfo
       (
           "CHP_PHARMACIST_Korean",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Korean",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Korean_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PHARMACIST_Korean_(0).xlsx"
       );
       public static CampaignInfo CHP_PHARMACIST_Mandarin = new CampaignInfo
       (
           "CHP_PHARMACIST_Mandarin",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Mandarin",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Mandarin_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PHARMACIST_Mandarin_(0).xlsx"
       );
       public static CampaignInfo CHP_PHARMACIST_Vietnamese = new CampaignInfo
       (
           "CHP_PHARMACIST_Vietnamese",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Vietnamese",
           "MstrCamp_MTMPharmacist_CHPAdhoc_Vietnamese_Hx",
           @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists-" + year.ToString() + @"\(0)\CHP_PHARMACIST_Vietnamese_(0).xlsx"
       );

       */
    }
}
